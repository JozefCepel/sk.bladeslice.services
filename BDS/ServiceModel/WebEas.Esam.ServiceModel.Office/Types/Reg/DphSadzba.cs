using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Reg
{
    [DataContract]
    [Schema("reg")]
    [Alias("C_DphSadzba")]
    public class DphSadzba : BaseEntity
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        [PfeColumn(Text = "_C_DPHSadzba_Id", Mandatory = true)]
        public int C_DPHSadzba_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_TypId", Mandatory = true)]
        public byte TypId { get; set; }

        [DataMember]
        [PfeColumn(Text = "Sadzba DPH", Mandatory = true)]
        public byte DPH { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť od", Type = PfeDataType.Date, Mandatory = true)]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť do", Type = PfeDataType.Date)]
        public DateTime? PlatnostDo { get; set; }

    }
}
