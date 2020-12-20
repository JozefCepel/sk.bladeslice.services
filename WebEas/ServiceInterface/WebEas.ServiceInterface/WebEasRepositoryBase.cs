using CloneExtensions;
using Ninject;
using ServiceStack;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using WebEas.Core.Base;
using WebEas.Esam.ServiceModel.Office;
using WebEas.Ninject;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Dto;
using WebEas.ServiceModel.Office.Egov.Reg.Types;
using WebEas.ServiceModel.Types;

namespace WebEas.ServiceInterface
{
    /// <summary>
    /// Repository base
    /// </summary>
    public abstract class WebEasRepositoryBase : WebEasCoreRepositoryBase, IWebEasRepositoryBase
    {

        /// <summary>
        /// Renders root node for current module
        /// </summary>
        /// <param name="kodPolozky">Toto je iba pomocna info pri renderovani stromu. Vzdy vraciame cely strom, ale napr. na tuto konkretnu polozku ide req. <see cref="DapRepository.Modul.cs"/></param>
        /// <returns>Full HierarchyNode of module</returns>
        public virtual HierarchyNode RenderModuleRootNode(string kodPolozky) => throw new NotImplementedException();

        public virtual string Code => throw new NotImplementedException();


        /// <summary>
        /// Zoznam ribbon stlpcov na ktorych sa bude robit filter na BE
        /// </summary>
        public List<string> RibbonColumnsToBEFilter { get; set; } = new List<string>();

        /// <summary>
        /// Db Collation
        /// </summary>
        public string DBCollate { get; set; } = "Latin1_General_CI_AI";

        /// <summary>
        /// Logovanie EGOV zmien (Insert, Update, Delete) do IAP
        /// </summary>
        public virtual void EgovLogChangesToIap<T>(IDbCommand dbCmd, T obj, Operation operacia, long id = 0) where T : class, IBaseEntity
        {

        }

        #region List

        /// <summary>
        /// Gets entity by Id.
        /// </summary>
        /// <param name="id">value of primary key</param>
        /// <param name="columns">if specified, returns only with selected columns</param>
        /// <returns>The data or null if not found</returns>
        public T GetById<T>(object id, params string[] columns) where T : class
        {
            if (id == null)
            {
                return null;
            }

            var primaryKeyName = typeof(T).GetPrimaryKeyName();
            if (columns != null && columns.Any())
            {
                var cols = new List<string>(columns);
                if (!cols.Contains(primaryKeyName))
                {
                    cols.Add(primaryKeyName);
                }

                return GetList(Db.From<T>().Where(string.Concat(primaryKeyName, " = {0}"), id).Select(cols.ToArray()).Limit(1)).FirstOrDefault();
            }
            
            var data = GetList<T>(new Filter(primaryKeyName, id));
            return data.FirstOrDefault();
        }

        /// <summary>
        /// Gets the record and sets the access flags.
        /// <param name="columns">if specified, returns only with selected columns</param>
        /// </summary>
        public T GetRecord<T>(object id, params string[] columns) where T : class
        {
            var rec = GetById<T>(id, columns);
            SetAccessFlag(rec);
            return rec;
        }

        /// <summary>
        /// Gets the filtered list of entities
        /// </summary>
        public List<T> GetList<T>() where T : class
        {
            return this.GetList<T>(null, null);
        }

        /// <summary>
        /// Gets the filtered list of entities
        /// </summary>
        public List<T> GetList<T>(Filter filter) where T : class
        {
            //return this.GetList<T>(filter, null); - preco kaskadovo?
            return Db.Exec((IDbCommand dbCmd) => dbCmd.GetList<T>(this, filter));
        }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public List<T> GetList<T>(Expression<Func<T, bool>> filter) where T : class
        {
            // - OrmLite ma chybnu implementaciu pre IN (), takze nemozme pouzivat :(
            //var s = new ServiceStack.OrmLite.SqlServer.SqlServerExpression<T>(this.Db.GetDialectProvider()).Where(filter);
            //return this.Db.Exec((IDbCommand dbCmd) => dbCmd.GetList<T>(this, Filter.FromSql(s.WhereExpression), null, null));
            return Db.Exec((IDbCommand dbCmd) => dbCmd.GetList<T>(this, WebEasFilterExpression.DecodeFilter(filter)));
        }


        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="filter">The filter.</param>
        /// <param name="pagging">The pagging.</param>
        /// <returns></returns>
        public List<T> GetList<T>(Filter filter = null, PaggingParameters pagging = null) where T : class
        {
            //return this.GetList<T>(filter, pagging, null); - preco kaskadovo?
            return Db.Exec((IDbCommand dbCmd) => dbCmd.GetList<T>(this, filter, pagging));
        }

        /// <summary>
        /// Gets the filtered list of entities
        /// </summary>
        /// <remarks>
        /// POZOR: tato metoda sa vola dynamicky cez MakeGenericMethod() !!! preto jej zmeny mozu znefunkcnit dane volania
        /// - volania sa daju vyhladat pomocou stringu 'GetMethod("GetList",'
        /// </remarks>
        public List<T> GetList<T>(Filter filter = null,
                                  PaggingParameters pagging = null,
                                  List<PfeSortAttribute> userSort = null,
                                  List<string> hiddenFields = null,
                                  List<string> selectedFields = null,
                                  Filter ribbonFilter = null,
                                  HierarchyNode node = null) where T : class
        {
            return Db.Exec((IDbCommand dbCmd) => dbCmd.GetList<T>(this, filter, pagging, userSort, hiddenFields, selectedFields, ribbonFilter, node));
        }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <remarks>
        /// POZOR: tato metoda sa vola dynamicky cez MakeGenericMethod() !!! preto jej zmeny mozu znefunkcnit dane volania
        /// - volania sa daju vyhladat pomocou stringu 'GetMethod("GetList",'
        /// </remarks>
        public List<T> GetList<T>(BaseListDto listDto, HierarchyNode node, PaggingParameters pagging) where T : class
        {
            if (node == null)
            {
                if (string.IsNullOrEmpty(listDto.KodPolozky))
                {
                    node = RenderModuleRootNode(Code).FindFirstNode(typeof(T));
                }
                else
                {
                    node = RenderModuleRootNode(listDto.KodPolozky).FindNode(listDto.KodPolozky);
                }
            }

            #region Check authorization

            HierarchyNode rootNode = node.GetRootNode();
            //HashSet<string> sessionRoles = this.Session.Roles;

            //if (rootNode.Kod != "reg" && !sessionRoles.Contains(string.Format("{0}_MEMBER", rootNode.Kod.ToUpper())))
            //{
            //    throw new WebEasUnauthorizedAccessException();
            //}
            //else if (rootNode.Kod == "reg")
            //{
            //    List<string> memberRoles = sessionRoles.Where(p => p.EndsWith("_MEMBER")).ToList();
            //    if (memberRoles.Count == 0)
            //    {
            //        throw new WebEasUnauthorizedAccessException();
            //    }
            //}

            #endregion

            #region Filter

            var filter = new Filter();
            Filter ribbonFilter = null;
            bool useBEFilter = (node.KodPolozky.StartsWith("pla-evp-evza")); //BE filter je iba v tychto polozkach stromu

            // ------ BaseListDto.Filter (filter from FE)
            if (!string.IsNullOrEmpty(listDto.Filter))
            {
                string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(listDto.Filter));
                List<PfeFilterUrl> decodeFilters = ServiceStack.Text.JsonSerializer.DeserializeFromString<List<PfeFilterUrl>>(decoded);

                if (decodeFilters.Count > 0)
                {
                    var decodeFiltersSQL = new List<PfeFilterUrl>();
                    var decodeFiltersBE = new List<PfeFilterUrl>();

                    foreach (var el in decodeFilters.Select(a => new { rf = a.RibbonFilter, filter = new PfeFilter(a), df = a }))
                    {
                        if (el.rf && RibbonColumnsToBEFilter.Contains(el.filter.Field) && useBEFilter)
                            decodeFiltersBE.Add(el.df);
                        else
                            decodeFiltersSQL.Add(el.df);
                    }


                    if (decodeFiltersSQL.Any())
                    {
                        var f = Filter.BuildFilter(decodeFiltersSQL, node.ModelType.GetModelDefinition(), DBCollate);
                        if (filter.IsEmpty())
                        {
                            filter = f;
                        }
                        else
                        {
                            filter.And(f);
                        }
                    }

                    if (decodeFiltersBE.Any())
                    {
                        ribbonFilter = Filter.BuildFilter(decodeFiltersBE, node.ModelType.GetModelDefinition(), null);
                    }
                }
            }
            
            // specialita, velmi specificke prepojenie gridov
            if (node.KodPolozky.Equals("rzp-prh-kmpl") || node.KodPolozky.Equals("rzp-evi-den") || node.KodPolozky.Equals("rzp-evi-zmena-pol") || node.KodPolozky.Equals("rzp-evi-navrh-pol"))
            {
                int mes = 0;
                int rok = 0;

                if (filter.FilterElements.Any())
                {
                    //Pri zložitejších podmienkach zasielan=ych z FE nemusím mať rovno FilterElement ale ešte Filter objekt
                    ExtractFilterFromMasterDetailConnection(ref mes, ref rok, filter);
                }

                if (mes > 0) //Vnúti pevný filter na stav, keďže FIN 1-12 sa napočítava iba zo zaúčtovaných dát
                {
                    if (node.KodPolozky.Equals("rzp-evi-zmena-pol"))
                    {
                        filter.And(FilterElement.LessThanOrEq("Mesiac", mes));
                        filter.And(FilterElement.In("C_StavEntity_Id", new[] { (int)StavEntityEnum.SCHVALENY, (int)StavEntityEnum.EXPORTOVANY, (int)StavEntityEnum.ODOSLANY }));  // ZMENY
                    }
                    else if (node.KodPolozky.Equals("rzp-evi-navrh-pol"))
                    {
                        filter.And(FilterElement.In("C_StavEntity_Id", new[] { (int)StavEntityEnum.SCHVALENY, (int)StavEntityEnum.EXPORTOVANY, (int)StavEntityEnum.ODOSLANY }));  // ZMENY
                    }
                    else if (node.KodPolozky.Equals("rzp-evi-den"))
                    {
                        filter.And(FilterElement.LessThanOrEq("UOMesiac", mes));
                        filter.And(FilterElement.In("C_StavEntity_Id", new[] { (int)StavEntityEnum.ZAUCTOVANY, (int)StavEntityEnum.ZAUCTOVANY_RZP}));
                        filter.And(FilterElement.In("C_TypBiznisEntity_Id", new[] { (int)TypBiznisEntityEnum.BAN, (int)TypBiznisEntityEnum.PDK, (int)TypBiznisEntityEnum.IND }));
                    }
                    else if (node.KodPolozky.Equals("rzp-prh-kmpl"))
                    {
                        filter.And(FilterElement.Eq("Obdobie", mes));
                        filter.And(FilterElement.Eq("Rok", rok));
                    }
                }
            }

            // ------ HirarchyNode.AdditionalFilter
            if (node.AdditionalFilter != null)
            {
                var f = new Filter();
                f.And(node.AdditionalFilter.Clone());
                if (filter.IsEmpty())
                {
                    filter = f;
                }
                else
                {
                    filter.And(f);
                }
            }

            // ------ HirarchyNode.ParametrizedDbFilter
            Dictionary<string, object> nodeDynamicFilter = node.ParametrizedDbFilter();
            if (nodeDynamicFilter != null)
            {
                var f = new Filter();
                foreach (KeyValuePair<string, object> val in nodeDynamicFilter)
                {
                    f.And(FilterElement.Eq(val.Key, val.Value.ToString()));
                }
                if (filter.IsEmpty())
                {
                    filter = f;
                }
                else
                {
                    filter.And(f);
                }
            }

            // ------ BaseListDto.AdditionalAttributes (another filter from FE)
            if (listDto.AdditionalAttributes != null)
            {
                var f = new Filter();
                foreach (KeyValuePair<string, string> val in listDto.AdditionalAttributes)
                {
                    f.And(FilterElement.Eq(val.Key, val.Value));
                }
                if (filter.IsEmpty())
                {
                    filter = f;
                }
                else
                {
                    filter.And(f);
                }
            }

            if (!string.IsNullOrEmpty(listDto.AdditionalFilterSql))
            {
                var f = new Filter(FilterElement.Custom(Encoding.UTF8.GetString(Convert.FromBase64String(listDto.AdditionalFilterSql))));
                if (filter.IsEmpty())
                {
                    filter = f;
                }
                else
                {
                    filter.And(f);
                }
            }


            #endregion

            #region Sort

            List<PfeSortAttribute> userSort = null;

            if (!string.IsNullOrEmpty(listDto.Sort))
            {
                byte[] filterBytes = Convert.FromBase64String(listDto.Sort);
                string resultSort = Encoding.UTF8.GetString(filterBytes);

                List<Sort> decodedSort = ServiceStack.Text.JsonSerializer.DeserializeFromString<List<Sort>>(resultSort);

                userSort = (from t in decodedSort select new PfeSortAttribute(t.Field, t.SortDirection == "1" ? PfeOrder.Asc : PfeOrder.Desc)).ToList();
            }

            #endregion

            #region Hidden fields

            List<string> hiddenFields = null;
            if (!string.IsNullOrEmpty(listDto.HiddenFields))
            {
                byte[] hiddenFieldsBytes = Convert.FromBase64String(listDto.HiddenFields);
                string resultColumnsWithData = Encoding.UTF8.GetString(hiddenFieldsBytes);
                hiddenFields = ServiceStack.Text.JsonSerializer.DeserializeFromString<List<string>>(resultColumnsWithData);
            }

            #endregion

            #region Selected fields

            List<string> selectedFields = null;
            if (!string.IsNullOrEmpty(listDto.SelectedFields))
            {
                byte[] selectedFieldsBytes = Convert.FromBase64String(listDto.SelectedFields);
                string resultColumnsWithData = Encoding.UTF8.GetString(selectedFieldsBytes);
                selectedFields = ServiceStack.Text.JsonSerializer.DeserializeFromString<List<string>>(resultColumnsWithData);
            }

            #endregion

            List<T> data = this.GetList<T>(filter, pagging, userSort, hiddenFields, selectedFields, ribbonFilter, node);
            this.SetAccessFlag(data);

            return data;
        }

        private static void ExtractFilterFromMasterDetailConnection(ref int mes, ref int rok, Filter filter)
        {
            for (int i = 0; i < filter.FilterElements.Count; i++)
            {
                if (filter.FilterElements[i] is FilterElement el)
                {
                    if (Convert.ToString(el.Value).Contains("R@"))
                    {
                        string stmp = el.Value.ToString().Split('|')[1];
                        rok = stmp.Substring(0, stmp.Length - 2).ToInt();
                        el.Value = el.Value.ToString().Replace($"|{ rok }R@", "");
                        //Odseparoval som Rok. Mesiac buď bude tiež ako parameter, alebo sa má nastaviť na mesiac k dnešnému dňu
                        mes = rok == DateTime.Now.Year ? DateTime.Now.Month : rok < DateTime.Now.Year ? 12 : 1;
                    }
                    if (Convert.ToString(el.Value).Contains("M@"))
                    {
                        string stmp = el.Value.ToString().Split('|')[1];
                        mes = stmp.Substring(0, stmp.Length - 2).ToInt();
                        el.Value = el.Value.ToString().Replace($"|{ mes }M@", "");
                    }
                }
                else
                {
                    ExtractFilterFromMasterDetailConnection(ref mes, ref rok, (Filter)filter.FilterElements[i]);
                }
            }
        }

        /// <summary>
        /// Gets the list using an sqlexpression.
        /// </summary>
        public List<T> GetList<T>(SqlExpression<T> expression) where T : class
        {
            var records = Db.Select(expression);

            if (records is IEnumerable<IBaseEntity>)
            {
                foreach (var be in records as IEnumerable<IBaseEntity>)
                {
                    be.DefaultRecord = (be as T).GetClone();
                }
            }

            if (typeof(T).HasInterface(typeof(IAfterGetList)))
            {
                ((IAfterGetList)Activator.CreateInstance(typeof(T))).AfterGetList(this, ref records, null);
            }

            //Zatial nedavam, pouziva sa na vnutorne volania
            //SetAccessFlag(records);

            return records;
        }

        /// <summary>
        /// Gets the record.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public IBaseEntity GetById(IDto data)
        {
            if (data.GetType().GetProperties().Any(nav => nav.HasAttribute<PrimaryKeyAttribute>()))
            {
                PropertyInfo source = data.GetType().GetProperties().First(nav => nav.HasAttribute<PrimaryKeyAttribute>());
                object id = source.GetValue(data);

                if (id == null || (id is int && ((int)id) == 0) || (id is long && ((long)id) == 0L) || (id is short && ((short)id) == (short)0))
                {
                    throw new WebEasValidationException(string.Format("Wrong primary key : {0}", id), "Chýba identifikátor záznamu");
                }

                var originalDb = (IBaseEntity)this.GetType().GetMethod("GetById", new Type[] { typeof(object), typeof(string[]) }).MakeGenericMethod(data.EntityType).Invoke(this, new object[] { id, null });

                if (originalDb == null)
                {
                    throw new WebEasValidationException(null, "Pôvodný záznam v databáze nebol nájdený");
                }
                return originalDb;
            }
            else
            {
                throw new WebEasException(null, string.Format("Nie je definovaný primárny kľúč v dto {0}", data.GetType()));
            }
        }

        /// <summary>
        /// Gets the record filled.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public IBaseEntity GetByIdFilled(IDto data)
        {
            return data.GetEntity(this.GetById(data));
        }

        /// <summary>
        /// Gets the by id filled.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="data">The data.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public T GetByIdFilled<T>(IDto data, object id) where T : class, IBaseEntity
        {
            return (T)data.GetEntity(this.GetById<T>(id));
        }

        /// <summary>
        /// Access flag Setup
        /// </summary>
        /// <param name="viewData"></param>
        public virtual void SetAccessFlag(object viewData)
        {
        }

        /// <summary>
        /// Returns count of rows in database (for specified data type using specified filter)
        /// </summary>
        /// <remarks>
        /// POZOR: tato metoda sa vola dynamicky cez MakeGenericMethod() !!! preto jej zmeny mozu znefunkcnit dane volania
        /// - volania sa daju vyhladat pomocou stringu 'GetMethod("Count",'
        /// </remarks>
        public List<TreeNodeCount> Count<T>(Filter filter, string[] codes) where T : class
        {
            return this.Db.Exec((IDbCommand dbCmd) => dbCmd.GetCount<T>(this, filter, codes));
        }

        #endregion

        #region Create

        /// <summary>
        /// Creates the specified data.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public T Create<T>(IDto data)
            where T : class, IBaseEntity, new()
        {
            data.Validate(this);

            long id = -1;
            using (var transaction = GetActiveTransaction() == null ? BeginTransaction() : null)
            {
                try
                {
                    IBaseEntity entity = data.GetEntity();
                    id = this.InsertData(entity);

                    if (transaction != null)
                    {
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    
                    throw ex;
                }
            }

            if (id > 0)
            {
                return this.GetRecord<T>(id);
            }
            throw new WebEasException(null, "Chyba pri získavaní id záznamu");
        }

        /// <summary>
        /// Creates the row.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public long Create<T>(T obj) where T : class, IBaseEntity
        {
            long retval;

            using (var transaction = GetActiveTransaction() == null ? BeginTransaction() : null)
            {
                try
                {
                    retval = InsertData(obj);

                    if (transaction != null)
                    {
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }

                    throw ex;
                }
            }

            return retval;
        }

        /// <summary>
        /// Inserts the data.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="obj">The obj.</param>
        public long InsertData<T>(T obj) where T : class, IBaseEntity
        {
            return this.Db.Exec((IDbCommand dbCmd) => dbCmd.InsertData<T>(this, obj));
        }

        /// <summary>
        /// Inserts the data.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="objs">The objs.</param>
        public void InsertData<T>(params T[] objs) where T : class, IBaseEntity
        {
            this.Db.Exec((IDbCommand dbCmd) => dbCmd.InsertData<T>(this, objs));
        }

        /// <summary>
        /// Inserts all data.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="objs">The objs.</param>
        public void InsertAllData<T>(IEnumerable<T> objs) where T : class, IBaseEntity
        {
            this.Db.Exec((IDbCommand dbCmd) => dbCmd.InsertAllData<T>(this, objs));
        }

        #endregion

        #region Update

        /// <summary>
        /// Updates the row with transaction.
        /// Note: Use UpdateData if you have own transaction;
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="obj">The obj.</param>
        public int Update<T>(T obj) where T : class, IBaseEntity
        {
            int retval;

            using (var transaction = BeginTransaction())
            {
                try
                {
                    retval = this.UpdateData<T>(obj);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }

            return retval;
        }

        /// <summary>
        /// This method is declared to be used by dynamic invoke, as UpdateData() can not be :(
        /// </summary>
        private void UpdateDynamic<T>(T data) where T : class, IBaseEntity, new()
        {
            this.UpdateData(data);
        }

        /// <summary>
        /// Updates the only data binded from dto
        /// </summary>
        /// <param name="data">The data.</param>
        public void UpdateOnly(IDto data)
        {
            IBaseEntity entity = this.GetByIdFilled(data);

            //this.UpdateData(entity);
            typeof(WebEasRepositoryBase).GetMethod("UpdateDynamic", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(entity.GetType()).Invoke(this, new object[] { entity });
        }

        /// <summary>
        /// Updates the specified data.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public T Update<T>(IDto data)
            where T : class, IBaseEntity, new()
        {
            data.Validate(this);

            object id = null;

            using (var transaction = GetActiveTransaction() == null ? BeginTransaction() : null)
            {
                try
                {
                    IBaseEntity entity = this.GetByIdFilled(data);
                    id = entity.GetId();

                    //this.UpdateData(entity);
                    typeof(WebEasRepositoryBase).GetMethod("UpdateDynamic", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(entity.GetType()).Invoke(this, new object[] { entity });

                    if (transaction != null)
                    {
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }

                    throw ex;
                }
            }

            if (id != null)
            {
                return this.GetRecord<T>(id);
            }
            return null;
        }

        /// <summary>
        /// Updates the data without transaction.
        /// Note: Use Update() to run it in transaction.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="obj">The obj.</param>
        public int UpdateData<T>(T obj) where T : class, IBaseEntity
        {
            return Db.Exec((IDbCommand dbCmd) => dbCmd.UpdateData<T>(this, obj));
        }

        /// <summary>
        /// Bulk data update (no transaction)
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="objs">The objs.</param>
        public void UpdateData<T>(params T[] objs) where T : class, IBaseEntity
        {
            this.Db.Exec((IDbCommand dbCmd) => dbCmd.UpdateData(this, objs));
        }

        /// <summary>
        /// Updates all data (no transaction)
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="objs">The objs.</param>
        public void UpdateAllData<T>(IEnumerable<T> objs) where T : class, IBaseEntity
        {
            this.Db.Exec((IDbCommand dbCmd) => dbCmd.UpdateAllData<T>(this, objs));
        }

        #endregion

        #region Delete

        /// <summary>
        /// Deletes the entity by Id.
        /// </summary>
        public int Delete<T>(params object[] id) where T : class, IBaseEntity
        {
            int retval;
            IEnumerable ids = id.First() is IEnumerable ? (IEnumerable)id.First() : id;

            if (typeof(T).HasAttribute<AliasAttribute>() && typeof(T).FirstAttribute<AliasAttribute>().Name.ToUpper().StartsWith("C_"))
            {
                using var transaction = GetActiveTransaction() == null ? BeginTransaction() : null;
                try
                {
                    retval = this.Db.Exec((IDbCommand dbCmd) => dbCmd.DeleteData<T>(this, false, ids));
                    if (transaction != null)
                    {
                        transaction.Commit();
                    }

                    return retval;
                }
                catch (WebEasValidationException ex)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }

                    throw ex;
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }

                    Log.Info("Error in deleting data", ex);
                }
            }

            using (var transaction = GetActiveTransaction() == null ? BeginTransaction() : null)
            {
                try
                {
                    retval = this.Db.Exec((IDbCommand dbCmd) => dbCmd.DeleteData<T>(this, true, ids));
                    if (transaction != null)
                    {
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    
                    throw ex;
                }
            }

            return retval;
        }

        /// <summary>
        /// Deletes by id.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="objs">The id.</param>
        public int DeleteData<T>(params object[] id) where T : class, IBaseEntity
        {
            IEnumerable ids = id.First() is IEnumerable ? (IEnumerable)id.First() : id;
            if (typeof(T).HasAttribute<AliasAttribute>() && typeof(T).FirstAttribute<AliasAttribute>().Name.ToUpper().StartsWith("C_"))
            {
                try
                {
                    return Db.Exec((IDbCommand dbCmd) => dbCmd.DeleteData<T>(this, false, ids));
                }
                catch (Exception ex)
                {
                    Log.Info("Error in deleting data", ex);
                }
            }
            return Db.Exec((IDbCommand dbCmd) => dbCmd.DeleteData<T>(this, true, ids));
        }

        #endregion

        #region Procedure

        /// <summary>
        /// SQLs the procedure.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public DynamicParameters SqlProcedure(string name, DynamicParameters parameters, int? commandTimeout = null)
        {
            var sqlParmText = new StringBuilder();
            try
            {
                sqlParmText.Append("Sql. parametre: ");
                foreach (var meno in parameters.ParameterNames)
                {
                    var pValue = parameters.Get<dynamic>(meno);
                    if (pValue != null)
                    {
                        sqlParmText.AppendFormat("'{0}'='{1}', ", meno, pValue.ToString());
                    }
                }
                sqlParmText.Length--;

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                Db.Execute(name, parameters, transaction: ((OrmLiteConnection)Db).Transaction, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);
                stopwatch.Stop();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                //chyba s cislom 50000 indikuje ze to bolo z procky cez raiserror('text'...)
                if (ex.Number == 50000)
                {
                    // aj tak si radsej zalogujme chybu
                    Log.Error(string.Format("Validacna chyba - Nastala chyba pri volaní SQL procedúry {0} - {1} {2}", name, ex.Message, sqlParmText.ToString()), ex);
                    throw new WebEasValidationException(string.Format("Nastala chyba pri volaní SQL procedúry {0}", name), ex.Message, ex, sqlParmText.ToString());
                }
                else
                {
                    throw new WebEasException(ex.Message, null, ex, sqlParmText.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new WebEasException(ex.Message, null, ex, sqlParmText.ToString());
            }

            return parameters;
        }

        #endregion

        #region Transaction

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <returns></returns>
        public IDbTransaction BeginTransaction()
        {
            return Db.OpenTransaction(IsolationLevel.ReadCommitted);
        }

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <returns></returns>
        public IDbTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return Db.OpenTransaction(isolationLevel);
        }

        /// <summary>
        /// Gets the active transaction.
        /// </summary>
        /// <returns></returns>
        public IDbTransaction GetActiveTransaction()
        {
            if (this.Db is OrmLiteConnection && ((OrmLiteConnection)this.Db).Transaction != null)
            {
                return ((OrmLiteConnection)this.Db).Transaction;
            }
            return null;
        }

        #endregion

        #region Logovanie zmien

        /// <summary>
        /// Check if there is configured logging on some column in specified table (reg.C_LoggingConfig)
        /// </summary>
        public bool IsTableLogged(string schema, string table)
        {
            if (string.IsNullOrEmpty(schema) || string.IsNullOrEmpty(table))
            {
                return false;
            }
            Dictionary<string, int> tableLogs = this.GetCacheOptimizedTenant("db:columnslog", () =>
            {
                string sql = "SELECT LOWER([Schema] + '.' + [NazovTabulky]) as [Key], COUNT(*) as [Value] FROM [reg].[C_LoggingConfig] WHERE (DatumPlatnosti is null or DatumPlatnosti > getdate()) And (D_Tenant_Id = @tenant OR D_Tenant_Id IS NULL) GROUP BY LOWER([Schema] + '.' + [NazovTabulky])";

                return this.Db.Dictionary<string, int>(sql, new { tenant = this.Session.TenantId });
            });

            string key = string.Format("{0}.{1}", schema, table).ToLowerInvariant();
            return tableLogs.ContainsKey(key);
            //return this.Db.Scalar<int>(
            //	"SELECT COUNT(*) FROM reg.C_LoggingConfig WHERE (D_Tenant_Id = @tenant OR D_Tenant_Id IS NULL) AND [Schema] = @schema AND [NazovTabulky] = @table",
            //	new { tenant = this.Session.TenantId, schema = schema, table = table }) > 0;
        }

        /// <summary>
        /// Check if there is configured logging on some column in specified tables via attribute list (reg.C_LoggingConfig)
        /// </summary>
        public bool IsTableLogged(SourceTableAttribute[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (this.IsTableLogged(list[i].Schema, list[i].Table))
                {
                    return true;
                }
            }
            //not found
            return false;
        }

        /// <summary>
        /// Get list of changes for specified row Id in specified node.
        /// Returns null if logging is not defined for model under given node.
        /// Throws exception in case node not found.
        /// </summary>
        /// <param name="code">Kod polozky</param>
        /// <param name="rowId">Unique row ID (source data primary key)</param>
        public List<LoggingView> GetTableLogging(string code, long rowId)
        {
            HierarchyNode node = RenderModuleRootNode(code).FindNode(code);
            SourceTableAttribute[] stAttrib = node.ModelType.AllAttributes<SourceTableAttribute>();
            if (stAttrib == null || stAttrib.Length == 0)
            {
                return null;
            }

            Filter filter = null;

            if (stAttrib.Any(a => !string.IsNullOrEmpty(a.PrimaryKey)))
            {
                //kedze mame inu hodnotu pre primary key, musim si nacitat zaznam...
                object data = this.GetType().GetMethod("GetById", new Type[] { typeof(object), typeof(string[]) }).MakeGenericMethod(node.ModelType).Invoke(this, new object[] { rowId, null });
                if (data == null)
                {
                    return null; //alebo throw? :)
                }
                filter = new Filter();
                for (int i = 0; i < stAttrib.Length; i++)
                {
                    long realId = rowId;
                    if (!string.IsNullOrEmpty(stAttrib[i].PrimaryKey))
                    {
                        object value = node.ModelType.GetProperty(stAttrib[i].PrimaryKey).GetValue(data, null);
                        if (value == null)
                        {
                            continue;
                        }
                        else
                        {
                            realId = long.Parse(value.ToString());
                        }
                    }
                    filter.Or(Filter.AndElements(FilterElement.Eq("Row_Id", realId.ToString(), PfeDataType.Number), FilterElement.Eq("Schema", stAttrib[i].Schema), FilterElement.Eq("NazovTabulky", stAttrib[i].Table)));
                }
            }
            else if (stAttrib.Length == 1) //najjednoduchsi pripad :)
            {
                filter = Filter.AndElements(FilterElement.Eq("Row_Id", rowId.ToString(), PfeDataType.Number), FilterElement.Eq("Schema", stAttrib[0].Schema), FilterElement.Eq("NazovTabulky", stAttrib[0].Table));
            }
            else //jednoduchsi pripad, rowId je spolocne pre vsetky
            {
                filter = new Filter("Row_Id", rowId);
                var subFilter = new Filter();
                for (int i = 0; i < stAttrib.Length; i++)
                {
                    subFilter.Or(Filter.AndElements(FilterElement.Eq("Schema", stAttrib[i].Schema), FilterElement.Eq("NazovTabulky", stAttrib[i].Table)));
                }
                filter.And(subFilter);
            }

            var config = new List<LoggingConfig>();
            foreach (var sc in stAttrib)
            {
                config.AddRange(GetList<LoggingConfig>(x => x.Schema == sc.Schema && x.NazovTabulky == sc.Table));
            }

            var list = GetList<LoggingView>(filter);
            list.ForEach(x =>
            {
                if (!string.IsNullOrEmpty(x.NazovStlpca))
                {
                    var defaultColName = x.NazovStlpca;

                    #region Search
                    if (config.Any(nav =>
                       string.Equals(nav.Schema, x.Schema, StringComparison.CurrentCultureIgnoreCase) &&
                       string.Equals(nav.NazovTabulky, x.NazovTabulky, StringComparison.CurrentCultureIgnoreCase) &&
                       string.Equals(nav.NazovStlpca, x.NazovStlpca, StringComparison.CurrentCultureIgnoreCase) &&
                       nav.TypStlpca == "SEARCH"))
                    {
                        var prop = node.ModelType.AllProperties().Where(nav => nav.HasAttribute<PfeColumnAttribute>() && nav.GetCustomAttribute<PfeColumnAttribute>().NameField == x.NazovStlpca).FirstOrDefault();
                        if (prop != null)
                        {
                            var pfeCol = prop.FirstAttribute<PfeColumnAttribute>();
                            x.NazovStlpca = pfeCol.Text;

                            if (pfeCol.Xtype == PfeXType.SearchCompany || pfeCol.Xtype == PfeXType.SearchPerson || pfeCol.Xtype == PfeXType.SearchPersonCompany)
                            {

                                if (Guid.TryParse(x.PovodnaHodnota, out Guid idOsoby))
                                {
                                    x.PovodnaHodnota = Db.Scalar<string>("select reg.F_Osoba_FormatovanePriezviskoMeno(@id)", new { id = idOsoby });
                                }

                                if (Guid.TryParse(x.NovaHodnota, out idOsoby))
                                {
                                    x.NovaHodnota = Db.Scalar<string>("select reg.F_Osoba_FormatovanePriezviskoMeno(@id)", new { id = idOsoby });
                                }
                            }

                            if (pfeCol.Xtype == PfeXType.SearchAddress)
                            {
                                x.PovodnaHodnota = Db.Scalar<string>("select reg.F_AdresaSK(@id)", new { id = x.PovodnaHodnota });
                                x.NovaHodnota = Db.Scalar<string>("select reg.F_AdresaSK(@id)", new { id = x.NovaHodnota });
                            }
                        }
                    }
                    #endregion

                    #region Combo
                    if (config.Any(nav =>
                       string.Equals(nav.Schema, x.Schema, StringComparison.CurrentCultureIgnoreCase) &&
                       string.Equals(nav.NazovTabulky, x.NazovTabulky, StringComparison.CurrentCultureIgnoreCase) &&
                       string.Equals(nav.NazovStlpca, x.NazovStlpca, StringComparison.CurrentCultureIgnoreCase) &&
                       nav.TypStlpca == "COMBO"))
                    {
                        var prop = node.ModelType.AllProperties().Where(nav => nav.HasAttribute<PfeComboAttribute>() && nav.GetCustomAttribute<PfeComboAttribute>().ComboIdColumn == x.NazovStlpca).FirstOrDefault();
                        if (prop != null)
                        {
                            var pfeCol = prop.FirstAttribute<PfeColumnAttribute>();
                            if (pfeCol != null)
                            {
                                x.NazovStlpca = pfeCol.Text;
                            }

                            var comboList = GetListCombo(node.ModelType, prop.Name).Select(y => y.ToJson().FromJson<ComboResult>());
                            if (comboList.Any(z => z.Id == x.PovodnaHodnota))
                            {
                                x.PovodnaHodnota = comboList.FirstOrDefault(z => z.Id == x.PovodnaHodnota).Value;
                            }

                            if (comboList.Any(z => z.Id == x.NovaHodnota))
                            {
                                x.NovaHodnota = comboList.FirstOrDefault(z => z.Id == x.NovaHodnota).Value;
                            }
                        }
                    }
                    #endregion

                    PfeColumnAttribute pfeColStlpca = null;
                    if (defaultColName == x.NazovStlpca)
                    {
                        var property = node.ModelType.GetProperty(x.NazovStlpca);
                        if (property != null)
                        {
                            pfeColStlpca = property.FirstAttribute<PfeColumnAttribute>();
                            if (pfeColStlpca != null)
                            {
                                x.NazovStlpca = pfeColStlpca.Text;
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(x.PopisZmeny))
                    {
                        if (pfeColStlpca != null)
                        {
                            if (pfeColStlpca.Type == PfeDataType.Date)
                            {
                                if (x.PovodnaHodnota != null && DateTime.TryParse(x.PovodnaHodnota, out DateTime dtPovodnaHodnota))
                                {
                                    x.PovodnaHodnota = dtPovodnaHodnota.ToShortDateString();
                                }

                                if (x.NovaHodnota != null && DateTime.TryParse(x.NovaHodnota, out DateTime dtNovaHodnota))
                                {
                                    x.NovaHodnota = dtNovaHodnota.ToShortDateString();
                                }
                            }
                        }
                        x.PopisZmeny = string.Format("Zmena hodnoty stĺpca '{0}' z \"{1}\" na \"{2}\"", x.NazovStlpca, x.PovodnaHodnota, x.NovaHodnota);
                    }
                }
            });

            return list;
        }

        #endregion

        #region Gzip Compress

        public byte[] Zip(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);

            using var msi = new MemoryStream(bytes);
            using var mso = new MemoryStream();
            using (var gs = new GZipStream(mso, CompressionMode.Compress))
            {
                msi.CopyTo(gs);
            }

            return mso.ToArray();
        }

        public string Unzip(byte[] bytes)
        {
            using var msi = new MemoryStream(bytes);
            using var mso = new MemoryStream();
            using (var gs = new GZipStream(msi, CompressionMode.Decompress))
            {
                gs.CopyTo(mso);
            }

            return Encoding.UTF8.GetString(mso.ToArray());
        }

        #endregion

        #region List Combo - zoznam do comba

        public List<object> GetListCombo(IListComboDto request)
        {
            HierarchyNode node = RenderModuleRootNode(request.KodPolozky).FindNode(request.KodPolozky);
            string[] requestFields = String.IsNullOrEmpty(request.RequiredField) ? null : request.RequiredField.Split('/');
            return this.GetListCombo(node.ModelType, request.Column, request.KodPolozky, requestFields);
        }

        /// <summary>
        /// Lists the combo.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        private List<object> GetListCombo(Type modelType, string col, string kodPolozky = null, string[] requestFields = null)
        {
            PropertyInfo column = modelType.Properties().FirstOrDefault(nav => nav.Name.ToLower() == col.ToLower());
            if (column.HasAttribute<PfeColumnAttribute>() && column.HasAttribute<PfeComboAttribute>())
            {
                PfeComboAttribute combo = column.FirstAttribute<PfeComboAttribute>();
                if (combo.TableType == null && column.HasAttribute<ReferencesAttribute>())
                {
                    combo.TableType = column.FirstAttribute<ReferencesAttribute>().Type;
                }

                string[] requiredFields = column.FirstAttribute<PfeColumnAttribute>().RequiredFields;
                if (combo.TableType != null && combo.TableType.HasInterface(typeof(IStaticCombo)))
                {
                    var sc = (IStaticCombo)Activator.CreateInstance(combo.TableType);
                    sc.Repository = this;
                    sc.RequiredFields = (requiredFields == null || requiredFields.Length == 0) ? null : GetRequiredValue(requiredFields);
                    sc.KodPolozky = kodPolozky;
                    return sc.GetComboList(requestFields).ToList<object>();
                }


                if (requiredFields == null || requiredFields.Length == 0)
                {
                    return this.ListCombo(combo, null, col, kodPolozky);
                }

                if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.QueryString.AllKeys.Any(nav => requiredFields.Any(r => r.ToLower() == nav.ToLower())))
                {
                    var param = new Dictionary<string, string>();
                    foreach (string key in HttpContext.Current.Request.QueryString.AllKeys)
                    {
                        if (requiredFields.Any(nav => nav.ToLower() == key.ToLower()))
                        {
                            string name = requiredFields.First(nav => nav.ToLower() == key.ToLower());
                            string value = HttpContext.Current.Request.QueryString[key];
                            if (!String.IsNullOrEmpty(value) && value.ToLower() != "null")
                            {
                                param.Add(name, value);
                            }
                        }
                    }
                    return this.ListCombo(combo, param, col, kodPolozky);
                }

                if (requestFields != null && requestFields.Length > 0 && requiredFields.Length <= requestFields.Length)
                {
                    var param = new Dictionary<string, string>();
                    for (int i = 0; i < requiredFields.Length; i++)
                    {
                        param.Add(requiredFields[i], requestFields[i]);
                    }

                    return this.ListCombo(combo, param, col, kodPolozky);
                }
                return this.ListCombo(combo, null, col, kodPolozky);
                //throw new WebEasException(
                //    string.Format("Count of required fields mismatch expected {0}, received {1}",
                //        requiredFields != null && requiredFields.Length > 0 ? requiredFields.Join(",") : "(none)",
                //        requestFields != null && requestFields.Length > 0 ? requestFields.Join(",") : "(none)"),
                //    "Zle volanie služby - nesprávny počet parametrov!");                
            }

            if (column.HasAttribute<PfeComboAttribute>())
            {
                PfeComboAttribute combo = column.FirstAttribute<PfeComboAttribute>();
                if (combo.TableType == null && column.HasAttribute<ReferencesAttribute>())
                {
                    combo.TableType = column.FirstAttribute<ReferencesAttribute>().Type;
                }

                return this.ListCombo(combo, null, col, kodPolozky);
            }
            else if (column.HasAttribute<ReferencesAttribute>())
            {
                Type type = column.FirstAttribute<ReferencesAttribute>().Type;

                return this.ListCombo(new PfeComboAttribute(type), null, col, kodPolozky);
            }
            else
            {
                throw new WebEasException(String.Format("Column {0} has not defined combo", col));
            }
        }

        /// <summary>
        /// Lists the combo.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <param name="requiredFields">The required fields.</param>
        /// <returns></returns>
        private List<object> ListCombo(PfeComboAttribute attribute, Dictionary<string, string> requiredFields, string col, string kodPolozky)
        {
            string sql = string.Empty;

            if (!string.IsNullOrEmpty(attribute.Sql))
            {
                sql = attribute.Sql;
                if (requiredFields != null && requiredFields.Count > 0)
                {
                    var parameters = new Dictionary<string, object>();
                    foreach (KeyValuePair<string, string> item in requiredFields)
                    {
                        parameters.AddAndChangeNameIfExists(item.Key, item.Value);
                    }

                    return Db.Select<ComboResult>(sql, parameters).ToList<object>();
                }
                else
                {
                    return Db.Select<ComboResult>(sql).ToList<object>();
                }
            }
            else
            {
                if (attribute.TableType == null)
                {
                    throw new ArgumentNullException("Table type not defined");
                }

                try
                {
                    if (typeof(IPfeCustomizeCombo).IsAssignableFrom(attribute.TableType))
                    {
                        object modelObject = Activator.CreateInstance(attribute.TableType);

                        if (modelObject is IPfeCustomizeCombo)
                        {
                            ((IPfeCustomizeCombo)modelObject).ComboCustomize(this, col, kodPolozky, ref attribute);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new WebEasException("Customize Pfe Combo", ex);
                }

                string ten = "";

                bool isTenantEntity = attribute.TableType.HasInterface(typeof(ITenantEntity));
                bool isTenantEntityNullable = attribute.TableType.HasInterface(typeof(ITenantEntityNullable));

                if (isTenantEntity || isTenantEntityNullable)
                {
                    ten = $":{Session.TenantId}";
                }

                string alias = $"[{(attribute.TableType.HasAttribute<AliasAttribute>() ? attribute.TableType.FirstAttribute<AliasAttribute>().Name : attribute.TableType.Name)}]";

                string schema = attribute.TableType.HasAttribute<SchemaAttribute>() ? $"[{attribute.TableType.FirstAttribute<SchemaAttribute>().Name}]." : "";

                if (string.IsNullOrEmpty(attribute.ComboIdColumn))
                {
                    PropertyInfo keyCol = attribute.TableType.Properties().FirstOrDefault(nav => nav.HasAttribute<PrimaryKeyAttribute>());
                    if (keyCol == null)
                    {
                        throw new ArgumentNullException($"Primary key not defined on table {attribute.TableType.Name}");
                    }

                    attribute.ComboIdColumn = keyCol.HasAttribute<AliasAttribute>() ? keyCol.FirstAttribute<AliasAttribute>().Name : keyCol.Name;
                }

                if (string.IsNullOrEmpty(attribute.ComboDisplayColumn))
                {
                    IEnumerable<PropertyInfo> properties = attribute.TableType.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance).Where(nav => !nav.HasAttribute<PrimaryKeyAttribute>());
                    PropertyInfo valueCol = null;

                    if (properties.Any(nav => nav.HasAttribute<PfeValueColumnAttribute>()))
                    {
                        valueCol = properties.First(nav => nav.HasAttribute<PfeValueColumnAttribute>());
                    }
                    else if (attribute.TableType.GetProperties().Any(nav => nav.HasAttribute<PfeValueColumnAttribute>()))
                    {
                        valueCol = attribute.TableType.GetProperties().First(nav => nav.HasAttribute<PfeValueColumnAttribute>());
                    }
                    else if (properties.Any(nav => nav.Name.ToLower().ContainsAny("nazov", "name", "popis")))
                    {
                        valueCol = properties.First(nav => nav.Name.ToLower().ContainsAny("nazov", "name", "popis"));
                    }
                    else
                    {
                        valueCol = properties.Any(nav => nav.PropertyType == typeof(string)) ? properties.First(nav => nav.PropertyType == typeof(string)) : properties.First();
                    }

                    attribute.ComboDisplayColumn = valueCol.HasAttribute<PfeValueColumnAttribute>() && valueCol.FirstAttribute<PfeValueColumnAttribute>().HasSql() ? valueCol.FirstAttribute<PfeValueColumnAttribute>().Sql : valueCol.HasAttribute<AliasAttribute>() ? valueCol.FirstAttribute<AliasAttribute>().Name : valueCol.Name;
                }

                var sqlWhere = new StringBuilder();
                var parameters = new Dictionary<string, object>();

                if ((isTenantEntity || isTenantEntityNullable) && this.Session.TenantId != null)
                {
                    if (isTenantEntity)
                    {
                        sqlWhere.AppendFormat("(D_Tenant_Id = @{0})", parameters.AddAndChangeNameIfExists("D_Tenant_Id", this.Session.TenantIdGuid));
                    }
                    else
                    {
                        sqlWhere.AppendFormat("(D_Tenant_Id = @{0} OR D_Tenant_Id is null)", parameters.AddAndChangeNameIfExists("D_Tenant_Id", this.Session.TenantIdGuid));
                    }
                }
                else if ((isTenantEntity || isTenantEntityNullable) && this.Session.TenantId == null)
                {
                    sqlWhere.Append("D_Tenant_Id is null");
                }

                if (requiredFields != null && requiredFields.Count > 0)
                {
                    foreach (KeyValuePair<string, string> item in requiredFields)
                    {
                        if (attribute.TableType.GetProperties().Where(nav => nav.HasAttribute<AliasAttribute>()).Any(x => x.GetCustomAttribute<AliasAttribute>().Name == item.Key) ||
                            attribute.TableType.GetProperties().Any(x => x.Name == item.Key))
                        {
                            // Ak sa parameter nachadza v AdditionalWhereSql tak ho neposielame dalej na filtrovaciu podmienku
                            if ((attribute.AdditionalWhereSql ?? string.Empty).Contains(string.Concat("@", item.Key)))
                            {
                                continue;
                            }

                            string addedKey;
                            if (item.Value.Length > 20 && DateTime.TryParseExact(item.Value.Insert(20, "+"), "yyyy-MM-ddTHH:mm:ss zzz", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime dateTime))
                            {
                                addedKey = parameters.AddAndChangeNameIfExists(item.Key, dateTime);
                            }
                            else
                                addedKey = parameters.AddAndChangeNameIfExists(item.Key, item.Value);

                            string field = $"[{item.Key}] = @{addedKey}";
                            if (sqlWhere.Length > 0)
                            {
                                sqlWhere.AppendFormat(" AND {0}", field);
                            }
                            else
                            {
                                sqlWhere.Append(field);
                            }
                        }

                    }
                }

                if (!string.IsNullOrEmpty(attribute.AdditionalWhereSql))
                {
                    if (attribute.AdditionalWhereSql.Contains("@"))
                    {
                        foreach (var parameterName in Regex.Matches(attribute.AdditionalWhereSql, @"\@\w+").Cast<Match>().Select(m => m.Value.Remove(0, 1)).Distinct())
                        {
                            if (requiredFields != null && requiredFields.ContainsKey(parameterName))
                            {
                                if (requiredFields[parameterName].Length > 20 && DateTime.TryParseExact(requiredFields[parameterName].Insert(20, "+"), "yyyy-MM-ddTHH:mm:ss zzz", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime dateTime))
                                {
                                    parameters.AddAndChangeNameIfExists(parameterName, dateTime);
                                }
                                else
                                    parameters.AddAndChangeNameIfExists(parameterName, requiredFields[parameterName]);
                            }
                            else
                            {
                                if (!parameters.ContainsKey(parameterName.ToUpper()))
                                {
                                    parameters.AddAndChangeNameIfExists(parameterName, null);
                                }
                            }
                        }
                    }

                    if (sqlWhere.Length > 0)
                    {
                        sqlWhere.AppendFormat(" AND {0}", attribute.AdditionalWhereSql);
                    }
                    else
                    {
                        sqlWhere.Append(attribute.AdditionalWhereSql);
                    }
                }

                string sqlSelect = $"SELECT [{attribute.ComboIdColumn}],[{attribute.ComboDisplayColumn}]";
                if (attribute.ComboIdColumn.ToLower() == attribute.ComboDisplayColumn.ToLower())
                {
                    sqlSelect = $"SELECT [{attribute.ComboIdColumn}]";
                }
                
                var sqlSelectPart = new StringBuilder();
                var sqlOrderPart = new StringBuilder();

                if (attribute.AdditionalFields != null && attribute.AdditionalFields.Length > 0)
                {
                    ModelDefinition modelDefinition = attribute.TableType.GetModelDefinition();
                    List<FieldDefinition> fieldDefinition = modelDefinition.FieldDefinitions;

                    foreach (FieldDefinition def in fieldDefinition.Where(x => attribute.AdditionalFields.Contains(x.FieldName)))
                    {
                        if (sqlSelectPart.Length > 0)
                        {
                            sqlSelectPart.Append(",");
                        }

                        sqlSelectPart.AppendFormat("[{0}]", def.FieldName);
                    }
                }

                if (attribute.TableType.HasInterface(typeof(IOrsPravo)) && attribute.FilterByOrsPravo)
                {
                    sqlWhere.Append(" AND OrsPravo > 1");
                }

                if (sqlSelectPart.Length > 0)
                {
                    sqlSelect = string.Concat(sqlSelect, ", ", sqlSelectPart);
                }

                if (string.IsNullOrEmpty(attribute.CustomSortSqlExp))
                {
                    sqlOrderPart.AppendFormat("[{0}]", attribute.ComboDisplayColumn);
                }
                else
                {
                    sqlOrderPart.Append(attribute.CustomSortSqlExp);
                }

                if (sqlWhere.Length > 0)
                {
                    sql = $"{sqlSelect} FROM {schema}{alias} WHERE {sqlWhere} AND [{attribute.ComboDisplayColumn}] IS NOT NULL ORDER BY {sqlOrderPart}";
                }
                else
                {
                    sql = $"{sqlSelect} FROM {schema}{alias} WHERE [{attribute.ComboDisplayColumn}] IS NOT NULL ORDER BY {sqlOrderPart}";
                }

                object CreateExpandoObject(object obj)
                {
                    var dynamicObject = new ExpandoObject() as IDictionary<string, object>;
                    dynamicObject.Add("id", obj.GetPi(attribute.ComboIdColumn).GetValue(obj));
                    dynamicObject.Add("value", obj.GetPi(attribute.ComboDisplayColumn).GetValue(obj));
                    if (attribute.AdditionalFields != null && attribute.AdditionalFields.Length > 0)
                    {
                        foreach (var def in attribute.TableType.GetModelDefinition().FieldDefinitions.Where(w => attribute.AdditionalFields.Contains(w.FieldName)))
                        {
                            dynamicObject.Add(def.Name, obj.GetPi(def.Name).GetValue(obj));
                        }
                    }

                    return dynamicObject;
                }

                bool insertToCache = false;
                string cacheKey = $"cb:{attribute.TableType.FullName}{ten}{sql.GetHashCode()}";

                if ((alias.StartsWith("[C_") || attribute.TableType.HasAttribute<CachedAttribute>()) && !typeof(IPfeCustomizeCombo).IsAssignableFrom(attribute.TableType))
                {
                    if (parameters != null && parameters.Count > 0)
                    {
                        var sb = new StringBuilder();
                        foreach (KeyValuePair<string, object> par in parameters)
                        {
                            sb.AppendFormat("{0}:{1}:", par.Key, (par.Value != null) ? par.Value.ToString() : "null");
                        }

                        cacheKey += $":{sb.ToString().GetHashCode()}";
                    }

                    var listType = typeof(List<>).MakeGenericType(attribute.TableType);

                    //var cacheData2 = Cache.Get<object>(cacheKey);
                    var cacheData = typeof(ServiceStack.Caching.ICacheClient).GetMethod("Get", new[] { typeof(string) }).MakeGenericMethod(listType).Invoke(Cache, new object[] { cacheKey });
                    if (cacheData != null)
                    {
                        var result = ((IEnumerable)cacheData).Cast<object>();
                        if (result != null)
                        {
                            return result.Select(x =>
                            {
                                return CreateExpandoObject(x);
                            }
                            ).ToList();
                        }
                    }
                    insertToCache = true;
                }

                var objectsToCache = (IList)typeof(OrmLiteReadApi).GetMethod("SqlList", new[] { typeof(IDbConnection), typeof(string), typeof(object) }).MakeGenericMethod(attribute.TableType).Invoke(Db, new object[] { Db, sql, parameters });

                if (attribute.TableType.HasInterface(typeof(IAfterGetList)))
                {
                    var obj = (IAfterGetList)Activator.CreateInstance(attribute.TableType);
                    var methodAfterGetList = typeof(IAfterGetList).GetMethods().First(x => x.Name == "AfterGetList");
                    methodAfterGetList.MakeGenericMethod(attribute.TableType).Invoke(obj, new object[] { this, objectsToCache, null });
                }

                if (insertToCache)
                {
                    SetToCacheLocal(cacheKey, objectsToCache, new TimeSpan(3, 0, 0));
                }

                var listObjects = objectsToCache.Cast<object>();
                return listObjects.Select(x =>
                {
                    return CreateExpandoObject(x);
                }
                ).ToList();
            }
        }

        #endregion


        #region Helpers

        private Dictionary<string, string> GetRequiredValue(string[] requiredFields)
        {
            var param = new Dictionary<string, string>();

            if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.QueryString.AllKeys.Any(nav => requiredFields.Any(r => r.ToLower() == nav.ToLower())))
            {
                foreach (string key in HttpContext.Current.Request.QueryString.AllKeys)
                {
                    if (requiredFields.Any(nav => nav.ToLower() == key.ToLower()))
                    {
                        string name = requiredFields.First(nav => nav.ToLower() == key.ToLower());
                        string value = HttpContext.Current.Request.QueryString[key];
                        if (!String.IsNullOrEmpty(value) && value.ToLower() != "null")
                        {
                            param.Add(name, value);
                        }
                    }
                }
            }

            return (param.Count > 0) ? param : null;
        }

        /// <summary>
        /// Zistenie 'friendly' message danej vynimky.
        /// Traversuje InnerException-s a preferuje typ WebEasValidationException alebo ked neni tak posledny WebEasException
        /// </summary>
        public string GetExceptionMessage(Exception ex)
        {
            WebEasException lastEasEx = null;
            while (ex != null)
            {
                if (ex is WebEasValidationException)
                {
                    return ((WebEasValidationException)ex).HasMessageUser
                        ? ((WebEasValidationException)ex).MessageUser
                        : ((WebEasValidationException)ex).Message;
                }
                else if (ex is WebEasException)
                {
                    lastEasEx = (WebEasException)ex;
                }
                if (ex.InnerException == null)
                {
                    return lastEasEx == null ? ex.Message : (lastEasEx.HasMessageUser ? lastEasEx.MessageUser : lastEasEx.Message);
                }
                ex = ex.InnerException;
            }
            return "Interná chyba";
        }

        public void RetryOnException(int times, TimeSpan delay, Action operation)
        {
            var attempts = 0;
            while (true)
            {
                try
                {
                    attempts++;
                    operation();
                    break;
                }
                catch (Exception ex)
                {
                    if (attempts == times)
                        throw;

                    Log.Error($"Chyba pri volania operacie po {attempts} pokuse", ex);

                    System.Threading.Tasks.Task.Delay(delay).Wait();
                }
            }
        }

        #endregion Helpers

        #region Cache

        /// <summary>
        /// Invalidate tree counts for given (sub)path
        /// If null/empty string is passed, all counts will be invalidated (in all modules)
        /// </summary>
        public void InvalidateTreeCountsForPath(string path)
        {
            string pattern = string.Format(@"treecounts:{0}.*", path).ToLowerInvariant();
            RemoveFromCacheByRegexOptimizedTenant(pattern);
        }

        /// <summary>
        /// Gets from cache.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        protected T GetFromCache<T>(string key, bool useGzipCompression = false)
        {
            string cleankey = key.Replace("!", ":");

            if (useGzipCompression)
            {
                var bytes = GetFromCache<byte[]>(cleankey);
                return bytes != null ? Unzip(bytes).FromJson<T>() : default;
            }
            else
            {
                return Cache.Get<T>(cleankey);
            }
        }

        /// <summary>
        /// Saves the file to cache.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="timeTo">The time to.</param>
        /// <returns></returns>
        protected string SaveFileToCache(CachedFile file, TimeSpan? timeTo = null)
        {
            string key = string.Format("file:{0}", file.TempId);
            Cache.Set(key, file, timeTo ?? new TimeSpan(1, 0, 0));
            return file.TempId;
        }

        /// <summary>
        /// Gets the file from cache.
        /// </summary>
        /// <param name="tempId">The temp id.</param>
        /// <returns></returns>
        protected CachedFile GetFileFromCache(string tempId)
        {
            string key = string.Format("file:{0}", tempId);
            return Cache.Get<CachedFile>(key);
        }

        /// <summary>
        /// Removes the file from cache.
        /// </summary>
        /// <param name="tempId">The temp id.</param>
        protected void RemoveFileFromCache(string tempId)
        {
            string key = string.Format("file:{0}", tempId);
            Cache.Remove(key);
        }

        /// <summary>
        /// Sets to cache.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <param name="timeTo">The time to.</param>
        protected void SetToCache<T>(string key, T data, TimeSpan? expireCacheIn = null, bool useGzipCompression = false)
        {
            string cleankey = key.Replace("!", ":");
            var expiresIn = expireCacheIn ?? new TimeSpan(3, 0, 0);

            if (useGzipCompression)
            {
                Cache.Set(cleankey, data != null ? Zip(data.ToJson()) : null, expiresIn);
            }
            else
            {
                Cache.Set(cleankey, data, expiresIn);
            }
        }

        protected void SetToCacheOptimizedTenant<T>(string key, T data, TimeSpan? expireCacheIn = null, bool useGzipCompression = false)
        {
            key = string.Format("ten:{0}:{1}", Session.TenantId, key);
            SetToCache(key, data, expireCacheIn, useGzipCompression);
        }

        /// <summary>
        /// Removes from cache.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        public void RemoveFromCache(Type objectType)
        {
            try
            {
                if (objectType.HasAttribute<AliasAttribute>() && objectType.FirstAttribute<AliasAttribute>().Name.StartsWith("C_") || objectType.HasAttribute<CachedAttribute>())
                {
                    string key = string.Format("urn:{0}:.*", objectType.FullName);
                    RemoveFromCacheByRegexLocal(key);

                    if (objectType.HasInterface(typeof(ITenantEntity)))
                    {
                        key = string.Format("cb:{0}:{1}:.*", objectType.FullName, this.Session.TenantId.ToUpper());
                        RemoveFromCacheByRegexLocal(key);
                    }
                    else
                    {
                        key = string.Format("cb:{0}:.*", objectType.FullName);
                        RemoveFromCacheByRegexLocal(key);
                    }

                    if (PublicReferences.ContainsKey(objectType.FullName))
                    {
                        foreach (string refs in PublicReferences[objectType.FullName])
                        {
                            key = string.Format("urn:{0}:.*", refs);
                            RemoveFromCacheByRegexLocal(key);
                        }
                    }
                }

                if (objectType.HasAttribute<PublicReferenceAttribute>())
                {
                    foreach (var refs in objectType.GetCustomAttributes<PublicReferenceAttribute>().Select(nav => nav.Reference.FullName))
                    {
                        string key = string.Format("urn:{0}:.*", refs);
                        RemoveFromCacheByRegexLocal(key);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new WebEasException("Error in removing from cache", ex);
            }
        }

        private static Dictionary<string, List<string>> publicReferences;
        private static readonly object lockPublicReferences = new object();

        /// <summary>
        /// Gets the public references.
        /// </summary>
        /// <value>The public references.</value>
        public static Dictionary<string, List<string>> PublicReferences
        {
            get
            {
                if (publicReferences == null || publicReferences.Count == 0)
                {
                    lock (lockPublicReferences)
                    {
                        if (publicReferences == null || publicReferences.Count == 0)
                        {
                            var pubReferences = new Dictionary<string, List<string>>();
                            IEnumerable<IServiceModel> models = NinjectServiceLocator.Kernel.GetAll<IServiceModel>();

                            foreach (IServiceModel model in models)
                            {
                                IEnumerable<Type> types = model.GetType().Assembly.GetTypes().Where(nav => nav.HasAttribute<PublicReferenceAttribute>());
                                foreach (Type t in types)
                                {
                                    pubReferences.Add(t.FullName, t.GetCustomAttributes<PublicReferenceAttribute>().Select(nav => nav.Reference.FullName).ToList());
                                }
                            }
                            publicReferences = pubReferences;
                        }
                    }
                }
                return publicReferences;
            }
        }

        /// <summary>
        /// Sets to cache local.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <param name="expireCacheIn">The expire cache in.</param>
        protected void SetToCacheLocal<T>(string key, T data, TimeSpan? expireCacheIn = null)
        {
            string k = key.Replace("!", ":");

            Context.Current.LocalMachineCache.Set(k, data, expireCacheIn.HasValue ? new TimeSpan(expireCacheIn.Value.Ticks / 6) : new TimeSpan(0, 10, 0));
            Cache.Set(k, data, expireCacheIn ?? new TimeSpan(3, 0, 0));
        }

        /// <summary>
        /// Removes from cache.
        /// </summary>
        /// <param name="key">The key.</param>
        protected void RemoveFromCache(string key)
        {
            Cache.Remove(key.Replace("!", ":"));
        }

        /// <summary>
        /// Removes from cache by pattern.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        protected void RemoveFromCacheByRegex(string pattern)
        {
            Cache.RemoveByRegex(pattern.Replace("!", ":"));
        }

        protected void RemoveFromCacheByRegexOptimizedTenant(string pattern)
        {
            pattern = string.Format("ten:{0}:{1}", this.Session.TenantId, pattern);
            RemoveFromCacheByRegex(pattern);
        }

        /// <summary>
        /// Removes from cache local.
        /// </summary>
        /// <param name="key">The key.</param>
        protected void RemoveFromCacheLocal(string key)
        {
            string k = key.Replace("!", ":");
            Context.Current.LocalMachineCache.Remove(k);
            Cache.Remove(k);
        }

        /// <summary>
        /// Removes from cache local.
        /// </summary>
        /// <param name="key">The key.</param>
        protected void RemoveFromCacheByRegexLocal(string pattern)
        {
            string k = pattern.Replace("!", ":");
            Context.Current.LocalMachineCache.RemoveByRegex(k);
            Cache.RemoveByRegex(k);
        }

        /// <summary>
        /// Gets the cache optimized tenant.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <param name="expireCacheIn">The expire cache in.</param>
        /// <returns></returns>
        public T GetCacheOptimizedTenant<T>(string key, Func<T> data, TimeSpan? expireCacheIn = null)
        {
            key = string.Format("ten:{0}:{1}", Session.TenantId, key);
            return GetCacheOptimized(key, data, expireCacheIn);
        }

        /// <summary>
        /// Gets the cache optimized tenant local.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <param name="expireCacheIn">The expire cache in.</param>
        /// <returns></returns>
        public T GetCacheOptimizedTenantLocal<T>(string key, Func<T> data, TimeSpan? expireCacheIn = null)
        {
            key = string.Format("{0}:ten:{1}", key, Session.TenantId);
            return GetCacheOptimizedLocal(key, data, expireCacheIn);
        }

        /// <summary>
        /// Gets the cache optimized session.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <param name="expireCacheIn">The expire cache in.</param>
        /// <returns></returns>
        public T GetCacheOptimizedSession<T>(string key, Func<T> data, TimeSpan? expireCacheIn = null)
        {
            key = $"sessions:{Session.Id}:{key}";
            return GetCacheOptimized(key, data, expireCacheIn);
        }

        /// <summary>
        /// Gets the cache optimized.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <param name="expireCacheIn">The expire cache in.</param>
        /// <returns></returns>
        public T GetCacheOptimized<T>(string key, Func<T> data, TimeSpan? expireCacheIn = null)
        {
            T result = GetFromCache<T>(key);
            if (result == null)
            {
                result = data.Invoke();
                SetToCache(key, result, expireCacheIn);
            }

            return result;
        }

        /*
        /// <summary>
        /// executes function with acquired lock on redis record
        /// </summary>
        /// <param name="key">key to lock</param>
        /// <param name="data">function to execute</param>
        /// <param name="lockTime">lock time</param>
        public void AcquireRedisLock(string key, Func<bool> data, TimeSpan lockTime)
        {
            if (Context.Current.IsRedisCaching)
            {
                using (((Context.Current.Cache as ServiceStack.Redis.RedisClientManagerCacheClient).GetClient() as ServiceStack.Redis.IRedisClient).AcquireLock(string.Concat(key, ".lock"), lockTime))
                {
                    data.Invoke();
                }
            }
            else
                throw new NotSupportedException("Only for Redis cache");
        }
        */

        public void AcquireRedisLock(string key, Func<bool> data, TimeSpan lockTime)
        {
            if (Cache is ServiceStack.Redis.RedisClientManagerCacheClient)
            {
                using var redisClient = RedisManager.GetClient();
                using (redisClient.AcquireLock(string.Concat(key, ".lock"), lockTime))
                {
                    data.Invoke();
                }
            }
        }
        /// <summary>
        /// Gets the cache optimized.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <param name="expireCacheIn">The expire cache in.</param>
        /// <returns></returns>
        protected T GetCacheOptimized<T>(string key, Func<T> data, TimeSpan? expireCacheIn, Func<bool> useCache)
        {
            if (useCache.Invoke())
            {
                return GetCacheOptimized(key, data, expireCacheIn);
            }
            else
            {
                return data.Invoke();
            }
        }

        private delegate void SetToCacheDelegate<T>(string key, T data, TimeSpan? expireCacheIn = null);

        /// <summary>
        /// Gets the cache optimized local.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <param name="expireCacheIn">The expire cache in.</param>
        /// <returns></returns>
        protected T GetCacheOptimizedLocal<T>(string key, Func<T> data, TimeSpan? expireCacheIn = null)
        {
            if (Cache is ServiceStack.Redis.RedisClientManagerCacheClient)
            {
                T result = Context.Current.LocalMachineCache.Get<T>(key);
                if (result == null)
                {
                    result = Context.Current.LocalMachineCache.Get<T>(key);
                    if (result == null)
                    {
                        result = data.Invoke();
                    }
                    SetToCacheLocal(key, result, expireCacheIn);
                }
                return result;
            }
            else
            {
                return GetCacheOptimized(key, data, expireCacheIn);
            }
        }

        protected void RemoveFromCacheOptimizedTenant(string key)
        {
            key = string.Format("ten:{0}:{1}", this.Session.TenantId, key);
            this.RemoveFromCache(key);
        }

        protected void RemoveFromCacheOptimizedSession(string key)
        {
            key = string.Format("{0}:{1}", this.Session.Id, key);
            this.RemoveFromCache(key);
        }

        #endregion


        #region default row values for modules

        /// <summary>
        /// GetDefaultRowValues
        /// </summary>
        /// <param name="code"></param>
        /// <param name="masterCode"></param>
        /// <param name="masterRowId"></param>
        /// <returns></returns>
        public virtual object GetRowDefaultValues(string code, string masterCode, string masterRowId)
        {
            return null;
        }

        #endregion

        /// <summary>
        /// Ak je kodPolozky modul, vrati cely strom prav, inak vrati pravo na konkretnu polozku
        /// </summary>
        /// <param name="kodPolozky"></param>
        /// <returns></returns>
        public List<UserNodeRight> GetUserTreeRights(string kodPolozky)
        {
            var kodPolozkyClean = HierarchyNodeExtensions.RemoveParametersFromKodPolozky(HierarchyNodeExtensions.CleanKodPolozky(kodPolozky.ToLower()));
            var modul = kodPolozkyClean.Split('-')[0];

            var prava = GetCacheOptimizedTenant($"pfe:TreeRights_{modul}_{Session.UserId}", () =>
            {
                var reader = Db.ExecuteReader($"Exec [cfe].[PR_UserTreeRights] '{modul}', '{Session.UserId}'");
                var p = reader.Parse<UserNodeRight>().ToList();
                reader.Close();

                return p;
            }, new TimeSpan(24, 0, 0));

            //if (modul == "cfe" && Session.AdminLevel != AdminLevel.SysAdmin)
            //{
            //    if (prava.FirstOrDefault(pr => pr.Kod == "cfe")?.Pravo > 1)
            //    {
            //        Session.AdminLevel = AdminLevel.CfeAdmin;
            //        Cache.Set(Session.UniqueKey, Session, new TimeSpan(1, 0, 0));
            //    }
            //    else
            //    {
            //        Session.AdminLevel = AdminLevel.User;
            //        Cache.Set(Session.UniqueKey, Session, new TimeSpan(1, 0, 0));
            //    }
            //}

            if (Session.AdminLevel == AdminLevel.SysAdmin)
                prava.ForEach(p => p.Pravo = 4);

            return (kodPolozkyClean == modul) ? prava : prava.Where(p => p.Kod == kodPolozkyClean).ToList();
        }


        #region Nastavenie

        /// <summary>
        /// Vráti hodnotu používateľského alebo tenantského nastavenia podľa zadaných kritérií.
        /// Ak sa dopytuje na používateľské nastavenie ktoré používateľ nemá zadané, použije sa príslušné tenantské. Ak nastavenie nie je zadané vôbec, vráti sa preddefinovaná hodnota.
        /// </summary>
        /// <param name="modul">Kód modulu</param>
        /// <param name="kod">Kód nastavenia</param>
        /// <returns>String</returns>
        public string GetNastavenieS(string modul, string kod)
        {
            if (string.IsNullOrEmpty(Session.TenantId))
            {
                return "";
            }
            else
            {
                return GetCacheOptimizedTenant(string.Format("GetNastavenie:{0}:{1}", modul, kod), () =>
                {
                    return Db.Select<string>(string.Format("select reg.F_NastavenieS('{0}','{1}','{2}')", modul, kod, Session.UserId)).FirstOrDefault();
                });
            }
        }

        /// <summary>
        /// Vráti hodnotu používateľského alebo tenantského nastavenia podľa zadaných kritérií.
        /// Ak sa dopytuje na používateľské nastavenie ktoré používateľ nemá zadané, použije sa príslušné tenantské. Ak nastavenie nie je zadané vôbec, vráti sa preddefinovaná hodnota.
        /// </summary>
        /// <param name="modul">Kód modulu</param>
        /// <param name="kod">Kód nastavenia</param>
        /// <returns>Long</returns>
        public long GetNastavenieI(string modul, string kod)
        {
            if (string.IsNullOrEmpty(Session.TenantId))
            {
                return 0;
            }
            else
            {
                var res = GetCacheOptimizedTenant(string.Format("GetNastavenie:{0}:{1}", modul, kod), () =>
                {
                    return Db.Select<long?>(string.Format("select reg.F_NastavenieI('{0}','{1}','{2}')", modul, kod, Session.UserId)).FirstOrDefault();
                });

                return res.GetValueOrDefault();
            }
        }

        public DateTime? CheckESAMStartDate(DateTime? DatumDo)
        {
            DateTime? datumStart = GetNastavenieD("reg", "eSAMStart");
            if (datumStart != null)
            {
                return datumStart.Value.AddDays(-1);
            }
            else if (DatumDo != null)
            {
                UpdateNastavenieBase nast = new UpdateNastavenieBase
                {
                    dHodn = DatumDo.Value.AddDays(1),
                    Nazov = "eSAMStart"
                };
                UpdateNastavenie(nast, "reg");
                return DatumDo;
            }
            else
            {
                throw new WebEasException(null, "Nie je nastavený parameter eSAMStart!");
            }
        }

        /// <summary>
        /// Vráti hodnotu používateľského alebo tenantského nastavenia podľa zadaných kritérií.
        /// Ak sa dopytuje na používateľské nastavenie ktoré používateľ nemá zadané, použije sa príslušné tenantské. Ak nastavenie nie je zadané vôbec, vráti sa preddefinovaná hodnota.
        /// </summary>
        /// <param name="modul">Kód modulu</param>
        /// <param name="kod">Kód nastavenia</param>
        /// <returns>Boolean</returns>
        public bool GetNastavenieB(string modul, string kod)
        {
            if (string.IsNullOrEmpty(Session.TenantId))
            {
                return false;
            }
            else
            {
                var res = GetCacheOptimizedTenant(string.Format("GetNastavenie:{0}:{1}", modul, kod), () =>
                {
                    return Db.Select<bool?>(string.Format("select reg.F_NastavenieB('{0}','{1}','{2}')", modul, kod, Session.UserId)).FirstOrDefault();
                });

                return res.GetValueOrDefault();
            }
        }

        /// <summary>
        /// Vráti hodnotu používateľského alebo tenantského nastavenia podľa zadaných kritérií.
        /// Ak sa dopytuje na používateľské nastavenie ktoré používateľ nemá zadané, použije sa príslušné tenantské. Ak nastavenie nie je zadané vôbec, vráti sa preddefinovaná hodnota.
        /// </summary>
        /// <param name="modul">Kód modulu</param>
        /// <param name="kod">Kód nastavenia</param>
        /// <returns>Boolean</returns>
        public DateTime? GetNastavenieD(string modul, string kod)
        {
            if (string.IsNullOrEmpty(Session.TenantId))
            {
                return null;
            }
            else
            {
                var res = GetCacheOptimizedTenant(string.Format("GetNastavenie:{0}:{1}", modul, kod), () =>
                {
                    return Db.Select<DateTime?>(string.Format("select reg.F_NastavenieD('{0}','{1}','{2}')", modul, kod, Session.UserId)).FirstOrDefault();
                });

                return res;
            }
        }

        public NastavenieView UpdateNastavenie(UpdateNastavenieBase updateNastavenie)
        {
            return UpdateNastavenie(updateNastavenie, ActualModul);
        }

        public NastavenieView UpdateNastavenie(UpdateNastavenieBase updateNastavenie, string explicitModul)
        {
            using (var transaction = BeginTransaction())
            {

                try
                {
                    string typ = Db.Scalar<string>("Select Typ from [reg].[V_Nastavenie] where Nazov = @nazov and Modul = @modul", new { nazov = updateNastavenie.Nazov, modul = explicitModul });
                    var p = new DynamicParameters();
                    p.Add("@tenant", Session.TenantIdGuid, dbType: DbType.Guid);
                    p.Add("@modul", explicitModul, dbType: DbType.String);
                    p.Add("@nazov", updateNastavenie.Nazov, dbType: DbType.String);
                    //p.Add("@pouzivatel", null, dbType: DbType.String);
                    switch (typ)
                    {
                        case "I":
                            p.Add("@bigint", updateNastavenie.iHodn, dbType: DbType.Int64);
                            break;
                        case "S":
                            p.Add("@string", updateNastavenie.sHodn, dbType: DbType.String);
                            break;
                        case "B":
                            p.Add("@bit", updateNastavenie.bHodn, dbType: DbType.Boolean);
                            break;
                        case "D":
                            p.Add("@date", updateNastavenie.dHodn, dbType: DbType.Date);
                            break;
                        case "T":
                            p.Add("@datetime", updateNastavenie.tHodn, dbType: DbType.DateTime);
                            break;
                        case "N":
                            p.Add("@numeric", updateNastavenie.nHodn, dbType: DbType.Decimal);
                            break;
                    }

                    SqlProcedure("[reg].[PR_Nastavenie]", p);

                    transaction.Commit();
                }
                catch (WebEasValidationException ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new WebEasException("Nastala chyba pri volaní SQL procedúry [reg].[PR_Nastavenie]", "Parameter sa nepodarilo upraviť kvôli internej chybe", ex);
                }
            }

            RemoveFromCacheByRegexOptimizedTenant("GetNastavenie:.*");

            return GetList<NastavenieView>(x => x.Nazov == updateNastavenie.Nazov && x.Modul == explicitModul).FirstOrDefault();
        }

        /// <summary>
        /// Get actual module code based on actual repository class namespace.
        /// Returns 3 characters code (lowercase) or null if can not resolve
        /// </summary>
        public string ActualModul
        {
            get
            {
                string nameSpace = this.GetType().Namespace;
                if (nameSpace.StartsWith("WebEas.Esam.ServiceInterface.Office.") && (nameSpace.Length == 39 || (nameSpace.Length > 39 && nameSpace[39] == '.')))
                {
                    return nameSpace.Substring(36, 3).ToLowerInvariant();
                }
                //not above case???
                return null;
            }
        }

        /// <summary>
        /// Get actual module Id from cfe.V_Modul
        /// </summary>
        public byte ActualModul_Id
        {
            get
            {
                return (EsamModules.SingleOrDefault(x => x.Kod == ActualModul) ?? throw new WebEasException($"Modul '{ActualModul}' neexistuje v cfe.V_Modul !")).C_Modul_Id;
            }
        }

        public List<EsamModul> EsamModules
        {
            get
            {
                return GetCacheOptimized("cfe:Modul", () =>
                {
                    return Db.Select<EsamModul>();
                }, new TimeSpan(5, 0, 0, 0));
            }
        }

        #endregion
    }
}