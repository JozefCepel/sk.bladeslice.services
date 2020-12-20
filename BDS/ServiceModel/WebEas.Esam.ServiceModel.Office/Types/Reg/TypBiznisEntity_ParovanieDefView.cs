using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Office.Egov.Reg.Types;

namespace WebEas.Esam.ServiceModel.Office.Types.Reg
{
    [Schema("reg")]
    [Alias("V_TypBiznisEntity_ParovanieDef")]
    [DataContract]
    public class TypBiznisEntity_ParovanieDefView : TypBiznisEntity_ParovanieDef, IBaseView
    {
        [DataMember]
        [PfeColumn(Text = "Typ dokladu 1")]
        [PfeCombo(typeof(TypBiznisEntityView), ComboDisplayColumn = nameof(TypBiznisEntityView.KodNazov), IdColumn = nameof(C_TypBiznisEntity_Id_1), AdditionalWhereSql = "KodORS = 'ORJ' AND Kod <> 'IND'")]
        public string TypBeKodNazov1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Typ dokladu 2")]
        [PfeCombo(typeof(TypBiznisEntityView), ComboDisplayColumn = nameof(TypBiznisEntityView.KodNazov), IdColumn = nameof(C_TypBiznisEntity_Id_2), AdditionalWhereSql = "KodORS = 'ORJ' AND Kod <> 'IND'")]
        public string TypBeKodNazov2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Typ väzby", Mandatory = true)]
        [PfeCombo(typeof(ParovanieTypCombo), IdColumn = nameof(ParovanieTyp), ComboDisplayColumn = nameof(ParovanieTypCombo.Nazov), ComboIdColumn = nameof(ParovanieTypCombo.Id))]
        public string ParovanieTypNazov { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string ZmenilMeno { get; set; }

    }
}
