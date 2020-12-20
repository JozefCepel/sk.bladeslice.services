using ServiceStack.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Office.Egov.Reg.Types;

namespace WebEas.Esam.ServiceModel.Office.Types.Reg
{
    [DataContract]
    [Schema("reg")]
    [Alias("V_BiznisEntita_Zaloha")]
    public class BiznisEntita_ZalohaView : BiznisEntita_Zaloha, IPfeCustomize, IBaseView
    {
        // virtualny stlpec, ID sa potom zapise podla typu do D_BiznisEntita_Id_ZF / D_DokladBANPol_Id / D_UhradaParovanie_Id v DTo
        [DataMember]
        [Ignore]
        [PfeColumn(Text = "_Zaloha_Id", Mandatory = true)]
        public long? Zaloha_Id
        {
            get
            {
                switch (C_Typ_Id)
                {
                    case (int)TypEnum.UhradaDZF:
                    case (int)TypEnum.UhradaOZF:
                    case (int)TypEnum.ZalohyPoskytnute:
                    case (int)TypEnum.ZalohyPrijate:
                        return (D_UhradaParovanie_Id == null) ? -D_DokladBANPol_Id : D_UhradaParovanie_Id;
                    default: // Manual
                        return null;
                }
            }
            set { }
        }

        [DataMember]
        [PfeColumn(Text = "S", Editable = false, ReadOnly = true, Tooltip = "Spracované")]
        public bool? S_FA { get; set; }
        
        [DataMember]
        [PfeColumn(Text = "_C_TBE_Id_FA", ReadOnly = true, Editable = false)]
        public short? C_TypBiznisEntity_Id_FA { get; set; } // Napĺňame v GetRowDefaultValues - na základe hodnoty sa potom vyfiltruje combo TypNazov

        [DataMember]
        [PfeColumn(Text = "_D_Osoba_Id", ReadOnly = true, Editable = false)]
        public long? D_Osoba_Id { get; set; }  // Napĺňame v GetRowDefaultValues (pokial je novy), puziva sa na filter v combe VS

        [DataMember]
        [PfeColumn(Text = "Typ", RequiredFields = new[] { nameof(C_TypBiznisEntity_Id_FA)})]
        [PfeCombo(typeof(TypView), IdColumn = nameof(C_Typ_Id), ComboIdColumn = nameof(TypView.C_Typ_Id), ComboDisplayColumn = nameof(TypView.Nazov),
                  AdditionalWhereSql = "(C_Typ_Id = 12) OR (@C_TypBiznisEntity_Id_FA = 2 AND C_Typ_Id IN (105, 115)) OR (@C_TypBiznisEntity_Id_FA = 3 AND C_Typ_Id IN (106, 116))", CustomSortSqlExp = "Nazov")]
        public string TypNazov { get; set; }

        // je aj dole v tabulke (pretazeny stlpec)
        [DataMember]
        [PfeColumn(Text = "VS", Tooltip = "Variabilný symbol", Xtype = PfeXType.SearchFieldSS,
                   RequiredFields = new[] { nameof(C_Typ_Id), nameof(D_Osoba_Id), nameof(Rok) })]
        [PfeCombo(typeof(VSCombo), IdColumn = nameof(Zaloha_Id), ComboIdColumn = nameof(VSCombo.Id), ComboDisplayColumn = nameof(VSCombo.Value),
                  AdditionalFields = new[] { nameof(VSCombo.DM_Nevyfakturovane), nameof(VSCombo.DatumUhrady), nameof(VSCombo.D_BiznisEntita_Id_ZF), nameof(VSCombo.Popis) }, 
                  AllowComboCustomValue = true)]
        [StringLength(40)]
        public new string VS { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string ZmenilMeno { get; set; }

        private PfeSearchFieldDefinition ZalSearchFieldDefinition(TypEnum conditionFilterValue, string code, string nameField, string additionalFilterDesc)
        {
            var res = new PfeSearchFieldDefinition()
            {
                Condition = new List<PfeFilterAttribute>
                    {
                        new PfeFilterAttribute
                        {
                            Field = nameof(C_Typ_Id),
                            ComparisonOperator = "eq",
                            Value = (int)conditionFilterValue,
                            LogicOperator = "AND",
                            LeftBrace = 1,
                            RightBrace = 1
                        }
                    },
                Code = code,
                NameField = nameField,
                DisplayField = "VS",
                AdditionalFilterDesc = additionalFilterDesc
            };

            return res;
        }

        public void CustomizeModel(PfeDataModel model, IWebEasRepositoryBase repository, HierarchyNode node, string filter, object masterNodeParameter, string masterNodeKey)
        {
            if (model.Fields != null)
            {

                #region SearchFieldDefinition

                // dvojicka VSCombo, ak sa nieco meni kukni aj tam
                var vs = model.Fields.FirstOrDefault(p => p.Name == nameof(VS));
                if (vs != null)
                {
                    vs.SearchFieldDefinition = new List<PfeSearchFieldDefinition>
                    {
                        // Nevyfakturovane
                        ZalSearchFieldDefinition(TypEnum.UhradaDZF, "fin-pol-par", nameof(D_UhradaParovanie_Id), "(Spracované, nevyfakturované a dodávateľ z faktúry)"),
                        ZalSearchFieldDefinition(TypEnum.UhradaOZF, "fin-pol-par", nameof(D_UhradaParovanie_Id), "(Spracované, nevyfakturované a odberateľ z faktúry)"),
                    };
                }

                #endregion

            }
        }

    }
}