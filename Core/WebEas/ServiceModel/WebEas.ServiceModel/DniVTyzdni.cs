using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [DataContract]
    public enum DniVTyzdni:byte
    {
        [EnumMember(Value = "Pondelok")]
        Monday = 1,
        [EnumMember(Value = "Utorok")]
        Tuesday = 2,
        [EnumMember(Value = "Streda")]
        Wednesday = 3,
        [EnumMember(Value = "Štvrtok")]
        Thursday = 4,
        [EnumMember(Value = "Piatok")]
        Friday = 5,
        [EnumMember(Value = "Sobota")]
        Saturday = 6,
        [EnumMember(Value = "Nedeľa")]
        Sunday = 7
    }
}
