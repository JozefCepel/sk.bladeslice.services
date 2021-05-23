using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Reg.Types
{
    [Schema("reg")]
    [Alias("V_TypPredkontCombo")]
    [DataContract]
    public class TypPredkontComboView: IPfeCustomizeCombo
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        public int C_Typ_Id { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        [DataMember]
        public int RzpDefinicia { get; set; }

        [DataMember]
        public int? SkupinaPredkont_Id { get; set; }

        [DataMember]
        public short? Poradie { get; set; }

        [DataMember]
        public string PolozkaText { get; set; }

        public void ComboCustomize(IWebEasRepositoryBase repository, string column, string kodPolozky, ref PfeComboAttribute comboAttribute)
        {
            int dphRezim = (int)repository.GetNastavenieI("reg", "RezimDph");

            if (dphRezim == 0)
            {
                comboAttribute.AdditionalWhereSql += $"{(string.IsNullOrEmpty(comboAttribute.AdditionalWhereSql) ? string.Empty : " AND ") }{nameof(C_Typ_Id)} != 6";
            }
        }
    }
}
