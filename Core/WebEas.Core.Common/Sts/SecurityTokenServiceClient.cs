using System;
using System.IdentityModel.Tokens;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.ServiceModel.Security.Tokens;
using System.Xml;
using System.Collections.ObjectModel;
using WebEas.Log;

namespace WebEas.Core.Sts
{
    /// <summary>
    /// 
    /// </summary>
    internal class SecurityTokenServiceClient
    {
        /// <summary>
        /// Gets or sets the security token service endpoint.
        /// </summary>
        /// <value>The security token service endpoint.</value>
        public string SecurityTokenServiceEndpoint { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenServiceClient" /> class.
        /// </summary>
        /// <param name="securityTokenServiceEndpoint">The security token service endpoint.</param>
        internal SecurityTokenServiceClient(string securityTokenServiceEndpoint)
        {
            if (String.IsNullOrEmpty(securityTokenServiceEndpoint))
                throw new ArgumentNullException("SecurityTokenServiceEndpoint");
            this.SecurityTokenServiceEndpoint = securityTokenServiceEndpoint;
        }

        /// <summary>
        /// Get Security Token from STS server.
        /// </summary>
        /// <param name="userCertificate">The user certificate.</param>
        /// <param name="serviceProviderEndpoint">The service provider endpoint.</param>
        /// <param name="HTTPHeaders">The HTTP headers.</param>
        /// <param name="DisplayClaims">The display claims.</param>
        /// <returns></returns>
        public SecurityToken GetSecurityTokenFromStsByUserCertificate(X509Certificate2 userCertificate, Uri serviceProviderEndpoint, HTTPHeaderCollection httpHeaders, ref DisplayClaimCollection DisplayClaims)
        {
            SecurityToken issuedToken = null;

            IssuedSecurityTokenProvider provider = new IssuedSecurityTokenProvider();
            provider.SecurityTokenSerializer = new WSSecurityTokenSerializer();

            //Relying Party's identifier
            provider.TargetAddress = new EndpointAddress(serviceProviderEndpoint);
            provider.IssuerAddress = new EndpointAddress(new Uri(this.SecurityTokenServiceEndpoint));

            // Add display token to the request
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement requestDisplayTokenXml = xmlDoc.CreateElement("i:RequestDisplayToken", "http://schemas.xmlsoap.org/ws/2005/05/identity");
            provider.TokenRequestParameters.Add(requestDisplayTokenXml);

            // Set basic reqeust information
            provider.SecurityAlgorithmSuite = SecurityAlgorithmSuite.Basic256;
            provider.KeyEntropyMode = SecurityKeyEntropyMode.ServerEntropy;
            provider.MessageSecurityVersion = MessageSecurityVersion.WSSecurity11WSTrust13WSSecureConversation13WSSecurityPolicy12BasicSecurityProfile10;

            // authenticate using client certificate
            ClientCredentials c = new ClientCredentials();
            c.ClientCertificate.Certificate = userCertificate;
            provider.IssuerChannelBehaviors.Add(c);

            // add custom headers and get displaytoken if applicable
            SamlMessageInspectorBehavior s = new SamlMessageInspectorBehavior();
            // add headers before request
            s.CurrentSamlInspector.HTTPHeaders = httpHeaders;
            provider.IssuerChannelBehaviors.Add(s);

            provider.IssuerChannelBehaviors.Add(new WebEasLogMessageInspector(LogType.Debug));

            // create transport binding
            HttpsTransportBindingElement tbe = new HttpsTransportBindingElement();
            tbe.AuthenticationScheme = AuthenticationSchemes.Anonymous;
            tbe.RequireClientCertificate = true;
            tbe.KeepAliveEnabled = false;

            CustomBinding stsBinding = new CustomBinding(tbe);                        

            provider.IssuerBinding = stsBinding;
            provider.Open();

            //Request a token from ADFS STS
            issuedToken = provider.GetToken(TimeSpan.FromSeconds(30));

            //get display claims after request
            DisplayClaims = s.CurrentSamlInspector.DisplayClaims;

            provider.Close();

            return issuedToken;
        }

        /// <summary>
        /// Gets the name of the security token from STS by user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="serviceProviderEndpoint">The service provider endpoint.</param>
        /// <param name="HTTPHeaders">The HTTP headers.</param>
        /// <param name="DisplayClaims">The display claims.</param>
        /// <returns></returns>
        public SecurityToken GetSecurityTokenFromStsByUserName(string userName, string password, Uri serviceProviderEndpoint, HTTPHeaderCollection httpHeaders, ref DisplayClaimCollection DisplayClaims)
        {
            SecurityToken issuedToken = null;

            IssuedSecurityTokenProvider provider = new IssuedSecurityTokenProvider();
            provider.SecurityTokenSerializer = new WSSecurityTokenSerializer();

            //Relying Party's identifier
            provider.TargetAddress = new EndpointAddress(serviceProviderEndpoint);
            provider.IssuerAddress = new EndpointAddress(new Uri(this.SecurityTokenServiceEndpoint));

            // Add display token to the request
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement requestDisplayTokenXml = xmlDoc.CreateElement("i:RequestDisplayToken", "http://schemas.xmlsoap.org/ws/2005/05/identity");
            provider.TokenRequestParameters.Add(requestDisplayTokenXml);

            // Set basic reqeust information
            provider.SecurityAlgorithmSuite = SecurityAlgorithmSuite.Basic256;
            provider.KeyEntropyMode = SecurityKeyEntropyMode.ServerEntropy;
            provider.MessageSecurityVersion = MessageSecurityVersion.WSSecurity11WSTrust13WSSecureConversation13WSSecurityPolicy12BasicSecurityProfile10;

            // authenticate using client certificate
            ClientCredentials c = new ClientCredentials();
            c.UserName.UserName = userName;
            c.UserName.Password = password;
            provider.IssuerChannelBehaviors.Add(c);

            // add custom headers and get displaytoken if applicable
            SamlMessageInspectorBehavior s = new SamlMessageInspectorBehavior();
            // add headers before request
            s.CurrentSamlInspector.HTTPHeaders = httpHeaders;
            provider.IssuerChannelBehaviors.Add(s);

            // create transport binding
            HttpsTransportBindingElement tbe = new HttpsTransportBindingElement();
            tbe.AuthenticationScheme = AuthenticationSchemes.Basic;
            tbe.RequireClientCertificate = false;
            tbe.KeepAliveEnabled = false;

            CustomBinding stsBinding = new CustomBinding(tbe);

            provider.IssuerBinding = stsBinding;
            provider.Open();

            //Request a token from ADFS STS
            issuedToken = provider.GetToken(TimeSpan.FromSeconds(30));

            //get display claims after request
            DisplayClaims = s.CurrentSamlInspector.DisplayClaims;

            provider.Close();

            return issuedToken;
        }
    }

    internal class DisplayClaim
    {
        /// <summary>
        /// Gets or sets the type of the claim.
        /// </summary>
        /// <value>The type of the claim.</value>
        public string ClaimType { get; internal set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; internal set; }

        /// <summary>
        /// Gets or sets the display tag.
        /// </summary>
        /// <value>The display tag.</value>
        public string DisplayTag { get; internal set; }

        /// <summary>
        /// Gets or sets the display value.
        /// </summary>
        /// <value>The display value.</value>
        public string DisplayValue { get; internal set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("ClaimType: {0}, Description: {1}, DisplayTag: {2}, DisplayValue: {3}", ClaimType, Description, DisplayTag, DisplayValue);
        }
    }

    internal class HTTPHeader
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }
    }

    internal class DisplayClaimCollection : Collection<DisplayClaim>
    {

    }

    internal class HTTPHeaderCollection : Collection<HTTPHeader>
    { }
}
