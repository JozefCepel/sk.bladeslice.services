using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Types.Reg;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Fin
{
    [DataContract]
    [Schema("fin")]
    [Alias("D_DokladBANPol")]
    public class DokladBANPol : BaseTenantEntity
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        [PfeColumn(Text = "_D_DokladBANPol_Id")]
        public long D_DokladBANPol_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Rok", ReadOnly = true)]
        public short Rok { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_BiznisEntita_Id")]
        public long D_BiznisEntita_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Pč")]
        public int Poradie { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_Typ_Id", DefaultValue = (int)TypEnum.Kredit)]
        public int C_Typ_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_StavEntity_Id_Parovanie")]
        public int? C_StavEntity_Id_Parovanie { get; set; }

        [DataMember]
        [PfeColumn(Text = "VS", Tooltip = "Variabilný symbol")]
        [StringLength(40)]
        public string VS { get; set; }

        [DataMember]
        [PfeColumn(Text = "ŠS", Tooltip = "Špecifický symbol")]
        [StringLength(10)]
        public string SS { get; set; }

        [DataMember]
        [PfeColumn(Text = "KS", Tooltip = "Konštantný symbol")]
        [StringLength(4)]
        public string KS { get; set; }

        [DataMember]
        [PfeColumn(Text = "Dátum pohybu", Type = PfeDataType.Date)]
        public DateTime DatumPohybu { get; set; }

        [DataMember]
        [PfeColumn(Text = "Dátum reálnej úhrady/valuty", Type = PfeDataType.Date)]
        public DateTime DatumValuta { get; set; }

        [DataMember]
        [PfeColumn(Text = "Osoba")]
        public string Osoba { get; set; }

        [DataMember]
        [PfeColumn(Text = "Bankový účet osoby")]
        public string OsobaBankaUcetNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_Stredisko_Id")]
        public int? C_Stredisko_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_Projekt_Id")]
        public long? C_Projekt_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_UctKluc_Id1")]
        public long? C_UctKluc_Id1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_UctKluc_Id2")]
        public long? C_UctKluc_Id2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_UctKluc_Id3")]
        public long? C_UctKluc_Id3 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Popis")]
        [StringLength(256)]
        public string Popis { get; set; }

        [DataMember]
        [PfeColumn(Text = "Suma úhrady", DefaultValue = 0)]
        public decimal Suma { get; set; }

        [DataMember]
        [PfeColumn(Text = "IBAN")]
        [StringLength(40)]
        public string IBAN { get; set; }

        [DataMember]
        [PfeColumn(Text = "BIC")]
        [StringLength(15)]
        public string BIC { get; set; }
    }
}
