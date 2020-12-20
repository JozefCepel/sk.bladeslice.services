using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// Enumerator pre typ pohladu
    /// </summary>
    [DataContract(Name = "LayoutType")]
    public enum PfeLayoutType
    {
        [EnumMember(Value = "0")]
        None = 0,
        [EnumMember(Value = "1")]
        Vertical = 1,
        [EnumMember(Value = "2")]
        Horizontal = 2,
    }
}