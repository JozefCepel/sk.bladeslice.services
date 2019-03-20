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
        [PfeColumn(Text = "_PriPol")]
        public string PriPol { get; set; }

        [DataMember]
        [PfeColumn(Text = "_VydPol")]
        public string VydPol { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true)]
        public string ZmenilMeno { get; set; }
    }
}
