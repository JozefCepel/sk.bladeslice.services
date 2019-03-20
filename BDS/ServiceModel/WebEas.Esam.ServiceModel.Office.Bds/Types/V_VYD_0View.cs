using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("dbo")]
    [Alias("V_VYD_0")]
    [DataContract]
    public class V_VYD_0View : tblD_VYD_0
    {
        [DataMember]
        [PfeColumn(Text = "_ORJ")]
        public string ORJ { get; set; }

        [DataMember]
        [PfeColumn(Text = "_SKL")]
        public string SKL { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true)]
        public string ZmenilMeno { get; set; }
    }
}
