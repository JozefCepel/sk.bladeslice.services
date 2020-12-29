using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Reg
{
    [Schema("reg")]
    [Alias("C_Stredisko")]
    [DataContract]
    public class StrediskoCis : BaseTenantEntity
    {
        [AutoIncrement]
        [DataMember]
        [PrimaryKey]
        public int C_Stredisko_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_Stredisko_Id_Parent")]
        public int? C_Stredisko_Id_Parent { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_Stredisko_Id_Externe")]
        public string C_Stredisko_Id_Externe { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kód", Mandatory = true)]
        [StringLength(2, 30)]
        public string Kod { get; set; }

        [DataMember]
        [PfeColumn(Text = "Pč", Mandatory = true)]
        public short Poradie { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov", Mandatory = true)]
        [StringLength(100)]
        public string Nazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Popis", Xtype = PfeXType.TextareaWW)]
        public string Popis { get; set; }

        [DataMember]
        [PfeColumn(Text = "Podnikateľská činnosť", DefaultValue = false)]
        public bool PodnCinn { get; set; }

        [DataMember]
        [PfeColumn(Text = "DCOM", DefaultValue = 0, ReadOnly = true)]
        public bool? DCOM { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť od", Type = PfeDataType.Date, Mandatory = true)]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť do", Type = PfeDataType.Date)]
        public DateTime? PlatnostDo { get; set; }
    }
}
