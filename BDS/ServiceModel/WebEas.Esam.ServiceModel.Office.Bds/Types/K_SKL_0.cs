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
        public int K_SKL_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Code", Mandatory = true)]
        public string KOD { get; set; }

        [DataMember]
        [PfeColumn(Text = "Name", Mandatory = true)]
        public string SKL { get; set; }

        [DataMember]
        [PfeColumn(Text = "Rank", Mandatory = true)]
        [PfeSort(Rank = 1, Sort = PfeOrder.Asc)]
        public int Serial_No { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Rozsah", DefaultValue = 1)]
        public short? ROZSAH { get; set; }

        [DataMember]
        [PfeColumn(Text = "Warehouse period", DefaultValue = 1)]
        public byte SO { get; set; }

        [DataMember]
        [PfeColumn(Text = "Warehouse group")]
        public string SKL_GRP { get; set; }

        [DataMember]
        [PfeColumn(Text = "Allow amount to minus", DefaultValue = false)]
        public bool SKL_MINUS { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Zobraz pri výdaji aj nulové položky", DefaultValue = false)]
        public bool? SHOW_VYD_ZERO_SKL { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Zahrnúť do min/max", DefaultValue = true)]
        public bool CHECK_IN_SKL_MIN_MAX { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Zahrnúť do rezervácií", DefaultValue = true)]
        public bool CHECK_IN_REZ { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Poznámka")]
        public string POZN { get; set; }
    }
}
