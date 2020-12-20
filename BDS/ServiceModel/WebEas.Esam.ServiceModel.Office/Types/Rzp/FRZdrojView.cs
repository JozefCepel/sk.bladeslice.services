using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Rzp
{
    [Schema("rzp")]
    [Alias("V_FRZdroj")]
    [PfeCaption("Rozpočet - Kódy zdrojov")]
    [Cached]
    [DataContract]
    public class FRZdrojView : FRZdrojCis, IBaseView
    {
        [DataMember]
        [PfeColumn(Text = "_ZdrojFull")]
        public string ZdrojFull { get; set; }

        [DataMember]
        [PfeColumn(Text = "Typ organizácie")]
        public string OrganizaciaTyp { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string ZmenilMeno { get; set; }
    }
}
