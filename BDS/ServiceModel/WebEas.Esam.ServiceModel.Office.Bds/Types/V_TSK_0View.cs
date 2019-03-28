using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("dbo")]
    [Alias("V_TSK_0")]
    [DataContract]
    public class V_TSK_0View : tblK_TSK_0
    {
        [DataMember]
        [PfeCombo(typeof(SimulationType), NameColumn = "SKL_SIMULATION")]
        [PfeColumn(Text = "3D simulation")]
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
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true)]
        public string ZmenilMeno { get; set; }
    }
}
