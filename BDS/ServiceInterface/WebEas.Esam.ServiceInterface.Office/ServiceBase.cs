using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using WebEas.Core.Base;
using WebEas.Esam.ServiceModel.Office;
using WebEas.ServiceInterface;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceInterface.Office
{
    [Authenticate]
    public class ServiceBase : WebEasCoreServiceBase, ServiceModel.Office.IServiceBase
    {
        public IServerEvents ServerEvents { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceBase" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public ServiceBase(IRepositoryBase repository) : base(repository)
        {
        }

        /// <summary>
        /// Gets or sets the repository.
        /// </summary>
        /// <value>The repository.</value>
        public new IRepositoryBase Repository
        {
            get
            {
                return (IRepositoryBase)repository;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            if (repository != null)
            {
                repository.Dispose();
            }
        }

        public override void OnBeforeExecute(object requestDto)
        {
            var webeasRepositoryBase = (IRepositoryBase)repository;
            webeasRepositoryBase.Session = SessionAs<IWebEasSession>();

            if (HostContext.ServiceName != "pfe" && requestDto.GetType().Name != "NotifyPersonDataChangeDto" && requestDto.GetType().Name != "LongOperationListDto")
            {
                if (requestDto.GetType().HasAttribute<RouteAttribute>())
                {
                    var rootNode = webeasRepositoryBase.RenderModuleRootNode(webeasRepositoryBase.Code);
                    var routeUrl = requestDto.GetType().FirstAttribute<RouteAttribute>().Path;
                    var usernoderights = webeasRepositoryBase.GetUserTreeRights(webeasRepositoryBase.Code);
                    var hierarchyNodesWithUrl = rootNode.Children.RecursiveSelect(w => w.Children).Where(x => x.Actions.Any(z => z.Url != null && z.Url.Contains(routeUrl)));

                    //kontrola na akciu
                    foreach (var node in hierarchyNodesWithUrl)
                    {
                        var userTreeRight = usernoderights.FirstOrDefault(r => r.Kod == RepairNodeKey(node.KodPolozky));
                        if (node.GeneratedNode)
                        {

                        }
                        foreach (NodeAction act in node.Actions.Where(z => z.Url != null && z.Url.Contains(routeUrl)))
                        {
                            if (act.ActionType is NodeActionType.MenuButtonsAll)
                            {
                                act.MenuButtons.ForEach((x) => {
                                    if (!HierarchyNode.HasRolePrivileges(x, userTreeRight))
                                    {
                                        throw new WebEasUnauthorizedAccessException();
                                    }
                                });
                            }
                            else
                            {
                                if (!HierarchyNode.HasRolePrivileges(act, userTreeRight))
                                {
                                    throw new WebEasUnauthorizedAccessException();
                                }
                            }
                        }
                    }

                    // kontrola na ListDto
                    if (requestDto.GetType().HasInterface(typeof(IListDto)))
                    {
                        var kodPolozky = RepairNodeKey(((IListDto)requestDto).KodPolozky);
                        var userTreeRight = usernoderights.FirstOrDefault(r => r.Kod == kodPolozky);

                        if (userTreeRight == null || userTreeRight.Pravo == 0)
                        {
                            var node = rootNode.Find(kodPolozky);
                            if (node == null || !node.GeneratedNode || !HasParentPermissionForGeneratedNode(node, usernoderights))
                                throw new WebEasUnauthorizedAccessException();
                        }
                    }

                    // kontrola na ListComboDto
                    if (requestDto.GetType().HasInterface(typeof(IListComboDto)))
                    {
                        var kodPolozky = RepairNodeKey(((IListComboDto)requestDto).KodPolozky);
                        var userTreeRight = usernoderights.FirstOrDefault(r => r.Kod == kodPolozky);

                        if (userTreeRight == null || userTreeRight.Pravo == 0)
                        {
                            var node = rootNode.Find(kodPolozky);
                            if (node == null || !node.GeneratedNode || !HasParentPermissionForGeneratedNode(node, usernoderights))
                                throw new WebEasUnauthorizedAccessException();
                        }
                    }

                }
            }

            base.OnBeforeExecute(requestDto);
        }

        /// <summary>
        /// Udaje k polozke stromu
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public object GetList(IListDto request)
        {
            HierarchyNode node = Repository.RenderModuleRootNode(request.KodPolozky).FindNode(request.KodPolozky);
            var pagging = new PaggingParameters { PageNumber = request.Page, PageSize = request.Limit };

            var keys = this.Request.QueryString.AllKeys.Except(new string[] { "KodPolozky", "_dc", "page", "start", "limit", "filters", "Mat. group", "sort" }).ToList();
            if (keys.Count > 0)
            {
                Dictionary<string, string> props = this.GetPropertyList(node.ModelType);
                foreach (string key in keys)
                {
                    if (props.Keys.Contains(key.ToLower()))
                    {
                        if (request.AdditionalAttributes == null)
                        {
                            request.AdditionalAttributes = new Dictionary<string, string>();
                        }
                        string p = props[key.ToLower()];
                        request.AdditionalAttributes.Add(p, this.Request.QueryString[key].Replace("'", ""));
                    }
                }
            }

            object data = this.Repository.GetType().GetMethod("GetList", new Type[] { typeof(BaseListDto), typeof(HierarchyNode), typeof(PaggingParameters) }).MakeGenericMethod(node.ModelType).Invoke(this.Repository, new object[] { request, node, pagging });

            if (pagging != null && this.Response != null)
            {
                this.Response.AddHeader("Records-Count", pagging.RecordsCount.ToString());
            }

            return data;
        }

        /// <summary>
        /// Zistenie poctu zaznamov pre zadane polozky stromu.
        /// Nezname kody preskoci, inak vrati zoznam parov { kod polozky - pocet zaznamov }
        /// Ak dany nod v stromceku nema zadefinovane zistovanie poctu, pocet zaznamov pre tento kod bude nastaveny na -1 (nezobrazovat).
        /// </summary>
        public List<TreeNodeCount> GetTreeCounts(IGetTreeCounts request)
        {
            if (request.Codes == null || request.Codes.Length == 0)
                return null;

            return Repository.GetTreeCounts(request);
        }

        public List<T> NacitatZoznam<T>(Expression<Func<T, bool>> filter, string tenantId) where T : class
        {
            return this.Repository.NacitatZoznam<T>(filter, tenantId);
        }

        public object NacitatJedinecneNazvy<T>(object request, string filter = null) where T : class
        {
            return this.Repository.NacitatJedinecneNazvy<T>(request, filter);
        }

        public object NacitatJedinecneNazvyOptimized<T>(object request) where T : class
        {
            return this.ToOptimizedResultUsingCachePublic<T>(request, () =>
            {
                return this.Repository.NacitatJedinecneNazvy<T>(request);
            });
        }

        public object NacitatFiltrovanyZoznamOptimized<T>(object request) where T : class
        {
            return this.ToOptimizedResultUsingCachePublic<T>(request, () =>
            {
                return this.Repository.NacitatFiltrovanyZoznam<T>(request);
            });
        }

        public object NacitatFiltrovanyZoznamSPrilohamiOptimized<T, TPriloha>(object request, string idColumn)
            where T : class
            where TPriloha : class
        {
            return this.ToOptimizedResultUsingCachePublic<T>(request, () =>
            {
                return this.Repository.NacitatFiltrovanyZoznamSPrilohami<T, TPriloha>(request, idColumn);
            });
        }

        /// <summary>
        /// Gets the session info.
        /// </summary>
        /// <returns></returns>
        protected object GetSessionInfo()
        {
            var dbReleased = Repository.GetNastavenieD("sys", "Deploy");
            return new
            {
                Version = Context.Info.ApplicationVersion,
                Released = Context.Info.Updated.ToString("dd.MM.yyyy HH:mm"),
                Environment = Repository.GetNastavenieS("sys", "Environment"),
                DbReleased = dbReleased.HasValue ? dbReleased.Value.ToString("dd.MM.yyyy HH:mm") : string.Empty,
                IsRedisCache = Cache is ServiceStack.Redis.RedisClientManagerCacheClient,
                Repository.Session
            };
        }

        private Dictionary<string, string> GetPropertyList(Type type)
        {
            var properties = new Dictionary<string, string>();

            foreach (PropertyInfo p in type.GetProperties())
            {
                string name = p.HasAttribute<DataMemberAttribute>() ? string.IsNullOrEmpty(p.FirstAttribute<DataMemberAttribute>().Name) ? p.Name.ToLower() : p.FirstAttribute<DataMemberAttribute>().Name.ToLower() : p.Name.ToLower();
                string dbName = p.HasAttribute<AliasAttribute>() ? p.FirstAttribute<AliasAttribute>().Name : p.Name;

                properties.AddIfNotExists(name, dbName);
            }

            return properties;
        }
        private static string RepairNodeKey(string nodeKey)
        {
            if (nodeKey.ToLower().StartsWith("all-"))
            {
                if (nodeKey.Contains("!"))
                {
                    nodeKey = HierarchyNodeExtensions.CleanKodPolozky(nodeKey);
                }
                else
                {
                    nodeKey = HostContext.ServiceName + nodeKey.Substring(3); //Namiesto "all" dám meno modulu
                }
            }
            return HierarchyNodeExtensions.RemoveParametersFromKodPolozky(nodeKey);
        }

        private bool HasParentPermissionForGeneratedNode(HierarchyNode node, List<UserNodeRight> usernoderights)
        {
            if (node.GeneratedNode && node.Parent != null)
            {
                return HasParentPermissionForGeneratedNode(node.Parent, usernoderights);
            }
            else
            {
                var kodPolozky = RepairNodeKey(node.KodPolozky);
                var userTreeRight = usernoderights.FirstOrDefault(r => r.Kod == kodPolozky);
                return userTreeRight != null && userTreeRight.Pravo != 0;
            }
        }
    }
}