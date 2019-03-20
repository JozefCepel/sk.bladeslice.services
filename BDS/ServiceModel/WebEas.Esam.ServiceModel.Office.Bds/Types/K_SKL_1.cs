using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("dbo")]
    [Alias("K_SKL_1")]
    [DataContract]
    public class tblK_SKL_1 : BaseEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        [PfeColumn(Text = "_K_SKL_1")]
        public int K_SKL_1 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_K_SKL_0")]
        public int K_SKL_0 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_K_TSK_0")]
        public int K_TSK_0 { get; set; }
    }
}
