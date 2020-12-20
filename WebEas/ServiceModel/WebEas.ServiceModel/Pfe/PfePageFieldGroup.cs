using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract(Name = "PageFieldGroup")]
    public class PfePageFieldGroup : IEquatable<PfePageFieldGroup>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PfePageFieldGroup" /> class.
        /// </summary>
        public PfePageFieldGroup()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PfePageFieldGroup" /> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="fields">The fields.</param>
        public PfePageFieldGroup(string text, List<string> fields)
        {
            this.Text = text;
            this.Fields = fields;
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        [DataMember(Name = "txt")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the fields.
        /// </summary>
        /// <value>The fields.</value>
        [DataMember(Name = "flds")]
        public List<string> Fields { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("Text: {0}, Fields: {1}", this.Text, this.Fields);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = 17;
                result = result * 23 + ((this.Text != null) ? this.Text.GetHashCode() : 0);
                result = result * 23 + ((this.Fields != null) ? this.Fields.GetHashCode() : 0);
                return result;
            }
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(PfePageFieldGroup other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(this.Text, other.Text) &&
                   Equals(this.Fields, other.Fields);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise,
        /// false.
        /// </returns>
        public override bool Equals(object obj)
        {
            PfePageFieldGroup temp = obj as PfePageFieldGroup;
            if (temp == null)
            {
                return false;
            }
            return this.Equals(temp);
        }
    }
}