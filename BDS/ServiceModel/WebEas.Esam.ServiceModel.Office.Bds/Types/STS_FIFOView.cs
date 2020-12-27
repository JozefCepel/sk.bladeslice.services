using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("dbo")]
    [Alias("STS_FIFO_BDS")]
    [DataContract]
    public class STS_FIFOView
    {
        //[DataMember]
        //[PfeColumn(Text = "_K_SKL_0")]
        //public int? K_SKL_0 { get; set; }

        [PrimaryKey]
        [DataMember]
        [PfeColumn(Text = "ID_POL")]
        public string ID_POL { get; set; }

        [DataMember]
        [PfeColumn(Text = "ID_JOIN")]
        public string ID_JOIN { get; set; }

        [DataMember]
        [PfeColumn(Text = "Warehouse")]
        [PfeCombo(typeof(tblK_SKL_0), ComboIdColumn = "K_SKL_0", ComboDisplayColumn = "SKL")]
        public string Sklad { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_K_TSK_0")]
        //public int? K_TSK_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Mat. group", RequiredFields = new[] { "K_SKL_0" })]
        [PfeCombo(typeof(V_SKL_1View), ComboIdColumn = "K_TSK_0", ComboDisplayColumn = "TSK")]
        public string TSK { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_K_TYP_0")]
        //public int K_TYP_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Warehouse group",LoadWhenVisible = true)]
        public string SKL_GRP { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_Typ")]
        //public string Typ { get; set; }

        [DataMember]
        [PfeColumn(Text = "Code")]
        public string KOD { get; set; }

        [DataMember]
        [PfeColumn(Text = "EAN code")]
        public string EAN { get; set; }

        [DataMember]
        [PfeColumn(Text = "Name")]
        public string NAZOV { get; set; }

        [DataMember]
        [PfeColumn(Text = "UoM", Tooltip = "Unit of Measure")]
        public string MJ { get; set; }

        [DataMember]
        [PfeColumn(Text = "Position")]
        public string LOCATION { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_WARRANTY")]
        //public int? WARRANTY { get; set; }

        [DataMember]
        [PfeColumn(Text = "Note")]
        public string POZN { get; set; }

        [DataMember]
        [PfeColumn(Text = "Number of pieces")]
        public decimal? POC_KS { get; set; }

        [DataMember]
        [PfeColumn(Text = "Number of pieces - IN")]
        public decimal? POC_KS_P { get; set; }

        [DataMember]
        [PfeColumn(Text = "Number of pieces - OUT")]
        public decimal? POC_KS_V { get; set; }

        [DataMember]
        [PfeColumn(Text = "Price")]
        public decimal? SKL_CENA { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_SKL_CENA_NO_ZERO")]
        //public bool? SKL_CENA_NO_ZERO { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_UCT_SUMA")]
        //public decimal? UCT_SUMA { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_DPH")]
        //public decimal? DPH { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_ID_KAT")]
        //public int? ID_KAT { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_ATT_FILE_NAME")]
        //public string ATT_FILE_NAME { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_KOD_EXT")]
        //public string KOD_EXT { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_NAZOV_EXT")]
        //public string NAZOV_EXT { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_POC_KS_ORDER")]
        //public decimal? POC_KS_ORDER { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_OBP_ORDER")]
        //public string OBP_ORDER { get; set; }

        [DataMember]
        [PfeColumn(Text = "Weight / UoM", DefaultValue = 0)]
        public decimal? WT { get; set; }

        [DataMember]
        [PfeColumn(Text = "Weight UoM")]
        public string WT_MJ { get; set; }

        [DataMember]
        [PfeColumn(Text = "_WT_TOTAL")]
        public decimal? WT_TOTAL { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_AR")]
        //public decimal? AR { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_AR_TOTAL")]
        //public decimal? AR_TOTAL { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_AR_MJ")]
        //public string AR_MJ { get; set; }

        [DataMember]
        [PfeColumn(Text = "Density")]
        public decimal? HUST { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_CUST_STR_1")]
        //public string CUST_STR_1 { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_CUST_STR_2")]
        //public string CUST_STR_2 { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_CUST_NUM_1")]
        //public decimal? CUST_NUM_1 { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_CUST_NUM_2")]
        //public decimal? CUST_NUM_2 { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_CUST_BLN_1")]
        //public bool? CUST_BLN_1 { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_CUST_BLN_2")]
        //public bool? CUST_BLN_2 { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_VALID_TO_STS")]
        //public DateTime? VALID_TO_STS { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_MIN_MN")]
        //public decimal? MIN_MN { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_MAX_MN")]
        //public decimal? MAX_MN { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_N_CENA")]
        //public decimal? N_CENA { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_K_MEN_0_N_CENA")]
        //public int? K_MEN_0_N_CENA { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_PC1")]
        //public decimal? PC1 { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_PC2")]
        //public decimal? PC2 { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_PC3")]
        //public decimal? PC3 { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_PC4")]
        //public decimal? PC4 { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_PC5")]
        //public decimal? PC5 { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_PC6")]
        //public decimal? PC6 { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_PC7")]
        //public decimal? PC7 { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_PC8")]
        //public decimal? PC8 { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_PC9")]
        //public decimal? PC9 { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_VYP")]
        //public bool? VYP { get; set; }

        [DataMember]
        [PfeColumn(Text = "Serial No.")]
        public string SN { get; set; }

        [DataMember]
        [PfeColumn(Text = "Batch")]
        public string SARZA { get; set; }

        [DataMember]
        [PfeColumn(Text = "Last reception", Type = PfeDataType.Date)]
        public DateTime? DAT_PRI { get; set; }

        [DataMember]
        [PfeColumn(Text = "Outer size")]
        public string OUTER_SIZE { get; set; }
    }
}
