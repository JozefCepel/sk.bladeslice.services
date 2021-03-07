using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("bds")]
    [Alias("V_OBP_0")]
    [DataContract]
    public class V_OBP_0View : tblK_OBP_0
    {
        [DataMember]
        [PfeColumn(Text = "Business partner type")]
        [PfeCombo(typeof(tblK_TOB_0), ComboIdColumn = "K_TOB_0", ComboDisplayColumn = "TOB")]
        public string TOB { get; set; }

        [DataMember]
        [PfeColumn(Text = "VAT payer", ReadOnly = true)]
        public bool IS_PLATCA { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Created by", Hidden = true, Editable = false, ReadOnly = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Edited by", Hidden = true, Editable = false, ReadOnly = true)]
        public string ZmenilMeno { get; set; }
    }
}
