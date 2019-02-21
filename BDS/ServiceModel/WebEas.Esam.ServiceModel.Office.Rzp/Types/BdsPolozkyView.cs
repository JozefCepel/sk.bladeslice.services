using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("rzp")]
    [Alias("V_BdsPolozky")]
    [DataContract]
    public class BdsPolozkyView : BdsPolozky
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
        public string BdsUcet { get; set; }

        [DataMember]
        [PfeColumn(Text = "Rozpočtová položka - názov", Hidden = true, Hideable = false)]
        public string BdsUcetNazov { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true)]
        public string ZmenilMeno { get; set; }
    }
}