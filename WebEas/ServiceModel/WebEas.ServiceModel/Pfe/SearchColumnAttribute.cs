using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{ 
    /// <summary>
    /// Definuje ktore properties budu pouzite vo filtri pri selecte z db
    /// </summary>
    [DataContract]
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class SearchColumnsAttribute : Attribute
    {
        public SearchColumnsAttribute(params string[] cols)
        {
            this.Columns = cols.ToList();
        }

        /// <summary>
        /// Obsahuje aliasy stlpca, podla ktorych sa bude filtrovat hodnota zadana v tomto atribute
        /// </summary>
        public List<string> Columns { get; set; }
    }
}