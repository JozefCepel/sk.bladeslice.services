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
        [PfeColumn(Text = "_K_OBP_0")]
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
        [PfeColumn(Text = "_ICO")]
        public string ICO { get; set; }
        [DataMember]
        [PfeColumn(Text = "_DRC")]
        public string DRC { get; set; }
        [DataMember]
        [PfeColumn(Text = "_NAZOV1")]
        public string NAZOV1 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_NAZOV2")]
        public string NAZOV2 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_ULICA_S")]
        public string ULICA_S { get; set; }
        [DataMember]
        [PfeColumn(Text = "_PSC_S")]
        public string PSC_S { get; set; }
        [DataMember]
        [PfeColumn(Text = "_OBEC_S")]
        public string OBEC_S { get; set; }
        [DataMember]
        [PfeColumn(Text = "_NAZOV1_P")]
        public string NAZOV1_P { get; set; }
        [DataMember]
        [PfeColumn(Text = "_NAZOV2_P")]
        public string NAZOV2_P { get; set; }
        [DataMember]
        [PfeColumn(Text = "_ULICA_P")]
        public string ULICA_P { get; set; }
        [DataMember]
        [PfeColumn(Text = "_PSC_P")]
        public string PSC_P { get; set; }
        [DataMember]
        [PfeColumn(Text = "_OBEC_P")]
        public string OBEC_P { get; set; }
        [DataMember]
        [PfeColumn(Text = "_STAT")]
        public string STAT { get; set; }
        [DataMember]
        [PfeColumn(Text = "_DEALER")]
        public int? DEALER { get; set; }
        [DataMember]
        [PfeColumn(Text = "_SPLAT")]
        public short? SPLAT { get; set; }
        [DataMember]
        [PfeColumn(Text = "_ZLAVA")]
        public decimal? ZLAVA { get; set; }
        [DataMember]
        [PfeColumn(Text = "_PENALE")]
        public decimal? PENALE { get; set; }
        [DataMember]
        [PfeColumn(Text = "_KREDIT")]
        public int KREDIT { get; set; }
        [DataMember]
        [PfeColumn(Text = "_OSOBA")]
        public string OSOBA { get; set; }
        [DataMember]
        [PfeColumn(Text = "_FUNKCIA")]
        public string FUNKCIA { get; set; }
        [DataMember]
        [PfeColumn(Text = "_TEL1")]
        public string TEL1 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_TEL2")]
        public string TEL2 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_FAX")]
        public string FAX { get; set; }
        [DataMember]
        [PfeColumn(Text = "_MOBIL")]
        public string MOBIL { get; set; }
        [DataMember]
        [PfeColumn(Text = "_EMAIL")]
        public string EMAIL { get; set; }
        [DataMember]
        [PfeColumn(Text = "_WWW")]
        public string WWW { get; set; }
        [DataMember]
        [PfeColumn(Text = "_POZN")]
        public string POZN { get; set; }
        [DataMember]
        [PfeColumn(Text = "_IC_DPH")]
        public string IC_DPH { get; set; }
        [DataMember]
        [PfeColumn(Text = "_K_OPK_0")]
        public int? K_OPK_0 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_NICKNAME")]
        public string NICKNAME { get; set; }
        [DataMember]
        [PfeColumn(Text = "_OBP_C")]
        public string OBP_C { get; set; }
        [DataMember]
        [PfeColumn(Text = "_OBP_POTENCIAL")]
        public int OBP_POTENCIAL { get; set; }
        [DataMember]
        [PfeColumn(Text = "_DOPL_TEXT")]
        public string DOPL_TEXT { get; set; }
        [DataMember]
        [PfeColumn(Text = "_IS_PLATCA")]
        public bool? IS_PLATCA { get; set; }
    }
}
