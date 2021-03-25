using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Reg
{
    [Schema("reg")]
    [Alias("V_DphSadzba")]
    [DataContract]
    public class DphSadzbaView : DphSadzba, IBaseView
    {
        [DataMember]
        [PfeColumn(Text = "Typ sadzby")]
        [PfeCombo(typeof(TypSazbyCombo), IdColumn = nameof(TypId))]
        [Ignore]
        public string TypSadzby => TypSazbyCombo.GetText(TypId);

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string ZmenilMeno { get; set; }
    }
}
