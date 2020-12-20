using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [DataContract]
    public class PfeSummaryRow : IEquatable<PfeSummaryRow>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PfeSummaryRow" /> class.
        /// </summary>
        public PfeSummaryRow()
        {
        }

        /// <summary>
        /// Summary name
        /// </summary>
        /// <value>The text.</value>
        [DataMember(Name = "sn")]
        public string SummaryName { get; set; }

        /// <summary>
        /// Summary type
        /// </summary>
        /// <value>The text.</value>
        [DataMember(Name = "st")]
        public string SummaryType { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("SummaryName: {0}, SummaryType: {1}", SummaryName, SummaryType);
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
                result = result * 23 + ((SummaryName != null) ? SummaryName.GetHashCode() : 0);
                result = result * 23 + ((SummaryType != null) ? SummaryType.GetHashCode() : 0);
                return result;
            }
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(PfeSummaryRow other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(this.SummaryName, other.SummaryName) &&
                   Equals(this.SummaryType, other.SummaryType);
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
            PfeSummaryRow temp = obj as PfeSummaryRow;
            if (temp == null)
            {
                return false;
            }
            return Equals(temp);
        }
    }
}