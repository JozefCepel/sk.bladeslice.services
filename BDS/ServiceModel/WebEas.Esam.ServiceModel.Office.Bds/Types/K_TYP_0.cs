using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("dbo")]
    [Alias("K_TYP_0")]
    [DataContract]
    public class tblK_TYP_0 : BaseEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        [PfeColumn(Text = "_K_TYP_0")]
        public int K_TYP_0 { get; set; }
        [DataMember]
        [PfeColumn(Text = "Názov")]
        public string NAZOV { get; set; }
        [DataMember]
        [PfeColumn(Text = "_DPH_0")]
        public bool? DPH_0 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_DPH_1")]
        public bool? DPH_1 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_DPH_2")]
        public bool? DPH_2 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_UCT_DKL")]
        public bool? UCT_DKL { get; set; }
        [DataMember]
        [PfeColumn(Text = "_HLASENIE_DPH")]
        public bool? HLASENIE_DPH { get; set; }
        [DataMember]
        [PfeColumn(Text = "_KAT_MAT")]
        public bool? KAT_MAT { get; set; }
        [DataMember]
        [PfeColumn(Text = "_CUST_NAZOV")]
        public string CUST_NAZOV { get; set; }
        [DataMember]
        [PfeColumn(Text = "_IS_HIDDEN")]
        public bool IS_HIDDEN { get; set; }
        [DataMember]
        [PfeColumn(Text = "_IS_CUST")]
        public bool IS_CUST { get; set; }
        [DataMember]
        [PfeColumn(Text = "_TYP_RANK")]
        public int? TYP_RANK { get; set; }
    }
}
