using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Osa
{
    [DataContract]
    [Schema("osa")]
    [Alias("C_OsobaTyp")]
    public class OsobaTyp : BaseEntity
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        public short C_OsobaTyp_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kód")]
        [StringLength(1)]
        public string Kod { get; set; }

        [DataMember]
        [PfeColumn(Text = "Typ osoby", Mandatory = true)]
        [StringLength(40)]
        public string Nazov { get; set; }
    }
}
