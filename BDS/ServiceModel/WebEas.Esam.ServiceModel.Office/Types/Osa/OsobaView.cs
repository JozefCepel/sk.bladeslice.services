using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Osa
{
    [Schema("osa")]
    [Alias("V_Osoba")]
    [DataContract]
    public class OsobaView : Osoba, IBaseView
    {
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
        [PfeColumn(Text = "Formát. meno (priezv.)", ReadOnly = true, Editable = false)]
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
        [PfeColumn(Text = "TP/Sídlo", Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string AdresaTPSidlo { get; set; }

        [DataMember]
        [PfeColumn(Text = "Typ osoby")]
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
        [PfeColumn(Text = "_C_FO_StavExistencny_Id", ReadOnly = true)]
        public short? C_FO_StavExistencny_Id { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string ZmenilMeno { get; set; }
    }
}
