using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Reg
{
    [DataContract]
    [Schema("reg")]
    [Alias("C_Mena")]
    public class Mena : BaseEntity
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        public short C_Mena_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kód")]
        [StringLength(3)]
        [PfeValueColumn]
        public string Kod { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov")]
        [StringLength(255)]
        public string Nazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Anglický názov")]
        [StringLength(255)]
        public string English { get; set; }

        [DataMember]
        [PfeColumn(Text = "Pč")]
        public byte? Poradie { get; set; }

        [DataMember]
        [PfeColumn(Text = "Znak")]
        public string Znak { get; set; }

        [DataMember]
        [PfeColumn(Text = "Frakcia")]
        public byte? Frakcia { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť od", Type = PfeDataType.Date, Mandatory = true)]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť do", Type = PfeDataType.Date)]
        public DateTime? PlatnostDo { get; set; }
    }
 
}
