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
        [PfeColumn(Text = "_D_PRI_1")]
        public int D_PRI_1 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_D_PRI_0")]
        public int? D_PRI_0 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_K_TSK_0")]
        public int? K_TSK_0 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_K_TYP_0")]
        public int K_TYP_0 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_KOD")]
        public string KOD { get; set; }
        [DataMember]
        [PfeColumn(Text = "_NAZOV")]
        public string NAZOV { get; set; }
        [DataMember]
        [PfeColumn(Text = "_POC_KS")]
        public decimal? POC_KS { get; set; }
        [DataMember]
        [PfeColumn(Text = "_MJ")]
        public string MJ { get; set; }
        [DataMember]
        [PfeColumn(Text = "_D_CENA")]
        public decimal? D_CENA { get; set; }
        [DataMember]
        [PfeColumn(Text = "_Z_CENA")]
        public decimal? Z_CENA { get; set; }
        [DataMember]
        [PfeColumn(Text = "_RANK")]
        public int? RANK { get; set; }
        [DataMember]
        [PfeColumn(Text = "_BAL_KS")]
        public decimal? BAL_KS { get; set; }
        [DataMember]
        [PfeColumn(Text = "_EAN")]
        public string EAN { get; set; }
        [DataMember]
        [PfeColumn(Text = "_WARRANTY")]
        public int? WARRANTY { get; set; }
        [DataMember]
        [PfeColumn(Text = "_SN")]
        public string SN { get; set; }
        [DataMember]
        [PfeColumn(Text = "_LOCATION")]
        public string LOCATION { get; set; }
        [DataMember]
        [PfeColumn(Text = "_KOD_EXT")]
        public string KOD_EXT { get; set; }
        [DataMember]
        [PfeColumn(Text = "_NAZOV_EXT")]
        public string NAZOV_EXT { get; set; }
        [DataMember]
        [PfeColumn(Text = "_SARZA")]
        public string SARZA { get; set; }
    }
}
