using System;
using System.Linq;

namespace WebEas.ServiceModel
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]    
    public class UseThisTypeAttribute : Attribute
    {
    }
}