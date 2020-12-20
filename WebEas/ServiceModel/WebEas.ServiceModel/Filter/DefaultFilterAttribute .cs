using System;
using System.Linq;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// Default Filter
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct, AllowMultiple = false)]
    public class DefaultFilterAttribute : Attribute
    {
        public DefaultFilterAttribute(string filterFormated, params object[] parameters)
        {
            this.DefaultFilter = string.Format(filterFormated, parameters);
        }

        /// <summary>
        /// Gets or sets the default filter.
        /// </summary>
        /// <value>The default filter.</value>
        public string DefaultFilter { get; set; }
    }
}