using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebEas.Services.Esb.Rsd;

namespace WebEas.Services.Esb.Test
{
    [TestClass]
    public class RsdServiceTest : TestBaseProxy
    {
        /// <summary>
        /// Gets the benefit info test.
        /// </summary>
        [TestMethod]
        public void GetBenefitInfoTest()
        {
            using (var proxy = new RsdProxy(null))
            {                 
                ApplicantResponse response = proxy.GetFamilyRelatedBenefitsForApplicant(new ApplicantRequest { BenefitType = new BenefitTypeType { Code = "DZ-25", Name = "n" }, ClaimFrom = new DateTime(2014,6,6), DcomID = "9d9f9cc4-2675-4d20-87e3-fb43d55bd176", DossierID = "5200" });
                Trace.WriteLine(response.RecordID);
            }
        }
    }
}