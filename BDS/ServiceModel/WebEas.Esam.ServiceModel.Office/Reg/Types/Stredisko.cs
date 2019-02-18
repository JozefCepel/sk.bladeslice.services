using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Reg.Types
{
    [Schema("reg")]
    [Alias("C_Stredisko")]
    [DataContract]
    public class Stredisko : BaseTenantEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public long C_Stredisko_Id { get; set; }

        [PfeColumn(Text = "Kód", Mandatory = true)]
        [DataMember]
        public string Kod { get; set; }

        [PfeColumn(Text = "Názov", Mandatory = true)]
        [DataMember]
        public string Nazov { get; set; }

        [PfeColumn(Text = "Popis", Xtype = PfeXType.TextareaWW)]
        [DataMember]
        public string Popis { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť od", Type = PfeDataType.Date, Mandatory = true)]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť do", Type = PfeDataType.Date)]
        public DateTime? PlatnostDo { get; set; }
    }
}
