using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    [DataContract(Name = "Filter")]
    public class PfeFilterAttribute : Attribute, IEquatable<PfeFilterAttribute>
    {
        /// <summary>
        /// Konštruktor pre vytvorenie filtra
        /// </summary>
        /// <param name="field">Nazov stlpca</param>
        /// <param name="comparisionOperator">Operator porovnania</param>
        /// <param name="value">Hodnota pre vyhladanie</param>
        /// <param name="onlyIdentical">Len ideticke</param>
        /// <param name="logicOperator">Logicke operatory</param>
        /// <param name="leftBrace">Povolenie lavej zatvorky</param>
        /// <param name="rightBrace">Povolenie pravej zatvorky</param>
        public PfeFilterAttribute(string field, string comparisonOperator, object value, bool onlyIdentical, string logicOperator, byte leftBrace, byte rightBrace, PfeDataType type)
        {
            this.Field = field;
            this.ComparisonOperator = comparisonOperator;
            this.Value = value;
            this.OnlyIdentical = onlyIdentical;
            this.LogicOperator = logicOperator;
            this.LeftBrace = leftBrace;
            this.RightBrace = rightBrace;
            this.Type = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PfeFilterAttribute" /> class.
        /// </summary>
        public PfeFilterAttribute()
        {
        }

        /// <summary>
        /// Nazov stlpca
        /// </summary>
        [DataMember(Name = "fld")]
        public string Field { get; set; }

        /// <summary>
        /// Zoznam operatorov pre porovnanie: =, <, >, >=, <=, <>, LIKE, NOT LIKE
        /// </summary>
        [DataMember(Name = "cop")]
        public string ComparisonOperator { get; set; }

        /// <summary>
        /// Hodnota na vyhladanie
        /// </summary>
        [DataMember(Name = "val")]
        public object Value { get; set; }

        /// <summary>
        /// Plati len pre  operatory = a <>, pre ostatne vzdy false
        /// </summary>
        [DataMember(Name = "oid")]
        public bool OnlyIdentical { get; set; }

        /// <summary>
        /// Logicky operator, pre prvu hodnotu vzdy null
        /// </summary>
        [DataMember(Name = "lop")]
        public string LogicOperator { get; set; }

        /// <summary>
        /// Shoulds the serialize LogicOperator.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeLogicOperator()
        {
            return this.LogicOperator != null;
        }

        /// <summary>
        /// Pocet lavych zatvoriek
        /// </summary>
        [DataMember(Name = "lbr")]
        public byte LeftBrace { get; set; }

        /// <summary>
        /// Pocet pravych zatvoriek
        /// </summary>
        [DataMember(Name = "rbr")]
        public byte RightBrace { get; set; }

        /// <summary>
        /// Typ filtra
        /// </summary>
        [DataMember(Name = "typ")]
        public PfeDataType Type { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("Field: {0}, ComparisonOperator: {1}, Value: {2}, OnlyIdentical: {3}, LogicOperator: {4}, LeftBrace: {5}, RightBrace: {6}, Type: {7}", this.Field, this.ComparisonOperator, this.Value, this.OnlyIdentical, this.LogicOperator, this.LeftBrace, this.RightBrace, this.Type);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = 17;
                result = result * 23 + base.GetHashCode();
                result = result * 23 + ((this.Field != null) ? this.Field.GetHashCode() : 0);
                result = result * 23 + ((this.ComparisonOperator != null) ? this.ComparisonOperator.GetHashCode() : 0);
                result = result * 23 + ((this.Value != null) ? this.Value.GetHashCode() : 0);
                result = result * 23 + this.OnlyIdentical.GetHashCode();
                result = result * 23 + ((this.LogicOperator != null) ? this.LogicOperator.GetHashCode() : 0);
                result = result * 23 + this.LeftBrace.GetHashCode();
                result = result * 23 + this.RightBrace.GetHashCode();
                result = result * 23 + this.Type.GetHashCode();
                return result;
            }
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(PfeFilterAttribute other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return base.Equals(other) &&
                   Equals(this.Field, other.Field) &&
                   Equals(this.ComparisonOperator, other.ComparisonOperator) &&
                   Equals(this.Value, other.Value) &&
                   this.OnlyIdentical == other.OnlyIdentical &&
                   Equals(this.LogicOperator, other.LogicOperator) &&
                   this.LeftBrace == other.LeftBrace &&
                   this.RightBrace == other.RightBrace &&
                   this.Type == other.Type;                   
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
            PfeFilterAttribute temp = obj as PfeFilterAttribute;
            if (temp == null)
            {
                return false;
            }
            return this.Equals(temp);
        }
    }
}