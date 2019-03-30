using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("dbo")]
    [Alias("K_SKL_2")]
    [DataContract]
    public class tblK_SKL_2 : BaseEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public int K_SKL_2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_K_SKL_0", Mandatory = true)]
        public int K_SKL_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Position", Mandatory = true)]
        public string LOCATION { get; set; }

        [DataMember]
        [PfeColumn(Text = "Description")]
        public string POPIS { get; set; }
    }
}
