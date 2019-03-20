using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Rzp.Types
{
    [Schema("rzp")]
    [Alias("V_RzpPol")]
    [DataContract]
    public class RzpPolView : RzpPol
    {
        [DataMember]
        [PfeColumn(Text = "_Kód")]
        public string FKDot { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Názov")]
        public string FKNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Rozpočtová položka", ReadOnly = true)]
        [PfeValueColumn]
        public string RzpUcet { get; set; }

        [DataMember]
        [PfeColumn(Text = "Rozpočtová položka - názov", Hidden = true, Hideable = false)]
        public string RzpUcetNazov { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true)]
        public string ZmenilMeno { get; set; }
    }
}