using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [DataContract]
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class HierarchyNodeParameterAttribute : Attribute
    {
    }
}