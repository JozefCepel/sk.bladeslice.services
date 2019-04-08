using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel.Office.Egov.Reg.Types;

namespace WebEas.ServiceModel.Reg.Types
{
    [Schema("reg")]
    [Alias("V_StavEntity")]
    [DataContract]
    public class StavEntityView : BaseEntity
    {
        /// <summary>
        /// Gets or sets the c_staventity_id.
        /// </summary>
        /// <value>The c_staventity_id.</value>
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public int C_StavEntity_Id { get; set; }

        /// <summary>
        /// Gets or sets the c_StavovyPriestor_id.
        /// </summary>
        /// <value>The c_StavovyPriestor_id.</value>
        [DataMember]
        [PfeSort(Rank = 1)]
        public int C_StavovyPriestor_Id { get; set; }

        /// <summary>
        /// Gets or sets the nazov.
        /// </summary>
        /// <value>The nazov.</value>
        [DataMember]
        [PfeColumn(Text = "Name", Rank = 3, Width = 220)]
        [PfeValueColumn]
        [Required]
        [StringLength(255)]
        public string Nazov { get; set; }

        /// <summary>
        /// Gets or sets the nazov stavovy priestor.
        /// </summary>
        /// <value>The nazov stavovy priestor.</value>
        [DataMember]
        [PfeColumn(Text = "Oblasť", Rank = 4, Width = 200)]
        [PfeCombo(typeof(StavovyPriestor), IdColumnCombo = "C_StavovyPriestor_Id")]
        public string NazovStavovyPriestor { get; set; }

        /// <summary>
        /// Gets or sets the Kod.
        /// </summary>
        /// <value>The Kod.</value>
        [DataMember]
        [PfeColumn(Text = "Code", Rank = 5, Width = 220)]
        [Required]
        [StringLength(50)]
        public string Kod { get; set; }

        /// <summary>
        /// Gets or sets the Strom.
        /// </summary>
        /// <value>The Strom.</value>
        [DataMember]
        [PfeColumn(Text = "Strom", Rank = 6, Width = 220)]
        [StringLength(50)]
        public string Strom { get; set; }

        /// <summary>
        /// Gets or sets the D_Formular_Id.
        /// </summary>
        /// <value>The D_Formular_Id.</value>
        [DataMember]
        public int C_Formular_Id { get; set; }

        /// <summary>
        /// Gets or sets the d_ formular_ nazov.
        /// </summary>
        /// <value>The d_ formular_ nazov.</value>
        [DataMember]
        [PfeColumn(Text = "Formulár šablóny", Rank = 6, Width = 180)]
        [PfeCombo(typeof(Formular), IdColumnCombo = "C_Formular_Id", DisplayColumn = "Nazov", AdditionalWhereSql = "VstupnyFormular=0")]
        public string C_Formular_Nazov { get; set; }

        /// <summary>
        /// Gets or sets the PociatocnyStav.
        /// </summary>
        /// <value>The PociatocnyStav.</value>
        [DataMember]
        [PfeColumn(Text = "Počiatočný stav", Rank = 7)]
        public bool JePociatocnyStav { get; set; }

        /// <summary>
        /// Gets or sets the KoncovyStav.
        /// </summary>
        /// <value>The KoncovyStav.</value>
        [DataMember]
        [PfeColumn(Text = "Koncový stav", Rank = 8)]
        public bool JeKoncovyStav { get; set; }

        /// <summary>
        /// Gets or sets the JeKladneVybavenie.
        /// </summary>
        /// <value>The JeKladneVybavenie.</value>
        [DataMember]
        [PfeColumn(Text = "Kladné vybavenie", Rank = 9)]
        public bool JeKladneVybavenie { get; set; }

        /// <summary>
        /// Gets or sets the Textacia.
        /// </summary>
        /// <value>The Textacia.</value>
        [DataMember]
        [PfeColumn(Text = "Textácia", Rank = 10, Width = 220)]
        [StringLength(512)]
        public string Textacia { get; set; }

        /// <summary>
        /// Gets or sets the BiznisAkcia.
        /// </summary>
        /// <value>The BiznisAkcia.</value>
        [DataMember]
        [PfeColumn(Text = "Biznis akcia", Rank = 11, Width = 130)]
        [StringLength(255)]
        public string BiznisAkcia { get; set; }

        /// <summary>
        /// Gets or sets the PovinnyDokument.
        /// </summary>
        /// <value>The PovinnyDokument.</value>
        [DataMember]
        [PfeColumn(Text = "Povinný dokument", Rank = 12)]
        public bool PovinnyDokument { get; set; }

        //[Ignore]
        //[PfeIgnore]
        //public List<StavEntityView> NasledovneStavy { get; set; }
    }
}
