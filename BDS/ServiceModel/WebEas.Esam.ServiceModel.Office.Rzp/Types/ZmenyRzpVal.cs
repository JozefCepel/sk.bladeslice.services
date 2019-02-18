using ServiceStack.DataAnnotations;
using System;
using System.Linq;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Rzp.Types
{
    [Schema("rzp")]
    [Alias("D_ZmenyRzpVal")]
    [DataContract]
    public class ZmenyRzpVal : BaseTenantEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public long D_ZmenyRzpVal_Id { get; set; }

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
        [PfeColumn(Text = "Suma", Mandatory = true)]
        public decimal SumaZmeny { get; set; }

        [DataMember]
        [PfeColumn(Text = "Popis", Xtype = PfeXType.TextareaWW, Mandatory = true)]
        public string Popis { get; set; }
    }
}
