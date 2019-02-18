using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using Ninject;
using ServiceStack;
using ServiceStack.Logging;
using ServiceStack.Text;
using ServiceStack.Web;
using WebEas.Core.Ninject;

namespace WebEas.Core.Base
{
    /// <summary>
    /// Base appHost Initialization
    /// </summary>
    public abstract class WebEasAppHostBase : AppHostBase
    {
#if DEBUG
        protected Feature disableFeatures = Feature.Jsv | Feature.Soap;

#else
        protected Feature disableFeatures = Feature.Jsv | Feature.Soap | Feature.Csv | Feature.Markdown | Feature.Metadata | Feature.Razor | Feature.RequestInfo | Feature.Soap11 | Feature.Soap12;
#endif
        
        protected Feature disableFeaturesDebug = Feature.Jsv | Feature.Soap;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasAppHostBase" /> class.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="assembliesWithServices">The assemblies with services.</param>
        protected WebEasAppHostBase(string serviceName, params System.Reflection.Assembly[] assembliesWithServices)
            : base(serviceName, assembliesWithServices)
        {
            LogManager.LogFactory = new WebEas.Log.WebEasNLogFactory();
            CultureInfo culture = CultureInfo.CreateSpecificCulture("sk-SK");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Adds the ninject binding.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        /// <returns></returns>
        public abstract IKernel AddNinjectBinding(IKernel kernel);

        /// <summary>
        /// Configures the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        public override void Configure(Funq.Container container)
        {
            this.CustomErrorHttpHandlers[System.Net.HttpStatusCode.NotFound] = new WebEas.Core.Handlers.WebEasNotFoundHttpHandler();

            container.Adapter = new NinjectContainerAdapter(this.AddNinjectBinding(NinjectContainerAdapter.CreateKernel()));
            //new EnumSerializerConfigurator().WithAssemblies(new List<Assembly> { typeof(HierarchyNode).Assembly }).WithNullableEnumSerializers().Configure();                       
            //  this.Plugins.Add(new ValidationFeature());            
            this.ConfigureSerialization();
        }

        public override void OnUncaughtException(IRequest httpReq, IResponse httpRes, string operationName, Exception ex)
        {
            httpRes.WriteToResponse(httpReq, WebEasErrorHandling.CreateErrorResponse(httpReq, null, ex));
        }

        /// <summary>
        /// Creates the service runner.
        /// </summary>
        /// <typeparam name="TRequest">The type of the T request.</typeparam>
        /// <param name="actionContext">The action context.</param>
        /// <returns></returns>
        public override IServiceRunner<TRequest> CreateServiceRunner<TRequest>(ServiceStack.Host.ActionContext actionContext)
        {
            return new WebEas.Core.Base.WebEasServiceRunner<TRequest>(this, actionContext);
        }

        /// <summary>
        /// Configures the serialization.
        /// </summary>
        private void ConfigureSerialization()
        {
            //MSSQL DateTime nema zonu, kedze viem , ze sme posunuti, musim  si zonu pridat a potom dam do ISO8601 - "o"
            JsConfig<DateTime>.SerializeFn = time => new DateTime(time.Ticks, DateTimeKind.Local).ToString("o");
            JsConfig<DateTime?>.SerializeFn = time => time.HasValue ? new DateTime(time.Value.Ticks, DateTimeKind.Local).ToString("o") : null;
            JsConfig<TimeSpan>.SerializeFn = time => new DateTime(time.Ticks, DateTimeKind.Local).ToString("o");
            JsConfig<TimeSpan?>.SerializeFn = time => time.HasValue ? new DateTime(time.Value.Ticks, DateTimeKind.Local).ToString("o") : null;
            JsConfig<TimeSpan>.DeSerializeFn = time =>
            {
                DateTime result;
                if (DateTime.TryParse(time, out result))
                {
                    return result.TimeOfDay;
                }
                return new TimeSpan();
            };
            JsConfig<TimeSpan?>.DeSerializeFn = time =>
            {
                DateTime result;
                if (DateTime.TryParse(time, out result))
                {
                    return result.TimeOfDay;
                }
                return null;
            };
            JsConfig<Guid>.SerializeFn = guid => guid.ToString();
            JsConfig<Guid?>.SerializeFn = guid => guid.HasValue ? guid.ToString() : null;
            JsConfig<string>.DeSerializeFn = rt => { return (string.IsNullOrEmpty(rt) || rt == "null" ? null : rt); };
            JsConfig.IncludeNullValues = true;
            JsConfig.ThrowOnDeserializationError = true;
            JsConfig.AllowRuntimeTypeWithAttributesNamed.Add("SerializableAttribute");
            //JsConfig.ThrowOnDeserializationError = true;            
            //JsConfig.DateHandler = DateHandler.ISO8601;  //--  Dava z MSSQL pre DateTime  zonu UTC s riadkom nizsie
            //JsConfig.AssumeUtc = true;  -- bez tohto zona chyba t.j 'Z' na konci, posun -000
            //JsConfig.DateHandler = DateHandler.DCJSCompatible;  // Tu je casova zona spravna, ale boli problemy na FE
            //JsConfig.DateHandler = DateHandler.TimestampOffset; // Original nastavenie, pre MSSQL Datetime je vsak 000                    
            //JsConfig.AppendUtcOffset = true;  // tajomny atribut, nic nerobi a zda sa nic nepokazi - pozriet /ServiceStack.Text/Common/DateTimeSerializer.cs
        }
    }
}