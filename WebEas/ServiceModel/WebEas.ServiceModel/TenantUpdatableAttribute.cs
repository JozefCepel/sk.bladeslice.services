using System;
using System.Linq;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// Oznaci entitu, ze sa updatuje aj tenant
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class TenantUpdatableAttribute : Attribute
    {
    }
}