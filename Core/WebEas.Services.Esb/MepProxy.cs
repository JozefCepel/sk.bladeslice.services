using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;

namespace WebEas.Services.Esb
{
    public class MepProxy : ProxyBase, IDisposable
    {
        private const string ServiceUrl = "https://lbsoa.intra.dcom.sk/soa/mep/1.0/mepGwServices";

        private WebEas.Services.Esb.MepService.MEPGatewayChannel proxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="MepProxy" /> class.
        /// </summary>
        /// <param name="stsThumbprint">The STS thumbprint.</param>
        public MepProxy(string stsThumbprint) : base(stsThumbprint)
        {
            this.proxy = WebEas.Core.Sts.SecurityTokenServiceHelper.CreateChannelProxy<WebEas.Services.Esb.MepService.MEPGatewayChannel>(ServiceUrl, stsThumbprint);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MepProxy" /> class.
        /// </summary>
        /// <param name="stsThumbprint">The STS thumbprint.</param>
        /// <param name="session">The session.</param>
        public MepProxy(string stsThumbprint, IWebEasSession session) : base(stsThumbprint)
        {
            this.proxy = WebEas.Core.Sts.SecurityTokenServiceHelper.CreateChannelProxy<WebEas.Services.Esb.MepService.MEPGatewayChannel>(session, ServiceUrl, stsThumbprint);
        }

        /// <summary>
        /// Sends the payment order.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public MepService.SendPaymentOrderResponse SendPaymentOrder(MepService.SendPaymentOrderRequest request)
        {
            try
            {
                var sendPaymentOrder = new MepService.SendPaymentOrder { SendPaymentOrderRequest = request };
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.SendPaymentOrder(sendPaymentOrder);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response;
            }
            catch (FaultException<WebEas.Services.Esb.MepService.DcomFaultType> ex)
            {
                throw new WebEasProxyException(null, ex.Message, request);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in SendPaymentOrder", "Nastala chyba pri volaní externej služby!", ex, request);
            }
        }

        /// <summary>
        /// Sends the payment received information.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public MepService.SendPaymentReceivedInformationResponse SendPaymentReceivedInformation(MepService.SendPaymentReceivedInformationRequest request)
        {
            try
            {
                var sendPaymentReceivedInformation = new MepService.SendPaymentReceivedInformation { SendPaymentReceivedInformationRequest = request };
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.SendPaymentReceivedInformation(sendPaymentReceivedInformation);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response;
            }
            catch (FaultException<WebEas.Services.Esb.MepService.DcomFaultType> ex)
            {
                throw new WebEasProxyException(null, ex.Message, request);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in SendPaymentReceivedInformation", "Nastala chyba pri volaní externej služby!", ex, request);
            }
        }

        /// <summary>
        /// Sends the procedure termination.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public MepService.SendProcedureTerminationResponse SendProcedureTermination(MepService.SendProcedureTerminationRequest request)
        {
            try
            {
                var sendProcedureTermination = new MepService.SendProcedureTermination { SendProcedureTerminationRequest = request };
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.SendProcedureTermination(sendProcedureTermination);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response;
            }
            catch (FaultException<WebEas.Services.Esb.MepService.DcomFaultType> ex)
            {
                throw new WebEasProxyException(null, ex.Message, request);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in SendProcedureTermination", "Nastala chyba pri volaní externej služby!", ex, request);
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
