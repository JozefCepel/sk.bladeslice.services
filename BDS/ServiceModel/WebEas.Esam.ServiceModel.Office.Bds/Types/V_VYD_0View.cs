using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("bds")]
    [Alias("V_VYD_0")]
    [DataContract]
    public class V_VYD_0View : tblD_VYD_0, IPfeCustomize
    {
        [DataMember]
        [PfeColumn(Text = "_C_StavEntity_Id")]
        public int C_StavEntity_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Organisation unit")]
        [PfeCombo(typeof(tblK_ORJ_0), ComboIdColumn = "K_ORJ_0", ComboDisplayColumn = "ORJ")]
        public string ORJ { get; set; }

        [DataMember]
        [PfeColumn(Text = "Warehouse", RequiredFields = new[] { nameof(K_ORJ_0) })]
        [PfeCombo(typeof(V_ORJ_1View), ComboIdColumn = nameof(V_ORJ_1View.K_SKL_0), ComboDisplayColumn = nameof(V_ORJ_1View.SKL))]
        public string SKL { get; set; }

        [DataMember]
        [PfeColumn(Text = "Customer", Xtype = PfeXType.SearchFieldSS, Mandatory = true)]
        [PfeCombo(typeof(V_OBP_0View), ComboDisplayColumn = nameof(V_OBP_0View.IdFormatMeno),
            ComboIdColumn = nameof(V_OBP_0View.K_OBP_0),
            AdditionalWhereSql = "K_TOB_0 IN  (1, 3)",  //Odberateľ
            Tpl = "{value};{AdresaTPSidlo}",
            AdditionalFields = new[] { nameof(V_OBP_0View.AdresaTPSidlo) })]
        public string IdFormatMeno { get; set; }

        // virtualny stlpec, ID by malo byt zhodne s K_OBP_0, vyuziva sa na druhe combo, v DTo sa to este kontroluje ci su rovnake
        [DataMember]
        [Ignore]
        [PfeColumn(Text = "_K_OBP_0_Fake")]
        public long? K_OBP_0_Fake => !string.IsNullOrEmpty(AdresaTPSidlo) ? K_OBP_0 : null; // Init, preberieme hodnotu

        [DataMember]
        [PfeColumn(Text = "Address", Xtype = PfeXType.SearchFieldSS)]
        [PfeCombo(typeof(V_OBP_0View), ComboDisplayColumn = nameof(V_OBP_0View.AdresaTPSidlo), IdColumn = nameof(K_OBP_0_Fake),
            ComboIdColumn = nameof(V_OBP_0View.K_OBP_0),
            AdditionalWhereSql = "K_TOB_0 IN  (1, 3)",  //Odberateľ
            Tpl = "{value};{IdFormatMeno}",
            AdditionalFields = new[] { nameof(V_OBP_0View.IdFormatMeno) })]
        public string AdresaTPSidlo { get; set; }

        [DataMember]
        [PfeColumn(Text = "Name 1", ReadOnly = true, Editable = false)]
        public string NAZOV1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Name 2", ReadOnly = true, Editable = false)]
        public string NAZOV2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Business ID No.", ReadOnly = true, Editable = false)]
        //[PfeCombo(typeof(tblK_OBP_0), ComboIdColumn = "K_OBP_0", ComboDisplayColumn = "ICO")]
        public string ICO { get; set; }

        [DataMember]
        [PfeColumn(Text = "Street", ReadOnly = true, Editable = false)]
        public string ULICA_S { get; set; }

        [DataMember]
        [PfeColumn(Text = "ZIP code", ReadOnly = true, Editable = false)]
        public string PSC_S { get; set; }

        [DataMember]
        [PfeColumn(Text = "City", ReadOnly = true, Editable = false)]
        public string OBEC_S { get; set; }

        [DataMember]
        [PfeColumn(Text = "State", ReadOnly = true, Editable = false)]
        public string STAT { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Created by", Hidden = true, Editable = false, ReadOnly = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Edited by", Hidden = true, Editable = false, ReadOnly = true)]
        public string ZmenilMeno { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_K_OBP_0_ICO")]
        //[Ignore]
        //public int? K_OBP_0_ICO
        //{
        //    get
        //    {
        //        return K_OBP_0;
        //    }
        //}

        //[DataMember]
        //[PfeColumn(Text = "_K_OBP_0_NAZOV1")]
        //[Ignore]
        //public int? K_OBP_0_NAZOV1
        //{
        //    get
        //    {
        //        return K_OBP_0;
        //    }
        //}

        public void CustomizeModel(PfeDataModel model, IWebEasRepositoryBase repository, HierarchyNode node, string filter, HierarchyNode masterNode)
        {
            if (model?.Fields == null || node == null) return;

            #region Browser dialog na osobu
            var nazov1Field = model.Fields.FirstOrDefault(p => p.Name == nameof(IdFormatMeno));
            if (nazov1Field != null)
            {
                nazov1Field.SearchFieldDefinition = new List<PfeSearchFieldDefinition>
                {
                    new PfeSearchFieldDefinition
                    {
                        Code = "bds-kat-obp",
                        NameField = nameof(V_OBP_0View.K_OBP_0),
                        DisplayField = nameof(V_OBP_0View.IdFormatMeno),
                        AdditionalFilterSql = $"K_TOB_0 IN  (1, 3)",
                        AdditionalFilterDesc = "Customers"
                    }
                };
            }

            var adresaField = model.Fields.FirstOrDefault(p => p.Name == nameof(AdresaTPSidlo));
            if (adresaField != null)
            {
                adresaField.SearchFieldDefinition = new List<PfeSearchFieldDefinition>
                {
                    new PfeSearchFieldDefinition
                    {
                        Code = "bds-kat-obp",
                        NameField = nameof(V_OBP_0View.K_OBP_0),
                        DisplayField = nameof(V_OBP_0View.AdresaTPSidlo),
                        AdditionalFilterSql = $"K_TOB_0 IN  (1, 3)",
                        AdditionalFilterDesc = "Customers"
                    }
                };
            }

            #endregion

            #region Disable polí - na základe stavu

            List<string> enblField = new() { "Poznamka", "DL_C" };

            foreach (PfeColumnAttribute col in model.Fields.Where(f => !f.Text.StartsWith("_") && f.Editable && !enblField.Contains(f.Name) &&
                                                                      (!f.ReadOnly || f.Xtype == PfeXType.Combobox || f.Xtype == PfeXType.SearchFieldSS || f.Xtype == PfeXType.SearchFieldMS)))
            {
                col.Validator ??= new PfeValidator { Rules = new List<PfeRule>() };

                col.Validator.Rules.Add(new PfeRule
                {
                    ValidatorType = PfeValidatorType.Disable,
                    Condition = new List<PfeFilterAttribute>
                    {
                        new PfeFilterAttribute
                        {
                            Field = nameof(V),
                            ComparisonOperator = "eq",
                            Value = true
                        }
                    }
                });
            }

            #endregion
        }
    }
}
