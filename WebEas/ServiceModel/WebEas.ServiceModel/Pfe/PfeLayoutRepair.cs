using System;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [DataContract(Name = "Layout")]
    public class PfeLayoutRepair : IEquatable<PfeLayoutRepair>
    {
        public PfeLayoutRepair()
        {
        }

        [DataMember(Name = "lyts")]
        public PfeLayout[] Layout { get; set; }

        /// <summary>
        /// Data pre typ double click action.
        /// </summary>
        /// <value>The double click action.</value>
        [DataMember(Name = "dca")]
        public NodeAction DoubleClickAction { get; set; }

        /// <summary>
        /// Čakať na vstupné údaje
        /// </summary>
        [DataMember(Name = "wfid")]
        public bool? WaitForInputData { get; set; }

        [DataMember(Name = "uab")]
        public bool? UseAsBrowser { get; set; }

        [DataMember(Name = "uabr")]
        public int? UseAsBrowserRank { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("Layout: {0}", this.Layout);
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
                result = result * 23 + ((this.Layout != null) ? this.Layout.GetHashCode() : 0);
                return result;
            }
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(PfeLayoutRepair other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(this.Layout, other.Layout);
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
            var temp = obj as PfeLayoutRepair;
            if (temp == null)
            {
                return false;
            }
            return this.Equals(temp);
        }
    }
}