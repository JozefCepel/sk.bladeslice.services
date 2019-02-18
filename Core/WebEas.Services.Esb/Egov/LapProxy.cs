using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;

namespace WebEas.Services.Esb.Egov
{ 
    /// <summary>
    /// Lap proxy
    /// </summary>
    public class LapProxy : ProxyBase, IDisposable
    {
        private const string ServiceUrl = "https://lbsoa.intra.dcom.sk/soa/egov/lap/1.0/lap";        
        
        private Egov.LapService.ILapServiceChannel proxy;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="LapProxy" /> class.
        /// </summary>
        public LapProxy(string stsThumbprint) : base(stsThumbprint)
        {
            this.proxy = WebEas.Core.Sts.SecurityTokenServiceHelper.CreateChannelProxy<Egov.LapService.ILapServiceChannel>(ServiceUrl, stsThumbprint);
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
            catch (FaultException<Egov.LapService.dcomFault> ex)
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
            catch (FaultException<Egov.LapService.dcomFault> ex)
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