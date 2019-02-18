using Ninject;
using Ninject.Web.Common;
using ServiceStack;
using ServiceStack.Logging;
using ServiceStack.OrmLite;
using WebEas.Auth;
using WebEas.Esam.ServiceInterface.Office;
using WebEas.Esam.ServiceInterface.Office.Cfe;
using WebEas.Esam.ServiceInterface.Office.Rzp;
using WebEas.Esam.ServiceInterface.Pfe;
using WebEas.ServiceInterface;

namespace WebEas.Esam.Pfe
{
    /// <summary>
    /// 
    /// </summary>
    public class AppHost : EsamAppHostBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppHost" /> class.
        /// </summary>
        public AppHost() : base("Egovernment", typeof(PfeService).Assembly)
        {
        }

        /// <summary>
        /// Configure the given container with the
        /// registrations provided by the funqlet.
        /// </summary>
        /// <param name="container">Container to register.</param>
        public override void Configure(Funq.Container container)
        {
            OrmLiteConfig.CommandTimeout = 60;
            WebEas.Context.Current.CurrentEndpoint = WebEas.Context.EndpointType.Office;
            WebEas.Log.WebEasNLogLogger.Application = "PFE";
            base.Configure(container);

            // new EnumSerializerConfigurator().WithAssemblies(new List<Assembly> { typeof(HierarchyNode).Assembly }).WithNullableEnumSerializers().Configure();

            this.SetConfig(new HostConfig
            {
                WsdlServiceNamespace = "http://schemas.dcom.sk/private/Egov/pfe/1.0",
                SoapServiceName = "EsamPfe",
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
            LogManager.GetLogger("Kernel").Info("Loading kernel");
            kernel.Bind<IRoleList>().To<ServiceModel.Office.Reg.ServiceModel>();
            kernel.Bind<IRoleList>().To<ServiceModel.Office.Rzp.ServiceModel>();
            kernel.Bind<IRoleList>().To<ServiceModel.Office.Cfe.ServiceModel>();

            kernel.Bind<IRoleList>().To<ServiceModel.Office.RolesDefinition.OfficeRoleList>();

            kernel.Bind<IWebEasServiceInterface>().To<WebEas.Esam.ServiceInterface.Office.Rzp.ServiceInterface>();
            kernel.Bind<IWebEasServiceInterface>().To<WebEas.Esam.ServiceInterface.Office.Cfe.ServiceInterface>();
            kernel.Bind<IWebEasServiceInterface>().To<ServiceInterface.Office.Reg.ServiceInterface>();

            kernel.Bind<IPfeRepository>().To<PfeRepository>().InRequestScope().WithPropertyValue("StsThumbPrint", this.GetThumbprint("StsThumbprint"));
            kernel.Bind<IRzpRepository>().To<RzpRepository>().InRequestScope().WithPropertyValue("StsThumbPrint", this.GetThumbprint("StsThumbprint"));
            kernel.Bind<ICfeRepository>().To<CfeRepository>().InRequestScope().WithPropertyValue("StsThumbPrint", this.GetThumbprint("StsThumbprint"));
            base.AddNinjectBinding(kernel);
            LogManager.GetLogger("Kernel").Info("Loading kernel done");
            return kernel;
        }
    }
}