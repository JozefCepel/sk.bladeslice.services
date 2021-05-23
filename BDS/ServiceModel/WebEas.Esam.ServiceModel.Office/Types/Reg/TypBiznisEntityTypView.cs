using ServiceStack.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Office.Egov.Reg.Types;

namespace WebEas.Esam.ServiceModel.Office.Types.Reg
{
    [Schema("reg")]
    [Alias("V_TypBiznisEntity_Typ")]
    [DataContract]
    public class TypBiznisEntityTypView : TypBiznisEntityTyp, IBaseView, IPfeCustomize
    {

        [DataMember]
        [PfeIgnore] //Potrebujem iba pre načítanie Default hodnoty ak je Class ako podgrid
        public int? SkupinaPredkont_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_TypKod")]
        public string TypKod { get; set; }

        [DataMember]
        [PfeColumn(Text = "Typ", Xtype = PfeXType.SearchFieldSS)]
        [PfeCombo(typeof(TypView), ComboDisplayColumn = nameof(TypView.Nazov), IdColumn = nameof(C_Typ_Id),
            AdditionalFields = new[] { nameof(TypView.PolozkaText) }, Tpl = "{value};{PolozkaText}",
            CustomSortSqlExp = "PolozkaText DESC, Nazov ASC")]
        public string TypNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Položka")]
        public bool Polozka { get; set; }

        [DataMember]
        [PfeColumn(Text = "Typ dokladu")]
        [PfeCombo(typeof(TypBiznisEntity), ComboDisplayColumn = nameof(TypBiznisEntity.Nazov), IdColumn = nameof(C_TypBiznisEntity_Id))]
        public string TypBiznisEntityNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kniha", RequiredFields = new[] { nameof(C_TypBiznisEntity_Id) })]
        [PfeCombo(typeof(TypBiznisEntity_Kniha), ComboDisplayColumn = nameof(TypBiznisEntity_Kniha.Kod), IdColumn = nameof(C_TypBiznisEntity_Kniha_Id))]
        public string KnihaKod { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string ZmenilMeno { get; set; }

        public void CustomizeModel(PfeDataModel model, IWebEasRepositoryBase repository, HierarchyNode node, string filter, HierarchyNode masterNode)
        {
            if (model.Fields != null)
            {
                var typFld = model.Fields.FirstOrDefault(p => p.Name == nameof(TypNazov));
                if (typFld != null)
                {
                    typFld.SearchFieldDefinition = new List<PfeSearchFieldDefinition>
                    {
                        new PfeSearchFieldDefinition
                        {
                            Code = "reg-cis-typ",
                            NameField = nameof(TypView.C_Typ_Id),
                            DisplayField = nameof(TypView.Nazov)
                        }
                    };
                }
            }
        }
    }
}
