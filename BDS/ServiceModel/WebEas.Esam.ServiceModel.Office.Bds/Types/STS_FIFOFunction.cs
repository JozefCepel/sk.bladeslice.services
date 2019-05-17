using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("dbo")]
    [Alias("fnStsFifo_BDS (@iSkl, @DatTo, @LOCATION, @SN, @SARZA, @SKL_CENA)")]
    [DataContract]
    public class STS_FIFOFunction
    {
        [PrimaryKey]
        [DataMember]
        [PfeColumn(Text = "_ID_POL")]
        public string ID_POL { get; set; }

        [DataMember]
        [PfeColumn(Text = "_ID_JOIN")]
        public string ID_JOIN { get; set; }

        [DataMember]
        [PfeColumn(Text = "Warehouse")]
        public int? iSkl { get; set; }

        [DataMember]
        [PfeColumn(Text = "To date")]
        public DateTime? DatTo { get; set; }

        [DataMember]
        [PfeColumn(Text = "Show location?", DefaultValue = 1)]
        public bool bLOCATION { get; set; }

        [DataMember]
        [PfeColumn(Text = "Show SN?", DefaultValue = 1)]
        public bool bSN { get; set; }

        [DataMember]
        [PfeColumn(Text = "Show Batch?", DefaultValue = 1)]
        public bool bSARZA { get; set; }

        [DataMember]
        [PfeColumn(Text = "Show price", DefaultValue = 1)]
        public bool bSKL_CENA { get; set; }

        [DataMember]
        [PfeColumn(Text = "_K_SKL_0")]
        public int? K_SKL_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_K_TSK_0")]
        public int? K_TSK_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_K_TYP_0")]
        public int K_TYP_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_SKL_GRP")]
        public string SKL_GRP { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Sklad")]
        public string Sklad { get; set; }

        [DataMember]
        [PfeColumn(Text = "_TSK")]
        public string TSK { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Typ")]
        public string Typ { get; set; }

        [DataMember]
        [PfeColumn(Text = "_KOD")]
        public string KOD { get; set; }

        [DataMember]
        [PfeColumn(Text = "_EAN")]
        public string EAN { get; set; }

        [DataMember]
        [PfeColumn(Text = "_NAZOV")]
        public string NAZOV { get; set; }

        [DataMember]
        [PfeColumn(Text = "_MJ")]
        public string MJ { get; set; }

        [DataMember]
        [PfeColumn(Text = "_WARRANTY")]
        public int? WARRANTY { get; set; }

        [DataMember]
        [PfeColumn(Text = "_POZN")]
        public string POZN { get; set; }

        [DataMember]
        [PfeColumn(Text = "_LOCATION")]
        public string LOCATION { get; set; }

        [DataMember]
        [PfeColumn(Text = "_SN")]
        public string SN { get; set; }

        [DataMember]
        [PfeColumn(Text = "_SARZA")]
        public string SARZA { get; set; }

        [DataMember]
        [PfeColumn(Text = "_SKL_CENA")]
        public decimal? SKL_CENA { get; set; }

        [DataMember]
        [PfeColumn(Text = "_POC_KS")]
        public decimal? POC_KS { get; set; }

        [DataMember]
        [PfeColumn(Text = "_POC_KS_P")]
        public decimal? POC_KS_P { get; set; }

        [DataMember]
        [PfeColumn(Text = "_POC_KS_V")]
        public decimal? POC_KS_V { get; set; }

        [DataMember]
        [PfeColumn(Text = "_SKL_CENA_NO_ZERO")]
        public bool? SKL_CENA_NO_ZERO { get; set; }

        [DataMember]
        [PfeColumn(Text = "_UCT_SUMA")]
        public decimal? UCT_SUMA { get; set; }

        [DataMember]
        [PfeColumn(Text = "_DPH")]
        public decimal? DPH { get; set; }

        [DataMember]
        [PfeColumn(Text = "_ID_KAT")]
        public int? ID_KAT { get; set; }

        [DataMember]
        [PfeColumn(Text = "_ATT_FILE_NAME")]
        public string ATT_FILE_NAME { get; set; }

        [DataMember]
        [PfeColumn(Text = "_KOD_EXT")]
        public string KOD_EXT { get; set; }

        [DataMember]
        [PfeColumn(Text = "_NAZOV_EXT")]
        public string NAZOV_EXT { get; set; }

        [DataMember]
        [PfeColumn(Text = "_POC_KS_ORDER")]
        public decimal? POC_KS_ORDER { get; set; }

        [DataMember]
        [PfeColumn(Text = "_OBP_ORDER")]
        public string OBP_ORDER { get; set; }

        [DataMember]
        [PfeColumn(Text = "_WT")]
        public decimal? WT { get; set; }

        [DataMember]
        [PfeColumn(Text = "_WT_TOTAL")]
        public decimal? WT_TOTAL { get; set; }

        [DataMember]
        [PfeColumn(Text = "_WT_MJ")]
        public string WT_MJ { get; set; }

        [DataMember]
        [PfeColumn(Text = "_AR")]
        public decimal? AR { get; set; }

        [DataMember]
        [PfeColumn(Text = "_AR_TOTAL")]
        public decimal? AR_TOTAL { get; set; }

        [DataMember]
        [PfeColumn(Text = "_AR_MJ")]
        public string AR_MJ { get; set; }

        [DataMember]
        [PfeColumn(Text = "_HUST")]
        public decimal? HUST { get; set; }

        [DataMember]
        [PfeColumn(Text = "_CUST_STR_1")]
        public string CUST_STR_1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_CUST_STR_2")]
        public string CUST_STR_2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_CUST_NUM_1")]
        public decimal? CUST_NUM_1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_CUST_NUM_2")]
        public decimal? CUST_NUM_2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_CUST_BLN_1")]
        public bool? CUST_BLN_1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_CUST_BLN_2")]
        public bool? CUST_BLN_2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_VALID_TO_STS")]
        public DateTime? VALID_TO_STS { get; set; }

        [DataMember]
        [PfeColumn(Text = "_MIN_MN")]
        public decimal? MIN_MN { get; set; }

        [DataMember]
        [PfeColumn(Text = "_MAX_MN")]
        public decimal? MAX_MN { get; set; }

        [DataMember]
        [PfeColumn(Text = "_N_CENA")]
        public decimal? N_CENA { get; set; }

        [DataMember]
        [PfeColumn(Text = "_K_MEN_0_N_CENA")]
        public int? K_MEN_0_N_CENA { get; set; }

        [DataMember]
        [PfeColumn(Text = "_PC1")]
        public decimal? PC1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_PC2")]
        public decimal? PC2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_PC3")]
        public decimal? PC3 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_PC4")]
        public decimal? PC4 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_PC5")]
        public decimal? PC5 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_PC6")]
        public decimal? PC6 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_PC7")]
        public decimal? PC7 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_PC8")]
        public decimal? PC8 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_PC9")]
        public decimal? PC9 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_VYP")]
        public bool? VYP { get; set; }

        [DataMember]
        [PfeColumn(Text = "_DAT_PRI")]
        public DateTime? DAT_PRI { get; set; }

        [DataMember]
        [PfeColumn(Text = "_OUTER_SIZE")]
        public string OUTER_SIZE { get; set; }
    }
}
