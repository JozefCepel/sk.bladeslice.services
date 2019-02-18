using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Reg.Types;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Rzp.Types
{
    [Schema("rzp")]
    [Alias("V_IntDoklad")]
    [DataContract]
    public class IntDokladView : IntDoklad, IPfeCustomize
    {
        [DataMember]
        [PfeColumn(Text = "Číslo dokladu", Mandatory = true)]
        public string CisloDokladu { get; set; }

        [DataMember]
        [PfeColumn(Text = "Variabilný symbol")]
        public string VS { get; set; }

        [DataMember]
        [PfeColumn(Text = "Účtovné obdobie", Mandatory = true)]
        public byte? UO { get; set; }

        [DataMember]
        [PfeColumn(Text = "StrediskoId", Editable = true, Mandatory = false, Hidden = true, Hideable = false)]
        public long C_Stredisko_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kód strediska", ReadOnly = true)]
        public string StrediskoKod { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov strediska", ReadOnly = true)]
        public string StrediskoNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Stredisko", RequiredFields = new[] { "DatumDokladu" })]
        [PfeCombo(typeof(StrediskoView), NameColumn = "C_Stredisko_Id", AdditionalWhereSql = "((CASE WHEN ISDATE (@DatumDokladu) = 1 THEN @DatumDokladu END) BETWEEN PlatnostOd AND ISNULL(PlatnostDo, CASE WHEN ISDATE (@DatumDokladu) = 1 THEN @DatumDokladu END))")]
        public string StrediskoKodNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Stav", ReadOnly = true)]
        public string StavNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Typ dokladu", ReadOnly = true)]
        public string TypDokladu { get; set; }

        [DataMember]
        [PfeColumn(Text = "Dátum", Type = PfeDataType.Date, Mandatory = true)]
        public DateTime DatumDokladu { get; set; }

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
                var datumDokladu = model.Fields.FirstOrDefault(p => p.Name == "DatumDokladu");

                if (datumDokladu != null)
                {
                    datumDokladu.Validator = new PfeValidator
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
                                        Field = "StavNazov",
                                        ComparisonOperator = "eq",
                                        Value = "Zaúčtovaný",
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