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
                new HierarchyNode("skl","Warehouse")
                {
                    #region Warehouse
                    
                    Children = new List<HierarchyNode>
                    {
                        new HierarchyNode<V_PRI_0View>("pri", "Príjemky", null, HierarchyNodeType.DatovaPolozka)
                        {
                            SelectionMode = PfeSelection.Multi,
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.Change, roles_writer),
                                new NodeAction(NodeActionType.Delete, typeof(DeleteD_PRI_0)) {SelectionMode = PfeSelection.Multi },
                                new NodeAction(NodeActionType.Update, typeof(UpdateD_PRI_0))
                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateD_PRI_0), roles_writer),
                            Children = new List<HierarchyNode>
                            {
                                new HierarchyNode<V_PRI_1View>("pol", "Položky", null, HierarchyNodeType.DatovaPolozka)
                                {
                                    SelectionMode = PfeSelection.Multi,
                                    Actions = new List<NodeAction>
                                    {
                                        new NodeAction(NodeActionType.Change, roles_writer),
                                        new NodeAction(NodeActionType.Delete, typeof(DeleteD_PRI_1)) {SelectionMode = PfeSelection.Multi },
                                        new NodeAction(NodeActionType.Update, typeof(UpdateD_PRI_1))
                                    }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateD_PRI_1), roles_writer),
                                }
                            }
                        },
                        new HierarchyNode<V_VYD_0View>("vyd", "Výdajky", null, HierarchyNodeType.DatovaPolozka)
                        {
                            SelectionMode = PfeSelection.Multi,
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.Change, roles_writer),
                                new NodeAction(NodeActionType.Delete, typeof(DeleteD_VYD_0)) {SelectionMode = PfeSelection.Multi },
                                new NodeAction(NodeActionType.Update, typeof(UpdateD_VYD_0))
                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateD_VYD_0), roles_writer),
                            Children = new List<HierarchyNode>
                            {
                                new HierarchyNode<V_VYD_1View>("pol", "Položky", null, HierarchyNodeType.DatovaPolozka)
                                {
                                    SelectionMode = PfeSelection.Multi,
                                    Actions = new List<NodeAction>
                                    {
                                        new NodeAction(NodeActionType.Change, roles_writer),
                                        new NodeAction(NodeActionType.Delete, typeof(DeleteD_VYD_1)) {SelectionMode = PfeSelection.Multi },
                                        new NodeAction(NodeActionType.Update, typeof(UpdateD_VYD_1))
                                    }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateD_VYD_1), roles_writer),
                                }
                            }
                        },
                        new HierarchyNode<V_SIM_0View>("sim", "3D simulačné dáta", null, HierarchyNodeType.DatovaPolozka)
                        {
                            SelectionMode = PfeSelection.Multi,
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.Change, roles_writer),
                                new NodeAction(NodeActionType.Delete, typeof(DeleteD_SIM_0)) {SelectionMode = PfeSelection.Multi },
                                new NodeAction(NodeActionType.Update, typeof(UpdateD_SIM_0))
                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateD_SIM_0), roles_writer)
                        },
                        new HierarchyNode<BdsDummyData>("sts", "Warehouse info", null, HierarchyNodeType.Program)
                        {
                        }
                    }
                    
                    #endregion
                },
                new HierarchyNode("kat", "Catalogs")
                {
                    #region KATALÓGY
                    
                    Children = new List<HierarchyNode>
                    {
                        new HierarchyNode<V_MAT_0View>("mat", "Materials", null, HierarchyNodeType.Ciselnik)
                        {
                            SelectionMode = PfeSelection.Multi,
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.Change, roles_writer),
                                new NodeAction(NodeActionType.Delete, typeof(DeleteK_MAT_0)) {SelectionMode = PfeSelection.Multi },
                                new NodeAction(NodeActionType.Update, typeof(UpdateK_MAT_0))
                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateK_MAT_0), roles_writer)
                        },
                        new HierarchyNode<V_OBP_0View>("obp", "Business partners", null, HierarchyNodeType.Ciselnik)
                        {
                            SelectionMode = PfeSelection.Multi,
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.Change, roles_writer),
                                new NodeAction(NodeActionType.Delete, typeof(DeleteK_OBP_0)) {SelectionMode = PfeSelection.Multi },
                                new NodeAction(NodeActionType.Update, typeof(UpdateK_OBP_0))
                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateK_OBP_0), roles_writer)
                        }
                    }
                    
                    #endregion
                },
                new HierarchyNode("cfg", "Configuration")
                {
                    #region Konfigurácia
                    
                    Children = new List<HierarchyNode>
                    {
                        new HierarchyNode("ors", "Organisation structure")
                        {
                            Children = new List<HierarchyNode>
                            {
                                new HierarchyNode<V_ORJ_0View>("orj", "Organisation units", null, HierarchyNodeType.Ciselnik)
                                {
                                    SelectionMode = PfeSelection.Multi,
                                    Actions = new List<NodeAction>
                                    {
                                        new NodeAction(NodeActionType.Change, roles_writer),
                                        new NodeAction(NodeActionType.Delete, typeof(DeleteK_ORJ_0)) {SelectionMode = PfeSelection.Multi },
                                        new NodeAction(NodeActionType.Update, typeof(UpdateK_ORJ_0))
                                    }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateK_ORJ_0), roles_writer)
                                },
                                new HierarchyNode<V_SKL_0View>("skl", "Warehouses", null, HierarchyNodeType.Ciselnik)
                                {
                                    SelectionMode = PfeSelection.Multi,
                                    Actions = new List<NodeAction>
                                    {
                                        new NodeAction(NodeActionType.Change, roles_writer),
                                        new NodeAction(NodeActionType.Delete, typeof(DeleteK_SKL_0)) {SelectionMode = PfeSelection.Multi },
                                        new NodeAction(NodeActionType.Update, typeof(UpdateK_SKL_0))
                                    }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateK_SKL_0), roles_writer),
                                    Children = new List<HierarchyNode>
                                    {
                                        new HierarchyNode<V_SKL_1View>("tsk", "Material groups assignment", null, HierarchyNodeType.Ciselnik)
                                        {
                                            SelectionMode = PfeSelection.Multi,
                                            Actions = new List<NodeAction>
                                            {
                                                new NodeAction(NodeActionType.Change, roles_writer),
                                                new NodeAction(NodeActionType.Delete, typeof(DeleteK_SKL_1)) {SelectionMode = PfeSelection.Multi },
                                                new NodeAction(NodeActionType.Update, typeof(UpdateK_SKL_1))
                                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateK_SKL_1), roles_writer)
                                        },
                                        new HierarchyNode<V_SKL_2View>("loc", "Warehouse positions", null, HierarchyNodeType.Ciselnik)
                                        {
                                            SelectionMode = PfeSelection.Multi,
                                            Actions = new List<NodeAction>
                                            {
                                                new NodeAction(NodeActionType.Change, roles_writer),
                                                new NodeAction(NodeActionType.Delete, typeof(DeleteK_SKL_2)) {SelectionMode = PfeSelection.Multi },
                                                new NodeAction(NodeActionType.Update, typeof(UpdateK_SKL_2))
                                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateK_SKL_2), roles_writer)
                                        }
                                    }
                                },
                                new HierarchyNode<V_ORJ_1View>("orsk", "Warehouse to org.unit assignment", null, HierarchyNodeType.Ciselnik)
                                {
                                    SelectionMode = PfeSelection.Multi,
                                    Actions = new List<NodeAction>
                                    {
                                        new NodeAction(NodeActionType.Change, roles_writer),
                                        new NodeAction(NodeActionType.Delete, typeof(DeleteK_ORJ_1)) {SelectionMode = PfeSelection.Multi },
                                        new NodeAction(NodeActionType.Update, typeof(UpdateK_ORJ_1))
                                    }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateK_ORJ_1), roles_writer)
                                }
                            }

                        },
                        new HierarchyNode<V_TSK_0View>("tsk", "Material groups", null, HierarchyNodeType.Ciselnik)
                        {
                            SelectionMode = PfeSelection.Multi,
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.Change, roles_writer),
                                new NodeAction(NodeActionType.Delete, typeof(DeleteK_TSK_0)) {SelectionMode = PfeSelection.Multi },
                                new NodeAction(NodeActionType.Update, typeof(UpdateK_TSK_0))
                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateK_TSK_0), roles_writer)
                        }
                    }
                    
                    #endregion
                },
                new HierarchyNode("sm", "Modul administration")
                {
                    #region Správa modulu
                    
                    Children = new List<HierarchyNode>
                    {
                        new HierarchyNode<Nastavenie>("mset", "Parameters configuration", null, HierarchyNodeType.Settings)
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
            HierarchyNodeDependency.One2ManyBack2One("bds-cfg-ors-orj", "bds-cfg-ors-orsk", "K_ORJ_0", "Priradenie Warehouseov", "Org. unit"),
            HierarchyNodeDependency.One2ManyBack2One("bds-cfg-ors-skl", "bds-cfg-ors-orsk", "K_SKL_0", "Priradenie orj", "Warehouse"),
            HierarchyNodeDependency.One2ManyBack2One("bds-cfg-ors-skl", "bds-cfg-ors-skl-tsk", "K_SKL_0", "Priradenie skupín", "Warehouses"),
            HierarchyNodeDependency.One2ManyBack2One("bds-cfg-ors-skl", "bds-cfg-ors-skl-loc", "K_SKL_0", "Umiestnenia", "Warehouse"),
            HierarchyNodeDependency.One2ManyBack2One("bds-skl-pri", "bds-skl-pri-pol", "D_PRI_0", "Položky", "Hlavička"),
            HierarchyNodeDependency.One2ManyBack2One("bds-skl-vyd", "bds-skl-vyd-pol", "D_VYD_0", "Položky", "Hlavička"),
            HierarchyNodeDependency.One2ManyBack2One("bds-skl-pri-pol", "bds-skl-sim", "D_PRI_1", "Simulation data", "Položka príjemky"),
            HierarchyNodeDependency.One2ManyBack2One("bds-skl-vyd-pol", "bds-skl-sim", "D_VYD_1", "Simulation data", "Položka výdajky")
        );
        #endregion

        #region Generovanie akcii (tlacitok)

        #endregion
    }
}
