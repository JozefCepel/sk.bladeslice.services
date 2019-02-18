using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;

namespace WebEas.Services.Esb.Egov
{
    /// <summary>
    /// Dap proxy
    /// </summary>
    public class DapProxy : ProxyBase, IDisposable
    {
        private const string ServiceUrl = "https://lbsoa.intra.dcom.sk/soa/egov/dap/1.0/dap";

        private Egov.DapService.IDapServiceChannel proxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="DapProxy" /> class.
        /// </summary>
        public DapProxy(string stsThumbprint) : base(stsThumbprint)
        {
            //#if DEBUG
            //this.proxy = WebEas.Core.Sts.SecurityTokenServiceHelper.CreateChannelProxyDebugLocalhost<Egov.DapService.IDapServiceChannel>("http://localhost/wcf/DapService.svc");
            //#else
            this.proxy = WebEas.Core.Sts.SecurityTokenServiceHelper.CreateChannelProxy<Egov.DapService.IDapServiceChannel>(ServiceUrl, stsThumbprint);
            //#endif
        }

        /// <summary>
        /// Saves to evidence.
        /// </summary>
        /// <param name="dPodanieId">The d podanie id.</param>
        public void SaveToEvidence(long dPodanieId)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                this.proxy.SaveToEvidence(dPodanieId);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
            }
            catch (FaultException<Egov.DapService.dcomFault> ex)
            {
                throw new WebEasProxyException(string.Format("Error in SaveToEvidence {0} {1}", ex.Detail.faultCause, ex.Detail.faultCode), ex.Detail.faultMessage, ex, dPodanieId);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in SaveToEvidence", ex, dPodanieId);
            }
        }

        /// <summary>
        /// Saves xml to evidence.
        /// </summary>
        /// <param name="dPodanieId">The d podanie id.</param>
        /// <param name="xml">The xml.</param>
        public void SaveToEvidenceXml(long dPodanieId, string xml)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                this.proxy.SaveToEvidenceXml(dPodanieId, xml);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
            }
            catch (FaultException<Egov.DapService.dcomFault> ex)
            {
                throw new WebEasProxyException(string.Format("Error in SaveToEvidenceXml {0} {1}", ex.Detail.faultCause, ex.Detail.faultCode), ex.Detail.faultMessage, ex, dPodanieId, xml);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in SaveToEvidenceXml", ex, dPodanieId, xml);
            }
        }

        /// <summary>
        /// Generates the prescription.
        /// </summary>
        /// <param name="dVymerId">The d vymer id.</param>
        public void GeneratePrescription(long dVymerId)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                this.proxy.GeneratePrescription(dVymerId);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
            }
            catch (FaultException<Egov.DapService.dcomFault> ex)
            {
                throw new WebEasProxyException(string.Format("Error in GeneratePrescription {0} {1}", ex.Detail.faultCause, ex.Detail.faultCode), ex.Detail.faultMessage, ex, dVymerId);                
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GeneratePrescription", ex, dVymerId);
            }
        }

        /// <summary>
        /// Raises the internal payment.
        /// </summary>
        /// <param name="data">The data.</param>
        public WebEas.Services.Esb.Egov.DapService.RaiseInternalPaymentResponse RaiseInternalPayment(WebEas.Services.Esb.Egov.DapService.InternalPayment data)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var res = this.proxy.RaiseInternalPayment(data);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return res;
            }
            catch (FaultException<Egov.DapService.dcomFault> ex)
            {
                throw new WebEasProxyException(string.Format("Error in RaiseInternalPayment {0} {1}", ex.Detail.faultCause, ex.Detail.faultCode), ex.Detail.faultMessage, ex, data);                  
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in RaiseInternalPayment", ex, data);
            }
        }

        /// <summary>
        /// Cancels the internal payment.
        /// </summary>
        /// <param name="dPodanieId">The d podanie id.</param>
        public void CancelInternalPayment(long dPodanieId)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                this.proxy.CancelInternalPayment(dPodanieId);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
            }
            catch (FaultException<Egov.DapService.dcomFault> ex)
            {
                throw new WebEasProxyException(string.Format("Error in CancelInternalPayment {0} {1}", ex.Detail.faultCause, ex.Detail.faultCode), ex.Detail.faultMessage, ex, dPodanieId);  
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in CancelInternalPayment", ex, dPodanieId);
            }
        }

        /// <summary>
        /// Notifies the record related event.
        /// </summary>
        /// <param name="notifyRecordRelatedEvent">The notify record related event.</param>
        public void NotifyRecordRelatedEvent(WebEas.Services.Esb.Egov.DapService.NotifyRecordRelatedEvent notifyRecordRelatedEvent)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                this.proxy.NotifyRecordRelatedEvent(notifyRecordRelatedEvent);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
            }
            catch (FaultException<Egov.DapService.dcomFault> ex)
            {
                throw new WebEasProxyException(string.Format("Error in NotifyRecordRelatedEvent {0} {1}", ex.Detail.faultCause, ex.Detail.faultCode), ex.Detail.faultMessage, ex, notifyRecordRelatedEvent);                 
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in NotifyRecordRelatedEvent", ex, notifyRecordRelatedEvent);
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