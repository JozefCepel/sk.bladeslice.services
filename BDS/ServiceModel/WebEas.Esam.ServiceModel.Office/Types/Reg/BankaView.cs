using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Reg
{
    [Schema("reg")]
    [Alias("V_Banka")]
    [DataContract]
    public class BankaView : Banka, IBaseView
    {
        [DataMember]
        [PfeColumn(Text = "Štát")]
        [PfeCombo(typeof(StatView), ComboDisplayColumn = nameof(StatView.NazovSkratka), IdColumn = nameof(C_Stat_Id))]
        public string StatNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "_KodNazov")]
        public string KodNazov { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string ZmenilMeno { get; set; }
    }
}
