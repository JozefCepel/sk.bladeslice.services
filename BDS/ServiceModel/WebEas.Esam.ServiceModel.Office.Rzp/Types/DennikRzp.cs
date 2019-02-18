using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Rzp.Types
{
    [Schema("rzp")]
    [Alias("D_DennikRzp")]
    [DataContract]
    public class DennikRzp : BaseTenantEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public long D_DennikRzp_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_IntDoklad_Id")]
        public long? D_IntDoklad_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_RzpPolozky_Id")]
        public long C_RzpPolozky_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_Program_Id")]
        public long? D_Program_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_Stredisko_Id")]
        public long? C_Stredisko_Id { get; set; }

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
