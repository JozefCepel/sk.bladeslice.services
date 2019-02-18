using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebEas.Services.Esb.Person;

namespace WebEas.Services.Esb.Test
{
    [TestClass]
    public class PersonServiceTest : TestBaseProxy
    {
        /// <summary>
        /// Finds the person test.
        /// </summary>
        [TestMethod]
        public void FindPersonTest()
        {
            using (WebEas.Services.Esb.PersonProxy proxy = new WebEas.Services.Esb.PersonProxy(null))
            {
                var result = proxy.FindPerson(new WebEas.Services.Esb.Person.VyhladavacieKriteria
                {

                    VyhladavacieKriteriaOsoby = new WebEas.Services.Esb.Person.VyhladavacieKriteriaOsoby
                    {
                        VyhladavacieKriteriaFyzickejOsoby = new VyhladavacieKriteriaFyzickejOsoby 
                        { 
                            //RodneCislo = "7010101111",
                            Meno = "Vilma",
                            Priezvisko = "Dodatočná",
                            RodneCislo = null,                            
                            CisloDokladu = null
                        }

                        //Item = new VyhladavacieKriteriaPravnickejOsoby
                        //{
                        //    Ico = "",
                        //    ObchodneMeno = "",
                        //    PravnaFormaId = null
                        //}
                    }
                });                                                            
            }

            //using (WebEas.Services.Esb.PersonProxy proxy = new WebEas.Services.Esb.PersonProxy(null))
            //{
            //    var result = proxy.FindPerson(new WebEas.Services.Esb.Person.VyhladavacieKriteria
            //    {

            //        VyhladavacieKriteriaOsoby = new WebEas.Services.Esb.Person.VyhladavacieKriteriaOsoby
            //        {
            //            VyhladavacieKriteriaFyzickejOsoby = new VyhladavacieKriteriaFyzickejOsoby
            //            {
            //                //RodneCislo = "7010101111",
            //                Meno = "Petra",
            //                Priezvisko = "Odlišná",
            //                RodneCislo = null,                            
            //                CisloDokladu = null
            //            }

            //            //Item = new VyhladavacieKriteriaPravnickejOsoby
            //            //{
            //            //    Ico = "",
            //            //    ObchodneMeno = "",
            //            //    PravnaFormaId = null
            //            //}
            //        }
            //    });
            //}
        }

        /// <summary>
        /// Finds the person test.
        /// </summary>
        [TestMethod]
        public void GetPersonTest()
        {
            using (WebEas.Services.Esb.PersonProxy proxy = new WebEas.Services.Esb.PersonProxy(null))
            {
                var result = proxy.GetPerson(new WebEas.Services.Esb.Person.OsobaRequest()
                {
                    OsobaId = "5282d296-47ab-499e-82fd-b460c0985b11"
                });
            }
        }


        [TestMethod]
        public void GetNaturalPersonSingleTest()
        {
            using (WebEas.Services.Esb.PersonProxy proxy = new WebEas.Services.Esb.PersonProxy(null))
            {
                var result2 = proxy.GetNaturalPerson("2426CFBF-1540-4FAD-A82D-1DF510EAD8E5");
            }
        }

        [TestMethod]
        public void GetLegalPersonSingleTest()
        {
            using (WebEas.Services.Esb.PersonProxy proxy = new WebEas.Services.Esb.PersonProxy(null))
            {
                var result = proxy.GetLegalPerson("830A4118-2E5C-457C-9175-425C2CD21C81");
                
            }
        }

        [TestMethod]
        public void GetNaturalPersonTest()
        {
            using (WebEas.Services.Esb.PersonProxy proxy = new WebEas.Services.Esb.PersonProxy(null))
            {
                var res = proxy.FindPerson(new VyhladavacieKriteria
                {
                    VyhladavacieKriteriaOsoby = new VyhladavacieKriteriaOsoby()
                    {
                        VyhladavacieKriteriaFyzickejOsoby = new VyhladavacieKriteriaFyzickejOsoby()
                    },
                    VyhledatVEvidenciTenanta = true
                });
                Assert.IsNotNull(res.Osoba);
                Assert.IsTrue(res.Osoba.Any());

                foreach (var osoba in res.Osoba)
                {
                    var result = proxy.GetNaturalPerson(osoba.Id);
                }
            }
        }

        [TestMethod]
        public void GetLegalPerson()
        {
            using (WebEas.Services.Esb.PersonProxy proxy = new WebEas.Services.Esb.PersonProxy(null))
            {
                var res = proxy.FindPerson(new VyhladavacieKriteria
                {
                    VyhladavacieKriteriaOsoby = new VyhladavacieKriteriaOsoby()
                    {
                        VyhladavacieKriteriaPravnickejOsoby = new VyhladavacieKriteriaPravnickejOsoby()
                    },
                    VyhledatVEvidenciTenanta = true
                });
                Assert.IsNotNull(res.Osoba);
                Assert.IsTrue(res.Osoba.Any());

                foreach (var osoba in res.Osoba)
                {
                    var result = proxy.GetLegalPerson(osoba.Id);
                }
            }
        }

        [TestMethod]
        public void GetNaturalPersonsTest()
        {
            using (WebEas.Services.Esb.PersonProxy proxy = new WebEas.Services.Esb.PersonProxy(null))
            {
                var res = proxy.FindPerson(new VyhladavacieKriteria
                {
                    VyhladavacieKriteriaOsoby = new VyhladavacieKriteriaOsoby()
                    {
                        VyhladavacieKriteriaFyzickejOsoby = new VyhladavacieKriteriaFyzickejOsoby()
                    },
                    VyhledatVEvidenciTenanta = true
                });
                Assert.IsNotNull(res.Osoba);
                Assert.IsTrue(res.Osoba.Any());

                var result = proxy.GetNaturalPersons(res.Osoba.Select(x => x.Id).ToArray());
            }
        }

        [TestMethod]
        public void GetLegalPersonsTest()
        {
            using (WebEas.Services.Esb.PersonProxy proxy = new WebEas.Services.Esb.PersonProxy(null))
            {
                var res = proxy.FindPerson(new VyhladavacieKriteria
                {
                    VyhladavacieKriteriaOsoby = new VyhladavacieKriteriaOsoby()
                    {
                        VyhladavacieKriteriaPravnickejOsoby = new VyhladavacieKriteriaPravnickejOsoby()
                    },
                    VyhledatVEvidenciTenanta = true
                });
                Assert.IsNotNull(res.Osoba);
                Assert.IsTrue(res.Osoba.Any());

                var result = proxy.GetLegalPersons(res.Osoba.Select(x => x.Id).ToArray());
            }
        }

        [TestMethod]
        public void GetNaturalPersonsTestFromFile()
        {
            using (WebEas.Services.Esb.PersonProxy proxy = new WebEas.Services.Esb.PersonProxy(null))
            {
                //var osoby = File.ReadAllLines(@"c:\tmp\FOTEST.txt");
                //var result = proxy.GetNaturalPersons(osoby.ToArray());
            }
        }

        [TestMethod]
        public void GetLegalPersonsTestFromFile()
        {
            using (WebEas.Services.Esb.PersonProxy proxy = new WebEas.Services.Esb.PersonProxy(null))
            {
                //var osoby = File.ReadAllLines(@"c:\tmp\POTEST.txt");
                //var result = proxy.GetLegalPersons(osoby.ToArray());
            }
        }

        /*
        [TestMethod]
        public void GetNaturalLegalPersonsTestFromFileAsync()
        {
            using (var proxy = WebEas.Core.Sts.SecurityTokenServiceHelper.CreateChannelProxy<Person.PersonServicesChannel>(PersonProxy.ServiceUrl, null))
            {
                var osobyFO = File.ReadAllLines(@"c:\tmp\FOTEST.txt");
                var osobyPO = File.ReadAllLines(@"c:\tmp\POTEST.txt");

                var resFO = new List<Task<getNaturalPersonsResponse>>();
                var resPO = new List<Task<getLegalPersonsResponse>>();

                for (int i = 0; i < 1; i++)
                {
                    resFO.Add(proxy.getNaturalPersonsAsync(new getNaturalPersons { GetNaturalPersonsRequest = osobyFO.Take(100).ToArray() }));
                    resPO.Add(proxy.getLegalPersonsAsync(new getLegalPersons { GetLegalPersonsRequest = osobyPO.Take(100).ToArray() }));
                }

                Task.WaitAll(resFO.ToArray());
                Task.WaitAll(resPO.ToArray());

            }
        }
        */

        /// <summary>
        /// Finds the person test.
        /// </summary>
        [TestMethod]
        public void GetPersonFaultTest()
        {
            try
            {
                using (WebEas.Services.Esb.PersonProxy proxy = new WebEas.Services.Esb.PersonProxy(null))
                {
                    var result = proxy.GetPerson(new WebEas.Services.Esb.Person.OsobaRequest()
                    {
                        OsobaId = "309037583174"
                    });
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException is FaultException<WebEas.Services.Esb.Person.DcomFaultType>)
                {

                }
                else throw;
            }
        }

        /// <summary>
        /// AddPersonBankAccount test
        /// </summary>
        [TestMethod]
        public void AddPersonBankAccountTest()
        {
            long? bankoveSpojenieId = null;
            try
            {
                using (WebEas.Services.Esb.PersonProxy proxy = new WebEas.Services.Esb.PersonProxy(null))
                {
                    bankoveSpojenieId = proxy.AddPersonBankAccount(new BankoveSpojenieNaZapis
                        {
                            PersonId = "582F7010-258C-4E83-94A1-9DFE0D77951A",
                            //TypBankovehoSpojeniaId = 1,
                            //CisloUctu = "2626753484",
                            //KodBanky = 1100,
                            //KodBankySpecified = true,
                            //NazovBanky = "Tatrabanka"

                            //Bic = "SUBASKBX",
                            Bic = "SUBXX",
                            Iban = "SK3302000000000000012351",
                            NazovBanky = "Všeobecná úverová banka",
                            TypBankovehoSpojeniaId = 2
                        });
                }

                //Console.WriteLine("bankoveSpojenieId: " + bankoveSpojenieId);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException is FaultException<WebEas.Services.Esb.Person.DcomFaultType>)
                {

                }
                else throw;
            }
        }   
    }
}
