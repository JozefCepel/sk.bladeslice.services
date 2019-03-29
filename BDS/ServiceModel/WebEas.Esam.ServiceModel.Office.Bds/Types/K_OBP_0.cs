using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("dbo")]
    [Alias("K_OBP_0")]
    [DataContract]
    public class tblK_OBP_0 : BaseEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public int K_OBP_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_K_TOB_0")]
        public int K_TOB_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_K_PRF_0")]
        public int K_PRF_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_K_OPC_0")]
        public int K_OPC_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_K_OPK_0")]
        public int? K_OPK_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "IČO")]
        public string ICO { get; set; }

        [DataMember]
        [PfeColumn(Text = "DIČ")]
        public string DRC { get; set; }

        [DataMember]
        [PfeColumn(Text = "IČ DPH")]
        public string IC_DPH { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov")]
        public string NAZOV1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov 2")]
        public string NAZOV2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Ulica")]
        public string ULICA_S { get; set; }

        [DataMember]
        [PfeColumn(Text = "PSČ")]
        public string PSC_S { get; set; }

        [DataMember]
        [PfeColumn(Text = "Obec")]
        public string OBEC_S { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov 1 - Prevádzka")]
        public string NAZOV1_P { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov 2 - Prevádzka")]
        public string NAZOV2_P { get; set; }

        [DataMember]
        [PfeColumn(Text = "Ulica - Prevádzka")]
        public string ULICA_P { get; set; }

        [DataMember]
        [PfeColumn(Text = "PSČ - Prevádzka")]
        public string PSC_P { get; set; }

        [DataMember]
        [PfeColumn(Text = "Obec - Prevádzka")]
        public string OBEC_P { get; set; }

        [DataMember]
        [PfeColumn(Text = "Štát")]
        public string STAT { get; set; }

        [DataMember]
        [PfeColumn(Text = "Interný názov")]
        public string NICKNAME { get; set; }

        [DataMember]
        [PfeColumn(Text = "Číslo OBP")]
        public string OBP_C { get; set; }

        [DataMember]
        [PfeColumn(Text = "_DEALER")]
        public int? DEALER { get; set; }

        [DataMember]
        [PfeColumn(Text = "Splatnosť", DefaultValue = 0)]
        public short? SPLAT { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zľava", DefaultValue = 0)]
        public decimal? ZLAVA { get; set; }

        [DataMember]
        [PfeColumn(Text = "_PENALE", DefaultValue = 0)]
        public decimal? PENALE { get; set; }

        [DataMember]
        [PfeColumn(Text = "_KREDIT", DefaultValue = 0)]
        public int KREDIT { get; set; }

        [DataMember]
        [PfeColumn(Text = "Osoba")]
        public string OSOBA { get; set; }

        [DataMember]
        [PfeColumn(Text = "Funkcia")]
        public string FUNKCIA { get; set; }

        [DataMember]
        [PfeColumn(Text = "Telefón 1")]
        public string TEL1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Telefón 2")]
        public string TEL2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Fax")]
        public string FAX { get; set; }

        [DataMember]
        [PfeColumn(Text = "Mobil")]
        public string MOBIL { get; set; }

        [DataMember]
        [PfeColumn(Text = "E-mail")]
        public string EMAIL { get; set; }

        [DataMember]
        [PfeColumn(Text = "WWW")]
        public string WWW { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Poznámka")]
        public string POZN { get; set; }

        [DataMember]
        [PfeColumn(Text = "Potenciál zákazníka", DefaultValue = 0)]
        public int OBP_POTENCIAL { get; set; }

        [DataMember]
        [PfeColumn(Text = "Doplňujúci text")]
        public string DOPL_TEXT { get; set; }
    }
}
