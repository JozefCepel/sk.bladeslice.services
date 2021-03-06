﻿using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [DataContract]
    [Schema("cfe")]
    [Alias("V_Right")]
    public class RightView : Right, IBaseView
    {
        [DataMember]
        [PfeColumn(Text = "Modul")]
        [PfeCombo(typeof(ModulView), IdColumn = nameof(C_Modul_Id), ComboDisplayColumn = nameof(ModulView.Nazov))]
        public string ModulNazov { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Created by", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Edited by", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string ZmenilMeno { get; set; }

    }
}

