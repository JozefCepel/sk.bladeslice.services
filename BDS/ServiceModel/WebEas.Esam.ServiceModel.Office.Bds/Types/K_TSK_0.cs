using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("dbo")]
    [Alias("K_TSK_0")]
    [DataContract]
    public class tblK_TSK_0 : BaseEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        [PfeColumn(Text = "_K_TSK_0")]
        public int K_TSK_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Group name", Mandatory = true)]
        public string TSK { get; set; }

        [DataMember]
        [PfeColumn(Text = "_3D simulácia")]
        public byte SKL_SIMULATION { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Poznámka")]
        public string POZN { get; set; }

        [DataMember]
        [PfeColumn(Text = "_TOVAR", DefaultValue = false)]
        public bool? TOVAR { get; set; }

        [DataMember]
        [PfeColumn(Text = "_MATERIAL", DefaultValue = true)]
        public bool? MATERIAL { get; set; }

        [DataMember]
        [PfeColumn(Text = "_OSIVO", DefaultValue = false)]
        public bool? OSIVO { get; set; }

        [DataMember]
        [PfeColumn(Text = "_BALENIE", DefaultValue = false)]
        public bool? BALENIE { get; set; }

        [DataMember]
        [PfeColumn(Text = "_VYROBOK", DefaultValue = false)]
        public bool VYROBOK { get; set; }

        [DataMember]
        [PfeColumn(Text = "_PREP_K", DefaultValue = 0)]
        public decimal? PREP_K { get; set; }

        [DataMember]
        [PfeColumn(Text = "_PREP_M", DefaultValue = 0)]
        public decimal? PREP_M { get; set; }

        [DataMember]
        [PfeColumn(Text = "_PREP_M1", DefaultValue = 0)]
        public byte? PREP_M1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_OM1", DefaultValue = 0)]
        public decimal? OM1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_OM2", DefaultValue = 0)]
        public decimal? OM2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_OM3", DefaultValue = 0)]
        public decimal? OM3 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_OM4", DefaultValue = 0)]
        public decimal? OM4 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_OM5", DefaultValue = 0)]
        public decimal? OM5 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_OM6", DefaultValue = 0)]
        public decimal OM6 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_OM7", DefaultValue = 0)]
        public decimal OM7 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_OM8", DefaultValue = 0)]
        public decimal OM8 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_OM9", DefaultValue = 0)]
        public decimal OM9 { get; set; }
    }
}