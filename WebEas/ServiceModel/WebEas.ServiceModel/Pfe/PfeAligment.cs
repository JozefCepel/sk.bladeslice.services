using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// Zarovnanie stlpca
    /// </summary>
    [DataContract(Name = "Aligment")]
    public enum PfeAligment
    {
        [EnumMember(Value = "0")]
        Default = 0,
        [EnumMember(Value = "1")]
        Right = 1,
        [EnumMember(Value = "2")]
        Left = 2,
        [EnumMember(Value = "3")]
        Center = 3,
        [EnumMember(Value = "-1")]
        Unknown=-1
    }    
}