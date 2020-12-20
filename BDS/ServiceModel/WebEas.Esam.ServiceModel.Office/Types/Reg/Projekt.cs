using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Reg
{
    [Schema("reg")]
    [Alias("C_Projekt")]
    [DataContract]
    public class Projekt : BaseTenantEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public long C_Projekt_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_Projekt_Id_Externe")]
        public string C_Projekt_Id_Externe { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kód", Mandatory = true)]
        [StringLength(30)]
        public string Kod { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov", Mandatory = true)]
        [StringLength(100)]
        public string Nazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Popis", Xtype = PfeXType.TextareaWW)]
        public string Popis { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_RzpTyp_Id")] //Combo
        public int? C_RzpTyp_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť od", Type = PfeDataType.Date, Mandatory = true)]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť do", Type = PfeDataType.Date)]
        public DateTime? PlatnostDo { get; set; }

        [DataMember]
        [PfeColumn(Text = "Dátum dotácie", Type = PfeDataType.Date)]
        public DateTime? DatumDotacie { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Poskytovatel")]
        public Guid? Poskytovatel { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_FRZdroj_Id")]
        public long? C_FRZdroj_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Suma")]
        public decimal? Suma { get; set; }
    }
}
