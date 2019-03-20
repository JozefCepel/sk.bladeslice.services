using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Rzp.Types
{
    [Schema("rzp")]
    [Alias("C_FRZdroj")]
    [DataContract]
    public class FRZdrojCis : BaseEntity
    {
        [DataMember]
        [PrimaryKey]
        [AutoIncrement]
        public long C_FRZdroj_Id { get; set; }

        [DataMember]
        public int C_OrganizaciaTyp_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "ZD1")]
        public string ZD1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "ZD2")]
        public string ZD2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "ZD3")]
        public string ZD3 { get; set; }

        [DataMember]
        [PfeColumn(Text = "ZD4")]
        public string ZD4 { get; set; }

        [DataMember]
        [PfeColumn(Text = "ZD5")]
        public string ZD5 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kód")]
        public string ZdrojKod { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov")]
        public string ZdrojNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platný")]
        public bool Platny { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť od", Type = PfeDataType.Date, Mandatory = true)]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť do", Type = PfeDataType.Date)]
        public DateTime? PlatnostDo { get; set; }
    }
}
