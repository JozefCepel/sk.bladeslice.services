﻿using System;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [Schema("cfe")]
    [Alias("V_Tenant")]
    [DataContract]
    public class TenantView : Tenant
    {
        [DataMember]
        [PfeColumn(Text = "Typ")]
        [PfeCombo(typeof(TenantType), NameColumn = "C_TenantType_Id", DisplayColumn = "Typ")]
        [IgnoreInsertOrUpdate]
        public string TenantTypeName { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true)]
        [IgnoreInsertOrUpdate]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true)]
        [IgnoreInsertOrUpdate]
        public string ZmenilMeno { get; set; }
    }
}
