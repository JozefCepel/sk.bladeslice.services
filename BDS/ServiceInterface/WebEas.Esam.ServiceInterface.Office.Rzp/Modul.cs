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
        public const string Code = "rzp";

        #region Roly uzivatelov

        static readonly string[] roles_member = new string[] { RolesDefinition.Bds.Roles.BdsMember };
        static readonly string[] roles_writer = new string[] { RolesDefinition.Bds.Roles.BdsWriter };
        static readonly string[] roles_ciswriter = new string[] { RolesDefinition.Bds.Roles.BdsCisWriter };
        static readonly string[] roles_admin = new string[] { RolesDefinition.Bds.Roles.BdsAdmin };
        static readonly string[] roles_admin_ciswriter = new string[] { RolesDefinition.Bds.Roles.BdsAdmin, RolesDefinition.Bds.Roles.BdsCisWriter };

        #endregion

        public readonly static HierarchyNode HierarchyTree = new HierarchyNode(Code, "Rozpočet")
        {
            Children = new List<HierarchyNode>
            {
                new HierarchyNode("evi","Evidence")
                {
                    #region EVIDENCIA
                    
                    Children = new List<HierarchyNode>
                    {
                    }
                    
                    #endregion
                },
                new HierarchyNode("def", "Definitions")
                {
                    #region Definície
                    
                    Children = new List<HierarchyNode>
                    {
                        new HierarchyNode<CieleView>("ciel", "Ciele", null, HierarchyNodeType.Ciselnik)
                        {
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.Change, roles_member),
                                new NodeAction(NodeActionType.Delete, typeof(DeleteCiele)) {SelectionMode = PfeSelection.Multi },
                                new NodeAction(NodeActionType.Update, typeof(UpdateCiele))
                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateCiele), roles_member),
                            Children = new List<HierarchyNode>
                            {
                                new HierarchyNode<CieleUkazView>("ukazovatel", "Ukazovatele", null, HierarchyNodeType.Ciselnik)
                                {
                                    Actions = new List<NodeAction>
                                    {
                                        new NodeAction(NodeActionType.Change, roles_member),
                                        new NodeAction(NodeActionType.Delete, typeof(DeleteCieleUkaz)) {SelectionMode = PfeSelection.Multi },
                                        new NodeAction(NodeActionType.Update, typeof(UpdateCieleUkaz))
                                    }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateCieleUkaz), roles_member),
                                }
                            },
                            LayoutDependencies = new List<LayoutDependency>
                            {
                                LayoutDependency.OneToMany("rzp-def-ciel-ukazovatel", "D_PRCiele_Id", "Ukazovatele")
                            }
                        }
                    }
                    
                    #endregion
                },
                new HierarchyNode("cis", "Číselníky")
                {
                    #region Číselníky
                    
                    Children = new List<HierarchyNode>
                    {
                        new HierarchyNode<FRZdrojView>("zdr", "Kódy zdrojov", null, HierarchyNodeType.StatickyCiselnik)
                        {
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
                        /*
                        new HierarchyNode<BdsDummyData>("log", "Logovanie zmien *", null, HierarchyNodeType.Ciselnik)
                        {
                            #region Logovanie zmien
                            
                            #endregion
                        },
                        */
                        new HierarchyNode<EntitaHistoriaStavovView>("hzs", "História zmien stavov", null, HierarchyNodeType.Ciselnik)
                        {
                            #region História zmien stavov
                            
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
            HierarchyNodeDependency.One2ManyBack2One("rzp-def-prr", "rzp-def-ciel", "D_Program_Id", "Ciele", "Programový rozpočet"),
            HierarchyNodeDependency.One2ManyBack2One("rzp-def-prs", "rzp-def-ciel", "D_Program_Id", "Ciele", "Programový rozpočet - sumárne"),
            HierarchyNodeDependency.One2ManyBack2One("rzp-cis-zdr", "rzp-def-prij", "C_FRZdroj_Id", "Príjmové rzp. položky", "Zdroj"),
            HierarchyNodeDependency.One2ManyBack2One("rzp-cis-zdr", "rzp-def-vyd", "C_FRZdroj_Id", "Výdajové rzp. položky", "Zdroj"),
            HierarchyNodeDependency.One2ManyBack2One("rzp-cis-fk", "rzp-def-vyd", "C_FRFK_Id", "Výdajové rzp. položky", "Funkčná kl."),
            HierarchyNodeDependency.One2ManyBack2One("rzp-cis-ek", "rzp-def-prij", "C_FREK_Id", "Príjmové rzp. položky", "Ekonomická kl."),
            HierarchyNodeDependency.One2ManyBack2One("rzp-cis-ek", "rzp-def-vyd", "C_FREK_Id", "Výdajové rzp. položky", "Ekonomická kl.")
        );
        #endregion

        #region Generovanie akcii (tlacitok)

        #endregion
    }
}
