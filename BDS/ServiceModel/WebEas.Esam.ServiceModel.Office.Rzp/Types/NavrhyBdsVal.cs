using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("rzp")]
    [Alias("D_NavrhyBdsVal")]
    [DataContract]
    public class NavrhyBdsVal : BaseTenantEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public long D_NavrhyBdsVal_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_NavrhZmenyBds_Id", Mandatory = true)]
        public long D_NavrhZmenyBds_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_BdsPolozky_Id")]
        public long? C_BdsPolozky_Id { get; set; }

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
        public decimal SchvalenyBds { get; set; }

        [DataMember]
        [PfeColumn(Text = "Návrh rozpočtu Rok+1", Mandatory = true)]
        public decimal NavrhBds1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Návrh rozpočtu Rok+2", Mandatory = true)]
        public decimal NavrhBds2 { get; set; }
    }
}
