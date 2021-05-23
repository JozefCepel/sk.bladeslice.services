using ServiceStack.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Osa
{
    [Schema("osa")]
    [Alias("V_Osoba")]
    [DataContract]
    public class OsobaView : Osoba, IBaseView, IOsobaExt, IPfeCustomizeCombo
    {
        [DataMember]
        [PfeColumn(Text = "Poznámka", ReadOnly = true)]
        public new string Poznamka { get; set; }

        [DataMember]
        [PfeColumn(Text = "Externé ISO ID osoby", ReadOnly = true, Editable = false)]
        public long? D_Osoba_Id_ExterneISO { get; set; }

        [DataMember]
        [PfeColumn(Text = "RČ / IČO", ReadOnly = true, Editable = false)]
        [StringLength(20)]
        [PfeValueColumn]
        public string Identifikator { get; set; }

        [DataMember]
        [PfeColumn(Text = "Formát. meno", ReadOnly = true, Editable = false)]
        [StringLength(460)]
        public string FormatMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Osoba", ReadOnly = true, Editable = false)]
        [StringLength(460)]
        public string FormatMenoSort { get; set; }

        [DataMember]
        [PfeColumn(Text = "Formát. meno (komplet)", ReadOnly = true, Editable = false)]
        [StringLength(460)]
        public string FormatMenoFull { get; set; }

        [DataMember]
        [PfeColumn(Text = "_IdentifikatorMeno", ReadOnly = true, Editable = false)] //Pomocné na combo polia
        public string IdentifikatorMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "_IdFormatMeno", ReadOnly = true, Editable = false)] //Pomocné na combo pole
        public string IdFormatMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "TP / Sídlo", Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string AdresaTPSidlo { get; set; }

        [DataMember]
        [PfeColumn(Text = "Typ osoby", ReadOnly = true)]
        public string OsobaTypKod { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_FakturaciaVztah_Id")]
        public short C_FakturaciaVztah_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Fakt.vzťah")]
        [PfeCombo(typeof(FakturaciaVztahCombo), IdColumn = nameof(C_FakturaciaVztah_Id), ComboDisplayColumn = nameof(FakturaciaVztahCombo.Nazov))]
        [Ignore]
        public string FakturaciaVztah { get { return FakturaciaVztahCombo.GetText(C_FakturaciaVztah_Id); } }

        [DataMember]
        [PfeColumn(Text = "Splatnosť faktúry")]
        public short? FakturaciaSplatnost { get; set; }

        [DataMember]
        [PfeColumn(Text = "Nekontrolovať VS")]
        public bool NekontrolovatVS { get; set; }

        [DataMember]
        [PfeColumn(Text = "Rozsah nespôsobilosti")]
        public string NesposobilyRozsah { get; set; }

        [DataMember]
        [PfeColumn(Text = "Pobyt na obci")]
        public bool PobytNaObci { get; set; }

        [DataMember]
        [PfeColumn(Text = "Príznak 1")]
        public bool Priznak1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "DFA", LoadWhenVisible = true, ReadOnly = true, Editable = false, Hidden = true)]
        public bool? DFA { get; set; }

        [DataMember]
        [PfeColumn(Text = "DZF", LoadWhenVisible = true, ReadOnly = true, Editable = false, Hidden = true)]
        public bool? DZF { get; set; }

        [DataMember]
        [PfeColumn(Text = "OFA", LoadWhenVisible = true, ReadOnly = true, Editable = false, Hidden = true)]
        public bool? OFA { get; set; }

        [DataMember]
        [PfeColumn(Text = "OZF", LoadWhenVisible = true, ReadOnly = true, Editable = false, Hidden = true)]
        public bool? OZF { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_FO_StavExistencny_Id", ReadOnly = true)]
        public short? C_FO_StavExistencny_Id { get; set; }

        #region IOsobaExt

        [DataMember]
        [PfeColumn(Text = "Daňovník", LoadWhenVisible = true, ReadOnly = true, Editable = false, Hidden = true)]
        public bool? Danovnik { get; set; }

        [DataMember]
        [PfeColumn(Text = "Dodávateľ", LoadWhenVisible = true, ReadOnly = true, Editable = false, Hidden = true)]
        public bool? Dodavatel { get; set; }

        [DataMember]
        [PfeColumn(Text = "Odberateľ", LoadWhenVisible = true, ReadOnly = true, Editable = false, Hidden = true)]
        public bool? Odberatel { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zákazník", LoadWhenVisible = true, ReadOnly = true, Editable = false, Hidden = true)]
        public bool? Zakaznik { get; set; }

        [DataMember]
        [PfeColumn(Text = "Neuhr. DaP", LoadWhenVisible = true, ReadOnly = true, Editable = false, Hidden = true)]
        public decimal? DM_Neuhradene_DAP { get; set; }

        [DataMember]
        [PfeColumn(Text = "Neuhr. DFA", LoadWhenVisible = true, ReadOnly = true, Editable = false, Hidden = true)]
        public decimal? DM_Neuhradene_DFA { get; set; }

        [DataMember]
        [PfeColumn(Text = "Neuhr. DZF", LoadWhenVisible = true, ReadOnly = true, Editable = false, Hidden = true)]
        public decimal? DM_Neuhradene_DZF { get; set; }

        [DataMember]
        [PfeColumn(Text = "Neuhr. OFA", LoadWhenVisible = true, ReadOnly = true, Editable = false, Hidden = true)]
        public decimal? DM_Neuhradene_OFA { get; set; }

        [DataMember]
        [PfeColumn(Text = "Neuhr. OZF", LoadWhenVisible = true, ReadOnly = true, Editable = false, Hidden = true)]
        public decimal? DM_Neuhradene_OZF { get; set; }

        [DataMember]
        [PfeColumn(Text = "Saldokonto osoby", LoadWhenVisible = true, ReadOnly = true, Editable = false, Hidden = true)]
        public decimal? DM_Neuhradene { get; set; }

        [DataMember]
        [PfeColumn(Text = "Vysporiadané", LoadWhenVisible = true, ReadOnly = true, Editable = false, Tooltip = "Vysporiadané pohľadávky a záväzky")]
        public bool? P { get; set; }

        #endregion

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string ZmenilMeno { get; set; }

        public void ComboCustomize(IWebEasRepositoryBase repository, string column, string kodPolozky, ref PfeComboAttribute comboAttribute)
        {
            if (column.ToLower() == "idformatmeno" || column.ToLower() == "adresatpsidlo")
            {
                if (kodPolozky.StartsWith("crm-dod-")) // Dodávatelia
                {
                    comboAttribute.AdditionalWhereSql = $"C_FakturaciaVztah_Id IN ({(int)FakturaciaVztahEnum.DOD}, {(int)FakturaciaVztahEnum.DOD_ODB})";
                }
                else if (kodPolozky.StartsWith("crm-odb-")) // Odberatelia
                {
                    comboAttribute.AdditionalWhereSql = $"C_FakturaciaVztah_Id IN ({(int)FakturaciaVztahEnum.ODB}, {(int)FakturaciaVztahEnum.DOD_ODB})";
                }
                else if (kodPolozky.StartsWith("fin-pok-pdk") && column.ToLower() == "idformatmeno") // Pokladničné doklady
                {
                    comboAttribute.Tpl = "{value};{AdresaTPSidlo};daň: {Danovnik};dod: {Dodavatel};odb: {Odberatel}";
                    var additionalFields = new[] { nameof(AdresaTPSidlo), nameof(Danovnik), nameof(Dodavatel), nameof(Odberatel) };
                    comboAttribute.AdditionalFields = comboAttribute.AdditionalFields != null ? comboAttribute.AdditionalFields.Union(additionalFields).Distinct().ToArray() : additionalFields;
                }
                else if (kodPolozky.StartsWith("fin-pok-pdk") && column.ToLower() == "adresatpsidlo") // Pokladničné doklady
                {
                    comboAttribute.Tpl = "{value};{IdFormatMeno};daň: {Danovnik};dod: {Dodavatel};odb: {Odberatel}";
                    var additionalFields = new[] { nameof(IdFormatMeno), nameof(Danovnik), nameof(Dodavatel), nameof(Odberatel) };
                    comboAttribute.AdditionalFields = comboAttribute.AdditionalFields != null ? comboAttribute.AdditionalFields.Union(additionalFields).Distinct().ToArray() : additionalFields;
                }

                //Combo služba sa volá aj v špeciálnom prípade z dialógu P/Z.
                //Vtedy sa volá bez "!" a natvrdo sa má pridať filter na "Zákazník = 1"
                if (kodPolozky.Contains("fin-pok-pdk") && !kodPolozky.Contains("!"))
                {
                    comboAttribute.AdditionalWhereSql += (!string.IsNullOrEmpty(comboAttribute.AdditionalWhereSql) ? " AND " : string.Empty) + nameof(Zakaznik) + " = 1";
                }
            }
        }
    }
}
