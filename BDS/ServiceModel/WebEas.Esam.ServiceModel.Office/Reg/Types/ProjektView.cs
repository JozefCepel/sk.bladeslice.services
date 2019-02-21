using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Reg.Types
{
    [Schema("reg")]
    [Alias("V_Projekt")]
    [DataContract]
    public class ProjektView : Projekt
    {
        [DataMember]
        [PfeColumn(Text = "Druh", ReadOnly = true)]
        public string TypBdsName { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kód zdroja", ReadOnly = true)]
        public string ZdrojKod { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov zdroja", ReadOnly = true)]
        public string ZdrojNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Projekt", Hidden = true, Hideable = false)]
        [PfeValueColumn]
        public string KodNazov { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true)]
        public string ZmenilMeno { get; set; }
    }
}
