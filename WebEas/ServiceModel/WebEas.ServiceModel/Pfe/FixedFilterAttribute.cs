using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [DataContract]
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class FixedFilterAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PfeOriginalAliasAttribute" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public FixedFilterAttribute(string value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The name.</value>
        [DataMember]
        public string Value { get; set; }
    }
}