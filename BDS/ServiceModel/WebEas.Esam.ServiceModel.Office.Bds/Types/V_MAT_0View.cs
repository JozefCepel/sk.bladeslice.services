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
        [PfeCombo(typeof(tblK_TSK_0), ComboIdColumn = "K_TSK_0", ComboDisplayColumn = "TSK")]
        public string TSK { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Created by", Hidden = true, Editable = false, ReadOnly = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Edited by", Hidden = true, Editable = false, ReadOnly = true)]
        public string ZmenilMeno { get; set; }
    }
}
