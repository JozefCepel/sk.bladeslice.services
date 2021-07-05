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
            string folderCaption = "Add", string buttonCaption = "", PfeSelection pfeSelection = PfeSelection.Single, NodeActionIcons actionIcon = NodeActionIcons.Default)
        {
            if (list.Where(f => f.Caption == folderCaption).Count() == 0)
            {
                NodeAction na;
                na = new NodeAction(NodeActionType.MenuButtonsAll)
                {
                    ActionIcon = (folderCaption == "Add") ? NodeActionIcons.Plus : actionIcon,
                    Caption = folderCaption,
                    MenuButtons = new List<NodeAction>()
                    {
                        new NodeAction(type, actionType, buttonCaption) { SelectionMode = pfeSelection }
                    }
                };
                list.Insert(0, na);
            }
            else
            {
                list.Where(f => f.Caption == folderCaption).First().MenuButtons.Add(new NodeAction(type, actionType, buttonCaption));
            };

            return list;
        }

        public static List<NodeAction> AddWorkFlowActions(this List<NodeAction> list, IWebEasRepositoryBase baseRepository,
                                                         Type actionSpracovat, Type actionPredkontovat, Type actionSkontrolovat, Type actionZauctovat,
                                                         bool rzp, bool uct, bool exdDap = false)
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
                if ((editRzp || editUct) && (!rzpAccounter || !uctAccounter))
                {
                    list.Add(new NodeAction(NodeActionType.SkontrolovatZauctovanie, actionSkontrolovat)
                    { SelectionMode = PfeSelection.Multi, IdField = "D_BiznisEntita_Id", Url = "/office/reg/long/SkontrolovatZauctovanie" });
                }

                if (rzpAccounter || uctAccounter)
                {
                    list.Add(new NodeAction(NodeActionType.ZauctovatDoklad, actionZauctovat)
                    { SelectionMode = PfeSelection.Multi, IdField = "D_BiznisEntita_Id", Url = "/office/reg/long/ZauctovatDoklad" });
                }
            }

            return list;
        }

        public static List<NodeAction> AddReportActions(this List<NodeAction> list, IWebEasRepositoryBase baseRepository, bool uct, bool addPoklDoklad, bool addKryciList)
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
                if (addKryciList)
                {
                    n.MenuButtons.Add(new NodeAction(NodeActionType.ReportKryciList)
                    {
                        SelectionMode = PfeSelection.Multi,
                        IdField = "D_BiznisEntita_Id",
                        Url = $"/office/crm/long/ReportKryciList",
                        GroupType = "ReportFilter"
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
                n.MenuButtons.Add(new NodeAction(NodeActionType.ReportUctovnyDoklad)
                    {
                        SelectionMode = PfeSelection.Multi,
                        IdField = "D_BiznisEntita_Id",
                        Url = $"/office/uct/long/ReportUctDoklad",
                        GroupType = "ReportFilter"
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
                list.Add(n);
            }
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
                    Url = $"/office/{code}/GeneratePolozkyPPP",
                    GroupType = "PostRequest"
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


            var changeStateDtoTypeName = $"WebEas.Esam.ServiceModel.Office.{code}.Dto.ChangeStateDto, WebEas.Esam.ServiceModel.Office.{code}";
            var changeStateDtoType = Type.GetType(changeStateDtoTypeName, false, true);
            if (changeStateDtoType != null)
            {
                // TODO: Role budu riesene zvlast na kod polozky cez CFE
                buttons.MenuButtons.Add(new NodeAction(NodeActionType.ZmenaStavu, changeStateDtoType) { IdField = "D_BiznisEntita_Id" });
            }

            list.Add(buttons);

            if (code == "crm" && zdrojTypBiznisEntity != TypBiznisEntityEnum.Unknown && typBiznisEntityNastavView != null)
            {
                var crmDoklady = new List<TypBiznisEntityEnum>()
                {
                    TypBiznisEntityEnum.DFA,
                    TypBiznisEntityEnum.DZF,
                    TypBiznisEntityEnum.DZM,
                    TypBiznisEntityEnum.DOB,
                    TypBiznisEntityEnum.DCP,
                    TypBiznisEntityEnum.DDP,
                    TypBiznisEntityEnum.OFA,
                    TypBiznisEntityEnum.DOL,
                    TypBiznisEntityEnum.OZF,
                    TypBiznisEntityEnum.OZM,
                    TypBiznisEntityEnum.OOB,
                    TypBiznisEntityEnum.OCP,
                    TypBiznisEntityEnum.ODP
                };

                var typyBe = typBiznisEntityNastavView.Where(x => crmDoklady.Any(z => z == (TypBiznisEntityEnum)x.C_TypBiznisEntity_Id));
                if (typyBe.Any(x => x.KodORS == "ORJ" && x.EvidenciaSystem == true && (TypBiznisEntityEnum)x.C_TypBiznisEntity_Id == zdrojTypBiznisEntity))
                {
                    foreach (var typBe in typyBe.Where(x => x.C_TypBiznisEntity_Id != (int)zdrojTypBiznisEntity))
                    {
                        buttons.MenuButtons.Add(new NodeAction(NodeActionType.CopyTo)
                        {
                            SelectionMode = PfeSelection.Multi,
                            Url = $"/office/crm/CopyDoklad?CielTyp=" + typBe.C_TypBiznisEntity_Id,
                            GroupType = "PostRequest",
                            IdField = "D_BiznisEntita_Id",
                            Caption = "Export - " + typBe.TypDokladu.ToLower()
                        });
                    }
                }
            }

            return list;
        }
    }
}