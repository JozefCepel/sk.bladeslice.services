using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Reg
{
    [DataContract]
    [Schema("reg")]
    [Alias("C_Banka")]
    public class BankaCis : BaseTenantEntityNullable
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        [PfeColumn(Text = "Banka")]
        public short C_Banka_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Štát", Mandatory = true)]
        public short C_Stat_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov banky", Mandatory = true)]
        [StringLength(255)]
        public string Nazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kód banky")]
        [StringLength(30)]
        public string Kod { get; set; }

        [DataMember]
        [PfeColumn(Text = "BIC", Mandatory = true)]
        [StringLength(15)]
        public string BIC { get; set; }
    }
}
