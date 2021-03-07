using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("bds")]
    [Alias("V_PRI_1")]
    [DataContract]
    public class V_PRI_1View : tblD_PRI_1
    {
        [DataMember]
        [PfeColumn(Text = "Code", RequiredFields = new[] { "K_TSK_0" }, Mandatory = true)] //Pomocne pole iba pre ucel COMBO POLA
        [PfeCombo(typeof(tblK_MAT_0), ComboIdColumn = "KOD", ComboDisplayColumn = "KOD")]
        public string KOD_ID { get; set; }

        [DataMember]
        [PfeColumn(Text = "Receipt No.")]
        [PfeCombo(typeof(tblD_PRI_0), ComboIdColumn = "D_PRI_0", ComboDisplayColumn = "DKL_C")]
        public string DKL_C { get; set; }

        [DataMember]
        [PfeColumn(Text = "V", Editable = false, ReadOnly = true)]
        public bool V { get; set; }

        [DataMember]
        [PfeColumn(Text = "Receipt item", ReadOnly = true)]
        public string PriPol { get; set; }

        [DataMember]
        [PfeColumn(Text = "_K_SKL_0", ReadOnly = true)] //Iba kvoli RequiredField
        public int K_SKL_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Mat. group", RequiredFields = new[] { "K_SKL_0" })]
        [PfeCombo(typeof(V_SKL_1View), ComboIdColumn = "K_TSK_0", ComboDisplayColumn = "TSK")]
        public string TSK { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Created by", Hidden = true, Editable = false, ReadOnly = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Edited by", Hidden = true, Editable = false, ReadOnly = true)]
        public string ZmenilMeno { get; set; }
    }
}
