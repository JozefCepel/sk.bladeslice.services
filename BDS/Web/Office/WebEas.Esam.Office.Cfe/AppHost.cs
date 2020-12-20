using Ninject;
using Ninject.Web.Common;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Web;
using System.Collections.Generic;
using WebEas.Auth;
using WebEas.Esam.ServiceInterface.Office;
using WebEas.Esam.ServiceInterface.Office.Cfe;
using WebEas.ServiceInterface;

namespace WebEas.Esam.Office.Cfe
{
    public class AppHost : EsamAppHostBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppHost" /> class.
        /// </summary>
        public AppHost() : base("cfe", typeof(CfeService).Assembly)
        {
        }

        /// <summary>
        /// Configure the given container with the
        /// registrations provided by the funqlet.
        /// </summary>
        /// <param name="container">Container to register.</param>
        public override void Configure(Funq.Container container)
        {
            ServiceStack.OrmLite.OrmLiteConfig.CommandTimeout = 60;
            WebEas.Context.Current.CurrentEndpoint = Context.EndpointType.Office;
            WebEas.Log.WebEasNLogLogger.Application = "CFE";
            base.Configure(container);

            this.SetConfig(new HostConfig
            {
                WsdlServiceNamespace = "http://schemas.webeas.sk/office/esam/office/1.0",
                SoapServiceName = "EsamOfficeObs",
#if DEBUG || DEVELOP || INT
                DebugMode = true,
                EnableFeatures = Feature.All.Remove(this.disableFeaturesDebug),
#else
                DebugMode = false,
                EnableFeatures = Feature.All.Remove(this.disableFeatures),
#endif
                DefaultContentType = MimeTypes.Json,
                AllowJsonpRequests = true
            });
        }

        /// <summary>
        /// Adds the ninject binding.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        /// <returns></returns>
        public override IKernel AddNinjectBinding(IKernel kernel)
        {
            base.AddNinjectBinding(kernel);

            kernel.Bind<ICfeRepository, ServiceModel.Office.IRepositoryBase>().To<CfeRepository>().InRequestScope();

            return kernel;
        }
    }
}