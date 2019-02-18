using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;

namespace WebEas.Services.Esb.Egov
{
    /// <summary>
    /// 
    /// </summary>
    public class PlaProxy : ProxyBase, IDisposable
    {
        private const string ServiceUrl = "https://lbsoa.intra.dcom.sk/soa/egov/pla/1.0/pla";

        private Egov.PlaService.IPlaServiceChannel proxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaProxy" /> class.
        /// </summary>
        public PlaProxy(string stsThumbprint) : base(stsThumbprint)
        {
            //#if DEBUG
            //this.proxy = WebEas.Core.Sts.SecurityTokenServiceHelper.CreateChannelProxyDebugLocalhost<Egov.PlaService.IPlaServiceChannel>("http://localhost/wcf/PlaService.svc");
            //#else
            this.proxy = WebEas.Core.Sts.SecurityTokenServiceHelper.CreateChannelProxy<Egov.PlaService.IPlaServiceChannel>(ServiceUrl, stsThumbprint);
            //#endif
        }

        /// <summary>
        /// Saves to evidence.
        /// </summary>
        /// <param name="dPodanieId">The d podanie id.</param>
        public void MepPayment(DateTime date, string refPlat, decimal amount, string messageId)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                this.proxy.MepPayment(date, refPlat, amount, messageId);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
            }
            catch (FaultException<Egov.EdmService.dcomFault> ex)
            {
                throw new WebEasProxyException(string.Format("Error in SaveToEvidence {0} {1}", ex.Detail.faultCause, ex.Detail.faultCode), ex.Detail.faultMessage, ex, date,refPlat,amount);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in SaveToEvidence", ex, date, refPlat, amount);
            }
        }

        /// <summary>
        /// Sends the prescription.
        /// </summary>
        /// <param name="prescription">The prescription.</param>
        public void SendPrescription(WebEas.Services.Esb.Egov.PlaService.PrescriptionFromDap prescription)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                this.proxy.SendPrescription(prescription);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
            }
            catch (FaultException<Egov.PlaService.dcomFault> ex)
            {
                throw new WebEasProxyException(string.Format("Error in SendPrescription {0} {1}", ex.Detail.faultCause, ex.Detail.faultCode), ex.Detail.faultMessage, ex, prescription);                
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in SendPrescription", ex, prescription);
            }
        }

        /// <summary>
        /// Discards the prescription.
        /// </summary>
        /// <param name="dVymerSplatkaIdList">The d vymer splatka id list.</param>
        public void DiscardPrescription(List<long> dVymerSplatkaIdList)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                this.proxy.DiscardPrescription(dVymerSplatkaIdList);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
            }
            catch (FaultException<Egov.PlaService.dcomFault> ex)
            {
                throw new WebEasProxyException(string.Format("Error in DiscardPrescription {0} {1}", ex.Detail.faultCause, ex.Detail.faultCode), ex.Detail.faultMessage, ex, dVymerSplatkaIdList);                                
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in DiscardPrescription", ex, dVymerSplatkaIdList);
            }
        }

        /// <summary>
        /// Updates the prescription mep id.
        /// </summary>
        /// <param name="dVymerSplatkaIdList">The d vymer splatka id list.</param>
        /// <param name="mepId">The mep id.</param>
        public void UpdatePrescriptionMepId(List<long> dVymerSplatkaIdList, string mepId)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                this.proxy.UpdatePrescriptionsMepId(dVymerSplatkaIdList, mepId);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
            }
            catch (FaultException<Egov.PlaService.dcomFault> ex)
            {
                throw new WebEasProxyException(string.Format("Error in UpdatePrescriptionMepId {0} {1}", ex.Detail.faultCause, ex.Detail.faultCode), ex.Detail.faultMessage, ex, dVymerSplatkaIdList, mepId);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in UpdatePrescriptionMepId", ex, dVymerSplatkaIdList, mepId);
            }
        }

        /// <summary>
        /// Changes the payments.
        /// </summary>
        /// <param name="dVymerId">The Vymer ID.</param>
        /// <param name="datum">The datum.</param>
        /// <param name="spravny">The spravny.</param>
        public void ChangePayments(long dVymerId, DateTime? datum, bool? spravny)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                this.proxy.ChangePayments(dVymerId, datum, spravny);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
            }
            catch (FaultException<Egov.PlaService.dcomFault> ex)
            {
                throw new WebEasProxyException(string.Format("Error in ChangePayments {0} {1}", ex.Detail.faultCause, ex.Detail.faultCode), ex.Detail.faultMessage, ex, dVymerId, datum,spravny);                                                
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in ChangePayments", ex, dVymerId, datum, spravny);
            }
        }

        /// <summary>
        /// Refunds the of overpaid.
        /// </summary>
        public void RefundOfOverpaid()
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                this.proxy.RefundOfOverpaid();
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
            }
            catch (FaultException<Egov.PlaService.dcomFault> ex)
            { 
                throw new WebEasProxyException(string.Format("Error in RefundOfOverpaid {0} {1}", ex.Detail.faultCause, ex.Detail.faultCode), ex.Detail.faultMessage, ex);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in RefundOfOverpaid", ex);
            }
        }

        /// <summary>
        /// Changes the due date.
        /// </summary>
        public void ChangeDueDate()
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                this.proxy.ChangeDueDate();
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
            }
            catch (FaultException<Egov.PlaService.dcomFault> ex)
            {
                throw new WebEasProxyException(string.Format("Error in ChangeDueDate {0} {1}", ex.Detail.faultCause, ex.Detail.faultCode), ex.Detail.faultMessage, ex);                
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in ChangeDueDate", ex);
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