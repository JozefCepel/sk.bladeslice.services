using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("dbo")]
    [Alias("D_SIM_0")]
    [DataContract]
    public class tblD_SIM_0 : BaseEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        [PfeColumn(Text = "_D_SIM_0")]
        public int D_SIM_0 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_D_PRI_0")]
        public int? D_PRI_0 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_D_PRI_1")]
        public int? D_PRI_1 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_D_VYD_0")]
        public int? D_VYD_0 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_D_VYD_1")]
        public int? D_VYD_1 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_RANK")]
        public int? RANK { get; set; }
        [DataMember]
        [PfeColumn(Text = "_PV")]
        public byte PV { get; set; }
        [DataMember]
        [PfeColumn(Text = "_SN")]
        public string SN { get; set; }
        [DataMember]
        [PfeColumn(Text = "_A1")]
        public int A1 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_A2")]
        public int A2 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_B1")]
        public int B1 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_B2")]
        public int B2 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_D1")]
        public int D1 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_D2")]
        public int D2 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_L1")]
        public int L1 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_L2")]
        public int L2 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_POC_KS")]
        public int POC_KS { get; set; }
        [DataMember]
        [PfeColumn(Text = "_POC_KS_VYREZ")]
        public decimal? POC_KS_VYREZ { get; set; }
        [DataMember]
        [PfeColumn(Text = "_POC_KS_ZVYSOK")]
        public decimal? POC_KS_ZVYSOK { get; set; }
        [DataMember]
        [PfeColumn(Text = "_ZVYSOK_SPOTREBA")]
        public bool ZVYSOK_SPOTREBA { get; set; }
        [DataMember]
        [PfeColumn(Text = "_POZN")]
        public string POZN { get; set; }
        [DataMember]
        [PfeColumn(Text = "_SARZA")]
        public string SARZA { get; set; }
        [DataMember]
        [PfeColumn(Text = "_LOCATION")]
        public string LOCATION { get; set; }
        [DataMember]
        [PfeColumn(Text = "_SKL_CENA")]
        public decimal SKL_CENA { get; set; }
        [DataMember]
        [PfeColumn(Text = "_POC_KS_PLT")]
        public decimal POC_KS_PLT { get; set; }
        [DataMember]
        [PfeColumn(Text = "_OUTER_SIZE")]
        public string OUTER_SIZE { get; set; }
        [DataMember]
        [PfeColumn(Text = "_OUTER_SIZE_FINAL")]
        public string OUTER_SIZE_FINAL { get; set; }
    }
}
