using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using WebEas.Esam.ServiceModel.Office.Cfe.Dto;
using WebEas.Esam.ServiceModel.Office.Cfe.Types;
using WebEas.Esam.ServiceModel.Office.Types.Cfe;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Types;

namespace WebEas.Esam.ServiceInterface.Office.Cfe
{
    public partial class CfeRepository
    {
        public override string Code => "cfe";

        public override HierarchyNode RenderModuleRootNode(string kodPolozky)
        {
            var rootNode = new HierarchyNode(Code, "Administrácia")
            {
                Children = new List<HierarchyNode>
            {
                new HierarchyNode("admin", "Administrácia práv")
                {
                    #region Administrácia práv
                    Children = new List<HierarchyNode>
                    {
                        new HierarchyNode<UserView>("users", "Používatelia", null, icon : HierarchyNodeIconCls.Users)
                        {
                            #region Používatelia
                            SelectionMode = PfeSelection.Multi,
                            RowsCounterRule = -1,
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.Change),
                                new NodeAction(NodeActionType.CopyUserPermissions, typeof(CopyUserPermissions)),
                                new NodeAction(NodeActionType.BlockUser, typeof(BlockUser), "Zablokovať") {SelectionMode = PfeSelection.Multi },
                                new NodeAction(NodeActionType.ChangePassword, typeof(ChangePassword)),
                                new NodeAction(NodeActionType.Update, typeof(UpdateUser)),
                                new NodeAction(NodeActionType.Delete, typeof(DeleteUser)) {SelectionMode = PfeSelection.Multi }
                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateUser)),
                            DefaultValues = new List<NodeFieldDefaultValue>
                            {
                                    new NodeFieldDefaultValue(nameof(UserView.PlatnostOd), DateTime.Now)
                            },

                            Children = new List<HierarchyNode>
                            {
                                new HierarchyNode<UserRoleView>("roles", "Priradenie do rolí", null, icon : HierarchyNodeIconCls.UserTag)
                                {
                                    SelectionMode = PfeSelection.Multi,
                                    Actions = new List<NodeAction>
                                    {
                                        new NodeAction(NodeActionType.Change),
                                        new NodeAction(NodeActionType.Delete, typeof(DeleteUserRole)) {SelectionMode = PfeSelection.Multi },
                                        new NodeAction(NodeActionType.Update, typeof(UpdateUserRole))
                                    }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateUserRole)),
                                    DefaultValues = new List<NodeFieldDefaultValue>
                                    {
                                          new NodeFieldDefaultValue(nameof(UserRoleView.PlatnostOd), DateTime.Now)
                                    }
                                },
                                new DatabaseHierarchyNode<ModulUserView>("modul", "Modul", (DatabaseHierarchyNode node) => { return RenderPouzivateliaModuly(node) ; }, null, icon : HierarchyNodeIconCls.PuzzlePiece)
                                {
                                    #region Modul
                                    Children = new List<HierarchyNode>
                                    {
                                        new HierarchyNode<RightUserView>("rights", "Pridelenie základných práv", null, icon : HierarchyNodeIconCls.HandRight)
                                        {
                                            SelectionMode = PfeSelection.Multi,
                                            Actions = new List<NodeAction>
                                            {
                                                new NodeAction(NodeActionType.AddRight, typeof(AddRightPermission)){ SelectionMode = PfeSelection.Multi, IdField = "V_RightUser_Id"},
                                                new NodeAction(NodeActionType.RemoveRight, typeof(RemoveRightPermission)) {SelectionMode = PfeSelection.Multi, IdField = "D_RightPermission_Id" }
                                            },
                                        },
                                        //new HierarchyNode<DummyData>("sum", "Sumár základných práv *", null, icon : HierarchyNodeIconCls.StatickyCiselnik)
                                        //{
                                        //},
                                        new HierarchyNode<TreeUserView>("hrch", "Práva na stromovú štruktúru", null, icon : HierarchyNodeIconCls.HandRight)
                                        {
                                            Actions = new List<NodeAction>
                                            {
                                                //new NodeAction(NodeActionType.Change),
                                                //new NodeAction(NodeActionType.Update, typeof(UpdateTreePermission))
                                                new NodeAction(NodeActionType.SetRightNo, typeof(TreeSetRightNo)){SelectionMode = PfeSelection.Multi, IdField = "V_TreeUser_Id" },
                                                new NodeAction(NodeActionType.SetRightRead, typeof(TreeSetRightRead)){SelectionMode = PfeSelection.Multi, IdField = "V_TreeUser_Id" },
                                                new NodeAction(NodeActionType.SetRightUpdate, typeof(TreeSetRightUpdate)){SelectionMode = PfeSelection.Multi, IdField = "V_TreeUser_Id" },
                                                new NodeAction(NodeActionType.SetRightFull, typeof(TreeSetRightFull)){SelectionMode = PfeSelection.Multi, IdField = "V_TreeUser_Id" }
                                            },
                                        },
                                        new HierarchyNode<OrsElementTypeUsersView>("tors", "Typy prvkov ORŠ", null, icon : HierarchyNodeIconCls.Sitemap)
                                        {
                                            SelectionMode = PfeSelection.Multi,
                                            Actions = new List<NodeAction>
                                            {
                                                new NodeAction(NodeActionType.SetRightNo, typeof(OETSetRightNo)){SelectionMode = PfeSelection.Multi, IdField = "V_OrsElementTypeUser_Id" },
                                                new NodeAction(NodeActionType.SetRightRead, typeof(OETSetRightRead)){SelectionMode = PfeSelection.Multi, IdField = "V_OrsElementTypeUser_Id" },
                                                new NodeAction(NodeActionType.SetRightUpdate, typeof(OETSetRightUpdate)){SelectionMode = PfeSelection.Multi, IdField = "V_OrsElementTypeUser_Id" },
                                                new NodeAction(NodeActionType.SetRightFull, typeof(OETSetRightFull)){SelectionMode = PfeSelection.Multi, IdField = "V_OrsElementTypeUser_Id" }
                                            },
                                            Children = new List<HierarchyNode>
                                            {
                                                new HierarchyNode<OrsElementUserView>("ors", "Pridelenie práv na prvky ORŠ", null, icon : HierarchyNodeIconCls.HandRight)
                                                {
                                                    SelectionMode = PfeSelection.Multi,
                                                    Actions = new List<NodeAction>
                                                    {
                                                        new NodeAction(NodeActionType.SetRightNo, typeof(OESetRightNo)){SelectionMode = PfeSelection.Multi, IdField = "V_OrsElementUser_Id" },
                                                        new NodeAction(NodeActionType.SetRightRead, typeof(OESetRightRead)){SelectionMode = PfeSelection.Multi, IdField = "V_OrsElementUser_Id" },
                                                        new NodeAction(NodeActionType.SetRightUpdate, typeof(OESetRightUpdate)){SelectionMode = PfeSelection.Multi, IdField = "V_OrsElementUser_Id" },
                                                        new NodeAction(NodeActionType.SetRightFull, typeof(OESetRightFull)){SelectionMode = PfeSelection.Multi, IdField = "V_OrsElementUser_Id" }

                                                    }
                                                }
                                            }
                                        },
                                    },
                                    Actions = new List<NodeAction>
                                    {
                                        new NodeAction(NodeActionType.CopyUserPermissions, typeof(CopyUserPermissions))
                                    }
                                    #endregion
                                }
                            }, 
                            #endregion
                        },
                        new HierarchyNode<RoleView>("role", "Role", null, icon : HierarchyNodeIconCls.Tags)
                        {
                            #region Role
                            SelectionMode = PfeSelection.Multi,
                            RowsCounterRule = -1,
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.Change),
                                new NodeAction(NodeActionType.Delete, typeof(DeleteRole)) {SelectionMode = PfeSelection.Multi },
                                new NodeAction(NodeActionType.Update, typeof(UpdateRole))
                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateRole)),

                            // Rola [1,2,3] (folder)

                            #endregion
                        }
                    } 
                    #endregion
                },
            new HierarchyNode("def", "Definície")
            {
                #region Definície
                Children = new List<HierarchyNode>
                {
                    // D_Tenant/ (SysAdmin všetko, ostatní uvidia iba svojho tenanta)
                    new HierarchyNode<TenantView>("ten", "Moja obec", null , icon : HierarchyNodeIconCls.Building)
                    {
                        #region Zoznam tenantov
                        Actions = new List<NodeAction>
                        {
                            new NodeAction(NodeActionType.Change),
                            new NodeAction(NodeActionType.Update, typeof(UpdateTenant)),
                            new NodeAction(NodeActionType.Delete, typeof(DeleteTenant) ) ,
                        }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateTenant), "Založenie novej organizácie"),
                        Children = new List<HierarchyNode>
                        {
                            // read ikonka
                            new HierarchyNode<UserTenantView>("users", "Priradenie používateľov", null, icon : HierarchyNodeIconCls.HandRight)
                            {
                                SelectionMode = PfeSelection.Multi,
                                Actions = new List<NodeAction>
                                {
                                    new NodeAction(NodeActionType.Change),
                                    new NodeAction(NodeActionType.Delete, typeof(DeleteUserTenant)) {SelectionMode = PfeSelection.Multi },
                                    new NodeAction(NodeActionType.Update, typeof(UpdateUserTenant))
                                }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateUserTenant)),
                            },
                        }, 
                        #endregion
                    },
                    // (Read ikonka - importované skriptom) /C_Modul
                    new HierarchyNode<ModulView>("mod", "Moduly", null, icon : HierarchyNodeIconCls.PuzzlePiece)
                    {
                        #region Moduly
                        SelectionMode = PfeSelection.Multi,

                        Actions = new List<NodeAction>
                        {
                            new NodeAction(NodeActionType.RefreshModuleTree, typeof(RefreshModuleTree)){SelectionMode = PfeSelection.Multi, IdField = "C_Modul_Id" }
                        },
                        Children = new List<HierarchyNode>
                        {
                            new HierarchyNode<RightView>("prava", "Základné práva", null, icon : HierarchyNodeIconCls.Key)
                            {
                            },
                            new HierarchyNode<TreeView>("strom", "Stromová štruktúra", null, icon : HierarchyNodeIconCls.Sitemap)
                            {
                            },
                            new HierarchyNode<OrsElementTypeView>("tors", "Typy prvkov organizačnej štruktúry", null, icon : HierarchyNodeIconCls.Delicious)
                            {
                                SelectionMode = PfeSelection.Multi,
                                Actions = new List<NodeAction>
                                {
                                    new NodeAction(NodeActionType.ObnovitZoznamORS, typeof(ObnovitZoznamORS),"Obnoviť zoznam elementov ORŠ") {SelectionMode = PfeSelection.Multi, IdField = "C_OrsElementType_Id"},
                                },
                                Children = new List<HierarchyNode>
                                {
                                    new HierarchyNode<OrsElementView>("ors", "Prvky organizačnej štruktúry", null, icon : HierarchyNodeIconCls.ThLarge)
                                    {
                                    }
                                }
                            },
                        } 
                        #endregion
                    }
                } 
                #endregion
            },
                GenerateNodeSpravaModulu(Code, typeof(UpdateNastavenie))
        }
            }

            #region LayoutDependencies
        .SetLayoutDependencies(
            HierarchyNodeDependency.One2ManyBack2One("cfe-admin-users", "cfe-def-ten-users", "D_User_Id", "V obciach", "Používateľ"),
            HierarchyNodeDependency.One2ManyBack2One("cfe-admin-users", "cfe-admin-users-roles", "D_User_Id", "Role", "Používateľ"),
            HierarchyNodeDependency.One2ManyBack2One("cfe-def-ten", "cfe-def-ten-users", "D_Tenant_Id", "Používateľ", "Obec"),
            HierarchyNodeDependency.One2ManyBack2One("cfe-def-mod", "cfe-def-mod-prava", "C_Modul_Id", "Základné práva", "Modul"),
            HierarchyNodeDependency.One2ManyBack2One("cfe-def-mod", "cfe-def-mod-strom", "C_Modul_Id", "Stromová štrukúra", "Modul"),
            HierarchyNodeDependency.One2ManyBack2One("cfe-def-mod", "cfe-def-mod-tors", "C_Modul_Id", "Typy prvkov ORŠ", "Modul"),
            HierarchyNodeDependency.One2ManyBack2One("cfe-def-mod", "cfe-def-mod-tors", "C_Modul_Id", "Typy prvkov ORŠ", "Modul"),
            HierarchyNodeDependency.One2ManyBack2One("cfe-def-mod-tors", "cfe-def-mod-tors-ors", "C_OrsElementType_Id", "Prvky ORŠ", "Typy ORŠ"),
            HierarchyNodeDependency.One2ManyBack2One("cfe-admin-users-modul-rights", "cfe-admin-users-modul", "D_User_Id", "Základné práva", "Práva"),
            HierarchyNodeDependency.One2ManyBack2One("cfe-admin-users-modul-hrch", "cfe-admin-users-modul", "D_User_Id", "Základné práva", "Hierarchia"),
            HierarchyNodeDependency.One2ManyBack2One("cfe-admin-users-modul-tors", "cfe-admin-users-modul", "D_User_Id", "Základné práva", "Prvky ORŠ"),
            HierarchyNodeDependency.One2ManyBack2One("cfe-admin-users-modul-tors-ors", "cfe-admin-users-modul-tors", "V_OrsElementTypeUser_Id", "Prvky ORŠ", "Typy ORŠ")
            );
            #endregion

            #region Generovanie akcii (tlacitok)

            var polozkyCustomRezim = new List<string>
            {
                "cfe-admin-users"
            };

            if (polozkyCustomRezim.Contains(kodPolozky) || kodPolozky == Code)
            {
                var dcomRezim = GetNastavenieI("reg", "eSAMRezim") == 1;
                if (dcomRezim)
                {
                    foreach (var node in rootNode.Children.SelectMany(x => x.Children).Where(x => polozkyCustomRezim.Contains(x.KodPolozky)))
                    {
                        node.Actions.AddMenuButtonsAll(NodeActionType.SynchronizeDcomUsers, typeof(SynchronizeDcomUsersDto), folderCaption: "DCOM", actionIcon: NodeActionIcons.Refresh);
                    }
                        
                }
            }

            #endregion

            return rootNode;
        }

        public List<DatabaseHierarchyNode> RenderPouzivateliaModuly(DatabaseHierarchyNode staticData)
        {
            var result = new List<DatabaseHierarchyNode>();
            foreach (var modul in EsamModules)
            {
                var node = staticData.Clone();
                node.Parameter = modul.C_Modul_Id;
                node.Nazov = $" {modul.Kod} - {modul.Nazov}";

                result.Add(node);
            }

            return result;
        }

        #region ORS Hierarchy Node
        private List<OrsElementType> GetElementTypesHierarchyNode(int IdModul)
        {
            return GetCacheOptimized($"cfe:ORSElementTypes", () =>
            {
                return GetList<OrsElementType>();
            }, new TimeSpan(24, 0, 0)).Where(orst => orst.C_Modul_Id == IdModul).ToList();
        }

        public List<DatabaseHierarchyNode> RenderORSElementsModuly(DatabaseHierarchyNode staticData)
        {
            //List<int> ORSet = GetElementTypesHierarchyNode(IdModul).Select(o => o.C_OrsElementType_Id).ToList();
            //List<DatabaseHierarchyNode> result = GetModulyHierarchyNode(staticData, (int)staticData.Parent.Parent.Parent.Parameter);
            List<OrsElement> elementyCache = GetCacheOptimized($"cfe:ORSElements", () =>
            {
                return GetList<OrsElement>();
            }, new TimeSpan(24, 0, 0));

            var result = new List<DatabaseHierarchyNode>();
            foreach (var element in elementyCache.Where(e => e.C_OrsElementType_Id == (int)staticData.Parent.Parameter))
            {
                var node = staticData.Clone();
                node.Parameter = element.C_OrsElement_Id;
                node.Nazov = element.ListValue;
                result.Add(node);
            }

            return result;
        }

        public List<DatabaseHierarchyNode> RenderORSElementTypesModuly(DatabaseHierarchyNode staticData)
        {
            var elementyCache = GetElementTypesHierarchyNode((int)staticData.Parent.Parent.Parameter);

            var result = new List<DatabaseHierarchyNode>();
            foreach (var element in elementyCache)
            {
                var node = staticData.Clone();
                node.Parameter = element.C_OrsElementType_Id;
                node.Nazov = element.DbView;
                result.Add(node);
            }

            return result;
        }

        public List<DatabaseHierarchyNode> RenderORSElementsModulyRoot(DatabaseHierarchyNode staticData)
        {
            List<DatabaseHierarchyNode> result = new List<DatabaseHierarchyNode>();

            if (GetElementTypesHierarchyNode((int)staticData.Parent.Parameter).Any())
            {
                var node = staticData.Clone();
                //node.Parameter = element.C_OrsElement_Id;
                node.Nazov = "Organizačná štruktúra";
                result.Add(node);
            }

            return result;
        }
        #endregion
    }
}
