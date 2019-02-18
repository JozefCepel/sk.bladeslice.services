using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace WebEas.Services.Esb.Test
{
    [TestClass]
    public class IsoGwTest : TestBaseProxy
    {
        [TestMethod]
        public void GetVehicleInfoTest()
        {
            using (var proxy = new WebEas.Services.Esb.IsoGwProxy(null))
            {
                bool result = proxy.HasService("abc", "abc");

                Trace.WriteLine(result);
            }
        }
    }
}
