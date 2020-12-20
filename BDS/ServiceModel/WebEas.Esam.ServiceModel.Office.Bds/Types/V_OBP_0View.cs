using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("dbo")]
    [Alias("V_OBP_0_BDS")]
    [DataContract]
    public class V_OBP_0View : tblK_OBP_0
    {
        [DataMember]
        [PfeColumn(Text = "Typ OBP")]
        [PfeCombo(typeof(tblK_TOB_0), ComboIdColumn = "K_TOB_0", ComboDisplayColumn = "TOB")]
        public string TOB { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platca DPH", ReadOnly = true)]
        public bool IS_PLATCA { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true)]
        public string ZmenilMeno { get; set; }
    }
}
