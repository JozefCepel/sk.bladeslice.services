using System;
using System.Linq;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class PublicReferenceAttribute : Attribute
    {
        public PublicReferenceAttribute(Type reference)
        {
            this.Reference = reference;
        }

        /// <summary>
        /// Gets or sets the reference.
        /// </summary>
        /// <value>The reference.</value>
        public Type Reference { get; set; }
    }
}