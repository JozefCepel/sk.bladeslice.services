using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel.Office.Egov.Reg.Types
{
    [DataContract]
    [Schema("reg")]
    [Alias("D_BiznisEntita")]
    public class BiznisEntita : BaseTenantEntity
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        [PfeColumn(Text = "Biznis entita ID")]
        public long D_BiznisEntita_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Typ biznis entity")]
        public short C_TypBiznisEntity_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Stav entity")]
        public int C_StavEntity_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Stavový priestor")]
        public int C_StavovyPriestor_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Stredisko")]
        public int? C_Stredisko_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Bank. účet")]
        public int? C_BankaUcet_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Pokladnica")]
        public int? C_Pokladnica_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Položka stromu")]
        public string PolozkaStromu { get; set; }

        [DataMember]
        [PfeColumn(Text = "Účtovné obdobie")]
        public byte UOMesiac { get; set; }

        [DataMember]
        [PfeColumn(Text = "Rok")]
        public short Rok { get; set; }

        [DataMember]
        [PfeColumn(Text = "Vystavené")]
        public DateTime? DatumVystavenia { get; set; }

        [DataMember]
        [PfeColumn(Text = "Prijaté")]
        public DateTime? DatumPrijatia { get; set; }

        [DataMember]
        [PfeColumn(Text = "Splatnosť")]
        public DateTime? DatumSplatnosti { get; set; }

        [DataMember]
        [PfeColumn(Text = "DUÚP")]
        public DateTime? DatumUUP { get; set; }

        [DataMember]
        [PfeColumn(Text = "DVDP")]
        public DateTime? DatumVDP { get; set; }

        [DataMember]
        [PfeColumn(Text = "Doc. number ID")]
        public int? C_Cislovanie_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Doc. number")]
        public int? Cislo { get; set; }

        [DataMember]
        [PfeColumn(Text = "Interné číslo")]
        public string CisloInterne { get; set; }

        [DataMember]
        [PfeColumn(Text = "Externé číslo")]
        public string CisloExterne { get; set; }

        [DataMember]
        [PfeColumn(Text = "Variab. symbol")]
        public string VS { get; set; }

        [DataMember]
        [PfeColumn(Text = "Osoba")]
        public int? D_Osoba_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Mena")]
        public short C_Mena_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kurz ECB")]
        public decimal KurzECB { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kurz banka")]
        public decimal KurzBanka { get; set; }

        [DataMember]
        public bool PS { get; set; }

    }
}
