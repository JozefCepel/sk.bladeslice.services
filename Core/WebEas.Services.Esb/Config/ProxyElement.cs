using System;
using System.Configuration;
using System.Linq;

namespace WebEas.Services.Esb.Config
{
    /// <summary>
    /// 
    /// </summary>
    public class ProxyElement : ConfigurationSection
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        [ConfigurationProperty("url", IsRequired = false, DefaultValue = "http://localhost")]
        [RegexStringValidator(@"https?\://\S+")]
        public string Url
        {
            get
            {
                return (string)this["url"];
            }
            set
            {
                this["url"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the use service model.
        /// </summary>
        /// <value>The use service model.</value>
        [ConfigurationProperty("model", IsRequired = false, DefaultValue = false)]
        public bool UseServiceModel
        {
            get
            {
                return (bool)this["model"];
            }
            set
            {
                this["model"] = value;
            }
        }
    }
}