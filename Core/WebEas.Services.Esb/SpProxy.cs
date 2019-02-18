using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using WebEas.Services.Esb.Sp;

namespace WebEas.Services.Esb
{
    /// <summary>
    /// 
    /// </summary>
    public class SpProxy : ProxyBase, IDisposable
    {
        private const string ServiceUrl = "https://lbsoa.intra.dcom.sk/soa/sp/1.0/spService";

        private Sp.SPGatewayChannel proxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpProxy" /> class.
        /// </summary>
        /// <param name="stsThumbprint">The STS thumbprint.</param>
        public SpProxy(string stsThumbprint)
            : base(stsThumbprint)
        {
            this.proxy = WebEas.Core.Sts.SecurityTokenServiceHelper.CreateChannelProxy<Sp.SPGatewayChannel>(ServiceUrl, stsThumbprint);
        }

        /// <summary>
        /// Gets the benefit info.
        /// </summary>
        /// <param name="claimFrom">The claim from.</param>
        /// <param name="claimTo">The claim to. Dat. rozsah moze byt max 12 mesiacov! v opacnom pripade to na WS zahuci.</param>
        /// <param name="dcomId">The DCOM id.</param>
        /// <param name="dossierId">The dossier id.</param>
        /// <param name="insuranceTypeRequired">The insurance type required.</param>
        /// <returns></returns>
        public GetBenefitInfoResponse GetBenefitInfo(DateTime claimFrom, DateTime? claimTo, string dcomId, string dossierId, List<InsuranceTypeRequiredType> insuranceTypeRequired)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new getBenefitInfoReq
                {
                    GetBenefitInfoReq1 = new GetBenefitInfoRequest
                    {
                        ClaimFrom = claimFrom,
                        ClaimTo = claimTo ?? claimFrom.AddYears(1),
                        ClaimToSpecified = true,
                        DcomID = dcomId,
                        DossierID = dossierId,
                        InsuranceTypeRequired = insuranceTypeRequired == null ? null : insuranceTypeRequired.ToArray()
                    }
                };
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                getBenefitInfoRes response = this.proxy.GetBenefitInfo(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.GetBenefitInfoRes1;
            }
            catch (FaultException<WebEas.Services.Esb.Sp.DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetBenefitInfo", "Nastala chyba pri volaní služby SP", ex, claimFrom, claimTo, dcomId, dossierId, insuranceTypeRequired);
            }
        }

        /// <summary>
        /// Checks the employment status.
        /// </summary>
        /// <param name="dateOfInterest">The date of interest.</param>
        /// <param name="dcomId">The DCOM id.</param>
        /// <param name="dossierId">The dossier id.</param>
        /// <param name="organizationId">The organization id.</param>
        /// <returns></returns>
        public CheckEmploymentStatusResponse CheckEmploymentStatus(DateTime dateOfInterest, string dcomId, string dossierId, string organizationId)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new checkEmploymentStatusReq { CheckEmploymentStatusReq1 = new CheckEmploymentStatusRequest { DateOfInterest = dateOfInterest, DcomID = dcomId, DossierID = dossierId, OrganizationID = organizationId } };
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                checkEmploymentStatusRes response = this.proxy.CheckEmploymentStatus(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.CheckEmploymentStatusRes1;
            }
            catch (FaultException<WebEas.Services.Esb.Sp.DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in CheckEmploymentStatus", "Nastala chyba pri volaní služby SP", ex, dateOfInterest, dcomId, dossierId, organizationId);
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