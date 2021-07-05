using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Cfe
{
    [DataContract]
    [Schema("cfe")]
    [Alias("C_OrganizaciaTypDetail")]
    public class OrganizaciaTypDetail : BaseEntity
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        [PfeColumn(Text = "Typ organizácie detail ID", Mandatory = true)]
        public byte C_OrganizaciaTypDetail_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kód")]
        [StringLength(3)]
        public string Kod { get; set; }

        [DataMember]
        [PfeColumn(Text = "Typ")]
        [StringLength(50)]
        public string Typ { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov", Mandatory = true)]
        [StringLength(255)]
        public string Nazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Typ organizácie ID", Mandatory = true)]
        public byte C_OrganizaciaTyp_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť od", Type = PfeDataType.Date, Mandatory = true)]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť do", Type = PfeDataType.Date)]
        public DateTime? PlatnostDo { get; set; }

    }
}
