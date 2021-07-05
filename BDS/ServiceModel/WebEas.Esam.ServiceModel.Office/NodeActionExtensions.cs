using System;
using System.Collections.Generic;
using System.Linq;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Office.Egov.Reg.Types;

namespace WebEas.Esam.ServiceModel.Office
{
    public static class NodeActionExtensions
    {
        /// <summary>
        /// Generuje akcie v submenu do MenuButtonsAll
        /// </summary>
        /// <returns></returns>
        public static List<NodeAction> AddMenuButtonsAll(this List<NodeAction> list, NodeActionType type, Type actionType,
            string folderCaption = "Add", string buttonCaption = "", PfeSelection pfeSelection = PfeSelection.Single, NodeActionIcons actionIcon = NodeActionIcons.Default, string url = null)
        {
            var nodeAction = new NodeAction(type, actionType, buttonCaption) { SelectionMode = pfeSelection };
            if (!string.IsNullOrEmpty(url))
            {
                nodeAction.Url = url;
            }

            if (list.Where(f => f.Caption == folderCaption).Count() == 0)
            {
                NodeAction na;
                na = new NodeAction(NodeActionType.MenuButtonsAll)
                {
                    ActionIcon = (folderCaption == "Add") ? NodeActionIcons.Plus : actionIcon,
                    Caption = folderCaption,
                    MenuButtons = new List<NodeAction>()
                    {
                        nodeAction
                    }
                };
                list.Insert(0, na);
            }
            else
            {
                list.Where(f => f.Caption == folderCaption).First().MenuButtons.Add(nodeAction);
            };

            return list;
        }

        public static List<NodeAction> AddWorkFlowActions(this List<NodeAction> list, IWebEasRepositoryBase baseRepository,
                                                         Type actionSpracovat, Type actionPredkontovat, Type actionSkontrolovat, Type actionZauctovat,
                                                         bool rzp, bool uct, bool exdDap = false, bool polozky = false)
        {
            bool rzpAccounter = rzp && baseRepository.Session.HasRole("RZP_ACCOUNTER");
            bool uctAccounter = uct && baseRepository.Session.HasRole("UCT_ACCOUNTER");
            bool editRzp = rzp && baseRepository.GetUserTreeRights("rzp-evi-den").FirstOrDefault() != null && baseRepository.GetUserTreeRights("rzp-evi-den").First().Pravo >= (int)Pravo.Upravovat;
            bool editUct = uct && baseRepository.GetUserTreeRights("uct-evi-den").FirstOrDefault() != null && baseRepository.GetUserTreeRights("uct-evi-den").First().Pravo >= (int)Pravo.Upravovat;

            if (rzp && !uct) //zjednotiť akoby to mali rovnaké
            {
                uctAccounter = rzpAccounter;
                editUct = editRzp;
            }
            else if (!rzp && uct) //zjednotiť akoby to mali rovnaké
            {
                rzpAccounter = uctAccounter;
                editRzp = editUct;
            }

            if (actionSpracovat != null)
            {
                list.Add(new NodeAction(NodeActionType.SpracovatDoklad, actionSpracovat)
                { SelectionMode = PfeSelection.Multi, IdField = "D_BiznisEntita_Id", Url = "/office/reg/long/SpracovatDoklad" });
            }

            if ((actionPredkontovat != null || exdDap) && (editRzp || editUct))
            {
                if (exdDap)
                {
                    list.Add(new NodeAction(NodeActionType.PredkontovatExdDap, actionPredkontovat)
                    {
                        SelectionMode = PfeSelection.Single,
                        IdField = "D_BiznisEntita_Id",
                        Url = "/office/dap/long/PredkontovatDokladDaP"
                    });
                }
                else
                {
                    list.Add(new NodeAction(NodeActionType.Predkontovat, actionPredkontovat)
                    {
                        SelectionMode = PfeSelection.Multi,
                        IdField = "D_BiznisEntita_Id",
                        Url = "/office/reg/long/PredkontovatDoklad"
                    });
                }
            }

            if (actionSkontrolovat != null && actionZauctovat != null) //Vzdy sa posielaju obe naraz
            {
                var actions = new List<NodeAction>();
                if ((editRzp || editUct) && (!rzpAccounter || !uctAccounter))
                {
                    actions.Add(new NodeAction(NodeActionType.SkontrolovatZauctovanie, actionSkontrolovat)
                    { SelectionMode = PfeSelection.Multi, IdField = "D_BiznisEntita_Id", Url = "/office/reg/long/SkontrolovatZauctovanie" });
                }

                if (rzpAccounter || uctAccounter)
                {
                    actions.Add(new NodeAction(NodeActionType.ZauctovatDoklad, actionZauctovat)
                    { SelectionMode = PfeSelection.Multi, IdField = "D_BiznisEntita_Id", Url = "/office/reg/long/ZauctovatDoklad" });
                }

                if (actions.Any())
                {
                    if (polozky)
                    {
                        var na = new NodeAction(NodeActionType.MenuButtonsAll)
                        {
                            Caption = actions.First().Caption,
                            ActionIcon = actions.First().ActionIcon,
                            MenuButtons = actions
                        };

                        list.Insert(0, na);
                    }
                    else
                    {
                        list.AddRange(actions);
                    }
                }
            }

            return list;
        }

        public static List<NodeAction> AddReportActions(this List<NodeAction> list, IWebEasRepositoryBase baseRepository, bool uct, bool addUctDoklad, bool addPoklDoklad, bool addKryciList, string dklCaption)
        {
            bool rzpAccounter = baseRepository.Session.HasRole("RZP_ACCOUNTER");   // rzp vzdy
            bool uctAccounter = uct && baseRepository.Session.HasRole("UCT_ACCOUNTER");
            bool readRzp = baseRepository.GetUserTreeRights("rzp-evi-den").FirstOrDefault() != null && baseRepository.GetUserTreeRights("rzp-evi-den").First().Pravo >= (int)Pravo.Citat;
            bool readUct = uct && baseRepository.GetUserTreeRights("uct-evi-den").FirstOrDefault() != null && baseRepository.GetUserTreeRights("uct-evi-den").First().Pravo >= (int)Pravo.Citat;

            if ((rzpAccounter && readRzp) || (uctAccounter && readUct))
            {
                var n = new NodeAction(NodeActionType.MenuButtons)
                {
                    Caption = "Zostavy",
                    SelectionMode = PfeSelection.Multi,
                    ActionIcon = NodeActionIcons.Zostavy,
                    MenuButtons = new List<NodeAction>()
                };
                if (addPoklDoklad)
                {
                    n.MenuButtons.Add(new NodeAction(NodeActionType.ReportPoklDoklad)
                    {
                        SelectionMode = PfeSelection.Multi,
                        IdField = "D_BiznisEntita_Id",
                        Url = $"/office/fin/long/ReportPoklDoklad",
                        GroupType = "ReportFilter"
                    });
                    n.MenuButtons.Add(new NodeAction(NodeActionType.ViewReportPoklDoklad)
                    {
                        SelectionMode = PfeSelection.Multi,
                        IdField = "D_BiznisEntita_Id",
                        Url = $"fin/PoklDokladReport.trdp",
                        GroupType = "ReportViewer"
                    });
                    n.MenuButtons.Add(new NodeAction(NodeActionType.PrintReportPoklDoklad)
                    {
                        SelectionMode = PfeSelection.Multi,
                        IdField = "D_BiznisEntita_Id",
                        Url = $"/office/fin/long/ReportPoklDoklad",
                        GroupType = "ReportViewer"
                    });
                }
                if (dklCaption != "")
                {
                    n.MenuButtons.Add(new NodeAction(NodeActionType.ReportDoklad)
                    {
                        Caption = dklCaption + " - pdf",
                        SelectionMode = PfeSelection.Multi,
                        IdField = "D_BiznisEntita_Id",
                        Url = $"/office/crm/long/ReportDoklad",
                        GroupType = "ReportFilter",
                        Separator = addPoklDoklad ? "-" : string.Empty
                    });
                    n.MenuButtons.Add(new NodeAction(NodeActionType.ViewReportDoklad)
                    {
                        Caption = dklCaption + " - náhľad",
                        SelectionMode = PfeSelection.Multi,
                        IdField = "D_BiznisEntita_Id",
                        Url = $"crm/DokladReport.trdp",
                        GroupType = "ReportViewer"
                    });
                    n.MenuButtons.Add(new NodeAction(NodeActionType.PrintReportDoklad)
                    {
                        Caption = dklCaption + " - tlač",
                        SelectionMode = PfeSelection.Multi,
                        IdField = "D_BiznisEntita_Id",
                        Url = $"/office/crm/long/ReportDoklad",
                        GroupType = "ReportViewer"
                    });
                }
                if (addKryciList)
                {
                    n.MenuButtons.Add(new NodeAction(NodeActionType.ReportKryciList)
                    {
                        SelectionMode = PfeSelection.Multi,
                        IdField = "D_BiznisEntita_Id",
                        Url = $"/office/crm/long/ReportKryciList",
                        GroupType = "ReportFilter",
                        Separator = addPoklDoklad || dklCaption != "" ? "-" : string.Empty
                    });
                    n.MenuButtons.Add(new NodeAction(NodeActionType.ViewReportKryciList)
                    {
                        SelectionMode = PfeSelection.Multi,
                        IdField = "D_BiznisEntita_Id",
                        Url = $"crm/KryciListReport.trdp",
                        GroupType = "ReportViewer"
                    });
                    n.MenuButtons.Add(new NodeAction(NodeActionType.PrintReportKryciList)
                    {
                        SelectionMode = PfeSelection.Multi,
                        IdField = "D_BiznisEntita_Id",
                        Url = $"/office/crm/long/ReportKryciList",
                        GroupType = "ReportViewer"
                    });
                }
                if (addUctDoklad)
                {
                    n.MenuButtons.Add(new NodeAction(NodeActionType.ReportUctovnyDoklad)
                    {
                        SelectionMode = PfeSelection.Multi,
                        IdField = "D_BiznisEntita_Id",
                        Url = $"/office/uct/long/ReportUctDoklad",
                        GroupType = "ReportFilter",
                        Separator = addPoklDoklad || dklCaption != "" || addKryciList ? "-" : string.Empty
                    });
                    n.MenuButtons.Add(new NodeAction(NodeActionType.ViewReportUctovnyDoklad)
                    {
                        SelectionMode = PfeSelection.Multi,
                        IdField = "D_BiznisEntita_Id",
                        Url = $"uct/UctDokladReport.trdp",
                        GroupType = "ReportViewer"
                    });
                    n.MenuButtons.Add(new NodeAction(NodeActionType.PrintReportUctovnyDoklad)
                    {
                        SelectionMode = PfeSelection.Multi,
                        IdField = "D_BiznisEntita_Id",
                        Url = $"/office/uct/long/ReportUctDoklad",
                        GroupType = "ReportViewer"
                    });
                }
                list.Add(n);
            }
            return list;
        }

        public static List<NodeAction> AddReportKnihaFaktur(this List<NodeAction> list, IWebEasRepositoryBase baseRepository)
        {
            var n = new NodeAction(NodeActionType.MenuButtonsAll)
            {
                Caption = "Zostavy",
                ActionIcon = NodeActionIcons.Zostavy,
                MenuButtons = new List<NodeAction>()
                {
                    new NodeAction(NodeActionType.ReportKnihaFaktur) {Url = $"/office/crm/long/ReportKnihaFaktur" }, //  GroupType = "ReportFilter"
                    new NodeAction(NodeActionType.ViewReportKnihaFaktur) {Url = $"crm/KnihaFakturReport.trdp" }, // , GroupType = "ReportViewer"
                    new NodeAction(NodeActionType.PrintReportKnihaFaktur) {Url = $"/office/crm/long/ReportKnihaFaktur" } // , GroupType = "ReportViewer"
                }
            };
            list.Add(n);
            return list;
        }

        public static List<NodeAction> AddDokladActions(
            this List<NodeAction> list,
            IWebEasRepositoryBase repository,
            string code,
            TypBiznisEntityEnum typBiznisEntityEnum = TypBiznisEntityEnum.Unknown,
            TypBiznisEntityEnum zdrojTypBiznisEntity = TypBiznisEntityEnum.Unknown,
            List<Types.Reg.TypBiznisEntityNastavView> typBiznisEntityNastavView = null)
        {
            var buttons = new NodeAction(NodeActionType.MenuButtons)
            {
                ActionIcon = NodeActionIcons.EllipsisV, //AngleDoubleRight
                Caption = "Viac", //Iné
                SelectionMode = PfeSelection.Multi,
                MenuButtons = new List<NodeAction>()
            };

            if (code == "crm" || code == "fin" || code == "rzp" || code == "uct")
            {
                buttons.MenuButtons.Add(new NodeAction(NodeActionType.CopyMe)
                {
                    SelectionMode = PfeSelection.Multi,
                    Url = $"/office/{code}/CopyDoklad",
                    GroupType = "PostRequest",
                    IdField = "D_BiznisEntita_Id"
                });
            }

            if (typBiznisEntityEnum == TypBiznisEntityEnum.DFA
                || typBiznisEntityEnum == TypBiznisEntityEnum.DZF
                || typBiznisEntityEnum == TypBiznisEntityEnum.OFA
                || typBiznisEntityEnum == TypBiznisEntityEnum.OZF
                || code == "dap"
                )
            {
                buttons.MenuButtons.Add(new NodeAction(NodeActionType.VytvoritPlatPrikaz)
                {
                    SelectionMode = PfeSelection.Multi,
                    Url = "/office/fin/GeneratePolozkyPPP"
                });
            }

            if (code == "crmxx")
            {
                var buttonsExport = new NodeAction(NodeActionType.MenuButtons)
                {
                    Caption = "Export",
                    SelectionMode = PfeSelection.Multi,
                    MenuButtons = new List<NodeAction>()
                };
                buttons.MenuButtons.Add(buttonsExport);
                //list.Add(buttonsExport);
            }

            if (code != "dap") //dane síce majú službu na zmenu stavu, ale nikde nie je použité
            {
                var changeStateDtoTypeName = $"WebEas.Esam.ServiceModel.Office.{code}.Dto.ChangeStateDto, WebEas.Esam.ServiceModel.Office.{code}";
                var changeStateDtoType = Type.GetType(changeStateDtoTypeName, false, true);
                if (changeStateDtoType != null)
                {
                    // TODO: Role budu riesene zvlast na kod polozky cez CFE
                    buttons.MenuButtons.Add(new NodeAction(NodeActionType.ZmenaStavu, changeStateDtoType) { IdField = "D_BiznisEntita_Id" });
                }
            }

            list.Add(buttons);

            if (code == "crm" && zdrojTypBiznisEntity != TypBiznisEntityEnum.Unknown && typBiznisEntityNastavView != null)
            {
                var crmDokladyDod = new List<TypBiznisEntityEnum>()
                {
                    TypBiznisEntityEnum.DFA,
                    TypBiznisEntityEnum.DZF,
                    TypBiznisEntityEnum.DZM,
                    TypBiznisEntityEnum.DOB,
                    TypBiznisEntityEnum.DCP,
                    TypBiznisEntityEnum.DDP,

                };

                var crmDokladyOdb = new List<TypBiznisEntityEnum>()
                {
                    TypBiznisEntityEnum.OFA,
                    TypBiznisEntityEnum.DOL,
                    TypBiznisEntityEnum.OZF,
                    TypBiznisEntityEnum.OZM,
                    TypBiznisEntityEnum.OOB,
                    TypBiznisEntityEnum.OCP,
                    TypBiznisEntityEnum.ODP
                };

                var typyBe = typBiznisEntityNastavView.Where(x => crmDokladyDod.Union(crmDokladyOdb).Any(z => z == (TypBiznisEntityEnum)x.C_TypBiznisEntity_Id));
                if (typyBe.Any(x => x.KodORS == "ORJ" && x.EvidenciaSystem == true && (TypBiznisEntityEnum)x.C_TypBiznisEntity_Id == zdrojTypBiznisEntity))
                {
                    var typyBeZdroj = typyBe.Where(x => x.C_TypBiznisEntity_Id != (int)zdrojTypBiznisEntity).ToList();

                    if (zdrojTypBiznisEntity == TypBiznisEntityEnum.DZF && typyBeZdroj.Any(x => x.C_TypBiznisEntity_Id == (int)TypBiznisEntityEnum.DFA))
                    {
                        var tbe = typyBeZdroj.First(x => x.C_TypBiznisEntity_Id == (int)TypBiznisEntityEnum.DFA);
                        buttons.MenuButtons.Add(CopyDokladAction(NodeActionType.CopyToFA, tbe, NodeActionIcons.AngleDoubleRight, true));
                        typyBeZdroj.Remove(tbe);
                    }

                    if (zdrojTypBiznisEntity == TypBiznisEntityEnum.OZF && typyBeZdroj.Any(x => x.C_TypBiznisEntity_Id == (int)TypBiznisEntityEnum.OFA))
                    {
                        var tbe = typyBeZdroj.First(x => x.C_TypBiznisEntity_Id == (int)TypBiznisEntityEnum.OFA);
                        buttons.MenuButtons.Add(CopyDokladAction(NodeActionType.CopyToFA, tbe, NodeActionIcons.AngleDoubleRight, true));
                        typyBeZdroj.Remove(tbe);
                    }

                    bool first;
                    var defPar = Enum.GetNames(typeof(ParovanieDefEnum));
                    if (defPar.Any(x => x.StartsWith(zdrojTypBiznisEntity.ToString())))
                    {
                        first = true;
                        foreach (var def in defPar.Where(x => x.StartsWith(zdrojTypBiznisEntity.ToString())))
                        {
                            var par = def.Split('_');
                            if (typyBeZdroj.Any(x => ((TypBiznisEntityEnum)x.C_TypBiznisEntity_Id).ToString() == par[1]))
                            {
                                var tbe = typyBeZdroj.First(x => ((TypBiznisEntityEnum)x.C_TypBiznisEntity_Id).ToString() == par[1]);
                                buttons.MenuButtons.Add(CopyDokladAction(NodeActionType.CopyTo, tbe, NodeActionIcons.AngleDoubleRight, first, "&refLeft=" + (int)Enum.Parse(typeof(ParovanieDefEnum), def)));
                                first = false;
                                typyBeZdroj.Remove(tbe);
                            }
                        }
                    }

                    if (defPar.Any(x => x.EndsWith(zdrojTypBiznisEntity.ToString())))
                    {
                        first = true;
                        foreach (var def in defPar.Where(x => x.EndsWith(zdrojTypBiznisEntity.ToString())))
                        {
                            var par = def.Split('_');
                            if (typyBeZdroj.Any(x => ((TypBiznisEntityEnum)x.C_TypBiznisEntity_Id).ToString() == par[0]))
                            {
                                var tbe = typyBeZdroj.First(x => ((TypBiznisEntityEnum)x.C_TypBiznisEntity_Id).ToString() == par[0]);
                                buttons.MenuButtons.Add(CopyDokladAction(NodeActionType.CopyTo, tbe, NodeActionIcons.AngleDoubleLeft, first, "&refRight=" + (int)Enum.Parse(typeof(ParovanieDefEnum), def)));
                                first = false;
                                typyBeZdroj.Remove(tbe);
                            }
                        }
                    }

                    if (crmDokladyDod.Contains(zdrojTypBiznisEntity))
                    {
                        first = true;
                        foreach (var tbe in typyBeZdroj.Where(x => crmDokladyDod.Contains((TypBiznisEntityEnum)x.C_TypBiznisEntity_Id)).ToList())
                        {
                            buttons.MenuButtons.Add(CopyDokladAction(NodeActionType.CopyTo, tbe, NodeActionIcons.FilesO, first));
                            first = false;
                            typyBeZdroj.Remove(tbe);
                        }

                        first = true;
                        foreach (var tbe in typyBeZdroj.ToList())
                        {
                            buttons.MenuButtons.Add(CopyDokladAction(NodeActionType.CopyTo, tbe, NodeActionIcons.FilesO, first));
                            first = false;
                            typyBeZdroj.Remove(tbe);
                        }
                    }

                    if (crmDokladyOdb.Contains(zdrojTypBiznisEntity))
                    {
                        first = true;
                        foreach (var tbe in typyBeZdroj.Where(x => crmDokladyOdb.Contains((TypBiznisEntityEnum)x.C_TypBiznisEntity_Id)).ToList())
                        {
                            buttons.MenuButtons.Add(CopyDokladAction(NodeActionType.CopyTo, tbe, NodeActionIcons.FilesO, first));
                            first = false;
                            typyBeZdroj.Remove(tbe);
                        }

                        first = true;
                        foreach (var tbe in typyBeZdroj.ToList())
                        {
                            buttons.MenuButtons.Add(CopyDokladAction(NodeActionType.CopyTo, tbe, NodeActionIcons.FilesO, first));
                            first = false;
                            typyBeZdroj.Remove(tbe);
                        }
                    }
                }
            }

            if (code == "fin")
            {
                buttons.MenuButtons.Add(new NodeAction(NodeActionType.StornovatDoklad)
                {
                    Url = $"/office/{code}/StornovatDoklad",
                    GroupType = "PostRequest",
                    IdField = "D_BiznisEntita_Id"
                });
            }

            return list;
        }

        private static NodeAction CopyDokladAction(NodeActionType type, Types.Reg.TypBiznisEntityNastavView typBe, NodeActionIcons nodeActionIcon, bool separator, string additionalParam = null)
        {
            return new NodeAction(type)
            {
                SelectionMode = PfeSelection.Multi,
                Url = $"/office/crm/CopyDoklad?CielTyp=" + typBe.C_TypBiznisEntity_Id + (additionalParam ?? string.Empty),
                GroupType = type == NodeActionType.CopyTo ? "PostRequest" : "",
                IdField = "D_BiznisEntita_Id",
                Caption = "Export - " + typBe.TypDokladu.ToLower(),
                ActionIcon = nodeActionIcon,
                Separator = (separator ? "-" : string.Empty)
            };
        }

        public static List<NodeAction> AddReportPrehladFa(this List<NodeAction> list)
        {
            var n = new NodeAction(NodeActionType.MenuButtonsAll)
            {
                Caption = "Zostavy",
                ActionIcon = NodeActionIcons.Zostavy,
                MenuButtons = new List<NodeAction>()
                {
                    new NodeAction(NodeActionType.ReportPrehladFa) { Url = $"/office/crm/long/ReportPrehladFaPdf", GroupType = "ReportFilter"},
                    new NodeAction(NodeActionType.ViewReportPrehladFa) { Url = $"crm/PrehladFaReport.trdp", GroupType = "ReportViewer"},
                    new NodeAction(NodeActionType.PrintReportPrehladFa) { Url = $"/office/crm/long/ReportPrehladFaPdf", GroupType = "ReportViewer"}
                }
            };
            list.Add(n);
            return list;
        }
    }
}