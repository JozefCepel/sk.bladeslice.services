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
        public int K_MAT_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_K_TSK_0")]
        public int K_TSK_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Code", Mandatory = true)]
        public string KOD { get; set; }

        [DataMember]
        [PfeColumn(Text = "Name", Mandatory = true)]
        public string NAZOV { get; set; }

        [DataMember]
        [PfeColumn(Text = "_DPH", DefaultValue = 0)]
        public decimal DPH { get; set; }

        [DataMember]
        [PfeColumn(Text = "UoM", Tooltip = "Unit of Measure")]
        public string MJ { get; set; }

        [DataMember]
        [PfeColumn(Text = "Note")]
        public string POZN { get; set; }

        [DataMember]
        [PfeColumn(Text = "Purchase price", DefaultValue = 0)]
        public decimal N_CENA { get; set; }

        [DataMember]
        [PfeColumn(Text = "Price 1", DefaultValue = 0)]
        public decimal PC1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Price 2", DefaultValue = 0)]
        public decimal PC2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Price 3", DefaultValue = 0)]
        public decimal PC3 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Price 4", DefaultValue = 0)]
        public decimal PC4 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Price 5", DefaultValue = 0)]
        public decimal PC5 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Price 6", DefaultValue = 0)]
        public decimal PC6 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Price 7", DefaultValue = 0)]
        public decimal PC7 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Price 8", DefaultValue = 0)]
        public decimal PC8 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Price 9", DefaultValue = 0)]
        public decimal PC9 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Min. quantity", DefaultValue = 0)]
        public decimal MIN_MN { get; set; }

        [DataMember]
        [PfeColumn(Text = "Max. quantity", DefaultValue = 0)]
        public decimal MAX_MN { get; set; }

        [DataMember]
        [PfeColumn(Text = "EAN code")]
        public string EAN { get; set; }

        [DataMember]
        [PfeColumn(Text = "Warranty", DefaultValue = 0)]
        public int WARRANTY { get; set; }

        [DataMember]
        [PfeColumn(Text = "_VRB_INE", DefaultValue = 0)]
        public decimal VRB_INE { get; set; }

        [DataMember]
        [PfeColumn(Text = "Weight / UoM", DefaultValue = 0)]
        public decimal WT { get; set; }

        [DataMember]
        [PfeColumn(Text = "Weight UoM")]
        public string WT_MJ { get; set; }

        [DataMember]
        [PfeColumn(Text = "IST code")]
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
        [PfeColumn(Text = "Density", DefaultValue = 0)]
        public decimal HUST { get; set; }
    }
}
