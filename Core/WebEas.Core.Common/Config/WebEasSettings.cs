using System;
using System.Configuration;
using System.Linq;

namespace WebEas.Config
{
    /// <summary>
    /// 
    /// </summary>
    public class WebEasSettings : ConfigurationSection
    {
        private static readonly WebEasSettings settings = ConfigurationManager.GetSection("WebEasSettings") as WebEasSettings;

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <value>The settings.</value>
        public static WebEasSettings Settings
        {
            get
            {
                return settings;
            }
        }

        /// <summary>
        /// Gets or sets the log.
        /// </summary>
        /// <value>The log.</value>
        [ConfigurationProperty("log")]
        public WebEasLogElement Log
        {
            get
            {
                return (WebEasLogElement)this["log"];
            }
            set
            {
                this["log"] = value;
            }
        }
    }
}