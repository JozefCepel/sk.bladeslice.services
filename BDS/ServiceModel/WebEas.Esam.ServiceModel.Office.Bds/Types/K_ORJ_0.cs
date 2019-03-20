using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("dbo")]
    [Alias("K_ORJ_0")]
    [DataContract]
    public class tblK_ORJ_0 : BaseEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        [PfeColumn(Text = "_K_ORJ_0")]
        public int K_ORJ_0 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_ORJ")]
        public string ORJ { get; set; }
        [DataMember]
        [PfeColumn(Text = "_POZN")]
        public string POZN { get; set; }
        [DataMember]
        [PfeColumn(Text = "_KOD")]
        public string KOD { get; set; }
    }
}
