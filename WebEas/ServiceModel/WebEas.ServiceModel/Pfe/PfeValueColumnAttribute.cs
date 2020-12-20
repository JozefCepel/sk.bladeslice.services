using System;
using System.Linq;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class PfeValueColumnAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PfeValueColumnAttribute" /> class.
        /// </summary>
        public PfeValueColumnAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PfeValueColumnAttribute" /> class.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        public PfeValueColumnAttribute(string sql)
        {
            this.Sql = sql;
        }

        /// <summary>
        /// Gets or sets the SQL.
        /// </summary>
        /// <value>The SQL.</value>
        public string Sql { get; private set; }

        /// <summary>
        /// Determines whether this instance has SQL.
        /// </summary>
        /// <returns></returns>
        public bool HasSql()
        {
            return !String.IsNullOrEmpty(this.Sql);
        }
    }
}