using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// Moznosti usporiadania
    /// </summary>
    [DataContract(Name = "Selection")]
    public enum PfeSelection
    {
        [EnumMember(Value = "1")]
        Single = 1,
        [EnumMember(Value = "2")]
        Multi = 2
    }    
}