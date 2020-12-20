using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Reg
{
    [DataContract]
    [Schema("reg")]
    [Alias("C_Stat")]
    public class StatCis : BaseEntity
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        public short C_Stat_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kód")]
        [StringLength(3)]
        public string Kod { get; set; }

        [DataMember]
        [PfeColumn(Text = "Skratka")]
        [StringLength(2)]
        public string Skratka { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov")]
        [StringLength(24)]
        public string Nazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Úplný názov")]
        [StringLength(144)]
        public string NazovUplny { get; set; }

        [DataMember]
        [PfeColumn(Text = "Anglický názov")]
        [StringLength(24)]
        public string NazovEn { get; set; }

        [DataMember]
        [PfeColumn(Text = "Anglický úplný názov")]
        [StringLength(144)]
        public string NazovEnUplny { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť od", Type = PfeDataType.Date, Mandatory = true)]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť do", Type = PfeDataType.Date)]
        public DateTime? PlatnostDo { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_Mena_Id")]
        public short? C_Mena_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Štát EÚ")]
        public bool EU { get; set; }
    }

    [Flags]
    public enum StatEnum
    {
        Slovensko = 703,
    }
}
