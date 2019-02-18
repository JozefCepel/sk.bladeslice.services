using System;
using System.Collections.Generic;
using System.Linq;
using WebEas.Esam.ServiceModel.Office.Cfe.Dto;
using WebEas.Esam.ServiceModel.Office.Cfe.Types;
using WebEas.ServiceInterface;
using WebEas.ServiceModel;
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

                        new HierarchyNode<UserView>("users", "Používatelia", null, HierarchyNodeType.Ciselnik)
                        {
                            Children = new List<HierarchyNode>
                            {
                                new HierarchyNode<RoleUsersView>("roleusers", "Priradenie do rolí", null, HierarchyNodeType.Ciselnik)
                                {
                                    SelectionMode = PfeSelection.Multi,
                                    Actions = new List<NodeAction>
                                    {
                                        new NodeAction(NodeActionType.Change, roles_member),
                                        new NodeAction(NodeActionType.Delete, typeof(DeleteRoleUsers)) {SelectionMode = PfeSelection.Multi },
                                        new NodeAction(NodeActionType.Update, typeof(UpdateRoleUsers))
                                    }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateRoleUsers), roles_member),
                                },
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
                            //    new NodeAction(NodeActionType.Delete, typeof(DeleteNavrhZmenyRzp)) {SelectionMode = PfeSelection.Multi },
                            //    new NodeAction(NodeActionType.Update, typeof(UpdateNavrhZmenyRzp)),
                            //    new NodeAction(NodeActionType.ZmenaStavuPodania, typeof(ChangeStateDto), roles_writer) { Caption = "Spracovať" },
                            //    new NodeAction(NodeActionType.PrevziatNavrhRozpoctu, roles_writer)
                            //}.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateNavrhZmenyRzp), roles_member),
                            //LayoutDependencies = new List<LayoutDependency>
                            //{
                            //    LayoutDependency.OneToMany("rzp-evi-navrh-pol", "D_NavrhZmenyRzp_Id", "Položky návrhov"),
                            //    LayoutDependency.OneToMany("rzp-sm-hzs", "D_NavrhZmenyRzp_Id", "História zmien stavov")
                            //}
                        },
                        new HierarchyNode<RoleView>("role", "Role", null, HierarchyNodeType.DatovaPolozka)
                        {
                            // Rola [1,2,3] (folder)

                        }
                    }
                },
                new HierarchyNode("def", "Definície")
                {
                    Children = new List<HierarchyNode>
                    {
                        // D_Tenant/ (SysAdmin všetko, ostatní uvidia iba svojho tenanta)
                        new HierarchyNode<TenantView>("ten", "Zoznam tenantov", null, HierarchyNodeType.Ciselnik)
                        {
                            Children = new List<HierarchyNode>
                            {
                                // read ikonka
                                new HierarchyNode<TenantUsersView>("tenantusers", "Priradenie používateľov", null, HierarchyNodeType.StatickyCiselnik)
                                {
                                },
                                // Používateľ [1,2,3] folder
                            },
                            //Actions = new List<NodeAction>
                            //{
                            //    new NodeAction(NodeActionType.Change, roles_member),
                            //    new NodeAction(NodeActionType.Delete, typeof(DeleteNavrhZmenyRzp)) {SelectionMode = PfeSelection.Multi },
                            //    new NodeAction(NodeActionType.Update, typeof(UpdateNavrhZmenyRzp)),
                            //    new NodeAction(NodeActionType.ZmenaStavuPodania, typeof(ChangeStateDto), roles_writer) { Caption = "Spracovať" },
                            //    new NodeAction(NodeActionType.PrevziatNavrhRozpoctu, roles_writer)
                            //}.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateNavrhZmenyRzp), roles_member),
                            //LayoutDependencies = new List<LayoutDependency>
                            //{
                            //    LayoutDependency.OneToMany("rzp-evi-navrh-pol", "D_NavrhZmenyRzp_Id", "Položky návrhov"),
                            //    LayoutDependency.OneToMany("rzp-sm-hzs", "D_NavrhZmenyRzp_Id", "História zmien stavov")
                            //}
                        },
                        new HierarchyNode<DepartmentView>("odd", "Oddelenia", null, HierarchyNodeType.DatovaPolozka)
                        {
                            // D_Department
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.Change, roles_member),
                                new NodeAction(NodeActionType.Delete, typeof(DeleteDepartment)) {SelectionMode = PfeSelection.Multi },
                                new NodeAction(NodeActionType.Update, typeof(UpdateDepartment))
                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateDepartment), roles_member)
                        },
                        // (Read ikonka - importované skriptom) /C_Module
                        new HierarchyNode<RoleView>("role", "Moduly", null, HierarchyNodeType.StatickyCiselnik)
                        {
                            Children = new List<HierarchyNode>
                            {
                            //   Modul[1,2,3]
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

        #region Generovanie akcii (tlacitok)

        #endregion
    }
}
