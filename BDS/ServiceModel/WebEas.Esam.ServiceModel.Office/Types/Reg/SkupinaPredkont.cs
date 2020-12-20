using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Reg
{
    [DataContract]
    [Schema("reg")]
    [Alias("V_SkupinaPredkont")]
    public class SkupinaPredkont //: IPfeCustomizeCombo
    {
        [DataMember]
        [PrimaryKey]
        public int? SkupinaPredkont_Id { get; set; }

        [DataMember]
        public string SkupinaPredkont_Popis { get; set; }

        //public void ComboCustomize(IWebEasRepositoryBase repository, string column, string kodPolozky, ref PfeComboAttribute comboAttribute)
        //{
        //    if (column.ToLower() == "SkupinaPredkont_Popis".ToLower() && kodPolozky == "uct-def-kont-konf-uct")
        //    {
        //        comboAttribute.AdditionalWhereSql = $"SkupinaPredkont_Id IN ({ (int)SkupinaPredkontEnum.Bankove_vypisy }, {(int)SkupinaPredkontEnum.Prijmove_PD }, {(int)SkupinaPredkontEnum.Vydajove_PD }, {(int)SkupinaPredkontEnum.ExtDoklady_DaP })";
        //    }
        //}
    }
}
