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
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using WebEas.Core.Base;
using WebEas.Ninject;
using WebEas.ServiceModel;
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
        /// Gets entity by Id and sets access flags.
        /// </summary>
        /// <returns>The data or null if not found</returns>
        public T GetById<T>(object id) where T : class
        {
            List<T> data = this.GetList<T>(new Filter(typeof(T).GetPrimaryKeyName(), id));
            return data.FirstOrDefault();
        }

        /// <summary>
        /// Gets the record and sets the access flags.
        /// </summary>
        public T GetRecord<T>(object id) where T : class
        {
            List<T> data = this.GetList<T>(new Filter(typeof(T).GetPrimaryKeyName(), id));
            this.SetAccessFlag(data);
            return data.FirstOrDefault();
        }

        /// <summary>
        /// Gets entity by referential Id.
        /// </summary>
        /// <returns>The data or null if not found</returns>
        public T GetById<T>(string column, object id) where T : class
        {
            return this.GetList<T>(new Filter(column, id)).FirstOrDefault();
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
            return this.Db.Exec((IDbCommand dbCmd) => dbCmd.GetList<T>(this, filter, null, null));
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
            return this.Db.Exec((IDbCommand dbCmd) => dbCmd.GetList<T>(this, WebEasFilterExpression.DecodeFilter(filter), null, null));
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
            return this.Db.Exec((IDbCommand dbCmd) => dbCmd.GetList<T>(this, filter, pagging, null, null));
        }

        /// <summary>
        /// Gets the filtered list of entities
        /// </summary>
        /// <remarks>
        /// POZOR: tato metoda sa vola dynamicky cez MakeGenericMethod() !!! preto jej zmeny mozu znefunkcnit dane volania
        /// - volania sa daju vyhladat pomocou stringu 'GetMethod("GetList",'
        /// </remarks>
        public List<T> GetList<T>(Filter filter = null, PaggingParameters pagging = null, List<PfeSortAttribute> userSort = null, List<string> columnsWithData = null, Filter ribbonFilter = null) where T : class
        {
            return this.Db.Exec((IDbCommand dbCmd) => dbCmd.GetList<T>(this, filter, pagging, userSort, columnsWithData, ribbonFilter));
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
                    node = Modules.FindFirstNode(typeof(T));
                }
                else
                {
                    node = Modules.FindNode(listDto.KodPolozky);
                }
            }

            #region Check authorization

            HierarchyNode rootNode = node.GetRootNode();
            HashSet<string> sessionRoles = this.Session.Roles;

            if (rootNode.Kod != "reg" && !sessionRoles.Contains(string.Format("{0}_MEMBER", rootNode.Kod.ToUpper())))
            {
                throw new WebEasUnauthorizedAccessException();
            }
            else if (rootNode.Kod == "reg")
            {
                List<string> memberRoles = sessionRoles.Where(p => p.EndsWith("_MEMBER")).ToList();
                if (memberRoles.Count == 0)
                {
                    throw new WebEasUnauthorizedAccessException();
                }
            }

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
            if(node.KodPolozky.Equals("rzp-evi-intd-pol") || node.KodPolozky.Equals("rzp-evi-zmena-pol"))
            {
                int mes = 0;
                string stmp;
                foreach (FilterElement el in filter.FilterElements)
                {
                    if (Convert.ToString(el.Value).EndsWith("M@")) {
                        stmp = el.Value.ToString().Split('|')[1];
                        mes = stmp.Substring(0, stmp.Length - 2).ToInt(); 
                        el.Value = el.Value.ToString().Split('|')[0];
                    }
                }
                if (mes > 0)
                {
                    if (node.KodPolozky.Equals("rzp-evi-zmena-pol"))
                    {
                        filter.And(FilterElement.LessThanOrEq("Mesiac", mes));
                        filter.And(FilterElement.In("C_StavEntity_Id", new[] { (int)StavEntityEnumRzp.SCHVALENY, (int)StavEntityEnumRzp.ODOSLANY }));  // ZMENY
                    }
                    else { 
                        filter.And(FilterElement.LessThanOrEq("UOMesiac", mes));
                        filter.And(FilterElement.Eq("C_StavEntity_Id", (int)StavEntityEnumInd.ZAUCTOVANY));  // IND
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

            #region Columns with data

            var columnsWithData = new List<string>();
            if (!string.IsNullOrEmpty(listDto.ColumnsWithData))
            {
                byte[] columnsWithDataBytes = Convert.FromBase64String(listDto.ColumnsWithData);
                string resultColumnsWithData = Encoding.UTF8.GetString(columnsWithDataBytes);
                columnsWithData = ServiceStack.Text.JsonSerializer.DeserializeFromString<List<string>>(resultColumnsWithData);
            }

            #endregion

            List<T> data = this.GetList<T>(filter, pagging, userSort, columnsWithData, ribbonFilter);
            this.SetAccessFlag(data);

            return data;
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

                var originalDb = (IBaseEntity)this.GetType().GetMethod("GetById", new Type[] { typeof(object) }).MakeGenericMethod(data.EntityType).Invoke(this, new object[] { id });

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
        public int Count<T>(Filter filter) where T : class
        {
            return this.Db.Exec((IDbCommand dbCmd) => dbCmd.GetCount<T>(this, filter));
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
            data.Validate();

            long id = -1;
            IDbTransaction transaction = this.BeginTransaction();

            try
            {
                IBaseEntity entity = data.GetEntity();
                id = this.InsertData(entity);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
            finally
            {
                this.EndTransaction(transaction);
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
        public int Create<T>(T obj) where T : class, IBaseEntity
        {
            int retval;
            IDbTransaction transaction = this.BeginTransaction();

            try
            {
                retval = (int)this.InsertData<T>(obj);

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
            finally
            {
                this.EndTransaction(transaction);
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
            IDbTransaction transaction = this.BeginTransaction();
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
            finally
            {
                this.EndTransaction(transaction);
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
            data.Validate();

            object id = null;

            IDbTransaction transaction = this.BeginTransaction();

            try
            {
                IBaseEntity entity = this.GetByIdFilled(data);
                id = entity.GetId();

                //this.UpdateData(entity);
                typeof(WebEasRepositoryBase).GetMethod("UpdateDynamic", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(entity.GetType()).Invoke(this, new object[] { entity });

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
            finally
            {
                this.EndTransaction(transaction);
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
            return this.Db.Exec((IDbCommand dbCmd) => dbCmd.UpdateData<T>(this, obj));
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
        public int Delete<T>(object id) where T : class, IBaseEntity
        {
            int retval;

            if (typeof(T).HasAttribute<AliasAttribute>() && typeof(T).FirstAttribute<AliasAttribute>().Name.ToUpper().StartsWith("C_"))
            {
                IDbTransaction tr = this.BeginTransaction();
                try
                {
                    retval = this.Db.Exec((IDbCommand dbCmd) => dbCmd.DeleteData<T>(this, id));
                    tr.Commit();
                    return retval;
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    Log.Info("Error in deleting data", ex);
                }
                finally
                {
                    this.EndTransaction(tr);
                }
            }

            IDbTransaction transaction = this.BeginTransaction();
            try
            {
                retval = this.Db.Exec((IDbCommand dbCmd) => dbCmd.SafeDeleteData<T>(this, id));
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
            finally
            {
                this.EndTransaction(transaction);
            }

            return retval;
        }

        /// <summary>
        /// Deletes by id.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="objs">The id.</param>
        public int DeleteData<T>(object id) where T : class, IBaseEntity
        {
            int retval;

            if (typeof(T).HasAttribute<AliasAttribute>() && typeof(T).FirstAttribute<AliasAttribute>().Name.ToUpper().StartsWith("C_"))
            {
                try
                {
                    retval = this.Db.Exec((IDbCommand dbCmd) => dbCmd.DeleteData<T>(this, id));
                    return retval;
                }
                catch (Exception ex)
                {
                    Log.Info("Error in deleting data", ex);
                }
            }
            return this.Db.Exec((IDbCommand dbCmd) => dbCmd.SafeDeleteData<T>(this, id));
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
            IDbTransaction transaction = this.Db.BeginTransaction(IsolationLevel.ReadCommitted);
            if (this.Db is OrmLiteConnection && ((OrmLiteConnection)this.Db).Transaction == null)
            {
                ((OrmLiteConnection)this.Db).Transaction = transaction;
            }
            return transaction;
        }

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <returns></returns>
        public IDbTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            IDbTransaction transaction = this.Db.BeginTransaction(isolationLevel);
            if (this.Db is OrmLiteConnection && ((OrmLiteConnection)this.Db).Transaction == null)
            {
                ((OrmLiteConnection)this.Db).Transaction = transaction;
            }
            return transaction;
        }

        /// <summary>
        /// Ends the transaction.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        public void EndTransaction(IDbTransaction transaction)
        {
            if (transaction != null)
            {
                transaction.Dispose();
                if (this.Db is OrmLiteConnection && ((OrmLiteConnection)this.Db).Transaction != null)
                {
                    ((OrmLiteConnection)this.Db).Transaction = null;
                }
            }
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
            HierarchyNode node = Modules.FindNode(code);
            SourceTableAttribute[] stAttrib = node.ModelType.AllAttributes<SourceTableAttribute>();
            if (stAttrib == null || stAttrib.Length == 0)
            {
                return null;
            }

            Filter filter = null;

            if (stAttrib.Any(a => !string.IsNullOrEmpty(a.PrimaryKey)))
            {
                //kedze mame inu hodnotu pre primary key, musim si nacitat zaznam...
                object data = this.GetType().GetMethod("GetById", new Type[] { typeof(object) }).MakeGenericMethod(node.ModelType).Invoke(this, new object[] { rowId });
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
                    if (config.Any( nav=> 
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
                                Guid idOsoby;

                                if (Guid.TryParse(x.PovodnaHodnota, out idOsoby))
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
                        var prop = node.ModelType.AllProperties().Where(nav => nav.HasAttribute<PfeComboAttribute>() && nav.GetCustomAttribute<PfeComboAttribute>().IdColumnCombo == x.NazovStlpca).FirstOrDefault();
                        if (prop != null)
                        {
                            var pfeCol = prop.FirstAttribute<PfeColumnAttribute>();
                            if (pfeCol != null)
                            {
                                x.NazovStlpca = pfeCol.Text;   
                            }

                            var comboList = GetListCombo(node.ModelType, prop.Name);
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
                                DateTime dtPovodnaHodnota;
                                if (x.PovodnaHodnota != null && DateTime.TryParse(x.PovodnaHodnota, out dtPovodnaHodnota))
                                {
                                    x.PovodnaHodnota = dtPovodnaHodnota.ToShortDateString();
                                }

                                DateTime dtNovaHodnota;
                                if (x.NovaHodnota != null && DateTime.TryParse(x.NovaHodnota, out dtNovaHodnota))
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



        #region List Combo - zoznam do comba
        
        public List<ComboResult> GetListCombo(IListComboDto request)
        {
            HierarchyNode node = Modules.FindNode(request.KodPolozky);
            string[] requestFields = String.IsNullOrEmpty(request.RequiredField) ? null : request.RequiredField.Split('/');
            return this.GetListCombo(node.ModelType, request.Column, requestFields);
        }
        
        /// <summary>
        /// Lists the combo.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public List<ComboResult> GetListCombo(Type modelType, string col, string[] requestFields = null)
        {
            PropertyInfo column = modelType.Properties().FirstOrDefault(nav => nav.Name.ToLower() == col.ToLower());
            if (column.HasAttribute<PfeColumnAttribute>() && column.HasAttribute<PfeComboAttribute>())
            {
                PfeComboAttribute combo = column.FirstAttribute<PfeComboAttribute>();
                if (combo.TableType == null && column.HasAttribute<ReferencesAttribute>())
                {
                    combo.TableType = column.FirstAttribute<ReferencesAttribute>().Type;
                }
                
                if (combo.TableType != null && combo.TableType.HasInterface(typeof(IStaticCombo)))
                {
                    var sc = (IStaticCombo)Activator.CreateInstance(combo.TableType);
                    sc.Repository = this;
                    return sc.GetComboList(requestFields);
                }
                
                string[] requiredFields = column.FirstAttribute<PfeColumnAttribute>().RequiredFields;
                if (requiredFields == null || requiredFields.Length == 0)
                {
                    return this.ListCombo(combo, null);
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
                    return this.ListCombo(combo, param);
                }
                
                if (requestFields != null && requestFields.Length > 0 && requiredFields.Length <= requestFields.Length)
                {
                    var param = new Dictionary<string, string>();
                    for (int i = 0; i < requiredFields.Length; i++)
                    {
                        param.Add(requiredFields[i], requestFields[i]);
                    }
                    
                    return this.ListCombo(combo, param);
                }
                return this.ListCombo(combo, null);
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
                
                return this.ListCombo(combo, null);
            }
            else if (column.HasAttribute<ReferencesAttribute>())
            {
                Type type = column.FirstAttribute<ReferencesAttribute>().Type;
                
                return this.ListCombo(new PfeComboAttribute(type), null);
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
        private List<ComboResult> ListCombo(PfeComboAttribute attribute, Dictionary<string, string> requiredFields)
        {
            string sql = String.Empty;
            
            if (!String.IsNullOrEmpty(attribute.Sql))
            {
                sql = attribute.Sql;
                if (requiredFields != null && requiredFields.Count > 0)
                {
                    var parameters = new Dictionary<string, object>();
                    foreach (KeyValuePair<string, string> item in requiredFields)
                    {
                        parameters.AddAndChangeNameIfExists(item.Key, item.Value);
                    }

                    return this.Db.Select<ComboResult>(sql, parameters);
                }
                else
                {
                    return this.Db.Select<ComboResult>(sql);
                }
            }
            else
            {
                if (attribute.TableType == null)
                {
                    throw new ArgumentNullException("Table type not defined");
                }

                string ten = "";
                
                if (attribute.TableType.HasInterface(typeof(ITenantEntity)) || attribute.TableType.HasInterface(typeof(ITenantEntityNullable)))
                {
                    ten = string.Format(":{0}", this.Session.TenantId);
                }

                string key = string.Format("cb:{0}{1}{2}{3}{4}", attribute.TableType.FullName, ten, string.IsNullOrEmpty(attribute.IdColumnCombo) ? "" : string.Format(":{0}", attribute.IdColumnCombo), string.IsNullOrEmpty(attribute.DisplayColumn) ? "" : string.Format(":{0}", attribute.DisplayColumn), attribute.AdditionalWhereSql == null ? "" : string.Format(":{0}", attribute.AdditionalWhereSql.GetHashCode().ToString()));
                
                if (requiredFields != null && requiredFields.Count > 0)
                {
                    var sb = new StringBuilder();
                    foreach (KeyValuePair<string, string> field in requiredFields)
                    {
                        sb.AppendFormat("{0}:{1}:", field.Key, field.Value);
                    }
                    
                    key += string.Format(":{0}", sb.ToString().GetHashCode());
                }

                string alias = string.Format("[{0}]", attribute.TableType.HasAttribute<AliasAttribute>() ? attribute.TableType.FirstAttribute<AliasAttribute>().Name : attribute.TableType.Name);
                
                return this.GetCacheOptimized(key, () =>
                {
                    string schema = attribute.TableType.HasAttribute<SchemaAttribute>() ? String.Format("[{0}].", attribute.TableType.FirstAttribute<SchemaAttribute>().Name) : "";
                    
                    if (String.IsNullOrEmpty(attribute.IdColumnCombo))
                    {
                        PropertyInfo keyCol = attribute.TableType.Properties().FirstOrDefault(nav => nav.HasAttribute<PrimaryKeyAttribute>());
                        if (keyCol == null)
                        {
                            throw new ArgumentNullException(String.Format("Primary key not defined on table {0}", attribute.TableType.Name));
                        }
                        
                        attribute.IdColumnCombo = keyCol.HasAttribute<AliasAttribute>() ? keyCol.FirstAttribute<AliasAttribute>().Name : keyCol.Name;
                    }
                    
                    if (String.IsNullOrEmpty(attribute.DisplayColumn))
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
                        else if (properties.Any(nav => nav.Name.ToLower().ContainsAny("nazov", "name", "Description")))
                        {
                            valueCol = properties.First(nav => nav.Name.ToLower().ContainsAny("nazov", "name", "Description"));
                        }
                        else
                        {
                            valueCol = properties.Any(nav => nav.PropertyType == typeof(string)) ? properties.First(nav => nav.PropertyType == typeof(string)) : properties.First();
                        }
                        
                        attribute.DisplayColumn = valueCol.HasAttribute<PfeValueColumnAttribute>() && valueCol.FirstAttribute<PfeValueColumnAttribute>().HasSql() ? valueCol.FirstAttribute<PfeValueColumnAttribute>().Sql : valueCol.HasAttribute<AliasAttribute>() ? valueCol.FirstAttribute<AliasAttribute>().Name : valueCol.Name;
                    }
                    
                    var sqlWhere = new StringBuilder();
                    var parameters = new Dictionary<string, object>();
                    
                    bool isTenantEntity = attribute.TableType.HasInterface(typeof(ITenantEntity));
                    bool isTenantEntityNullable = attribute.TableType.HasInterface(typeof(ITenantEntityNullable));
                    
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
                                string addedKey;
                                if (item.Value.Length > 20 && DateTime.TryParseExact(item.Value.Insert(20, "+"), "yyyy-MM-ddTHH:mm:ss zzz", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime dateTime))
                                {
                                    addedKey = parameters.AddAndChangeNameIfExists(item.Key, dateTime);
                                }
                                else
                                    addedKey = parameters.AddAndChangeNameIfExists(item.Key, item.Value);

                                string field = string.Format("[{0}] = @{1}", item.Key, addedKey);
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

                    if (sqlWhere.Length > 0)
                    {
                        sql = string.Format("SELECT [{0}] AS [Id], {1} AS [Value] FROM {2}{3} WHERE {4} AND {1} IS NOT NULL ORDER BY [Value]", attribute.IdColumnCombo, attribute.DisplayColumn, schema, alias, sqlWhere);
                    }
                    else
                    {
                        sql = string.Format("SELECT [{0}] AS [Id], {1} AS [Value] FROM {2}{3} WHERE {1} IS NOT NULL ORDER BY [Value]", attribute.IdColumnCombo, attribute.DisplayColumn, schema, alias);
                    }
                    
                    return this.Db.SqlList<ComboResult>(sql, parameters);
                }, new TimeSpan(3, 0, 0), () => alias.StartsWith("[C_") || attribute.TableType.HasAttribute<CachedAttribute>());
            }
        }

        #endregion


        #region Helpers
        
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
        /// Gets from cache.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        protected T GetFromCache<T>(string key)
        {
            return Cache.Get<T>(key.Replace("!", ":"));
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
        protected void SetToCache<T>(string key, T data, TimeSpan? expireCacheIn = null)
        {
            Cache.Set(key.Replace("!", ":"), data, expireCacheIn ?? new TimeSpan(3, 0, 0));
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
                    this.RemoveFromCacheByRegexLocal(key);
                    
                    if (objectType.HasInterface(typeof(ITenantEntity)))
                    {
                        key = string.Format("cb:{0}:{1}:.*", objectType.FullName, this.Session.TenantId.ToUpper());
                        this.RemoveFromCacheByRegexLocal(key);
                    }
                    else
                    {
                        key = string.Format("cb:{0}:.*", objectType.FullName);
                        this.RemoveFromCacheByRegexLocal(key);
                    }
                    
                    if (PublicReferences.ContainsKey(objectType.FullName))
                    {
                        foreach (string refs in PublicReferences[objectType.FullName])
                        {
                            key = string.Format("urn:{0}:.*", refs);
                            this.RemoveFromCacheByRegexLocal(key);
                        }
                    }
                }

                if (objectType.HasAttribute<PublicReferenceAttribute>())
                {
                    foreach(var refs in objectType.GetCustomAttributes<PublicReferenceAttribute>().Select(nav=>nav.Reference.FullName))
                    {
                        string key = string.Format("urn:{0}:.*", refs);
                        this.RemoveFromCacheByRegexLocal(key);
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
                                IEnumerable<Type> types = model.GetType().GetAssembly().GetTypes().Where(nav => nav.HasAttribute<PublicReferenceAttribute>());
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
            
            WebEas.Context.Current.LocalMachineCache.Set<T>(k, data, expireCacheIn.HasValue ? new TimeSpan(expireCacheIn.Value.Ticks / 6) : new TimeSpan(0, 10, 0));
            Cache.Set<T>(k, data, expireCacheIn ?? new TimeSpan(3, 0, 0));
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
        
        /// <summary>
        /// Removes from cache local.
        /// </summary>
        /// <param name="key">The key.</param>
        protected void RemoveFromCacheLocal(string key)
        {
            string k = key.Replace("!", ":");
            WebEas.Context.Current.LocalMachineCache.Remove(k);
            Cache.Remove(k);
        }
        
        /// <summary>
        /// Removes from cache local.
        /// </summary>
        /// <param name="key">The key.</param>
        protected void RemoveFromCacheByRegexLocal(string pattern)
        {
            string k = pattern.Replace("!", ":");
            WebEas.Context.Current.LocalMachineCache.RemoveByRegex(k);
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
            key = string.Format("ten:{0}:{1}", this.Session.TenantId, key);
            return this.GetCacheOptimized<T>(key, data, expireCacheIn);
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
            key = string.Format("{0}:ten:{1}", key, this.Session.TenantId);
            return this.GetCacheOptimizedLocal<T>(key, data, expireCacheIn);
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
            key = string.Format("{0}:{1}", this.Session.UniqueKey, key);
            return this.GetCacheOptimized<T>(key, data, expireCacheIn);
        }
        
        /// <summary>
        /// Gets the cache optimized session local.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <param name="expireCacheIn">The expire cache in.</param>
        /// <returns></returns>
        public T GetCacheOptimizedSessionLocal<T>(string key, Func<T> data, TimeSpan? expireCacheIn = null)
        {
            key = string.Format("{0}:{1}", this.Session.UniqueKey, key);
            return this.GetCacheOptimizedLocal<T>(key, data, expireCacheIn);
        }

        /// <summary>
        /// Gets the cache optimized.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <param name="expireCacheIn">The expire cache in.</param>
        /// <returns></returns>
        public T GetCacheOptimized<T>(string key, Func<T> data, TimeSpan? expireCacheIn = null)
        {
            T result = Cache.Get<T>(key);
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
                using (var redisClient = RedisManager.GetClient())
                {
                    using (redisClient.AcquireLock(string.Concat(key, ".lock"), lockTime))
                    {
                        data.Invoke();
                    }
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
                return this.GetCacheOptimized<T>(key, data, expireCacheIn);
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
                T result = WebEas.Context.Current.LocalMachineCache.Get<T>(key);
                if (result == null)
                {
                    result = WebEas.Context.Current.LocalMachineCache.Get<T>(key);
                    if (result == null)
                    {
                        result = data.Invoke();
                    }
                    this.SetToCacheLocal<T>(key, result, expireCacheIn);
                }
                return result;
            }
            else
            {
                return this.GetCacheOptimized<T>(key, data, expireCacheIn);
            }
        }
        
        protected void RemoveFromCacheOptimizedTenant(string key)
        {
            key = string.Format("ten:{0}:{1}", this.Session.TenantId, key);
            this.RemoveFromCacheOptimized(key);
        }

        protected void RemoveFromCacheOptimizedSession(string key)
        {
            key = string.Format("{0}:{1}", this.Session.UniqueKey, key);
            this.RemoveFromCacheOptimized(key);
        }
        

        protected void RemoveFromCacheOptimized(string key)
        {
            Cache.Remove(key.Replace("!", ":"));
        }
        
        protected void RemoveFromCacheOptimizedByRegex(string pattern)
        {
            Cache.RemoveByRegex(pattern.Replace("!", ":"));
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


    }
}