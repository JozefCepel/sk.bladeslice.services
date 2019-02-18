using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using WebEas.Core.Sts;

namespace WebEas.Services.Esb.Egov
{
    public class EviProxy : ProxyBase, IDisposable
    {
        private const string ServiceUrl = "https://lbsoa.intra.dcom.sk/soa/egov/evi/1.0/evi";
        
        private Egov.EviService.IEviServiceChannel proxy;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="EviProxy" /> class.
        /// </summary>
        public EviProxy(string stsThumbprint) : base(stsThumbprint)
        {
            this.proxy = SecurityTokenServiceHelper.CreateChannelProxy<Egov.EviService.IEviServiceChannel>(ServiceUrl, stsThumbprint);
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
            catch (FaultException<Egov.EviService.dcomFault> ex)
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
            catch (FaultException<Egov.EviService.dcomFault> ex)
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
        /// Create pes
        /// </summary>
        /// <param name="pes"></param>
        /// <returns></returns>
        public EviService.PesView CreatePes(EviService.CreatePes pes)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var res = proxy.CreatePes(pes);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return res;
            }
            catch (FaultException<Egov.EviService.dcomFault> ex)
            {
                throw new WebEasProxyException(string.Format("Error in CreatePes {0} {1}", ex.Detail.faultCause, ex.Detail.faultCode), ex.Detail.faultMessage, ex, pes);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in CreatePes", ex, pes);
            }
        }

        /// <summary>
        /// Update pes
        /// </summary>
        /// <param name="pes"></param>
        /// <returns></returns>
        public EviService.PesView UpdatePes(EviService.UpdatePes pes)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var res = proxy.UpdatePes(pes);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return res;
            }
            catch (FaultException<Egov.EviService.dcomFault> ex)
            {
                throw new WebEasProxyException(string.Format("Error in UpdatePes {0} {1}", ex.Detail.faultCause, ex.Detail.faultCode), ex.Detail.faultMessage, ex, pes);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in UpdatePes", ex, pes);
            }
        }

        /// <summary>
        /// Delete pes
        /// </summary>
        /// <param name="pes"></param>
        /// <returns></returns>
        public long DeletePes(EviService.DeletePes pes)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var res = proxy.DeletePes(pes);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return res;
            }
            catch (FaultException<Egov.EviService.dcomFault> ex)
            {
                throw new WebEasProxyException(string.Format("Error in DeletePes {0} {1}", ex.Detail.faultCause, ex.Detail.faultCode), ex.Detail.faultMessage, ex, pes);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in DeletePes", ex, pes);
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