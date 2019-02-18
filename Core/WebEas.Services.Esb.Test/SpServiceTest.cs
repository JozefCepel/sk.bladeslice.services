using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebEas.Services.Esb.Sp;

namespace WebEas.Services.Esb.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class SpServiceTest : TestBaseProxy
    {
        /// <summary>
        /// Gets the benefit info test.
        /// </summary>
        [TestMethod]
        public void GetBenefitInfoTest()
        {
            using (var proxy = new SpProxy(null))
            {
                GetBenefitInfoResponse response = proxy.GetBenefitInfo(new DateTime(2014, 6, 6), null, "9d9f9cc4-2675-4d20-87e3-fb43d55bd176", "5200", new List<InsuranceTypeRequiredType> { new InsuranceTypeRequiredType { InsuranceType = "NP" } });
                Trace.WriteLine(response.RecordID);
            }
        }

        /// <summary>
        /// Checks the employment status test.
        /// </summary>
        [TestMethod]
        public void CheckEmploymentStatusTest()
        {
            using (var proxy = new SpProxy(null))
            {
                CheckEmploymentStatusResponse response = proxy.CheckEmploymentStatus(new DateTime(2015,1,1), "04906c71-c856-42f2-8b92-ab1b7ef586ea", "5200", "31365078");
                Trace.WriteLine(response.RecordID);
            }
        }
    }
}