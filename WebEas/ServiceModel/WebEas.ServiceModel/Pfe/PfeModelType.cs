using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{ 
    /// <summary>
    /// Enumerator pre typ pohladu
    /// </summary>
    [DataContract(Name = "ModelType")]
    public enum PfeModelType
    {
        [EnumMember(Value = "0")]
        None = 0,
        [EnumMember(Value = "1")]
        Grid = 1,
        [EnumMember(Value = "2")]
        Form = 2,
        [EnumMember(Value = "3")]
        Layout = 3,
        [EnumMember(Value = "4")]
        Map = 4,
        [EnumMember(Value = "5")]
        Graph = 5,
        [EnumMember(Value = "6")]
        Pivot = 6
    }    
}