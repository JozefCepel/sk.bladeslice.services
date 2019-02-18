using System;
using System.Diagnostics;
using System.ServiceModel;
using WebEas.Services.Esb.Kn;

namespace WebEas.Services.Esb
{
    /// <summary>
    /// Kataster nehnutelnosti
    /// </summary>
    public class KnProxy : ProxyBase, IDisposable
    {
        private const string ServiceUrl = "https://lbsoa.intra.dcom.sk/soa/kn/1.0/knservice";

        private KNGWPortTypeChannel proxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpProxy" /> class.
        /// </summary>
        /// <param name="stsThumbprint">The STS thumbprint.</param>
        public KnProxy(string stsThumbprint)
            : base(stsThumbprint)
        {
            proxy = Core.Sts.SecurityTokenServiceHelper.CreateChannelProxy<KNGWPortTypeChannel>(ServiceUrl, stsThumbprint);
        }

        /// <summary>
        /// Zobrazs the LV.
        /// </summary>
        /// <param name="cisloLV">The byt.</param>
        /// <param name="kuCode">The ku code.</param>
        /// <param name="parcelaCNumber">The parcela C number.</param>
        /// <param name="parcelaENumber">The parcela E number.</param>
        /// <param name="supisneCislo">supisne cislo.</param>
        /// <returns></returns>
        public LVRespType ZobrazLV(string cisloLV, string kuCode, string parcelaCNumber, string parcelaENumber, string supisneCislo)
        {
            try
            {
                Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new zobrazLV { lvReq = new LVReqType { cisloLV = cisloLV, ku_code = kuCode, cisloParcelyC = parcelaCNumber, cisloParcelyE = parcelaENumber, supisneCislo = supisneCislo } };
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.zobrazLV(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.lvResp;
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in ZobrazLV", ex, cisloLV, kuCode, parcelaCNumber, parcelaENumber, supisneCislo);
            }
        }

        /// <summary>
        /// Stiahni LV.
        /// </summary>
        /// <param name="cisloLV">The byt.</param>
        /// <param name="kuCode">The ku code.</param>
        /// <param name="parcelaCNumber">The parcela C number.</param>
        /// <param name="parcelaENumber">The parcela E number.</param>
        /// <param name="supisneCislo">supisne cislo.</param>
        /// <param name="cisloSpisu">cislo spisu.</param>
        /// <returns></returns>
        public StiahniLVRespType StiahniLV(string cisloLV, string kuCode, string parcelaCNumber, string parcelaENumber, string supisneCislo, long cisloSpisu)
        {
            try
            {
                Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new stiahniLV { stiahniLvReq = new StiahniLVReqType { cisloLV = cisloLV, ku_code = kuCode, cisloParcelyC = parcelaCNumber, cisloParcelyE = parcelaENumber, supisneCislo = supisneCislo, cisloSpisu = cisloSpisu } };
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.stiahniLV(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.stiahniLvResp;
            }
            catch (FaultException ex)
            {
                throw new WebEasValidationException(ex.Message, ex.Reason.ToString(), ex, cisloLV, kuCode, parcelaCNumber, parcelaENumber, supisneCislo, cisloSpisu);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in StiahniLV", ex, cisloLV, kuCode, parcelaCNumber, parcelaENumber, supisneCislo, cisloSpisu);
            }
        }

        /// <summary>
        /// Manualnes the sparovanie osob.
        /// </summary>
        /// <param name="sparoval">The sparoval.</param>
        /// <param name="sparovaneOsobyList">The sparovane osoby list.</param>
        /// <returns></returns>
        public SparovaneOsobyRespType ManualneSparovanieOsob(string sparoval, ParReqType[] sparovaneOsobyList)
        {
            try
            {
                Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new manualneSparovanieOsob { manualneSparovaneOsobyReq = new SparovaneOsobyReqType { sparoval = sparoval, sparovaneOsobyList = sparovaneOsobyList } };
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.manualneSparovanieOsob(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.manualneSparovaneOsobyResp;
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in ManualneSparovanieOsob", ex, sparoval, sparovaneOsobyList);
            }
        }

        /// <summary>
        /// Updatovacies the parovacie procesy.
        /// </summary>
        /// <param name="listObci">The list obci.</param>
        public void UpdatovacieParovacieProcesy(ObceUpdateType[] listObci)
        {
            try
            {
                Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new updatovacieParovacieProcesy { updateParovanieReq = new UpdatovacieParovacieProcesyType { listObci = listObci } };
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                proxy.updatovacieParovacieProcesy(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in UpdatovacieParovacieProcesy", ex, listObci);
            }
        }

        /// <summary>
        /// Imports the obce AKU.
        /// </summary>
        /// <param name="obecFull">The obec full.</param>
        /// <returns></returns>
        public ResponseType ImportObceAKU(ObecType obecFull)
        {
            try
            {
                Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new importObceAKU { importObceAKUReq = new ImportObceAKUType { obecFull = obecFull } };
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.importObceAKU(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.importObceAKUResp;
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in ImportObceAKU", ex, obecFull);
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
                if (proxy != null && proxy is IDisposable)
                {
                    if (proxy.State == System.ServiceModel.CommunicationState.Faulted)
                    {
                        proxy.Abort();
                    }

                    proxy.Dispose();
                }
                proxy = null;
            }
            catch (Exception ex)
            {
                throw new WebEasException("Error in dispose", ex);
            }
        }
    }
}