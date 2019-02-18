using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebEas.Services.Esb.Mdfx;

namespace WebEas.Services.Esb.Test
{
    [TestClass]
    public class FormsServiceTest : TestBaseProxy
    {
        [TestMethod]
        public void FindServiceByFormNamespaceTest()
        {
            using (var client = new WebEas.Services.Esb.FormsProxy(null))
            {
                List<string> data = client.FindServiceByFormNamespace("http://schemas.gov.sk/form/DCOM_eDemokracia_ZiadostOPoskytnutieInformacieFO_sk/1.0");

                foreach (string item in data)
                {
                    Trace.WriteLine(item);
                }
            }
        }

        [TestMethod]
        public void GetClerkFormsTest()
        {
            using (var client = new WebEas.Services.Esb.FormsProxy(null))
            {
                List<formDto_type> data = client.GetClerkForms("Litava", null,null);

                foreach (formDto_type item in data)
                {
                    Trace.WriteLine(item.ToString());
                }
            }
        }

        [TestMethod]
        public void GetFormDetailTest()
        {
            using (var client = new WebEas.Services.Esb.FormsProxy(null))
            {
                formDto_type data = client.GetFormDetail("http://schemas.gov.sk/form/DCOM_eDemokracia_ZiadostOPoskytnutieInformacieFO_sk/1.1");
                
                Trace.WriteLine(data);
            }
        }

        [TestMethod]
        public void GetLatestFormVersionTest()
        {
            using (var client = new WebEas.Services.Esb.FormsProxy(null))
            {
                var data = client.GetLatestFormVersion("http://schemas.gov.sk/form/DCOM_eDemokracia_ZiadostOPoskytnutieInformacieFO_sk/1.0");
                
                Trace.WriteLine(data);
            }
        }

        
    }
}