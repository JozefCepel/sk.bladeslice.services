using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("dbo")]
    [Alias("V_PRI_0")]
    [DataContract]
    public class tblV_PRI_0View : tblD_PRI_0
    {
        [DataMember]
        public string ICO { get; set; }

        [DataMember]
        public string NAZOV1 { get; set; }

        [DataMember]
        public string NAZOV2 { get; set; }

        [DataMember]
        public string ULICA_S { get; set; }

        [DataMember]
        public string PSC_S { get; set; }

        [DataMember]
        public string OBEC_S { get; set; }

        [DataMember]
        public string ORJ { get; set; }

        [DataMember]
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
