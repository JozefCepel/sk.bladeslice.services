using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Rzp
{
    [Schema("rzp")]
    [Alias("D_RzpDennik")]
    [DataContract]
    public class RzpDennik : BaseTenantEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public long D_RzpDennik_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_BiznisEntita_Id")]
        public long D_BiznisEntita_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_DokladBANPol_Id")]
        public long? D_DokladBANPol_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_UhradaParovanie_Id")]
        public long? D_UhradaParovanie_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_RzpPol_Id", Mandatory = true)]
        public long? C_RzpPol_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_Program_Id")]
        public long? D_Program_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_Stredisko_Id")]
        public int? C_Stredisko_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_Projekt_Id")]
        public long? C_Projekt_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Popis", Xtype = PfeXType.Textarea)]
        public string Popis { get; set; }

        [DataMember]
        [PfeColumn(Text = "Suma", Mandatory = true, DefaultValue = 0)]
        public decimal Suma { get; set; }

        [DataMember]
        [PfeColumn(Text = "Počet", Mandatory = true, DefaultValue = 1)]
        public decimal Pocet { get; set; }

        [DataMember]
        [PfeColumn(Text = "Pč", Mandatory = true)]
        public int Poradie { get; set; }

        [DataMember]
        [PfeColumn(Text = "Rok", ReadOnly = true)]
        public short Rok { get; set; }

        [DataMember]
        [PfeColumn(Text = "_DatumPlatnosti", Hidden = true)]
        public DateTime? DatumPlatnosti { get; set; }
    }
}
