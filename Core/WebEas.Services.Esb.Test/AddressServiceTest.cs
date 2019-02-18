using System;
using System.Diagnostics;
using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.Logging;
using WebEas.Services.Esb.Address;

namespace WebEas.Services.Esb.Test
{
    [TestClass]
    public class AddressServiceTest : TestBaseProxy
    {
        [TestMethod]
        public void GetAddressTest()
        {
            using (var proxy = new WebEas.Services.Esb.AddressProxy(null))
            {
                Adresa result = proxy.GetAddress(new WebEas.Services.Esb.Address.GetAddressRequest
                {
                    addressId = 1
                });

                Debugger.Break();
            }
        }

        [TestMethod]
        public void AddAddressTest()
        {
            try
            {
                using (var proxy = new WebEas.Services.Esb.AddressProxy(null))
                {
                    LogManager.GetLogger("Test").Info("Test");
                    var result = proxy.AddAddress(new AdresaKUlozeni
                    {
                        StatId = 703,
                        Kraj = "Košický",
                        Obec = "Gyňov",
                        Okres = "Košice-okolie",
                        OrientacneCislo = "7",
                        PSC = "044 14",
                        SupisneCislo = "130",
                        Ulica = "Nová",
                        NestrukturovaneUdaje = " "
                    });

                    Console.WriteLine(result);
                    Debugger.Break();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(WebEas.WebEasExceptionExtensions.ToDescription(ex));
                throw;
            }
        }

        [TestMethod]
        public void SearchSlovakAddressTest()
        {
            using (var proxy = new WebEas.Services.Esb.AddressProxy(null))
            {
                SlovenskaAdresa[] result = proxy.SearchSlovakAddress(new WebEas.Services.Esb.Address.VyhledavaciKriteria
                {
                    Kraj = "Košický",
                    Obec = "Gyňov",
                    Okres = "Košice-okolie",
                    OrientacneCislo = "7",
                    PSC = "044 14",
                    SupisneCislo = "130",
                    Ulica = "Nová"

                    //OrientacneCislo = "6",
                    ////SupisneCislo = "130",
                    ////Ulica = "Nová",
                    //Ulica = "Nešporova",
                    ////Obec = "Gyňov",
                    //Obec = "Košice",
                    ////Kraj = "Košický"
                });

                result.ToString();

                Debugger.Break();
            }
        }

        [TestMethod]
        public void SearchSlovakAddressFaultTest()
        {
            try
            {
                using (var proxy = new WebEas.Services.Esb.AddressProxy(null))
                {
                    Adresa result = proxy.GetAddress(new GetAddressRequest { });
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException is FaultException<WebEas.Services.Esb.Address.LokalneRegistreFault>)
                {
                }
                else
                {
                    throw;
                }
            }
        }

        [TestMethod]
        public void SearchSlovakAddressObject_OkresyPodlaNazvu_VratiPozadovaneData()
        {
            using (var proxy = new WebEas.Services.Esb.AddressProxy(null))
            {
                ObjektAdresy[] result = proxy.SearchSlovakAddressObject(new VyhledavaciKriteria
                    {
                        //OrientacneCislo = "7",
                        //Ulica = "Nová",
                        //Obec = "Gyňov",
                        //Kraj = "Banskobystrický"
                        KrajId = 7,
                        //Obec = "Haniska"
                    },
                TypObjektuAdresy.OBEC);
            }
        }
    }
}