using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [DataContract]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Field | AttributeTargets.GenericParameter | AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    public class PfeLayoutDependencyAttribute : Attribute
    {
    }
}