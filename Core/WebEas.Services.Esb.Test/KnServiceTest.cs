using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebEas.Services.Esb.Kn;

namespace WebEas.Services.Esb.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class KnServiceTest : TestBaseProxy
    {
        /// <summary>
        /// Gets the benefit info test.
        /// </summary>
        [TestMethod]
        public void ZobrazLvTest()
        {
            using (var proxy = new KnProxy(null))
            {
                LVRespType response = proxy.ZobrazLV("1", "1", "1", "1", "1");
                Trace.WriteLine(response.Kod);
            }
        }
    }
}