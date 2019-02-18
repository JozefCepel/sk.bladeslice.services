using System;
using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebEas.Services.Esb.Test
{
    [TestClass]
    public class DmsDocumentServiceTest : TestBaseProxy
    {
        [TestMethod]
        public void FindAllDocumentsTest()
        {
            using (WebEas.Services.Esb.DmsDocumentProxy proxy = new WebEas.Services.Esb.DmsDocumentProxy(null))
            {
                var result = proxy.FindAllDocuments("87358b5c-bfcb-42b0-8b86-c0d9aba9dc69", "", null, true);
            }
        }

        [TestMethod]
        public void ReadDocumentContentTest()
        {
            using (WebEas.Services.Esb.DmsDocumentProxy proxy = new WebEas.Services.Esb.DmsDocumentProxy(null))
            {
                var result = proxy.ReadDocumentContent("9FF9C399-6FF3-4EFD-891A-7FFAFC5C02B4", "E9A3C76B-8754-4F4F-9A80-A2A8E244E7A2");               
            }
        }

        [TestMethod]
        public void ReadDocumentByIdTest()
        {
            using (WebEas.Services.Esb.DmsDocumentProxy proxy = new WebEas.Services.Esb.DmsDocumentProxy(null))
            {
                var result = proxy.ReadDocumentById("9FF9C399-6FF3-4EFD-891A-7FFAFC5C02B4", "4cbe27e1-9c2c-4f75-bebc-a68fd9c06bca",string.Empty);
            }
        }

        [TestMethod]
        public void ReadDocumentContentFaultTest()
        {
            try
            {
                using (WebEas.Services.Esb.DmsDocumentProxy proxy = new WebEas.Services.Esb.DmsDocumentProxy(null))
                {
                    //var result = proxy.ReadDocumentContent("9FF9C399-6FF3-4EFD-891A-7FFAFC5C02B4", "E9A3C76B-8754-4F4F-9A80-A2A8E244E7A2");
                    var result = proxy.ReadDocumentContent("9FF9C399-6FF3-4EFD-891A-7FFAFC5C02B4", "E9A3C76B-8754-4F4F-9A80-A2A8E244E7A1");
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException is FaultException<WebEas.Services.Esb.Dms.Document.DMSFault>)
                {

                }
                else throw;
            }
        }
    }
}