using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;
using WebEas.ServiceModel.Reg.Types;

namespace WebEas.ServiceModel.Office.Egov.Reg.Types
{
    [Schema("reg")]
    [Alias("V_TypBiznisEntity")]
    [DataContract]
    public class TypBiznisEntityView : TypBiznisEntity
    {
        [DataMember]
        [PfeColumn(Text = "Oblasť", Width = 200, ReadOnly = true)]
        [PfeCombo(typeof(StavovyPriestor), IdColumnCombo = "C_StavovyPriestor_Id")]
        public string C_StavovyPriestor_Nazov { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true)]
        public string ZmenilMeno { get; set; }
    }

    [Schema("reg")]
    [Alias("C_TypBiznisEntity")]
    [DataContract]
    [Dial(DialType.Global, DialKindType.BackOffice)]
    public class TypBiznisEntity : BaseEntity
    {
        /// <summary>
        /// Gets or sets the C_TypBiznisEnity_Id.
        /// </summary>
        /// <value>The C_TypBiznisEnity_Id.</value>
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]        
        public int C_TypBiznisEntity_Id { get; set; }

        /// <summary>
        /// Gets or sets the C_StavovyPriestor_Id.
        /// </summary>
        /// <value>The C_StavovyPriestor_Id.</value>
        [DataMember]        
        [PfeSort(Rank = 1)]
        public int C_StavovyPriestor_Id { get; set; }

        /// <summary>
        /// Gets or sets the nazov.
        /// </summary>
        /// <value>The nazov.</value>
        [DataMember]
        [PfeColumn(Text = "Typ dokladu", Width = 200)]
        [PfeSort(Rank = 2)]
        public string Nazov { get; set; }
    }

    public enum TypBiznisEntityEnum
    {
        Unknown = -999,

        #region RZP biznis entity

        RZP_IntDoklad = 1,

        #endregion RZP biznis entity
    }
}