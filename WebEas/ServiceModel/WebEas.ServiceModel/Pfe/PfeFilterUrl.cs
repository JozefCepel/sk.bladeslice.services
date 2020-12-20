using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [DataContract(Name = "FilterUrl")]
    public class PfeFilterUrl : IEquatable<PfeFilterUrl>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PfeFilterUrl" /> class.
        /// </summary>
        public PfeFilterUrl()
        {
        }

        /// <summary>
        /// Gets or sets the config.
        /// </summary>
        /// <value>The config.</value>
        [DataMember(Name = "cfg")]
        public string Config { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        [DataMember(Name = "val")]
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets Ribbon Filter.
        /// </summary>
        /// <value>The value.</value>
        [DataMember(Name = "RF")]
        public bool RibbonFilter { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"Config: {Config}, Value: {Value}, RF: {RibbonFilter}";
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
                result = result * 23 + Config.GetHashCode();
                result = result * 23 + Value.GetHashCode();
                result = result * 23 + RibbonFilter.GetHashCode();
                return result;
            }
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(PfeFilterUrl other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(Config, other.Config) &&
                   Equals(Value, other.Value) &&
                   Equals(RibbonFilter, other.RibbonFilter);
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
            var temp = obj as PfeFilterUrl;
            if (temp == null)
            {
                return false;
            }
            return this.Equals(temp);
        }
    }
}