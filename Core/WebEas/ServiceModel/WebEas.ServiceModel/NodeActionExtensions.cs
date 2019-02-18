using System;
using System.Collections.Generic;
using System.Linq;

namespace WebEas.ServiceModel
{
    public static class NodeActionExtensions
    {
        /// <summary>
        /// Prida do zoznamu Zobrazenie EPodPodanie, EFormPodanie, Spis
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="requiredRoles">The required roles.</param>
        /// <returns></returns>
        public static List<NodeAction> AddNodeActionPodanie(this List<NodeAction> list, string[] requiredRoles)
        {
            list.Insert(0, new NodeAction(NodeActionType.ZobrazEPodPodanie, requiredRoles));
            list.Insert(1, new NodeAction(NodeActionType.ZobrazEFormPodanie, requiredRoles));
            list.Insert(2, new NodeAction(NodeActionType.ZobrazSpis, requiredRoles));
            return list;
        }

        /// <summary>
        /// Prida do zoznamu Detail žiadateľa
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="requiredRoles">The required roles.</param>
        /// <returns></returns>
        public static List<NodeAction> AddNodeActionOsoba(this List<NodeAction> list, string[] requiredRoles, string OsobaIdField)
        {
            list.Add(new NodeAction(NodeActionType.ZobrazOsobu, requiredRoles) { IdField = OsobaIdField });
            return list;
        }

        /// <summary>
        /// Generuje akcie v submenu do MenuButtonsAll
        /// </summary>
        /// <returns></returns>
        public static List<NodeAction> AddMenuButtonsAll(this List<NodeAction> list, NodeActionType type, Type actionType,
            string[] requiredRoles, string folderCaption = "Pridať", string buttonCaption = "", PfeSelection pfeSelection = PfeSelection.Single)
        {
            if (list.Where(f => f.Caption == folderCaption).Count() == 0)
            {
                NodeAction na;
                na = new NodeAction(NodeActionType.MenuButtonsAll, requiredRoles)
                {
                    ActionIcon = (folderCaption == "Pridať") ? NodeActionIcons.Plus : NodeActionIcons.Default,
                    Caption = folderCaption,
                    MenuButtons = new List<NodeAction>()
                    {
                        new NodeAction(type, actionType, buttonCaption, requiredRoles) { SelectionMode = pfeSelection }
                    }
                };
                list.Insert(0, na);
            }
            else
            {
                list.Where(f => f.Caption == folderCaption).First().MenuButtons.Add(new NodeAction(type, actionType, buttonCaption, requiredRoles));
            };

            return list;
        }


    }
}