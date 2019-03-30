using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("dbo")]
    [Alias("D_SIM_0")]
    [DataContract]
    public class tblD_SIM_0 : BaseEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public int D_SIM_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_PRI_0")]
        public int? D_PRI_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_PRI_1")]
        public int? D_PRI_1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_VYD_0")]
        public int? D_VYD_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_VYD_1")]
        public int? D_VYD_1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "R", DefaultValue = 0, Mandatory = true)]
        public int? RANK { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Typ")]
        public byte PV { get; set; }

        [DataMember]
        [PfeColumn(Text = "SN")]
        public string SN { get; set; }

        [DataMember]
        [PfeColumn(Text = "a 1 [mm]", DefaultValue = 0)]
        public int A1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "a 2 [mm]", DefaultValue = 0)]
        public int A2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "b 1 [mm]", DefaultValue = 0)]
        public int B1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "b 2 [mm]", DefaultValue = 0)]
        public int B2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "D [mm]", DefaultValue = 0)]
        public int D1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "d [mm]", DefaultValue = 0)]
        public int D2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "L 1 [mm]", DefaultValue = 0)]
        public int L1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "L 2 [mm]", DefaultValue = 0)]
        public int L2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Počet kusov", DefaultValue = 0)]
        public int POC_KS { get; set; }

        [DataMember]
        [PfeColumn(Text = "Výrez [dm3]", DefaultValue = 0)]
        public decimal? POC_KS_VYREZ { get; set; }

        [DataMember]
        [PfeColumn(Text = "Odrezok [dm3]", DefaultValue = 0)]
        public decimal? POC_KS_ZVYSOK { get; set; }

        [DataMember]
        [PfeColumn(Text = "Polotovar [dm3]", DefaultValue = 0)]
        public decimal POC_KS_PLT { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Zvyšok do spotreby", DefaultValue = false)]
        public bool ZVYSOK_SPOTREBA { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Poznámka")]
        public string POZN { get; set; }

        [DataMember]
        [PfeColumn(Text = "Šarža")]
        public string SARZA { get; set; }

        [DataMember]
        [PfeColumn(Text = "Position")]
        public string LOCATION { get; set; }

        [DataMember]
        [PfeColumn(Text = "_SKL_CENA", DefaultValue = 0)]
        public decimal SKL_CENA { get; set; }

        [DataMember]
        [PfeColumn(Text = "Vstupné rozmery")]
        public string OUTER_SIZE { get; set; }

        [DataMember]
        [PfeColumn(Text = "Výstupné rozmery")]
        public string OUTER_SIZE_FINAL { get; set; }
    }
}
