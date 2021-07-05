using Ninject;
using ServiceStack;
using ServiceStack.Api.OpenApi;
using ServiceStack.Auth;
using ServiceStack.Data;
using ServiceStack.Host;
using ServiceStack.Messaging;
using ServiceStack.Messaging.Redis;
using ServiceStack.MiniProfiler;
using ServiceStack.MiniProfiler.Data;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.SqlServer;
using ServiceStack.Redis;
using ServiceStack.Request.Correlation;
using ServiceStack.Text.EnumMemberSerializer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using WebEas.Core.Base;
using WebEas.Esam.ServiceModel.Office;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Dto;
using System.Linq;

namespace WebEas.Esam.ServiceInterface.Office
{
    /// <summary>
    /// Nadstavba App Host Base
    /// </summary>
    public abstract class EsamAppHostBase : WebEasAppHostBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EsamAppHostBase" /> class.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="assembliesWithServices">The assemblies with services.</param>
        protected EsamAppHostBase(string serviceName, params System.Reflection.Assembly[] assembliesWithServices) : base(serviceName, assembliesWithServices)
        {
#if DEBUG
            WebEas.Log.WebEasNLogConfig.SetConfig(ConfigurationManager.ConnectionStrings["EsamLogConnString"].ConnectionString, console: true);
#else
            WebEas.Log.WebEasNLogConfig.SetConfig(ConfigurationManager.ConnectionStrings["EsamLogConnString"].ConnectionString, console:false);
#endif
        }

        /// <summary>
        /// Configures the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        public override void Configure(Funq.Container container)
        {
#if DEBUG
            System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, cert, chain, errors) =>
            {
                return cert.Subject.Contains("isodatalan.intra.dcom.sk");
            });
#endif
            //this.Plugins.RemoveAll(x => x is MarkdownFormat);
            base.Configure(container);

            //Plugins.Add(new CancellableRequestsFeature());
            Plugins.Add(new MiniProfilerFeature());
            //Plugins.Add(new RequestCorrelationFeature());

#if DEBUG || DEVELOP || INT || TEST || ITP
            Plugins.Add(new OpenApiFeature());
#endif
            new EnumSerializerConfigurator().WithAssemblies(new List<Assembly> { typeof(HierarchyNode).Assembly }).Configure();
            Plugins.Add(new AuthFeature(() =>
                new EsamSession(),
                    new IAuthProvider[] {
                    new EsamCredentialsAuthProvider(),
                    }, htmlRedirect: "/#login"));
                    
#if DEBUG || DEVELOP
            Config.UseSecureCookies = false;
#else
            Config.UseSameSiteCookies = true;
#endif

            var mse = new ServerEventsFeature()
            {
                LimitToAuthenticatedUsers = true
            };
            //mse.HeartbeatInterval = TimeSpan.FromSeconds(30);
            //mse.IdleTimeout = TimeSpan.FromSeconds(mse.HeartbeatInterval.TotalSeconds * 3);
            //LC: Pri 30000 to raz pada raz nie. 
            Plugins.Add(mse);
        }

        /// <summary>
        /// Adds the ninject binding.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        /// <returns></returns>
        public override IKernel AddNinjectBinding(IKernel kernel)
        {

            //var dialect = new SqlServerOrmLiteDialectProvider();
            // var dialect = SqlServerDialect.Provider;
            //UTC nakoniec nezapinam, mame vlastny serializer vid EgovAppHost; 
            //dialect.EnsureUtc(true);            
            // Kvôli lepsej konfigurovatelnosti pouzijem MSSql Provider - ak bude cas, potvrdte/vyvratte mi tento nazor
            // SqlServerDialect.Provider
            var provider = new SqlServerOrmLiteDialectProvider();

            var olcf = new OrmLiteConnectionFactory(ConfigurationManager.ConnectionStrings["EsamConnString"].ConnectionString, provider)
            {
                ConnectionFilter = x => new ProfiledDbConnection(x, Profiler.Current)
            };

            kernel.Bind<IDbConnectionFactory>().ToMethod(c => olcf);

            return kernel;
        }

        protected void ConfigureMessageServiceForLongOperations<T>(Funq.Container container, bool startMessageService = true) where T : LongOperationStartDtoBase
        {
            container.Register<IMessageService>(c => new RedisMqServer(c.Resolve<IRedisClientsManager>()));
            container.Resolve<IMessageService>().RegisterHandler<T>(m =>
            {
                var req = new BasicRequest
                {
                    Verb = HttpMethods.Post
                };

                req.Headers["X-ss-id"] = m.GetBody().SessionId;
                var response = ExecuteMessage(m, req);
                return response;
            });

            if (startMessageService)
            {
                container.Resolve<IMessageService>().Start();
            }
        }

        protected void ConfigureMessageServiceForReports<T>(Funq.Container container) where T : Reports.Dto.ReportDataRequestDto
        {
            Routes.Add<T>("/GetReportData");
            container.Resolve<IMessageService>().RegisterHandler<T>(m =>
            {
                var req = new BasicRequest
                {
                    Verb = HttpMethods.Post
                };

                req.Headers["X-ss-id"] = m.GetBody().SessionId;
                var response = ExecuteMessage(m, req);
                return response;
            });
        }

        public static void ProcessLongOperationStatus(LongOperationStatus longOperationStatus, IRedisClient redisClient, IServerEvents serverEvents)
        {
            var hashId = string.Concat("LongOperationStatus:", longOperationStatus.ProcessKey.Split('!')[0], ":", longOperationStatus.TenantId);
            var setId = string.Concat("LongOperationStatus:Keys:", longOperationStatus.ProcessKey.Split('!')[0], ":", longOperationStatus.TenantId);
            var processKey = string.Concat(longOperationStatus.UserId, "!", longOperationStatus.ProcessKey);

            redisClient.SetEntryInHash(hashId, processKey, longOperationStatus.ToJson());
            redisClient.AddItemToSortedSet(setId, processKey, longOperationStatus.Changed);
            redisClient.AddItemToSortedSet(string.Concat(setId, ":", longOperationStatus.UserId), processKey, longOperationStatus.Changed);

            var keys = redisClient.GetAllItemsFromSortedSet(setId);
            serverEvents.NotifyChannel(longOperationStatus.TenantId + ":" + longOperationStatus.ProcessKey.Split('!')[0], new LongOperationStatusCount { Tenant = keys.Count, User = keys.Count(x => x.StartsWith(longOperationStatus.UserId)) });
        }
    }
}