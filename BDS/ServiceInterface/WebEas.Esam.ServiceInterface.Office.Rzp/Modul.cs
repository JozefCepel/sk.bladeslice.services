using System;
using System.Collections.Generic;
using System.Linq;
using WebEas.Esam.ServiceModel.Office.Rzp.Dto;
using WebEas.Esam.ServiceModel.Office.Rzp.Types;
using WebEas.ServiceInterface;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Office.Egov.Reg.Types;
using RolesDefinition = WebEas.Esam.ServiceModel.Office.RolesDefinition;

namespace WebEas.Esam.ServiceInterface.Office.Rzp
{
    public static class Modul
    {
        public const string Code = "rzp";

        #region Roly uzivatelov

        static readonly string[] roles_member = new string[] { RolesDefinition.Rzp.Roles.RzpMember };
        static readonly string[] roles_writer = new string[] { RolesDefinition.Rzp.Roles.RzpWriter };
        static readonly string[] roles_ciswriter = new string[] { RolesDefinition.Rzp.Roles.RzpCisWriter };
        static readonly string[] roles_admin = new string[] { RolesDefinition.Rzp.Roles.RzpAdmin };
        static readonly string[] roles_admin_ciswriter = new string[] { RolesDefinition.Rzp.Roles.RzpAdmin, RolesDefinition.Rzp.Roles.RzpCisWriter };

        #endregion

        public readonly static HierarchyNode HierarchyTree = new HierarchyNode(Code, "Rozpočet")
        {
            Children = new List<HierarchyNode>
            {
                new HierarchyNode("evi","Evidencia")
                {
                    #region EVIDENCIA
                    
                    Children = new List<HierarchyNode>
                    {
                        new HierarchyNode<NavrhRzpView>("navrh", "Návrhy rozpočtu", f => f.Typ == false, HierarchyNodeType.Ciselnik)
                        {
                            Children = new List<HierarchyNode>
                            {
                                new HierarchyNode<NavrhyRzpValView>("pol", "Položky návrhov", f => f.Typ == false, HierarchyNodeType.Ciselnik)
                                {
                                    SelectionMode = PfeSelection.Multi,
                                    Actions = new List<NodeAction>
                                    {
                                        new NodeAction(NodeActionType.Change, roles_member),
                                        new NodeAction(NodeActionType.Delete, typeof(DeleteNavrhyRzpVal)) {SelectionMode = PfeSelection.Multi },
                                        new NodeAction(NodeActionType.Update, typeof(UpdateNavrhyRzpVal))
                                    }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateNavrhyRzpVal), roles_member),
                                },
                            },
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.Change, roles_member),
                                new NodeAction(NodeActionType.Delete, typeof(DeleteNavrhZmenyRzp)) {SelectionMode = PfeSelection.Multi },
                                new NodeAction(NodeActionType.Update, typeof(UpdateNavrhZmenyRzp)),
                                new NodeAction(NodeActionType.ZmenaStavuPodania, typeof(ChangeStateDto), roles_writer) { Caption = "Spracovať" },
                                new NodeAction(NodeActionType.PrevziatNavrhRozpoctu, roles_writer)
                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateNavrhZmenyRzp), roles_member),
                            LayoutDependencies = new List<LayoutDependency>
                            {
                                LayoutDependency.OneToMany("rzp-evi-navrh-pol", "D_NavrhZmenyRzp_Id", "Položky návrhov"),
                                LayoutDependency.OneToMany("rzp-sm-hzs", "D_NavrhZmenyRzp_Id", "História zmien stavov")
                            }
                        },

                        new HierarchyNode<ZmenyRzpView>("zmena", "Zmeny rozpočtu", f => f.Typ == true, HierarchyNodeType.Ciselnik)
                        {
                            Children = new List<HierarchyNode>
                            {
                                new HierarchyNode<ZmenyRzpValView>("pol", "Položky zmien", f => f.Typ == true, HierarchyNodeType.Ciselnik)
                                {
                                    Actions = new List<NodeAction>
                                    {
                                        new NodeAction(NodeActionType.Change, roles_member),
                                        new NodeAction(NodeActionType.Delete, typeof(DeleteZmenyRzpVal)) {SelectionMode = PfeSelection.Multi },
                                        new NodeAction(NodeActionType.Update, typeof(UpdateZmenyRzpVal))
                                    }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateZmenyRzpVal), roles_member),
                                },
                            },
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.Change, roles_member),
                                new NodeAction(NodeActionType.Delete, typeof(DeleteNavrhZmenyRzp)) {SelectionMode = PfeSelection.Multi },
                                new NodeAction(NodeActionType.Update, typeof(UpdateNavrhZmenyRzp)),
                                new NodeAction(NodeActionType.ZmenaStavuPodania, typeof(ChangeStateDto), roles_writer) { Caption = "Spracovať" }
                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateNavrhZmenyRzp), roles_member),
                            LayoutDependencies = new List<LayoutDependency>
                            {
                                LayoutDependency.OneToMany("rzp-evi-zmena-pol", "D_NavrhZmenyRzp_Id", "Položky zmien"),
                                LayoutDependency.OneToMany("rzp-sm-hzs", "D_NavrhZmenyRzp_Id", "História zmien stavov")
                            }
                        },

                        new HierarchyNode<IntDokladView>("intd", "Interné doklady", null, HierarchyNodeType.Ciselnik)
                        {
                            Children = new List<HierarchyNode>
                            {
                                new HierarchyNode<DennikRzpView>("pol", "Rozpočtový denník", null, HierarchyNodeType.Ciselnik)
                                {
                                    Actions = new List<NodeAction>
                                    {
                                        new NodeAction(NodeActionType.Change, roles_member),
                                        new NodeAction(NodeActionType.Delete, typeof(DeleteDennikRzp)) {SelectionMode = PfeSelection.Multi },
                                        new NodeAction(NodeActionType.Update, typeof(UpdateDennikRzp))
                                    }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateDennikRzp), roles_member),
                                },
                            },
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.Change, roles_member),
                                new NodeAction(NodeActionType.Delete, typeof(DeleteIntDoklad)) {SelectionMode = PfeSelection.Multi },
                                new NodeAction(NodeActionType.Update, typeof(UpdateIntDoklad))
                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateIntDoklad), roles_member),

                            LayoutDependencies = new List<LayoutDependency>
                            {
                                LayoutDependency.OneToMany("rzp-evi-intd-pol", "D_IntDoklad_Id", "Rozpočtový denník"),
                                LayoutDependency.OneToMany("rzp-sm-hzs", "D_BiznisEntita_Id", "História zmien stavov")

                            }
                        },

                        new HierarchyNode<RzpDummyData>("ciel", "Ciele *", null, HierarchyNodeType.Ciselnik)
                        {
                            #region Ukazovatele
                            
                            Children = new List<HierarchyNode>
                            {
                                new HierarchyNode<RzpDummyData>("ukazov", "Ukazovatele *", null, HierarchyNodeType.DatovaPolozka)
                                {
                                }
                            }
                            
                            #endregion
                        }
                    }
                    
                    #endregion
                },
                new HierarchyNode("prh", "Prehľady")
                {
                    #region Prehľady
                    
                    Children = new List<HierarchyNode>
                    {
                        new HierarchyNode("rzppol", "Rozpočtové položky *")
                        {
                            Children = new List<HierarchyNode>
                            {
                                new HierarchyNode<RzpDummyData>("plnenie", "Plnenie rozpočtu *", null, HierarchyNodeType.Money)
                                {
                                },
                                new HierarchyNode<RzpDummyData>("cerpanie", "Čerpanie rozpočtu *", null, HierarchyNodeType.Money)
                                {
                                },
                            }
                        },
                        new HierarchyNode("dennik", "Rozpočtový denník *")
                        {
                            Children = new List<HierarchyNode>
                            {
                                new HierarchyNode<RzpDummyData>("plnenie", "Denník plnenia rozpočtu *", null, HierarchyNodeType.DatovaPolozka)
                                {
                                },
                                new HierarchyNode<RzpDummyData>("cerpanie", "Denník čerpania rozpočtu *", null, HierarchyNodeType.DatovaPolozka)
                                {
                                }
                            }
                        },
                        new HierarchyNode<RzpDummyData>("prgrzp", "Programový rozpočet *", null, HierarchyNodeType.DatovaPolozka)
                        {
                        },
                    }
                    
                    #endregion
                },
                new HierarchyNode("vyk", "Výkazy")
                {
                    #region Výkazy
                    
                    Children = new List<HierarchyNode>
                    {
                        new HierarchyNode<Fin1Vykaz>("f112", "Výkaz FIN 1-12", null, HierarchyNodeType.DatovaPolozka)
                        {
                            Actions = new List<NodeAction>
                            {
                                   new NodeAction(NodeActionType.MenuButtonsAll, roles_member)
                                    {
                                        // takto vies zadat aj hlavnu ikonu, inac sa to robi cez AddMenuButtonsAll
                                        ActionIcon = NodeActionIcons.History,
                                        Caption = "História",
                                        MenuButtons = new List<NodeAction>()
                                        {
                                            new NodeAction(NodeActionType.UlozDoHistorieF112, null, "", roles_member) { SelectionMode = PfeSelection.Single },
                                        }
                                    }
                            },
                            Children = new List<HierarchyNode>
                            {
                                new HierarchyNode<Fin1>("f112h", "História", null, HierarchyNodeType.History)
                                {
                                    Children = new List<HierarchyNode>
                                    {
                                        new HierarchyNode<Fin1Pol>("f112p", "Položky výkazu", null, HierarchyNodeType.Ciselnik)
                                        {
                                        },
                                    },
                                    LayoutDependencies = new List<LayoutDependency>
                                    {
                                        LayoutDependency.OneToMany("rzp-vyk-f112-f112h-f112p", "D_Fin1_Id", "Položky výkazov")
                                    }
                                }
                            },
                            LayoutDependencies = new List<LayoutDependency>
                            {
                                LayoutDependency.OneToMany("rzp-evi-intd-pol", "FinKey", "Rozpočtový denník")
                            }
                        }
                    }

                    #endregion
                },
                new HierarchyNode("def", "Definície")
                {
                    #region Definície
                    
                    Children = new List<HierarchyNode>
                    {
                        new HierarchyNode<ProgramView>("prr", "Programový rozpočet", f => f.PRTyp == 1, HierarchyNodeType.ProgramovyRozpocet)
                        {
                            #region Programový rozpočet
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.Change, roles_member),
                                new NodeAction(NodeActionType.Delete, typeof(DeleteDefPrrProgramovyRozpocet)) {SelectionMode = PfeSelection.Multi },
                                new NodeAction(NodeActionType.Update, typeof(UpdateDefPrrProgramovyRozpocet))
                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateDefPrrProgramovyRozpocet), roles_member),
                            Children = new List<HierarchyNode>
                            {
                                new DatabaseHierarchyNode<ProgramovyRozpocetProgramView>("prg", "Program", (DatabaseHierarchyNode node) => { return Modules.Load<IRzpRepository>().RenderProgramovyRozpocet(node); }, new Filter("PRTyp", "2"), HierarchyNodeType.Program)
                                {
                                    Children = new List<HierarchyNode>
                                    {
                                        new DatabaseHierarchyNode<ProgramovyRozpocetPodprogramView>("pprg", "Podprogram", (DatabaseHierarchyNode node) => { return Modules.Load<IRzpRepository>().RenderProgramovyRozpocet(node); }, new Filter("PRTyp", "3"), HierarchyNodeType.Podprogram)
                                        {
                                            Children = new List<HierarchyNode>
                                            {
                                                new DatabaseHierarchyNode<ProgramovyRozpocetPrvokView>("prv", "Prvok", (DatabaseHierarchyNode node) => { return Modules.Load<IRzpRepository>().RenderProgramovyRozpocet(node); }, null, HierarchyNodeType.Prvok)
                                                {
                                                    Actions = new List<NodeAction>
                                                    {
                                                        new NodeAction(NodeActionType.Change, roles_member),
                                                        new NodeAction(NodeActionType.Delete, typeof(DeleteDefPrrProgramovyRozpocet)) {SelectionMode = PfeSelection.Multi },
                                                        new NodeAction(NodeActionType.Update, typeof(UpdateDefPrrProgramovyRozpocet))
                                                    },
                                                    LayoutDependencies = new List<LayoutDependency>
                                                    {
                                                        LayoutDependency.OneToMany("rzp-def-ciel", "D_Program_Id", "Ciele")//Som na úrovni prvku, takže môžem ísť rovno cez ID
                                                    }
                                                }
                                            },
                                            Actions = new List<NodeAction>
                                            {
                                                new NodeAction(NodeActionType.Change, roles_member),
                                                new NodeAction(NodeActionType.Delete, typeof(DeleteDefPrrProgramovyRozpocet)) {SelectionMode = PfeSelection.Multi },
                                                new NodeAction(NodeActionType.Update, typeof(UpdateDefPrrProgramovyRozpocet))
                                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateDefPrrProgramovyRozpocet), roles_member),
                                            LayoutDependencies = new List<LayoutDependency>
                                            {
                                                LayoutDependency.OneToMany("rzp-def-ciel", "D_Program_Id", "Ciele")//Som na úrovni prvku, takže môžem ísť rovno cez ID
                                            }
                                        }
                                    },
                                    Actions = new List<NodeAction>
                                    {
                                        new NodeAction(NodeActionType.Change, roles_member),
                                        new NodeAction(NodeActionType.Delete, typeof(DeleteDefPrrProgramovyRozpocet)) {SelectionMode = PfeSelection.Multi },
                                        new NodeAction(NodeActionType.Update, typeof(UpdateDefPrrProgramovyRozpocet))
                                    }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateDefPrrProgramovyRozpocet), roles_member),
                                    LayoutDependencies = new List<LayoutDependency>
                                    {
                                        LayoutDependency.OneToMany("rzp-def-ciel", "PRKod,PP", "Ciele") //Aplikácia zobrazí všetky ciele z podúrovní
                                        
                                    }
                                }
                            },
                            LayoutDependencies = new List<LayoutDependency>
                            {
                                LayoutDependency.OneToMany("rzp-def-ciel", "program,P", "Ciele") //Aplikácia zobrazí všetky ciele z podúrovní
                                //LayoutDependency.OneToMany("rzp-def-ciel", "D_Program_Id", "Ciele")
                            }
                            
                            #endregion
                        },
                        new HierarchyNode<ProgramView>("prs", "Programový rozpočet - sumárne", null, HierarchyNodeType.Ciselnik)
                        {
                            #region Programový rozpočet
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.Change, roles_member),
                                new NodeAction(NodeActionType.Delete, typeof(DeleteDefPrrProgramovyRozpocet)) {SelectionMode = PfeSelection.Multi },
                                new NodeAction(NodeActionType.Update, typeof(UpdateDefPrrProgramovyRozpocet))
                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateDefPrrProgramovyRozpocet), roles_member),
                            LayoutDependencies = new List<LayoutDependency>
                            {
                                LayoutDependency.OneToMany("rzp-def-ciel", "D_Program_Id", "Ciele")
                            }
                            #endregion
                        },
                        new HierarchyNode<RzpPolozkyPrijView>("prij", "Príjmové rozpočtové položky", f => f.PrijemVydaj == 1, HierarchyNodeType.Ciselnik)
                        {
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.Change, roles_member),
                                new NodeAction(NodeActionType.Delete, typeof(DeleteRzpPolozky)) {SelectionMode = PfeSelection.Multi },
                                new NodeAction(NodeActionType.Update, typeof(UpdateRzpPolozky))
                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateRzpPolozky), roles_member)
                        },
                        new HierarchyNode<RzpPolozkyVydView>("vyd", "Výdajové rozpočtové položky", f => f.PrijemVydaj == 2 , HierarchyNodeType.Ciselnik)
                        {
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.Change, roles_member),
                                new NodeAction(NodeActionType.Delete, typeof(DeleteRzpPolozky)) {SelectionMode = PfeSelection.Multi },
                                new NodeAction(NodeActionType.Update, typeof(UpdateRzpPolozky))
                            }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateRzpPolozky), roles_member)
                        },
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
                new HierarchyNode("rep", "Výkazy *")
                {
                    #region Definície
                    
                    #endregion
                },
                //new HierarchyNode("rep", "Reporty")
                //{
                //    #region Reporty
                   
                //    Children = new List<HierarchyNode>
                //    {
                //        new HierarchyNode("ucto", "Účtovníctvo")
                //        {
                //            #region Účtovníctvo
                            
                //            #endregion
                //        },
                //        new HierarchyNode("rozpo", "Rozpočet")
                //        {
                //            #region Rozpočet
                            
                //            #endregion
                //        }
                //    }
                    
                //    #endregion
                //},
                new HierarchyNode("cis", "Číselníky")
                {
                    #region Číselníky
                    
                    Children = new List<HierarchyNode>
                    {
                        //new HierarchyNode("ucto", "Účtovníctvo")
                        //{
                        //    RowsCounterRule = -3,
                            
                        //    #region Účtovníctvo
                            
                        //    Children = new List<HierarchyNode>
                        //    {
                        //        new HierarchyNode<RzpDummyData>("cisdokl", "Číslovanie dokladov", null, HierarchyNodeType.DatovaPolozka)
                        //        {
                        //        },
                        //    }
                            
                        //    #endregion
                        //},
                        new HierarchyNode<FRZdrojView>("zdr", "Kódy zdrojov", null, HierarchyNodeType.StatickyCiselnik)
                        {
                        },
                        new HierarchyNode<FRFKView>("fk", "Funkčná klasifikácia", null, HierarchyNodeType.StatickyCiselnik)
                        {
                        },
                        new HierarchyNode<FREKView>("ek", "Ekonomická klasifikácia", null, HierarchyNodeType.StatickyCiselnik)
                        {
                        }
                        //new HierarchyNode("rozpoek", "Rozpočet ekonomická klasifikácia *")
                        //{
                        //    #region Rozpočet ekonomická klasifikácia
                            
                        //    Children = new List<HierarchyNode>
                        //    {
                        //        new HierarchyNode<RzpDummyData>("hk", "Hlavná kategória *", null, HierarchyNodeType.Ciselnik)
                        //        {
                        //            Actions = new List<NodeAction>
                        //            {
                        //                new NodeAction(NodeActionType.Change),
                        //                new NodeAction(NodeActionType.Delete),
                        //                new NodeAction(NodeActionType.Update)
                        //            }
                        //        },
                        //        new HierarchyNode<RzpDummyData>("kat", "Kategória *", null, HierarchyNodeType.Ciselnik)
                        //        {
                        //            Actions = new List<NodeAction>
                        //            {
                        //                new NodeAction(NodeActionType.Change),
                        //                new NodeAction(NodeActionType.Delete),
                        //                new NodeAction(NodeActionType.Update)
                        //            }
                        //        },
                        //        new HierarchyNode<RzpDummyData>("pol", "Položka *", null, HierarchyNodeType.Ciselnik)
                        //        {
                        //            Actions = new List<NodeAction>
                        //            {
                        //                new NodeAction(NodeActionType.Change),
                        //                new NodeAction(NodeActionType.Delete),
                        //                new NodeAction(NodeActionType.Update)
                        //            }
                        //        },
                        //    }
                            
                        //    #endregion
                        //}
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
                        new HierarchyNode<RzpDummyData>("log", "Logovanie zmien *", null, HierarchyNodeType.Ciselnik)
                        {
                            #region Logovanie zmien
                            
                            #endregion
                        },
                        new HierarchyNode<EntitaHistoriaStavovView>("hzs", "História zmien stavov", null, HierarchyNodeType.Ciselnik)
                        {
                            #region História zmien stavov
                            
                            #endregion
                        },
                        //new HierarchyNode<RzpDummyData>("pre", "Preklady", null, HierarchyNodeType.Ciselnik)
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
