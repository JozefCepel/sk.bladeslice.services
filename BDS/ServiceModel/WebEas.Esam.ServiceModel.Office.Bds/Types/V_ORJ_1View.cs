using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("bds")]
    [Alias("V_ORJ_1")]
    [DataContract]
    public class V_ORJ_1View : tblK_ORJ_1
    {
        [DataMember]
        [PfeColumn(Text = "Warehouse")]
        [PfeCombo(typeof(tblK_SKL_0), ComboIdColumn = "K_SKL_0", ComboDisplayColumn = "SKL")]
        public string SKL { get; set; }

        [DataMember]
        [PfeColumn(Text = "Organisation unit")]
        [PfeCombo(typeof(tblK_ORJ_0), ComboIdColumn = "K_ORJ_0", ComboDisplayColumn = "ORJ")]
        public string ORJ { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Created by", Hidden = true, Editable = false, ReadOnly = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Edited by", Hidden = true, Editable = false, ReadOnly = true)]
        public string ZmenilMeno { get; set; }
    }
}
