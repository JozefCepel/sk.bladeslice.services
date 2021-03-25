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
    public class TypBiznisEntityView : TypBiznisEntity, IBaseView
    {
        [DataMember]
        [PfeColumn(Text = "Stavový priestor", ReadOnly = true)]
        [PfeCombo(typeof(StavovyPriestor), ComboIdColumn = nameof(StavovyPriestor.C_StavovyPriestor_Id))]
        public string C_StavovyPriestor_Nazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "_KodNazov")]
        public string KodNazov { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
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
        public short C_TypBiznisEntity_Id { get; set; }

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

    [Flags]
    public enum TypBiznisEntityEnum
    {
        Unknown = -999,
        IND    = 1,  // Interny Doklad
        DFA    = 2,  // Dodavatelska Faktura
        OFA    = 3,  // Odberatelska Faktura
        PDK    = 4,  // Pokladnicny Doklad
        PRI    = 5,  // Prijemka
        VYD    = 6,  // Vydajka
        PRE    = 7,  // Prevodka
        BAN    = 8,  // Bankovy Vypis
        ODP    = 9,  // Odberatelska Dopyt
        DDP    = 10, // Dodavatelsky Dopyt
        OCP    = 11, // Odberatelska Cenova Ponuka
        DCP    = 12, // Dodavatelska Cenova Ponuka
        OOB    = 13, // Odberatelska Objednavka
        DOB    = 14, // Dodavatelska Objednavka
        OZM    = 15, // Odberatelska Zmluva
        DZM    = 16, // Dodavatelska Zmluva
        OZF    = 17, // Odberatelska Zalohova Faktura
        DZF    = 18, // Dodavatelska Zalohova Faktura
        PPP    = 19, // Platobny Prikaz
        DOL    = 20, // Dodaci List
        DAP    = 50  //Rozhodnutie - dane a poplatky
    }
}