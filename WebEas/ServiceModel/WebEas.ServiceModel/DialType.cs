using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// Typ číselníka
    /// </summary>
    [DataContract]
    public enum DialType : short
    {
        /// <summary>
        /// Statický číselník - Enumeracia definovaných hodnôt - pevných
        /// </summary>
        [EnumMember(Value = "STATIC")]
        Static = 1,
        /// <summary>
        /// Lokálný číselník
        /// </summary>
        [EnumMember(Value = "LOCAL")]
        Local = 2,
        /// <summary>
        /// Globálný číselník
        /// </summary>
        [EnumMember(Value = "GLOBAL")]
        Global = 3,
        /// <summary>
        /// Obsahuje lokálne aj globálne hodnoty
        /// </summary>
        [EnumMember(Value = "HYBRID")]
        Hybrid = 4,
        /// <summary>
        /// Neznámy typ číselníka
        /// </summary>
        [EnumMember(Value = "UNKNOWN")]
        Unknown = -1
    }
}