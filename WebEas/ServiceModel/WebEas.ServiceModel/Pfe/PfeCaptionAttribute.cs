using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// Popis
    /// </summary>
    [DataContract]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Field | AttributeTargets.GenericParameter | AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    public class PfeCaptionAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PfeCaptionAttribute" /> class.
        /// </summary>
        /// <param name="caption">The caption.</param>
        public PfeCaptionAttribute(string caption)
        {
            this.Caption = caption;
        }

        /// <summary>
        /// Gets or sets the caption.
        /// </summary>
        /// <value>The caption.</value>
        [DataMember]
        public string Caption { get; set; }
    }
}