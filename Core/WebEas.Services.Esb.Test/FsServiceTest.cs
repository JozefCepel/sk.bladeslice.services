using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebEas.Services.Esb.Fs;

namespace WebEas.Services.Esb.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class FsServiceTest : TestBaseProxy
    {
        /// <summary>
        /// SendGetDutyArrearsInfoRequest test.
        /// </summary>
        [TestMethod]
        public void SendGetDutyArrearsInfoRequestTest()
        {
            try
            {
                using (var proxy = new FsProxy(null))
                {
                    var response = proxy.SendGetDutyArrearsInfoRequest("9d9f9cc4-2675-4d20-87e3-fb43d55bd176", "5200");
                    Trace.WriteLine(response.DossierID);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// GetDutyArrearsInfo test.
        /// </summary>
        [TestMethod]
        public void GetDutyArrearsInfoTest()
        {
            try
            {
                using (var proxy = new FsProxy(null))
                {
                    var response = proxy.GetDutyArrearsInfo("04906c71-c856-42f2-8b92-ab1b7ef586ea", "5200");
                    Trace.WriteLine(response.RecordID);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}