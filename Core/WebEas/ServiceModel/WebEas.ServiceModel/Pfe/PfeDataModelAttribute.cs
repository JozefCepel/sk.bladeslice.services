using System;
using System.Linq;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]    
    public class PfeDataModelAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PfeDataModelAttribute" /> class.
        /// </summary>
        public PfeDataModelAttribute()
        {
            this.Type = PfeModelType.None;
            this.RowFilterEnabled = false;
            this.MultiSortEnabled = false;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public PfeModelType Type { get; set; }

        /// <summary>
        /// Gets or sets the row filter enabled.
        /// </summary>
        /// <value>The row filter enabled.</value>
        public bool RowFilterEnabled { get; set; }

        /// <summary>
        /// Gets or sets the multi sort enabled.
        /// </summary>
        /// <value>The multi sort enabled.</value>
        public bool MultiSortEnabled { get; set; }
    }
}