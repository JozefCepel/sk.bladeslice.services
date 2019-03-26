using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("dbo")]
    [Alias("V_PRI_1")]
    [DataContract]
    public class V_PRI_1View : tblD_PRI_1
    {
        [DataMember]
        [PfeColumn(Text = "Číslo príjemky")]
        [PfeCombo(typeof(tblD_PRI_0), IdColumnCombo = "D_PRI_0", DisplayColumn = "DKL_C")]
        public string DKL_C { get; set; }

        [DataMember]
        [PfeColumn(Text = "V", Editable = false, ReadOnly = true)]
        public bool V { get; set; }

        [DataMember]
        [PfeColumn(Text = "Položka príjemky", ReadOnly = true)]
        public string PriPol { get; set; }

        [DataMember]
        [PfeColumn(Text = "Skupina")]
        public string TSK { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true)]
        public string ZmenilMeno { get; set; }
    }
}
