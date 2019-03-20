using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.Configuration;
using Ninject;
using ServiceStack;
using ServiceStack.Api.OpenApi;
using ServiceStack.Data;
using ServiceStack.Formats;
using ServiceStack.MiniProfiler;
using ServiceStack.MiniProfiler.Data;
using ServiceStack.OrmLite;
using ServiceStack.Text.EnumMemberSerializer;
using ServiceStack.Web;
using WebEas.Core;
using WebEas.Core.Base;
using WebEas.Esam.ServiceModel.Office;
using WebEas.ServiceModel;

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
            WebEas.Log.WebEasNLogConfig.SetConfig(ConfigurationManager.ConnectionStrings["EsamLogConnString"].ConnectionString, console:true);
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
            this.Plugins.RemoveAll(x => x is MarkdownFormat);
            base.Configure(container);

#if DEBUG || DEVELOP || INT || TEST
            Plugins.Add(new OpenApiFeature());
#endif

            new EnumSerializerConfigurator().WithAssemblies(new List<Assembly> { typeof(HierarchyNode).Assembly }).Configure();
        }

        /// <summary>
        /// Called when [uncaught exception].
        /// </summary>
        /// <param name="httpReq">The HTTP req.</param>
        /// <param name="httpRes">The HTTP res.</param>
        /// <param name="operationName">Name of the operation.</param>
        /// <param name="ex">The ex.</param>
        public override void OnUncaughtException(IRequest httpReq, IResponse httpRes, string operationName, Exception ex)
        {
            HttpError response = WebEasErrorHandling.CreateErrorResponse(httpReq, null, ex);

            #if INT || DEVELOP || DEBUG

            if (response.Response is WebEas.Exceptions.WebEasResponseStatus)
            {
#if DEBUG || DEVELOP
                ((WebEas.Exceptions.WebEasResponseStatus)response.Response).DetailMessage += $"{Environment.NewLine}http://esam-dev.datalan.sk/esam/api/pfe/lll/{ex.GetIdentifier()}";
#endif

            }
            #endif
            
            httpRes.WriteToResponse(httpReq, response);
        }

        /// <summary>
        /// Creates the service runner.
        /// </summary>
        /// <typeparam name="TRequest">The type of the T request.</typeparam>
        /// <param name="actionContext">The action context.</param>
        /// <returns></returns>
        public override IServiceRunner<TRequest> CreateServiceRunner<TRequest>(ServiceStack.Host.ActionContext actionContext)
        {
            return new EsamServiceRunner<TRequest>(this, actionContext);
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
            var provider = new WebEasOrmLiteDialectProvider
            {
                SelectIdentitySql = "SELECT SCOPE_IDENTITY() AS 'Identity'"
            };

            var olcf = new OrmLiteConnectionFactory(ConfigurationManager.ConnectionStrings["EsamConnString"].ConnectionString, provider)
            {
                ConnectionFilter = x => new ProfiledDbConnection(x, Profiler.Current)
            };

            kernel.Bind<IDbConnectionFactory>().ToMethod(c => olcf);

            kernel.Bind<IWebEasSessionProvider>().To<EsamSessionProvider>().When(x => System.Web.HttpContext.Current != null).InSingletonScope();
            kernel.Bind<IWebEasSessionProvider>().To<EsamSessionProvider>().When(x => System.Web.HttpContext.Current == null).InThreadScope();

            return kernel;
        }

        protected string GetThumbprint(string name)
        {
            if (String.IsNullOrEmpty(WebConfigurationManager.AppSettings[name]))
            {
                throw new WebEasException(string.Format("{0} is not defined in appsettings of the web.config", name));
            }
            return WebConfigurationManager.AppSettings[name];
        }
    }
}