using System;
using System.Configuration;
using System.Linq;

namespace WebEas.Services.Esb.Config
{
    /// <summary>
    /// 
    /// </summary>
    [ConfigurationCollection(typeof(ProxyElement))]
    public class ProxyElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// When overridden in a derived class, creates a new <see cref="T:System.Configuration.ConfigurationElement" />.
        /// </summary>
        /// <returns>
        /// A newly created <see cref="T:System.Configuration.ConfigurationElement" />.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new ProxyElement();
        }

        /// <summary>
        /// Gets the element key for a specified configuration element when overridden
        /// in a derived class.
        /// </summary>
        /// <param name="element">The <see cref="T:System.Configuration.ConfigurationElement" />
        /// to return the key for.</param>
        /// <returns>
        /// An <see cref="T:System.Object" /> that acts as the key for the specified
        /// <see cref="T:System.Configuration.ConfigurationElement" />.
        /// </returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ProxyElement)element).Name;
        }
    }
}