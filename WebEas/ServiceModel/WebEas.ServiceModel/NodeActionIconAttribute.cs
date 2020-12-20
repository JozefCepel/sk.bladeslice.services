using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    [DataContract(Name = "Action icon")]
    public class NodeActionIconAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the icon
        /// </summary>
        [DataMember]
        public NodeActionIcons Icon { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeActionIconAttribute" /> class.
        /// </summary>
        /// <param name="icon">The node action icon.</param>
        public NodeActionIconAttribute(NodeActionIcons icon)
        {
            this.Icon = icon;
        }
    }
}