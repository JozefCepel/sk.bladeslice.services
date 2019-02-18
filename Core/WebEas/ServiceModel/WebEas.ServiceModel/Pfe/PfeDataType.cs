using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// Datovy typ
    /// </summary>
    [DataContract(Name = "DataType")]
    public enum PfeDataType
    {
        [EnumMember(Value = "0")]
        Default = 0,
        [EnumMember(Value = "1")]
        Boolean = 1,
        [EnumMember(Value = "2")]
        Text = 2,
        [EnumMember(Value = "3")]
        Number = 3,
        [EnumMember(Value = "4")]
        Date = 4,
        [EnumMember(Value = "5")]
        Time = 5,
        [EnumMember(Value = "6")]
        DateTime = 6,
        [EnumMember(Value = "-1")]
        Unknown = -1
    }
}