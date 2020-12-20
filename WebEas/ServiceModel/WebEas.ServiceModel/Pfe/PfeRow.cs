using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [DataContract(Name = "Row")]
    public class PfeRow : IEquatable<PfeRow>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PfeRow" /> class.
        /// </summary>
        /// <param name="fieldGroups">The field groups.</param>
        public PfeRow(List<PfePageFieldGroup> fieldGroups)
        {
            this.FieldGroups = fieldGroups;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PfeRow" /> class.
        /// </summary>
        public PfeRow()
        { 
        }

        /// <summary>
        /// Gets or sets the field groups.
        /// </summary>
        /// <value>The field groups.</value>
        [DataMember(Name = "flgs")]
        public List<PfePageFieldGroup> FieldGroups { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("FieldGroups: {0}", this.FieldGroups);
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
                result = result * 23 + ((this.FieldGroups != null) ? this.FieldGroups.GetHashCode() : 0);
                return result;
            }
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(PfeRow other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(this.FieldGroups, other.FieldGroups);
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
            PfeRow temp = obj as PfeRow;
            if (temp == null)
            {
                return false;
            }
            return this.Equals(temp);
        }
    }
}