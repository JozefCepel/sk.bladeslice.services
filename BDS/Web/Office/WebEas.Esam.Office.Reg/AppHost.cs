using Ninject;
using Ninject.Web.Common;
using ServiceStack;
using WebEas.Auth;
using WebEas.Esam.ServiceInterface.Office;
using WebEas.ServiceInterface;

namespace WebEas.Esam.Office.Reg
{
    /// <summary>
    /// 
    /// </summary>
    public class AppHost : EsamAppHostBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppHost" /> class.
        /// </summary>
        public AppHost() : base("Egovernment", typeof(OfficeService).Assembly)
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
            WebEas.Log.WebEasNLogLogger.Application = "REG";

            base.Configure(container);

            this.SetConfig(new HostConfig
            {
                WsdlServiceNamespace = "http://schemas.webeas.sk/office/esam/office/1.0",
                SoapServiceName = "EsamOfficeReg",
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

            //JsConfig<List<FilesDto>>.DeSerializeFn = data => JsonSerializer.DeserializeFromString<List<FilesDto>>(data);

            //JsConfig.ThrowOnDeserializationError = true;
            //JsConfig.OnDeserializationError = (object instance, System.Type propertyType, string propertyName, string propertyValueStr, System.Exception ex) =>
            //{
            //    Console.WriteLine(ex.Message);
            //};
        }

        /// <summary>
        /// Adds the ninject binding.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        /// <returns></returns>
        public override IKernel AddNinjectBinding(IKernel kernel)
        {
            base.AddNinjectBinding(kernel);

            kernel.Bind<IRoleList>().To<ServiceModel.Office.Reg.ServiceModel>();
            kernel.Bind<IRoleList>().To<ServiceModel.Office.Rzp.ServiceModel>();
            kernel.Bind<IRoleList>().To<ServiceModel.Office.Cfe.ServiceModel>();

            kernel.Bind<IRoleList>().To<ServiceModel.Office.RolesDefinition.OfficeRoleList>();
            kernel.Bind<IWebEasServiceInterface>().To<ServiceInterface.Office.Reg.ServiceInterface>();
            kernel.Bind<IOfficeRepository>().To<OfficeRepository>().InRequestScope().WithPropertyValue("StsThumbPrint", GetThumbprint("StsThumbprint"));            

            return kernel;
        }
    }
}