using System;
using System.Linq;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// Definícia vlastnosti číselníka
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class DialAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DialAttribute" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public DialAttribute(DialType type = DialType.Unknown, DialKindType kind = DialKindType.Unknown)
        {
            this.Type = type;
            this.Kind = kind;
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public DialType Type { get; set; }

        /// <summary>
        /// Gets or sets the kind.
        /// </summary>
        /// <value>The kind.</value>
        public DialKindType Kind { get; set; }
    }
}