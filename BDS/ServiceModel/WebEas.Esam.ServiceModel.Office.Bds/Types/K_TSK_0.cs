using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("dbo")]
    [Alias("K_TSK_0")]
    [DataContract]
    public class tblK_TSK_0 : BaseEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        [PfeColumn(Text = "_K_TSK_0")]
        public int K_TSK_0 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_TSK")]
        public string TSK { get; set; }
        [DataMember]
        [PfeColumn(Text = "_POZN")]
        public string POZN { get; set; }
        [DataMember]
        [PfeColumn(Text = "_TOVAR")]
        public bool? TOVAR { get; set; }
        [DataMember]
        [PfeColumn(Text = "_MATERIAL")]
        public bool? MATERIAL { get; set; }
        [DataMember]
        [PfeColumn(Text = "_OSIVO")]
        public bool? OSIVO { get; set; }
        [DataMember]
        [PfeColumn(Text = "_PREP_K")]
        public decimal? PREP_K { get; set; }
        [DataMember]
        [PfeColumn(Text = "_PREP_M")]
        public decimal? PREP_M { get; set; }
        [DataMember]
        [PfeColumn(Text = "_BALENIE")]
        public bool? BALENIE { get; set; }
        [DataMember]
        [PfeColumn(Text = "_VYROBOK")]
        public bool? VYROBOK { get; set; }
        [DataMember]
        [PfeColumn(Text = "_OM1")]
        public decimal? OM1 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_OM2")]
        public decimal? OM2 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_OM3")]
        public decimal? OM3 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_OM4")]
        public decimal? OM4 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_OM5")]
        public decimal? OM5 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_PREP_M1")]
        public byte? PREP_M1 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_SKL_SIMULATION")]
        public byte SKL_SIMULATION { get; set; }
        [DataMember]
        [PfeColumn(Text = "_OM6")]
        public decimal OM6 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_OM7")]
        public decimal OM7 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_OM8")]
        public decimal OM8 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_OM9")]
        public decimal OM9 { get; set; }
    }
}
