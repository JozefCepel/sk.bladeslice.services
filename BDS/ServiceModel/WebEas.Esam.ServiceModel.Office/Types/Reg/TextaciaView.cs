﻿using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Office.Egov.Reg.Types;

namespace WebEas.Esam.ServiceModel.Office.Types.Reg
{
    [DataContract]
    [Schema("reg")]
    [Alias("V_Textacia")]
    public class TextaciaView : Textacia
    {
        [DataMember]
        [PfeColumn(Text = "Typ dokladu")]
        [PfeCombo(typeof(TypBiznisEntity), ComboDisplayColumn = nameof(TypBiznisEntity.Nazov), IdColumn = nameof(C_TypBiznisEntity_Id),
                  AdditionalWhereSql = "C_TypBiznisEntity_Id IN (1, 2, 3, 4, 8, 13, 14, 15, 16, 17, 18)")]
        public string TypBiznisEntityNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kniha", RequiredFields = new[] { nameof(C_TypBiznisEntity_Id)} )]
        [PfeCombo(typeof(TypBiznisEntity_KnihaView), IdColumn = nameof(C_TypBiznisEntity_Kniha_Id),
                  ComboDisplayColumn = nameof(TypBiznisEntity_KnihaView.Kod),
                  AdditionalWhereSql = "C_TypBiznisEntity_Id = @C_TypBiznisEntity_Id",
                  Tpl = "{value};{Nazov}",
                  AdditionalFields = new[] { nameof(TypBiznisEntity_KnihaView.Nazov) }
            )]
        public string Kniha { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Druh dane")]   // bude sa riesit niekedy v buducnosti
        // [PfeCombo(typeof(DruhView), IdColumn = nameof(C_Druh_Id), AdditionalFields = new[] { nameof(DruhView.Druh) })]
        public string DruhDane { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string ZmenilMeno { get; set; }
    }
}
