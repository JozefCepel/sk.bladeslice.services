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

        [EnumMember(Value = "editable")] //na FE 1
        Editable = 1 << 0,

        [EnumMember(Value = "readonly")] //na FE 2
        ReadOnly = 1 << 1,

        [EnumMember(Value = "mandatory")] //na FE 4
        Mandatory = 1 << 2,

        [EnumMember(Value = "hidden")] //na FE 8
        Hidden = 1 << 3,

        [EnumMember(Value = "hideable")] //na FE 16
        Hiddeable = 1 << 4,

        [EnumMember(Value = "sortable")] //na FE 32
        Sortable = 1 << 5,

        [EnumMember(Value = "filterable")] //na FE 64
        Filterable = 1 << 6,

        [EnumMember(Value = "singlecombofilter")] //na FE 128
        SingleComboFilter = 1 << 7,

        [EnumMember(Value = "allowcombocustomvalue")] //na FE 256
        AllowComboCustomValue = 1 << 8,

        [EnumMember(Value = "nullable")] //na FE 512
        Nullable = 1 << 9,

        [EnumMember(Value = "loadwhenvisible")] //na FE 1024
        LoadWhenVisible = 1 << 10,

        [EnumMember(Value = "searchcombofromleft")] //na FE 2048
        SearchComboFromLeft = 1 << 11,
    }
}