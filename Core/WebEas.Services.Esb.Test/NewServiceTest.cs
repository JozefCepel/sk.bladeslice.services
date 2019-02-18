using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebEas.Services.Esb.Nev;

namespace WebEas.Services.Esb.Test
{
    [TestClass]
    public class NewServiceTest : TestBaseProxy
    {
        [TestMethod]
        public void GetVehicleInfoTest()
        {
            using (var proxy = new WebEas.Services.Esb.NevProxy(null))
            {
              //  VehicleInfo result = proxy.GetVehicleInfo("F3C79D2D-552D-47B5-A50E-EFF1002C2A61", "BA753FN", true, true);

               // Trace.WriteLine(result.RecordID);
            }
        }
    }
}