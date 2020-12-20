using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// Konfiguracia pre zobrazenie xtype SearchFieldSS a SearchFieldMS
    /// </summary>
    [DataContract(Name = "sfd")]
    public class PfeSearchFieldDefinition : IEquatable<PfeSearchFieldDefinition>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PfeSearchFieldDefinition" /> class.
        /// </summary>
        public PfeSearchFieldDefinition()
        {
        }

        public PfeSearchFieldDefinition(List<PfeFilterAttribute> condition, string code, string nameField, string displayField)
        {
            Condition = condition;
            Code = code;
            NameField = nameField;
            DisplayField = displayField;
        }

        /// <summary>
        /// Podmienka na zaklade ktorej sa vyhodnocuje. - cd
        /// </summary>
        [DataMember(Name = "cd")]
        public List<PfeFilterAttribute> Condition { get; set; }

        /// <summary>
        /// Kod polozky - code
        /// </summary>
        [DataMember(Name = "code")]
        public string Code { get; set; }

        /// <summary>
        /// Field, ktory zobrat z Browsera a vlozit do 'Id'.
        /// </summary>
        [DataMember(Name = "nfd")]
        public string NameField { get; set; }

        /// <summary>
        /// Field, ktory zobrat z Browsera a vlozit do textu.
        /// </summary>
        [DataMember(Name = "dfd")]
        public string DisplayField { get; set; }

        /// <summary>
        /// Aplikuje dany filter na pohlad
        /// </summary>
        [DataMember(Name = "afs")]
        public string AdditionalFilterSql { get; set; }

        /// <summary>
        /// Popis aplikovaneho filtra na pohlad
        /// </summary>
        [DataMember(Name = "afd")]
        public string AdditionalFilterDesc { get; set; }

        /// <summary>
        /// Input search field
        /// </summary>
        [DataMember(Name = "isf")]
        public string InputSearchField { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $" Condition: {Condition}, Code: {Code}, NameField: {NameField}, DisplayField: {DisplayField}, AdditionalFilterSql: {AdditionalFilterSql}, AdditionalFilterDesc: {AdditionalFilterDesc}, InputSearchField: {InputSearchField}";
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = 17;
                result = result * 23 + ((Condition != null) ? Condition.GetHashCode() : 0);
                result = result * 23 + ((Code != null) ? Code.GetHashCode() : 0);
                result = result * 23 + ((NameField != null) ? NameField.GetHashCode() : 0);
                result = result * 23 + ((DisplayField != null) ? DisplayField.GetHashCode() : 0);
                result = result * 23 + ((AdditionalFilterSql != null) ? AdditionalFilterSql.GetHashCode() : 0);
                result = result * 23 + ((AdditionalFilterDesc != null) ? AdditionalFilterDesc.GetHashCode() : 0);
                result = result * 23 + AdditionalFilterDesc.GetHashCode();
                return result;
            }
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(PfeSearchFieldDefinition other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(Condition, other.Condition) &&
                   Equals(Code, other.Code) &&
                   Equals(NameField, other.NameField) &&
                   Equals(DisplayField, other.DisplayField) &&
                   Equals(AdditionalFilterSql, other.AdditionalFilterSql) &&
                   Equals(AdditionalFilterDesc, other.AdditionalFilterDesc) &&
                   Equals(InputSearchField, other.InputSearchField);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise,
        /// false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is PfeSearchFieldDefinition temp))
            {
                return false;
            }
            return Equals(temp);
        }
    }
}