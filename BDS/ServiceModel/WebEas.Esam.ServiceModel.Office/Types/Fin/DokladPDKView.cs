using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Types.Cfe;
using WebEas.Esam.ServiceModel.Office.Types.Osa;
using WebEas.Esam.ServiceModel.Office.Types.Reg;
using WebEas.Esam.ServiceModel.Office.Types.Uct;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Fin
{
    [Schema("fin")]
    [Alias("V_DokladPDK")]
    [DataContract]
    public class DokladPDKView : BiznisEntitaDokladView, IPfeCustomize, IBaseView
    {
        [DataMember]
        [PfeColumn(Text = "Úhrady P/Z", ReadOnly = true)]
        public decimal DM_Uhrady { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Úhrady CM", ReadOnly = true)]
        public decimal CM_Uhrady { get; set; }

        [DataMember]
        [PfeColumn(Text = "Príjem", ReadOnly = true)]
        public decimal DM_Prijem { get; set; }

        [DataMember]
        [PfeColumn(Text = "Výdaj", ReadOnly = true)]
        public decimal DM_Vydaj { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Príjem CM", ReadOnly = true)]
        public decimal CM_Prijem { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Výdaj CM", ReadOnly = true)]
        public decimal CM_Vydaj { get; set; }

        [DataMember]
        [PfeColumn(Text = "Rozdiel", ReadOnly = true)]
        public decimal DM_Rozdiel { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Rozdiel CM", ReadOnly = true)]
        public decimal CM_Rozdiel { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Suma", Tooltip = "Suma dokladu (v domácej mene)", ReadOnly = true)]
        public new decimal DM_Suma { get; set; }

        //V PDK sa základ bez DPH napočítava z položiek po odpočítaní Zak1 a Zak2, keďŽe tie bez DPH neviem určiť
        [DataMember]
        [PfeColumn(Text = "Základ - bez DPH", Tooltip = "Suma základu pre nulovú sadzbu DPH (v domácej mene)", ReadOnly = true)]
        public new decimal DM_Zak0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_User_Id_DokladVyhotovil", Mandatory = true)]
        public Guid D_User_Id_DokladVyhotovil { get; set; }

        [DataMember]
        [PfeColumn(Text = "Vyhotovil")]
        [PfeCombo(typeof(UserComboView), IdColumn = nameof(D_User_Id_DokladVyhotovil), ComboDisplayColumn = nameof(UserComboView.FullName), AdditionalWhereSql = "C_Modul_Id = 7")]
        public string DokladVyhotovil { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_User_Id_Podpisal")]
        public Guid? D_User_Id_Podpisal { get; set; }

        [DataMember]
        [PfeColumn(Text = "Schválil")]
        [PfeCombo(typeof(UserComboView), IdColumn = nameof(D_User_Id_Podpisal), ComboDisplayColumn = nameof(UserComboView.FullName), AdditionalWhereSql = "C_Modul_Id = 6")]
        public string PodpisalMeno { get; set; }


        [DataMember]
        [PfeColumn(Text = "Starý zostatok", ReadOnly = true)]
        public decimal DM_ZostatokStary { get; set; }

        [DataMember]
        [PfeColumn(Text = "Nový zostatok", ReadOnly = true)]
        public decimal DM_ZostatokNovy { get; set; }

        [DataMember]
        [PfeColumn(Text = "DCOM", DefaultValue = 0, ReadOnly = true)]
        public bool DCOM { get; set; }

        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string ZmenilMeno { get; set; }

        [DataMember]
        [HierarchyNodeParameter]
        public new int? C_Pokladnica_Id { get; set; }

        public new void CustomizeModel(PfeDataModel model, IWebEasRepositoryBase repository, HierarchyNode node, string filter, object masterNodeParameter, string masterNodeKey)
        {
            base.CustomizeModel(model, repository, node, filter, masterNodeParameter, masterNodeKey);
            if (model.Fields != null)
            {
                var eSAMRezim = ((IRepositoryBase)repository).GetNastavenieI("reg", "eSAMRezim");
                var isoZdroj = ((IRepositoryBase)repository).GetNastavenieI("reg", "ISOZdroj");
                var isoZdrojNazov = ((IRepositoryBase)repository).GetNastavenieS("reg", "ISOZdrojNazov");

                if (eSAMRezim != 1)
                {
                    var dcom = model.Fields.FirstOrDefault(p => p.Name == nameof(DCOM));
                    dcom.Text = "_DCOM";
                }

                if (eSAMRezim == 1)
                {
                    var na = new NodeAction(NodeActionType.MenuButtonsAll)
                    {
                        ActionIcon = NodeActionIcons.Refresh,
                        Caption = "DCOM",
                        MenuButtons = new List<NodeAction>()
                        {
                            new NodeAction(NodeActionType.DoposlanieUhradDoDcomu)
                            {
                                SelectionMode = PfeSelection.Multi,
                                IdField = "D_BiznisEntita_Id",
                                Url = "/office/reg/long/DoposlanieUhradDoDcomu"
                            }
                        }
                    };
                    node.Actions.Add(na);
                }

                var osobaKontaktKomu = model.Fields.FirstOrDefault(p => p.Name == nameof(OsobaKontaktKomu));

                osobaKontaktKomu.Validator ??= new PfeValidator { Rules = new List<PfeRule>() };

                osobaKontaktKomu.Validator.Rules.Add(new PfeRule
                {
                    ValidatorType = PfeValidatorType.SetLabel,
                    Label = "Prijaté od",
                    Condition = new List<PfeFilterAttribute>
                    {
                        new PfeFilterAttribute
                        {
                            Field = nameof(C_TypBiznisEntity_Kniha_Id),
                            ComparisonOperator = "eq",
                            Value = (int)TypBiznisEntity_KnihaEnum.Prijmove_pokladnicne_doklady,
                            LeftBrace = 1,
                            RightBrace = 1
                        }
                    }
                });

                osobaKontaktKomu.Validator.Rules.Add(new PfeRule
                {
                    ValidatorType = PfeValidatorType.SetLabel,
                    Label = "Vydané komu",
                    Condition = new List<PfeFilterAttribute>
                    {
                        new PfeFilterAttribute
                        {
                            Field = nameof(C_TypBiznisEntity_Kniha_Id),
                            ComparisonOperator = "eq",
                            Value = (int)TypBiznisEntity_KnihaEnum.Vydajove_pokladnicne_doklady,
                            LeftBrace = 1,
                            RightBrace = 1
                        }
                    }
                });

                if (isoZdroj > 0 && repository.Session.Roles.Where(w => w.Contains("REG_MIGRATOR")).Any())
                {
                    if (node.Actions.Any(x => x.ActionType == NodeActionType.MenuButtonsAll))
                    {
                        var polozkaMenuAll = node.Actions.Where(x => x.ActionType == NodeActionType.MenuButtonsAll && x.Caption == "ISO").FirstOrDefault();
                        if (polozkaMenuAll != null)
                        {
                            polozkaMenuAll.Caption = isoZdrojNazov;
                        }
                    }
                }
                else
                {
                    node.Actions.RemoveAll(x => x.ActionType == NodeActionType.MenuButtonsAll && x.Caption == "ISO");
                }
            }
        }
    }
}
