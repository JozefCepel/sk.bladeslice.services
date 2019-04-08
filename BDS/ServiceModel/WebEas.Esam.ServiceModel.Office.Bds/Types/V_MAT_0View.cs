using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("dbo")]
    [Alias("V_MAT_0")]
    [DataContract]
    public class V_MAT_0View : tblK_MAT_0
    {
        [DataMember]
        [PfeColumn(Text = "Mat. group")]
        [PfeCombo(typeof(tblK_TSK_0), IdColumnCombo = "K_TSK_0", DisplayColumn = "TSK")]
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
