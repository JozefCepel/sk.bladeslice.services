using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace WebEas.ServiceModel.Office.Egov.Reg.Types
{
    [Schema("dap")]
    [Alias("V_PostovaPoukazkaVymerSplatky")]
    [DataContract]
    [XmlType("Label")]
    public class PostovaPoukazkaVymerSplatkyView : PostovaPoukazkaBaseView
    {
    }

    [Schema("dap")]
    [Alias("V_PostovaPoukazkaVyzva")]
    [DataContract]
    [XmlType("Label")]
    public class PostovaPoukazkaVyzvaView: PostovaPoukazkaBaseView
    {
    }

    [Schema("dap")]
    [Alias("V_PostovaPoukazkaVymer")]
    [DataContract]
    [XmlType("Label")]
    public class PostovaPoukazkaVymerView: PostovaPoukazkaBaseView
    {
    }

    [DataContract]
    public class PostovaPoukazkaBaseView
    {
        [DataMember]
        [PrimaryKey]
        public long ID { get; set; }

        [DataMember]
        public string VymerDan { get; set; }

        [DataMember]
        public string VymerDan2 { get; set; }

        [DataMember]
        public string VymerDanSlovom { get; set; }

        [DataMember]
        public string VymerDanCenty { get; set; }

        [DataMember]
        public int? VymerDanEura { get; set; }

        [DataMember]
        public string BankaIban { get; set; }

        [DataMember]
        public string VS { get; set; }

        [DataMember]
        public string Adresat { get; set; }

        [DataMember]
        public string BarCodePostovaPoukazka { get; set; }

        [DataMember]
        public string KodSpracovania { get; set; }

        [DataMember]
        public string KS { get; set; }

        [DataMember]
        public string AdresatPriezvisko { get; set; }

        [DataMember]
        public string AdresatMeno { get; set; }

        [DataMember]
        public string AdresatUlicaPOBox { get; set; }

        [DataMember]
        public string AdresatOnlyObec { get; set; }

        [DataMember]
        public string AdresatCislo { get; set; }

        [DataMember]
        public string AdresatPSC { get; set; }

        [DataMember]
        public string AdresnyRiadok1 { get; set; }

        [DataMember]
        public string AdresnyRiadok2 { get; set; }

        [DataMember]
        public string AdresnyRiadok3 { get; set; }

        [DataMember]
        public string AdresnyRiadok3PSC { get; set; }

        [DataMember]
        public string AdresnyRiadok3PostaObec { get; set; }

        [DataMember]
        public string Adresa { get; set; }

        [DataMember]
        public string UradAdresnyRiadok1 { get; set; }

        [DataMember]
        public string UradAdresnyRiadok2 { get; set; }

        [DataMember]
        public string UradAdresnyRiadok3 { get; set; }

        [DataMember]
        public string UradAdresa { get; set; }

        [DataMember]
        public string UradNazov { get; set; }

        [DataMember]
        public string SpravaPreAdresata { get; set; }
  }
}
