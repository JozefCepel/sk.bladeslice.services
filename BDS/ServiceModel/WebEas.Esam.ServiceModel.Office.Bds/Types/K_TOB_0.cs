using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("dbo")]
    [Alias("K_TOB_0")]
    [DataContract]
    public class tblK_TOB_0 : BaseEntity
    {
        [AutoIncrement]
        [PrimaryKey]
        [DataMember]
        [PfeColumn(Text = "_K_TOB_0")]
        public int K_TOB_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Typ OBP")]
        public string TOB { get; set; }
    }
}