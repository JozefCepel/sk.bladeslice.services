using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;
using System.Xml.Serialization;

namespace WebEas.ServiceModel.Office.Egov.Reg.Types
{
    [DataContract]
    public class AdresnyStitokBaseView
    {
        [DataMember]
        public long ID { get; set; }

        [DataMember]
        public string CpvRok { get; set; }

        [DataMember]
        public string Oslovenie { get; set; }

        [DataMember]
        public string VS { get; set; }

        [DataMember]
        public string AdresnyRiadok1 { get; set; }

        [DataMember]
        public string AdresnyRiadokCastObce1 { get; set; }

        [DataMember]
        public string Adresat { get; set; }

        [DataMember]
        public string AdresnyRiadok2 { get; set; }

        [DataMember]
        public string Stat { get; set; }

        [DataMember]
        public string Adresa { get; set; }

        [DataMember]
        public string FormatovaneMenoSort { get; set; }

        [DataMember]
        public string UradAdresnyRiadok1 { get; set; }

        [DataMember]
        public string UradAdresnyRiadok2 { get; set; }

        [DataMember]
        public string UradAdresnyRiadok3 { get; set; }

        [DataMember]
        public string UradNazov { get; set; }

        [DataMember]
        public string UradAdresa { get; set; }

        [DataMember]
        public string Druh { get; set; }

        [DataMember]
        public string Rok { get; set; }

        [DataMember]
        public string VarSymbol { get; set; }

        [DataMember]
        public string SpisCisloZapisu { get; set; }

        [DataMember]
        public string UradnyDokumentRegistraturneCislo { get; set; }

        [DataMember]
        public string Identifikator { get; set; }
  }

    [Schema("reg")]
    [Alias("V_AdresnyStitokVyzva")]
    [DataContract]
    [XmlType("Label")]
    public class AdresnyStitokVyzvaView : AdresnyStitokBaseView
    {
    }

    [Schema("reg")]
    [Alias("V_AdresnyStitokVymer")]
    [DataContract]
    [XmlType("Label")]
    public class AdresnyStitokVymerView : AdresnyStitokBaseView
    {
    }
}