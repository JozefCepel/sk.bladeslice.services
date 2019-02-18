using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// Druh číselníka
    /// </summary>
    [DataContract]
    public enum DialKindType:short
    {
        /// <summary>
        /// Možnosť vybrať len z dostupných hodnôt
        /// </summary>
        [EnumMember(Value = "A")]
        ARadio = 1,
        /// <summary>
        /// Ak neexistuje záznam automatický sa vytvorí
        /// </summary>
        [EnumMember(Value = "B")]       
        BNewAvailable = 2,
        /// <summary>
        /// Všetky neexistujúce sa priradia pod jednotný kód -999
        /// </summary>
        [EnumMember(Value = "C")]
        CNewSpecialCode = 3,
        /// <summary>
        /// Back office ciselnik
        /// </summary>
        [EnumMember(Value = "BackOffice")]        
        BackOffice = 4,
        /// <summary>
        /// Statický číselník
        /// </summary>
        [EnumMember(Value = "S")]
        Static = 5,
        /// <summary>
        /// Neznámy druh
        /// </summary>
        [EnumMember(Value = "UNKNOWN")]
        Unknown = -1      
    }
}