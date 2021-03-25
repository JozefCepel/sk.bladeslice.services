using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Types.Reg;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Crm
{
    [DataContract]
    [Schema("crm")]
    [Alias("D_DokladCRMPol")]
    public class DokladCRMPol : BaseTenantEntity
    {
        [DataMember]
        [PrimaryKey]
        [AutoIncrement]
        [PfeColumn(Text = "_D_DokladCRMPol_Id")]
        public long D_DokladCRMPol_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_BiznisEntita_Id")]
        public long D_BiznisEntita_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Rok", ReadOnly = true)]
        public short Rok { get; set; }

        [DataMember]
        [PfeColumn(Text = "Pč", Mandatory = true)]
        public int Poradie { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_Typ_Id", Mandatory = true)]
        public int C_Typ_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kód")]
        [StringLength(40)]
        public string Kod { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov", Mandatory = true)]
        [StringLength(180)]
        public string Nazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Výrobné číslo")]
        [StringLength(40)]
        public string VyrobneCislo { get; set; }

        [DataMember]
        [PfeColumn(Text = "MJ")]
        [StringLength(8)]
        public string MJ { get; set; }

        [DataMember]
        [PfeColumn(Text = "Množstvo", Mandatory = true, DefaultValue = 0)]
        public decimal Mnozstvo { get; set; }

        [DataMember]
        [PfeColumn(Text = "Cena", Mandatory = true, DefaultValue = 0)]
        public decimal DM_Cena { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Z-Cena", Mandatory = true, DefaultValue = 0)]
        public decimal CM_Cena { get; set; }

        [DataMember]
        [PfeColumn(Text = "DPH", Mandatory = true)]
        public byte DPH { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_Stredisko_Id")]
        public int? C_Stredisko_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_Projekt_Id")]
        public long? C_Projekt_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_UctKluc1_Id")]
        public long? C_UctKluc1_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_UctKluc2_Id")]
        public long? C_UctKluc2_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_UctKluc3_Id")]
        public long? C_UctKluc3_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_ZdrojTransferCis_Id")]
        public int? C_ZdrojTransferCis_Id { get; set; }

    }
}
