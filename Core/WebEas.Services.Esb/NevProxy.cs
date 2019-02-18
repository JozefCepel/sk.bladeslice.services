using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using WebEas.Services.Esb.Nev;

namespace WebEas.Services.Esb
{
    public class NevProxy : ProxyBase, IDisposable
    {
        private const string ServiceUrl = "https://lbsoa.intra.dcom.sk/soa/nev/1.0/vehicleService";

        private Nev.NEVGatewayChannel proxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="NevProxy" /> class.
        /// </summary>
        public NevProxy(string stsThumbprint) : base(stsThumbprint)
        {
            this.proxy = WebEas.Core.Sts.SecurityTokenServiceHelper.CreateChannelProxy<Nev.NEVGatewayChannel>(ServiceUrl, stsThumbprint);
        }

        /// <summary>
        /// Gets VehicleInfo by subjectUId and ECV
        /// </summary>
        /// <param name="subjectId"></param>
        /// <param name="ecv"></param>
        public VehicleInfo GetVehicleInfo(string subjectId, string ecv, string dossierID, bool findHolder = true, bool findOwner = true)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                getVehicleInfoRes response = this.proxy.GetVehicleInfo(new Nev.getVehicleInfoReq()
                {
                    GetVehicleInfoReq1 = new Nev.SearchCriteria
                    {
                        DcomID = subjectId,
                        FindHolder = findHolder,
                        FindOwner = findOwner,
                        VehicleRegistrationNumber = ecv,
                        DossierID = dossierID
                    }
                });
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.GetVehicleInfoRes1;
            }
            catch (FaultException<WebEas.Services.Esb.Nev.DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetVehicleInfo", ex, subjectId, ecv);
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