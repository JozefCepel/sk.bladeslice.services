using System;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [DataContract(Name = "Flag")]
    [Flags]
    public enum HierarchyNodeFlag
    {
        [EnumMember(Value = "None")]
        None = 0,

        [EnumMember(Value = "hact")]
        HasActions = 1 << 0,

        [EnumMember(Value = "leaf")]
        Leaf = 1 << 1,
    }
}