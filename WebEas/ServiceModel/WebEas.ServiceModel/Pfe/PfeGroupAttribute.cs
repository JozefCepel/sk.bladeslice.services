using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    [DataContract(Name = "Group")]
    public class PfeGroupAttribute : Attribute, IEquatable<PfeGroupAttribute>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Group" /> class.
        /// </summary>
        public PfeGroupAttribute()
        {
        }

        /// <summary>
        /// Konštruktor pre vytvorenie skupiny
        /// </summary>
        /// <param name="field">Nazov stlpca</param>
        /// <param name="Sort">Sposob usporiadania</param>
        /// <param name="Collapsed">Rozbalenie/zbalenie skupiny</param>
        public PfeGroupAttribute(string field, PfeOrder sort = PfeOrder.Asc, bool collapsed = false)
        {
            this.Field = field;
            this.Sort = sort;
            this.Collapsed = collapsed;
        }

        /// <summary>
        /// Konštruktor pre vytvorenie skupiny
        /// </summary>
        /// <param name="field">Nazov stlpca</param>
        /// <param name="Sort">Sposob usporiadania</param>
        /// <param name="Collapsed">Rozbalenie/zbalenie skupiny</param>
        public PfeGroupAttribute(PfeOrder sort = PfeOrder.Asc, bool collapsed = false)
        { 
            this.Sort = sort;
            this.Collapsed = collapsed;
        }

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
        /// Indikacia zbalenej/rozbalenej skupiny
        /// </summary>        
        [DataMember(Name = "col")]
        public bool Collapsed { get; set; }

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
                result = result * 23 + this.Sort.GetHashCode();
                result = result * 23 + this.Collapsed.GetHashCode();
                return result;
            }
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(PfeGroupAttribute other)
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
                   this.Sort.Equals(other.Sort) &&
                   this.Collapsed == other.Collapsed;
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
            PfeGroupAttribute temp = obj as PfeGroupAttribute;
            if (temp == null)
            {
                return false;
            }
            return this.Equals(temp);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("Field: {0}, Sort: {1}, Collapsed: {2}", this.Field, this.Sort, this.Collapsed);
        }
    }
}