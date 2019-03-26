﻿using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("dbo")]
    [Alias("V_SKL_1")]
    [DataContract]
    public class V_SKL_1View : tblK_SKL_1
    {
        [DataMember]
        [PfeColumn(Text = "Warehouse")]
        [PfeCombo(typeof(tblK_SKL_0), IdColumnCombo = "K_SKL_0", DisplayColumn = "SKL")]
        public string SKL { get; set; }

        [DataMember]
        [PfeColumn(Text = "Skupina")]
        [PfeCombo(typeof(tblK_TSK_0), IdColumnCombo = "K_TSK_0", DisplayColumn = "TSK")]
        public string TSK { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true)]
        public string ZmenilMeno { get; set; }
    }
}