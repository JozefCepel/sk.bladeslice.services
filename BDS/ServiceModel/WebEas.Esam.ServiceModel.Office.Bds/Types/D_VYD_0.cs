using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("dbo")]
    [Alias("D_VYD_0")]
    [DataContract]
    public class tblD_VYD_0 : BaseEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        [PfeColumn(Text = "_D_VYD_0")]
        public int D_VYD_0 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_K_OBP_0")]
        public int? K_OBP_0 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_K_SKL_0")]
        public int K_SKL_0 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_K_POH_0")]
        public int K_POH_0 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_K_MEN_0")]
        public int K_MEN_0 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_K_ORJ_0")]
        public int K_ORJ_0 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_KURZ")]
        public decimal? KURZ { get; set; }
        [DataMember]
        [PfeColumn(Text = "IČO")]
        public string ICO { get; set; }
        [DataMember]
        [PfeColumn(Text = "Názov 1")]
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
        [PfeColumn(Text = "_PS")]
        public bool? PS { get; set; }
        [DataMember]
        [PfeColumn(Text = "Číslo dokladu")]
        public string DKL_C { get; set; }
        [DataMember]
        [PfeColumn(Text = "Dodací list")]
        public string DL_C { get; set; }
        [DataMember]
        [PfeColumn(Text = "V")]
        public bool? V { get; set; }
        [DataMember]
        [PfeColumn(Text = "_Z")]
        public bool? Z { get; set; }
        [DataMember]
        [PfeColumn(Text = "Dátum výdaja")]
        public DateTime DAT_DKL { get; set; }
        [DataMember]
        [PfeColumn(Text = "_Poznámka")]
        public string POZN { get; set; }
        [DataMember]
        [PfeColumn(Text = "_D_SUMA")]
        public decimal? D_SUMA { get; set; }
        [DataMember]
        [PfeColumn(Text = "_Z_SUMA")]
        public decimal? Z_SUMA { get; set; }
        [DataMember]
        [PfeColumn(Text = "_D_DPH1")]
        public decimal? D_DPH1 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_D_DPH2")]
        public decimal? D_DPH2 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_SO")]
        public byte? SO { get; set; }
        [DataMember]
        [PfeColumn(Text = "_P")]
        public bool? P { get; set; }
    }
}