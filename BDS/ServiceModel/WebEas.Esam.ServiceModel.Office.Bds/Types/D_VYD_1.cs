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
        [PfeColumn(Text = "_K_TSK_0", Mandatory = true)]
        public int K_TSK_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_K_TYP_0")]
        public int K_TYP_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Code", Mandatory = true, Xtype = PfeXType.SearchFieldSS)] //, RequiredFields = new[] { "K_TSK_0" }
        [PfeCombo(typeof(V_MAT_0View), ComboIdColumn = nameof(V_MAT_0View.KOD), ComboDisplayColumn = nameof(V_MAT_0View.KOD), IdColumn = nameof(KOD),
            AdditionalFields = new[] { nameof(V_MAT_0View.NAZOV), nameof(V_MAT_0View.TSK), nameof(V_MAT_0View.K_TSK_0), nameof(V_MAT_0View.EAN), nameof(V_MAT_0View.MJ), nameof(V_MAT_0View.N_CENA) },
            Tpl = "{value};{TSK}")]
        public string KOD { get; set; }

        [DataMember]
        [PfeColumn(Text = "Name")]
        public string NAZOV { get; set; }

        [DataMember]
        [PfeColumn(Text = "Amount", DefaultValue = 0, Mandatory = true, DecimalPlaces = 4)]
        public decimal POC_KS { get; set; }

        [DataMember]
        [PfeColumn(Text = "UoM", Tooltip = "Unit of Measure")]
        public string MJ { get; set; }

        [DataMember]
        [PfeColumn(Text = "Price", DefaultValue = 0, Mandatory = true)]
        public decimal D_CENA { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Z_CENA", DefaultValue = 0)]
        public decimal? Z_CENA { get; set; }

        [DataMember]
        [PfeColumn(Text = "R", DefaultValue = 1, Mandatory = true)]
        public int? RANK { get; set; }

        [DataMember]
        [PfeColumn(Text = "_BAL_KS", DefaultValue = 0)]
        public decimal BAL_KS { get; set; }

        [DataMember]
        [PfeColumn(Text = "_BAL_KS1", DefaultValue = 1)]
        public decimal? BAL_KS1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "EAN code")]
        public string EAN { get; set; }

        [DataMember]
        [PfeColumn(Text = "_WARRANTY", DefaultValue = 0)]
        public int WARRANTY { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "Serial No.", RequiredFields = new[] { nameof(K_TSK_0), nameof(KOD) })]
        //[PfeCombo(typeof(STS_FIFOFull), ComboIdColumn = nameof(STS_FIFOFull.SN), ComboDisplayColumn = nameof(STS_FIFOFull.SN), IdColumn = nameof(SN),
        //    AdditionalFields = new[] { nameof(STS_FIFOFull.SARZA), nameof(STS_FIFOFull.POC_KS), nameof(STS_FIFOFull.LOCATION), nameof(STS_FIFOFull.SKL_CENA) },
        //    Tpl = "{value};{SARZA};{LOCATION};{POC_KS};{SKL_CENA}")]
        //public string SN { get; set; }

        [DataMember]
        [PfeColumn(Text = "Serial No.")]
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
        [PfeColumn(Text = "Batch")]
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
        [PfeColumn(Text = "D", DefaultValue = 0)]
        public int D3D_D1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "d", DefaultValue = 0)]
        public int D3D_D2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Number of pieces", DefaultValue = 0)]
        public int D3D_POC_KS { get; set; }
    }
}
