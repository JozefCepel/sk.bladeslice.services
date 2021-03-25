using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Reg
{
    [DataContract]
    [Schema("reg")]
    [Alias("C_Pokladnica")]
    public class Pokladnica : BaseTenantEntity
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        public int C_Pokladnica_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_TypeElement_Id")]
        [Ignore]
        public string TypeElement_Id => "2_" + C_Pokladnica_Id;

        [DataMember]
        [PfeColumn(Text = "_C_Pokladnica_Id_Externe")]
        public string C_Pokladnica_Id_Externe { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov", Mandatory = true)]
        [StringLength(255)]
        public string Nazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kód", Mandatory = true)]
        [StringLength(2, 30)]
        [PfeValueColumn]
        public string Kod { get; set; }

        [DataMember]
        [PfeColumn(Text = "Pč", Mandatory = true)]
        public byte Poradie { get; set; }

        [DataMember]
        [PfeColumn(Text = "DCOM", DefaultValue = 0, ReadOnly = true)]
        public bool? DCOM { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť od", Type = PfeDataType.Date, Mandatory = true)]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť do", Type = PfeDataType.Date)]
        public DateTime? PlatnostDo { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_Mena_Id", Mandatory = true, DefaultValue = (short)MenaEnum.EUR)]
        public short C_Mena_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_User_Id_Podpisal")]
        public Guid? D_User_Id_Podpisal { get; set; }
    }
}
