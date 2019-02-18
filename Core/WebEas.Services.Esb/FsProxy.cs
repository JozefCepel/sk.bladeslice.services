using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using WebEas.Services.Esb.Fs;

namespace WebEas.Services.Esb
{
    /// <summary>
    /// 
    /// </summary>
    public class FsProxy : ProxyBase, IDisposable
    {
        private const string ServiceUrl = "https://lbsoa.intra.dcom.sk/soa/fsgw/1.0/fs";

        private Fs.FSGatewayChannel proxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="FsProxy" /> class.
        /// </summary>
        /// <param name="stsThumbprint">The STS thumbprint.</param>
        public FsProxy(string stsThumbprint)
            : base(stsThumbprint)
        {
            this.proxy = WebEas.Core.Sts.SecurityTokenServiceHelper.CreateChannelProxy<Fs.FSGatewayChannel>(ServiceUrl, stsThumbprint);
        }

        /// <summary>
        /// Gets duty arrears info
        /// </summary>
        /// <param name="dcomId">The DCOM id.</param>
        /// <param name="dossierId">The dossier id.</param>
        /// <returns></returns>
        public GetDutyArrearsInfoResponse GetDutyArrearsInfo(string dcomId, string dossierId)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new getDutyArrearsInfoReq { GetDutyArrearsInfoReq1 = new GetDutyArrearsInfoRequest() { DcomID = dcomId, DossierID = dossierId } };
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                getDutyArrearsInfoRes response = this.proxy.GetDutyArrearsInfo(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.GetDutyArrearsInfoRes1;
            }
            catch (FaultException<WebEas.Services.Esb.Fs.DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetDutyArrearsInfo", "Nastala chyba pri volaní služby FS", ex, dcomId, dossierId);
            }
        }

        /// <summary>
        /// Sends get duty arrears info request
        /// </summary>
        /// <param name="dcomId">The DCOM id.</param>
        /// <param name="dossierId">The dossier id.</param>
        /// <returns></returns>
        public SendGetDutyArrearsInfoRequestResponse SendGetDutyArrearsInfoRequest(string dcomId, string dossierId)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new sendGetDutyArrearsInfoRequestReq { SendGetDutyArrearsInfoRequestReq1 = new SendGetDutyArrearsInfoRequestRequest { DcomID = dcomId, DossierID = dossierId } };
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                sendGetDutyArrearsInfoRequestRes response = this.proxy.SendGetDutyArrearsInfoRequest(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.SendGetDutyArrearsInfoRequestRes1;
            }
            catch (FaultException<WebEas.Services.Esb.Fs.DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in SendGetDutyArrearsInfoRequest", "Nastala chyba pri volaní služby FS", ex, dcomId, dossierId);
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