using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Security.Tokens;

namespace WebEas.Sts
{
    /// <summary>
    /// 
    /// </summary>
    public class CachedClientCredentialsSecurityTokenManager : ClientCredentialsSecurityTokenManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CachedClientCredentialsSecurityTokenManager" /> class.
        /// </summary>
        /// <param name="clientCredentials">The client credentials.</param>
        public CachedClientCredentialsSecurityTokenManager(CachedClientCredentials clientCredentials)
            : base(clientCredentials)
        {
        }

        /// <summary>
        /// Gets a security token provider that meets the specified security token
        /// requirements.
        /// </summary>
        /// <param name="tokenRequirement">A <see cref="T:System.IdentityModel.Selectors.SecurityTokenRequirement" />
        /// that specifies the security token requirements.</param>
        /// <returns>
        /// A <see cref="T:System.IdentityModel.Selectors.SecurityTokenProvider" />
        /// that provides security tokens that meet the specified requirements for outgoing
        /// SOAP messages.
        /// </returns>
        public override System.IdentityModel.Selectors.SecurityTokenProvider CreateSecurityTokenProvider(System.IdentityModel.Selectors.SecurityTokenRequirement tokenRequirement)
        {
            var provider = base.CreateSecurityTokenProvider(tokenRequirement) as IssuedSecurityTokenProvider;

            if (provider == null)
            {
                return base.CreateSecurityTokenProvider(tokenRequirement);
            }

            var cachedProvider = new CachedIssuedSecurityTokenProvider(provider, (CachedClientCredentials)this.ClientCredentials);
            return cachedProvider;
        }
    }
}