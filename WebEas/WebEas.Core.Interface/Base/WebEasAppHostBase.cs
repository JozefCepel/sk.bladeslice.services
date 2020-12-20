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

        public override ServiceStackHost Init()
        {
            ServiceStack.Licensing.RegisterLicense(@"8008-e1JlZjo4MDA4LE5hbWU6IkRBVEFMQU4sIGEucy4iLFR5cGU6QnVzaW5lc3MsTWV0YTowLEhhc2g6TzZNRlQxNGFTb1RyV2J3OVJtQkk5T0YwUVQxUTdYUVZLc1Q4OTZCTURyV00yVG1SdlFTeGlNR0J2elljM2dnMGUxZlpTRGxmV1JmYjhGa0RURUlRMG5IU0hSaFMyYzU3T0FPS0pvaXJXdzZZTzN3a2RpWWNNL2dtYnlsTnFScDMxVUVZY2FSZklaWkxoamxXMzlod25WRHRyZ0tBVURjWnpMa0NtWGY0d2ZvPSxFeHBpcnk6MjAyMS0wMy0yM30=");
            return base.Init();
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
            container.Adapter = new NinjectContainerAdapter(this.AddNinjectBinding(NinjectContainerAdapter.CreateKernel()));
            //new EnumSerializerConfigurator().WithAssemblies(new List<Assembly> { typeof(HierarchyNode).Assembly }).WithNullableEnumSerializers().Configure();                       
            //  this.Plugins.Add(new ValidationFeature());            
            this.ConfigureSerialization();

            ServiceExceptionHandlers.Add((httpReq, request, exception) => {
                //log your exceptions here
                
                var response = WebEasErrorHandling.CreateErrorResponse(httpReq, request, exception);

                if (response.Response is Exceptions.WebEasResponseStatus)
                {
#if DEBUG || DEVELOP
                    ((Exceptions.WebEasResponseStatus)response.Response).DetailMessage += $"{Environment.NewLine}http://esam-dev.datalan.sk/esam/api/pfe/lll/{exception.GetIdentifier()}";
#endif

                }

                return response;
                
            });

            //Handle Unhandled Exceptions occurring outside of Services
            //E.g. Exceptions during Request binding or in filters:
            //this.UncaughtExceptionHandlers.Add((req, res, operationName, ex) => {
                //res.Write($"Error: {ex.GetType().Name}: {ex.Message}");
                //res.EndRequest(skipHeaders: true);
            //});
        }

        /// <summary>
        /// Configures the serialization.
        /// </summary>
        private void ConfigureSerialization()
        {
            //MSSQL DateTime nema zonu, kedze viem , ze sme posunuti, musim  si zonu pridat a potom dam do ISO8601 - "o"
            JsConfig<DateTime>.IncludeDefaultValue = true;
            JsConfig<DateTime>.SerializeFn = time => new DateTime(time.Ticks, DateTimeKind.Utc).ToString("o");
            JsConfig<DateTime?>.SerializeFn = time => time.HasValue ? new DateTime(time.Value.Ticks, DateTimeKind.Utc).ToString("o") : null;
            JsConfig<TimeSpan>.SerializeFn = time => new DateTime(time.Ticks, DateTimeKind.Utc).ToString("o");
            JsConfig<TimeSpan?>.SerializeFn = time => time.HasValue ? new DateTime(time.Value.Ticks, DateTimeKind.Utc).ToString("o") : null;
            JsConfig<TimeSpan>.DeSerializeFn = time =>
            {
                /*if (DateTime.TryParse(time, out DateTime result))
                {
                    return result.TimeOfDay;
                }
                return new TimeSpan();*/
                return DateTime.Parse(time).TimeOfDay;
            };

            JsConfig<TimeSpan?>.DeSerializeFn = time =>
            {
                /*if (DateTime.TryParse(time, out DateTime result))
                {
                    return result.TimeOfDay;
                }
                return null;*/

                if (!string.IsNullOrWhiteSpace(time))
                {
                    return DateTime.Parse(time).TimeOfDay;
                }
                return null;
            };

            JsConfig<Guid>.SerializeFn = guid => guid.ToString();
            JsConfig<Guid?>.SerializeFn = guid => guid.HasValue ? guid.ToString() : null;

            JsConfig<string>.DeSerializeFn = rt => { return (string.IsNullOrEmpty(rt) || rt == "null" ? null : rt); };

            JsConfig.IncludeNullValues = true;
            JsConfig.ThrowOnError = true;
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