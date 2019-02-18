using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{ 
    [DataContract]
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class PfeOriginalSchemaAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PfeOriginalSchemaAttribute" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public PfeOriginalSchemaAttribute(string name)
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