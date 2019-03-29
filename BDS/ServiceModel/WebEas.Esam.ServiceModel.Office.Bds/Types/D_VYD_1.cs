using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("dbo")]
    [Alias("D_VYD_1")]
    [DataContract]
    public class tblD_VYD_1 : BaseEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public int D_VYD_1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_VYD_0")]
        public int D_VYD_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_K_TSK_0")]
        public int? K_TSK_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_K_TYP_0")]
        public int K_TYP_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kód")]
        public string KOD { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov")]
        public string NAZOV { get; set; }

        [DataMember]
        [PfeColumn(Text = "Počet MJ", DefaultValue = 0)]
        public decimal? POC_KS { get; set; }

        [DataMember]
        [PfeColumn(Text = "MJ")]
        public string MJ { get; set; }

        [DataMember]
        [PfeColumn(Text = "Cena", DefaultValue = 0)]
        public decimal? D_CENA { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Z_CENA", DefaultValue = 0)]
        public decimal? Z_CENA { get; set; }

        [DataMember]
        [PfeColumn(Text = "R", DefaultValue = 1)]
        public int? RANK { get; set; }

        [DataMember]
        [PfeColumn(Text = "_BAL_KS", DefaultValue = 0)]
        public decimal? BAL_KS { get; set; }

        [DataMember]
        [PfeColumn(Text = "_BAL_KS1", DefaultValue = 1)]
        public decimal? BAL_KS1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "EAN kód")]
        public string EAN { get; set; }

        [DataMember]
        [PfeColumn(Text = "_WARRANTY", DefaultValue = 0)]
        public int WARRANTY { get; set; }

        [DataMember]
        [PfeColumn(Text = "SN")]
        public string SN { get; set; }

        [DataMember]
        [PfeColumn(Text = "Position")]
        public string LOCATION { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Position dest")]
        public string LOCATION_DEST { get; set; }

        [DataMember]
        [PfeColumn(Text = "_KOD_EXT")]
        public string KOD_EXT { get; set; }

        [DataMember]
        [PfeColumn(Text = "_NAZOV_EXT")]
        public string NAZOV_EXT { get; set; }

        [DataMember]
        [PfeColumn(Text = "Šarža")]
        public string SARZA { get; set; }

        [DataMember]
        [PfeColumn(Text = "a", DefaultValue = 0)]
        public int D3D_A { get; set; }

        [DataMember]
        [PfeColumn(Text = "b", DefaultValue = 0)]
        public int D3D_B { get; set; }

        [DataMember]
        [PfeColumn(Text = "L", DefaultValue = 0)]
        public int D3D_L { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D", DefaultValue = 0)]
        public int D3D_D1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "d", DefaultValue = 0)]
        public int D3D_D2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Počet kusov 3D", DefaultValue = 0)]
        public int D3D_POC_KS { get; set; }
    }
}
