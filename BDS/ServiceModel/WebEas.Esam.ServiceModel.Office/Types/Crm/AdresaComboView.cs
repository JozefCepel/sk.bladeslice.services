using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Crm
{
    [Schema("crm")]
    [Alias("V_AdresaCombo")]
    [DataContract]
    public class AdresaComboView
    {
        [DataMember]
        [PrimaryKey]
        [PfeColumn(Text = "D_ADR_Adresa_Id")]
        public long D_ADR_Adresa_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_Osoba_Id")]
        public long D_Osoba_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "C_ADR_Typ_Id")]
        public long C_ADR_Typ_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "AdresaCombo")]
        public string AdresaCombo { get; set; }

    }
}
