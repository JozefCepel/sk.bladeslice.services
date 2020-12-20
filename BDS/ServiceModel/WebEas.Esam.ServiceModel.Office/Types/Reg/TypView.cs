using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Types.Reg;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Reg
{
    [Schema("reg")]
    [Alias("V_Typ")]
    [DataContract]
    public class TypView : Typ, IBaseView
    {
        [DataMember]
        [PfeColumn(Text = "Účtovanie do rozpočtu")]
        [PfeCombo(typeof(RegRzpDefiniciaCombo), IdColumn = nameof(RzpDefinicia), ComboDisplayColumn = nameof(RegRzpDefiniciaCombo.Nazov))]
        [Ignore]
        public string RzpDefiniciaText { get { return RegRzpDefiniciaCombo.GetText(RzpDefinicia); } }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string ZmenilMeno { get; set; }
    }
}
