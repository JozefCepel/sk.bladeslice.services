using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Reg.Types
{
    [DataContract]
    [Schema("reg")]
    [Alias("V_TextaciaPol")]
    public class TextaciaPolView : TextaciaPol, IPfeCustomize
    {
        [DataMember]
        [PfeColumn(Text = "Textácia")]
        [PfeCombo(typeof(TextaciaView), IdColumn = nameof(C_Textacia_Id), ComboDisplayColumn = nameof(TextaciaView.Nazov), ComboIdColumn = nameof(TextaciaView.C_Textacia_Id))]
        public string TextaciaNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Typ dátumu")]
        [PfeCombo(typeof(DatumTypCombo), IdColumn = nameof(DatumTyp))]
        [Ignore]
        public string DatumTypText => DatumTypCombo.GetText(DatumTyp);

        [DataMember]
        [PfeColumn(Text = "_C_TypBiznisEntity_Id", Hidden = true, Editable = false, ReadOnly = true)]
        public short? C_TypBiznisEntity_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_TypBiznisEntity_Kniha_Id", Hidden = true, Editable = false, ReadOnly = true)]
        public int? C_TypBiznisEntity_Kniha_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_Druh_Id", Hidden = true, Editable = false, ReadOnly = true)]
        public long? C_Druh_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_RokOd", Hidden = true, Editable = false, ReadOnly = true)]
        public short RokOd { get; set; }

        [DataMember]
        [PfeColumn(Text = "_RokDo", Hidden = true, Editable = false, ReadOnly = true)]
        public short? RokDo { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string ZmenilMeno { get; set; }

        public void CustomizeModel(PfeDataModel model, IWebEasRepositoryBase repository, HierarchyNode node, string filter, HierarchyNode masterNode)
        {
            var dat = model.Fields.FirstOrDefault(p => p.Name == nameof(Datum));
            if (dat != null)
            {
                dat.Validator ??= new PfeValidator
                {
                    Rules = new List<PfeRule>()
                };
                dat.Validator.Rules.Add(new PfeRule
                {
                    ValidatorType = PfeValidatorType.Disable,
                    Condition = new List<PfeFilterAttribute>
                    {
                        new PfeFilterAttribute
                        {
                            Field = nameof(DatumTyp),
                            ComparisonOperator = "ne",
                            Value = null,
                            LogicOperator = "AND"
                        }
                    }
                });
            }
        }
    }
}
