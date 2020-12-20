using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Uct
{
    [DataContract]
    [Schema("uct")]
    [Alias("V_UctKluc")]
    public class UctKlucView : UctKluc, IBaseView
    {
        [DataMember]
        [PfeCombo(typeof(UctKlucCombo), IdColumn = nameof(UctovnyKluc), ComboDisplayColumn = nameof(UctKlucCombo.Nazov))]
        [PfeColumn(Text = "Účtovný kľúč")]
        public string UctovnyKlucText { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string ZmenilMeno { get; set; }
    }
}
