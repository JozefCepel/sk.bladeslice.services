using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Types.Reg;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Office.Egov.Cis.Types;

namespace WebEas.Esam.ServiceModel.Office.Reg.Types
{
    [DataContract]
    [Schema("reg")]
    [Alias("C_MenaKurz")]
    public class MenaKurz : BaseEntity
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        public long C_MenaKurz_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_Mena_Id", Mandatory = true, DefaultValue = (short)MenaEnum.EUR)]
        public short C_Mena_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kurz")]
        public decimal Kurz { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť od", Type = PfeDataType.Date, Mandatory = true)]
        public DateTime PlatnostOd { get; set; }
    }
}