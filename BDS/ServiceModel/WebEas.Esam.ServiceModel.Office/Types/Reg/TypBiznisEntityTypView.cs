using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Office.Egov.Reg.Types;

namespace WebEas.Esam.ServiceModel.Office.Types.Reg
{
    [Schema("reg")]
    [Alias("V_TypBiznisEntity_Typ")]
    [DataContract]
    public class TypBiznisEntityTypView : TypBiznisEntityTyp, IBaseView
    {
        [DataMember]
        [PfeColumn(Text = "Typ")]
        [PfeCombo(typeof(Typ), ComboDisplayColumn = nameof(Typ.Nazov), IdColumn = nameof(C_Typ_Id))]
        public string TypNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Položka")]
        public bool Polozka { get; set; }

        [DataMember]
        [PfeColumn(Text = "Typ dokladu")]
        [PfeCombo(typeof(TypBiznisEntity), ComboDisplayColumn = nameof(TypBiznisEntity.Nazov), IdColumn = nameof(C_TypBiznisEntity_Id))]
        public string TypBiznisEntityNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kniha", RequiredFields = new[] { nameof(C_TypBiznisEntity_Id) })]
        [PfeCombo(typeof(TypBiznisEntity_Kniha), ComboDisplayColumn = nameof(TypBiznisEntity_Kniha.Kod), IdColumn = nameof(C_TypBiznisEntity_Kniha_Id))]
        public string KnihaKod { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string ZmenilMeno { get; set; }
    }
}
