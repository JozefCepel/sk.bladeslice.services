using System;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Web.Configuration;
using ServiceStack.Logging;
using WebEas.Log;
using WebEas.Sts;

namespace WebEas.Core.Sts
{
    public static class SecurityTokenServiceHelper
    {
        //      private const string SpEndpoint = "https://lbsoa.intra.dcom.sk/soa/trust";
        //        private const string IdpEndpoint = "https://lbsts.intra.dcom.sk:49443/adfs/services/trust/13/certificatetransport";
        //  private const string CertThumbprint = "5c fd 1f 94 c0 97 d5 17 20 5b 32 00 d3 a5 c8 0a 95 5e 87 10";
        //private const string CertThumbprint = "‎6c 7b 59 63 0e 8f 1a 9c fb e8 ba 71 f8 f6 fb 88 87 a4 05 e7";
        /// <summary>
        /// Gets the cert thumbprint.
        /// </summary>
        /// <value>The cert thumbprint.</value>
        private static string CertThumbprint
        {
            get
            {
                if (string.IsNullOrEmpty(WebConfigurationManager.AppSettings["StsThumbprint"]))
                {
                    throw new WebEasException("StsThumbprint (can be a cert. name) is not defined in appsettings of the web.config");
                }
                return WebConfigurationManager.AppSettings["StsThumbprint"];
            }
        }

        /// <summary>
        /// Gets the sp endpoint.
        /// </summary>
        /// <value>The sp endpoint.</value>
        private static string SpEndpoint
        {
            get
            {
                if (String.IsNullOrEmpty(WebConfigurationManager.AppSettings["StsSpEndpoint"]))
                {
                    throw new WebEasException("StsSpEndpoint is not defined in appsettings of the web.config");
                }
                return WebConfigurationManager.AppSettings["StsSpEndpoint"];
            }
        }

        /// <summary>
        /// Gets the idp endpoint.
        /// </summary>
        /// <value>The idp endpoint.</value>
        private static string IdpEndpoint
        {
            get
            {
                if (String.IsNullOrEmpty(WebConfigurationManager.AppSettings["StsIdpEndpoint"]))
                {
                    throw new WebEasException("StsIdpEndpoint is not defined in appsettings of the web.config");
                }
                return WebConfigurationManager.AppSettings["StsIdpEndpoint"];
            }
        }

        /// <summary>
        /// Creates the channel proxy.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="serviceUrl">The service URL.</param>
        /// <param name="certThumbprint">The cert thumbprint.</param>
        /// <returns></returns>
        public static T CreateChannelProxy<T>(IWebEasSession session, string serviceUrl, string certThumbprint = null, bool transactionflow = false) where T : System.ServiceModel.IClientChannel
        {
            return CreateChannelProxy<T>(serviceUrl, session.IamDcomToken, session.TenantId, certThumbprint, transactionflow);
        }

        /// <summary>
        /// Creates the channel proxy.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="serviceUrl">The service URL.</param>
        /// <param name="certThumbprint">The cert thumbprint.</param>
        /// <returns></returns>
        public static T CreateChannelProxy<T>(string serviceUrl, string certThumbprint = null, bool transactionflow = false) where T : System.ServiceModel.IClientChannel
        {
#pragma warning disable 0618
            return CreateChannelProxy<T>(WebEas.Context.Current.Session, serviceUrl, certThumbprint, transactionflow);
#pragma warning restore 0618
        }

        /// <summary>
        /// Creates the channel proxy.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="serviceUrl">The service URL.</param>
        /// <param name="openAMSessionId">The open AM session id.</param>
        /// <param name="tenantId">The tenant id.</param>
        /// <param name="certThumbprint">The cert thumbprint.</param>
        /// <returns></returns>
        public static T CreateChannelProxy<T>(string serviceUrl, string openAMSessionId, string tenantId, string certThumbprint = null, bool transactionflow = false) where T : System.ServiceModel.IClientChannel
        {
            try
            {
                if (string.IsNullOrEmpty(certThumbprint))
                {
                    certThumbprint = CertThumbprint;
                }

                SecurityToken token = GetStsToken(openAMSessionId, tenantId, certThumbprint);

                var binding = new WS2007FederationHttpBinding(
                    WSFederationHttpSecurityMode.TransportWithMessageCredential);

                binding.TransactionFlow = transactionflow;

                binding.Security.Message.EstablishSecurityContext = false;

                binding.MaxBufferPoolSize = Int32.MaxValue;
                binding.MaxReceivedMessageSize = Int32.MaxValue;
                binding.ReaderQuotas.MaxArrayLength = Int32.MaxValue;
                binding.ReaderQuotas.MaxBytesPerRead = Int32.MaxValue;
                binding.ReaderQuotas.MaxDepth = Int32.MaxValue;
                binding.ReaderQuotas.MaxNameTableCharCount = Int32.MaxValue;
                binding.ReaderQuotas.MaxStringContentLength = Int32.MaxValue;

                binding.ReceiveTimeout = new TimeSpan(0, 6, 0);
                binding.OpenTimeout = new TimeSpan(0, 3, 0);
                binding.SendTimeout = new TimeSpan(0, 5, 0);
                binding.CloseTimeout = new TimeSpan(0, 4, 0);

                // set up channel factory
                var factory = new ChannelFactory<T>(
                    binding,
                    new EndpointAddress(serviceUrl));

                factory.Endpoint.EndpointBehaviors.Add(new WebEasLogMessageInspector(LogType.Info));
                factory.Credentials.SupportInteractive = false;
                factory.Credentials.UseIdentityConfiguration = true;

                // create channel with specified token
                return factory.CreateChannelWithIssuedToken(token);
            }
            catch (Exception ex)
            {
                throw new WebEasException(String.Format("Creating secure channel to service {0} failed! -{1}", typeof(T).Name, ex.Message), ex);
            }
        }

        /// <summary>
        /// Try to open the specified service and try to recover it if required.
        /// </summary>
        /// <param name="service">The service to open.</param>
        /// <param name="throwOnError">Indicates if an exception must be thwown on error.</param>
        /// <returns><c>true</c> if <paramref name="service"/> could be opened; <c>false</c> if <paramref name="service"/> has been set to <c>null</c> and needs to be re-created.</returns>
        public static bool CheckService<T>(ref T proxy, string serviceUrl, string certThumbprint = null, bool transactionflow = false, bool throwOnError = true) where T : class, System.ServiceModel.IClientChannel
        {
            // Do nothing the service is not yet created.
            if (proxy == null)
            {
                return false;
            }

            try
            {
                switch (proxy.State)
                {
                    case CommunicationState.Faulted:
                        proxy.Abort();
                        proxy = CreateChannelProxy<T>(serviceUrl, certThumbprint, transactionflow);
                        proxy.Open();
                        break;
                    case CommunicationState.Opened:
                        //No need to close an open service ( CAL )
                        //proxy.Close();
                        //proxy.Open();
                        break;
                    case CommunicationState.Closed:
                        proxy = CreateChannelProxy<T>(serviceUrl, certThumbprint, transactionflow);
                        proxy.Open();
                        break;
                    case CommunicationState.Created:
                        proxy.Open();
                        break;
                }
            }
            catch
            {
                if (proxy != null && proxy.State == CommunicationState.Faulted)
                {
                    proxy.Abort();
                }
                proxy = null;
                if (throwOnError)
                {
                    throw;
                }
            }
            return proxy != null;
        }

        /// <summary>
        /// Creates the channel proxy debug localhost.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="serviceUrl">The service URL.</param>
        /// <param name="transactionflow">The transactionflow.</param>
        /// <returns></returns>
        public static T CreateChannelProxyDebugLocalhost<T>(string serviceUrl, bool transactionflow = false)
        {
            try
            {
                var transportElement = new HttpTransportBindingElement();

                transportElement.AuthenticationScheme = AuthenticationSchemes.Anonymous;
                transportElement.KeepAliveEnabled = false;

                var messegeElement = new TextMessageEncodingBindingElement
                {
                    MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None),
                    ReaderQuotas =
                    {
                        MaxArrayLength = Int32.MaxValue,
                        MaxBytesPerRead = Int32.MaxValue,
                        MaxDepth = Int32.MaxValue,
                        MaxNameTableCharCount = Int32.MaxValue,
                        MaxStringContentLength = Int32.MaxValue
                    }
                };

                var binding = new CustomBinding(messegeElement, transportElement);
                if (transactionflow)
                {
                    var transaction = new TransactionFlowBindingElement();
                    binding.Elements.Add(transaction);
                }
                binding.ReceiveTimeout = new TimeSpan(0, 6, 0);
                binding.OpenTimeout = new TimeSpan(0, 3, 0);
                binding.SendTimeout = new TimeSpan(0, 5, 0);
                binding.CloseTimeout = new TimeSpan(0, 4, 0);

                var factory = new ChannelFactory<T>(
                    binding,
                    new EndpointAddress(serviceUrl));

                factory.Endpoint.EndpointBehaviors.Add(new WebEasLogMessageInspector(LogType.Info));
                factory.Credentials.SupportInteractive = false;
                factory.Credentials.UseIdentityConfiguration = true;

                return factory.CreateChannel();
            }
            catch (Exception ex)
            {
                throw new WebEasException(string.Format("Problem pri vytvoreni debug proxy pre {0}", serviceUrl), ex);
            }
        }

        /// <summary>
        /// Gets the certificate.
        /// </summary>
        /// <param name="thumbprint">The thumbprint.</param>
        /// <returns></returns>
        public static X509Certificate2 GetCertificate(string thumbprint)
        {
            if (string.IsNullOrEmpty(thumbprint))
            {
                thumbprint = CertThumbprint;
            }

            var localMachineCert = new X509Store(StoreLocation.LocalMachine);
            localMachineCert.Open(OpenFlags.ReadOnly);

            X509CertificateCollection authCerts = localMachineCert.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, true);
            if (authCerts.Count == 0 && !string.IsNullOrEmpty(thumbprint))
            {
                foreach (var cert in localMachineCert.Certificates)
                {
                    if (cert.Subject.ToLower().Contains(thumbprint.ToLower() + ","))
                    {
                        // cert.Verify() -- ak bude treba zapracovat aj validaciu certu, zatial budeme brat iba podla platnosti
                        if (DateTime.Now > cert.NotBefore && DateTime.Now < cert.NotAfter)
                        {
                            authCerts.Add(cert);
                            break;
                        }
                    }
                }
            }
            localMachineCert.Close();
            if (authCerts.Count != 1)
            {
                throw new WebEasProxyException(string.Format("Certificate with thumbprint '{0}' is not installed on Local Machine certification store!", thumbprint));
            }

            return new X509Certificate2(authCerts[0]);
        }

        /// <summary>
        /// Gets the STS token.
        /// </summary>
        /// <returns></returns>
        private static SecurityToken GetStsToken(string openAMSessionId, string tenantId, string certThumbprint = null)
        {
            SecurityToken token = null;
            string valueToken = valueToken = string.IsNullOrEmpty(openAMSessionId) ? string.IsNullOrEmpty(tenantId) ? "anonymous" : string.Format("tenant:{0}$", tenantId) : string.Format("tokenid:{0}$", openAMSessionId);
            X509Certificate2 authCert = null;
            var displayClaims = new DisplayClaimCollection();

            string key = string.Format("sts:{0}:{1}:{2}", certThumbprint, tenantId, valueToken).Replace(" ", "").Replace(":", "").Replace("$", "").Replace("*", "").Replace(".", "");

            try
            {
                SecurityTokenCache cache = WebEas.Context.Current.LocalMachineCache.Get<WebEas.Sts.SecurityTokenCache>(key);

                if (cache == null || !cache.IsValidToken())
                {
                    var localMachineCert = new X509Store(StoreLocation.LocalMachine);
                    localMachineCert.Open(OpenFlags.ReadOnly);

                    X509CertificateCollection authCerts = localMachineCert.Certificates.Find(X509FindType.FindByThumbprint, certThumbprint, true);
                    if (authCerts.Count == 0 && !string.IsNullOrEmpty(certThumbprint))
                    {
                        foreach (var cert in localMachineCert.Certificates)
                        {
                            if (cert.Subject.ToLower().Contains(certThumbprint.ToLower()))
                            {
                                if (DateTime.Now > cert.NotBefore && DateTime.Now < cert.NotAfter)
                                {
                                    authCerts.Add(cert);
                                    break;
                                }
                            }
                        }
                    }
                    localMachineCert.Close();
                    if (authCerts.Count != 1)
                    {
                        throw new WebEasProxyException(string.Format("Certificate with thumbprint '{0}' is not installed on Local Machine certification store!", certThumbprint));
                    }

                    authCert = new X509Certificate2(authCerts[0]);

                    // create STSclient
                    // custom code in order to add HTTP headers
                    var stsClient = new SecurityTokenServiceClient(IdpEndpoint);

                    // get token with custom parameters
                    // also get display token so we can see the claims
                    // prepare custom parameters
                    var hc = new HTTPHeaderCollection();

                    var customParameter = new HTTPHeader { Name = "X-MS-Client-Application", Value = valueToken };
                    hc.Add(customParameter);

                    token = stsClient.GetSecurityTokenFromStsByUserCertificate(authCert, new Uri(SpEndpoint), hc, ref displayClaims);

                    var newToken = new WebEas.Sts.SecurityTokenCache(key, token);
                    WebEas.Context.Current.LocalMachineCache.Set<WebEas.Sts.SecurityTokenCache>(newToken.Key, newToken, newToken.Token.ValidTo.AddMinutes(-2));
                    LogManager.GetLogger("STS").Info(string.Format("Getting token with key {0}", key));
                    return token;
                }
                else
                {
                    LogManager.GetLogger("STS").Info(string.Format("Returning cached token with key {0}", key));
                    return cache.Token;
                }
            }
            catch (FaultException ex)
            {
                if (ex.Message != null && ex.Message.Contains("MSIS7069"))
                {
                    throw new WebEasProxyException(string.Format("Security token is not Valid {0} / {1}", valueToken, authCert == null ? null : authCert.Subject), "Session nie je aktuálna", ex, valueToken, authCert == null ? null : authCert.Subject, displayClaims);
                }
                else
                {
                    throw new WebEasProxyException(string.Format("Cannot obtain security token with {0}", valueToken), "Nepodarilo sa získať povolenie pre externú službu", ex, valueToken, authCert == null ? null : authCert.Subject, displayClaims);
                }
            }
            catch (SecurityNegotiationException ex)
            {
                throw new WebEasProxyException(string.Format("Token not obtained. {0} - cert {1} - check Certificates if IIS APPPOOL/Apppool name has privileges!", valueToken, certThumbprint), "Použitý certifikát nemá oprávnenia k danému modulu", ex, authCert == null ? null : authCert.Subject, displayClaims);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException is WebException)
                {
                    if (((WebException)ex.InnerException).Response != null && ((WebException)ex.InnerException).Response is HttpWebResponse && ((HttpWebResponse)((WebException)ex.InnerException).Response).StatusCode == HttpStatusCode.Forbidden)
                    {
                        throw new WebEasProxyException(string.Format("Security Token Service invocation failed! Token not obtained. {0}", valueToken), "Zakázaný prístup k získaniu overenia", ex, authCert == null ? null : authCert.Subject, displayClaims);
                    }
                }
                throw new WebEasProxyException(string.Format("Security Token Service invocation failed! Token not obtained. {0}", valueToken), "Nie je možné identifikovať používateľa", ex, authCert == null ? null : authCert.Subject, displayClaims);
            }
        }
    }
}