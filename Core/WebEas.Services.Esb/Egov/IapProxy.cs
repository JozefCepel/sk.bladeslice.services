using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;

namespace WebEas.Services.Esb.Egov
{
    public class IapProxy : ProxyBase, IDisposable
    {
        private const string ServiceUrl = "https://lbsoa.intra.dcom.sk/soa/egov/iap/1.0/iap";
        
        private Egov.IapService.IIapServiceChannel proxy;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="IapProxy" /> class.
        /// </summary>
        public IapProxy(string stsThumbprint) : base(stsThumbprint)
        {
            this.proxy = WebEas.Core.Sts.SecurityTokenServiceHelper.CreateChannelProxy<Egov.IapService.IIapServiceChannel>(ServiceUrl, stsThumbprint, true);
        }
        
        /// <summary>
        /// Creates the automatich publish.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="potvrdenie">The potvrdenie.</param>
        public void CreateAutomatichPublish(IapService.Zasobnik buffer, IapService.PotvrdeniePublikovania potvrdenie)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                this.proxy.CreateAutomatichPublish(buffer, potvrdenie);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
            }
            catch (FaultException<Egov.IapService.dcomFault> ex)
            {
                throw new WebEasProxyException(string.Format("Error in Create Automatic Publish {0} {1}", ex.Detail.faultCause, ex.Detail.faultCode), ex.Detail.faultMessage, ex, buffer,potvrdenie);                            
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in Create Automatic Publish", ex, buffer,potvrdenie);
            }
        }

        /// <summary>
        /// Creates the manual publish.
        /// </summary>
        /// <param name="insertData">The insert data.</param>
        /// <param name="updateData">The update data.</param>
        public void CreateManualPublish(List<IapService.Zasobnik> insertData, List<IapService.Zasobnik> updateData)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                this.proxy.CreateManualtPublish(insertData, updateData);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
            }
            catch (FaultException<Egov.IapService.dcomFault> ex)
            {
                throw new WebEasProxyException(string.Format("Error in Create Automatic Publish {0} {1}", ex.Detail.faultCause, ex.Detail.faultCode), ex.Detail.faultMessage, ex, insertData, updateData);                                            
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in Create Automatic Publish", ex, insertData,updateData);
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