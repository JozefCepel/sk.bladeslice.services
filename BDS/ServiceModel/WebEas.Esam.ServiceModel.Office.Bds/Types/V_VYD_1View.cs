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
        [PfeCombo(typeof(tblD_VYD_0), ComboIdColumn = "D_VYD_0", ComboDisplayColumn = "DKL_C")]
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
        [PfeCombo(typeof(V_SKL_1View), ComboIdColumn = "K_TSK_0", ComboDisplayColumn = "TSK")]
        public string TSK { get; set; }

        [DataMember]
        [PfeColumn(Text = "_3D simulácia")]
        public byte SKL_SIMULATION { get; set; }

        [DataMember]
        [PfeCombo(typeof(SimulationType), IdColumn = "SKL_SIMULATION")]
        [PfeColumn(Text = "3D simulation", ReadOnly = true, Editable = false)]
        [Ignore]
        public string SKL_SIMULATIONText
        {
            get
            {
                return SimulationType.GetText(SKL_SIMULATION);
            }
        }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Created by", Hidden = true, Editable = false, ReadOnly = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Edited by", Hidden = true, Editable = false, ReadOnly = true)]
        public string ZmenilMeno { get; set; }
    }
}
