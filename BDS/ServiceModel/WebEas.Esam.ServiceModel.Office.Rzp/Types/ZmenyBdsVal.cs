using ServiceStack.DataAnnotations;
using System;
using System.Linq;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("rzp")]
    [Alias("D_ZmenyBdsVal")]
    [DataContract]
    public class ZmenyBdsVal : BaseTenantEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public long D_ZmenyBdsVal_Id { get; set; }

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
        [PfeColumn(Text = "Suma", Mandatory = true)]
        public decimal SumaZmeny { get; set; }

        [DataMember]
        [PfeColumn(Text = "Popis", Xtype = PfeXType.TextareaWW, Mandatory = true)]
        public string Popis { get; set; }
    }
}
