using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [DataContract(Name = "Pages")]
    public class PfePageSerialization
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PfePage" /> class.
        /// </summary>
        public PfePageSerialization()
        {
            this.Pages = new List<PfePage>();
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        [DataMember(Name = "pges")]
        public List<PfePage> Pages;

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("Pages: {0}", this.Pages);
        }
    }
}