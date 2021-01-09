using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("dbo")]
    [Alias("D_PRI_1")]
    [DataContract]
    public class tblD_PRI_1 : BaseEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public int D_PRI_1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_PRI_0")]
        public int? D_PRI_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_K_TSK_0", Mandatory = true)]
        public int K_TSK_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_K_TYP_0")]
        public int K_TYP_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_KOD", Mandatory = true)] //Pole sa napĺňa za KOD_ID, na ktorom je combo
        public string KOD { get; set; }

        [DataMember]
        [PfeColumn(Text = "Name")]
        public string NAZOV { get; set; }

        [DataMember]
        [PfeColumn(Text = "Amount", DefaultValue = 0, Mandatory = true)]
        public decimal? POC_KS { get; set; }

        [DataMember]
        [PfeColumn(Text = "UoM", Tooltip = "Unit of Measure")]
        public string MJ { get; set; }

        [DataMember]
        [PfeColumn(Text = "Price", DefaultValue = 0)]
        public decimal? D_CENA { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Z_CENA", DefaultValue = 0)]
        public decimal? Z_CENA { get; set; }

        [DataMember]
        [PfeColumn(Text = "R", DefaultValue = 1, Mandatory = true)]
        public int? RANK { get; set; }

        [DataMember]
        [PfeColumn(Text = "_BAL_KS", DefaultValue = 0)]
        public decimal? BAL_KS { get; set; }

        [DataMember]
        [PfeColumn(Text = "EAN code")]
        public string EAN { get; set; }

        [DataMember]
        [PfeColumn(Text = "_WARRANTY", DefaultValue = 0)]
        public int WARRANTY { get; set; }

        [DataMember]
        [PfeColumn(Text = "Serial No.")]
        public string SN { get; set; }

        [DataMember]
        [PfeColumn(Text = "Position")]
        public string LOCATION { get; set; }

        [DataMember]
        [PfeColumn(Text = "_KOD_EXT")]
        public string KOD_EXT { get; set; }

        [DataMember]
        [PfeColumn(Text = "_NAZOV_EXT")]
        public string NAZOV_EXT { get; set; }

        [DataMember]
        [PfeColumn(Text = "Batch")]
        public string SARZA { get; set; }
    }
}
