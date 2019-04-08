﻿using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("dbo")]
    [Alias("V_VYD_0")]
    [DataContract]
    public class V_VYD_0View : tblD_VYD_0
    {
        [DataMember]
        [PfeColumn(Text = "Organisation unit")]
        [PfeCombo(typeof(tblK_ORJ_0), IdColumnCombo = "K_ORJ_0", DisplayColumn = "ORJ")]
        public string ORJ { get; set; }

        [DataMember]
        [PfeColumn(Text = "Warehouse", RequiredFields = new[] { "K_ORJ_0" })]
        [PfeCombo(typeof(V_ORJ_1View), IdColumnCombo = "K_SKL_0", DisplayColumn = "SKL")]
        public string SKL { get; set; }

        [DataMember]
        [PfeColumn(Text = "Customer")]
        [PfeCombo(typeof(tblK_OBP_0), IdColumnCombo = "K_OBP_0", DisplayColumn = "NAZOV1", AdditionalWhereSql = "K_TOB_0 IN  (1, 3)")] //Odberateľ
        public string NAZOV1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Name 2", ReadOnly = true, Editable = false)]
        public string NAZOV2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "IČO", ReadOnly = true, Editable = false)]
        //[PfeCombo(typeof(tblK_OBP_0), IdColumnCombo = "K_OBP_0", DisplayColumn = "ICO")]
        public string ICO { get; set; }

        [DataMember]
        [PfeColumn(Text = "Ulica", ReadOnly = true, Editable = false)]
        public string ULICA_S { get; set; }

        [DataMember]
        [PfeColumn(Text = "PSČ", ReadOnly = true, Editable = false)]
        public string PSC_S { get; set; }

        [DataMember]
        [PfeColumn(Text = "Obec", ReadOnly = true, Editable = false)]
        public string OBEC_S { get; set; }

        [DataMember]
        [PfeColumn(Text = "štát", ReadOnly = true, Editable = false)]
        public string STAT { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true)]
        public string ZmenilMeno { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_K_OBP_0_ICO")]
        //[Ignore]
        //public int? K_OBP_0_ICO
        //{
        //    get
        //    {
        //        return K_OBP_0;
        //    }
        //}

        //[DataMember]
        //[PfeColumn(Text = "_K_OBP_0_NAZOV1")]
        //[Ignore]
        //public int? K_OBP_0_NAZOV1
        //{
        //    get
        //    {
        //        return K_OBP_0;
        //    }
        //}
    }
}
