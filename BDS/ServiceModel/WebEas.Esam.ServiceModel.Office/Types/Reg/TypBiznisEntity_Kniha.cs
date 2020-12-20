
using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Reg
{
    [DataContract]
    [Schema("reg")]
    [Alias("C_TypBiznisEntity_Kniha")]
    public class TypBiznisEntity_Kniha : BaseTenantEntityNullable
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        public int C_TypBiznisEntity_Kniha_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kód", Mandatory = true)]
        [StringLength(6)]
        public string Kod { get; set; }

        [DataMember]
        [PfeColumn(Text = "Typ dokladu", Mandatory = true)]
        public short C_TypBiznisEntity_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov", Mandatory = true)]
        [StringLength(100)]
        public string Nazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Pč", Mandatory = true)]
        public byte Poradie { get; set; }

        [DataMember]
        [PfeColumn(Text = "ID skupiny predkontácie")]
        public int? SkupinaPredkont_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Skupina predkontácie")]
        [StringLength(40)]
        public string SkupinaPredkont_Popis { get; set; }
    }
}
