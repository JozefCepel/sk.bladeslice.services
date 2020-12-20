using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// Enumerator pre typ validatora
    /// </summary>
    [DataContract(Name = "PfeValidatorType")]
    public enum PfeValidatorType
    {
        /// <summary>
        /// Enable - pole je pri splnenej podmienke Enablované
        /// </summary>
        [EnumMember(Value = "Enable")]
        Enable,

        /// <summary>
        /// Disable - pole je pri splnenej podmienke Disablované
        /// </summary>
        [EnumMember(Value = "Disable")]
        Disable,

        /// <summary>
        /// Visible - pole a jeho popis sú pri splnenej podmienke Visible
        /// </summary>
        [EnumMember(Value = "Visible")]
        Visible,

        /// <summary>
        /// Hidden - pole a jeho popis sú pri splnenej podmienke Hidden
        /// </summary>
        [EnumMember(Value = "Hidden")]
        Hidden,

        /// <summary>
        /// SetLabel (+ doplnkové pole "slbl") - nastaví sa do labelu poľa text z atribútu "slbl". Ak hodnota obsahuje v texte názov iného poľa v zobáčikoch <Pole1>, tak sa replacne
        /// </summary>
        [EnumMember(Value = "SetLabel")]
        SetLabel,

        /// <summary>
        /// SetValue (+ doplnkové pole "sval") - nastaví sa do hodnoty poľa  text zo "sval". Ak hodnota obsahuje v texte názov iného poľa v zobáčikoch <Pole1>, tak sa replacne. Ak je v texte <NULL>, tak sa vloží NULL.
        /// </summary>
        [EnumMember(Value = "SetValue")]
        SetValue,

        /// <summary>
        /// SetMin (+ doplnkové pole "smin") - nastaví minimálnu hodnotu poľa z atribútu "smi". Ak hodnota obsahuje v texte názov iného poľa v zobáčikoch <Pole1>, tak sa vloží hodnota z tohto poľa.
        /// </summary>
        [EnumMember(Value = "SetMin")]
        SetMin,

        /// <summary>
        /// SetMax (+ doplnkové pole "smax") - nastaví maximálnu hodnotu poľa z atribútu "smax". Ak hodnota obsahuje v texte názov iného poľa v zobáčikoch <Pole1>, tak sa vloží hodnota z tohto poľa.
        /// </summary>
        [EnumMember(Value = "SetMax")]
        SetMax,

        /// <summary>
        /// SetMandatory - nastaví, že pole je povinné 
        /// </summary>
        [EnumMember(Value = "SetMandatory")]
        SetMandatory,

        /// <summary>
        /// SetNoMandatory - nastaví, že pole je nepovinné 
        /// </summary>
        [EnumMember(Value = "SetNoMandatory")]
        SetNoMandatory
    }
}
