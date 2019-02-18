using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using WebEas.Services.Esb.Rsd;

namespace WebEas.Services.Esb
{
    /// <summary>
    /// Rsd proxy
    /// </summary>
    public class RsdProxy : ProxyBase, IDisposable
    {
        private const string ServiceUrl = "https://lbsoa.intra.dcom.sk/soa/rsd/1.0/rsdService";

        private Rsd.RSDGatewayChannel proxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="RsdProxy" /> class.
        /// </summary>
        /// <param name="stsThumbprint">The STS thumbprint.</param>
        public RsdProxy(string stsThumbprint)
            : base(stsThumbprint)
        {
            this.proxy = WebEas.Core.Sts.SecurityTokenServiceHelper.CreateChannelProxy<Rsd.RSDGatewayChannel>(ServiceUrl, stsThumbprint);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RsdProxy" /> class.
        /// </summary>
        /// <param name="stsThumbprint">The STS thumbprint.</param>
        /// <param name="session">The session.</param>
        public RsdProxy(string stsThumbprint, IWebEasSession session)
            : base(stsThumbprint)
        {
            this.proxy = WebEas.Core.Sts.SecurityTokenServiceHelper.CreateChannelProxy<Rsd.RSDGatewayChannel>(session, ServiceUrl, stsThumbprint);
        }

        /// <summary>
        /// Gets the family related benefits for applicant.
        /// </summary>
        /// <param name="applicantRequest">The applicant request.</param>
        /// <returns></returns>
        public Rsd.ApplicantResponse GetFamilyRelatedBenefitsForApplicant(Rsd.ApplicantRequest applicantRequest)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref this.proxy, ServiceUrl, this.stsThumbPrint);
                var httpRequestProperty = new HttpRequestMessageProperty();
                httpRequestProperty.Headers.Add("Content-Type", "application/soap+xml;charset=UTF-8;action=\"GetFamilyRelatedBenefitsForApplicant\"");
                
                using (var contextScope = new OperationContextScope(proxy))
                {
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                    var request = new Rsd.getFamilyRelatedBenefitsForApplicantReq();
                    request.GetFamilyRelatedBenefitsForApplicantReq1 = applicantRequest;
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();
                    getFamilyRelatedBenefitsForApplicantRes response = this.proxy.GetFamilyRelatedBenefitsForApplicant(request);
                    stopwatch.Stop();
                    LogRequestDuration(ServiceUrl, stopwatch);
                    return response.GetFamilyRelatedBenefitsForApplicantRes1;
                }
                
            }
            catch (FaultException<Rsd.DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetFamilyRelatedBenefitsForApplicant", "Nastala chyba pri volaní služby Rsd", ex, applicantRequest);
            }
        }

        /// <summary>
        /// Gets the material need benefits for applicant.
        /// </summary>
        /// <param name="applicantRequest">The applicant request.</param>
        /// <returns></returns>
        public Rsd.ApplicantResponse GetMaterialNeedBenefitsForApplicant(Rsd.ApplicantRequest applicantRequest)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref this.proxy, ServiceUrl, this.stsThumbPrint);
                var httpRequestProperty = new HttpRequestMessageProperty();
                httpRequestProperty.Headers.Add("Content-Type", "application/soap+xml;charset=UTF-8;action=\"GetMaterialNeedBenefitsForApplicant\"");

                using (var contextScope = new OperationContextScope(proxy))
                {
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                    var request = new Rsd.getMaterialNeedBenefitsForApplicantReq();
                    request.GetMaterialNeedBenefitsForApplicantReq1 = applicantRequest;
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();
                    getMaterialNeedBenefitsForApplicantRes response = this.proxy.GetMaterialNeedBenefitsForApplicant(request);
                    stopwatch.Stop();
                    LogRequestDuration(ServiceUrl, stopwatch);
                    return response.GetMaterialNeedBenefitsForApplicantRes1;
                }
            }
            catch (FaultException<Rsd.DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetMaterialNeedBenefitsForApplicant", "Nastala chyba pri volaní služby Rsd", ex, applicantRequest);
            }
        }

        /// <summary>
        /// Gets the health disability benefits for applicant.
        /// </summary>
        /// <param name="applicantRequest">The applicant request.</param>
        /// <returns></returns>
        public Rsd.ApplicantResponse GetHealthDisabilityBenefitsForApplicant(Rsd.ApplicantRequest applicantRequest)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref this.proxy, ServiceUrl, this.stsThumbPrint);
                var httpRequestProperty = new HttpRequestMessageProperty();
                httpRequestProperty.Headers.Add("Content-Type", "application/soap+xml;charset=UTF-8;action=\"GetHealthDisabilityBenefitsForApplicant\"");

                using (var contextScope = new OperationContextScope(proxy))
                {
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                    var request = new Rsd.getHealthDisabilityBenefitsForApplicantReq();
                    request.GetHealthDisabilityBenefitsForApplicantReq1 = applicantRequest;
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();
                    getHealthDisabilityBenefitsForApplicantRes response = this.proxy.GetHealthDisabilityBenefitsForApplicant(request);
                    stopwatch.Stop();
                    LogRequestDuration(ServiceUrl, stopwatch);
                    return response.GetHealthDisabilityBenefitsForApplicantRes1;
                }
            }
            catch (FaultException<Rsd.DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetHealthDisabilityBenefitsForApplicant", "Nastala chyba pri volaní služby Rsd", ex, applicantRequest);
            }
        }

        /// <summary>
        /// Gets the person disability status.
        /// </summary>
        /// <param name="statusRequest">The status request.</param>
        /// <returns></returns>
        public Rsd.GetPersonDisabilityStatusResponse GetPersonDisabilityStatus(Rsd.GetPersonDisabilityStatusRequest statusRequest)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref this.proxy, ServiceUrl, this.stsThumbPrint);
                var httpRequestProperty = new HttpRequestMessageProperty();
                httpRequestProperty.Headers.Add("Content-Type", "application/soap+xml;charset=UTF-8;action=\"GetPersonDisabilityStatus\"");

                using (var contextScope = new OperationContextScope(proxy))
                {
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                    var request = new Rsd.getPersonDisabilityStatusReq();
                    request.GetPersonDisabilityStatusReq1 = statusRequest;
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();
                    getPersonDisabilityStatusRes response = this.proxy.GetPersonDisabilityStatus(request);
                    stopwatch.Stop();
                    LogRequestDuration(ServiceUrl, stopwatch);
                    return response.GetPersonDisabilityStatusRes1;
                }
            }
            catch (FaultException<Rsd.DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetPersonDisabilityStatus", "Nastala chyba pri volaní služby Rsd", ex, statusRequest);
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