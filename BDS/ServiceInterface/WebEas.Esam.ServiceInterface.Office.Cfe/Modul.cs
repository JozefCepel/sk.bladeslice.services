using System.Collections.Generic;
using System.Linq;
using WebEas.Esam.ServiceModel.Office.Cfe.Dto;
using WebEas.Esam.ServiceModel.Office.Cfe.Types;
using WebEas.ServiceInterface;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Types;
using RolesDefinition = WebEas.Esam.ServiceModel.Office.RolesDefinition;

namespace WebEas.Esam.ServiceInterface.Office.Cfe
{
    public static class Modul
    {
        public const string Code = "cfe";

        #region Roly uzivatelov

        static readonly string[] roles_member = new string[] { RolesDefinition.Cfe.Roles.CfeMember };
        static readonly string[] roles_writer = new string[] { RolesDefinition.Cfe.Roles.CfeWriter };
        static readonly string[] roles_ciswriter = new string[] { RolesDefinition.Cfe.Roles.CfeCisWriter };
        static readonly string[] roles_admin = new string[] { RolesDefinition.Cfe.Roles.CfeAdmin };
        static readonly string[] roles_admin_ciswriter = new string[] { RolesDefinition.Cfe.Roles.CfeAdmin, RolesDefinition.Cfe.Roles.CfeCisWriter };

        #endregion

        public readonly static HierarchyNode HierarchyTree = new HierarchyNode(Code, "Administrácia")
        {
            Children = new List<HierarchyNode>
            {
                new HierarchyNode("admin", "Administrácia práv")
                {
                    Children = new List<HierarchyNode>
                    {

                        new HierarchyNode<UserView>("users", "Používatelia", null, HierarchyNodeType.DatovaPolozka)
                        {
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.Change, roles_member),
                                new NodeAction(NodeActionType.Delete, typeof(DeleteUser)) {SelectionMode = PfeSelection.Multi },
                                new NodeAction(NodeActionType.Update, typeof(UpdateUser))
                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateUser), roles_member),

                            Children = new List<HierarchyNode>
                            {
                                new HierarchyNode<RoleUsersView>("roles", "Priradenie do rolí", null, HierarchyNodeType.Ciselnik)
                                {
                                    SelectionMode = PfeSelection.Multi,
                                    Actions = new List<NodeAction>
                                    {
                                        new NodeAction(NodeActionType.Change, roles_member),
                                        new NodeAction(NodeActionType.Delete, typeof(DeleteRoleUsers)) {SelectionMode = PfeSelection.Multi },
                                        new NodeAction(NodeActionType.Update, typeof(UpdateRoleUsers))
                                    }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateRoleUsers), roles_member),
                                },
                                new DatabaseHierarchyNode<WebEas.Esam.ServiceModel.Office.Cfe.Types.Modul>("modul", "Modul", (DatabaseHierarchyNode node) => { return Modules.Load<ICfeRepository>().RenderPouzivateliaModuly(node) ; }, null, HierarchyNodeType.Priecinok)
                                {
                                    Actions = new List<NodeAction>
                                    {
                                        //new NodeAction(NodeActionType.Change, roles_member),
                                        //new NodeAction(NodeActionType.Delete, typeof(DeleteRole)) {SelectionMode = PfeSelection.Multi },
                                        //new NodeAction(NodeActionType.Update, typeof(UpdateRole))
                                    },
                                    Children = new List<HierarchyNode>
                                    {
                                        new HierarchyNode<RightPermissionView>("rights", "Pridelenie základných práv", null, HierarchyNodeType.Ciselnik)
                                        {
                                            SelectionMode = PfeSelection.Multi,
                                            Actions = new List<NodeAction>
                                            {
                                                new NodeAction(NodeActionType.Change, roles_member),
                                                new NodeAction(NodeActionType.Delete, typeof(DeleteRightPermission)) {SelectionMode = PfeSelection.Multi },
                                                new NodeAction(NodeActionType.Update, typeof(UpdateRightPermission))
                                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateRightPermission), roles_member),
                                        },
                                        new HierarchyNode<RightPermissionView>("sum", "Sumár základných práv", null, HierarchyNodeType.Ciselnik)
                                        {
                                            SelectionMode = PfeSelection.Multi,
                                            Actions = new List<NodeAction>
                                            {
                                                new NodeAction(NodeActionType.Change, roles_member),
                                                new NodeAction(NodeActionType.Delete, typeof(DeleteRightPermission)) {SelectionMode = PfeSelection.Multi },
                                                new NodeAction(NodeActionType.Update, typeof(UpdateRightPermission))
                                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateRightPermission), roles_member),
                                        },
                                        new HierarchyNode<RightPermissionView>("hrch", "Práva na hierarchiu modulu", null, HierarchyNodeType.Ciselnik)
                                        {
                                            SelectionMode = PfeSelection.Multi,
                                            Actions = new List<NodeAction>
                                            {
                                                new NodeAction(NodeActionType.Change, roles_member),
                                                new NodeAction(NodeActionType.Delete, typeof(DeleteRightPermission)) {SelectionMode = PfeSelection.Multi },
                                                new NodeAction(NodeActionType.Update, typeof(UpdateRightPermission))
                                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateRightPermission), roles_member),
                                        },
                                        new HierarchyNode<RightPermissionView>("ors", "Organizačná štruktúra", null, HierarchyNodeType.Priecinok)
                                        {
                                            SelectionMode = PfeSelection.Multi,
                                            Actions = new List<NodeAction>
                                            {
                                                new NodeAction(NodeActionType.Change, roles_member),
                                                new NodeAction(NodeActionType.Delete, typeof(DeleteRightPermission)) {SelectionMode = PfeSelection.Multi },
                                                new NodeAction(NodeActionType.Update, typeof(UpdateRightPermission))
                                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateRightPermission), roles_member),
                                            Children= new List<HierarchyNode>
                                            {
                                                new HierarchyNode<RightPermissionView>("ors", "Organizačná štruktúra", null, HierarchyNodeType.Priecinok)
                                                {
                                                    SelectionMode = PfeSelection.Multi,
                                                    Actions = new List<NodeAction>
                                                    {
                                                        new NodeAction(NodeActionType.Change, roles_member),
                                                        new NodeAction(NodeActionType.Delete, typeof(DeleteRightPermission)) {SelectionMode = PfeSelection.Multi },
                                                        new NodeAction(NodeActionType.Update, typeof(UpdateRightPermission))
                                                    }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateRightPermission), roles_member),
                                                }
                                            }
                                        }
                                    }
                                }
                                // Používateľ [1,2,3] folder
                                //   Modul[1,2,3] 
                                //      Pridelenie základných práv
                                //      Sumár základných práv
                                //      Práva na hierarchiu modulu
                                //      Organizačná štruktúra
                                //          Prvky organizačnej štruktúry[1,2,3] /C_OrsElement/-
                            },
                            //Actions = new List<NodeAction>
                            //{
                            //    new NodeAction(NodeActionType.Change, roles_member),
                            //    new NodeAction(NodeActionType.Delete, typeof(DeleteRzp)) {SelectionMode = PfeSelection.Multi },
                            //    new NodeAction(NodeActionType.Update, typeof(UpdateRzp)),
                            //    new NodeAction(NodeActionType.ZmenaStavuPodania, typeof(ChangeStateDto), roles_writer) { Caption = "Spracovať" },
                            //    new NodeAction(NodeActionType.PrevziatNavrhRozpoctu, roles_writer)
                            //}.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateRzp), roles_member),
                            //LayoutDependencies = new List<LayoutDependency>
                            //{
                            //    LayoutDependency.OneToMany("rzp-evi-navrh-pol", "D_Rzp_Id", "Položky návrhov"),
                            //    LayoutDependency.OneToMany("rzp-sm-hzs", "D_Rzp_Id", "História zmien stavov")
                            //}
                        },
                        new HierarchyNode<RoleView>("role", "Role", null, HierarchyNodeType.DatovaPolozka)
                        {
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.Change, roles_member),
                                new NodeAction(NodeActionType.Delete, typeof(DeleteRole)) {SelectionMode = PfeSelection.Multi },
                                new NodeAction(NodeActionType.Update, typeof(UpdateRole))
                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateRole), roles_member),

                            // Rola [1,2,3] (folder)

                        }
                    }
                },
            new HierarchyNode("def", "Definície")
            {
                Children = new List<HierarchyNode>
                    {
                        // D_Tenant/ (SysAdmin všetko, ostatní uvidia iba svojho tenanta)
                        new HierarchyNode<TenantView>("ten", "Zoznam tenantov", null, HierarchyNodeType.DatovaPolozka)
                        {
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.Change, roles_member),
                                new NodeAction(NodeActionType.Update, typeof(UpdateTenant)) // len Update! create/delete cez script
                            },
                            Children = new List<HierarchyNode>
                            {
                                // read ikonka
                                new HierarchyNode<TenantUsersView>("tenusers", "Priradenie používateľov", null, HierarchyNodeType.Ciselnik)
                                {
                                    SelectionMode = PfeSelection.Multi,
                                    Actions = new List<NodeAction>
                                    {
                                        new NodeAction(NodeActionType.Change, roles_member),
                                        new NodeAction(NodeActionType.Delete, typeof(DeleteTenantUsers)) {SelectionMode = PfeSelection.Multi },
                                        new NodeAction(NodeActionType.Update, typeof(UpdateTenantUsers))
                                    }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateTenantUsers), roles_member),
                                },
                                // Používateľ [1,2,3] folder
                            },
                            //Actions = new List<NodeAction>
                            //{
                            //    new NodeAction(NodeActionType.Change, roles_member),
                            //    new NodeAction(NodeActionType.Delete, typeof(DeleteRzp)) {SelectionMode = PfeSelection.Multi },
                            //    new NodeAction(NodeActionType.Update, typeof(UpdateRzp)),
                            //    new NodeAction(NodeActionType.ZmenaStavuPodania, typeof(ChangeStateDto), roles_writer) { Caption = "Spracovať" },
                            //    new NodeAction(NodeActionType.PrevziatNavrhRozpoctu, roles_writer)
                            //}.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateRzp), roles_member),
                            //LayoutDependencies = new List<LayoutDependency>
                            //{
                            //    LayoutDependency.OneToMany("rzp-evi-navrh-pol", "D_Rzp_Id", "Položky návrhov"),
                            //    LayoutDependency.OneToMany("rzp-sm-hzs", "D_Rzp_Id", "História zmien stavov")
                            //}
                        },
                        // (Read ikonka - importované skriptom) /C_Modul
                        new HierarchyNode<ModulView>("role", "Moduly", null, HierarchyNodeType.DatovaPolozka)
                        {
                            Children = new List<HierarchyNode>
                            {
                                new HierarchyNode<RightView>("prava", "Základné práva", null, HierarchyNodeType.Ciselnik)
                                {
                                    SelectionMode = PfeSelection.Multi,
                                    Actions = new List<NodeAction>
                                    {
                                        new NodeAction(NodeActionType.Change, roles_member),
                                        new NodeAction(NodeActionType.Delete, typeof(DeleteRight)) {SelectionMode = PfeSelection.Multi },
                                        new NodeAction(NodeActionType.Update, typeof(UpdateRight))
                                    }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateRight), roles_member),
                                },
                                new HierarchyNode<TreeView>("strom", "Stromová štrukúra", null, HierarchyNodeType.Ciselnik)
                                {
                                    SelectionMode = PfeSelection.Multi,
                                    Actions = new List<NodeAction>
                                    {
                                        new NodeAction(NodeActionType.Change, roles_member),
                                        new NodeAction(NodeActionType.Delete, typeof(DeleteTenantUsers)) {SelectionMode = PfeSelection.Multi },
                                        new NodeAction(NodeActionType.Update, typeof(UpdateTenantUsers))
                                    }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateTenantUsers), roles_member),
                                },
                                new HierarchyNode<OrsElementTypeView>("ors", "Typy prvkov organizačnej štruktúry", null, HierarchyNodeType.StatickyCiselnik)
                                {
                                    SelectionMode = PfeSelection.Multi,
                                    Actions = new List<NodeAction>
                                    {
                                        new NodeAction(NodeActionType.Change, roles_member),
                                        new NodeAction(NodeActionType.Delete, typeof(DeleteTenantUsers)) {SelectionMode = PfeSelection.Multi },
                                        new NodeAction(NodeActionType.Update, typeof(UpdateTenantUsers))
                                    }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateTenantUsers), roles_member),
                                },
                            //   Modul[1,2,3]Základné práva
                            }
                        },
                        new HierarchyNode<RoleView>("cis", "Číselníky", null, HierarchyNodeType.Priecinok)
                        {
                            // Typ tenanta /C_TenantType - zatial nie!

                        }
                    }
            },
        },
            Roles = roles_member.ToList()
        };

        #region LayoutDependencies
        //.SetLayoutDependencies(
        //    HierarchyNodeDependency.One2ManyBack2One("cfe-def-role-prava", "cfe-def-role", "C_Module_Id", "Ciele", "Programový rozpočet")
        //);
        #endregion

        #region Generovanie akcii (tlacitok)

        #endregion
    }
}
