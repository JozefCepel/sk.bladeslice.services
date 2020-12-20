using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// Enumerator pre typ spravy
    /// </summary>
    [DataContract(Name = "PfeMessageType")]
    public enum PfeMessageType
    {
        /// <summary>
        /// MessageWarning (+ doplnkové pole "msg") - pred uložením (zatlačenie tlačítka uloženia) sa podmienka vyhodnotí a ak platí, tak sa zobrazi warning dialóg Ok/Cancel (Ak OK - tak uloží; ak Cancel tak sa neuloží a editácia zostane aktívna)
        /// </summary>
        [EnumMember(Value = "MessageWarning")]
        MessageWarning,

        /// <summary>
        /// MessageError (+ doplnkové pole "msg") - pred uložením (zatlačenie tlačítka uloženia) sa podmienka vyhodnotí a ak platí, tak sa zobrazi error dialóg s tlačítko Ok a nedovolí uložiť a editácia zostane aktívna. 
        /// </summary>
        [EnumMember(Value = "MessageError")]
        MessageError,

        /// <summary>
        /// MessageInfo (+ doplnkové pole "msg") - pred uložením (zatlačenie tlačítka uloženia) sa podmienka vyhodnotí a ak platí, tak sa zobrazi informačný dialóg s tlačítko Ok. Zatlačenie neobmedzuje uloženie, iba zobrazuje informačnú správu. 
        /// </summary>
        [EnumMember(Value = "MessageInfo")]
        MessageInfo
    }
}
