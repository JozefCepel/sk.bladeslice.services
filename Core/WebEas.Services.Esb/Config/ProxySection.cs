using System;
using System.Configuration;
using System.Linq;

namespace WebEas.Services.Esb.Config
{
    /// <summary>
    /// 
    /// </summary>
    public class ProxySection : ConfigurationSection
    {
        /// <summary>
        /// Gets or sets the proxies.
        /// </summary>
        /// <value>The proxies.</value>
        [ConfigurationProperty("proxies", IsDefaultCollection = true)]
        public ProxyElementCollection Proxies
        {
            get
            {
                return (ProxyElementCollection)this["proxies"];
            }
            set
            {
                this["proxies"] = value;
            }
        }
    }
}