using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [DataContract(Name = "Validator")]
    public class PfeValidator : IEquatable<PfeValidator>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PfeValidator" /> class.
        /// </summary>
        public PfeValidator()
        {
        }

        public PfeValidator(List<PfeRule> rules, List<PfeMessage> messages)
        {
            Rules = rules ?? new List<PfeRule>();
            Messages = messages ?? new List<PfeMessage>();
        }

        /// <summary>
        /// Rules
        /// </summary>
        [DataMember(Name = "rules")]
        public List<PfeRule> Rules { get; set; }

        /// <summary>
        /// Messages
        /// </summary>
        [DataMember(Name = "msg")]
        public List<PfeMessage> Messages { get; set; }

        /// <summary>
        /// Shoulds the serialize Messages.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeMessages()
        {
            return this.Messages != null;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"Rules: {Rules}, Messages: {Messages}";
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
                result = result * 23 + Rules.GetHashCode();
                result = result * 23 + Messages.GetHashCode();
                return result;
            }
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(PfeValidator other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(Rules, other.Rules) &&
                   Equals(Messages, other.Messages);
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
            if (!(obj is PfeValidator temp))
            {
                return false;
            }
            return Equals(temp);
        }
    }
}