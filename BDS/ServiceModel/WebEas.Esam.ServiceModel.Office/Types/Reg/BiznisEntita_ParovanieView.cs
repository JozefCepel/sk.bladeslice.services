using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Office.Egov.Reg.Types;

namespace WebEas.Esam.ServiceModel.Office.Types.Reg
{
    [DataContract]
    [Schema("reg")]
    [Alias("V_BiznisEntita_Parovanie")]
    public class BiznisEntita_ParovanieView : BiznisEntita_Parovanie, IPfeCustomize, IBaseView
    {
        [DataMember]
        [PfeColumn(Text = "_Id typ biznis entity master", ReadOnly = true, Editable = false)]
        [Ignore]
        public short C_TypBiznisEntity_Id_Master { get; set; } // JC: Napĺňame v GetRowDefaultValues - na základe hodnoty sa vyfiltruje combo TypBeParovanieNazov

        [DataMember]
        [PfeColumn(Text = "_Id biznis entita master", ReadOnly = true, Editable = false)]
        [Ignore]
        public short D_BiznisEntita_Id_Master { get; set; }  // JC: Napĺňame v GetRowDefaultValues - po výbere párovania sa predplní na FE ID do hodnoty _1 alebo _2

        [DataMember]
        [PfeColumn(Text = "_D_Osoba_Id", ReadOnly = true, Editable = false)]
        public long? D_Osoba_Id { get; set; }  // Napĺňame v GetRowDefaultValues (pokial je novy), puziva sa na filter v combach BiznisEntitaPopis1/2

        [DataMember]
        [PfeColumn(Text = "Typ párovania", Mandatory = true, RequiredFields = new[] { "C_TypBiznisEntity_Id_Master", "Plnenie"})] // JC: Plnenie je doplnené iba preto aby sa vyvolala combo služba aj v globálnej položke párovaní (ukáže všetky párovania)
        [PfeCombo(typeof(TypBiznisEntity_ParovanieDefView),
            IdColumn = nameof(C_TypBiznisEntity_ParovanieDef_Id),
            ComboIdColumn = nameof(TypBiznisEntity_ParovanieDefView.C_TypBiznisEntity_ParovanieDef_Id),
            ComboDisplayColumn = nameof(TypBiznisEntity_ParovanieDefView.Nazov),
            AdditionalFields = new[] { nameof(TypBiznisEntity_ParovanieDefView.C_TypBiznisEntity_Id_1), nameof(TypBiznisEntity_ParovanieDefView.C_TypBiznisEntity_Id_2) },
            AdditionalWhereSql = "@Plnenie = @Plnenie AND (C_TypBiznisEntity_Id_1 = @C_TypBiznisEntity_Id_Master OR C_TypBiznisEntity_Id_2 = @C_TypBiznisEntity_Id_Master OR ISNULL(@C_TypBiznisEntity_Id_Master, 0) = 0)")]
        public string TypBeParovanieNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "_ParovanieTyp", ReadOnly = true, Editable = false)]
        public byte ParovanieTyp { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Id typ biznis entity 1", ReadOnly = true, Editable = false)]
        public short C_TypBiznisEntity_Id_1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Id typ biznis entity 2", ReadOnly = true, Editable = false)]
        public short C_TypBiznisEntity_Id_2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Doklad 1", Xtype = PfeXType.SearchFieldSS, RequiredFields = new[] { "D_BiznisEntita_Id_1", "C_TypBiznisEntity_ParovanieDef_Id", "C_TypBiznisEntity_Id_1", "D_Osoba_Id" })]
        [PfeCombo(typeof(BiznisEntitaView),
            IdColumn = nameof(D_BiznisEntita_Id_1),
            ComboIdColumn = nameof(BiznisEntitaView.D_BiznisEntita_Id),
            ComboDisplayColumn = nameof(BiznisEntitaView.BiznisEntitaPopis),
            AdditionalFields = new[] { nameof(BiznisEntitaView.Popis), nameof(BiznisEntitaView.DM_Suma) },
            Tpl = "{value};{Popis};{DM_Suma}",
            AdditionalWhereSql = @"C_TypBiznisEntity_Id = @C_TypBiznisEntity_Id_1 AND
                                   D_Osoba_Id = @D_Osoba_Id AND
                                   D_BiznisEntita_Id NOT IN (SELECT D_BiznisEntita_Id_1 FROM reg.V_BiznisEntita_Parovanie 
                                                             WHERE ParovanieTyp IN (1,2) AND 
                                                                   D_BiznisEntita_Id_1 <> ISNULL(@D_BiznisEntita_Id_1, 0) AND 
                                                                   C_TypBiznisEntity_ParovanieDef_Id = @C_TypBiznisEntity_ParovanieDef_Id)" // 1_1, 1_N
            )]
        public string BiznisEntitaPopis_1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Doklad 2", Xtype = PfeXType.SearchFieldSS, RequiredFields = new[] { "D_BiznisEntita_Id_2", "C_TypBiznisEntity_ParovanieDef_Id", "C_TypBiznisEntity_Id_2", "D_Osoba_Id" })]
        [PfeCombo(typeof(BiznisEntitaView),
            IdColumn = nameof(D_BiznisEntita_Id_2),
            ComboIdColumn = nameof(BiznisEntitaView.D_BiznisEntita_Id),
            ComboDisplayColumn = nameof(BiznisEntitaView.BiznisEntitaPopis),
            AdditionalFields = new[] { nameof(BiznisEntitaView.Popis), nameof(BiznisEntitaView.DM_Suma) },
            Tpl = "{value};{Popis};{DM_Suma}",
            AdditionalWhereSql = @"C_TypBiznisEntity_Id = @C_TypBiznisEntity_Id_2 AND
                                   D_Osoba_Id = @D_Osoba_Id AND
                                   D_BiznisEntita_Id NOT IN (SELECT D_BiznisEntita_Id_2 FROM reg.V_BiznisEntita_Parovanie 
                                                             WHERE ParovanieTyp IN (1,3) AND
                                                                   D_BiznisEntita_Id_2 <> ISNULL(@D_BiznisEntita_Id_2, 0) AND 
                                                                   C_TypBiznisEntity_ParovanieDef_Id = @C_TypBiznisEntity_ParovanieDef_Id)" // 1_1, N_1
        )]
        public string BiznisEntitaPopis_2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Číslo dokladu 1", ReadOnly = true, Xtype = PfeXType.Link)]
        public string CisloInterne_1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Číslo dokladu 2", ReadOnly = true, Xtype = PfeXType.Link)]
        public string CisloInterne_2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_URL_1", ReadOnly = true)] //Natvrdo v kóde hľadá k PfeXType.Link - field URL_1
        public string URL_1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_URL_2", ReadOnly = true)] //Natvrdo v kóde hľadá k PfeXType.Link - field URL_2
        public string URL_2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_DatumDokladu_1", ReadOnly = true, Type = PfeDataType.Date)]
        public DateTime DatumDokladu_1 { get; set; }

        [PfeColumn(Text = "_DatumDokladu_2", ReadOnly = true, Type = PfeDataType.Date)]
        public DateTime DatumDokladu_2 { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, ReadOnly = true, LoadWhenVisible = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, ReadOnly = true, LoadWhenVisible = true)]
        public string ZmenilMeno { get; set; }

        public void CustomizeModel(PfeDataModel model, IWebEasRepositoryBase repository, HierarchyNode node, string filter, HierarchyNode masterNode)
        {
            #region TypBeParovanieNazov

            var be = model.Fields.FirstOrDefault(p => p.Name == nameof(TypBeParovanieNazov));
            if (be != null)
            {
                be.Validator = new PfeValidator
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
                                        Field = nameof(DatumVytvorenia),
                                        ComparisonOperator = "ne",
                                        Value = null
                                    }
                                }
                            }
                        }
                };
            }

            var bep1 = model.Fields.FirstOrDefault(p => p.Name == nameof(BiznisEntitaPopis_1));
            if (bep1 != null)
            {
                bep1.SearchFieldDefinition = new List<PfeSearchFieldDefinition>
                    {
                        ParovanieSearchFieldDef(TypBiznisEntityEnum.DFA, "crm-dod-dfa", nameof(C_TypBiznisEntity_Id_1), nameof(BiznisEntitaPopis_1)),
                        ParovanieSearchFieldDef(TypBiznisEntityEnum.DZF, "crm-dod-dzf", nameof(C_TypBiznisEntity_Id_1), nameof(BiznisEntitaPopis_1)),
                        ParovanieSearchFieldDef(TypBiznisEntityEnum.DOB, "crm-dod-dob", nameof(C_TypBiznisEntity_Id_1), nameof(BiznisEntitaPopis_1)),
                        ParovanieSearchFieldDef(TypBiznisEntityEnum.DZM, "crm-dod-dzm", nameof(C_TypBiznisEntity_Id_1), nameof(BiznisEntitaPopis_1)),
                        // ParovanieSearchFieldDef(TypBiznisEntityEnum.DCP, "crm-dod-dcp", nameof(C_TypBiznisEntity_Id_1), nameof(BiznisEntitaPopis_1)),
                        // ParovanieSearchFieldDef(TypBiznisEntityEnum.DDP, "crm-dod-ddp", nameof(C_TypBiznisEntity_Id_1), nameof(BiznisEntitaPopis_1)),
                        ParovanieSearchFieldDef(TypBiznisEntityEnum.OFA, "crm-odb-ofa", nameof(C_TypBiznisEntity_Id_1), nameof(BiznisEntitaPopis_1)),
                        ParovanieSearchFieldDef(TypBiznisEntityEnum.OZF, "crm-odb-ozf", nameof(C_TypBiznisEntity_Id_1), nameof(BiznisEntitaPopis_1)),
                        ParovanieSearchFieldDef(TypBiznisEntityEnum.OOB, "crm-odb-oob", nameof(C_TypBiznisEntity_Id_1), nameof(BiznisEntitaPopis_1)),
                        ParovanieSearchFieldDef(TypBiznisEntityEnum.OZM, "crm-odb-ozm", nameof(C_TypBiznisEntity_Id_1), nameof(BiznisEntitaPopis_1)),
                        // ParovanieSearchFieldDef(TypBiznisEntityEnum.OCP, "crm-odb-ocp", nameof(C_TypBiznisEntity_Id_1), nameof(BiznisEntitaPopis_1)),
                        // ParovanieSearchFieldDef(TypBiznisEntityEnum.ODP, "crm-odb-odp", nameof(C_TypBiznisEntity_Id_1), nameof(BiznisEntitaPopis_1)),
                    };
            }

            var bep2 = model.Fields.FirstOrDefault(p => p.Name == nameof(BiznisEntitaPopis_2));
            if (bep2 != null)
            {
                bep2.SearchFieldDefinition = new List<PfeSearchFieldDefinition>
                    {
                        ParovanieSearchFieldDef(TypBiznisEntityEnum.DFA, "crm-dod-dfa", nameof(C_TypBiznisEntity_Id_2), nameof(BiznisEntitaPopis_2)),
                        ParovanieSearchFieldDef(TypBiznisEntityEnum.DZF, "crm-dod-dzf", nameof(C_TypBiznisEntity_Id_2), nameof(BiznisEntitaPopis_2)),
                        ParovanieSearchFieldDef(TypBiznisEntityEnum.DOB, "crm-dod-dob", nameof(C_TypBiznisEntity_Id_2), nameof(BiznisEntitaPopis_2)),
                        ParovanieSearchFieldDef(TypBiznisEntityEnum.DZM, "crm-dod-dzm", nameof(C_TypBiznisEntity_Id_2), nameof(BiznisEntitaPopis_2)),
                        // ParovanieSearchFieldDef(TypBiznisEntityEnum.DCP, "crm-dod-dcp", nameof(C_TypBiznisEntity_Id_2), nameof(BiznisEntitaPopis_2)),
                        // ParovanieSearchFieldDef(TypBiznisEntityEnum.DDP, "crm-dod-ddp", nameof(C_TypBiznisEntity_Id_2), nameof(BiznisEntitaPopis_2)),
                        ParovanieSearchFieldDef(TypBiznisEntityEnum.OFA, "crm-odb-ofa", nameof(C_TypBiznisEntity_Id_2), nameof(BiznisEntitaPopis_2)),
                        ParovanieSearchFieldDef(TypBiznisEntityEnum.OZF, "crm-odb-ozf", nameof(C_TypBiznisEntity_Id_2), nameof(BiznisEntitaPopis_2)),
                        ParovanieSearchFieldDef(TypBiznisEntityEnum.OOB, "crm-odb-oob", nameof(C_TypBiznisEntity_Id_2), nameof(BiznisEntitaPopis_2)),
                        ParovanieSearchFieldDef(TypBiznisEntityEnum.OZM, "crm-odb-ozm", nameof(C_TypBiznisEntity_Id_2), nameof(BiznisEntitaPopis_2)),
                        // ParovanieSearchFieldDef(TypBiznisEntityEnum.OCP, "crm-odb-ocp", nameof(C_TypBiznisEntity_Id_2), nameof(BiznisEntitaPopis_2)),
                        // ParovanieSearchFieldDef(TypBiznisEntityEnum.ODP, "crm-odb-odp", nameof(C_TypBiznisEntity_Id_2), nameof(BiznisEntitaPopis_2)),
                    };
            }
            #endregion

            #region Akcie

            if (masterNode.Kod == "par")
            {
                node.Actions.RemoveAll(x => x.ActionType != NodeActionType.ReadList);
            }

            #endregion
        }

        private PfeSearchFieldDefinition ParovanieSearchFieldDef(TypBiznisEntityEnum conditionFilterValue, string code, string sField, string sDisplayField)
        {
            var res = new PfeSearchFieldDefinition()
            {
                Condition = new List<PfeFilterAttribute>
                    {
                        new PfeFilterAttribute
                        {
                            Field = sField,
                            ComparisonOperator = "eq",
                            Value = (int)conditionFilterValue,
                            LogicOperator = "AND",
                            LeftBrace = 1,
                            RightBrace = 1
                        }
                    },
                Code = code,
                NameField = nameof(BiznisEntitaDokladView.D_BiznisEntita_Id),
                DisplayField = sDisplayField,
                AdditionalFilterDesc = "(osoba dokladu)",
                InputSearchField = nameof(BiznisEntitaDokladView.BiznisEntitaPopis)
            };
            return res;
        }

    }
}