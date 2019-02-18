using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Rzp.Types
{
    [Schema("rzp")]
    [Alias("D_NavrhyRzpVal")]
    [DataContract]
    public class NavrhyRzpVal : BaseTenantEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public long D_NavrhyRzpVal_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_NavrhZmenyRzp_Id", Mandatory = true)]
        public long D_NavrhZmenyRzp_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_RzpPolozky_Id")]
        public long? C_RzpPolozky_Id { get; set; }

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
