using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;

namespace WebEas.Services.Esb
{
    /// <summary>
    /// Iso Gateway
    /// </summary>
    public class IsoGwProxy : ProxyBase, IDisposable
    {
        private const string ServiceUrl = "https://lbsoa.intra.dcom.sk/soa/isogw/riso/2.0/riso";

        private IsoGw.registerISOChannel proxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="IsoGwProxy" /> class.
        /// </summary>
        /// <param name="stsThumbprint">The STS thumbprint.</param>
        public IsoGwProxy(string stsThumbprint)
            : base(stsThumbprint)
        {
            this.proxy = WebEas.Core.Sts.SecurityTokenServiceHelper.CreateChannelProxy<IsoGw.registerISOChannel>(ServiceUrl, stsThumbprint);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IsoGwProxy" /> class.
        /// </summary>
        public IsoGwProxy(string stsThumbprint, IWebEasSession session)
            : base(stsThumbprint)
        {
            this.proxy = WebEas.Core.Sts.SecurityTokenServiceHelper.CreateChannelProxy<IsoGw.registerISOChannel>(session, ServiceUrl, stsThumbprint);
        }

        /// <summary>
        /// Determines whether the specified municipal office id has service.
        /// </summary>
        /// <param name="municipalOfficeId">The municipal office id.</param>
        /// <param name="serviceNamespace">The service namespace.</param>
        /// <returns></returns>
        public bool HasService(string municipalOfficeId, string serviceNamespace)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref this.proxy, ServiceUrl, this.stsThumbPrint);

                var request = new IsoGw.hasServiceReq { ReqHasService = new IsoGw.ReqHasService { municipalOfficeId = municipalOfficeId, serviceNamespace = serviceNamespace } };
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                IsoGw.hasServiceRes response = this.proxy.hasService(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.ResHasService.result;
            }
            catch (FaultException<WebEas.Services.Esb.IsoGw.RegisterISOFault> ex)
            {
                string message = "DcomFault - HasService";
                if (ex.Detail != null)
                {
                    message += string.Format(" {0}({1})", ex.Detail.errorMessage, ex.Detail.errorCode);
                }

                throw new WebEasProxyException(message, string.Format("Nastala chyba pri volaní služby iso - {0}", ex.Detail.errorMessage), ex, municipalOfficeId, serviceNamespace);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in HasService", ex, municipalOfficeId, serviceNamespace);
            }
        }

        /// <summary>
        /// Gets the info.
        /// </summary>
        /// <param name="municipalOfficeId">The municipal office id.</param>
        /// <param name="serviceNamespace">The service namespace.</param>
        public IsoGw.ServiceDetail[] GetInfo(string municipalOfficeId, string serviceNamespace)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref this.proxy, ServiceUrl, this.stsThumbPrint);

                var request = new IsoGw.getInfoReq { ReqGetInfo = new IsoGw.ReqGetInfo { municipalOfficeId = municipalOfficeId, serviceNamespace = serviceNamespace } };
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                IsoGw.getInfoRes response = this.proxy.getInfo(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.ResGetInfo;
            }
            catch (FaultException<WebEas.Services.Esb.IsoGw.RegisterISOFault> ex)
            {
                string message = "DcomFault - GetInfo";
                if (ex.Detail != null)
                {
                    message += string.Format(" {0}({1})", ex.Detail.errorMessage, ex.Detail.errorCode);
                }

                throw new WebEasProxyException(message, string.Format("Nastala chyba pri volaní služby iso - {0}", ex.Detail.errorMessage), ex, municipalOfficeId, serviceNamespace);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetInfo", ex, municipalOfficeId, serviceNamespace);
            }
        }

        /// <summary>
        /// Gets the config.
        /// </summary>
        /// <param name="municipalOfficeId">The municipal office id.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public IsoGw.ResGetConfig GetConfig(string municipalOfficeId, IsoGw.ConfigParameter[] parameters)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref this.proxy, ServiceUrl, this.stsThumbPrint);

                var request = new IsoGw.getConfigReq { ReqGetConfig = new IsoGw.ReqGetConfig { municipalOfficeId = municipalOfficeId, configParameter = parameters } };
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                IsoGw.getConfigRes response = this.proxy.getConfig(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.ResGetConfig;
            }
            catch (FaultException<WebEas.Services.Esb.IsoGw.RegisterISOFault> ex)
            {
                string message = "DcomFault - GetConfig";
                if (ex.Detail != null)
                {
                    message += string.Format(" {0}({1})", ex.Detail.errorMessage, ex.Detail.errorCode);
                }

                throw new WebEasProxyException(message, string.Format("Nastala chyba pri volaní služby iso - {0}", ex.Detail.errorMessage), ex, municipalOfficeId, parameters);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetConfig", ex, municipalOfficeId, parameters);
            }
        }

        /// <summary>
        /// Gets the type of the operation.
        /// </summary>
        /// <param name="municipalOfficeId">The municipal office id.</param>
        /// <returns></returns>
        public IsoGw.OperationType GetOperationType(string municipalOfficeId)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref this.proxy, ServiceUrl, this.stsThumbPrint);

                var request = new IsoGw.getOperationTypeReq { ReqGetOperationType = new IsoGw.ReqGetOperationType { municipalOfficeId = municipalOfficeId } };
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                IsoGw.getOperationTypeRes response = this.proxy.getOperationType(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.ResGetOperationType.operationType;
            }
            catch (FaultException<WebEas.Services.Esb.IsoGw.RegisterISOFault> ex)
            {
                string message = "DcomFault - GetOperationType";
                if (ex.Detail != null)
                {
                    message += string.Format(" {0}({1})", ex.Detail.errorMessage, ex.Detail.errorCode);
                }

                throw new WebEasProxyException(message, string.Format("Nastala chyba pri volaní služby iso - {0}", ex.Detail.errorMessage), ex, municipalOfficeId);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetOperationType", ex, municipalOfficeId);
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

                    ((IDisposable) this.proxy).Dispose();
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