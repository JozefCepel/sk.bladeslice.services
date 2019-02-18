using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [DataContract(Name = "Message")]
    public class PfeMessage : IEquatable<PfeMessage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PfeMessage" /> class.
        /// </summary>
        public PfeMessage()
        {
        }

        public PfeMessage(PfeMessageType validatorType, List<PfeFilterAttribute> condition, string message)
        {
            MessageType = validatorType;
            Condition = condition ?? new List<PfeFilterAttribute>();
            Message = message;
        }

        /// <summary>
        /// Typ pfe validatora
        /// </summary>
        /// <value>The text.</value>
        [DataMember(Name = "tp")]
        public PfeMessageType MessageType { get; set; }

        /// <summary>
        /// Podmienka na zaklade ktorej sa vyhodnocuje validator. - cd
        /// </summary>
        [DataMember(Name = "cd")]
        public List<PfeFilterAttribute> Condition { get; set; }

        /// <summary>
        /// Message - msg
        /// </summary>
        [DataMember(Name = "msg")]
        public string Message { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"ValidatorType: {MessageType}, Condition: {Condition}, msg: {Message}";
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
                result = result * 23 + MessageType.GetHashCode();
                result = result * 23 + ((Condition != null) ? Condition.GetHashCode() : 0);
                result = result * 23 + ((Message != null) ? Message.GetHashCode() : 0);
                return result;
            }
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(PfeMessage other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(MessageType, other.MessageType) &&
                   Equals(Condition, other.Condition) &&
                   Equals(Message, other.Message);
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
            if (!(obj is PfeMessage temp))
            {
                return false;
            }
            return Equals(temp);
        }
    }
}