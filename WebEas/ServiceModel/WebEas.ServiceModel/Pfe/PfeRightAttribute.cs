using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// Popis
    /// </summary>
    [DataContract]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Enum | AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    public class PfeRightAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PfeCaptionAttribute" /> class.
        /// </summary>
        /// <param name="right">The caption.</param>
        public PfeRightAttribute(int right)
        {
            this.Right = right;
        }

        public PfeRightAttribute(Pravo right)
        {
            this.Right = (int) right;
        }

        /// <summary>
        /// Gets or sets minimal Riht Level.
        /// </summary>
        /// <value>The caption.</value>
        [DataMember]
        public int Right { get; set; }
    }
}