using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Osa
{
    [Schema("osa")]
    [Alias("V_OsobaTPSidloCombo")]
    [DataContract]
    public class OsobaTPSidloComboView
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        public long D_Osoba_Id { get; set; }

        [DataMember]
        public short C_OsobaTyp_Id { get; set; }

        [DataMember]
        [PfeValueColumn]
        public string Identifikator { get; set; }

        [DataMember]
        public string FormatMenoSort { get; set; }

        [DataMember]
        public string IdentifikatorMeno { get; set; }

        [DataMember]
        public short? FakturaciaSplatnost { get; set; }

        [DataMember]
        public string AdresaDlhaByt { get; set; }
    }
}