using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [DataContract(Name = "Rule")]
    public class PfeRule : IEquatable<PfeRule>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PfeRule" /> class.
        /// </summary>
        public PfeRule()
        {
        }

        public PfeRule(PfeValidatorType validatorType, List<PfeFilterAttribute> condition, string message, string slbl, string sval, string smin, string smax)
        {
            ValidatorType = validatorType;
            Condition = condition ?? new List<PfeFilterAttribute>();
            Message = message;
            Label = slbl;
            Value = sval;
            Min = smin;
            Max = smax;
        }

        /// <summary>
        /// Typ pfe validatora
        /// </summary>
        /// <value>The text.</value>
        [DataMember(Name = "tp")]
        public PfeValidatorType ValidatorType { get; set; }

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
        /// Shoulds the serialize Message.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeMessage()
        {
            return this.Message != null;
        }

        /// <summary>
        /// Label - slbl
        /// </summary>
        [DataMember(Name = "slbl")]
        public string Label { get; set; }

        /// <summary>
        /// Shoulds the serialize Label.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeLabel()
        {
            return this.Label != null;
        }

        /// <summary>
        /// Value - sval
        /// </summary>
        [DataMember(Name = "sval")]
        public string Value { get; set; }

        /// <summary>
        /// Shoulds the serialize Value.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeValue()
        {
            return this.Value != null;
        }

        /// <summary>
        /// Min - smin
        /// </summary>
        [DataMember(Name = "smin")]
        public string Min { get; set; }

        /// <summary>
        /// Shoulds the serialize Min.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeMin()
        {
            return this.Min != null;
        }

        /// <summary>
        /// Max - smax
        /// </summary>
        [DataMember(Name = "smax")]
        public string Max { get; set; }

        /// <summary>
        /// Shoulds the serialize Max.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeMax()
        {
            return this.Max != null;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"ValidatorType: {ValidatorType}, Condition: {Condition}, msg: {Message}, slbl: {Label}, sval: {Value}, smin: {Min}, smax: {Max}";
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
                result = result * 23 + ValidatorType.GetHashCode();
                result = result * 23 + ((Condition != null) ? Condition.GetHashCode() : 0);
                result = result * 23 + ((Message != null) ? Message.GetHashCode() : 0);
                result = result * 23 + ((Label != null) ? Label.GetHashCode() : 0);
                result = result * 23 + ((Value != null) ? Value.GetHashCode() : 0);
                result = result * 23 + ((Min != null) ? Min.GetHashCode() : 0);
                result = result * 23 + ((Max != null) ? Max.GetHashCode() : 0);
                return result;
            }
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(PfeRule other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(ValidatorType, other.ValidatorType) &&
                   Equals(Condition, other.Condition) &&
                   Equals(Message, other.Message) &&
                   Equals(Label, other.Label) &&
                   Equals(Value, other.Value) &&
                   Equals(Min, other.Min) &&
                   Equals(Max, other.Max);
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
            if (!(obj is PfeRule temp))
            {
                return false;
            }
            return Equals(temp);
        }
    }
}