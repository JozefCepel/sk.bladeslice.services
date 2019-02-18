using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Ninject;
using ServiceStack.Logging;
using WebEas.ServiceModel;
using WebEas.Ninject;
using WebEas.ServiceModel.Pfe.Dto;

namespace WebEas.ServiceInterface
{
    public class Modules
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Modules));
        private static readonly object lockObject = new Object();
        private static readonly object lockHierarchyNodeList = new Object();

        private static List<HierarchyNode> hierarchyNodeList = null;
        private static List<IWebEasServiceInterface> modules = null;

        /// <summary>
        /// Gets or sets the modul list.
        /// </summary>
        /// <value>The modul list.</value>
        public static List<HierarchyNode> HierarchyNodeList
        {
            get
            {
                if (hierarchyNodeList == null)
                {
                    lock (lockHierarchyNodeList)
                    {
                        if (hierarchyNodeList == null)
                        {
                            var hierarchyList = new List<HierarchyNode>();
                            log.Info("loading hierarchy");
                            List<IWebEasServiceInterface> list = List;
                            foreach (IWebEasServiceInterface module in list)
                            {
                                if (module.RootNode != null)
                                {
                                    // DCOMDEUS - 884: s glob. zaznamami nic nerobit + oprava stat. ciselnika
                                    foreach (var node in module.RootNode.Children.RecursiveSelect(x => x.Children))
                                    {
                                        Type tmp = node.ModelType;

                                        while (tmp != null)
                                        {
                                            var dialAtt = tmp.GetCustomAttributes(typeof(DialAttribute), true);
                                            if (dialAtt != null && dialAtt.Count() > 0)
                                            {
                                                if (((DialAttribute)dialAtt[0]).Type == DialType.Global)
                                                {
                                                    node.Typ = HierarchyNodeType.StatickyCiselnik;
                                                    log.Debug(string.Format("Opravena ikona na stat. ciselnik - {0};", node.KodPolozky));

                                                    bool removeMenButAll = false;
                                                    if (node.Actions.Any(x => x.ActionType == NodeActionType.MenuButtonsAll))
                                                    {
                                                        var menButAll = node.Actions.First(x => x.ActionType == NodeActionType.MenuButtonsAll);
                                                        menButAll.MenuButtons.RemoveAll(x => x.ActionType == NodeActionType.Create);
                                                        removeMenButAll = !menButAll.MenuButtons.Any();
                                                    }

                                                    if (removeMenButAll)
                                                    {
                                                        node.Actions.RemoveAll(x => x.ActionType == NodeActionType.MenuButtonsAll);
                                                    }

                                                    node.Actions.RemoveAll(x => x.ActionType == NodeActionType.Create);
                                                    node.Actions.RemoveAll(x => x.ActionType == NodeActionType.Change);
                                                    node.Actions.RemoveAll(x => x.ActionType == NodeActionType.Update);
                                                    node.Actions.RemoveAll(x => x.ActionType == NodeActionType.Delete);
                                                }
                                            }
                                            tmp = tmp.BaseType;
                                        }
                                    }
                                    hierarchyList.Add(module.RootNode);
                                }
                            }
                            if (hierarchyList != null && hierarchyList.Count > 1)
                            {
                                hierarchyList = hierarchyList.OrderBy(nav => nav.Nazov).ToList();
                            }
                            log.Info(string.Format("loaded {0} hierarchy modules", hierarchyList.Count));
                            hierarchyNodeList = hierarchyList;
                        }
                    }
                }
                return hierarchyNodeList;
            }
            set
            {
                hierarchyNodeList = value;
            }
        }

        /// <summary>
        /// Gets the modules.
        /// </summary>
        /// <value>The modules.</value>
        public static List<IWebEasServiceInterface> List
        {
            get
            {
                if (modules == null)
                {
                    lock (lockObject)
                    {
                        if (modules == null)
                        {
                            LogManager.GetLogger("modulList").Info("Loading modul list");
                            modules = NinjectServiceLocator.Kernel.GetAll<IWebEasServiceInterface>().ToList();
                            if (modules.Count() == 0)
                            {
                                LogManager.GetLogger("modulList").Error(string.Format("Cannot load modules {0}", modules.Count()));
                                Thread.Sleep(10);
                                modules = NinjectServiceLocator.Kernel.GetAll<IWebEasServiceInterface>().ToList();
                                LogManager.GetLogger("modulList").Error(string.Format("Cannot load modules {0}", modules.Count()));
                                if (modules.Count() == 0)
                                {
                                    throw new WebEasException("No modules loaded");
                                }
                            }
                            else
                            {
                                LogManager.GetLogger("modulList").Info(string.Format("Loaded {0} modules", modules.Count()));
                            }
                        }
                    }
                }
                return modules;
            }
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <returns></returns>
        public static T Load<T>()
        {
            T instance = NinjectServiceLocator.Kernel.Get<T>();
            if (instance == null)
            {
                throw new WebEasException(string.Format("Cannot load instance of {0}", typeof(T)));
            }
            return instance;
        }

        /// <summary>
        /// Finds the module.
        /// </summary>
        /// <param name="kodPolozky">The kod polozky.</param>
        /// <returns></returns>
        public static string FindModule(string kodPolozky)
        {
            HierarchyNode node = HierarchyNodeList.Find(kodPolozky);

            while (!node.IsRoot)
            {
                node = node.Parent;
            }

            return node.Kod;
        }

        /// <summary>
        /// Cleans the code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public static string CleanCode(string code)
        {
            if (string.IsNullOrEmpty(code))
                return code;

            if (code.Contains('!'))
            {
                return Regex.Replace(code, "(![^-]*)", "");
            }
            return code;
        }

        /// <summary>
        /// Finds the node. Throws exception if node is not found.
        /// </summary>
        /// <param name="kodPolozky">Node.Kod</param>
        public static HierarchyNode FindNode(string kodPolozky)
        {
            HierarchyNode node = HierarchyNodeList.Find(kodPolozky);

            if (node == null)
            {
                throw new Exception(String.Format("Pohľad {0} nenájdený", kodPolozky));
            }
            if (node.ModelType == null)
            {
                throw new Exception(String.Format("Pre {0} nie je definovaný dátovy model", kodPolozky));
            }

            return node;
        }

        /// <summary>
        /// Finds the node. Returns null if node is not found.
        /// </summary>
        /// <param name="kodPolozky">Node.Kod</param>
        public static HierarchyNode TryFindNode(string kodPolozky)
        {
            return HierarchyNodeList.Find(kodPolozky);
        }

        /// <summary>
        /// Finds the first node.
        /// </summary>
        /// <param name="typ">The typ.</param>
        /// <returns></returns>
        public static HierarchyNode FindFirstNode(Type typ)
        {
            HierarchyNode node = HierarchyNodeList.First(typ);

            if (node == null)
            {
                throw new WebEasException(string.Format("Položka stromu pre dátovy typ {0} nebola nájdená", typ.Name));
            }
            return node;
        }

        /// <summary>
        /// Finds the nodes.
        /// </summary>
        /// <param name="typ">The typ.</param>
        /// <returns></returns>
        public static List<HierarchyNode> FindNode(Type typ)
        {
            List<HierarchyNode> nodes = HierarchyNodeList.Find(typ);

            if (nodes == null)
            {
                throw new Exception(String.Format("Položky typu {0} nenájdené", typ.Name));
            }

            return nodes;
        }

        /// <summary>
        /// Aplikuje role na hierarchyNodeList, odstrani nepristupne nody
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="roles">The roles.</param>
        /// <returns></returns>
        public static HierarchyNode ApplyRoles(HierarchyNode node, HashSet<string> roles)
        {
            return ApplyRoles(new List<HierarchyNode> { node }, roles).FirstOrDefault();
        }

        /// <summary>
        /// Aplikuje role na hierarchyNodeList, odstrani nepristupne nody
        /// </summary>
        /// <param name="hierarchyNodeList"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public static List<HierarchyNode> ApplyRoles(List<HierarchyNode> hierarchyNodeList, HashSet<string> roles)
        {
            var modulList = new List<HierarchyNode>();

            for (int i = 0; i < hierarchyNodeList.Count; i++)
            {
                if (!hierarchyNodeList[i].IsInRole(roles))
                {
                    continue;
                }
                HierarchyNode node = hierarchyNodeList[i].Clone();

                modulList.Add(node);

                if (node.Children.Count > 0)
                {
                    node.Children = ApplyRoles(node.Children, roles);
                }
            }

            return modulList;
        }

        public static List<HierarchyNode> GetNodesByBiznisEntityType(ServiceModel.Office.Egov.Reg.Types.TypBiznisEntityEnum typ)
        {
            return HierarchyNodeList.Find(typ).ToList();
        }

        /// <summary>
        /// Generate tree node Správa modulu / Logovanie zmien, História zmien stavov
        /// </summary>
        public static HierarchyNode GenerateNodeSpravaModulu(string code, string[] roles_member, string[] roles_admin, string[] roles_ciswriter)
        {
            return new HierarchyNode("sm", "Správa modulu")
            {
                Children = new List<HierarchyNode>
                {
                    new HierarchyNode("log", "Logovanie zmien", typeof(ServiceModel.Types.LoggingView), new Filter("Schema", code), HierarchyNodeType.History, PfeSelection.Single, true)
                    {
                        Roles = roles_admin.ToList()
                    },
                    new HierarchyNode("hzs", "História zmien stavov", typeof(ServiceModel.Office.Egov.Reg.Types.EntitaHistoriaStavovView), new Filter("Modul", code), HierarchyNodeType.History,PfeSelection.Single, true)
                    {
                        Actions = new List<NodeAction>
                        {
                            new NodeAction(NodeActionType.MenuButtons, roles_member)
                            {
                                MenuButtons = new List<NodeAction>
                                {
                                    new NodeAction(NodeActionType.ZobrazEPodRozhodnutie, roles_member),
                                    new NodeAction(NodeActionType.ZobrazEFormRozhodnutie, roles_member),
                                }.AddNodeActionPodanie(roles_member)
                            }
                        },
                        Roles = roles_admin.ToList()
                    },
                    //Polozka preklady je zadefinovana ako cross-modulova, okrem REG, kory ma mat aj ine stlpce
                    new HierarchyNode("pre", "Preklady", typeof(TranslationColumnEntity), null, HierarchyNodeType.StatickyCiselnik, PfeSelection.Single, true)
                    {
                        Actions = new List<NodeAction>
                        {
                            new NodeAction(NodeActionType.ReadList, typeof(ListTranslationColumns), roles_ciswriter) { IdField = "UniqueIdentifier" }
                        },
                        LayoutDependencies = new List<LayoutDependency>()
                        {
                            LayoutDependency.OneToMany("reg-sm-pre-td", "UniqueIdentifier", "Slovník")
                        },
                        Roles = roles_ciswriter.ToList()
                    }
                },
                Roles = roles_ciswriter.ToList() // roles_admin = admin je nadradena rola. Polozka vsak obsahuje jednu polozku, ktora ma byt dostupna aj pre writtera
            };
        }

    }
}