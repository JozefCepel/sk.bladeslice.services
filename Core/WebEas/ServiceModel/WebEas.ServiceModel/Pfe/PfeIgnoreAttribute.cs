using System;
using System.Linq;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// Ignorovat pri vytváraní modelu
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]    
    public class PfeIgnoreAttribute : Attribute
    {
    }
}