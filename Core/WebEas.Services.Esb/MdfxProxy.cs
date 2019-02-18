using System;
using System.Diagnostics;
using System.Linq;
using WebEas.Services.Esb.Mdfx;

namespace WebEas.Services.Esb
{
    public class MdfxProxy : ProxyBase, IDisposable
    {
        private const string ServiceUrl = "https://lbsoa.intra.dcom.sk/soa/mdfx/1.0/mdfx";

        private Mdfx.MDFXPortTypeChannel proxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonProxy" /> class.
        /// </summary>
        public MdfxProxy(string stsThumbprint)
            : base(stsThumbprint)
        {
            this.proxy = WebEas.Core.Sts.SecurityTokenServiceHelper.CreateChannelProxy<Mdfx.MDFXPortTypeChannel>(ServiceUrl, stsThumbprint);
        }

        /// <summary>
        /// Finds the service by form namespace.
        /// </summary>
        /// <param name="formUri">The form URI.</param>
        /// <returns></returns>
        public string[] FindServiceByFormNamespace(string formUri)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new Mdfx.findServiceByFormNamespaceRequest(new Mdfx.findServiceByFormNamespaceReq_type { formURI = formUri });
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                findServiceByFormNamespaceResponse response = this.proxy.findServiceByFormNamespace(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.findServiceByFormNamespaceRes;
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in FindServiceByFormNamespace", ex, formUri);
            }
        }

        /// <summary>
        /// Gets the clerk forms.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="service">The service.</param>
        /// <param name="tenant">The tenant.</param>
        /// <returns></returns>
        public Mdfx.formDto_type[] GetClerkForms(string language, string service, string tenant)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new Mdfx.getClerkFormsRequest(new Mdfx.getClerkFormsReq_type { language = language, service = service, tenant = tenant });
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                getClerkFormsResponse response = this.proxy.getClerkForms(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.getClerkFormsRes;
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetClerkForms", ex, language, service, tenant);
            }
        }

        /// <summary>
        /// Gets the form detail.
        /// </summary>
        /// <param name="formNamespace">The form namespace.</param>
        /// <returns></returns>
        public Mdfx.formDto_type GetFormDetail(string formNamespace)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new Mdfx.getFormDetailRequest(new Mdfx.getFormDetailReq_type { formNamespace = formNamespace });
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                getFormDetailResponse response = this.proxy.getFormDetail(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.getFormDetailRes.formDto;
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetClerkForms", ex, formNamespace);
            }
        }

        /// <summary>
        /// Gets the form transformation.
        /// </summary>
        /// <param name="transformationType">Type of the transformation.</param>
        /// <param name="xmlData">The XML data.</param>
        /// <returns></returns>
        public byte[] GetFormTransformation(Mdfx.getFormTransformationReq_typeTransformationType transformationType, byte[] xmlData)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new Mdfx.getFormTransformationRequest(new Mdfx.getFormTransformationReq { transformationType = transformationType, xmlData = xmlData });
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                getFormTransformationResponse response = this.proxy.getFormTransformation(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.getFormTransformationRes.@out;
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetClerkForms", ex, transformationType, xmlData);
            }
        }

        /// <summary>
        /// Gets the latest form version.
        /// </summary>
        /// <param name="oldFormUri">The old form URI.</param>
        /// <returns></returns>
        public string[] GetLatestFormVersion(string oldFormUri)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new Mdfx.getLatestFormVersionRequest(new Mdfx.getLatestFormVersionReq_type { oldFormURI = oldFormUri });
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                getLatestFormVersionResponse response = this.proxy.getLatestFormVersion(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.getLatestFormVersionRes;
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetClerkForms", ex, oldFormUri);
            }
        }

        /// <summary>
        /// Prefetchs the required forms.
        /// </summary>
        /// <param name="prefetchRequiredForms">The prefetch required forms.</param>
        /// <returns></returns>
        public Mdfx.prefetchRequiredFormsRes_type PrefetchRequiredForms(string[] prefetchRequiredForms)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new Mdfx.prefetchRequiredFormsRequest(prefetchRequiredForms);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                prefetchRequiredFormsResponse response = this.proxy.prefetchRequiredForms(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.prefetchRequiredFormsRes;
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetClerkForms", ex, prefetchRequiredForms);
            }
        }

        /// <summary>
        /// Validates the form data.
        /// </summary>
        /// <param name="xmlData">The XML data.</param>
        /// <returns></returns>
        public bool ValidateFormData(byte[] xmlData)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new Mdfx.validateFormDataRequest(new Mdfx.validateFormDataReq_type { xmlData = xmlData });
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                validateFormDataResponse response = this.proxy.validateFormData(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.validateFormDataRes.isValid;
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetClerkForms", ex, xmlData);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (this.proxy != null && this.proxy is IDisposable)
                {
                    if (this.proxy.State == System.ServiceModel.CommunicationState.Faulted)
                    {
                        this.proxy.Abort();
                    }

                    ((IDisposable)this.proxy).Dispose();
                }
                this.proxy = null;
            }
            catch (Exception ex)
            {
                throw new WebEasException("Error in dispose", ex);
            }
        }
    }
}