﻿using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("dbo")]
    [Alias("D_PRI_1")]
    [DataContract]
    public class tblD_PRI_1 : BaseEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public int D_PRI_1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_PRI_0")]
        public int? D_PRI_0 { get; set; }

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
        [PfeColumn(Text = "EAN kód")]
        public string EAN { get; set; }

        [DataMember]
        [PfeColumn(Text = "_WARRANTY", DefaultValue = 0)]
        public int? WARRANTY { get; set; }

        [DataMember]
        [PfeColumn(Text = "SN")]
        public string SN { get; set; }

        [DataMember]
        [PfeColumn(Text = "Position")]
        public string LOCATION { get; set; }

        [DataMember]
        [PfeColumn(Text = "_KOD_EXT")]
        public string KOD_EXT { get; set; }

        [DataMember]
        [PfeColumn(Text = "_NAZOV_EXT")]
        public string NAZOV_EXT { get; set; }

        [DataMember]
        [PfeColumn(Text = "Šarža")]
        public string SARZA { get; set; }
    }
}