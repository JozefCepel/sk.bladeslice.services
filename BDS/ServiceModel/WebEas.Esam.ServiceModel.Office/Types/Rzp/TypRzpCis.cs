using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Rzp
{
    [Schema("rzp")]
    [Alias("C_RzpTyp")]
    [DataContract]
    public class RzpTypCis : BaseEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public int C_RzpTyp_Id { get; set; }

        [PfeColumn(Text = "Typ rozpočtu")]
        [PfeValueColumn]
        [DataMember]
        public string Nazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť od", Type = PfeDataType.Date, Mandatory = true)]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť do", Type = PfeDataType.Date)]
        public DateTime? PlatnostDo { get; set; }
    }
}