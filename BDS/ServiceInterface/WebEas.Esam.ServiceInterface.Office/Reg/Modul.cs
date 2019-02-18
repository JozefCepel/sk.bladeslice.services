using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebEas.Esam.ServiceModel.Office.Reg.Dto;
using WebEas.Esam.ServiceModel.Office.Reg.Types;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Office.Egov.Reg.Types;
using WebEas.ServiceModel.Reg.Types;

namespace WebEas.Esam.ServiceInterface.Office.Reg
{
    public static class Modul
    {
        #region Roly uzivatelov

        static readonly string[] roles_writer = new string[] { WebEas.ServiceModel.Office.Egov.Reg.Roles.RegWriter };
        static readonly string[] modules_roles_member = new string[]
        {
            ServiceModel.Office.RolesDefinition.Rzp.Roles.RzpMember,
        };
        static readonly string[] modules_roles_writer = new string[]
        {
            ServiceModel.Office.RolesDefinition.Rzp.Roles.RzpWriter,
        };

        #endregion

        public readonly static HierarchyNode HierarchyTree = new HierarchyNode(Code, "Registre")
        {
            Children = new List<HierarchyNode>
            {
                new HierarchyNode<DummyCombo>("def", "Definície *",null, HierarchyNodeType.Ciselnik)
                {
                    #region Definicie
                        
                    Children = new List<HierarchyNode>
                    {
                        new HierarchyNode<DummyCombo>("mod", "Moduly *", null, HierarchyNodeType.StatickyCiselnik)
                        {
                        }
                    }
                
                    #endregion
                },
                new HierarchyNode("ors", "Organizačná štruktúra")
                {
                    #region Organizačná štruktúra
                        
                    Children = new List<HierarchyNode>
                    {
                        new HierarchyNode<StrediskoView>("orj", "Strediská", null, HierarchyNodeType.Ciselnik)
                        {
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.Change, roles_writer),
                                new NodeAction(NodeActionType.Delete, typeof(DeleteStredisko)) {SelectionMode = PfeSelection.Multi },
                                new NodeAction(NodeActionType.Update, typeof(UpdateStredisko))
                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateStredisko), roles_writer),
                        },
                        new HierarchyNode<ProjektView>("prj", "Projekty", null, HierarchyNodeType.Ciselnik)
                        {
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.Change, roles_writer),
                                new NodeAction(NodeActionType.Delete, typeof(DeleteProjekt)) {SelectionMode = PfeSelection.Multi },
                                new NodeAction(NodeActionType.Update, typeof(UpdateProjekt))
                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateProjekt), roles_writer),
                        }
                    }
                
                    #endregion
                },
                new HierarchyNode("log", "Logovanie",null,null, HierarchyNodeType.Ciselnik)
                {
                    #region Logovanie
                            
                    Children = new List<HierarchyNode>
                    {
                        new HierarchyNode<LoggingConfig>("nlze", "Nastavenie logovania zmien entít", null, HierarchyNodeType.Ciselnik)
                        {
                            SelectionMode = PfeSelection.Multi,
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.Change, roles_writer),
                                new NodeAction(NodeActionType.Delete, typeof(DeleteLoggingConfig)),
                                new NodeAction(NodeActionType.Update, typeof(UpdateLoggingConfig))
                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateLoggingConfig), roles_writer)
                        }
                    }

                    #endregion
                },
                new HierarchyNode("ssp", "Úkony",null,null, HierarchyNodeType.Ciselnik)
                {
                    #region Úkony
                            
                    Children = new List<HierarchyNode>
                    {
                        new HierarchyNode<StavovyPriestorView>("kbss", "Číselník oblastí", null, HierarchyNodeType.Ciselnik)
                        {
                            SelectionMode = PfeSelection.Multi,
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.Change, roles_writer),
                                new NodeAction(NodeActionType.Delete, typeof(DeleteStavovyPriestor), roles_writer),
                                new NodeAction(NodeActionType.Update, typeof(UpdateStavovyPriestor), roles_writer)
                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateStavovyPriestor), roles_writer),
                            Children = new List<HierarchyNode>
                            {
                                new HierarchyNode<StavEntityView>("ciss", "Číselník stavov", null, HierarchyNodeType.Ciselnik)
                                {
                                    SelectionMode = PfeSelection.Multi,
                                    Actions = new List<NodeAction>
                                    {
                                        new NodeAction(NodeActionType.Change, roles_writer),
                                        new NodeAction(NodeActionType.Delete, typeof(DeleteStavEntity), roles_writer),
                                        new NodeAction(NodeActionType.Update, typeof(UpdateStavEntity), roles_writer)
                                    }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateStavEntity), roles_writer),
                                    LayoutDependencies = new List<LayoutDependency>
                                    {
                                        LayoutDependency.OneToMany("reg-ssp-dtr-txt", "C_StavEntity_Id", "Použitie sád textov")
                                    }
                                },
                                new HierarchyNode<StavEntityStavEntityView>("dfs", "Definovanie úkonov", null, HierarchyNodeType.Ciselnik)
                                {
                                    SelectionMode = PfeSelection.Multi,
                                    Actions = new List<NodeAction>
                                    {
                                        new NodeAction(NodeActionType.Change, roles_writer),
                                        new NodeAction(NodeActionType.Delete, typeof(DeleteStavEntityStavEntity)),
                                        new NodeAction(NodeActionType.Update, typeof(UpdateStavEntityStavEntity))
                                    }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateStavEntityStavEntity), roles_writer)
                                }
                            },
                            LayoutDependencies = new List<LayoutDependency>
                            {
                                LayoutDependency.OneToMany("reg-ssp-kbss-ciss", "C_StavovyPriestor_Id", "Číselník stavov"),
                                LayoutDependency.OneToMany("reg-ssp-tbe", "C_StavovyPriestor_Id", "Biznis entity"),
                            }
                        },
                        new HierarchyNode<TypBiznisEntityView>("tbe","Prepojenie oblastí s biznis entitami", null, HierarchyNodeType.Ciselnik)
                        {
                        }
                    }
                
                    #endregion
                }
            },
            Roles = new List<string>()
            {
                WebEas.ServiceModel.Office.Egov.Reg.Roles.RegMember
            }
        };

        public const string Code = "reg";
    }
}
