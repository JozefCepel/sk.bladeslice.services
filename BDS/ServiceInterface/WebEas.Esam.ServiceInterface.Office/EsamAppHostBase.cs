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
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using WebEas.Core.Base;
using WebEas.Esam.ServiceModel.Office;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Dto;

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

#if DEBUG || DEVELOP || INT || TEST
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
#endif
            
            Config.UseSameSiteCookies = true;

            //TODO: do buducnosti
            /*Plugins.Add(new ServerEventsFeature());

            container.Register<IServerEvents>(c =>
                new RedisServerEvents(c.Resolve<IRedisClientsManager>()));

            container.Resolve<IServerEvents>().Start();*/
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

        public static void ProcessLongOperationStatus(LongOperationStatus longOperationStatus, IRedisClient redisClient)
        {
            //TODO: HASH NIEJE SORTOVANY, odstranit podla datumu
            var hashId = string.Concat("LongOperationStatus:", longOperationStatus.ProcessKey.Split('!')[0], ":", longOperationStatus.TenantId);
            redisClient.SetEntryInHashIfNotExists(hashId, string.Concat(longOperationStatus.UserId, "!", longOperationStatus.ProcessKey), longOperationStatus.ToJson());

            if (redisClient.GetHashCount(hashId) > 99)
            {
                var allKeys = redisClient.GetHashKeys(hashId);
                for (int i = 49; i < allKeys.Count - 1; i++)
                {
                    redisClient.RemoveEntryFromHash(hashId, allKeys[i]);
                }
            }
        }
    }
}