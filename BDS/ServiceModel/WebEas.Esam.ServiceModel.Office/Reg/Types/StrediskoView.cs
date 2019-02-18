using ServiceStack.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Reg.Types
{
    [Schema("reg")]
    [Alias("V_Stredisko")]
    [DataContract]
    public class StrediskoView : Stredisko, IPfeCustomize
    {
        [DataMember]
        [PfeColumn(Text = "_Kód - Názov")]
        [PfeValueColumn]
        public string KodNazov { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true)]
        public string ZmenilMeno { get; set; }

        public void CustomizeModel(PfeDataModel model, IWebEasRepositoryBase repository, HierarchyNode node, string filter)
        {
            if (model.Fields != null)
            {
                var popisField = model.Fields.FirstOrDefault(p => p.Name == "Popis");

                if (popisField != null)
                {
                    popisField.Validator = new PfeValidator
                    {
                        Rules = new List<PfeRule>
                        {
                            new PfeRule
                            {
                                ValidatorType = PfeValidatorType.Disable,
                                Condition = new List<PfeFilterAttribute>
                                {
                                    new PfeFilterAttribute
                                    {
                                        Field = "Kod",
                                        ComparisonOperator = "eq",
                                        Value = "STR1",
                                        LogicOperator = "AND",
                                        Type = PfeDataType.Text,
                                        LeftBrace = 1,
                                        RightBrace = 1
                                    }
                                }
                            }
                        }
                    };
                }
            }
        }
    }
}
