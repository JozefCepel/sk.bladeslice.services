using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("dbo")]
    [Alias("STS_BDS")]
    [DataContract]
    public class STSView
    {
        //[DataMember]
        //[PfeColumn(Text = "_K_ORJ_0")]
        //public int K_ORJ_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Organisation unit")]
        [PfeCombo(typeof(tblK_ORJ_0), IdColumnCombo = "K_ORJ_0", DisplayColumn = "ORJ")]
        public string ORJ { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_K_SKL_0")]
        //public int? K_SKL_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Warehouse")]
        [PfeCombo(typeof(tblK_SKL_0), IdColumnCombo = "K_SKL_0", DisplayColumn = "SKL")]
        public string Sklad { get; set; }

        [DataMember]
        [PfeColumn(Text = "Poč. stav")]
        public bool? PS { get; set; }

        [DataMember]
        [PfeColumn(Text = "Date", Type = PfeDataType.Date)]
        public DateTime? Datum { get; set; }

        [DataMember]
        [PfeColumn(Text = "R")]
        public int? RANK { get; set; }

        [DataMember]
        [PfeColumn(Text = "_PV")]
        public short PV { get; set; }

        [DataMember]
        [PfeCombo(typeof(PrijemVydajCombo), NameColumn = "PV")]
        [PfeColumn(Text = "P/V", Tooltip = "Či ide o príjmovú alebo výdajovú položku")]
        [Ignore]
        public string PrijemVydajText
        {
            get
            {
                return PrijemVydajCombo.GetText(PV);
            }
        }

        [DataMember]
        [PfeColumn(Text = "_ID")]
        public int ID { get; set; }

        [DataMember]
        [PfeColumn(Text = "Doc. number")]
        public string DKL_C { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_K_TSK_0")]
        //public int? K_TSK_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Mat. group", RequiredFields = new[] { "K_SKL_0" })]
        [PfeCombo(typeof(V_SKL_1View), IdColumnCombo = "K_TSK_0", DisplayColumn = "TSK")]
        public string TSK { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_K_TYP_0")]
        //public int K_TYP_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Code")]
        public string KOD { get; set; }

        [DataMember]
        [PfeColumn(Text = "EAN code")]
        public string EAN { get; set; }

        [DataMember]
        [PfeColumn(Text = "Position")]
        public string LOCATION { get; set; }

        [DataMember]
        [PfeColumn(Text = "SN")]
        public string SN { get; set; }

        [DataMember]
        [PfeColumn(Text = "Batch")]
        public string SARZA { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_WARRANTY")]
        //public int? WARRANTY { get; set; }

        [DataMember]
        [PfeColumn(Text = "Name")]
        public string NAZOV { get; set; }

        [DataMember]
        [PfeColumn(Text = "UoM", Tooltip = "Unit of Measure")]
        public string MJ { get; set; }

        [DataMember]
        [PfeColumn(Text = "Number of pieces")]
        public decimal? KS { get; set; }

        [DataMember]
        [PfeColumn(Text = "Price")]
        public decimal? SKL_CENA { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_ROZDIEL_OCENENIA")]
        //public decimal? ROZDIEL_OCENENIA { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_DPH")]
        //public decimal? DPH { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_CUST_STR_DKL_1")]
        //public string CUST_STR_DKL_1 { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_CUST_STR_DKL_2")]
        //public string CUST_STR_DKL_2 { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_CUST_STR_DKL_3")]
        //public string CUST_STR_DKL_3 { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_CUST_NUM_DKL_1")]
        //public decimal CUST_NUM_DKL_1 { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_CUST_NUM_DKL_2")]
        //public decimal CUST_NUM_DKL_2 { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_CUST_NUM_DKL_3")]
        //public decimal CUST_NUM_DKL_3 { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_CUST_BLN_DKL_1")]
        //public bool CUST_BLN_DKL_1 { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_CUST_BLN_DKL_2")]
        //public bool CUST_BLN_DKL_2 { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_CUST_BLN_DKL_3")]
        //public bool CUST_BLN_DKL_3 { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_CUST_DAT_DKL_1")]
        //public DateTime? CUST_DAT_DKL_1 { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_CUST_DAT_DKL_2")]
        //public DateTime? CUST_DAT_DKL_2 { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_CUST_DAT_DKL_3")]
        //public DateTime? CUST_DAT_DKL_3 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Item note")]
        public string POZN_POL { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_UCTY")]
        //public string UCTY { get; set; }

        [DataMember]
        [PfeColumn(Text = "_DKL_ID")]
        public int DKL_ID { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_K_OBP_0")]
        //public int? K_OBP_0 { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_VYP")]
        //public bool? VYP { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_STATUS")]
        //public int? STATUS { get; set; }

        [DataMember]
        [PfeColumn(Text = "Type")]
        public string DKL_FROM { get; set; }

        [DataMember]
        [PfeColumn(Text = "_SKL_SORT")]
        public string SKL_SORT { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_K_POH_0")]
        //public int K_POH_0 { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_OBJ")]
        //public int OBJ { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_D_FSK_0")]
        //public int? D_FSK_0 { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_PRE_OCENENIE")]
        //public int PRE_OCENENIE { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_Typ")]
        //public string Typ { get; set; }

        [DataMember]
        [PfeColumn(Text = "Business partner")]
        public string OBP { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_ID_KAT")]
        //public int? ID_KAT { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "Nákupná cena - katalóg")]
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
        //[PfeColumn(Text = "_DPH_KAT")]
        //public decimal? DPH_KAT { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_NAZOV_KAT")]
        //public string NAZOV_KAT { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_MJ_KAT")]
        //public string MJ_KAT { get; set; }

        [DataMember]
        [PfeColumn(Text = "Catalogue note")]
        public string POZN_KAT { get; set; }

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
        //[PfeColumn(Text = "_OBP_ORDER")]
        //public string OBP_ORDER { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_POC_KS_ORDER")]
        //public decimal? POC_KS_ORDER { get; set; }

        [DataMember]
        [PfeColumn(Text = "Weight / UoM", DefaultValue = 0)]
        public decimal? WT { get; set; }

        [DataMember]
        [PfeColumn(Text = "Weight UoM")]
        public string WT_MJ { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_AR")]
        //public decimal? AR { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_AR_MJ")]
        //public string AR_MJ { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_WARRANTY_KAT")]
        //public int? WARRANTY_KAT { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_MIN_MN")]
        //public decimal? MIN_MN { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_MAX_MN")]
        //public decimal? MAX_MN { get; set; }

        [DataMember]
        [PfeColumn(Text = "Density", DefaultValue = 0)]
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

        [DataMember]
        [PfeColumn(Text = "Warehouse group", WithoutData = true)]
        public string SKL_GRP { get; set; }
    }
}
