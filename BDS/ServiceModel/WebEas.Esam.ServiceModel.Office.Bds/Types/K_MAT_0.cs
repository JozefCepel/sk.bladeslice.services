using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("dbo")]
    [Alias("K_MAT_0")]
    [DataContract]
    public class tblK_MAT_0 : BaseEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        [PfeColumn(Text = "_K_MAT_0")]
        public int K_MAT_0 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_K_TSK_0")]
        public int K_TSK_0 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_KOD")]
        public string KOD { get; set; }
        [DataMember]
        [PfeColumn(Text = "_NAZOV")]
        public string NAZOV { get; set; }
        [DataMember]
        [PfeColumn(Text = "_DPH")]
        public decimal? DPH { get; set; }
        [DataMember]
        [PfeColumn(Text = "_MJ")]
        public string MJ { get; set; }
        [DataMember]
        [PfeColumn(Text = "_POZN")]
        public string POZN { get; set; }
        [DataMember]
        [PfeColumn(Text = "_N_CENA")]
        public decimal? N_CENA { get; set; }
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
        [PfeColumn(Text = "_MIN_MN")]
        public decimal? MIN_MN { get; set; }
        [DataMember]
        [PfeColumn(Text = "_MAX_MN")]
        public decimal? MAX_MN { get; set; }
        [DataMember]
        [PfeColumn(Text = "_EAN")]
        public string EAN { get; set; }
        [DataMember]
        [PfeColumn(Text = "_WARRANTY")]
        public int? WARRANTY { get; set; }
        [DataMember]
        [PfeColumn(Text = "_VRB_INE")]
        public decimal? VRB_INE { get; set; }
        [DataMember]
        [PfeColumn(Text = "_WT")]
        public decimal WT { get; set; }
        [DataMember]
        [PfeColumn(Text = "_WT_MJ")]
        public string WT_MJ { get; set; }
        [DataMember]
        [PfeColumn(Text = "_IST")]
        public string IST { get; set; }
        [DataMember]
        [PfeColumn(Text = "_KOD_EXT")]
        public string KOD_EXT { get; set; }
        [DataMember]
        [PfeColumn(Text = "_NAZOV_EXT")]
        public string NAZOV_EXT { get; set; }
        [DataMember]
        [PfeColumn(Text = "_VALID_TO")]
        public DateTime? VALID_TO { get; set; }
        [DataMember]
        [PfeColumn(Text = "_HUST")]
        public decimal HUST { get; set; }
        [DataMember]
        [PfeColumn(Text = "_PC6")]
        public decimal PC6 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_PC7")]
        public decimal PC7 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_PC8")]
        public decimal PC8 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_PC9")]
        public decimal PC9 { get; set; }
    }
}
