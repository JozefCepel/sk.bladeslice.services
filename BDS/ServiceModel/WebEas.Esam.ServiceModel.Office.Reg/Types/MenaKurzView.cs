using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Types.Reg;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Reg.Types
{
    [Schema("reg")]
    [Alias("V_MenaKurz")]
    [DataContract]
    public class MenaKurzView : MenaKurz, IBaseView
    {
        [DataMember]
        [PfeColumn(Text = "Mena", Mandatory = true)]
        [PfeCombo(typeof(MenaView), ComboIdColumn = nameof(C_Mena_Id), ComboDisplayColumn = nameof(MenaView.Kod), AdditionalWhereSql = "Poradie > 0")]
        public string MenaKod { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string ZmenilMeno { get; set; }
    }
}
