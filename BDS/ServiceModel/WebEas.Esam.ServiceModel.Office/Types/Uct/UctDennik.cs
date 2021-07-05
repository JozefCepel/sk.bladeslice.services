using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Uct
{
    [DataContract]
    [Schema("uct")]
    [Alias("D_UctDennik")]
    public class UctDennik : BaseTenantEntity
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        public long D_UctDennik_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_BiznisEntita_Id", Mandatory = true)]
        public long D_BiznisEntita_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_UctDennik_Id_Ref")]
        public long? D_UctDennik_Id_Ref { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_DokladBANPol_Id")]
        public long? D_DokladBANPol_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_UhradaParovanie_Id")]
        public long? D_UhradaParovanie_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Rok", ReadOnly = true)]
        public short Rok { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_Osoba_Id")]
        public long? D_Osoba_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_UctRozvrh_Id", Mandatory = true)]
        public long? C_UctRozvrh_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Pč", Mandatory = true)]
        public int Poradie { get; set; }

        [DataMember]
        [PfeColumn(Text = "VS", Tooltip = "Variabilný symbol")]
        public string VS { get; set; }

        [DataMember]
        [PfeColumn(Text = "Splatnosť", Type = PfeDataType.Date)]
        public DateTime? DatumSplatnosti { get; set; }

        [DataMember]
        [PfeColumn(Text = "Dátum účtovania", Type = PfeDataType.Date, Editable = false, ReadOnly = true)]
        public DateTime DatumUctovania { get; set; }

        [DataMember]
        [PfeColumn(Text = "Popis", Xtype = PfeXType.Textarea)]
        public string Popis { get; set; }

        [DataMember]
        [PfeColumn(Text = "Má dať CM", Mandatory = true)]
        public decimal SumaMD_CM { get; set; }

        [DataMember]
        [PfeColumn(Text = "Dal CM", Mandatory = true)]
        public decimal SumaDal_CM { get; set; }

        [DataMember]
        [PfeColumn(Text = "Má dať", Mandatory = true)]
        public decimal SumaMD { get; set; }

        [DataMember]
        [PfeColumn(Text = "Dal", Mandatory = true)]
        public decimal SumaDal { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_Stredisko_Id")]
        public int? C_Stredisko_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_Projekt_Id")]
        public long? C_Projekt_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_UctKluc_Id1", Hidden = true)]
        public long? C_UctKluc_Id1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_UctKluc_Id2", Hidden = true)]
        public long? C_UctKluc_Id2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_UctKluc_Id3", Hidden = true)]
        public long? C_UctKluc_Id3 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_Typ_Id")] //Field pre ID-DaP
        public int? C_Typ_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_VymerPol_Id")] //Field pre ID-DaP
        public long? D_VymerPol_Id { get; set; }
    }
}
