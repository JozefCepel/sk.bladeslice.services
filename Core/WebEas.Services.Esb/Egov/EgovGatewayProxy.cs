using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;

namespace WebEas.Services.Esb.Egov
{
    public class EgovGatewayProxy : ProxyBase, IDisposable
    {
        private const string ServiceUrl = "https://lbsoa.intra.dcom.sk/soa/egovgateway/1.0/egovgateway";        
        
        private Egov.EgovGateway.IEgovGatewayServiceChannel proxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="DmsDocumentProxy" /> class.
        /// </summary>
        public EgovGatewayProxy(string stsThumbprint) : base(stsThumbprint)
        {
            this.proxy = WebEas.Core.Sts.SecurityTokenServiceHelper.CreateChannelProxy<Egov.EgovGateway.IEgovGatewayServiceChannel>(ServiceUrl, stsThumbprint);
        }

        /// <summary>
        /// Processes the submission.
        /// </summary>
        /// <param name="recordId">The record id.</param>
        /// <param name="identifier">The identifier.</param>
        public void ProcessSubmission(long recordId, string identifier)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                this.proxy.ProcessSubmission(recordId, identifier);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
            }
            catch (FaultException<Egov.EgovGateway.dcomFault> ex)
            {
                throw new WebEasProxyException(string.Format("Error in ProcessSubmission {0} {1}", ex.Detail.faultCause, ex.Detail.faultCode), ex.Detail.faultMessage, ex, recordId, identifier);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in ProcessSubmission", ex, recordId, identifier);
            }
        }

        /// <summary>
        /// Notifies the record related event.
        /// </summary>
        public void NotifyRecordRelatedEvent(EgovGateway.NotifyRecordRelatedEvent request)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                this.proxy.NotifyRecordRelatedEvent(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
            }
            catch (FaultException<Egov.EgovGateway.dcomFault> ex)
            {
                throw new WebEasProxyException(string.Format("Error in NotifyRecordRelatedEvent {0} {1}", ex.Detail.faultCause, ex.Detail.faultCode), ex.Detail.faultMessage, ex, request);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in NotifyRecordRelatedEvent", ex);
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