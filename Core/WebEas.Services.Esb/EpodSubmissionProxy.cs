using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;

namespace WebEas.Services.Esb
{
    /// <summary>
    /// Epodatelna submission proxy
    /// </summary>
    /// <example>
    /// using(EpodSubmissionProxy proxy = new EpodSubmissionProxy())
    /// {
    /// }
    /// </example>
    public class EpodSubmissionProxy : ProxyBase, IDisposable
    {
        private const string ServiceUrl = "https://lbsoa.intra.dcom.sk/soa/epodatelna/submission/1.0/submissionServices";

        private Epod.Submission.SubmissionServicesChannel proxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="EpodSubmissionProxy" /> class.
        /// </summary>
        public EpodSubmissionProxy(string stsThumbprint) : base(stsThumbprint)
        {
            this.proxy = WebEas.Core.Sts.SecurityTokenServiceHelper.CreateChannelProxy<Epod.Submission.SubmissionServicesChannel>(ServiceUrl, stsThumbprint);
        }

        /// <summary>
        /// Creates the submission.
        /// </summary>
        /// <param name="createSubmissionDto">The create submission dto.</param>
        public WebEas.Services.Esb.Epod.Submission.createSubmissionResponse CreateSubmission(WebEas.Services.Esb.Epod.Submission.CreateSubmissionDto createSubmissionDto)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref this.proxy, ServiceUrl, this.stsThumbPrint);

                var request = new Epod.Submission.createSubmission(createSubmissionDto);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var res = proxy.createSubmission(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return res;
            }
            catch (FaultException<WebEas.Services.Esb.Epod.Submission.DcomFault> ex)
            { 
                if (ex.Detail.faultMessage.Contains("DMS_NOT_AVAILABLE"))
                {
                    throw new WebEasProxyException("DMS", "Úložisko dokumentov nie je dostupné (DMS)", ex);
                }
             
                throw new WebEasProxyException(string.Format("Fault Exception in InsertRecord : {0}", ex.Message), string.Format("Nastala chyba pri volaní služby podateľne - {0}", ex.Detail.faultMessage), ex, createSubmissionDto, ex.Detail);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in createSubmission", "Nastala chyba pri volaní služby podateľne", ex, createSubmissionDto);
            }
        }

        /// <summary>
        /// Get the submission.
        /// </summary>
        /// <param name="getSubmissionDto">Get submission dto.</param>
        public Epod.Submission.GetSubmissionsResponseDto GetSubmissions(Epod.Submission.GetSubmissionsRequestDto getSubmissionDto)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref this.proxy, ServiceUrl, this.stsThumbPrint);

                var request = new Epod.Submission.getSubmissions(getSubmissionDto);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var res = this.proxy.getSubmissions(request).getSubmissionsResponseDto;
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return res;
            }
            catch (FaultException<WebEas.Services.Esb.Epod.Submission.DcomFault> ex)
            {
                if (ex.Detail.faultMessage.Contains("DMS_NOT_AVAILABLE"))
                {
                    throw new WebEasProxyException("DMS", "Úložisko dokumentov nie je dostupné (DMS)", ex);
                }

                // nechceme skarede hlasky na FE
                if (ex.Detail.faultMessage.Contains("org.apache"))
                {
                    throw new WebEasProxyException(string.Format("Fault Exception in GetSubmissions : {0}", ex.Message), "Nastala chyba pri volaní služby podateľne, zopakujte akciu neskôr prosím.", ex, getSubmissionDto, ex.Detail);
                }

                throw new WebEasProxyException(string.Format("Fault Exception in GetSubmissions : {0}", ex.Message), string.Format("Nastala chyba pri volaní služby podateľne - {0}", ex.Detail.faultMessage), ex, getSubmissionDto, ex.Detail);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetSubmissions", "Nastala chyba pri volaní služby podateľne", ex, getSubmissionDto);
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