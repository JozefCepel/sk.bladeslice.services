using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// Typ väzby 1:1, alebo 1:N
    /// </summary>
    [DataContract(Name = "RelationType")]
    public enum PfeRelationType
    {
        [EnumMember(Value = "ONETOONE")]
        OneToOne = 1,
        [EnumMember(Value = "ONETOMANY")]
        OneToMany = 2
    }
}