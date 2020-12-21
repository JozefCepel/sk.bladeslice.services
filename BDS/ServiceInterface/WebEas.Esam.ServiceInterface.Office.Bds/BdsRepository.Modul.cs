using System;
using System.Linq;
using System.Collections.Generic;
using WebEas.Esam.ServiceModel.Office.Bds.Types;
using WebEas.ServiceModel;
using WebEas.Esam.ServiceModel.Office.Bds.Dto;

namespace WebEas.Esam.ServiceInterface.Office.Bds
{
    public partial class BdsRepository
    {
        public override string Code => "bds";

        public override HierarchyNode RenderModuleRootNode(string kodPolozky)
        {
            var rootNode = new HierarchyNode(Code, "Blade Slice - warehouse")
            {
                Children = new List<HierarchyNode>
            {
                new HierarchyNode("skl","Stock")
                {
                    #region Stock
                    
                    Children = new List<HierarchyNode>
                    {
                        new HierarchyNode<V_PRI_0View>("pri", "Receipts", null, icon : HierarchyNodeIconCls.DatovaPolozka)
                        {
                            SelectionMode = PfeSelection.Multi,
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.Change),
                                new NodeAction(NodeActionType.Delete, typeof(DeleteD_PRI_0)) {SelectionMode = PfeSelection.Multi },
                                new NodeAction(NodeActionType.Update, typeof(UpdateD_PRI_0)),
                                new NodeAction(NodeActionType.VybavitDoklady, typeof(GetVybavDokladyReq)) {SelectionMode = PfeSelection.Multi },
                                new NodeAction(NodeActionType.OdvybavitDoklady, typeof(GetOdvybavDokladyReq)) {SelectionMode = PfeSelection.Multi }
                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateD_PRI_0)),
                            Children = new List<HierarchyNode>
                            {
                                new HierarchyNode<V_PRI_1View>("pol", "Items", null, icon : HierarchyNodeIconCls.DatovaPolozka)
                                {
                                    SelectionMode = PfeSelection.Multi,
                                    Actions = new List<NodeAction>
                                    {
                                        new NodeAction(NodeActionType.Change),
                                        new NodeAction(NodeActionType.Delete, typeof(DeleteD_PRI_1)) {SelectionMode = PfeSelection.Multi },
                                        new NodeAction(NodeActionType.Update, typeof(UpdateD_PRI_1))
                                    }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateD_PRI_1)),
                                }
                            }
                        },
                        new HierarchyNode<V_VYD_0View>("vyd", "Expenses", null, icon : HierarchyNodeIconCls.DatovaPolozka)
                        {
                            SelectionMode = PfeSelection.Multi,
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.Change),
                                new NodeAction(NodeActionType.Delete, typeof(DeleteD_VYD_0)) {SelectionMode = PfeSelection.Multi },
                                new NodeAction(NodeActionType.Update, typeof(UpdateD_VYD_0)),
                                new NodeAction(NodeActionType.VybavitDoklady, typeof(GetVybavDokladyReq)) {SelectionMode = PfeSelection.Multi },
                                new NodeAction(NodeActionType.OdvybavitDoklady, typeof(GetOdvybavDokladyReq)) {SelectionMode = PfeSelection.Multi }
                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateD_VYD_0)),
                            Children = new List<HierarchyNode>
                            {
                                new HierarchyNode<V_VYD_1View>("pol", "Items", null, icon : HierarchyNodeIconCls.DatovaPolozka)
                                {
                                    SelectionMode = PfeSelection.Multi,
                                    Actions = new List<NodeAction>
                                    {
                                        new NodeAction(NodeActionType.Change),
                                        new NodeAction(NodeActionType.Delete, typeof(DeleteD_VYD_1)) {SelectionMode = PfeSelection.Multi },
                                        new NodeAction(NodeActionType.Update, typeof(UpdateD_VYD_1))
                                    }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateD_VYD_1)),
                                }
                            }
                        },
                        new HierarchyNode<V_SIM_0View>("sim", "3D simulation data", null, icon : HierarchyNodeIconCls.DatovaPolozka)
                        {
                            SelectionMode = PfeSelection.Multi,
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.Change),
                                new NodeAction(NodeActionType.Delete, typeof(DeleteD_SIM_0)) {SelectionMode = PfeSelection.Multi },
                                new NodeAction(NodeActionType.Update, typeof(UpdateD_SIM_0))
                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateD_SIM_0))
                        },
                        new HierarchyNode<STS_FIFOView>("sts", "Stock info", null, HierarchyNodeType.Program)
                        {
                            Children = new List<HierarchyNode>
                            {
                                new HierarchyNode<STSView>("itm", "Stock moves", null, icon : HierarchyNodeIconCls.History)
                                {
                                }
                            }
                        }
                    }
                    
                    #endregion
                },
                new HierarchyNode("kat", "Catalogs")
                {
                    #region KATALÓGY
                    
                    Children = new List<HierarchyNode>
                    {
                        new HierarchyNode<V_MAT_0View>("mat", "Materials", null, icon : HierarchyNodeIconCls.Ciselnik)
                        {
                            SelectionMode = PfeSelection.Multi,
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.Change),
                                new NodeAction(NodeActionType.Delete, typeof(DeleteK_MAT_0)) {SelectionMode = PfeSelection.Multi },
                                new NodeAction(NodeActionType.Update, typeof(UpdateK_MAT_0))
                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateK_MAT_0))
                        },
                        new HierarchyNode<V_OBP_0View>("obp", "Business partners", null, icon : HierarchyNodeIconCls.Ciselnik)
                        {
                            SelectionMode = PfeSelection.Multi,
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.Change),
                                new NodeAction(NodeActionType.Delete, typeof(DeleteK_OBP_0)) {SelectionMode = PfeSelection.Multi },
                                new NodeAction(NodeActionType.Update, typeof(UpdateK_OBP_0))
                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateK_OBP_0))
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
                                new HierarchyNode<V_ORJ_0View>("orj", "Organisation units", null, icon : HierarchyNodeIconCls.Ciselnik)
                                {
                                    SelectionMode = PfeSelection.Multi,
                                    Actions = new List<NodeAction>
                                    {
                                        new NodeAction(NodeActionType.Change),
                                        new NodeAction(NodeActionType.Delete, typeof(DeleteK_ORJ_0)) {SelectionMode = PfeSelection.Multi },
                                        new NodeAction(NodeActionType.Update, typeof(UpdateK_ORJ_0))
                                    }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateK_ORJ_0))
                                },
                                new HierarchyNode<V_SKL_0View>("skl", "Warehouses", null, icon : HierarchyNodeIconCls.Ciselnik)
                                {
                                    SelectionMode = PfeSelection.Multi,
                                    Actions = new List<NodeAction>
                                    {
                                        new NodeAction(NodeActionType.Change),
                                        new NodeAction(NodeActionType.Delete, typeof(DeleteK_SKL_0)) {SelectionMode = PfeSelection.Multi },
                                        new NodeAction(NodeActionType.Update, typeof(UpdateK_SKL_0))
                                    }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateK_SKL_0)),
                                    Children = new List<HierarchyNode>
                                    {
                                        new HierarchyNode<V_SKL_1View>("tsk", "Material groups assignment", null, icon : HierarchyNodeIconCls.Ciselnik)
                                        {
                                            SelectionMode = PfeSelection.Multi,
                                            Actions = new List<NodeAction>
                                            {
                                                new NodeAction(NodeActionType.Change),
                                                new NodeAction(NodeActionType.Delete, typeof(DeleteK_SKL_1)) {SelectionMode = PfeSelection.Multi },
                                                new NodeAction(NodeActionType.Update, typeof(UpdateK_SKL_1))
                                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateK_SKL_1))
                                        },
                                        new HierarchyNode<V_SKL_2View>("loc", "Warehouse positions", null, icon : HierarchyNodeIconCls.Ciselnik)
                                        {
                                            SelectionMode = PfeSelection.Multi,
                                            Actions = new List<NodeAction>
                                            {
                                                new NodeAction(NodeActionType.Change),
                                                new NodeAction(NodeActionType.Delete, typeof(DeleteK_SKL_2)) {SelectionMode = PfeSelection.Multi },
                                                new NodeAction(NodeActionType.Update, typeof(UpdateK_SKL_2))
                                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateK_SKL_2))
                                        }
                                    }
                                },
                                new HierarchyNode<V_ORJ_1View>("orsk", "Warehouse to org.unit assignment", null, icon : HierarchyNodeIconCls.Ciselnik)
                                {
                                    SelectionMode = PfeSelection.Multi,
                                    Actions = new List<NodeAction>
                                    {
                                        new NodeAction(NodeActionType.Change),
                                        new NodeAction(NodeActionType.Delete, typeof(DeleteK_ORJ_1)) {SelectionMode = PfeSelection.Multi },
                                        new NodeAction(NodeActionType.Update, typeof(UpdateK_ORJ_1))
                                    }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateK_ORJ_1))
                                }
                            }

                        },
                        new HierarchyNode<V_TSK_0View>("tsk", "Material groups", null, icon : HierarchyNodeIconCls.Ciselnik)
                        {
                            SelectionMode = PfeSelection.Multi,
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.Change),
                                new NodeAction(NodeActionType.Delete, typeof(DeleteK_TSK_0)) {SelectionMode = PfeSelection.Multi },
                                new NodeAction(NodeActionType.Update, typeof(UpdateK_TSK_0))
                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateK_TSK_0))
                        }
                    }
                    
                    #endregion
                },
                new HierarchyNode("sm", "Administration")
                {
                    #region Správa modulu
                    
                    Children = new List<HierarchyNode>
                    {
                        new HierarchyNode<V_OWN_0View>("own", "My company", null, icon : HierarchyNodeIconCls.University)
                        {
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.Change),
                                new NodeAction(NodeActionType.Update, typeof(UpdateK_OWN_0))
                            }
                        },
                        new HierarchyNode<Nastavenie>("mset", "Parameters configuration", null, icon : HierarchyNodeIconCls.Settings)
                        {
                            #region Konfigurácia parametrov modulu
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.UpdateNastavenie, typeof(UpdateNastavenie))
                            }
                            #endregion
                        },
                        //new HierarchyNode<BdsDummyData>("pre", "Preklady", null, icon : HierarchyNodeIconCls.Ciselnik)
                        //{
                        //    #region História zmien stavov
                            
                        //    #endregion
                        //}
                    }
                    
                    #endregion
                }
            }
            }
            #region LayoutDependencies
        .SetLayoutDependencies(
                HierarchyNodeDependency.One2ManyBack2One("bds-cfg-ors-orj", "bds-cfg-ors-orsk", "K_ORJ_0", "Warehouse assign.", "Org. unit"),
                HierarchyNodeDependency.One2ManyBack2One("bds-cfg-ors-skl", "bds-cfg-ors-orsk", "K_SKL_0", "Org.unit assign.", "Warehouse"),
                HierarchyNodeDependency.One2ManyBack2One("bds-cfg-ors-skl", "bds-cfg-ors-skl-tsk", "K_SKL_0", "Groups assign.", "Warehouses"),
                HierarchyNodeDependency.One2ManyBack2One("bds-cfg-ors-skl", "bds-cfg-ors-skl-loc", "K_SKL_0", "Locations", "Warehouse"),
                HierarchyNodeDependency.One2ManyBack2One("bds-skl-pri", "bds-skl-pri-pol", "D_PRI_0", "Items", "Head"),
                HierarchyNodeDependency.One2ManyBack2One("bds-skl-vyd", "bds-skl-vyd-pol", "D_VYD_0", "Items", "Head"),
                HierarchyNodeDependency.One2ManyBack2One("bds-skl-pri-pol", "bds-skl-sim", "D_PRI_1", "Simulation data", "Receipt item"),
                HierarchyNodeDependency.One2ManyBack2One("bds-skl-vyd-pol", "bds-skl-sim", "D_VYD_1", "Simulation data", "Expenses item"),
                HierarchyNodeDependency.One2ManyBack2One("bds-skl-pri", "bds-skl-sim", "D_PRI_0", "Simulation data", "Receipt"),
                HierarchyNodeDependency.One2ManyBack2One("bds-skl-vyd", "bds-skl-sim", "D_VYD_0", "Simulation data", "Expense")
            );
            #endregion

            #region Generovanie akcii (tlacitok)

            #endregion
            return rootNode;
        }

    }
}
