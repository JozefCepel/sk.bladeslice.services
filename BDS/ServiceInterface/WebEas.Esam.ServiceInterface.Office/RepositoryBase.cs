using NLog;
using ServiceStack;
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
using WebEas.Esam.ServiceModel.Office;
using WebEas.Esam.ServiceModel.Office.Types;
using WebEas.ServiceInterface;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Dto;
using WebEas.ServiceModel.Office.Egov.Reg.Types;
using WebEas.ServiceModel.Types;

namespace WebEas.Esam.ServiceInterface.Office
{
    /// <summary>
    /// Repository base
    /// </summary>
    public abstract class RepositoryBase : WebEasRepositoryBase, IRepositoryBase
    {

        public RepositoryBase()
        {
            RibbonColumnsToBEFilter = new List<string>
            {
                "Adresa",
                "Adresa_MiestoDrzania",
                "DanovnikFormatovaneMenoSort",
                "DanovnikIdentifikator",
                "ExPrizn",
                "FormatovaneMeno",
                "FormatovaneMenoSort",
                "Identifikator",
                "MenoNazov",
                "OsobaFyzicka_Drzitel",
                "OsobaNazov",
                "PriezviskoMeno",
                "RodneCislo",
                "RodneCisloIco",
                "SparovanaOsoba"
                /*?
                KN_Ownership
                LvNo
                cRegParcelNo
                ParcelNo*/
            };
        }

        /// <summary>
        /// Gets or sets the save to evidence locale.
        /// </summary>
        /// <value>The save to evidence locale.</value>
        protected internal string SaveToEvidenceLocale { get; set; }

        public static readonly Regex RegexXmlns = new Regex("(?<start> (?:xmlns|targetNamespace)=[\"|'][^\"|']+_)(?<locale>\\w+)(?<end>/[^\"|']+)");
        public static readonly Regex RegexLanguage = new Regex("(?<start> Language=[\"|'])(?<locale>\\w+)(?<end>[\"|'])");

        private const string LoggerName = "ServiceRequestDurationLogger";
        private const int DaysTTLLongTime = 7;
        public static readonly Log.WebEasNLogLogger ReqTimeLogger = new Log.WebEasNLogLogger(LoggerName);


        #region List

        /// <summary>
        /// Gets the name of the current user formatted.
        /// </summary>
        /// <value>The name of the current user formatted.</value>
        public string CurrentUserFormattedName
        {
            get
            {
                return this.GetCacheOptimizedLocal(string.Format("curUsFormName:{0}", Session.DcomId), () => this.Db.Scalar<string>("SELECT FullName FROM cfe.V_Users WHERE D_User_Id = @dcomId", new { dcomId = Session.DcomId }), new TimeSpan(24, 0, 0));
            }
        }

        /// <summary>
        /// Gets the data of the current user formatted.
        /// </summary>
        /// <value>The name of the current user formatted.</value>
        public User CurrentUserData
        {
            get
            {
                Guid dcomIdGuid = Session.DcomIdGuid.HasValue ? Session.DcomIdGuid.Value : new Guid(Session.DcomId);
                return this.GetCacheOptimizedLocal(string.Format("curUsIamPouzivatel:{0}", Session.DcomId), () => this.GetPouzivatel(dcomIdGuid), new TimeSpan(24, 0, 0));
            }
        }

        #endregion

        #region Access flags

        public override void SetAccessFlag(object viewData)
        {
            base.SetAccessFlag(viewData);

            //TODO: zmeny specificke pre modul riesit override-om v repositore modulu + volat na zaciatku base.SetAccessFlag(viewData);
            if (viewData == null)
            {
                return;
            }

            if (viewData is IAccessFlag)
            {
                viewData = new ArrayList { viewData };
            }

            if (viewData is IEnumerable)
            {
                var e = viewData as IEnumerable;
                IEnumerable<IAccessFlag> baseEntityList = e.OfType<IAccessFlag>();

                Type entityType = e.GetType().GetProperty("Item").PropertyType;
                IList<PropertyInfo> props = new List<PropertyInfo>(entityType.GetProperties());

                PropertyInfo piRiesitel = props.FirstOrDefault(p => p.Name.ToUpper() == "RIESITEL");
                PropertyInfo piPohladId = props.FirstOrDefault(p => p.Name.ToUpper() == "D_POHLAD_ID");
                PropertyInfo piTenantId = props.FirstOrDefault(p => p.Name.ToUpper() == "D_TENANT_ID");

                string dcomId = this.Session.DcomId.ToUpper();

                PropertyInfo piOsoba = null;
                var pfeColumn = props.Where(p => p.HasAttribute<PfeColumnAttribute>());
                foreach (var col in pfeColumn)
                {
                    var personLink = col.GetCustomAttributes<PfeColumnAttribute>().FirstOrDefault();
                    if (personLink != null && personLink.Xtype == PfeXType.PersonLink)
                    {
                        piOsoba = props.FirstOrDefault(p => p.Name.ToUpper() == personLink.NameField.ToUpper());
                    }
                }

                foreach (IAccessFlag baseEntity in baseEntityList)
                {
                    string riesitel = piRiesitel == null ? null : (string)piRiesitel.GetValue(baseEntity);
                    Guid? tenantId = piTenantId == null ? null : (Guid?)piTenantId.GetValue(baseEntity);
                    int pohladId = piPohladId == null ? 0 : (int?)piPohladId.GetValue(baseEntity) ?? 0;
                    Guid? osobaId = piOsoba == null ? null : (Guid?)piOsoba.GetValue(baseEntity);
                    bool jeRiesitel = string.IsNullOrEmpty(riesitel) || dcomId == riesitel.ToUpper();

                    // AcccessFlag urcuje prava usera na dane akcie pre dany riadok

                    // if ((zbernySpis || jeRiesitel) && (piTenantId == null || tenantId.HasValue || this.Session.HasRole(Roles.SysAdmin)))
                    //{
                        baseEntity.AccessFlag |= (long)(NodeActionFlag.Create | NodeActionFlag.Update | NodeActionFlag.Delete);
                    //}

                    //if (osobaId.HasValue)
                    //{
                    //    baseEntity.AccessFlag |= (long)NodeActionFlag.ZobrazOsobu;
                    //}
                }
            }
        }

        #endregion

        #region Procedure

        /// <summary>
        /// SQLs the procedure.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public new DynamicParameters SqlProcedure(string name, DynamicParameters parameters, int? commandTimeout = null)
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
                LogRequestDuration("SqlProcedure", stopwatch, name);
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

        #region Ulozenie do evidencie / zmena stavu

        /// <summary>
        /// Gets the current state of the entity from DB (reg.D_BiznisEntita).
        /// </summary>
        /// <param name="id">Id of the entity (Corrensponds to D_BiznisEntita_Id)</param>
        /// <returns>C_StavEntity_Id</returns>
        public int GetState(long id)
        {
            //BiznisEntita entity = this.GetById<BiznisEntita>(id);
            //return entity.C_StavEntity_Id;
            return this.Db.Scalar<int>("SELECT C_StavEntity_Id FROM reg.D_BiznisEntita WHERE D_BiznisEntita_Id = @BiznisEntitaId", new { BiznisEntitaId = id });
        }

        /// <summary>
        /// Changes the state for entity which is searched by hierarchy node.
        /// Supports either biznis entity or submission (PodanieView)
        /// </summary>
        /// <param name="state">The state.</param>
        public void ChangeState(IChangeState state)
        {
            HierarchyNode node = Modules.FindNode(state.ItemCode);

            this.ChangeStateBiznisEntita(state.Id, state.IdNewState, state.ePodatelnaId);
        }

        /// <summary>
        /// Check whether the state can be changed from <see cref="actualState"/> to <see cref="newState"/>
        /// Throws WebEasException in case the states are from different state spaces or there is no direct path from actual state to desired state.
        /// </summary>
        /// <remarks>Note: will throw NullReferenceException in case the actual state does not exists!</remarks>
        /// <param name="actualState">Actual state of data</param>
        /// <param name="newState">Desired (new) state fo data</param>
        /// <returns>Information about both states and the state space (C_StavovyPriestor_Id)</returns>
        public PossibleStatesResult CheckPossibleState(int actualState, int newState)
        {
            PossibleStatesResult possibleState = this.Db.Single<PossibleStatesResult>(@"
SELECT so.Kod as OldState, so.C_StavEntity_Id as OldState_Id, sn.Kod as NewState, ss.C_StavEntity_Id_Child as NewState_Id, so.C_StavovyPriestor_Id as EntitySpace
FROM [reg].[C_StavEntity] so
LEFT JOIN [reg].[C_StavEntity] sn ON sn.C_StavEntity_Id = @NewStateId AND sn.C_StavovyPriestor_Id = so.C_StavovyPriestor_Id
LEFT JOIN [reg].[C_StavEntity_StavEntity] ss ON so.C_StavEntity_Id = ss.C_StavEntity_Id_Parent AND sn.C_StavEntity_Id = ss.C_StavEntity_Id_Child AND (ss.DatumPlatnosti is NULL OR ss.DatumPlatnosti > GETDATE())
WHERE so.C_StavEntity_Id = @OldStateId", new { OldStateId = actualState, NewStateId = newState });

            //if (possibleState == null) nemalo by nastat lebo existujuci stav musi byt v db...?
            if (possibleState.NewState == null)
            {
                throw new WebEasException(
                    string.Format("Zmena z ({0}) {1} na ({2}) {3} nie je povolená (stavový priestor: {4})", actualState, possibleState.OldState, newState, "_iny_stavovy_priestor_", possibleState.EntitySpace),
                    "Nový stav je mimo aktuálneho stavového priestoru");
            }
            else if (!possibleState.NewState_Id.HasValue)
            {
                throw new WebEasException(
                    string.Format("Zmena stavu z '{0}' ({1}) na '{2}' ({3}) nie je povolená (stavový priestor: {4})", possibleState.OldState, actualState, possibleState.NewState, newState, possibleState.EntitySpace),
                    string.Format("Zmena stavu z '{0}' na '{1}' nie je v konfigurácii príslušnej oblasti povolená", possibleState.OldState.ToLower(), possibleState.NewState.ToLower()));
            }
            return possibleState;
        }

        /// <summary>
        /// Changes the state for data which implements own state behaviour (outside of reg.D_BiznisEntita) without the DB transaction.
        /// If actual and desired states equals, will do nothing.
        /// Can throw WebEasException in case the states are from different state spaces or there is no direct path from actual state to desired state.
        /// </summary>
        /// <typeparam name="T">Data which implements IBaseEntity and IHasStateId</typeparam>
        /// <param name="entityId">The data id (primary key)</param>
        /// <param name="newStateId">The new state id.</param>
        public void ChangeState<T>(long entityId, int newStateId) where T : class, IBaseEntity, IHasStateId
        {
            T entita = this.Db.SingleById<T>(entityId);

            if (entita.C_StavEntity_Id == newStateId)
            {
                return;
            }

            this.CheckPossibleState(entita.C_StavEntity_Id, newStateId);
            int oldStateId = entita.C_StavEntity_Id;
            try
            {
                entita.C_StavEntity_Id = newStateId;
                //invalidate tree-counts cache
                // this.InvalidateTreeCountsForPath();

                this.UpdateData(entita);
            }
            catch (Exception ex)
            {
                throw new WebEasException("Nastala chyba pri zmene stavu", ex);
            }

        }

        /// <summary>
        /// Changes the state of Biznis entity (in reg.D_BiznisEntita) and log the change to history table (reg.D_Entita_HistoriaStavov) without the DB transaction.
        /// Can throw WebEasException in case the states are from different state spaces or there is no direct path from actual state to desired state.
        /// If actual and desired states equals, will throw exception too.
        /// </summary>
        /// <param name="entityId">The entity id (Corrensponds to D_BiznisEntita_Id)</param>
        /// <param name="newStateId">The new state id.</param>
        /// <param name="ePodatelnaId">The e podatelna id. (for history logging)</param>
        public void ChangeState(long entityId, int newStateId, long? ePodatelnaId = null)
        {
            this.ChangeState(entityId, newStateId, false, ePodatelnaId);
        }

        /// <summary>
        /// Changes the state of Biznis entity (in reg.D_BiznisEntita) and log the change to history table (reg.D_Entita_HistoriaStavov) without the DB transaction.
        /// Can throw WebEasException in case the states are from different state spaces or there is no direct path from actual state to desired state.
        /// Do not throw exception if actual and new state are same and ignoreIfSame is true.
        /// </summary>
        /// <param name="entityId">The entity id (Corrensponds to D_BiznisEntita_Id)</param>
        /// <param name="newStateId">The new state id.</param>
        /// <param name="ignoreIfSame">Whether to not throw exception if old and new states are same</param>
        /// <param name="ePodatelnaId">The e podatelna id. (for history logging)</param>
        public void ChangeState(long entityId, int newStateId, bool ignoreIfSame, long? ePodatelnaId)
        {
            if (ePodatelnaId == 0)// kontrola na chybny vstup
            {
                ePodatelnaId = null;
            }

            BiznisEntita entita = this.Db.SingleById<BiznisEntita>(entityId);

            //same and allow?
            if (ignoreIfSame && entita.C_StavEntity_Id == newStateId)
            {
                return;
            }

            //this call can throw exceptions...
            PossibleStatesResult stateInfo = this.CheckPossibleState(entita.C_StavEntity_Id, newStateId);

            int oldStateId = entita.C_StavEntity_Id;
            entita.C_StavEntity_Id = newStateId;

            string polozkaStromu = entita.PolozkaStromu;

            this.UpdateData(entita);

            var historia = new EntitaHistoriaStavov
            {
                ZmenaStavuDatum = DateTime.Now,
                D_Tenant_Id = entita.D_Tenant_Id,
                C_StavEntity_Id_New = newStateId,
                C_StavEntity_Id_Old = stateInfo.OldState_Id,
                C_StavovyPriestor_Id = entita.C_StavovyPriestor_Id,
                D_BiznisEntita_Id = entita.D_BiznisEntita_Id,
                C_TypBiznisEntity_Id = entita.C_TypBiznisEntity_Id,
                //D_Podanie_Id = null,
                //IDVytvorenehoZaznamuVPodatelni = ePodatelnaId
                // TODO: ePodatelnaId pojde z eSamu pravdepodobne cele prec
            };
            this.InsertData(historia);

            //invalidate tree-counts cache
            if (!string.IsNullOrEmpty(polozkaStromu))
            {
                this.InvalidateTreeCountsForPath(polozkaStromu);
            }
        }

        /// <summary>
        /// Changes the state of Biznis entity (in reg.D_BiznisEntita) and log the change to history table (reg.D_Entita_HistoriaStavov) inside the DB transaction.
        /// Can throw WebEasException in case the states are from different state spaces or there is no direct path from actual state to desired state.
        /// If actual and desired states equals, will throw exception too.
        /// </summary>
        /// <remarks>Same as ChangeState() but in transaction</remarks>
        /// <param name="entityId">The entity id (Corrensponds to D_BiznisEntita_Id)</param>
        /// <param name="newStateId">The new state id.</param>
        /// <param name="ePodatelnaId">The e podatelna id.</param>
        public void ChangeStateBiznisEntita(long entityId, int newStateId, long? ePodatelnaId)
        {
            using (System.Data.IDbTransaction transaction = this.BeginTransaction())
            {
                try
                {
                    this.BeforeChangeState(entityId, newStateId);

                    this.ChangeState(entityId, newStateId, false, ePodatelnaId);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new WebEasException("Nastala chyba pri zmene stavu", ex);
                }
            }
        }

        /// <summary>
        /// Perform custom logic before state is changed.
        /// </summary>
        /// <param name="entityId"><see cref="BiznisEntita"/> Id.</param>
        /// <param name="newStateId">Id of the new state.</param>
        protected virtual void BeforeChangeState(long entityId, int newStateId)
        {
        }

        #endregion


        #region Environment
        
        private static string dbEnvironment;
        private static DateTime? dbDeployTime;

        /// <summary>
        /// Gets the db environment.
        /// </summary>
        /// <value>The db environment.</value>
        public string DbEnvironment
        {
            get
            {
                if (dbEnvironment == null)
                {
                    this.LoadDatabaseEnvironment();
                }
                return dbEnvironment;
            }
        }
        
        /// <summary>
        /// Gets the db deploy time.
        /// </summary>
        /// <value>The db deploy time.</value>
        public DateTime? DbDeployTime
        {
            get
            {
                if (!dbDeployTime.HasValue)
                {
                    this.LoadDatabaseEnvironment();
                }
                return dbDeployTime;
            }
        }

        private void LoadDatabaseEnvironment()
        {
            try
            {
                DbEnvironment data = this.Db.Single<DbEnvironment>(
                    "select reg.F_NastavenieS(null,'reg','Server',null) as Environment," +
                    "reg.F_NastavenieT(default,'reg','Deploy',default) as DeployTime");
                dbEnvironment = data.Environment;
                dbDeployTime = data.DeployTime;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        #endregion

        #region Helpers
        
        /// <summary>
        /// Returns Filter instance from given lambda expression using ServiceStack.OrmLite.SqlServer.SqlServerExpression.Where()
        /// </summary>
        /// <remarks>
        /// Method is defined for debugging purpose only (may be removed in future)
        /// </remarks>
        public Filter GetFilter1<T>(Expression<Func<T, bool>> filter)
        {
            SqlExpression<T> s = new ServiceStack.OrmLite.SqlServer.SqlServerExpression<T>(this.Db.GetDialectProvider()).Where(filter);
            return Filter.FromSql(s.WhereExpression);
        }
        
        /// <summary>
        /// Returns Filter instance from given lambda expression using FilterExpression.DecodeFilter()
        /// </summary>
        /// <remarks>
        /// Method is defined for debugging purpose only (may be removed in future)
        /// </remarks>
        public Filter GetFilter2<T>(Expression<Func<T, bool>> filter)
        {
            return WebEasFilterExpression.DecodeFilter(filter);
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
        /// Gets the pouzivatel info from database.
        /// </summary>
        public User GetPouzivatel(Guid uid)
        {
            //Ked som posielal do metody priamo UID, tak to pri urcitych GUID-och nefungovalo. Po zmene na string je to OK
            return this.Db.SingleById<User>(uid.ToString());
        }

        public void LogRequestDuration(string serviceUrl, Stopwatch stopwatch, [System.Runtime.CompilerServices.CallerMemberName] string operation = "")
        {
            var logEventInfo = new LogEventInfo(LogLevel.Info, LoggerName, null);
            logEventInfo.Properties["ElapsedMilliseconds"] = stopwatch.ElapsedMilliseconds;
            logEventInfo.Properties["ServiceUrl"] = serviceUrl;
            logEventInfo.Properties["Operation"] = operation;
            logEventInfo.Properties["TenantId"] = Session.TenantId;
            logEventInfo.Properties["DcomId"] = Session.DcomId;
            ReqTimeLogger.LogInfo(logEventInfo);
        }

        public void LogRequestDuration(string serviceUrl, long elapsedMilliseconds, [System.Runtime.CompilerServices.CallerMemberName] string operation = "")
        {
            var logEventInfo = new LogEventInfo(LogLevel.Info, LoggerName, null);
            logEventInfo.Properties["ElapsedMilliseconds"] = elapsedMilliseconds;
            logEventInfo.Properties["ServiceUrl"] = serviceUrl;
            logEventInfo.Properties["Operation"] = operation;
            logEventInfo.Properties["TenantId"] = Session.TenantId;
            logEventInfo.Properties["DcomId"] = Session.DcomId;
            ReqTimeLogger.LogInfo(logEventInfo);
        }

        #endregion Helpers

        #region Tree counts (cached)
        
        /// <summary>
        /// Helper class used to lock cache access by code (avoid simultaneous cache updates for same key..)
        /// </summary>
        private static readonly SyncHelper treeCountsLockObjects = new SyncHelper();
        
        /// <summary>
        /// Get the counts associated with specified node by code (kod polozky)
        /// </summary>
        /// <param name="code">Kod polozky</param>
        /// <param name="countState">count of rows with state (specified by node)</param>
        /// <param name="countAll">count of all rows</param>
        public void GetTreeCounts(string code, out int countState, out int countAll)
        {
            //set as 'dont show' (for more info check the HierarchyNode class..)
            countState = -1;
            countAll = -1;
            
            HierarchyNode node = Modules.TryFindNode(code);
            if (node != null)
            {
                if (node.RowsCounterRule == -3)
                {
                    countState = -3;
                    if (node.CountAllRows)
                    {
                        countAll = -3;
                    }
                }
                else if (node.RowsCounterRule != 0)
                {
                    long? value = null;
                    //avoid simultaneous update of same key..
                    lock (treeCountsLockObjects.GetSyncObject(code))
                    {
                        //avoid invalidate all cache items at once?
                        var rnd = new Random();
                        int lifetimeMin = 50 + rnd.Next(15); //random from 50 - 65 mins
                        
                        string key = string.Format("treecounts:{0}:{1}:{2}", this.Session.TenantId, code, node.RowsCounterRule).ToLowerInvariant();
                        value = this.GetCacheOptimized<long?>(key, () =>
                        {
                            System.Diagnostics.Debug.WriteLine(string.Format("{0}__{1}", System.Threading.Thread.CurrentThread.ManagedThreadId.ToString(), key));
                            var filter = new Filter();
                            if (node.AdditionalFilter != null)
                            {
                                filter.Append(node.AdditionalFilter.Clone());
                            }
                            MethodInfo method = this.GetType().GetMethod("Count", new Type[] { typeof(Filter) });
                            MethodInfo func = method.MakeGenericMethod(node.ModelType);
                            
                            //pocet vsetky zaznamov
                            int allrows = -1;
                            if (node.CountAllRows)
                            {
                                allrows = (int)func.Invoke(this, new object[] { filter });
                            }
                            
                            //pocet zaznamov daneho stavu (moze byt 'hocijaky stav')
                            if (node.RowsCounterRule > 0)
                            {
                                //Pocty poloziek uz neriesime pre konkretny stav, ale napocitat sa maju tie polozky, ktore maju stav "nekoncový"
                                //filter.AndEq("C_StavEntity_Id", node.RowsCounterByStateId);
                                filter.AndEq("JeKoncovyStav", 0);
                            }
                            var rows = (int)func.Invoke(this, new object[] { filter });
                            
                            return (long?)((long)rows << 32) | (long)(uint)allrows;
                        }, TimeSpan.FromMinutes(lifetimeMin));
                    }
                    
                    if (value.HasValue)
                    {
                        countState = (int)(value.Value >> 32);
                        countAll = (int)(value.Value & 0x00000000ffffffff);
                    }
                }
            }
        }

        /// <summary>
        /// Invalidate tree counts for given (sub)path
        /// If null/empty string is passed, all counts will be invalidated (in all modules)
        /// </summary>
        protected void InvalidateTreeCountsForPath(string path)
        {
            string pattern = string.Format(@"treecounts:{0}:{1}.*", this.Session.TenantId, path).ToLowerInvariant();
            this.RemoveFromCacheOptimizedByRegex(pattern);
        }

        #endregion

        #region Long Operation

        private delegate void LongOperationProcessDelegate(EsamSession session, string processKey, Guid corrId, string operationName, string operationParameters);
        
        //START
        public LongOperationStatus LongOperationStart(string operationName, string operationParameters, string operationInfo)
        {
            if (string.IsNullOrEmpty(operationParameters))
                throw new WebEasValidationException(null, "Parametre pre danú operáciu nie sú špecifikované!");

            // Operacia bude vzdy unikatna
            // int hashCode = this.LongOperationGetHashCode(operationParameters);
            // string processKey = string.Format("dap!{0}!{1}", operationName, hashCode.ToString());
            string processKey = $"LongOperation!{ActualModul}!{Session.TenantId}!{Session.DcomId}!{operationName}!{Guid.NewGuid().ToString()}_{DateTimeOffset.Now.ToUnixTimeMilliseconds()}";

            LongOperationInfo longOperationInfo = null;
            if (!string.IsNullOrEmpty(operationInfo))
            {
                longOperationInfo = Encoding.UTF8.GetString(Convert.FromBase64String(operationInfo)).FromJson<LongOperationInfo>();
            }

            var operationStatus = new LongOperationStatus(processKey)
            {
                Percents = 1,
                CorrId = WebEas.Context.Current.CurrentCorrelationID,
                OperationName = operationName,
                OperationInfo = operationInfo,
                OperationParameters = operationParameters,
                Count = longOperationInfo?.Pocet ?? 0,
                Start = DateTime.Now,
                DcomId = Session.DcomId
            };

            operationStatus.State = LongOperationState.Waiting;
            operationStatus.Changed = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            SetToCache(processKey, operationStatus, new TimeSpan(DaysTTLLongTime, 0, 0, 0));

            var processDelegate = new LongOperationProcessDelegate(LongOperationProcess);
            processDelegate.BeginInvoke(Session as EsamSession, processKey, WebEas.Context.Current.CurrentCorrelationID, operationName, System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(operationParameters)), null, null);
            return new LongOperationStatus { CorrId = WebEas.Context.Current.CurrentCorrelationID, ProcessKey = processKey, Message = "Operácia je zaradená do spracovania" };
        }

        //RESTART
        public LongOperationStatus LongOperationRestart(string processKey)
        {
            var status = GetFromCache<LongOperationStatus>(processKey);

            if (status == null)
            {
                return new LongOperationStatus { ProcessKey = processKey, Message = "Operácia nenájdená!" };
            }

            if (status.State == LongOperationState.Running || status.State == LongOperationState.Waiting)
            {
                return new LongOperationStatus { ProcessKey = processKey, Message = "Operácia už čaká na dokončenie!" };
            }

            /*status.State = LongOperationState.Restored;
            status.Changed = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            SetToCache(processKey, status, new TimeSpan(DaysTTLLongTime, 0, 0));*/
            RemoveFromCache(processKey);
            return LongOperationStart(status.OperationName, status.OperationParameters, status.OperationInfo);
        }

        //STATUS
        public LongOperationStatus LongOperationStatus(string processKey)
        {
            LongOperationStatus status = this.GetFromCache<LongOperationStatus>(processKey);
            if (status == null)
            {
                return new LongOperationStatus { ProcessKey = processKey, Message = "Operácia nenájdená!" };
            }
            if (status.IsFinished && status.HasAttachement)//avoid sent attachment ...
            {
                WebEas.Context.Current.CurrentCorrelationID = status.CorrId;
                
                return new LongOperationStatus
                {
                    CorrId = status.CorrId,
                    ProcessKey = status.ProcessKey,
                    Count = status.Count,
                    Current = status.Current,
                    Error = status.Error,
                    Message = status.Message,
                    NotClose = status.NotClose,
                    Percents = status.Percents,
                    Start = status.Start,
                    HasAttachement = true,
                    //Result = new WebEas.Egov.Reports.RendererResult { DocumentName = ((WebEas.Egov.Reports.RendererResult)status.Result).DocumentName }
                };
            }
            
            return status;
        }
        
        //RESULT
        public object LongOperationResult(string processKey)
        {
            LongOperationStatus status = this.GetFromCache<LongOperationStatus>(processKey);
            if (status == null)
            {
                throw new WebEasValidationException(string.Format("Process key {0} not found", processKey), "Operácia nenájdená!");
            }
            WebEas.Context.Current.CurrentCorrelationID = status.CorrId;
            if (!status.IsFinished)
            {
                throw new WebEasValidationException(string.Format("Process in wrong state {0} {1}", processKey, status.Message), "Operácia nie je ukončená!");
            }
            
            // TODO : zatial vypnute, pre novy info panel
            //RemoveFromCache(processKey);
            return status.Result;
        }
        
        //CANCEL
        public LongOperationStatus LongOperationCancel(string processKey, bool notRemove)
        {
            var status = GetFromCache<LongOperationStatus>(processKey);
            if (status != null)
            {
                WebEas.Context.Current.CurrentCorrelationID = status.CorrId;
                if (notRemove)
                {
                    status.State = LongOperationState.Canceled;
                    status.Changed = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                    SetToCache(processKey, status, new TimeSpan(DaysTTLLongTime, 0, 0));
                }
                else
                    RemoveFromCache(processKey);
            }
            
            return new LongOperationStatus { ProcessKey = processKey, Message = "Proces zrušený!", CorrId = WebEas.Context.Current.CurrentCorrelationID };
        }

        //LIST
        public IOrderedEnumerable<LongOperationStatus> LongOperationList(bool perTenant, int skip, int take)
        {
            using (var redisClient = RedisManager.GetClient())
            {
                var redisKeys = perTenant
                ? redisClient.ScanAllKeys($"LongOperation:{ActualModul}:{Session.TenantId}:*")
                : redisClient.ScanAllKeys($"LongOperation:{ActualModul}:{Session.TenantId}:{Session.DcomId}:*");
                IOrderedEnumerable<LongOperationStatus> sortedOperations = new List<LongOperationStatus>().OrderBy(x => x.Changed);

                if (redisKeys.Any())
                {
                    var longOperations = redisClient.GetAll<LongOperationStatus>(redisKeys);
                    if (longOperations.Any())
                    {
                        sortedOperations = longOperations.Values.Skip(skip).Take(take == 0 ? 10 : take).OrderByDescending(x => x.Changed);
                    }
                }

                return sortedOperations;
            }
        }

        protected void LongOperationProcess(EsamSession session, string processKey, Guid corrId, string operationName, string operationParameters)
        {
            try
            {
                WebEas.Context.Current.CurrentCorrelationID = corrId;
                //pojdeme na ESB tenantnym volanim pri LongTime operaciach
                var newSession = session.ToJson().FromJson<EsamSession>();
                newSession.IamDcomToken = null;
                SetToCache(newSession.UniqueKey, newSession, new TimeSpan(DaysTTLLongTime, 0, 0));
                SetSession(newSession);
                ServiceStack.OrmLite.OrmLiteConfig.CommandTimeout = 3600; //docasne zapnutie vacsieho timeoutu
                LongOperationProcess(processKey, operationName, operationParameters);
            }
            catch (WebEasValidationException ex)
            {
                LongOperationSetState(processKey, ex);
            }
            catch (Exception ex)
            {
                var error = new WebEasException(string.Format("Error in operation {0} with parameters {1}. Process key {2} and sessionkey {3}", operationName, WebEas.Core.Log.WebEasNLogExtensions.ToJsonString(operationParameters), processKey, session.UniqueKey), ex);
                LongOperationSetState(processKey, error);
            }
            finally
            {
                ServiceStack.OrmLite.OrmLiteConfig.CommandTimeout = 60;
            }
        }
        
        protected virtual void LongOperationProcess(string processKey, string operationName, string operationParameters)
        {
            throw new WebEasException(string.Format("Long operation with the name {0} is not implemented", operationName), "Operácia nie je implementovaná!");
        }
        
        protected void LongOperationSetState(string processKey, WebEasException ex)
        {
            if (string.IsNullOrEmpty(processKey))
            {
                throw ex;
            }

            var status = GetFromCache<LongOperationStatus>(processKey) ?? new LongOperationStatus(processKey);

            if (ex.GetType() == typeof(WebEasValidationException))
            {
                status.Message = ex.MessageUser;
            }
            else
            {
                status.Message = string.Format("{0} Kontaktujte Call Centrum s kódom: {1}", ex.MessageUser, ex.GetIdentifier());
            }
            
            status.Error = ex;
            //status.Count = 0;
            status.CorrId = WebEas.Context.Current.CurrentCorrelationID;
            status.State = LongOperationState.Failed;
            status.Changed = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            SetToCache(processKey, status, new TimeSpan(DaysTTLLongTime, 0, 0));
            Log.Error(ex);
        }
        
        protected void LongOperationSetState(string processKey, string message = null, long? current = null, long? count = null, LongOperationState? state = null)
        {
            if (string.IsNullOrEmpty(processKey))
            {
                return;
            }
            var status = GetFromCache<LongOperationStatus>(processKey) ?? new LongOperationStatus(processKey);
            status.Message = string.IsNullOrEmpty(message) ? status.Message : message;
            status.Current = current ?? status.Current;
            status.Count = count ?? status.Count;
            status.Percents = 1;
            status.CorrId = WebEas.Context.Current.CurrentCorrelationID;
            status.State = state ?? status.State;
            status.Changed = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            SetToCache(processKey, status, new TimeSpan(DaysTTLLongTime, 0, 0));
        }

        protected void LongOperationSetStateFinished(string processKey, string message, object result, long? current = null, long? count = null, LongOperationState? state = null, string reportId = null)
        {
            if (string.IsNullOrEmpty(processKey))
            {
                return;
            }
            var status = GetFromCache<LongOperationStatus>(processKey) ?? new LongOperationStatus(processKey);
            status.Message = message;
            status.Result = result;

            if (status.Result == null)
            {
                status.Result = message;
            }

            //status.HasAttachement = result is WebEas.Egov.Reports.RendererResult;

            if (result != null)
            {
                status.Current = current ?? status.Current;
                status.Count = count ?? status.Count;
            }
            else
            {
                status.Current = status.Count;
            }
            
            status.Percents = 100;
            status.NotClose = true;
            status.CorrId = WebEas.Context.Current.CurrentCorrelationID;
            status.State = state ?? status.State;
            status.Changed = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            status.ReportId = reportId ?? status.ReportId;
            SetToCache(processKey, status, new TimeSpan(DaysTTLLongTime, 0, 0));
        }
        
        /// <summary>
        /// Longs the operation set state message.
        /// </summary>
        /// <param name="processKey">The process key.</param>
        /// <param name="message">The message.</param>
        protected void LongOperationSetStateMessage(string processKey, string message)
        {
            if (string.IsNullOrEmpty(processKey))
            {
                return;
            }
            var status = GetFromCache<LongOperationStatus>(processKey) ?? new LongOperationStatus(processKey);
            status.Message = message;
            //status.Count = 0;
            //status.Current = status.Count;
            //status.Percents = 100;
            status.NotClose = true;
            //status.Start = DateTime.Now;
            status.CorrId = WebEas.Context.Current.CurrentCorrelationID;
            SetToCache(processKey, status, new TimeSpan(DaysTTLLongTime, 0, 0));
        }
        
        private int LongOperationGetHashCode(string operationParameters)
        {
            return operationParameters.ToString().GetHashCode();
            /*int hashCode = 17;
            Dictionary<string, object> data = ServiceStack.Text.TypeSerializer.DeserializeFromString<Dictionary<string, object>>(operationParameters.ToString());
            foreach (object item in data.Values)
            {
                hashCode = hashCode * 23 + item.GetHashCode();
            }
            
            return hashCode;*/
        }

        #region Progress report functions

        /// <summary>
        /// Pomocna funkcia na oznamenie progresu.
        /// Vygeneruje message v tvare: "Operácia {action }bola spustená."
        /// </summary>
        protected void LongOperationStarted(string processKey, string action)
        {
            LongOperationSetState(processKey, message: $"Operácia \"{action}\" bola spustená.");
        }
        
        /// <summary>
        /// Pomocna funkcia na oznamenie progresu.
        /// Vygeneruje message v tvare: "Operácia {action}, riadok {row+1}/{total}" ak nieje zadany message
        /// </summary>
        protected void LongOperationReportProgress(string processKey, string action, int row, int count, string message = null)
        {
            if (string.IsNullOrEmpty(message))
            {
                message = $"Operácia \"{action}\", riadok {row + 1}/{count}";
            }

            LongOperationSetState(processKey, message: message, current: row, count: count);
        }
        
        /// <summary>
        /// Pomocna funkcia na oznamenie vysledku.
        /// Vygeneruje result v tvare: "Operácia {action} je ukončená. Počet úspešne spracovaných riadkov: {row} z celkového počtu {total}."
        /// </summary>
        protected void LongOperationFinished(string processKey, string action, int succeeded, int count)
        {
            string result = $"Operácia \"{action}\" je ukončená. Počet úspešne spracovaných riadkov: {succeeded} z celkového počtu {count}.";
            LongOperationSetStateFinished(processKey, string.Empty, result, succeeded, count, LongOperationState.Done);
        }
        
        /// <summary>
        /// Pomocna funkcia na oznamenie vysledku.
        /// Vygeneruje result v tvare: "Operácia {action} sa skončila (ne)úspešne."
        /// </summary>
        protected void LongOperationFinished(string processKey, string action, bool successfully)
        {
            string result = $"Operácia \"{action}\" sa skončila {(successfully ? "úspešne" : "neúspešne")}.";
            LongOperationSetStateFinished(processKey, string.Empty, result, state : successfully ? LongOperationState.Done : LongOperationState.Failed);
        }
        
        /// <summary>
        /// Pomocna funkcia na oznamenie vysledku.
        /// Vygeneruje result v tvare: "Operácia {action} je ukončená. {customText}."
        /// </summary>
        protected void LongOperationFinished(string processKey, string action, string customText)
        {
            string result = $"Operácia \"{action}\" je ukončená. {customText}";
            LongOperationSetStateFinished(processKey, string.Empty, result, state : LongOperationState.Done);
        }
        
        /// <summary>
        /// Pomocna funkcia na oznamenie vysledku s vlastnym resultom.
        /// Vygeneruje message v tvare: "Operácia {action} sa skončila (ne)úspešne."
        /// </summary>
        protected void LongOperationFinishedWithObject(string processKey, string action, bool successfully, object result, string reportId)
        {
            string message = $"Operácia \"{action}\" sa skončila {(successfully ? "úspešne" : "neúspešne")}.";
            LongOperationSetStateFinished(processKey, message, result, state: successfully ? LongOperationState.Done : LongOperationState.Failed, reportId: reportId);
        }
        
        /// <summary>
        /// Pomocna funkcia na oznamenie vysledku s vlastnym resultom.
        /// Vygeneruje message v tvare: "Operácia {action} je ukončená. Počet úspešne spracovaných riadkov: {row} z celkového počtu {total}."
        /// </summary>
        protected void LongOperationFinishedWithObject(string processKey, string action, int succeeded, int total, object result, string reportId)
        {
            string message = $"Operácia \"{action}\" je ukončená. Počet úspešne spracovaných riadkov: {succeeded} z celkového počtu {total}.";
            LongOperationSetStateFinished(processKey, message, result, state : LongOperationState.Done, reportId: reportId);
        }
        
        /// <summary>
        /// Pomocna funkcia na oznamenie vysledku s vlastnym resultom.
        /// Vygeneruje result v tvare: "Operácia {action} je ukončená. {customText}."
        /// </summary>
        protected void LongOperationFinishedWithObject(string processKey, string action, string customText, object result, string reportId)
        {
            string message = $"Operácia \"{action}\" je ukončená. {customText}";
            LongOperationSetStateFinished(processKey, message, result, state: LongOperationState.Done, reportId: reportId);
        }

        #endregion

        #endregion

        #region Nastavenie

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
                    return Db.Select<string>(string.Format("select reg.F_NastavenieS('{0}','{1}','{2}','{3}')", Session.TenantId, modul, kod, Session.DcomId)).FirstOrDefault();
                });
            }
        }

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
                    return Db.Select<long?>(string.Format("select reg.F_NastavenieI('{0}','{1}','{2}','{3}')", Session.TenantId, modul, kod, Session.DcomId)).FirstOrDefault();
                });

                return res.GetValueOrDefault();
            }
        }

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
                    return Db.Select<bool?>(string.Format("select reg.F_NastavenieB('{0}','{1}','{2}','{3}')", Session.TenantId, modul, kod, Session.DcomId)).FirstOrDefault();
                });

                return res.GetValueOrDefault();
            }
        }

        #endregion
    }
}