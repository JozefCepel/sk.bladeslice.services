using System;
using System.IdentityModel.Tokens;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security.Tokens;
using System.Xml;

namespace WebEas.Sts
{
    /// <summary>
    /// 
    /// </summary>
    public class CachedIssuedSecurityTokenProvider : IssuedSecurityTokenProvider, ICommunicationObject, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CachedIssuedSecurityTokenProvider" /> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="clientCredentials">The client credentials.</param>
        public CachedIssuedSecurityTokenProvider(IssuedSecurityTokenProvider provider, CachedClientCredentials clientCredentials) : base()
        {
            this.InnerProvider = provider;
            this.ClientCredentials = clientCredentials;

            this.CacheIssuedTokens = provider.CacheIssuedTokens;
            this.IdentityVerifier = provider.IdentityVerifier;
            this.IssuedTokenRenewalThresholdPercentage = provider.IssuedTokenRenewalThresholdPercentage;
            this.IssuerAddress = provider.IssuerAddress;
            this.IssuerBinding = provider.IssuerBinding;

            foreach (IEndpointBehavior item in provider.IssuerChannelBehaviors)
            {
                this.IssuerChannelBehaviors.Add(item);
            }

            this.KeyEntropyMode = provider.KeyEntropyMode;
            this.MaxIssuedTokenCachingTime = provider.MaxIssuedTokenCachingTime;
            this.MessageSecurityVersion = provider.MessageSecurityVersion;
            this.SecurityAlgorithmSuite = provider.SecurityAlgorithmSuite;
            this.SecurityTokenSerializer = provider.SecurityTokenSerializer;
            this.TargetAddress = provider.TargetAddress;

            foreach (XmlElement item in provider.TokenRequestParameters)
            {
                this.TokenRequestParameters.Add(item);
            }
        }

        private CachedClientCredentials ClientCredentials { get; set; }

        private IssuedSecurityTokenProvider InnerProvider { get; set; }

        /// <summary>
        /// Gets the token core.
        /// </summary>
        /// <param name="timeout">A <see cref="T:System.TimeSpan" /> after which the call
        /// times out.</param>
        /// <returns>
        /// The <see cref="T:System.IdentityModel.Tokens.SecurityToken" /> that represents
        /// the token core.
        /// </returns>
        protected override SecurityToken GetTokenCore(TimeSpan timeout)
        {
            SecurityToken securityToken = null;

            if (this.ClientCredentials.TokenCache.IsValidToken())
            {
                securityToken = this.ClientCredentials.TokenCache.Token;
            }
            else
            {
                securityToken = this.InnerProvider.GetToken(timeout);
                this.ClientCredentials.TokenCache.Token = securityToken;
            }

            return securityToken;
        }

        protected override SecurityToken RenewTokenCore(TimeSpan timeout, SecurityToken tokenToBeRenewed)
        {
            SecurityToken securityToken = null;

            if (this.ClientCredentials.TokenCache.IsValidToken())
            {
                securityToken = this.ClientCredentials.TokenCache.Token;
            }
            else
            {
                securityToken = this.InnerProvider.GetToken(timeout);
                this.ClientCredentials.TokenCache.Token = securityToken;
            }

            return securityToken;
        }

        public override bool SupportsTokenRenewal
        {
            get
            {
                return false;   
            }
        }
    }
}