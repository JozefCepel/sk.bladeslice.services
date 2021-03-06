﻿using ServiceStack.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Reg
{
    [Schema("reg")]
    [Alias("V_TypBiznisEntityNastav")]
    [DataContract]
    public class TypBiznisEntityNastavView : TypBiznisEntityNastav, IPfeCustomize, IBaseView
    {
        [PfeColumn(Text = "Stavový priestor", ReadOnly = true)]
        [DataMember]
        public string StavovyPriestor { get; set; }

        [PfeColumn(Text = "Kód ORŠ", ReadOnly = true)]
        [DataMember]
        public string KodORS { get; set; }

        [PfeColumn(Text = "Kód dokladu", ReadOnly = true)]
        [DataMember]
        public string KodDokladu { get; set; }

        [PfeColumn(Text = "Typ dokladu", ReadOnly = true)]
        [DataMember]
        public string TypDokladu { get; set; }

        [PfeColumn(Text = "Účtovaný", ReadOnly = true)]
        [DataMember]
        public bool Uctovany { get; set; }

        [DataMember]
        [PfeCombo(typeof(DatumTUEUDVCombo), IdColumn = nameof(DatumDokladuTU), ComboDisplayColumn = nameof(DatumTUEUDVCombo.Nazov))]
        [PfeColumn(Text = "Dátum dokladu TU", Mandatory = true, RequiredFields = new[] { nameof(C_TypBiznisEntity_Id) })]
        public string DatumDokladuTU { get; set; }

        [DataMember]
        [PfeCombo(typeof(DatumTUEUDVCombo), IdColumn = nameof(DatumDokladuEU), ComboDisplayColumn = nameof(DatumTUEUDVCombo.Nazov))]
        [PfeColumn(Text = "Dátum dokladu EÚ", Mandatory = true, RequiredFields = new[] { nameof(C_TypBiznisEntity_Id) })]
        public string DatumDokladuEU { get; set; }

        [DataMember]
        [PfeCombo(typeof(DatumTUEUDVCombo), IdColumn = nameof(DatumDokladuDV), ComboDisplayColumn = nameof(DatumTUEUDVCombo.Nazov))]
        [PfeColumn(Text = "Dátum dokladu DV", Mandatory = true, RequiredFields = new[] { nameof(C_TypBiznisEntity_Id) })]
        public string DatumDokladuDV { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Cust_StrediskoNaPolozke", ReadOnly = true)]
        public bool Cust_StrediskoNaPolozke { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Cust_ProjektNaPolozke", ReadOnly = true)]
        public bool Cust_ProjektNaPolozke { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Cust_UctKluc1NaPolozke", ReadOnly = true)]
        public bool Cust_UctKluc1NaPolozke { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Cust_UctKluc2NaPolozke", ReadOnly = true)]
        public bool Cust_UctKluc2NaPolozke { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Cust_UctKluc3NaPolozke", ReadOnly = true)]
        public bool Cust_UctKluc3NaPolozke { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Cust_EvidenciaDMS", ReadOnly = true)]
        public bool Cust_EvidenciaDMS { get; set; }

        [DataMember]
        [PfeColumn(Text = "Cust_EvidenciaSystem", ReadOnly = true)]
        public bool Cust_EvidenciaSystem { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Cust_CislovanieJedno", ReadOnly = true)]
        public bool Cust_CislovanieJedno { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Cust_DatumDokladuTU", ReadOnly = true)]
        public bool Cust_DatumDokladuTU { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Cust_DatumDokladuEU", ReadOnly = true)]
        public bool Cust_DatumDokladuEU { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Cust_DatumDokladuDV", ReadOnly = true)]
        public bool Cust_DatumDokladuDV { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Cust_UctovatPolozkovite", ReadOnly = true)]
        public bool Cust_UctovatPolozkovite { get; set; }

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
                var strediskoNaPolozke = model.Fields.FirstOrDefault(p => p.Name == nameof(StrediskoNaPolozke));
                #region strediskoNaPolozke
                if (strediskoNaPolozke != null)
                {
                    strediskoNaPolozke.Validator = new PfeValidator
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
                                        Field = nameof(KodDokladu),
                                        ComparisonOperator = "eq",
                                        Value = "PPP",
                                        LogicOperator = "OR",
                                        LeftBrace = 1,
                                        RightBrace = 1
                                    },
                                    new PfeFilterAttribute
                                    {
                                        Field = nameof(KodDokladu),
                                        ComparisonOperator = "eq",
                                        Value = "IND",
                                        LogicOperator = "OR",
                                        LeftBrace = 1,
                                        RightBrace = 1
                                    },
                                    new PfeFilterAttribute
                                    {
                                        Field = nameof(KodDokladu),
                                        ComparisonOperator = "eq",
                                        Value = "DAP",
                                        LogicOperator = "OR",
                                        LeftBrace = 1,
                                        RightBrace = 1
                                    }
                                }
                            }
                        }
                    };
                }
                #endregion

                var uctovatPolozkovite = model.Fields.FirstOrDefault(p => p.Name == nameof(UctovatPolozkovite));
                #region uctovatPolozkovite
                if (uctovatPolozkovite != null)
                {
                    uctovatPolozkovite.Validator = new PfeValidator
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
                                        Field = nameof(KodDokladu),
                                        ComparisonOperator = "eq",
                                        Value = "ODP",
                                        LogicOperator = "OR",
                                        LeftBrace = 1,
                                        RightBrace = 1
                                    },
                                    new PfeFilterAttribute
                                    {
                                        Field = nameof(KodDokladu),
                                        ComparisonOperator = "eq",
                                        Value = "DDP",
                                        LogicOperator = "OR",
                                        LeftBrace = 1,
                                        RightBrace = 1
                                    },
                                    new PfeFilterAttribute
                                    {
                                        Field = nameof(KodDokladu),
                                        ComparisonOperator = "eq",
                                        Value = "OCP",
                                        LogicOperator = "OR",
                                        LeftBrace = 1,
                                        RightBrace = 1
                                    },
                                    new PfeFilterAttribute
                                    {
                                        Field = nameof(KodDokladu),
                                        ComparisonOperator = "eq",
                                        Value = "DCP",
                                        LogicOperator = "OR",
                                        LeftBrace = 1,
                                        RightBrace = 1
                                    },
                                    new PfeFilterAttribute
                                    {
                                        Field = nameof(KodDokladu),
                                        ComparisonOperator = "eq",
                                        Value = "OOB",
                                        LogicOperator = "OR",
                                        LeftBrace = 1,
                                        RightBrace = 1
                                    },
                                    new PfeFilterAttribute
                                    {
                                        Field = nameof(KodDokladu),
                                        ComparisonOperator = "eq",
                                        Value = "DOB",
                                        LogicOperator = "OR",
                                        LeftBrace = 1,
                                        RightBrace = 1
                                    },
                                    new PfeFilterAttribute
                                    {
                                        Field = nameof(KodDokladu),
                                        ComparisonOperator = "eq",
                                        Value = "OZM",
                                        LogicOperator = "OR",
                                        LeftBrace = 1,
                                        RightBrace = 1
                                    },
                                    new PfeFilterAttribute
                                    {
                                        Field = nameof(KodDokladu),
                                        ComparisonOperator = "eq",
                                        Value = "DZM",
                                        LogicOperator = "OR",
                                        LeftBrace = 1,
                                        RightBrace = 1
                                    },
                                    new PfeFilterAttribute
                                    {
                                        Field = nameof(KodDokladu),
                                        ComparisonOperator = "eq",
                                        Value = "OZF",
                                        LogicOperator = "OR",
                                        LeftBrace = 1,
                                        RightBrace = 1
                                    },
                                    new PfeFilterAttribute
                                    {
                                        Field = nameof(KodDokladu),
                                        ComparisonOperator = "eq",
                                        Value = "DZF",
                                        LogicOperator = "OR",
                                        LeftBrace = 1,
                                        RightBrace = 1
                                    },
                                    new PfeFilterAttribute
                                    {
                                        Field = nameof(KodDokladu),
                                        ComparisonOperator = "eq",
                                        Value = "PPP",
                                        LogicOperator = "OR",
                                        LeftBrace = 1,
                                        RightBrace = 1
                                    },
                                    new PfeFilterAttribute
                                    {
                                        Field = nameof(KodDokladu),
                                        ComparisonOperator = "eq",
                                        Value = "DOL",
                                        LogicOperator = "OR",
                                        LeftBrace = 1,
                                        RightBrace = 1
                                    }
                                }
                            }
                        }
                    };
                }
                #endregion
            }
        }
    }

}
