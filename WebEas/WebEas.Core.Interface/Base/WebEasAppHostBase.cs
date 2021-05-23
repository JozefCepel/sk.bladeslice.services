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
            ServiceStack.Licensing.RegisterLicense(@"17741-e1JlZjoxNzc0MSxOYW1lOiJEQVRBTEFOLCBhLnMuIixUeXBlOkJ1c2luZXNzLE1ldGE6MCxIYXNoOlhBeTIxZi9GZUtGR1ZGRk1pTmlabXZVazhuc1lDLzA5ZGdlbGVJS0VjQUw1OVdEWnY0MnN2OFJVSGtsdXZ2ODJ6aUdDdFBKVE1DTUZObHBEY2huZzV2RmNLejFMcnJvbzVBQkJXZGwyWVdoeDZvVTRyZm9Jc2pKRnp1dlF1UCtuaCtZeGNmUkE5LzZsZFVrUWQvckR2dkhIQkpuelIxUjFPV1krM2YwUVc1bz0sRXhwaXJ5OjIwMjItMDMtMjR9");
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
#if DEBUG
                    ((Exceptions.WebEasResponseStatus)response.Response).DetailMessage += $"{Environment.NewLine}http://localhost:82/esam/api/pfe/lll/{exception.GetIdentifier()}";
#elif DEVELOP
                    ((Exceptions.WebEasResponseStatus)response.Response).DetailMessage += $"{Environment.NewLine}https://esam-dev.datalan.sk/esam/api/pfe/lll/{exception.GetIdentifier()}";
#elif ITP
                    ((Exceptions.WebEasResponseStatus)response.Response).DetailMessage += $"{Environment.NewLine}https://esam-test.datalan.sk/esam/api/pfe/lll/{exception.GetIdentifier()}";
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
            static DateTime ModifyDayLightSavingTime(DateTime date)
            {
                if (date.IsDaylightSavingTime())
                {
                    var localTime = date.AddHours(1);
                    return localTime;
                }
                return date;
            }

            //MSSQL DateTime nema zonu, kedze viem , ze sme posunuti, musim  si zonu pridat a potom dam do ISO8601 - "o"
            JsConfig<DateTime>.SerializeFn = time =>
            {
                return new DateTime(ModifyDayLightSavingTime(time).Ticks, DateTimeKind.Local).ToString("o");
            };


            JsConfig<DateTime?>.SerializeFn = time =>
            {
                return time.HasValue ? new DateTime(ModifyDayLightSavingTime(time.Value).Ticks, DateTimeKind.Local).ToString("o") : null;
            };

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