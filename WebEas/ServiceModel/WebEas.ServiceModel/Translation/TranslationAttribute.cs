using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// Atribut oznacujuci jazykovo prekladany stlpec
    /// </summary>
    [DataContract]
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class TranslationAttribute : Attribute
    {
    }
}
