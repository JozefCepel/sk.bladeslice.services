using System;
using System.Collections.Generic;
using System.Linq;
using WebEas.Esam.ServiceModel.Office;
using WebEas.Esam.ServiceModel.Office.Dto;
using WebEas.Esam.ServiceModel.Office.Reg.Dto;
using WebEas.Esam.ServiceModel.Office.Reg.Types;
using WebEas.Esam.ServiceModel.Office.Types.Reg;
using WebEas.ServiceInterface;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Office.Egov.Reg.Types;
using WebEas.ServiceModel.Reg.Types;

namespace WebEas.Esam.ServiceInterface.Office.Reg
{
    public partial class RegRepository
    {
        public override string Code => "reg";

        public override HierarchyNode RenderModuleRootNode(string kodPolozky)
        {
            var rootNode = new HierarchyNode(Code, "Registre")
            {
                Children = new List<HierarchyNode>
                {
                    new HierarchyNode("ors", "Organizačná štruktúra")
                    {
                        #region Organizačná štruktúra

                        Children = new List<HierarchyNode>
                        {
                            new HierarchyNode<StrediskoView>("orj", GetNastavenieS("reg", "OrjNazovMC"), null, icon : HierarchyNodeIconCls.Sitemap)
                            {
                                RowsCounterRule = -1,
                                Actions = new List<NodeAction>
                                {
                                    new NodeAction(NodeActionType.Change),
                                    new NodeAction(NodeActionType.Delete, typeof(DeleteStredisko)) {SelectionMode = PfeSelection.Multi },
                                    new NodeAction(NodeActionType.Update, typeof(UpdateStredisko)),
                                    new NodeAction(NodeActionType.MenuButtonsAll)
                                    {
                                        ActionIcon = NodeActionIcons.Plus,
                                        Caption = "Pridať",
                                        MenuButtons = new List<NodeAction>()
                                        {
                                            new NodeAction(NodeActionType.Create, typeof(CreateStredisko)),
                                        }
                                    },
                                    new NodeAction(NodeActionType.MenuButtonsAll)
                                    {
                                        ActionIcon = NodeActionIcons.Refresh,
                                        Caption = "ISO",
                                        MenuButtons = new List<NodeAction>()
                                        {
                                            new NodeAction(NodeActionType.MigraciaPociatocnehoStavu, typeof(MigraciaStavovDto)),
                                        }
                                    }
                                },
                                DefaultValues = new List<NodeFieldDefaultValue>
                                {
                                        new NodeFieldDefaultValue(nameof(StrediskoView.PlatnostOd), DateTime.Now)
                                },
                                LayoutDependencies = new List<LayoutDependency>
                                {
                                    LayoutDependency.OneToMany("cfe-admin-users-modul-tors-ors", "TypeElement_Id", "Práva"),
                                    LayoutDependency.OneToMany("uct-def-kont-konf-rzp", nameof(StrediskoView.C_Stredisko_Id), "Predkontácia do rzp."),
                                    LayoutDependency.OneToMany("uct-def-kont-konf-uct", nameof(StrediskoView.C_Stredisko_Id), "Predkontácia do účt.")
                                },

                                Children = new List<HierarchyNode>
                                {
                                    new HierarchyNode<CislovanieView>("one", " Spoločné číslovania", x => x.CislovanieJedno == true, icon : HierarchyNodeIconCls.ListOl)
                                    {
                                        SelectionMode = PfeSelection.Multi,
                                        Actions = new List<NodeAction>
                                        {
                                            new NodeAction(NodeActionType.Change),
                                            new NodeAction(NodeActionType.Delete, typeof(DeleteCislovanie), "Obnoviť predvolené číslovanie") {SelectionMode = PfeSelection.Multi },
                                            new NodeAction(NodeActionType.Update, typeof(UpdateCislovanie))
                                        },
                                    },
                                    new DatabaseHierarchyNode<CislovanieView>("cis", "Modul", (DatabaseHierarchyNode node) => { return RenderCisTree("ORJ", node) ; }, null, icon : HierarchyNodeIconCls.ListOl)
                                    {
                                        SelectionMode = PfeSelection.Multi,
                                        Actions = new List<NodeAction>
                                        {
                                            new NodeAction(NodeActionType.Change),
                                            new NodeAction(NodeActionType.Delete, typeof(DeleteCislovanie), "Obnoviť predvolené číslovanie") {SelectionMode = PfeSelection.Multi },
                                            new NodeAction(NodeActionType.Update, typeof(UpdateCislovanie))
                                        },
                                    }
                                }
                            },
                            new HierarchyNode<PokladnicaView>("pok", "Pokladnice", null, icon : HierarchyNodeIconCls.MoneyBillAlt)
                            {
                                RowsCounterRule = -1,
                                Actions = new List<NodeAction>
                                {
                                    new NodeAction(NodeActionType.Change),
                                    new NodeAction(NodeActionType.Delete, typeof(DeletePokladnica)) {SelectionMode = PfeSelection.Multi },
                                    new NodeAction(NodeActionType.Update, typeof(UpdatePokladnica)),
                                    new NodeAction(NodeActionType.MenuButtonsAll)
                                    {
                                        ActionIcon = NodeActionIcons.Plus,
                                        Caption = "Pridať",
                                        MenuButtons = new List<NodeAction>()
                                        {
                                            new NodeAction(NodeActionType.Create, typeof(CreatePokladnica)),
                                        }
                                    },
                                    new NodeAction(NodeActionType.MenuButtonsAll)
                                    {
                                        ActionIcon = NodeActionIcons.Refresh,
                                        Caption = "ISO",
                                        MenuButtons = new List<NodeAction>()
                                        {
                                            new NodeAction(NodeActionType.MigraciaPociatocnehoStavu, typeof(MigraciaStavovDto)),
                                        }
                                    }
                                },
                                DefaultValues = new List<NodeFieldDefaultValue>
                                {
                                    new NodeFieldDefaultValue(nameof(PokladnicaView.PlatnostOd), System.DateTime.Today),
                                    new NodeFieldDefaultValue(nameof(PokladnicaView.C_Mena_Id), (short)MenaEnum.EUR)
                                },
                                LayoutDependencies = new List<LayoutDependency>
                                {
                                    LayoutDependency.OneToMany("cfe-admin-users-modul-tors-ors", "TypeElement_Id", "Práva"),
                                    LayoutDependency.OneToMany("uct-def-kont-konf-uct", nameof(PokladnicaView.C_Pokladnica_Id), "Predkontácia do účt.")
                                },

                                Children = new List<HierarchyNode>
                                {
                                    new DatabaseHierarchyNode<CislovanieView>("cis", "Modul", (DatabaseHierarchyNode node) => { return RenderCisTree("POK", node) ; }, null, icon : HierarchyNodeIconCls.ListOl)
                                    {
                                        SelectionMode = PfeSelection.Multi,
                                        Actions = new List<NodeAction>
                                        {
                                            new NodeAction(NodeActionType.Change),
                                            new NodeAction(NodeActionType.Delete, typeof(DeleteCislovanie), "Obnoviť predvolené číslovanie") {SelectionMode = PfeSelection.Multi },
                                            new NodeAction(NodeActionType.Update, typeof(UpdateCislovanie))
                                        }
                                    }
                                }
                            },
                            new HierarchyNode<BankaUcetView>("vbu", "Bankové účty", null, icon : HierarchyNodeIconCls.CreditCard)
                            {
                                RowsCounterRule = -1,
                                Actions = new List<NodeAction>
                                {
                                    new NodeAction(NodeActionType.Change),
                                    new NodeAction(NodeActionType.Delete, typeof(DeleteBankaUcet)) {SelectionMode = PfeSelection.Multi },
                                    new NodeAction(NodeActionType.Update, typeof(UpdateBankaUcet)),
                                    new NodeAction(NodeActionType.MenuButtonsAll)
                                    {
                                        ActionIcon = NodeActionIcons.Plus,
                                        Caption = "Pridať",
                                        MenuButtons = new List<NodeAction>()
                                        {
                                            new NodeAction(NodeActionType.Create, typeof(CreateBankaUcet)),
                                        }
                                    },
                                    new NodeAction(NodeActionType.MenuButtonsAll)
                                    {
                                        ActionIcon = NodeActionIcons.Refresh,
                                        Caption = "ISO",
                                        MenuButtons = new List<NodeAction>()
                                        {
                                            new NodeAction(NodeActionType.MigraciaPociatocnehoStavu, typeof(MigraciaStavovDto)),
                                        }
                                    }
                                },
                                DefaultValues = new List<NodeFieldDefaultValue>
                                {
                                    new NodeFieldDefaultValue(nameof(BankaUcetView.PlatnostOd), System.DateTime.Today),
                                    new NodeFieldDefaultValue(nameof(BankaUcetView.C_Mena_Id), (short)MenaEnum.EUR)
                                },
                                LayoutDependencies = new List<LayoutDependency>
                                {
                                    LayoutDependency.OneToMany("cfe-admin-users-modul-tors-ors", "TypeElement_Id", "Práva"),
                                    LayoutDependency.OneToMany("uct-def-kont-konf-uct", nameof(BankaUcetView.C_BankaUcet_Id), "Predkontácia do účt.")
                                },

                                Children = new List<HierarchyNode>
                                {
                                    new DatabaseHierarchyNode<CislovanieView>("cis", "Modul", (DatabaseHierarchyNode node) => { return RenderCisTree("VBU", node) ; }, null, icon : HierarchyNodeIconCls.ListOl)
                                    {
                                        SelectionMode = PfeSelection.Multi,
                                        Actions = new List<NodeAction>
                                        {
                                            new NodeAction(NodeActionType.Change),
                                            new NodeAction(NodeActionType.Delete, typeof(DeleteCislovanie), "Obnoviť predvolené číslovanie") {SelectionMode = PfeSelection.Multi },
                                            new NodeAction(NodeActionType.Update, typeof(UpdateCislovanie))
                                        }
                                    }
                                }
                            }
                        }

                        #endregion Organizačná štruktúra
                    },
                    new HierarchyNode("cis", "Číselníky")
                    {
                        #region Definicie

                        Children = new List<HierarchyNode>
                        {
                            new HierarchyNode<ProjektView>("prj", "Projekty", null, icon : HierarchyNodeIconCls.Rocket)
                            {
                                RowsCounterRule = -1,
                                Actions = new List<NodeAction>
                                {
                                    new NodeAction(NodeActionType.Change),
                                    new NodeAction(NodeActionType.Delete, typeof(DeleteProjekt)) {SelectionMode = PfeSelection.Multi },
                                    new NodeAction(NodeActionType.Update, typeof(UpdateProjekt)),
                                    new NodeAction(NodeActionType.MenuButtonsAll)
                                    {
                                        ActionIcon = NodeActionIcons.Plus,
                                        Caption = "Pridať",
                                        MenuButtons = new List<NodeAction>()
                                        {
                                            new NodeAction(NodeActionType.Create, typeof(CreateProjekt)),
                                        }
                                    },
                                    new NodeAction(NodeActionType.MenuButtonsAll)
                                    {
                                        ActionIcon = NodeActionIcons.Refresh,
                                        Caption = "ISO", //Toto má iba KORWIN
                                        MenuButtons = new List<NodeAction>()
                                        {
                                            new NodeAction(NodeActionType.MigraciaPociatocnehoStavu, typeof(MigraciaStavovDto)),
                                        }
                                    }
                                },
                                DefaultValues = new List<NodeFieldDefaultValue>
                                {
                                    new NodeFieldDefaultValue(nameof(ProjektView.PlatnostOd), System.DateTime.Today),
                                    new NodeFieldDefaultValue(nameof(ProjektView.DatumDotacie), System.DateTime.Today),
                                }
                            },
                            new HierarchyNode<BankaView>("ban", "Banky", null, icon : HierarchyNodeIconCls.University)
                            {
                                Actions = new List<NodeAction>
                                {
                                    new NodeAction(NodeActionType.Change),
                                    new NodeAction(NodeActionType.Delete, typeof(DeleteBanka)) {SelectionMode = PfeSelection.Multi },
                                    new NodeAction(NodeActionType.Update, typeof(UpdateBanka))
                                }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateBanka)),
                            },
                            new HierarchyNode<MenaView>("mena", "Meny", null, icon : HierarchyNodeIconCls.Euro)
                            {
                                Children = new List<HierarchyNode>
                                {
                                    new HierarchyNode<MenaKurzView>("kurz", "Kurzový lístok", null, icon : HierarchyNodeIconCls.ChartLine)
                                    {
                                        Actions = new List<NodeAction>
                                        {
                                            new NodeAction(NodeActionType.Change),
                                            new NodeAction(NodeActionType.Delete, typeof(DeleteMenaKurz)) {SelectionMode = PfeSelection.Multi },
                                            new NodeAction(NodeActionType.Update, typeof(UpdateMenaKurz))
                                        }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateMenaKurz)),
                                        DefaultValues = new List<NodeFieldDefaultValue>
                                        {
                                            new NodeFieldDefaultValue(nameof(MenaView.PlatnostOd), System.DateTime.Today),
                                        }
                                    },
                                },
                            },
                            new HierarchyNode<StatView>("stat", "Štáty", null, icon : HierarchyNodeIconCls.GlobeEurope)
                            {
                            },
                            new HierarchyNode<TypView>("typ", "Systémové a užívateľské typy", null, icon : HierarchyNodeIconCls.Delicious)
                            {
                                Actions = new List<NodeAction>
                                {
                                    new NodeAction(NodeActionType.Change),
                                    new NodeAction(NodeActionType.Delete, typeof(DeleteTyp)) {SelectionMode = PfeSelection.Multi },
                                    new NodeAction(NodeActionType.Update, typeof(UpdateTyp))
                                }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateTyp)),
                                DefaultValues = new List<NodeFieldDefaultValue>
                                {
                                    new NodeFieldDefaultValue(nameof(TypView.Polozka), true),
                                },
                                LayoutDependencies = new List<LayoutDependency>
                                {
                                    LayoutDependency.OneToMany("reg-cis-typ-map", "C_Typ_Id", "Doklady")
                                },
                                Children = new List<HierarchyNode>
                                {
                                    new HierarchyNode<TypBiznisEntityTypView>("map", "Priradenie k dokladom", null, icon : HierarchyNodeIconCls.FileImport)
                                    {
                                        LayoutDependencies = new List<LayoutDependency>
                                        {
                                            LayoutDependency.OneToMany("uct-def-kont-konf-rzp", nameof(TypBiznisEntityTypView.C_Typ_Id), "Predkontácia do rzp."),
                                            LayoutDependency.OneToMany("uct-def-kont-konf-uct", nameof(TypBiznisEntityTypView.C_Typ_Id), "Predkontácia do účt.")
                                            
                                            //Nefunguje predpĺňanie prepojovacieho poľa - takže to takto zatiaľ nejdem robiť
                                            //LayoutDependency.OneToMany("uct-def-kont-konf-rzp", nameof(TypBiznisEntityTypView.C_Typ_Id) + ';' + nameof(TypBiznisEntityTypView.SkupinaPredkont_Id), "Predkontácia do rzp."),
                                            //LayoutDependency.OneToMany("uct-def-kont-konf-uct", nameof(TypBiznisEntityTypView.C_Typ_Id) + ';' + nameof(TypBiznisEntityTypView.SkupinaPredkont_Id), "Predkontácia do účt.")

                                        },
                                        Actions = new List<NodeAction>
                                        {
                                            new NodeAction(NodeActionType.Change),
                                            new NodeAction(NodeActionType.Delete, typeof(DeleteTypBiznisEntityTyp)) {SelectionMode = PfeSelection.Multi },
                                            new NodeAction(NodeActionType.Update, typeof(UpdateTypBiznisEntityTyp))
                                        }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateTypBiznisEntityTyp)),
                                    },
                                },
                            },
                            new HierarchyNode<KSView>("ks", "Konštantné symboly", null, icon : HierarchyNodeIconCls.StripeS)
                            {
                            },
                            new HierarchyNode("dph", "DPH")
                            {
                                Children = new List<HierarchyNode>
                                {
                                    new HierarchyNode<DphSadzbaView>("sadz", "Sadzby DPH", null, icon : HierarchyNodeIconCls.Percent)
                                    {
                                        Actions = new List<NodeAction>
                                        {
                                            new NodeAction(NodeActionType.Change),
                                            new NodeAction(NodeActionType.Delete, typeof(DeleteDphSadzba)) {SelectionMode = PfeSelection.Multi },
                                            new NodeAction(NodeActionType.Update, typeof(UpdateDphSadzba))
                                        }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateDphSadzba)),
                                        DefaultValues = new List<NodeFieldDefaultValue>
                                        {
                                            new NodeFieldDefaultValue(nameof(DphSadzba.PlatnostOd), DateTime.Today)
                                        },
                                    },
                                },
                            }
                        }

                        #endregion Definicie
                    },
                    new HierarchyNode("log", "Logovanie")
                    {
                        #region Logovanie

                        Children = new List<HierarchyNode>
                        {
                            new HierarchyNode<LoggingConfig>("nlze", "Nastavenie logovania zmien entít", null, icon : HierarchyNodeIconCls.LaptopCode)
                            {
                                SelectionMode = PfeSelection.Multi,
                                Actions = new List<NodeAction>
                                {
                                    new NodeAction(NodeActionType.Change),
                                    new NodeAction(NodeActionType.Delete, typeof(DeleteLoggingConfig)),
                                    new NodeAction(NodeActionType.Update, typeof(UpdateLoggingConfig))
                                }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateLoggingConfig))
                            }
                        }

                        #endregion Logovanie
                    },
                    new HierarchyNode("ssp", "Úkony")
                    {
                        #region Úkony

                        Children = new List<HierarchyNode>
                        {
                            new HierarchyNode<StavEntityView>("ciss", "Číselník stavov", null, icon : HierarchyNodeIconCls.TrafficLight)
                            {
                                SelectionMode = PfeSelection.Multi,
                                Actions = new List<NodeAction>
                                {
                                    new NodeAction(NodeActionType.Change),
                                    new NodeAction(NodeActionType.Delete, typeof(DeleteStavEntity)),
                                    new NodeAction(NodeActionType.Update, typeof(UpdateStavEntity))
                                }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateStavEntity))
                            },
                            new HierarchyNode<StavovyPriestorView>("kbss", "Stavové priestory", null, icon : HierarchyNodeIconCls.ExpandArrowsAlt)
                            {
                                SelectionMode = PfeSelection.Multi,
                                Actions = new List<NodeAction>
                                {
                                    new NodeAction(NodeActionType.Change),
                                    new NodeAction(NodeActionType.Delete, typeof(DeleteStavovyPriestor)),
                                    new NodeAction(NodeActionType.Update, typeof(UpdateStavovyPriestor))
                                }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateStavovyPriestor)),
                                Children = new List<HierarchyNode>
                                {
                                    new HierarchyNode<StavEntityStavEntityView>("dfs", "Úkony", null, icon : HierarchyNodeIconCls.PeopleCarry)
                                    {
                                        // bude len Read-Only - plnime s ciselnikov
                                        //SelectionMode = PfeSelection.Multi,
                                        //Actions = new List<NodeAction>
                                        //{
                                        //    new NodeAction(NodeActionType.Change),
                                        //    new NodeAction(NodeActionType.Delete, typeof(DeleteStavEntityStavEntity)),
                                        //    new NodeAction(NodeActionType.Update, typeof(UpdateStavEntityStavEntity))
                                        //}.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateStavEntityStavEntity))
                                    }
                                },
                                LayoutDependencies = new List<LayoutDependency>
                                {
                                    LayoutDependency.OneToMany("reg-ssp-kbss-dfs", "C_StavovyPriestor_Id", "Úkony"),
                                    LayoutDependency.OneToMany("reg-ssp-sptd", "C_StavovyPriestor_Id", "Priradené k dokladom")
                                }
                            },
                            new HierarchyNode<TypBiznisEntityView>("sptd", "Stav. priestory vs. typy dokladov", null, icon : HierarchyNodeIconCls.MapSigns)
                            {
                            }
                        }

                        #endregion Úkony
                    },
                    GenerateNodeSpravaModulu(Code, typeof(UpdateNastavenie))
                    .AddChildren( new  List<HierarchyNode>
                        {
                            new HierarchyNode<TypBiznisEntityNastavView>("snd", "Systémové nastavenia dokladov", null, icon : HierarchyNodeIconCls.Cogs)
                            {
                                SelectionMode = PfeSelection.Multi,
                                Actions = new List<NodeAction>
                                {
                                    new NodeAction(NodeActionType.Change),
                                    new NodeAction(NodeActionType.Update, typeof(UpdateTypBiznisEntityNastav)),
                                    new NodeAction(NodeActionType.RefreshDefault, typeof(RefreshDefault)){SelectionMode = PfeSelection.Multi, IdField = "D_TypBiznisEntityNastav_Id"},
                                },
                                Children = new List<HierarchyNode>
                                {
                                    new HierarchyNode<TypBiznisEntity_KnihaView>("kd", "Knihy dokladov", null, icon : HierarchyNodeIconCls.Book)
                                    {
                                        SelectionMode = PfeSelection.Multi,
                                        Actions = new List<NodeAction>
                                        {
                                            new NodeAction(NodeActionType.Change),
                                            new NodeAction(NodeActionType.Delete, typeof(DeleteTypBiznisEntity_Kniha)),
                                            new NodeAction(NodeActionType.Update, typeof(UpdateTypBiznisEntity_Kniha))
                                        }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateTypBiznisEntity_Kniha)),
                                        Children = new List<HierarchyNode>
                                        {
                                            new HierarchyNode<CislovanieView>("cis", "Číslovania", null, icon : HierarchyNodeIconCls.ListOl)
                                            {
                                                SelectionMode = PfeSelection.Multi,
                                                Actions = new List<NodeAction>
                                                {
                                                    new NodeAction(NodeActionType.Change),
                                                    new NodeAction(NodeActionType.Delete, typeof(DeleteCislovanie), "Obnoviť predvolené číslovanie") {SelectionMode = PfeSelection.Multi },
                                                    new NodeAction(NodeActionType.Update, typeof(UpdateCislovanie))
                                                }
                                            },
                                        },
                                        LayoutDependencies = new List<LayoutDependency>
                                        {
                                            LayoutDependency.OneToMany("cfe-admin-users-modul-tors-ors", "TypeElement_Id", "Práva")
                                        }
                                    },
                                    new HierarchyNode<TypBiznisEntity_ParovanieDefView>("dpar", "Definícia párovania", null, icon : HierarchyNodeIconCls.Random)
                                    {
                                        //SelectionMode = PfeSelection.Multi,
                                        Actions = new List<NodeAction>
                                        {
                                            new NodeAction(NodeActionType.Change, null, "Upraviť (SysAdmin)"),
                                            new NodeAction(NodeActionType.Delete, typeof(DeleteTypBiznisEntity_ParovanieDef), "Zmazať (SysAdmin)"),
                                            new NodeAction(NodeActionType.Update, typeof(UpdateTypBiznisEntity_ParovanieDef))
                                        }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateTypBiznisEntity_ParovanieDef), "Pridať", "Nový záznam (SysAdmin)")
                                    }
                                },
                                LayoutDependencies = new List<LayoutDependency>
                                {
                                    LayoutDependency.OneToMany("reg-cis-typ-map", "C_TypBiznisEntity_Id", "Priradenie typov")
                                }
                            },
                            new HierarchyNode<TextaciaView>("txt", "Textácie", null, icon : HierarchyNodeIconCls.QuoteLeft)
                            {
                                Actions = new List<NodeAction>
                                {
                                    new NodeAction(NodeActionType.Change),
                                    new NodeAction(NodeActionType.Delete, typeof(DeleteTextacia)) {SelectionMode = PfeSelection.Multi },
                                    new NodeAction(NodeActionType.Update, typeof(UpdateTextacia))
                                }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateTextacia)),
                                DefaultValues = new List<NodeFieldDefaultValue>
                                {
                                    new NodeFieldDefaultValue(nameof(TextaciaView.RokOd), DateTime.Today.Year)
                                },
                                LayoutDependencies = new List<LayoutDependency>
                                {
                                    LayoutDependency.OneToMany("reg-sm-txt-pol", "C_Textacia_Id", "Položky")
                                },
                                Children = new List<HierarchyNode>
                                {
                                    new HierarchyNode<TextaciaPolView>("pol", "Položky", null, icon : HierarchyNodeIconCls.QuoteRight)
                                    {
                                        Actions = new List<NodeAction>
                                        {
                                            new NodeAction(NodeActionType.Change),
                                            new NodeAction(NodeActionType.Delete, typeof(DeleteTextaciaPol)) {SelectionMode = PfeSelection.Multi },
                                            new NodeAction(NodeActionType.Update, typeof(UpdateTextaciaPol))
                                        }.AddMenuButtonsAll(NodeActionType.Create, typeof(CreateTextaciaPol)),
                                    },
                                },
                            }
                        })
                }
            }

            #region LayoutDependencies

            .SetLayoutDependencies(
                HierarchyNodeDependency.One2ManyBack2One("reg-sm-snd", "reg-sm-snd-kd", "C_TypBiznisEntity_Id", "Knihy dokladu", "Typ dokladu"),
                HierarchyNodeDependency.One2ManyBack2One("reg-cis-mena", "reg-cis-mena-kurz", "C_Mena_Id", "Kurzový listok", "Mena"),
                HierarchyNodeDependency.One2ManyBack2One("reg-sm-snd-kd", "reg-sm-snd-kd-cis", "C_TypBiznisEntity_Kniha_Id", "Číslovania", "Kniha")
            );

            #endregion LayoutDependencies

            var polozkyCustomRezim = new List<string>
            {
                "reg-sm-snd-dpar",
            };

            if (polozkyCustomRezim.Contains(kodPolozky))
            {
                if (Session.AdminLevel != AdminLevel.SysAdmin)
                {
                    foreach (var node in rootNode.Children.RecursiveSelect(w => w.Children).Where(x => polozkyCustomRezim.Contains(x.KodPolozky)))
                    {
                        node.Actions.RemoveAll(x => x.ActionType != NodeActionType.ReadList);
                    }
                }
            }

            return rootNode;
        }
    }
}