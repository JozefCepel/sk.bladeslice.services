using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("dbo")]
    [Alias("V_VYD_1")]
    [DataContract]
    public class V_VYD_1View : tblD_VYD_1
    {
        [DataMember]
        [PfeColumn(Text = "Číslo výdajky")]
        [PfeCombo(typeof(tblD_VYD_0), IdColumnCombo = "D_VYD_0", DisplayColumn = "DKL_C")]
        public string DKL_C { get; set; }

        [DataMember]
        [PfeColumn(Text = "V", Editable = false, ReadOnly = true)]
        public bool V { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Položka výdajky", ReadOnly = true)]
        public string VydPol { get; set; }

        [DataMember]
        [PfeColumn(Text = "_K_SKL_0", ReadOnly = true)] //Iba kvoli RequiredField
        public int K_SKL_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Mat. group", RequiredFields = new[] { "K_SKL_0" })]
        [PfeCombo(typeof(V_SKL_1View), IdColumnCombo = "K_TSK_0", DisplayColumn = "TSK")]
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
