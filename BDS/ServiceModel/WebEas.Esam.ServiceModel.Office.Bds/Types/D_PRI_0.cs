using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("dbo")]
    [Alias("D_PRI_0")]
    [DataContract]
    public class tblD_PRI_0 : BaseEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public int D_PRI_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_K_OBP_0", Mandatory = true)]
        public int? K_OBP_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_K_SKL_0")]
        public int K_SKL_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_K_MEN_0")]
        public int K_MEN_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_K_POH_0")]
        public int K_POH_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_K_ORJ_0")]
        public int K_ORJ_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_KURZ")]
        public decimal? KURZ { get; set; }

        [DataMember]
        [PfeColumn(Text = "_PS", DefaultValue = false)]
        public bool PS { get; set; }

        [DataMember]
        [PfeColumn(Text = "Doc. number", Editable = false)]
        public string DKL_C { get; set; }

        [DataMember]
        [PfeColumn(Text = "Delivery No.")]
        public string DL_C { get; set; }

        [DataMember]
        [PfeColumn(Text = "V", DefaultValue = false, Editable = false)]
        public bool V { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Z", DefaultValue = false)]
        public bool Z { get; set; }

        [DataMember]
        [PfeColumn(Text = "Date", Type = PfeDataType.Date, Mandatory = true)]
        public DateTime DAT_DKL { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Poznámka")]
        public string POZN { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Z_SUMA", DefaultValue = 0)]
        public decimal Z_SUMA { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_SUMA", DefaultValue = 0)]
        public decimal D_SUMA { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_DPH1", DefaultValue = 0)]
        public decimal D_DPH1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_DPH2", DefaultValue = 0)]
        public decimal D_DPH2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_SO", DefaultValue = 0)]
        public byte SO { get; set; }
    }
}
