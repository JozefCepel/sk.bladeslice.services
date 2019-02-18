using NLog;
using System;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Security;

namespace WebEas.Services.Esb
{
    public abstract class ProxyBase
    {
        private const string LoggerName = "ServiceRequestDurationLogger";
        private static readonly Log.WebEasNLogLogger ReqTimeLogger = new Log.WebEasNLogLogger(LoggerName);
        protected string stsThumbPrint;

        public ProxyBase(string stsThumbprint)
        {
            this.stsThumbPrint = stsThumbprint;
        }

        protected void CheckGlobalExceptions(Exception ex)
        {
            if (ex is MessageSecurityException && ex.InnerException != null && ex.InnerException is WebException && ex.InnerException.Message.Contains("403"))
            {
                X509Certificate2 cert = WebEas.Core.Sts.SecurityTokenServiceHelper.GetCertificate(this.stsThumbPrint);
                throw new WebEasProxyException(string.Format("Forbidden action for issuer: {0} subject: {1} - \"{2}\"", cert.Issuer, cert.Subject, cert.Thumbprint),ex);
            }
        }

        protected void LogRequestDuration(string serviceUrl, Stopwatch stopwatch, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            var logEventInfo = new LogEventInfo(LogLevel.Info, LoggerName, null);
            logEventInfo.Properties["ElapsedMilliseconds"] = stopwatch.ElapsedMilliseconds;
            logEventInfo.Properties["ServiceUrl"] = serviceUrl;
            logEventInfo.Properties["Operation"] = memberName;
            ReqTimeLogger.LogInfo(logEventInfo);
        }
    }
}