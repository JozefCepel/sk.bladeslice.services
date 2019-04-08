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
    /// <summary>
    /// 
    /// </summary>
    public class ServiceBase : WebEasCoreServiceBase, IEsamService
    {
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
        public virtual new IRepositoryBase Repository
        {
            get
            {
                return (IRepositoryBase)this.repository;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            if (this.repository != null)
            {
                this.repository.Dispose();
            }
        }

        /// <summary>
        /// Udaje k polozke stromu
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public object GetList(IListDto request)
        {
            HierarchyNode node = Modules.FindNode(request.KodPolozky);
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
        /// Gets the session info.
        /// </summary>
        /// <returns></returns>
        protected object GetSessionInfo()
        {
            return new
            {
                Version = WebEas.Context.Info.ApplicationVersion,
                Released = WebEas.Context.Info.Updated.ToString("dd.MM.yyyy HH:mm"),
                Environment = this.Repository.DbEnvironment,
                DbDeployed = this.Repository.DbDeployTime == null ? null : this.Repository.DbDeployTime.Value.ToString("dd.MM.yyyy HH:mm"),
                IsRedisCache = Cache is ServiceStack.Redis.RedisClientManagerCacheClient,
                Session = Repository.Session
            };
        }

        private Dictionary<string, string> GetPropertyList(Type type)
        {
            var properties = new Dictionary<string, string>();

            foreach (PropertyInfo p in type.GetProperties())
            {
                string name = p.HasAttribute<DataMemberAttribute>() ? string.IsNullOrEmpty(p.FirstAttribute<DataMemberAttribute>().Name) ? p.Name.ToLower() : p.FirstAttribute<DataMemberAttribute>().Name.ToLower() : p.Name.ToLower();
                string dbName = p.HasAttribute<AliasAttribute>() ? p.FirstAttribute<AliasAttribute>().Name : p.Name;

                properties.Add(name, dbName);
            }

            return properties;
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

            var result = new List<TreeNodeCount>(request.Codes.Length);

            //randomize array to achieve better (quicker) results
            //because GetTreeCounts() syncs access to cache
            var random = new Random();
            foreach (var code in request.Codes.OrderBy(a => random.Next()))
            {
                int rows = -1;
                int allrows = -1;
                this.Repository.GetTreeCounts(code, out rows, out allrows);
                result.Add(new TreeNodeCount() { Count = rows, CountAll = allrows, Code = code });
            }
            return result;
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

    }
}