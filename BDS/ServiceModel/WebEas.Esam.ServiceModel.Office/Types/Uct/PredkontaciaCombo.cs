using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Uct
{
    [DataContract]
    [Schema("uct")]
    [Alias("V_PredkontaciaCombo")]
    public class PredkontaciaCombo: IPfeCustomizeCombo
    {
        [DataMember]
        [PrimaryKey]
        [PfeColumn(Text = "Id")]
        public long C_Predkontacia_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Nazov")]
        public string Nazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Pč")]
        public short Poradie { get; set; }

        [DataMember]
        [PrimaryKey]
        [PfeColumn(Text = "C_TypBiznisEntity_Kniha_Id")]
        public int C_TypBiznisEntity_Kniha_Id { get; set; }

        public void ComboCustomize(IWebEasRepositoryBase repository, string column, string kodPolozky, ref PfeComboAttribute comboAttribute)
        {
            comboAttribute.CustomSortSqlExp = nameof(Poradie);
        }

    }

}
