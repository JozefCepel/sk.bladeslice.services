using System;
using System.Linq;
using System.ServiceModel.Description;

namespace WebEas.Sts
{
    /// <summary>
    /// 
    /// </summary>
    public class CachedClientCredentials : ClientCredentials
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CachedClientCredentials" /> class.
        /// </summary>
        /// <param name="tokenCache">The token cache.</param>
        public CachedClientCredentials(SecurityTokenCache tokenCache) : base()
        {
            this.TokenCache = tokenCache;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedClientCredentials" /> class.
        /// </summary>
        /// <param name="tokenCache">The token cache.</param>
        /// <param name="clientCredentials">The client credentials.</param>
        public CachedClientCredentials(SecurityTokenCache tokenCache, ClientCredentials clientCredentials) : base(clientCredentials)
        {
            this.TokenCache = tokenCache;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedClientCredentials" /> class.
        /// </summary>
        /// <param name="clientCredentials">The client credentials.</param>
        public CachedClientCredentials(CachedClientCredentials clientCredentials) : base(clientCredentials)
        {
            this.TokenCache = clientCredentials.TokenCache;
        }

        /// <summary>
        /// Gets or sets the token cache.
        /// </summary>
        /// <value>The token cache.</value>
        public SecurityTokenCache TokenCache { get; private set; }

        /// <summary>
        /// When overridden in a derived class, creates a new <see cref="T:System.IdentityModel.Selectors.SecurityTokenManager" />.
        /// </summary>
        /// <returns>
        /// The newly created <see cref="T:System.IdentityModel.Selectors.SecurityTokenManager" />.
        /// </returns>
        public override System.IdentityModel.Selectors.SecurityTokenManager CreateSecurityTokenManager()
        {
            // Begin Callout A
            return new CachedClientCredentialsSecurityTokenManager((CachedClientCredentials)this.Clone());
            // End Callout A
        }

        /// <summary>
        /// Creates a new copy of this <see cref="T:System.ServiceModel.Description.ClientCredentials" />
        /// instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.ServiceModel.Description.ClientCredentials" />
        /// instance.
        /// </returns>
        protected override ClientCredentials CloneCore()
        {
            return new CachedClientCredentials(this);
        }
    }
}