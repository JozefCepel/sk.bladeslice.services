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
        public int K_TYP_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov", Mandatory = true)]
        public string NAZOV { get; set; }

        [DataMember]
        [PfeColumn(Text = "_DPH_0", DefaultValue = false)]
        public bool? DPH_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_DPH_1", DefaultValue = false)]
        public bool? DPH_1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_DPH_2", DefaultValue = false)]
        public bool? DPH_2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_UCT_DKL", DefaultValue = false)]
        public bool? UCT_DKL { get; set; }

        [DataMember]
        [PfeColumn(Text = "_HLASENIE_DPH", DefaultValue = false)]
        public bool? HLASENIE_DPH { get; set; }

        [DataMember]
        [PfeColumn(Text = "_KAT_MAT", DefaultValue = false)]
        public bool? KAT_MAT { get; set; }

        [DataMember]
        [PfeColumn(Text = "_CUST_NAZOV")]
        public string CUST_NAZOV { get; set; }

        [DataMember]
        [PfeColumn(Text = "_IS_HIDDEN", DefaultValue = false)]
        public bool IS_HIDDEN { get; set; }

        [DataMember]
        [PfeColumn(Text = "_IS_CUST", DefaultValue = false)]
        public bool IS_CUST { get; set; }

        [DataMember]
        [PfeColumn(Text = "_TYP_RANK", DefaultValue = 0)]
        public int? TYP_RANK { get; set; }
    }
}
