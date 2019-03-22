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
        [PfeColumn(Text = "Sklad")]
        public string SKL { get; set; }
        [DataMember]
        [PfeColumn(Text = "Rozsah")]
        public short? ROZSAH { get; set; }
        [DataMember]
        [PfeColumn(Text = "Skladové obdobie")]
        public byte? SO { get; set; }
        [DataMember]
        [PfeColumn(Text = "Kód")]
        public string KOD { get; set; }
        [DataMember]
        [PfeColumn(Text = "Skupina skladov")]
        public string SKL_GRP { get; set; }
        [DataMember]
        [PfeColumn(Text = "Povoliť stav do mínusu")]
        public bool SKL_MINUS { get; set; }
        [DataMember]
        [PfeColumn(Text = "Zobraz pri výdaji aj nulové položky")]
        public bool? SHOW_VYD_ZERO_SKL { get; set; }
        [DataMember]
        [PfeColumn(Text = "Zahrnúť do min/max")]
        public bool CHECK_IN_SKL_MIN_MAX { get; set; }
        [DataMember]
        [PfeColumn(Text = "Zahrnúť do rezervácií")]
        public bool CHECK_IN_REZ { get; set; }
        [DataMember]
        [PfeColumn(Text = "_Poznámka")]
        public string POZN { get; set; }
    }
}
