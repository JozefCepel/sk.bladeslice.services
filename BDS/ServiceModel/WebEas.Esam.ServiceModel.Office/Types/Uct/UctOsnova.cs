using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Uct
{
    [DataContract]
    [Schema("uct")]
    [Alias("C_UctOsnova")]
    public class UctOsnova : BaseEntity
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        public long C_UctOsnova_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_OrganizaciaTyp_Id")]
        public byte? C_OrganizaciaTyp_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť od", Type = PfeDataType.Date)]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť do", Type = PfeDataType.Date)]
        public DateTime? PlatnostDo { get; set; }

        [DataMember]
        [PfeColumn(Text = "Účet")]
        public string SU { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov")]
        public string Nazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Typ", Hidden = true)]
        public string Typ { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Druh", Hidden = true)]
        public string Druh { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Saldokontný", Hidden =true)]
        public string SDK { get; set; }
    }
}
