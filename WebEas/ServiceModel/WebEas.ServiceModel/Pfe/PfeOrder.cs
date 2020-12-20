using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// Moznosti usporiadania
    /// </summary>
    [DataContract(Name = "Order")]
    public enum PfeOrder
    {
        [EnumMember(Value = "1")]
        Asc = 1,
        [EnumMember(Value = "2")]
        Desc = 2
    }    
}