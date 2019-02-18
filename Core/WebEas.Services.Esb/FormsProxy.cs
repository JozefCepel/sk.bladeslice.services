using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WebEas.Services.Esb.Mdfx;

namespace WebEas.Services.Esb
{
    public class FormsProxy : ProxyBase, IDisposable
    {
        private const string ServiceUrl = "https://lbsoa.intra.dcom.sk/soa/mdfx/1.0/mdfx";

        private Mdfx.MDFXPortTypeChannel proxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="IamRoleProxy" /> class.
        /// </summary>
        public FormsProxy(string stsThumbprint) : base(stsThumbprint)
        {
            this.proxy = WebEas.Core.Sts.SecurityTokenServiceHelper.CreateChannelProxy<Mdfx.MDFXPortTypeChannel>(ServiceUrl, stsThumbprint);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IamRoleProxy" /> class.
        /// </summary>
        public FormsProxy(string openAmSessionIs, string tenantId, string stsThumbprint) : base(stsThumbprint)
        {
            this.proxy = WebEas.Core.Sts.SecurityTokenServiceHelper.CreateChannelProxy<Mdfx.MDFXPortTypeChannel>(ServiceUrl, openAmSessionIs, tenantId, stsThumbprint);
        }
                
        /// <summary>
        /// Finds the service by form namespace.
        /// </summary>
        /// <param name="formNamespace">The form namespace.</param>
        /// <returns></returns>
        public List<string> FindServiceByFormNamespace(string formNamespace)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var result = new List<string>();
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                findServiceByFormNamespaceResponse response = this.proxy.findServiceByFormNamespace(new Mdfx.findServiceByFormNamespaceRequest(new Mdfx.findServiceByFormNamespaceReq_type { formURI = formNamespace }));
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);

                if (response != null && response.findServiceByFormNamespaceRes != null)
                {
                    result.AddRange(response.findServiceByFormNamespaceRes);                    
                }
                return result;                
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in FindServiceByFormNamespace", ex, formNamespace);
            }
        }

        /// <summary>
        /// Gets the clerk forms.
        /// </summary>
        /// <param name="tenant">The tenant.</param>
        /// <param name="language">The language.</param>
        /// <param name="service">The service.</param>
        /// <returns></returns>
        public List<formDto_type> GetClerkForms(string tenant, string language, string service)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var result = new List<formDto_type>();
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                getClerkFormsResponse response = this.proxy.getClerkForms(new Mdfx.getClerkFormsRequest(new Mdfx.getClerkFormsReq_type { language = language, service = service, tenant = tenant }));
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);

                if (response != null && response.getClerkFormsRes != null)
                {
                    result.AddRange(response.getClerkFormsRes);
                }
                return result;
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetClerkForms", ex, tenant, language, service);
            }
        }

        /// <summary>
        /// Gets the form detail.
        /// </summary>
        /// <param name="formNamespace">The form namespace.</param>
        /// <returns></returns>
        public formDto_type GetFormDetail(string formNamespace)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                getFormDetailResponse response = this.proxy.getFormDetail(new getFormDetailRequest(new getFormDetailReq_type { formNamespace = formNamespace }));
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                if (response != null && response.getFormDetailRes != null)
                {
                    return response.getFormDetailRes.formDto;
                }

                return null;
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetFormDetail", ex, formNamespace);
            }
        }

        /// <summary>
        /// Gets for detail.
        /// </summary>
        /// <param name="transformationType">Type of the transformation.</param>
        /// <param name="xmlData">The XML data.</param>
        /// <returns></returns>
        public byte[] GetFormTransformation(getFormTransformationReq_typeTransformationType transformationType, byte[] xmlData)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                getFormTransformationResponse response = this.proxy.getFormTransformation(new getFormTransformationRequest(new getFormTransformationReq { transformationType = transformationType, xmlData = xmlData }));
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);

                if (response != null && response.getFormTransformationRes != null)
                {
                    return response.getFormTransformationRes.@out;
                }

                return null;
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetForDetail", ex, transformationType, xmlData);
            }
        }

        /// <summary>
        /// Gets the latest form version.
        /// </summary>
        /// <param name="oldFormUri">The old form URI.</param>
        /// <returns></returns>
        public List<string> GetLatestFormVersion(string oldFormUri)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var result = new List<string>();
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                getLatestFormVersionResponse response = this.proxy.getLatestFormVersion(new getLatestFormVersionRequest(new getLatestFormVersionReq_type { oldFormURI = oldFormUri }));
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);

                if (response != null && response.getLatestFormVersionRes != null)
                {
                    result.AddRange(response.getLatestFormVersionRes);
                }

                return result;
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetLatestFormVersion", ex, oldFormUri);
            }
        }

        /// <summary>
        /// Prefetchs the required forms.
        /// </summary>
        /// <param name="requiredForms">The required forms.</param>
        public void PrefetchRequiredForms(List<string> requiredForms)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                this.proxy.prefetchRequiredForms(new prefetchRequiredFormsRequest { prefetchRequiredFormsReq = requiredForms.ToArray() });
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetLatestFormVersion", ex, requiredForms);
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

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                validateFormDataResponse response = this.proxy.validateFormData(new validateFormDataRequest(new validateFormDataReq_type { xmlData = xmlData }));
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                if (response != null && response.validateFormDataRes != null)
                {
                    return response.validateFormDataRes.isValid;
                }

                return false;
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetLatestFormVersion", ex, xmlData);
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
                        this.proxy.Abort();

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