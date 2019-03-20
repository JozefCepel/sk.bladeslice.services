using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("dbo")]
    [Alias("K_SKL_0")]
    [DataContract]
    public class tblK_SKL_0 : BaseEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        [PfeColumn(Text = "_K_SKL_0")]
        public int K_SKL_0 { get; set; }
        [DataMember]
        [PfeColumn(Text = "_SKL")]
        public string SKL { get; set; }
        [DataMember]
        [PfeColumn(Text = "_POZN")]
        public string POZN { get; set; }
        [DataMember]
        [PfeColumn(Text = "_ROZSAH")]
        public short? ROZSAH { get; set; }
        [DataMember]
        [PfeColumn(Text = "_SO")]
        public byte? SO { get; set; }
        [DataMember]
        [PfeColumn(Text = "_KOD")]
        public string KOD { get; set; }
        [DataMember]
        [PfeColumn(Text = "_SKL_GRP")]
        public string SKL_GRP { get; set; }
        [DataMember]
        [PfeColumn(Text = "_SKL_MINUS")]
        public bool SKL_MINUS { get; set; }
        [DataMember]
        [PfeColumn(Text = "_SHOW_VYD_ZERO_SKL")]
        public bool? SHOW_VYD_ZERO_SKL { get; set; }
        [DataMember]
        [PfeColumn(Text = "_CHECK_IN_SKL_MIN_MAX")]
        public bool CHECK_IN_SKL_MIN_MAX { get; set; }
        [DataMember]
        [PfeColumn(Text = "_CHECK_IN_REZ")]
        public bool CHECK_IN_REZ { get; set; }
    }
}
