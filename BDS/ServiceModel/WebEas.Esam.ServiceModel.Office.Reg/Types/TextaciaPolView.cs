using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Reg.Types
{
    [DataContract]
    [Schema("reg")]
    [Alias("V_TextaciaPol")]
    public class TextaciaPolView : TextaciaPol
    {

        [DataMember]
        [PfeColumn(Text = "Textácia")]
        [PfeCombo(typeof(TextaciaView), IdColumn = nameof(C_Textacia_Id), ComboDisplayColumn = nameof(TextaciaNazov), ComboIdColumn = nameof(C_Textacia_Id))]
        public string TextaciaNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Typ dátumu")]
        [PfeCombo(typeof(DatumTypCombo), IdColumn = nameof(DatumTyp))]
        [Ignore]
        public string DatumTypText => DatumTypCombo.GetText(DatumTyp);

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string ZmenilMeno { get; set; }

    }
}
