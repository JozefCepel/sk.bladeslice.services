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
        [PfeCombo(typeof(PV3DCombo), NameColumn = "PV")]
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
        [PfeColumn(Text = "Príjemka")]
        [PfeCombo(typeof(tblD_PRI_0), IdColumnCombo = "D_PRI_0", DisplayColumn = "DKL_C")]
        public string DKL_C_PRI { get; set; }

        [DataMember]
        [PfeColumn(Text = "Výdajka")]
        [PfeCombo(typeof(tblD_VYD_0), IdColumnCombo = "D_VYD_0", DisplayColumn = "DKL_C")]
        public string DKL_C_VYD { get; set; }

        [DataMember]
        [PfeColumn(Text = "Položka príjemky", RequiredFields = new[] { "D_PRI_0" })]
        [PfeCombo(typeof(V_PRI_1View), IdColumnCombo = "D_PRI_1", DisplayColumn = "PriPol")]
        public string PriPol { get; set; }

        [DataMember]
        [PfeColumn(Text = "Položka výdajky", RequiredFields = new[] { "D_VYD_0" })]
        [PfeCombo(typeof(V_VYD_1View), IdColumnCombo = "D_VYD_1", DisplayColumn = "VydPol")]
        public string VydPol { get; set; }

        [DataMember]
        [PfeColumn(Text = "V", Editable = false, ReadOnly = true)]
        public bool V { get; set; }
        
        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true)]
        public string ZmenilMeno { get; set; }
    }
}
