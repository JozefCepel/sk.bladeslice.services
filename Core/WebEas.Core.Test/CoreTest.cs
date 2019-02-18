using NUnit.Framework;
using ServiceStack.Logging;
using ServiceStack.Redis;
using System;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using WebEas.Core.Base;
using WebEas.Core.Log;
using WebEas.Log;
using WebEas.Services.Esb.Epod.CaseFile;

namespace WebEas.Core.Test
{
    [TestFixture]
    public class CoreTest
    {
        [Test]
        public void ErrorResponseTest()
        {
            var argExc = new ArgumentNullException("Argument null");

            var proxyExc = new WebEasProxyException(null, "Proxy chyba", argExc);

            var ttEx = new TargetInvocationException("invoc", proxyExc);

            var validExc = new WebEasValidationException(null, "Valid chyba", ttEx);

            var exc = new WebEasException(null, "Eas chyba", validExc);

            var response = WebEasErrorHandling.CreateWebEasResponseStatus(exc, null);
            response.StackTrace = exc.ToDescription();

            Assert.AreEqual(response.Caption, WebEasErrorHandling.ErrorProxyCaption);
            Assert.AreEqual(response.Message, exc.MessageUser);
            Assert.AreEqual(response.DetailMessage, string.Format("{0} - {1} - {2} - {3} - {4}", exc.Message, validExc.Message, ttEx.Message, proxyExc.Message, argExc.Message));
        }

        [Test]
        public void ErrorLogTest()
        {
            LogManager.LogFactory = new WebEasNLogFactory();
            WebEasNLogConfig.SetConfig(null, true, false, false);

            var proxy = new FaultException<ERegistryException>(new ERegistryException { message = "Test", ValidationError = new ValidationError[] { new ValidationError { type = ValidationErrorType.CONCURRENT_MODIFICATION } } });

            var eas = new WebEasProxyException("BLA BLA", proxy, new { test = "ABC", test2 = new { ab = 1 } });

            string data = eas.ToDescription();

            LogManager.GetLogger("typ").Error(eas);

            Assert.True(data.Contains("CONCURRENT_MODIFICATION"));
        }

        [Test]
        public void RedisClientTest()
        {
            var redisClient = new RedisClient();
            var redisKeys = redisClient.ScanAllKeys($"LongOperation:dap:*");
            var longOperations = redisClient.GetAll<LongOperationStatus>(redisKeys);

            var running = longOperations.Where(x => x.Value.State == LongOperationState.Running || x.Value.State == LongOperationState.Waiting);
            Assert.Pass(running.Select(x => x.Key + " - "  + x.Value.Message  + ": " + System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(x.Value.OperationInfo)) + Environment.NewLine).ToJsonString());
        }

        //[Test]
        //public void ErrorLogTest()
        //{
        //    WebEas.Core.Log.WebEasNLogLogger.Application = "Unit";
        //    WebEas.Core.Log.WebEasNLogConfig.SetConfig(@"server=LBDBCL12.INTRA.DCOM.SK,62905;database=Egov;uid=Egov_main;password=utTRe#r132;Max Pool Size=1000;Connect Timeout=120");
        //    LogManager.LogFactory = new WebEas.Core.Log.WebEasNLogFactory();
        //    ILog log = LogManager.GetLogger(typeof(CoreTest));
        //    log.Error("Log test");
        //}
    }
}
