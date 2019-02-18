using System;
using System.Linq;

namespace WebEas.ServiceModel
{ 
    /// <summary>
    /// Oznaci entitu, ze sa updatuje aj tenant
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class IgnoreInsertOrUpdateAttribute : Attribute
    {
    }
}