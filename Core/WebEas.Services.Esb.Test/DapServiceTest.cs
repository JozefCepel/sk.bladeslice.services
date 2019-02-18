using System;
using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebEas.Services.Esb.Test
{
    [TestClass]
    public class DapServiceTest : TestBaseProxy
    {
        [TestMethod]
        public void GeneratePrescriptionTest()
        {
            using (var proxy = new WebEas.Services.Esb.Egov.DapProxy(null))
            {
                proxy.GeneratePrescription(467);
            }
        }

        [TestMethod]
        public void RaiseInternalPaymentTest()
        {
            using (var proxy = new WebEas.Services.Esb.Egov.DapProxy(null))
            {
                proxy.RaiseInternalPayment(new Egov.DapService.InternalPayment());
            }
        }

        [TestMethod]
        public void CancelInternalPaymentTest()
        {
            using (var proxy = new WebEas.Services.Esb.Egov.DapProxy(null))
            {
                proxy.CancelInternalPayment(1);
            }
        }
    }
}
