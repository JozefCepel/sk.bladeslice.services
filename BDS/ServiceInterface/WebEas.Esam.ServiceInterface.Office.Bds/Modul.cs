using System;
using System.Collections.Generic;
using System.Linq;
using WebEas.Esam.ServiceModel.Office.Bds.Dto;
using WebEas.Esam.ServiceModel.Office.Bds.Types;
using WebEas.ServiceInterface;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Office.Egov.Reg.Types;
using RolesDefinition = WebEas.Esam.ServiceModel.Office.RolesDefinition;

namespace WebEas.Esam.ServiceInterface.Office.Bds
{
    public static class Modul
    {
        public const string Code = "bds";

        #region Roly uzivatelov

        static readonly string[] roles_member = new string[] { RolesDefinition.Bds.Roles.BdsMember };
        static readonly string[] roles_writer = new string[] { RolesDefinition.Bds.Roles.BdsWriter };
        static readonly string[] roles_ciswriter = new string[] { RolesDefinition.Bds.Roles.BdsCisWriter };
        static readonly string[] roles_admin = new string[] { RolesDefinition.Bds.Roles.BdsAdmin };
        static readonly string[] roles_admin_ciswriter = new string[] { RolesDefinition.Bds.Roles.BdsAdmin, RolesDefinition.Bds.Roles.BdsCisWriter };

        #endregion

        public readonly static HierarchyNode HierarchyTree = new HierarchyNode(Code, "Blade Slice")
        {
            Children = new List<HierarchyNode>
            {
                new HierarchyNode("skl","Sklad")
                {
                    #region SKLAD
                    
                    Children = new List<HierarchyNode>
                    {
                        new HierarchyNode<V_PRI_0View>("pri", "Príjemky", null, HierarchyNodeType.DatovaPolozka)
                        {
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.Change, roles_member),
                                new NodeAction(NodeActionType.Delete, typeof(DeleteD_PRI_0)) {SelectionMode = PfeSelection.Multi },
                                new NodeAction(NodeActionType.Update, typeof(UpdateD_PRI_0))
                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateD_PRI_0), roles_member),
                            Children = new List<HierarchyNode>
                            {
                                new HierarchyNode<V_PRI_1View>("pol", "Položky", null, HierarchyNodeType.DatovaPolozka)
                                {
                                    Actions = new List<NodeAction>
                                    {
                                        new NodeAction(NodeActionType.Change, roles_member),
                                        new NodeAction(NodeActionType.Delete, typeof(DeleteD_PRI_1)) {SelectionMode = PfeSelection.Multi },
                                        new NodeAction(NodeActionType.Update, typeof(UpdateD_PRI_1))
                                    }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateD_PRI_1), roles_member),
                                }
                            }
                        },
                        new HierarchyNode<V_VYD_0View>("vyd", "Výdajky", null, HierarchyNodeType.DatovaPolozka)
                        {
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.Change, roles_member),
                                new NodeAction(NodeActionType.Delete, typeof(DeleteD_VYD_0)) {SelectionMode = PfeSelection.Multi },
                                new NodeAction(NodeActionType.Update, typeof(UpdateD_VYD_0))
                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateD_VYD_0), roles_member),
                            Children = new List<HierarchyNode>
                            {
                                new HierarchyNode<V_VYD_1View>("pol", "Položky", null, HierarchyNodeType.DatovaPolozka)
                                {
                                    Actions = new List<NodeAction>
                                    {
                                        new NodeAction(NodeActionType.Change, roles_member),
                                        new NodeAction(NodeActionType.Delete, typeof(DeleteD_VYD_1)) {SelectionMode = PfeSelection.Multi },
                                        new NodeAction(NodeActionType.Update, typeof(UpdateD_VYD_1))
                                    }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateD_VYD_1), roles_member),
                                }
                            }
                        },
                        new HierarchyNode<V_SIM_0View>("sim", "3D simulačné dáta", null, HierarchyNodeType.DatovaPolozka)
                        {
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.Change, roles_member),
                                new NodeAction(NodeActionType.Delete, typeof(DeleteD_SIM_0)) {SelectionMode = PfeSelection.Multi },
                                new NodeAction(NodeActionType.Update, typeof(UpdateD_SIM_0))
                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateD_SIM_0), roles_member)
                        },
                        new HierarchyNode<BdsDummyData>("sts", "Stav skladu", null, HierarchyNodeType.Program)
                        {
                        }
                    }
                    
                    #endregion
                },
                new HierarchyNode("kat", "Katalógy")
                {
                    #region KATALÓGY
                    
                    Children = new List<HierarchyNode>
                    {
                        new HierarchyNode<V_MAT_0View>("mat", "Katalóg materiálov", null, HierarchyNodeType.Ciselnik)
                        {
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.Change, roles_member),
                                new NodeAction(NodeActionType.Delete, typeof(DeleteK_MAT_0)) {SelectionMode = PfeSelection.Multi },
                                new NodeAction(NodeActionType.Update, typeof(UpdateK_MAT_0))
                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateK_MAT_0), roles_member)
                        },
                        new HierarchyNode<V_OBP_0View>("obp", "Katalóg obchodných partnerov", null, HierarchyNodeType.Ciselnik)
                        {
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.Change, roles_member),
                                new NodeAction(NodeActionType.Delete, typeof(DeleteK_OBP_0)) {SelectionMode = PfeSelection.Multi },
                                new NodeAction(NodeActionType.Update, typeof(UpdateK_OBP_0))
                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateK_OBP_0), roles_member)
                        }
                    }
                    
                    #endregion
                },
                new HierarchyNode("cfg", "Konfigurácia")
                {
                    #region Konfigurácia
                    
                    Children = new List<HierarchyNode>
                    {
                        new HierarchyNode("ors", "Organizačná štruktúra")
                        {
                            Children = new List<HierarchyNode>
                            {
                                new HierarchyNode<V_ORJ_0View>("orj", "Organizačné jednotky", null, HierarchyNodeType.Ciselnik)
                                {
                                    Actions = new List<NodeAction>
                                    {
                                        new NodeAction(NodeActionType.Change, roles_member),
                                        new NodeAction(NodeActionType.Delete, typeof(DeleteK_ORJ_0)) {SelectionMode = PfeSelection.Multi },
                                        new NodeAction(NodeActionType.Update, typeof(UpdateK_ORJ_0))
                                    }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateK_ORJ_0), roles_member)
                                },
                                new HierarchyNode<V_SKL_0View>("skl", "Sklady", null, HierarchyNodeType.Ciselnik)
                                {
                                    Actions = new List<NodeAction>
                                    {
                                        new NodeAction(NodeActionType.Change, roles_member),
                                        new NodeAction(NodeActionType.Delete, typeof(DeleteK_SKL_0)) {SelectionMode = PfeSelection.Multi },
                                        new NodeAction(NodeActionType.Update, typeof(UpdateK_SKL_0))
                                    }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateK_SKL_0), roles_member),
                                    Children = new List<HierarchyNode>
                                    {
                                        new HierarchyNode<V_SKL_1View>("tsk", "Priradenie materiálových skupín", null, HierarchyNodeType.Ciselnik)
                                        {
                                            Actions = new List<NodeAction>
                                            {
                                                new NodeAction(NodeActionType.Change, roles_member),
                                                new NodeAction(NodeActionType.Delete, typeof(DeleteK_SKL_1)) {SelectionMode = PfeSelection.Multi },
                                                new NodeAction(NodeActionType.Update, typeof(UpdateK_SKL_1))
                                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateK_SKL_1), roles_member)
                                        },
                                        new HierarchyNode<V_SKL_2View>("loc", "Pozície na sklade", null, HierarchyNodeType.Ciselnik)
                                        {
                                            Actions = new List<NodeAction>
                                            {
                                                new NodeAction(NodeActionType.Change, roles_member),
                                                new NodeAction(NodeActionType.Delete, typeof(DeleteK_SKL_2)) {SelectionMode = PfeSelection.Multi },
                                                new NodeAction(NodeActionType.Update, typeof(UpdateK_SKL_2))
                                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateK_SKL_2), roles_member)
                                        }
                                    }
                                },
                                new HierarchyNode<V_ORJ_1View>("orjskl", "Priradenie skladov ORJ", null, HierarchyNodeType.Ciselnik)
                                {
                                    Actions = new List<NodeAction>
                                    {
                                        new NodeAction(NodeActionType.Change, roles_member),
                                        new NodeAction(NodeActionType.Delete, typeof(DeleteK_ORJ_1)) {SelectionMode = PfeSelection.Multi },
                                        new NodeAction(NodeActionType.Update, typeof(UpdateK_ORJ_1))
                                    }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateK_ORJ_1), roles_member)
                                }
                            }

                        },
                        new HierarchyNode<V_TSK_0View>("tsk", "Materiálové skupiny", null, HierarchyNodeType.Ciselnik)
                        {
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.Change, roles_member),
                                new NodeAction(NodeActionType.Delete, typeof(DeleteK_TSK_0)) {SelectionMode = PfeSelection.Multi },
                                new NodeAction(NodeActionType.Update, typeof(UpdateK_TSK_0))
                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateK_TSK_0), roles_member)
                        }
                    }
                    
                    #endregion
                },
                new HierarchyNode("sm", "Správa modulu")
                {
                    #region Správa modulu
                    
                    Children = new List<HierarchyNode>
                    {
                        new HierarchyNode<Nastavenie>("mset", "Konfigurácia parametrov modulu", null, HierarchyNodeType.Settings)
                        {
                            #region Konfigurácia parametrov modulu
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.UpdateNastavenie, typeof(UpdateNastavenie), roles_admin)
                            }
                            #endregion
                        },
                        //new HierarchyNode<BdsDummyData>("pre", "Preklady", null, HierarchyNodeType.Ciselnik)
                        //{
                        //    #region História zmien stavov
                            
                        //    #endregion
                        //}
                    }
                    
                    #endregion
                }
            },
            Roles = roles_member.ToList()
        }
        #region LayoutDependencies
        .SetLayoutDependencies(
            //HierarchyNodeDependency.One2ManyBack2One("rzp-def-prr", "rzp-def-ciel", "D_Program_Id", "D_PRI_0", "Programový rozpočet"),
        );
        #endregion

        #region Generovanie akcii (tlacitok)

        #endregion
    }
}
