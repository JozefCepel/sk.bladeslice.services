using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebEas.Services.Esb.Test
{
    [TestClass]
    public class PlaServiceTest : TestBaseProxy
    {
        [TestMethod]
        public void SendPrescriptionTest()
        {
            using (var proxy = new WebEas.Services.Esb.Egov.PlaProxy(null))
            {
                proxy.SendPrescription(new Egov.PlaService.PrescriptionFromDap { D_Vymer_Id = 467 });
            }
        }
    }
}
