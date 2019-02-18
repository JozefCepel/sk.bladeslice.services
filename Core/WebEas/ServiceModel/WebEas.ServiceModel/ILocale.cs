using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    public interface ILocale
    {
        /// <summary>
        /// Gets or sets the locale.
        /// </summary>
        /// <value>The locale.</value>
        [DataMember(Name = "locale")]
        string Locale { get; set; }
    }

    public interface IZdvLocale
    {
        /// <summary>
        /// Gets or sets the locale.
        /// </summary>
        /// <value>The locale (ZDV).</value>
        [DataMember(Name = "locale")]
        string Locale { get; set; }
    }
}