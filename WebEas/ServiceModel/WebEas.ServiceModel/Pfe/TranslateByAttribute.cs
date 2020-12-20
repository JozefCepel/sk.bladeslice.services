using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{ 
    /// <summary>
    /// Definuje prekladany stlpec a zaroven informacie, kam patri
    /// </summary>
    [DataContract]
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class TranslateByAttribute : Attribute
    {
        public TranslateByAttribute(string schema, string table, string column)
        {
            this.Schema = schema;
            this.Table = table;
            this.Column = column;
        }

        /// <summary>
        /// Schema, z ktorej sa bude brat Obsahuje aliasy stlpcov, podla ktorych sa bude filtrovat hodnota zadana v tomto atribute
        /// </summary>
        public string Schema { get; set; }

        public string Table { get; set; }

        public string Column { get; set; }
    }
}