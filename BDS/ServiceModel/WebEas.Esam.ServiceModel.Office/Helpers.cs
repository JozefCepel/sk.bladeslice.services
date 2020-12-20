using System.Collections.Generic;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office
{
    public class Helpers 
    {
        public static List<PfeSearchFieldDefinition> AddOsoba_SearchFieldDefinition()
        {
            return new List<PfeSearchFieldDefinition>
            {
                new PfeSearchFieldDefinition
                {
                    Condition = new List<PfeFilterAttribute>
                    {
                        new PfeFilterAttribute
                        {
                            Field = "C_OsobaTyp_Id",
                            ComparisonOperator = "eq",
                            Value = 1
                        }
                    },
                    Code = "osa-oso-fo",
                    NameField = "D_FO_Osoba_Id", // nepouzijeme nameof() lebo by sme to museli presunut sem z OSA -> vsetko sa zacinalo presuvat sem do ServiceModel.Office (vazby na dalsie zdrojaky)
                    DisplayField = "Identifikator",
                    InputSearchField = "InputSearchField"
                },
                new PfeSearchFieldDefinition
                {
                    Condition = new List<PfeFilterAttribute>
                    {
                        new PfeFilterAttribute
                        {
                            Field = "C_OsobaTyp_Id",
                            ComparisonOperator = "eq",
                            Value = 2
                        }
                    },
                    Code = "osa-oso-po",
                    NameField = "D_PO_Osoba_Id",
                    DisplayField = "ICO",
                    InputSearchField = "InputSearchField"
                }
            };
        }
    }
}
