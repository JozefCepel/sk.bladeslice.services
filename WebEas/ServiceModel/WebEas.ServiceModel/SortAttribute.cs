using System;

namespace WebEas.ServiceModel
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public class SortAttribute : Attribute
    {
        /// <summary>
        /// Konštruktor pre vytvorenie usporiadania
        /// </summary>
        /// <param name="field">Nazov stlpca</param>
        /// <param name="Sort">Sposob usporiadania, true v pripade Desc</param>
        public SortAttribute(string field, bool descSort = false)
        {
            this.Field = field;
            this.DescSort = descSort;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortAttribute" /> class.
        /// </summary>
        /// <param name="sort">The sort.</param>
        public SortAttribute(bool descSort = false)
        {
            this.DescSort = descSort;
        }

        /// <summary>
        /// Gets or sets the rank.
        /// </summary>
        /// <value>The rank.</value>
        public int Rank { get; set; }

        /// <summary>
        /// Nazov stlpca
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// Descending sort 
        /// </summary>
        public bool DescSort { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("Field: {0}, Sort: {1}", this.Field, this.DescSort ? "DESC" : "ASC");
        }
    }
}

