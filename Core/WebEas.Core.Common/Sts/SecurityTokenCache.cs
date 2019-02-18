using System;
using System.IdentityModel.Tokens;
using System.Linq;

namespace WebEas.Sts
{
    /// <summary>
    /// 
    /// </summary>
    public class SecurityTokenCache
    {
        private SecurityToken token;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenCache" /> class.
        /// </summary>
        public SecurityTokenCache()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenCache" /> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public SecurityTokenCache(string key, SecurityToken token)
        {
            this.Key = key;
            this.token = token;
        }

        /// <summary>
        /// Occurs when [token updated].
        /// </summary>
        public event EventHandler TokenUpdated;

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>The token.</value>
        public SecurityToken Token
        {
            get
            {
                return this.token;
            }
            set
            {
                this.token = value;
                if (this.TokenUpdated != null)
                {
                    this.TokenUpdated(this, null);
                }
            }
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public string Key { get; set; }

        /// <summary>
        /// Determines whether [is valid token].
        /// </summary>
        /// <returns></returns>
        public bool IsValidToken()
        {
            if (this.token == null)
            {
                return false;
            }

            return (DateTime.UtcNow <= this.token.ValidTo.ToUniversalTime());
        }
    }
}