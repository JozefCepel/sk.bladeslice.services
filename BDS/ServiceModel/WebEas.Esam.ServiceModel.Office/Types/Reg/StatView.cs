using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Reg
{
    [Schema("reg")]
    [Alias("V_Stat")]
    [DataContract]
    public class StatView : StatCis, IBaseView
    {
        [DataMember]
        [PfeColumn(Text = "Mena", ReadOnly = true)]
        public string MenaKod { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string ZmenilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "_NazovSkratka", ReadOnly = true)]
        public string NazovSkratka { get; set; }
    }
}
