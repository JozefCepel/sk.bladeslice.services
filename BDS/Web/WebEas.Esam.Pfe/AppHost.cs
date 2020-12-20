using Ninject;
using Ninject.Web.Common;
using ServiceStack;
using ServiceStack.Logging;
using ServiceStack.OrmLite;
using WebEas.Auth;
using WebEas.Esam.ServiceInterface.Office;
using WebEas.Esam.ServiceInterface.Office.Cfe;
using WebEas.Esam.ServiceInterface.Office.Bds;
using WebEas.Esam.ServiceInterface.Pfe;
using WebEas.Esam.ServiceModel.Office;
using WebEas.ServiceInterface;
using WebEas.ServiceModel;

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
        public AppHost() : base("pfe", typeof(PfeService).Assembly)
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

            kernel.Bind<ICfeRepository, IWebEasRepositoryBase>().To<CfeRepository>().InRequestScope();
            kernel.Bind<IBdsRepository, IWebEasRepositoryBase>().To<BdsRepository>().InRequestScope();
            //kernel.Bind<ICrmRepository, IWebEasRepositoryBase>().To<CrmRepository>().InRequestScope();
            //kernel.Bind<IDapRepository, IWebEasRepositoryBase>().To<DapRepository>().InRequestScope();
            //kernel.Bind<IDmsRepository, IWebEasRepositoryBase>().To<DmsRepository>().InRequestScope();
            //kernel.Bind<IFinRepository, IWebEasRepositoryBase>().To<FinRepository>().InRequestScope();
            //kernel.Bind<IOsaRepository, IWebEasRepositoryBase>().To<OsaRepository>().InRequestScope();
            //kernel.Bind<IRegRepository, IWebEasRepositoryBase>().To<RegRepository>().InRequestScope();
            //kernel.Bind<IRzpRepository, IWebEasRepositoryBase>().To<RzpRepository>().InRequestScope();
            //kernel.Bind<IUctRepository, IWebEasRepositoryBase>().To<UctRepository>().InRequestScope();

            kernel.Bind<IPfeRepository, IRepositoryBase> ().To<PfeRepository>().InRequestScope();

            base.AddNinjectBinding(kernel);
            LogManager.GetLogger("Kernel").Info("Loading kernel done");
            return kernel;
        }
    }
}