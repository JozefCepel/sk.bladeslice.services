using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Reg.Types
{
    [DataContract]
    [Schema("reg")]
    [Alias("C_KS")]
    public class KS : BaseEntity
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        [PfeColumn(Text = "Identifikátor")]
        public short C_KS_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kód")]
        public string Kod { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov")]
        public string Nazov { get; set; }

    }
}
