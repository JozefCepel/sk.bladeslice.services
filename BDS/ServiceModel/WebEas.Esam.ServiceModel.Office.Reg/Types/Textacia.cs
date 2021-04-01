using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Reg.Types
{
    [DataContract]
    [Schema("reg")]
    [Alias("C_Textacia")]
    public class Textacia : BaseTenantEntityNullable
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        [PfeColumn(Text = "_C_Textacia_Id", Mandatory = true)]
        public int C_Textacia_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kód")]
        [StringLength(7)]
        public string Kod { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov", Mandatory = true)]
        [StringLength(100)]
        public string Nazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Popis")]
        [StringLength(255)]
        public string Popis { get; set; }

        [DataMember]
        [PfeColumn(Text = "Typ dokladu")]
        public short? C_TypBiznisEntity_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kniha")]
        public int? C_TypBiznisEntity_Kniha_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Druh dane")]
        public long? C_Druh_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť od", Mandatory = true)]
        public short RokOd { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť do")]
        public short? RokDo { get; set; }

    }
}
