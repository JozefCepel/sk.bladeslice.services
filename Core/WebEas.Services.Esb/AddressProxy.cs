using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using WebEas.Services.Esb.Address;

namespace WebEas.Services.Esb
{
    public class AddressProxy : ProxyBase, IDisposable
    {
        private const string ServiceUrl = "https://lbsoa.intra.dcom.sk/soa/address/1.0/address";

        private Address.AddressServicesChannel proxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="IamRoleProxy" /> class.
        /// </summary>
        public AddressProxy(string stsThumbprint) : base(stsThumbprint)
        { 
            this.proxy = WebEas.Core.Sts.SecurityTokenServiceHelper.CreateChannelProxy<Address.AddressServicesChannel>(ServiceUrl, stsThumbprint);
        }

        /// <summary>
        /// Adds the address.
        /// </summary>
        /// <param name="adresaKUlozeni">The adresa K ulozeni.</param>
        /// <returns></returns>
        public AddAddressResult AddAddress(WebEas.Services.Esb.Address.AdresaKUlozeni adresaKUlozeni)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref this.proxy, ServiceUrl, this.stsThumbPrint);

                var request = new AddAddress(adresaKUlozeni);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                AddAddressResponse response = this.proxy.AddAddress(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.addAddressResult;
            }
            catch (FaultException<WebEas.Services.Esb.Address.LokalneRegistreFault> ex)
            {
                throw new WebEasValidationException(null, string.Format("Nepodarilo sa získať adresu z externej služby! {0}", ex.Message), ex, adresaKUlozeni);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in AddAddress", "Nepodarilo sa získať adresu z externej služby!", ex, adresaKUlozeni);
            }
        }

        /// <summary>
        /// Gets the address.
        /// </summary>
        /// <param name="getAddressRequest">The get address request.</param>
        /// <returns></returns>
        public WebEas.Services.Esb.Address.Adresa GetAddress(WebEas.Services.Esb.Address.GetAddressRequest getAddressRequest)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref this.proxy, ServiceUrl, this.stsThumbPrint);

                var request = new GetAddress(getAddressRequest);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                GetAddressResponse response = this.proxy.GetAddress(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.Adresa;
            }
            catch (FaultException<WebEas.Services.Esb.Address.LokalneRegistreFault> ex)
            {
                throw new WebEasValidationException(null, string.Format("Nepodarilo sa získať adresu z externej služby! {0}", ex.Message), ex, getAddressRequest);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetAddress", ex, getAddressRequest);
            }
        }

        /// <summary>
        /// List Flats
        /// </summary>
        /// <param name="listFlatsRequest"></param>
        /// <returns>Byt[]</returns>
        public Byt[] ListFlats(ListFlats listFlatsRequest)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref this.proxy, ServiceUrl, this.stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.ListFlats(listFlatsRequest);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.BytList;
            }
            catch (FaultException<WebEas.Services.Esb.Address.LokalneRegistreFault> ex)
            {
                throw new WebEasValidationException(null, string.Format("Nepodarilo sa získať byty z externej služby! {0}", ex.Message), ex, listFlatsRequest);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in ListFlats", ex, listFlatsRequest);
            }
        }

        /// <summary>
        /// List Ra Flats
        /// </summary>
        /// <param name="listFlatsRaRequest"></param>
        /// <returns>Byt[]</returns>
        public Byt[] ListRaFlats(ListRaFlats listRaFlatsRequest)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref this.proxy, ServiceUrl, this.stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.ListRaFlats(listRaFlatsRequest);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.BytList;
            }
            catch (FaultException<WebEas.Services.Esb.Address.LokalneRegistreFault> ex)
            {
                throw new WebEasValidationException(null, string.Format("Nepodarilo sa získať byty z externej služby! {0}", ex.Message), ex, listRaFlatsRequest);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in ListRaFlats", ex, listRaFlatsRequest);
            }
        }

        /// <summary>
        /// Gets the Dorucovacia Posta.
        /// </summary>
        /// <param name="GetDorucovaciaPostaRequest">The get dorucovacia posta request.</param>
        /// <returns></returns>
        public WebEas.Services.Esb.Address.DorucovaciaPostaResult GetDorucovaciaPosta(WebEas.Services.Esb.Address.GetDorucovaciaPosta GetDorucovaciaPostaRequest)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref this.proxy, ServiceUrl, this.stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.GetDorucovaciaPosta(GetDorucovaciaPostaRequest);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.DorucovaciaPostaResult;
            }
            catch (FaultException<WebEas.Services.Esb.Address.LokalneRegistreFault> ex)
            {
                throw new WebEasValidationException(null, string.Format("Nepodarilo sa získať doručovaciu poštu z externej služby! {0}", ex.Message), ex, GetDorucovaciaPostaRequest);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetDorucovaciaPosta", ex, GetDorucovaciaPostaRequest);
            }
        }

        /// <summary>
        /// Searchs the slovak address.
        /// </summary>
        /// <param name="vyhledavaciKriteria">The vyhledavaci kriteria.</param>
        /// <returns></returns>
        public WebEas.Services.Esb.Address.SlovenskaAdresa[] SearchSlovakAddress(WebEas.Services.Esb.Address.VyhledavaciKriteria vyhledavaciKriteria)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref this.proxy, ServiceUrl, this.stsThumbPrint);

                var request = new SearchSlovakAddress(vyhledavaciKriteria);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                SearchSlovakAddressResponse response = this.proxy.SearchSlovakAddress(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.SlovenskaAdresaList;
            }
            catch (FaultException<WebEas.Services.Esb.Address.LokalneRegistreFault> ex)
            {
                throw new WebEasValidationException(null, string.Format("Nepodarilo sa vyhľadať adresu z externej služby! {0}", ex.Message), ex, vyhledavaciKriteria);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in SearchSlovakAddress", ex, vyhledavaciKriteria);
            }
        }

        /// <summary>
        /// Searchs the slovak address object.
        /// </summary>
        /// <param name="vyhledavaciKriteria">The vyhledavaci kriteria.</param>
        /// <param name="typObjektuAdresy">The typ objektu adresy.</param>
        /// <returns></returns>
        public WebEas.Services.Esb.Address.ObjektAdresy[] SearchSlovakAddressObject(WebEas.Services.Esb.Address.VyhledavaciKriteria vyhledavaciKriteria, TypObjektuAdresy typObjektuAdresy)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref this.proxy, ServiceUrl, this.stsThumbPrint);

                var subRequest = new SearchSlovakAddressObjectRequest();
                subRequest.VyhledavaciKriteria = vyhledavaciKriteria;
                subRequest.TypObjektuAdresy = typObjektuAdresy;
                var request = new SearchSlovakAddressObject(subRequest);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                SearchSlovakAddressObjectResponse response = this.proxy.SearchSlovakAddressObject(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.ObjektAdresyList;
            }
            catch (FaultException<WebEas.Services.Esb.Address.LokalneRegistreFault> ex)
            {
                throw new WebEasValidationException(null, string.Format("Nepodarilo sa vyhľadať adresu z externej služby! {0}", ex.Message), ex, vyhledavaciKriteria,typObjektuAdresy);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in SearchSlovakAddressObject", ex, vyhledavaciKriteria, typObjektuAdresy);
            }
        }

        /// <summary>
        /// Get Flat Address
        /// </summary>
        /// <param name="idByt">flatId - Id bytu.</param>
        /// <returns></returns>
        public Byt GetFlatAddress(long idByt)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new GetFlatAddress
                {
                    GetFlatAddressRequest = new GetFlatAddressRequest
                    {
                        flatId = idByt
                    }
                };

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.GetFlatAddress(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.Byt;
            }
            catch (FaultException<LokalneRegistreFault> ex)
            {
                throw new WebEasValidationException(null, string.Format("Nepodarilo sa vyhľadať adresu z externej služby! {0}", ex.Message), ex, idByt);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetFlatAddress", ex, idByt);
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