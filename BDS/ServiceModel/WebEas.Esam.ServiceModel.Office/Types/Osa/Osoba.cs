using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Osa
{
    [DataContract]
    [Schema("osa")]
    [Alias("D_Osoba")]
    public class Osoba : BaseEntity
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        public long D_Osoba_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Externé ID osoby", ReadOnly = true, Editable = false)]
        public Guid? D_Osoba_Id_Externe { get; set; }

        [DataMember]
        [PfeColumn(Text = "Typ osoby")]
        public short C_OsobaTyp_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "eDesk adresa", Editable = false, ReadOnly = true)]
        [StringLength(255)]
        public string UpvsUri { get; set; }

        [DataMember]
        [PfeColumn(Text = "UPVS ID", Editable = false, ReadOnly = true)]
        [StringLength(200)]
        public string UpvsId { get; set; }

        [DataMember]
        [PfeColumn(Text = "Centrálna evidencia", ReadOnly = true, Editable = false)]
        public bool CentralnaEvidencia { get; set; }
    }
}
