using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [DataContract]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Field | AttributeTargets.GenericParameter | AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    public class PfeOriginalAliasAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PfeOriginalAliasAttribute" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public PfeOriginalAliasAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [DataMember]
        public string Name { get; set; }
    }
}