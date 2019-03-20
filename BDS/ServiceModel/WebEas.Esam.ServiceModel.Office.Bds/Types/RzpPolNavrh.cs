using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Rzp.Types
{
    [Schema("rzp")]
    [Alias("D_RzpPolNavrh")]
    [DataContract]
    public class RzpPolNavrh : BaseTenantEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public long D_RzpPolNavrh_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_Rzp_Id", Mandatory = true)]
        public long D_Rzp_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_RzpPol_Id")]
        public long? C_RzpPol_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_Program_Id")]
        public long? D_Program_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_Stredisko_Id")]
        public long? C_Stredisko_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_Projekt_Id")]
        public long? C_Projekt_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Schválený rozpočet", Mandatory = true)]
        public decimal SchvalenyRzp { get; set; }

        [DataMember]
        [PfeColumn(Text = "Návrh rozpočtu Rok+1", Mandatory = true)]
        public decimal NavrhRzp1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Návrh rozpočtu Rok+2", Mandatory = true)]
        public decimal NavrhRzp2 { get; set; }
    }
}
