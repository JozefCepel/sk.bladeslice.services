using System;
using System.Linq;
using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebEas.Services.Esb.Test
{
    [TestClass]
    public class EgovServiceTest : TestBaseProxy
    {
        [TestMethod]
        public void ProcessSubmissionFaultTest()
        {
          //  try
//            {
                using (var client = new WebEas.Services.Esb.Egov.EgovGatewayProxy(null))
                {
                    client.ProcessSubmission(-1, null);
                }
  //          }
            //catch (Exception ex)
            //{
            //    //if (ex.InnerException != null && ex.InnerException is FaultException<WebEas.Services.Esb.Egov.EgovGateway.EgovException>)
            //    //{
            //    //}
            //    //else
            //    //{
            //        throw;
            //   // }
            //}
        }

        [TestMethod]
        public void NotifyRecordRelatedEventTest()
        {
            using (var client = new WebEas.Services.Esb.Egov.EgovGatewayProxy(null))
            {
                client.NotifyRecordRelatedEvent(new Egov.EgovGateway.NotifyRecordRelatedEvent());
            }
        }
    }
}