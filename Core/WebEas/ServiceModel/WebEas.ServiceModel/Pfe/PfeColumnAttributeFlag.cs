using System;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [DataContract(Name = "Flag")]
    [Flags]
    public enum PfeColumnAttributeFlag
    {
        [EnumMember(Value = "none")]
        None = 0,

        [EnumMember(Value = "editable")]
        Editable = 1 << 0,

        [EnumMember(Value = "readonly")]
        ReadOnly = 1 << 1,

        [EnumMember(Value = "mandatory")]
        Mandatory = 1 << 2,

        [EnumMember(Value = "hidden")]
        Hidden = 1 << 3,

        [EnumMember(Value = "hideable")]
        Hiddeable = 1 << 4,

        [EnumMember(Value = "sortable")]
        Sortable = 1 << 5,

        [EnumMember(Value = "filterable")]
        Filterable = 1 << 6
    }
}