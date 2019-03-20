using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Rzp.Types
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
        public long? D_BiznisEntita_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_RzpPol_Id")]
        public long C_RzpPol_Id { get; set; }

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
        [PfeColumn(Text = "Popis", Xtype = PfeXType.TextareaWW, Mandatory = true)]
        public string Popis { get; set; }

        [DataMember]
        [PfeColumn(Text = "Suma", Mandatory = true)]
        public decimal Suma { get; set; }

        [DataMember]
        [PfeColumn(Text = "Počet", Mandatory = true)]
        public decimal Pocet { get; set; }

        [DataMember]
        [PfeColumn(Text = "Poradie", Mandatory = true)]
        public int Poradie { get; set; }
    }
}
