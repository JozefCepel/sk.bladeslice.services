using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// Enumerator pre typ pohladu
    /// </summary>
    [DataContract(Name = "LayoutWidgetType")]
    public enum PfeLayoutWidgetType
    {
        [EnumMember(Value = "0")]
        None = 0,
        [EnumMember(Value = "1")]
        Container = 1,
        [EnumMember(Value = "2")]
        View = 2,
    }
}