using System;
using System.Collections.Generic;
using System.Linq;

namespace WebEas.ServiceModel
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
            bool editRzp = rzp && baseRepository.GetUserTreeRights("rzp-evi-den").FirstOrDefault().Pravo >= (int)Pravo.Upravovat;
            bool editUct = uct && baseRepository.GetUserTreeRights("uct-evi-den").FirstOrDefault().Pravo >= (int)Pravo.Upravovat;

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
    }
}