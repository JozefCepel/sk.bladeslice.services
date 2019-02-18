using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    [DataContract(Name = "Sort")]
    public class PfeSortAttribute : Attribute, IEquatable<PfeSortAttribute>
    { 
        /// <summary>
        /// Konštruktor pre vytvorenie usporiadania
        /// </summary>
        /// <param name="field">Nazov stlpca</param>
        /// <param name="Sort">Sposob usporiadania</param>
        public PfeSortAttribute(string field, PfeOrder sort = PfeOrder.Asc)
        {
            this.Field = field;
            this.Sort = sort;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PfeSortAttribute" /> class.
        /// </summary>
        /// <param name="sort">The sort.</param>
        public PfeSortAttribute(PfeOrder sort = PfeOrder.Asc)
        {
            this.Sort = sort;
        }

        /// <summary>
        /// Gets or sets the rank.
        /// </summary>
        /// <value>The rank.</value>
        public int Rank { get; set; }

        /// <summary>
        /// Nazov stlpca
        /// </summary>
        [DataMember(Name = "fld")]
        public string Field { get; set; }

        /// <summary>
        /// Sposob usporiadania
        /// </summary>
        [DataMember(Name = "srt")]
        public PfeOrder Sort { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("Field: {0}, Sort: {1}", this.Field, this.Sort);
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
                result = result * 23 + ((this.Field != null) ? this.Field.GetHashCode() : 0);
                result = result * 23 + this.Sort.GetHashCode();
                return result;
            }
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(PfeSortAttribute other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(this.Field, other.Field) &&
                   this.Sort.Equals(other.Sort);
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
            PfeSortAttribute temp = obj as PfeSortAttribute;
            if (temp == null)
            {
                return false;
            }
            return this.Equals(temp);
        }
    }
}