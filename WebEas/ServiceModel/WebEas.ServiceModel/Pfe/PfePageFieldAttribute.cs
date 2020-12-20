using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    [DataContract(Name = "PageField")]
    public class PfePageFieldAttribute : Attribute, IEquatable<PfePageFieldAttribute>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PfePageFieldAttribute" /> class.
        /// </summary>
        public PfePageFieldAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PfePageFieldAttribute" /> class.
        /// </summary>
        /// <param name="field">The field.</param>
        public PfePageFieldAttribute(string field)
        {
            this.Field = field;
        }

        /// <summary>
        /// Gets or sets the field.
        /// </summary>
        /// <value>The field.</value>
        [DataMember(Name = "field")]
        public string Field { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("Field: {0}", this.Field);
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
                return result;
            }
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(PfePageFieldAttribute other)
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
                   Equals(this.Field, other.Field);
        }

        /// <summary>
        /// Returns a value that indicates whether this instance is equal to a specified
        /// object.
        /// </summary>
        /// <param name="obj">An <see cref="T:System.Object" /> to compare with this instance
        /// or null.</param>
        /// <returns>
        /// true if <paramref name="obj" /> equals the type and value of this instance;
        /// otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            PfePageFieldAttribute temp = obj as PfePageFieldAttribute;
            if (temp == null)
            {
                return false;
            }
            return this.Equals(temp);
        }
    }
}