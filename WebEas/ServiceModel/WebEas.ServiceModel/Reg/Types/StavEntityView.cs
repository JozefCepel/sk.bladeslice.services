using ServiceStack.DataAnnotations;
using System.Collections.Generic;
using System.Runtime.Serialization;
using WebEas.ServiceModel.Office.Egov.Reg.Types;

namespace WebEas.ServiceModel.Reg.Types
{
    [Schema("reg")]
    [Alias("V_StavEntity")]
    [DataContract]
    public class StavEntityView : BaseEntity, IPfeCustomizeCombo
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public int C_StavEntity_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov")]
        [PfeValueColumn]
        [Required]
        [StringLength(255)]
        public string Nazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kód")]
        [Required]
        [StringLength(50)]
        public string Kod { get; set; }

        [DataMember]
        [PfeColumn(Text = "Strom")]
        [StringLength(50)]
        public string Strom { get; set; }

        [DataMember]
        public int? C_Formular_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Formulár šablóny")]
        [PfeCombo(typeof(Formular), ComboIdColumn = nameof(Formular.Nazov), AdditionalWhereSql = "VstupnyFormular=0")]
        public string C_Formular_Nazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Počiatočný stav")]
        public bool JePociatocnyStav { get; set; }

        [DataMember]
        [PfeColumn(Text = "Koncový stav")]
        public bool JeKoncovyStav { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kladné vybavenie")]
        public bool JeKladneVybavenie { get; set; }

        [DataMember]
        [PfeColumn(Text = "Textácia")]
        [StringLength(512)]
        public string Textacia { get; set; }

        [DataMember]
        [PfeColumn(Text = "Biznis akcia")]
        [StringLength(255)]
        public string BiznisAkcia { get; set; }

        [DataMember]
        [PfeColumn(Text = "Povinný dokument")]
        public bool PovinnyDokument { get; set; }

        public void ComboCustomize(IWebEasRepositoryBase repository, string column, string kodPolozky, Dictionary<string, string> requiredFields, ref PfeComboAttribute comboAttribute)
        {
            if (column.ToLower() == "StavNazov".ToLower() && (kodPolozky.StartsWith("crm-dod-")) || kodPolozky.StartsWith("crm-odb-"))
            {
                comboAttribute.AdditionalWhereSql = "C_StavEntity_Id IN (-2, -1, 1, 7, 8, 10, 14, 15)";
            }
            else if(column.ToLower() == "StavNazov".ToLower() && (kodPolozky.StartsWith("fin-pok-") ||
                                                                  kodPolozky.StartsWith("fin-bnk-") ||
                                                                  kodPolozky.StartsWith("all-evi-intd") ||
                                                                  kodPolozky.StartsWith("uct-evi-exd-")))
            {
                comboAttribute.AdditionalWhereSql = "C_StavEntity_Id IN (1, 7, 10, 14, 15)";
            }
        }
    }
}
