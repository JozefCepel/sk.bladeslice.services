using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("dbo")]
    [Alias("V_SIM_0")]
    [DataContract]
    public class V_SIM_0View : tblD_SIM_0
    {
        [DataMember]
        [PfeCombo(typeof(PV3DCombo), IdColumn = "PV")]
        [PfeColumn(Text = "Typ")]
        [Ignore]
        public string PVText
        {
            get
            {
                return PV3DCombo.GetText(PV);
            }
        }

        [DataMember]
        [PfeColumn(Text = "Receipt")]
        [PfeCombo(typeof(tblD_PRI_0), ComboIdColumn = "D_PRI_0", ComboDisplayColumn = "DKL_C")] //, AdditionalWhereSql = "V = 0"
        public string DKL_C_PRI { get; set; }

        [DataMember]
        [PfeColumn(Text = "Expense")]
        [PfeCombo(typeof(tblD_VYD_0), ComboIdColumn = "D_VYD_0", ComboDisplayColumn = "DKL_C")] //, AdditionalWhereSql = "V = 0"
        public string DKL_C_VYD { get; set; }

        [DataMember]
        [PfeColumn(Text = "Receipt item", RequiredFields = new[] { "D_PRI_0" })]
        [PfeCombo(typeof(V_PRI_1View), ComboIdColumn = "D_PRI_1", ComboDisplayColumn = "PriPol")]
        public string PriPol { get; set; }

        [DataMember]
        [PfeColumn(Text = "Expense item", RequiredFields = new[] { "D_VYD_0" })]
        [PfeCombo(typeof(V_VYD_1View), ComboIdColumn = "D_VYD_1", ComboDisplayColumn = "VydPol")]
        public string VydPol { get; set; }

        [DataMember]
        [PfeColumn(Text = "V", Editable = false, ReadOnly = true)]
        public bool V { get; set; }

        [DataMember]
        [PfeColumn(Text = "_K_TSK_0", Editable = false, ReadOnly = true)]
        public int K_TSK_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_K_TYP_0", Editable = false, ReadOnly = true)]
        public int K_TYP_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Code", Editable = false, ReadOnly = true)]
        public string KOD { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Created by", Hidden = true, Editable = false, ReadOnly = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Edited by", Hidden = true, Editable = false, ReadOnly = true)]
        public string ZmenilMeno { get; set; }
    }
}
