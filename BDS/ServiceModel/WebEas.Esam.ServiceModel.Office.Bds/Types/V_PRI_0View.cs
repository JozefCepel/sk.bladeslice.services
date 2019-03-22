using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("dbo")]
    [Alias("V_PRI_0")]
    [DataContract]
    public class V_PRI_0View : tblD_PRI_0
    {
        [DataMember]
        [PfeColumn(Text = "Organizačná jednotka")]
        public string ORJ { get; set; }

        [DataMember]
        [PfeColumn(Text = "Sklad")]
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
