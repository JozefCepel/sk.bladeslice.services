using ClosedXML.Excel;
using NLog;
using ServiceStack;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
//using WebEas.Esam.DcomWs.IsoDap;
//using WebEas.Esam.DcomWs.IsoPla;
using WebEas.Esam.Reports;
using WebEas.Esam.Reports.Uct.Types;
using WebEas.Esam.ServiceModel.Office;
using WebEas.Esam.ServiceModel.Office.Cfe.Types;
using WebEas.Esam.ServiceModel.Office.Dto;
using WebEas.Esam.ServiceModel.Office.Types.Fin;
using WebEas.Esam.ServiceModel.Office.Types.Osa;
using WebEas.Esam.ServiceModel.Office.Types.Reg;
using WebEas.Esam.ServiceModel.Office.Types.Rzp;
using WebEas.Esam.ServiceModel.Office.Types.Uct;
using WebEas.Esam.ServiceModel.Office.Uct.Types;
using WebEas.ServiceInterface;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Dto;
using WebEas.ServiceModel.Office.Egov.Reg.Types;
using WebEas.ServiceModel.Types;
using static WebEas.Esam.Reports.Rzp.Types.ZostavaRzpDennik;
using static WebEas.Esam.Reports.Uct.Types.ZostavaUctDennik;
using static WebEas.Esam.Reports.Uct.Types.ZostavaUctDoklad;

namespace WebEas.Esam.ServiceInterface.Office
{
    /// <summary>
    /// Repository base
    /// </summary>
    public abstract class RepositoryBase : WebEasRepositoryBase, IRepositoryBase
    {
        private const string LoggerName = "ServiceRequestDurationLogger";
        private const int DaysTTLLongTime = 1;
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
                return this.GetCacheOptimizedLocal(string.Format("curUsFormName:{0}", Session.UserId), () => this.Db.Scalar<string>("SELECT FullName FROM cfe.V_Users WHERE D_User_Id = @dcomId", new { dcomId = Session.UserId }), new TimeSpan(24, 0, 0));
            }
        }

        /// <summary>
        /// Gets the name of the current user formatted.
        /// </summary>
        /// <value>The name of the current user formatted.</value>
        public string CurrentCompanyName
        {
            get
            {
                return this.GetCacheOptimizedLocal(string.Format("curCompName:{0}", Session.TenantId), () => this.Db.Scalar<string>("SELECT TOP 1 NAZOV1 + ', ' + OBEC_S FROM dbo.K_OWN_0"), new TimeSpan(24, 0, 0));
            }
        }

        /// <summary>
        /// Gets the name of the current user formatted.
        /// </summary>
        /// <value>The name of the current user formatted.</value>
        public string CurrentYear
        {
            get
            {
                return this.Db.Scalar<string>("SELECT TOP 1 Rok FROM dbo.K_OWN_0");
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
                Guid dcomIdGuid = Session.UserIdGuid.HasValue ? Session.UserIdGuid.Value : new Guid(Session.UserId);
                return this.GetCacheOptimizedLocal(string.Format("curUsIamPouzivatel:{0}", Session.UserId), () => this.GetPouzivatel(dcomIdGuid), new TimeSpan(24, 0, 0));
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
                string dcomId = this.Session.UserId.ToUpper();

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

                    if (piTenantId == null || tenantId.HasValue || this.Session.AdminLevel == AdminLevel.SysAdmin)
                    {
                        baseEntity.AccessFlag |= (long)(NodeActionFlag.Update | NodeActionFlag.ZmenaStavu | NodeActionFlag.Delete);
                    }

                    if (osobaId.HasValue)
                    {
                        baseEntity.AccessFlag |= (long)NodeActionFlag.ZobrazOsobu;
                    }
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
            ChangeStateBiznisEntita(state);
        }

        /// <summary>
        /// Check whether the state can be changed from <see cref="actualState"/> to <see cref="newState"/>
        /// Throws WebEasException in case the states are from different state spaces or there is no direct path from actual state to desired state.
        /// </summary>
        /// <remarks>Note: will throw NullReferenceException in case the actual state does not exists!</remarks>
        /// <param name="actualState">Actual state of data</param>
        /// <param name="newState">Desired (new) state fo data</param>
        /// <param name="stavovyPriestor">Stavovy priestor v ktorom sa to ma skontrolovat</param>
        /// <returns>Information about both states</returns>
        public PossibleStatesResult CheckPossibleState(int actualState, int newState, int stavovyPriestor)
        {
            string sSql;
            sSql = @"SELECT 
                             SO.Kod AS OldState,
                             SO.C_StavEntity_Id AS OldState_Id,
                             SN.Kod AS NewState,
                             SS.C_StavEntity_Id_Child AS NewState_Id,
                             SS.C_StavovyPriestor_Id AS EntitySpace
                        FROM reg.C_StavEntity SO
                        LEFT JOIN reg.C_StavEntity SN ON SN.C_StavEntity_Id = @NewStateId
                        LEFT JOIN reg.C_StavEntity_StavEntity SS ON
                             SS.C_StavovyPriestor_Id = @StavovyPriestorId AND
                             SS.C_StavEntity_Id_Parent = so.C_StavEntity_Id AND
                             SS.C_StavEntity_Id_Child = sn.C_StavEntity_Id AND
                             (SS.DatumPlatnosti IS NULL OR SS.DatumPlatnosti > GETDATE())
                        WHERE SO.C_StavEntity_Id = @OldStateId";
            PossibleStatesResult possibleState = this.Db.Single<PossibleStatesResult>(sSql, new { OldStateId = actualState, NewStateId = newState, StavovyPriestorId = stavovyPriestor });

            //if (possibleState == null) nemalo by nastat lebo existujuci stav musi byt v db...?
            if (possibleState.NewState == null)
            {
                throw new WebEasException(
                    string.Format("Zmena z ({0}) {1} na ({2}) {3} nie je povolená (stavový priestor: {4})", actualState, possibleState.OldState, newState, "_iny_stavovy_priestor_", stavovyPriestor),
                    "Nový stav je mimo aktuálneho stavového priestoru");
            }
            else if (!possibleState.NewState_Id.HasValue)
            {
                throw new WebEasException(
                    string.Format("Zmena stavu z '{0}' ({1}) na '{2}' ({3}) nie je povolená (stavový priestor: {4})", possibleState.OldState, actualState, possibleState.NewState, newState, stavovyPriestor),
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
        /// <param name="stavovyPriestor">Stavovy priestor v ktorom sa to ma skontrolovat</param>
        public int ChangeState<T>(long entityId, int newStateId, int stavovyPriestor) where T : class, IBaseEntity, IHasStateId
        {
            T entita = this.Db.SingleById<T>(entityId);

            int oldStateId = entita.C_StavEntity_Id;
            if (oldStateId == newStateId)
            {
                return oldStateId;
            }
            this.CheckPossibleState(entita.C_StavEntity_Id, newStateId, stavovyPriestor);
            try
            {
                entita.C_StavEntity_Id = newStateId;
                //invalidate tree-counts cache
                // this.InvalidateTreeCountsForPath();

                this.UpdateData(entita);

                return oldStateId;
            }
            catch (Exception ex)
            {
                throw new WebEasException(null, "Nastala chyba pri zmene stavu", ex);
            }

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
        public void ChangeState(IChangeState state, bool ignoreIfSame)
        {
            var entita = GetById<BiznisEntita>(state.Id);

            //same and allow?
            if (ignoreIfSame && entita.C_StavEntity_Id == state.IdNewState)
            {
                return;
            }

            //this call can throw exceptions...
            CheckPossibleState(entita.C_StavEntity_Id, state.IdNewState, entita.C_StavovyPriestor_Id);
            ChangeStateBiznisEntita(entita, state.IdNewState, vyjadrenieSpracovatela: state.VyjadrenieSpracovatela);

            //invalidate tree-counts cache
            if (!string.IsNullOrEmpty(entita.PolozkaStromu))
            {
                InvalidateTreeCountsForPath(entita.PolozkaStromu);
            }
        }

        public void ChangeStateBiznisEntita(BiznisEntita be, int idNewState, string vyjadrenieSpracovatela = null)
        {
            int oldStateId = be.C_StavEntity_Id;
            be.C_StavEntity_Id = idNewState;

            UpdateData(be);

            var kodPolozkyClean = HierarchyNodeExtensions.RemoveParametersFromKodPolozky(HierarchyNodeExtensions.CleanKodPolozky(be.PolozkaStromu.ToLower()));
            var moduleCode = kodPolozkyClean.Split('-')[0];

            var historia = new EntitaHistoriaStavov
            {
                DatumZmenaStavu = DateTime.Now,
                D_Tenant_Id = be.D_Tenant_Id,
                C_StavEntity_Id_New = idNewState,
                C_StavEntity_Id_Old = oldStateId,
                C_StavovyPriestor_Id = be.C_StavovyPriestor_Id,
                D_BiznisEntita_Id = be.D_BiznisEntita_Id,
                C_TypBiznisEntity_Id = be.C_TypBiznisEntity_Id,
                C_Modul_Id = EsamModules.Single(x => x.Kod == moduleCode).C_Modul_Id,
                VyjadrenieSpracovatela = vyjadrenieSpracovatela
            };
            InsertData(historia);
        }

        public void ZmenaStavuDokladov(List<BiznisEntita> biznisEntita, int idNewState, string processKey)
        {
            foreach (var be in biznisEntita)
            {
                LongOperationSetStateMessage(processKey, $"Zmena stavu dokladu ({biznisEntita.IndexOf(be) + 1}/{biznisEntita.Count})");
                ChangeStateBiznisEntita(be, idNewState);
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
        public void ChangeStateBiznisEntita(IChangeState state)
        {
            using var transaction = BeginTransaction();
            try
            {
                BeforeChangeState(state);

                ChangeState(state, false);

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new WebEasException(null, "Nastala chyba pri zmene stavu", ex);
            }
        }

        /// <summary>
        /// Perform custom logic before state is changed.
        /// </summary>
        /// <param name="entityId"><see cref="BiznisEntita"/> Id.</param>
        /// <param name="newStateId">Id of the new state.</param>
        protected virtual void BeforeChangeState(IChangeState state)
        {
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
            logEventInfo.Properties["DcomId"] = Session.UserId;
            ReqTimeLogger.LogInfo(logEventInfo);
        }

        public void LogRequestDuration(string serviceUrl, long elapsedMilliseconds, [System.Runtime.CompilerServices.CallerMemberName] string operation = "")
        {
            var logEventInfo = new LogEventInfo(LogLevel.Info, LoggerName, null);
            logEventInfo.Properties["ElapsedMilliseconds"] = elapsedMilliseconds;
            logEventInfo.Properties["ServiceUrl"] = serviceUrl;
            logEventInfo.Properties["Operation"] = operation;
            logEventInfo.Properties["TenantId"] = Session.TenantId;
            logEventInfo.Properties["DcomId"] = Session.UserId;
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
        public List<TreeNodeCount> GetTreeCounts(IGetTreeCounts treeCounts)
        {
            var result = new List<TreeNodeCount>(treeCounts.Codes.Length);

            foreach (var grpCodes in treeCounts.Codes.GroupBy(x => x.Contains('!') ? x.LastLeftPart('!') : x))
            {
                var node = RenderModuleRootNode(grpCodes.Key).TryFindNode(grpCodes.Key);
                if (node != null)
                {
                    if (node.RowsCounterRule == -3)
                    {
                        foreach (var code in grpCodes)
                        {
                            result.Add(new TreeNodeCount() { Count = -3, CountAll = node.CountAllRows ? -3 : -1, Code = code });
                        }
                    }
                    else if (node.RowsCounterRule != 0)
                    {
                        string treeCountKey = $"treecounts:{grpCodes.Key}:{node.RowsCounterRule}".ToLowerInvariant();
                        if (grpCodes.Any(x => x.Contains('!')))
                        {
                            treeCountKey = string.Concat(treeCountKey, ":", grpCodes.Select(x => x.LastRightPart('!')).Join(","));
                        }

                        List<TreeNodeCount> treeNodeCounts;
                        //avoid simultaneous update of same key..
                        lock (treeCountsLockObjects.GetSyncObject(treeCountKey))
                        {
                            //avoid invalidate all cache items at once?
                            var rnd = new Random();
                            int lifetimeMin = 50 + rnd.Next(15); //random from 50 - 65 mins

                            treeNodeCounts = GetCacheOptimizedTenant(treeCountKey, () =>
                            {
                                Debug.WriteLine(string.Format("{0}__{1}", System.Threading.Thread.CurrentThread.ManagedThreadId.ToString(), treeCountKey));
                                var filterSpecific = new Filter();
                                var filterAll = new Filter();
                                var allrows = new List<TreeNodeCount>();

                                if (node.AdditionalFilter != null)
                                {
                                    filterSpecific.Append(node.AdditionalFilter.Clone());
                                    filterAll.Append(node.AdditionalFilter.Clone());
                                }

                                MethodInfo method = this.GetType().GetMethod("Count", new Type[] { typeof(Filter), typeof(string[]) });
                                MethodInfo func = method.MakeGenericMethod(node.ModelType);

                                //pocet vsetkych zaznamov
                                if (node.CountAllRows)
                                {
                                    if (node.KodPolozky == "crm-dod-dfa" || node.KodPolozky == "crm-dod-dzf" ||
                                        node.KodPolozky == "crm-odb-ofa" || node.KodPolozky == "crm-odb-ozf")
                                    {
                                        filterAll.AndEq("P", 0); //Neuhradené
                                    }

                                    allrows = (List<TreeNodeCount>)func.Invoke(this, new object[] { filterAll, grpCodes.ToArray() });
                                }

                                //pocet zaznamov so specifickym filtrom
                                if (node.RowsCounterRule > 0)
                                {
                                    if (node.KodPolozky == "crm-dod-dfa" || node.KodPolozky == "crm-dod-dzf" ||
                                        node.KodPolozky == "crm-odb-ofa" || node.KodPolozky == "crm-odb-ozf")
                                    {
                                        filterSpecific.AndEq("R", 0); //Nezaúčtované
                                    }
                                    else
                                    {
                                        filterSpecific.AndEq("JeKoncovyStav", 0);
                                    }
                                }

                                var rows = (List<TreeNodeCount>)func.Invoke(this, new object[] { filterSpecific, grpCodes.ToArray() });

                                if (allrows.Any())
                                {
                                    foreach (var allrow in allrows.Where(x => rows.Any(z => z.Code == x.Code)))
                                    {
                                        rows.First(x => x.Code == allrow.Code).CountAll = allrow.Count;
                                    }
                                }

                                return rows;
                            }, TimeSpan.FromMinutes(lifetimeMin));
                        }

                        if (treeNodeCounts.Any())
                        {
                            result.AddRange(treeNodeCounts);
                        }
                    }
                }
            }

            return result;
        }

        #endregion

        #region Long Operation

        //private delegate void LongOperationProcessDelegate(EsamSession session, string processKey, Guid corrId, string operationName, string operationParameters);

        private void PublishMessage(LongOperationStartDtoBase longOperation, string kodModulu)
        {
            using var messageProducer = new ServiceStack.Messaging.RedisMessageProducer(RedisManager);
            var message = ServiceStack.Messaging.MessageFactory.Create(longOperation);
            var messageBody = message.Body as LongOperationStartDtoBase;
            messageBody.SessionId = Session.Id;
            messageBody.ProcessKey = string.Concat(kodModulu, "!", message.Id.ToString());
            messageProducer.Publish(message);
        }

        private LongOperationStatus GetOperationStatus(string processKey)
        {
            using var redisClient = RedisManager.GetClient();

            var modul = processKey.Split('!')[0];
            var opId = processKey.Split('!')[1];
            var status = redisClient.GetValueFromHash("RunningLongOperations", modul + "!" + Session.TenantId + "!" + Session.UserId + "!" + opId).FromJson<LongOperationStatus>();
            if (status == null)
            {
                var hashId = string.Concat("LongOperationStatus:", modul, ":", Session.TenantId);
                status = redisClient.GetValueFromHash(hashId, string.Concat(Session.UserId, "!", processKey)).FromJson<LongOperationStatus>();
            }

            return status;
        }

        private void SetRunningOperationStatus(LongOperationStatus operationStatus)
        {
            using var redisClient = RedisManager.GetClient();
            var modul = operationStatus.ProcessKey.Split('!')[0];
            var opId = operationStatus.ProcessKey.Split('!')[1];
            redisClient.SetEntryInHash("RunningLongOperations", modul + "!" + Session.TenantId + "!" + Session.UserId + "!" + opId, operationStatus.ToJson());
        }


        //START
        public LongOperationStatus LongOperationStart(LongOperationStartDtoBase request)
        {

#if DEBUG
            LongOperationInfo longOpInfo = null;
            if (!string.IsNullOrEmpty(request.OperationInfo))
            {
                longOpInfo = Encoding.UTF8.GetString(Convert.FromBase64String(request.OperationInfo)).FromJson<LongOperationInfo>();
            }
            //kvoli lokalnemu debugovaniu
            request.ProcessKey = string.Concat(longOpInfo?.Modul ?? ActualModul, "!", Guid.NewGuid().ToString());

            var debugOperationStatus = new LongOperationStatus(request.ProcessKey)
            {
                Percents = 1,
                CorrId = WebEas.Context.Current.CurrentCorrelationID,
                OperationName = request.OperationName,
                OperationInfo = request.OperationInfo,
                OperationParameters = request.OperationParameters,
                Count = longOpInfo?.Pocet ?? 0,
                Start = DateTime.Now,
                UserId = Session.UserId,
                TenantId = Session.TenantId,
                Message = "Operácia je zaradená do spracovania",
                State = LongOperationState.Waiting,
                Changed = DateTimeOffset.Now.ToUnixTimeMilliseconds()
            };

            using (var redisClient = RedisManager.GetClient())
            {
                var modul = request.ProcessKey.Split('!')[0];
                var opId = request.ProcessKey.Split('!')[1];
                redisClient.SetEntryInHash("RunningLongOperations", modul + "!" + Session.TenantId + "!" + Session.UserId + "!" + opId, debugOperationStatus.ToJson());
            }
#endif

            if (string.IsNullOrEmpty(request.ProcessKey))
            {
                if (string.IsNullOrEmpty(request.OperationParameters))
                    throw new WebEasValidationException(null, "Parametre pre danú operáciu nie sú špecifikované!");

                // Operacia bude vzdy unikatna
                // int hashCode = this.LongOperationGetHashCode(request.OperationParameters);
                // string processKey = string.Format("dap!{0}!{1}", operationName, hashCode.ToString());
                //string processKey = $"LongOperation!{ActualModul}!{Session.TenantId}!{Session.UserId}!{operationName}!{Guid.NewGuid().ToString()}_{DateTimeOffset.Now.ToUnixTimeMilliseconds()}";

                LongOperationInfo longOperationInfo = null;
                if (!string.IsNullOrEmpty(request.OperationInfo))
                {
                    longOperationInfo = Encoding.UTF8.GetString(Convert.FromBase64String(request.OperationInfo)).FromJson<LongOperationInfo>();
                }

                PublishMessage(request, longOperationInfo?.Modul ?? ActualModul);

                var operationStatus = new LongOperationStatus(request.ProcessKey)
                {
                    Percents = 1,
                    CorrId = WebEas.Context.Current.CurrentCorrelationID,
                    OperationName = request.OperationName,
                    OperationInfo = request.OperationInfo,
                    OperationParameters = request.OperationParameters,
                    Count = longOperationInfo?.Pocet ?? 0,
                    Start = DateTime.Now,
                    UserId = Session.UserId,
                    TenantId = Session.TenantId,
                    Message = "Operácia je zaradená do spracovania",
                    State = LongOperationState.Waiting,
                    Changed = DateTimeOffset.Now.ToUnixTimeMilliseconds()
                };

                using (var redisClient = RedisManager.GetClient())
                {
                    var modul = request.ProcessKey.Split('!')[0];
                    var opId = request.ProcessKey.Split('!')[1];
                    redisClient.SetEntryInHash("RunningLongOperations", modul + "!" + Session.TenantId + "!" + Session.UserId + "!" + opId, operationStatus.ToJson());
                }

                //var processDelegate = new LongOperationProcessDelegate(LongOperationProcess);
                //processDelegate.BeginInvoke(Session as EsamSession, processKey, WebEas.Context.Current.CurrentCorrelationID, operationName, System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(request.OperationParameters)), null, null);

                return new LongOperationStatus { CorrId = operationStatus.CorrId, ProcessKey = request.ProcessKey, Message = operationStatus.Message };
            }
            else
            {
                ExecuteLongOperation(request);
                using var redisClient = RedisManager.GetClient();
                var modul = request.ProcessKey.Split('!')[0];
                var opId = request.ProcessKey.Split('!')[1];
                var longOperation = redisClient.GetValueFromHash("RunningLongOperations", modul + "!" + Session.TenantId + "!" + Session.UserId + "!" + opId);
                if (!string.IsNullOrEmpty(longOperation))
                {
                    var operationStatus = longOperation.FromJson<LongOperationStatus>();
                    redisClient.RemoveEntryFromHash("RunningLongOperations", modul + "!" + Session.TenantId + "!" + Session.UserId + "!" + opId);
#if DEBUG
                    //kvoli lokalnemu debugovaniu
                    EsamAppHostBase.ProcessLongOperationStatus(operationStatus, redisClient);
#endif
                    return operationStatus;
                }
                else
                {
                    return new LongOperationStatus { ProcessKey = request.ProcessKey, Result = "Operácia nenájdená!" };
                }
            }
        }

        //RESTART
        public LongOperationStatus LongOperationRestart(string processKey)
        {
            //var status = GetFromCache<LongOperationStatus>(processKey);
            var status = GetOperationStatus(processKey);

            if (status == null)
            {
                return new LongOperationStatus { ProcessKey = processKey, Result = "Operácia nenájdená!" };
            }

            if (status.State == LongOperationState.Running || status.State == LongOperationState.Waiting)
            {
                return new LongOperationStatus { ProcessKey = processKey, Result = "Operácia už čaká na dokončenie!" };
            }

            /*status.State = LongOperationState.Restored;
            status.Changed = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            SetToCache(processKey, status, new TimeSpan(DaysTTLLongTime, 0, 0));*/
            //RemoveFromCache(processKey);

            return LongOperationStart(new LongOperationStartDtoBase { OperationName = status.OperationName, OperationParameters = status.OperationParameters, OperationInfo = status.OperationInfo });
        }

        //STATUS
        public LongOperationStatus LongOperationStatus(string processKey)
        {
            //LongOperationStatus status = this.GetFromCache<LongOperationStatus>(processKey);
            var status = GetOperationStatus(processKey);

            if (status == null)
            {
                return new LongOperationStatus { ProcessKey = processKey, Result = "Operácia nenájdená!" };
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
            //LongOperationStatus status = this.GetFromCache<LongOperationStatus>(processKey);
            var status = GetOperationStatus(processKey);
            if (status == null)
            {
                bool thowEx = true;
#if DEBUG
                thowEx = false;
#endif

                if (thowEx)
                {
                    throw new WebEasValidationException(string.Format("Process key {0} not found", processKey), "Operácia nenájdená!");
                }
                else
                {
                    return false;
                }
            }

            WebEas.Context.Current.CurrentCorrelationID = status.CorrId;
            if (!status.IsFinished)
            {
                throw new WebEasValidationException(string.Format("Process in wrong state {0} {1}", processKey, status.Message), "Operácia nie je ukončená!");
            }

            //RemoveFromCache(processKey);
            return status.Result;
        }

        //CANCEL
        public LongOperationStatus LongOperationCancel(string processKey, bool notRemove)
        {
            //TODO: DORIESIT !!!
            //var status = GetFromCache<LongOperationStatus>(processKey);
            var status = GetOperationStatus(processKey);
            if (status != null)
            {
                WebEas.Context.Current.CurrentCorrelationID = status.CorrId;
                /*if (notRemove)
                {
                    status.State = LongOperationState.Canceled;
                    status.Changed = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                    SetToCache(processKey, status, new TimeSpan(DaysTTLLongTime, 0, 0));
                }
                else
                    RemoveFromCache(processKey);*/
            }

            return new LongOperationStatus { ProcessKey = processKey, Result = "Proces zrušený!", CorrId = WebEas.Context.Current.CurrentCorrelationID };
        }

        //LIST
        public List<LongOperationStatus> LongOperationList(bool perTenant, int skip, int take)
        {
            //TODO: HASH NIEJE SORTOVANY, nemozem pouzit skip a take. Dopracovat
            var operationStatusHashId = string.Concat("LongOperationStatus:", ActualModul, ":", Session.TenantId);
            var redisKeys = new List<KeyValuePair<string, string>>();
            var list = new List<LongOperationStatus>();

            using var redisClient = RedisManager.GetClient();

            var runningOperationsPattern = perTenant ? ActualModul + "!" + Session.TenantId + "!*" : ActualModul + "!" + Session.TenantId + "!" + Session.UserId + "!*";
            var runningOperationsList = redisClient.ScanAllHashEntries("RunningLongOperations", runningOperationsPattern).Take(take);

            if (perTenant)
            {
                redisKeys.AddRange(redisClient.ScanAllHashEntries(operationStatusHashId).Take(take - runningOperationsList.Count()));
            }
            else
            {
                redisKeys.AddRange(redisClient.ScanAllHashEntries(operationStatusHashId, Session.UserId + "!*").Take(take - runningOperationsList.Count()));
            }

            if (redisKeys.Any())
            {
                list.AddRange(redisKeys.Select(x => x.Value.FromJson<LongOperationStatus>()).Union(runningOperationsList.Any() ? runningOperationsList.Select(x => x.Value.FromJson<LongOperationStatus>()) : new List<LongOperationStatus>()).OrderByDescending(x => x.Changed));
            }

            return list;
        }

        protected void ExecuteLongOperation(LongOperationStartDtoBase request)
        {
            try
            {
                // TODO: Sliding Session
                //SetToCache(newSession.UniqueKey, newSession, new TimeSpan(DaysTTLLongTime, 0, 0));

                OrmLiteConfig.CommandTimeout = 3600; //docasne zapnutie vacsieho timeoutu
                LongOperationStarted(request.ProcessKey, request.OperationName);
                LongOperationProcess(request);

                //Ak sme niekde zabudli ukoncit operaciu, tak to spravime tu
                //var status = GetFromCache<LongOperationStatus>(request.ProcessKey);
                var status = GetOperationStatus(request.ProcessKey);
                if (status != null && status.State != LongOperationState.Canceled && status.State != LongOperationState.Done && status.State != LongOperationState.Failed)
                {
                    LongOperationFinished(request.ProcessKey, request.OperationName, !status.HasError);
                }
            }
            catch (WebEasValidationException ex)
            {
                LongOperationSetState(request.ProcessKey, ex);
            }
            catch (Exception ex)
            {
                var error = new WebEasException($"Error in operation {request.OperationName} with parameters {WebEas.Core.Log.WebEasNLogExtensions.ToJsonString(request.OperationParameters)}. Process key {request.ProcessKey} and sessionkey {request.SessionId}", ex);
                LongOperationSetState(request.ProcessKey, error);
            }
            finally
            {
                OrmLiteConfig.CommandTimeout = 60;
            }
        }

        protected virtual void LongOperationProcess(LongOperationStartDtoBase request)
        {
            throw new WebEasException($"Long operation with the name {request.OperationName} is not implemented", "Operácia nie je implementovaná!");
        }

        protected void LongOperationSetState(string processKey, WebEasException ex)
        {
            if (string.IsNullOrEmpty(processKey))
            {
                throw ex;
            }

            //var status = GetFromCache<LongOperationStatus>(processKey) ?? new LongOperationStatus(processKey);
            var status = GetOperationStatus(processKey) ?? new LongOperationStatus(processKey);

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
            //SetToCache(processKey, status, new TimeSpan(DaysTTLLongTime, 0, 0));
            SetRunningOperationStatus(status);
            Log.Error(ex);
        }

        protected void LongOperationSetState(string processKey, string message = null, long? current = null, long? count = null, LongOperationState? state = null)
        {
            if (string.IsNullOrEmpty(processKey))
            {
                return;
            }
            //var status = GetFromCache<LongOperationStatus>(processKey) ?? new LongOperationStatus(processKey);
            var status = GetOperationStatus(processKey) ?? new LongOperationStatus(processKey);

            status.Message = string.IsNullOrEmpty(message) ? status.Message : message;
            status.Current = current ?? status.Current;
            status.Count = count ?? status.Count;
            status.Percents = 1;
            status.CorrId = WebEas.Context.Current.CurrentCorrelationID;
            status.State = state ?? status.State;
            status.Changed = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            //SetToCache(processKey, status, new TimeSpan(DaysTTLLongTime, 0, 0));
            SetRunningOperationStatus(status);
        }

        protected void LongOperationSetStateFinished(string processKey, string message, object result, long? current = null, long? count = null, LongOperationState? state = null, string reportId = null)
        {
            if (string.IsNullOrEmpty(processKey))
            {
                return;
            }
            //var status = GetFromCache<LongOperationStatus>(processKey) ?? new LongOperationStatus(processKey);
            var status = GetOperationStatus(processKey) ?? new LongOperationStatus(processKey);

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
            //SetToCache(processKey, status, new TimeSpan(DaysTTLLongTime, 0, 0));
            SetRunningOperationStatus(status);
        }

        /// <summary>
        /// Longs the operation set state message.
        /// </summary>
        /// <param name="processKey">The process key.</param>
        /// <param name="message">The message.</param>
        protected void LongOperationSetStateMessage(string processKey, string message, string reportId = null)
        {
            if (string.IsNullOrEmpty(processKey))
            {
                return;
            }

            //var status = GetFromCache<LongOperationStatus>(processKey) ?? new LongOperationStatus(processKey);
            var status = GetOperationStatus(processKey) ?? new LongOperationStatus(processKey);
            status.Message = message;
            //status.Count = 0;
            //status.Current = status.Count;
            //status.Percents = 100;
            status.NotClose = true;
            //status.Start = DateTime.Now;
            status.CorrId = WebEas.Context.Current.CurrentCorrelationID;
            status.ReportId = reportId ?? status.ReportId;
            //SetToCache(processKey, status, new TimeSpan(DaysTTLLongTime, 0, 0));
            SetRunningOperationStatus(status);
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
            LongOperationSetStateFinished(processKey, string.Empty, result, state: successfully ? LongOperationState.Done : LongOperationState.Failed);
        }

        /// <summary>
        /// Pomocna funkcia na oznamenie vysledku.
        /// Vygeneruje result v tvare: "Operácia {action} je ukončená. {customText}."
        /// </summary>
        protected void LongOperationFinished(string processKey, string action, string customText)
        {
            string result = $"Operácia \"{action}\" je ukončená. {customText}";
            LongOperationSetStateFinished(processKey, string.Empty, result, state: LongOperationState.Done);
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
            LongOperationSetStateFinished(processKey, message, result, state: LongOperationState.Done, reportId: reportId);
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

        #region TypBiznisEntityNastav

        public List<TypBiznisEntityNastavView> GetTypBiznisEntityNastavView()
        {
            return GetCacheOptimizedTenant(nameof(TypBiznisEntityNastavView), () =>
            {
                return GetList<TypBiznisEntityNastavView>();
            }, new TimeSpan(24, 0, 0));
        }

        public Dictionary<string, bool> GetTypBiznisEntityNastav()
        {
            var data = GetTypBiznisEntityNastavView();
            Dictionary<string, bool> result = new Dictionary<string, bool>
                {
                    //fin
                    { "fin-pok-pdk", data.Where(x => x.C_TypBiznisEntity_Id == (short)TypBiznisEntityEnum.PDK).Select(x => (bool)x.EvidenciaSystem).FirstOrDefault() },
                    { "fin-bnk-ban", data.Where(x => x.C_TypBiznisEntity_Id == (short)TypBiznisEntityEnum.BAN).Select(x => (bool)x.EvidenciaSystem).FirstOrDefault() },
                    { "fin-bnk-ppp", data.Where(x => x.C_TypBiznisEntity_Id == (short)TypBiznisEntityEnum.PPP).Select(x => (bool)x.EvidenciaSystem).FirstOrDefault() },
                    //uct, rzp
                    { "all-evi-intd", data.Where(x => x.C_TypBiznisEntity_Id == (short)TypBiznisEntityEnum.IND).Select(x => (bool)x.EvidenciaSystem).FirstOrDefault() },
                    { "all-evi-intd!uct", data.Where(x => x.C_TypBiznisEntity_Id == (short)TypBiznisEntityEnum.IND).Select(x => (bool)x.EvidenciaSystem).FirstOrDefault() },
                    { "all-evi-intd!rzp", data.Where(x => x.C_TypBiznisEntity_Id == (short)TypBiznisEntityEnum.IND).Select(x => (bool)x.EvidenciaSystem).FirstOrDefault() },
                    //crm
                    { "crm-dod-dfa", data.Where(x => x.C_TypBiznisEntity_Id == (short)TypBiznisEntityEnum.DFA).Select(x => (bool)x.EvidenciaSystem).FirstOrDefault() },
                    { "crm-dod-dzf", data.Where(x => x.C_TypBiznisEntity_Id == (short)TypBiznisEntityEnum.DZF).Select(x => (bool)x.EvidenciaSystem).FirstOrDefault() },
                    { "crm-dod-dzm", data.Where(x => x.C_TypBiznisEntity_Id == (short)TypBiznisEntityEnum.DZM).Select(x => (bool)x.EvidenciaSystem).FirstOrDefault() },
                    { "crm-dod-dob", data.Where(x => x.C_TypBiznisEntity_Id == (short)TypBiznisEntityEnum.DOB).Select(x => (bool)x.EvidenciaSystem).FirstOrDefault() },
                    { "crm-dod-dcp", data.Where(x => x.C_TypBiznisEntity_Id == (short)TypBiznisEntityEnum.DCP).Select(x => (bool)x.EvidenciaSystem).FirstOrDefault() },
                    { "crm-dod-ddp", data.Where(x => x.C_TypBiznisEntity_Id == (short)TypBiznisEntityEnum.DDP).Select(x => (bool)x.EvidenciaSystem).FirstOrDefault() },
                    { "crm-odb-ofa", data.Where(x => x.C_TypBiznisEntity_Id == (short)TypBiznisEntityEnum.OFA).Select(x => (bool)x.EvidenciaSystem).FirstOrDefault() },
                    { "crm-odb-dol", data.Where(x => x.C_TypBiznisEntity_Id == (short)TypBiznisEntityEnum.DOL).Select(x => (bool)x.EvidenciaSystem).FirstOrDefault() },
                    { "crm-odb-ozf", data.Where(x => x.C_TypBiznisEntity_Id == (short)TypBiznisEntityEnum.OZF).Select(x => (bool)x.EvidenciaSystem).FirstOrDefault() },
                    { "crm-odb-ozm", data.Where(x => x.C_TypBiznisEntity_Id == (short)TypBiznisEntityEnum.OZM).Select(x => (bool)x.EvidenciaSystem).FirstOrDefault() },
                    { "crm-odb-oob", data.Where(x => x.C_TypBiznisEntity_Id == (short)TypBiznisEntityEnum.OOB).Select(x => (bool)x.EvidenciaSystem).FirstOrDefault() },
                    { "crm-odb-ocp", data.Where(x => x.C_TypBiznisEntity_Id == (short)TypBiznisEntityEnum.OCP).Select(x => (bool)x.EvidenciaSystem).FirstOrDefault() },
                    { "crm-odb-odp", data.Where(x => x.C_TypBiznisEntity_Id == (short)TypBiznisEntityEnum.ODP).Select(x => (bool)x.EvidenciaSystem).FirstOrDefault() }
                };

            return result;
        }

        public string GetTypBiznisEntityNastav(TypBiznisEntityEnum typBiznisEntity, LokalitaEnum lokalita)
        {
            if (lokalita == LokalitaEnum.TU || lokalita == LokalitaEnum.TUS)
                return GetTypBiznisEntityNastavView().Where(x => x.C_TypBiznisEntity_Id == (int)typBiznisEntity).FirstOrDefault()?.DatumDokladuTU;
            else if (lokalita == LokalitaEnum.EU)
                return GetTypBiznisEntityNastavView().Where(x => x.C_TypBiznisEntity_Id == (int)typBiznisEntity).FirstOrDefault()?.DatumDokladuEU;
            else if (lokalita == LokalitaEnum.DV)
                return GetTypBiznisEntityNastavView().Where(x => x.C_TypBiznisEntity_Id == (int)typBiznisEntity).FirstOrDefault()?.DatumDokladuDV;

            return "";
        }

        // wrapper len na jednu hodnotu (INSERT/UPDATE)
        public void UpdateTypBiznisEntityNastav(TypBiznisEntityNastavView data)
        {
            UpdateTypBiznisEntityNastav(data.C_TypBiznisEntity_Id, (bool)data.StrediskoNaPolozke, (bool)data.ProjektNaPolozke,
                                        (bool)data.UctKluc1NaPolozke, (bool)data.UctKluc2NaPolozke, (bool)data.UctKluc3NaPolozke,
                                        (bool)data.EvidenciaDMS, (bool)data.EvidenciaSystem, (bool)data.CislovanieJedno,
                                        data.DatumDokladuTU, data.DatumDokladuEU, data.DatumDokladuDV, (bool)data.UctovatPolozkovite);
        }

        public void UpdateTypBiznisEntityNastav(short typBiznisEntity_Id, bool stredisko, bool projekt, bool uctKluc1, bool uctKluc2,
                                                bool uctKluc3, bool eDMS, bool eSystem, bool cislovanieJedno, string datumDokladuTU,
                                                string datumDokladuEU, string datumDokladuDV, bool uctovatPolozkovite)
        {
            using (var transaction = BeginTransaction())
            {
                try
                {
                    var p = new DynamicParameters();
                    p.Add("@tenant", Session.TenantIdGuid, dbType: DbType.Guid);
                    p.Add("@typBiznisEntity_Id", typBiznisEntity_Id, dbType: DbType.Byte);
                    p.Add("@stredisko", stredisko, dbType: DbType.Boolean);
                    p.Add("@projekt", projekt, dbType: DbType.Boolean);
                    p.Add("@uctKluc1", uctKluc1, dbType: DbType.Boolean);
                    p.Add("@uctKluc2", uctKluc2, dbType: DbType.Boolean);
                    p.Add("@uctKluc3", uctKluc3, dbType: DbType.Boolean);
                    p.Add("@eDMS", eDMS, dbType: DbType.Boolean);
                    p.Add("@eSystem", eSystem, dbType: DbType.Boolean);
                    p.Add("@cislovanieJedno", cislovanieJedno, dbType: DbType.Boolean);
                    p.Add("@datumDokladuTU", datumDokladuTU, dbType: DbType.String);
                    p.Add("@datumDokladuEU", datumDokladuEU, dbType: DbType.String);
                    p.Add("@datumDokladuDV", datumDokladuDV, dbType: DbType.String);
                    p.Add("@uctovatPolozkovite", uctovatPolozkovite, dbType: DbType.Boolean);
                    p.Add("@userId", Session.UserIdGuid, dbType: DbType.Guid);

                    SqlProcedure("[reg].[PR_NastavTypBE]", p);

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
                    throw new WebEasException("Nastala chyba pri volaní SQL procedúry [reg].[PR_NastavTypBE]", "Parameter sa nepodarilo upraviť kvôli internej chybe", ex);
                }
            }
            RemoveFromCacheOptimizedTenant(nameof(TypBiznisEntityNastavView));
        }

        public void RefreshDefaultTypBiznisEntityNastav(long[] IDs)
        {
            try
            {
                var idsRemove = IDs.Where(x => x > 0).ToList();
                if (idsRemove.Count > 0)
                {
                    string sql = $"delete from [reg].[D_TypBiznisEntityNastav] where D_TypBiznisEntityNastav_Id in ({string.Join(",", idsRemove)})";
                    Db.Execute(sql);
                }
            }
            catch (Exception ex)
            {
                throw new WebEasException("Nastala chyba pri volaní RefreshDefaultTypBiznisEntityNastav", ex.Message, ex);
            }

            RemoveFromCacheOptimizedTenant(nameof(TypBiznisEntityNastavView));
        }
        #endregion

        #region Biznis Entita
        private DateTime SetDatumDokladu(BiznisEntita be, string typBiznisEntity)
        {
            switch (typBiznisEntity)
            {
                case "DatumDokladu":
                    return be.DatumDokladu;
                case "DatumPrijatia":
                    return (DateTime)be.DatumPrijatia;
                case "DatumVystavenia":
                    return (DateTime)be.DatumVystavenia;
                case "DatumDodania":
                    return (DateTime)be.DatumDodania;
                default:
                    throw new NotImplementedException($"Type {typBiznisEntity} is not implemented"); ;
            }
        }

        public BiznisEntita PrepareBiznisEntitaData(bool create, BiznisEntita data)
        {

            BiznisEntita be;

            if (create)
            {
                if (data.C_TypBiznisEntity_Id == default)
                {
                    throw new WebEasException($"C_TypBiznisEntity_Id nie je nainicializovane !");
                }

                be = new BiznisEntita
                {
                    C_TypBiznisEntity_Id = data.C_TypBiznisEntity_Id,
                };
            }
            else
            {
                be = GetById<BiznisEntita>(data.D_BiznisEntita_Id);
            }

            // spolocne
            be.C_Lokalita_Id = data.C_Lokalita_Id;
            be.DatumDokladu = data.DatumDokladu;
            be.C_Stredisko_Id = data.C_Stredisko_Id;
            be.C_BankaUcet_Id = data.C_BankaUcet_Id;
            be.C_Pokladnica_Id = data.C_Pokladnica_Id;
            be.C_Projekt_Id = data.C_Projekt_Id;
            be.CisloExterne = data.CisloExterne;
            // be.D_BiznisEntita_Id_Externe = data.D_BiznisEntita_Id_Externe; - neriesime cez FE
            be.DatumPrijatia = data.DatumPrijatia;
            be.DatumVystavenia = data.DatumVystavenia;
            be.DatumSplatnosti = data.DatumSplatnosti;
            be.DatumDodania = data.DatumDodania;
            be.DatumVDP = data.DatumVDP;
            be.PS = data.PS;
            be.DM_CV = data.DM_CV;
            be.DM_DPH1 = data.DM_DPH1;
            be.DM_DPH2 = data.DM_DPH2;
            be.DM_Suma = data.DM_Suma;
            be.DM_Zak0 = data.DM_Zak0;
            be.DM_Zak1 = data.DM_Zak1;
            be.DM_Zak2 = data.DM_Zak2;
            be.CM_CV = data.CM_CV;
            be.CM_DPH1 = data.CM_DPH1;
            be.CM_DPH2 = data.CM_DPH2;
            be.CM_Suma = data.CM_Suma;
            be.CM_Zak0 = data.CM_Zak0;
            be.CM_Zak1 = data.CM_Zak1;
            be.CM_Zak2 = data.CM_Zak2;
            be.D_Osoba_Id = data.D_Osoba_Id;
            be.C_Mena_Id = (short)MenaEnum.EUR;
            be.KurzECB = 1;
            be.KurzBanka = 1;
            be.C_Predkontacia_Id = data.C_Predkontacia_Id;
            be.Cislo = data.Cislo;
            be.CisloInterne = data.CisloInterne;
            be.D_OsobaKontakt_Id_Komu = data.D_OsobaKontakt_Id_Komu;
            be.OsobaKontaktKomu = (data.D_OsobaKontakt_Id_Komu == null) ? data.OsobaKontaktKomu : null;
            be.D_ADR_Adresa_Id = data.D_ADR_Adresa_Id;
            be.C_TypBiznisEntity_Kniha_Id = data.C_TypBiznisEntity_Kniha_Id;
            be.Popis = data.Popis;

            if (be.C_TypBiznisEntity_Id != (short)TypBiznisEntityEnum.OFA && be.C_TypBiznisEntity_Id != (short)TypBiznisEntityEnum.OZF)
            {
                be.VS = data.VS; // iba pre OFA a OZF sa generuje cez generátor. Inak použijem zo služby.
            }

            // **** Datumy + Rok - potrebné nastaviť pred vygenerovaním čísla dokladu *****
            string tbe = GetTypBiznisEntityNastav((TypBiznisEntityEnum)be.C_TypBiznisEntity_Id, (LokalitaEnum)be.C_Lokalita_Id);

            switch ((TypBiznisEntityEnum)be.C_TypBiznisEntity_Id)
            {
                case TypBiznisEntityEnum.Unknown:
                case TypBiznisEntityEnum.PRI:
                case TypBiznisEntityEnum.VYD:
                case TypBiznisEntityEnum.PRE:
                    break;

                case TypBiznisEntityEnum.IND:
                    be.DatumPrijatia = be.DatumDokladu;
                    be.DatumVystavenia = be.DatumDokladu;
                    be.DatumSplatnosti = null;
                    be.DatumDodania = null;
                    be.DatumVDP = be.DatumDokladu;
                    break;

                case TypBiznisEntityEnum.DFA:
                    //be.DatumPrijatia     - Nastavované zo služby
                    //be.DatumVystavenia   - Nastavované zo služby
                    //be.DatumSplatnosti   - Nastavované zo služby
                    //be.DatumDodania      - Nastavované zo služby
                    be.DatumVDP = be.DatumDokladu;
                    be.DatumDokladu = SetDatumDokladu(be, tbe);
                    break;

                case TypBiznisEntityEnum.OFA:
                    be.DatumPrijatia = null;
                    //be.DatumVystavenia   - Nastavované zo služby
                    //be.DatumSplatnosti   - Nastavované zo služby
                    //be.DatumDodania      - Nastavované zo služby
                    be.DatumVDP = be.DatumDokladu;
                    be.DatumDokladu = SetDatumDokladu(be, tbe);
                    break;

                case TypBiznisEntityEnum.PDK:
                    be.DatumPrijatia = be.DatumDokladu;
                    be.DatumVystavenia = be.DatumDokladu;
                    be.DatumSplatnosti = null;
                    be.DatumDodania = null;
                    be.DatumVDP = be.DatumDokladu;
                    break;

                case TypBiznisEntityEnum.PPP:
                case TypBiznisEntityEnum.BAN:
                    be.DatumPrijatia = be.DatumDokladu;
                    be.DatumVystavenia = be.DatumDokladu;
                    be.DatumSplatnosti = null;
                    be.DatumDodania = null;
                    be.DatumVDP = null;
                    break;

                case TypBiznisEntityEnum.OZF:
                case TypBiznisEntityEnum.DZF:
                    //be.DatumPrijatia     - Nastavované zo služby
                    //be.DatumVystavenia   - Nastavované zo služby
                    //be.DatumSplatnosti   - Nastavované zo služby
                    be.DatumDodania = null;
                    be.DatumVDP = null;
                    be.DatumDokladu = SetDatumDokladu(be, tbe);
                    break;

                case TypBiznisEntityEnum.OZM:
                case TypBiznisEntityEnum.DZM:
                    //be.DatumPrijatia     - Nastavované zo služby
                    //be.DatumVystavenia   - Nastavované zo služby
                    //be.DatumSplatnosti  - Nastavované zo služby
                    //be.DatumDodania     -  Nastavované zo služby
                    be.DatumVDP = null;
                    be.DatumDokladu = SetDatumDokladu(be, tbe);
                    break;

                case TypBiznisEntityEnum.ODP:
                case TypBiznisEntityEnum.DDP:
                case TypBiznisEntityEnum.OCP:
                case TypBiznisEntityEnum.DCP:
                case TypBiznisEntityEnum.OOB:
                case TypBiznisEntityEnum.DOB:
                case TypBiznisEntityEnum.DOL:
                    //be.DatumPrijatia     - Nastavované zo služby
                    //be.DatumVystavenia   - Nastavované zo služby
                    be.DatumSplatnosti = null;
                    //be.DatumDodania -  Nastavované zo služby
                    be.DatumVDP = null;
                    be.DatumDokladu = SetDatumDokladu(be, tbe);
                    break;
                default:
                    break;
            }

            be.Rok = (short)be.DatumDokladu.Year;

            if (be.PS)
            {
                be.UOMesiac = 0;
            }
            else
            {
                if (be.C_StavEntity_Id != (int)StavEntityEnum.ZAUCTOVANY && be.C_StavEntity_Id != (int)StavEntityEnum.ZAUCTOVANY_RZP && be.C_StavEntity_Id != (int)StavEntityEnum.ZAUCTOVANY_UCT)
                {
                    be.UOMesiac = (byte)be.DatumDokladu.Month;
                }
            }

            // *****************

            if (create)
            {
                var hierarchyNodes = RenderModuleRootNode(Code).Children.RecursiveSelect(w => w.Children).Where(x => x.TypBiznisEntity != null && x.TypBiznisEntity.Any(w => (short)w == be.C_TypBiznisEntity_Id));
                var hierarchyNode = be.C_TypBiznisEntity_Id == (short)TypBiznisEntityEnum.IND
                    ? (hierarchyNodes.Count() == 1
                        ? hierarchyNodes.First()
                        : (hierarchyNodes.SingleOrDefault(x => x.TypBiznisEntityKnihaIntExt == data.C_TypBiznisEntity_Kniha_Id) ?? hierarchyNodes.SingleOrDefault(x => x.TypBiznisEntityKnihaIntExt == (int)TypBiznisEntity_KnihaEnum.Interne_doklady)))
                    : hierarchyNodes.SingleOrDefault();

                if (hierarchyNode == null)
                {
                    throw new WebEasException($"Nepodarilo sa nájsť kód položky stromu pre C_TypBiznisEntity_Id = {be.C_TypBiznisEntity_Id} !");
                }

                be.PolozkaStromu = hierarchyNode.KodPolozky;
                var tb = Db.Single<TypBiznisEntity>("SELECT C_StavovyPriestor_Id FROM reg.C_TypBiznisEntity WHERE C_TypBiznisEntity_Id = @typBiznisEntity", new { typBiznisEntity = be.C_TypBiznisEntity_Id });
                be.C_StavovyPriestor_Id = tb.C_StavovyPriestor_Id;
                be.C_StavEntity_Id = (int)StavEntityEnum.NOVY;

                if (string.IsNullOrEmpty(be.CisloInterne))
                {
                    //Zistenie cisla dokladu a v prípade VFA, VZF aj VS
                    int cisloDokladu = GetCisloDokladu(be.DatumDokladu, data, string.Empty, out string interneCisloDokladu, out string vs);
                    be.Cislo = cisloDokladu;
                    be.CisloInterne = interneCisloDokladu;

                    if (data.C_TypBiznisEntity_Id == (short)TypBiznisEntityEnum.OFA ||
                        data.C_TypBiznisEntity_Id == (short)TypBiznisEntityEnum.OZF)
                    {
                        be.VS = vs;
                    }
                }

                if (hierarchyNode.RowsCount != 0)
                {
                    InvalidateTreeCountsForPath(hierarchyNode.KodPolozky);
                }
            }

            // specificke
            switch ((TypBiznisEntityEnum)be.C_TypBiznisEntity_Id)
            {
                case TypBiznisEntityEnum.Unknown:
                case TypBiznisEntityEnum.PRI:  //Zatiaľ nezapracované
                case TypBiznisEntityEnum.VYD:  //Zatiaľ nezapracované
                case TypBiznisEntityEnum.PRE:  //Zatiaľ nezapracované
                case TypBiznisEntityEnum.IND:
                case TypBiznisEntityEnum.DOB:
                case TypBiznisEntityEnum.DDP:
                case TypBiznisEntityEnum.OCP:
                case TypBiznisEntityEnum.DZM:
                case TypBiznisEntityEnum.PPP:
                case TypBiznisEntityEnum.DOL:
                    be.CisloExterne = null;
                    be.VS = null;
                    break;

                case TypBiznisEntityEnum.DFA:
                case TypBiznisEntityEnum.DZF:
                    //be.CisloExterne - ukladám to čo príde zo služby
                    //be.VS           - ukladám to čo príde zo služby
                    break;

                case TypBiznisEntityEnum.PDK:
                case TypBiznisEntityEnum.BAN:
                case TypBiznisEntityEnum.ODP:
                case TypBiznisEntityEnum.DCP:
                case TypBiznisEntityEnum.OOB:
                case TypBiznisEntityEnum.OZM:
                    //be.CisloExterne - ukladám to čo príde zo služby
                    be.VS = null;
                    break;

                case TypBiznisEntityEnum.OFA:
                case TypBiznisEntityEnum.OZF:
                    be.CisloExterne = null;
                    //be.VS - nastavuje sa podľa číslovania
                    break;

                default:
                    break;
            }

            return be;
        }

        #endregion

        #region Int. doklad - UCT, RZP

        public DokladINDView CreateDokladIND(DokladDto data)
        {
            long id = 0;
            var be = PrepareBiznisEntitaData(true, data.ConvertToEntity());
            using (var transaction = BeginTransaction())
            {
                try
                {
                    id = InsertData(be);
                    var intDokl = new DokladIND
                    {
                        D_DokladIND_Id = id,
                        Rok = be.Rok,
                        //Popis = data.Popis,
                        Poznamka = data.Poznamka
                    };
                    InsertData(intDokl);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }

            return GetById<DokladINDView>(id);
        }

        public DokladINDView UpdateDokladIND(DokladDto ind)
        {
            using (var transaction = BeginTransaction())
            {
                try
                {
                    var be = PrepareBiznisEntitaData(false, ind.ConvertToEntity());
                    UpdateData(be);

                    var indNew = GetById<DokladIND>(ind.D_BiznisEntita_Id);
                    CopyProperties(ind, indNew);
                    UpdateData(indNew);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }

            return GetById<DokladINDView>(ind.D_BiznisEntita_Id);
        }

        public void DeleteDoklad<T>(long[] id) where T : class, IBaseEntity
        {
            using var tran = BeginTransaction();
            var be = GetList(Db.From<BiznisEntita>().Where(x => Sql.In(x.D_BiznisEntita_Id, id) && x.D_Tenant_Id == Session.TenantIdGuid).SelectDistinct(x => x.PolozkaStromu));
            DeleteData<T>(id);
            DeleteData<BiznisEntita>(id);
            tran.Commit();
            be.ForEach(x => InvalidateTreeCountsForPath(x.PolozkaStromu));
        }

        #endregion

        #region Doklad

        public long CreateDoklad(DokladDto data)
        {
            long id = 0;
            var be = PrepareBiznisEntitaData(true, data.ConvertToEntity());
            using (var transaction = BeginTransaction())
            {
                try
                {
                    id = InsertData(be);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }

            return id;
        }

        public long UpdateDoklad(DokladDto data)
        {
            using (var transaction = BeginTransaction())
            {
                try
                {
                    UpdateData(PrepareBiznisEntitaData(false, data.ConvertToEntity()));
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }

            return data.D_BiznisEntita_Id;
        }
        /*
       public bool PredkontujDoklad(PredkontovatDokladDto dokl, string processKey, out string reportId)
       {
           var doklady = GetList(Db.From<BiznisEntitaView>().Where(x => Sql.In(x.D_BiznisEntita_Id, dokl.D_BiznisEntita_Ids)));
           var chybneDokladyUct = new List<(long D_BiznisEntita_Id, string Chyba)>();
           var chybneDokladyRzp = new List<(long D_BiznisEntita_Id, string Chyba)>();
           string msg = null;
           reportId = null;
           bool success = false;

           if (doklady.Any(x => x.C_StavEntity_Id == (int)StavEntityEnum.NOVY))
           {
               SpracujDoklad(new SpracovatDokladDto
               {
                   Ids = doklady.Where(x => x.C_StavEntity_Id == (int)StavEntityEnum.NOVY).Select(x => x.D_BiznisEntita_Id).ToArray()
               }, processKey, out reportId, finishOperation: false);
           }

           if (!string.IsNullOrEmpty(reportId))
           {
               LongOperationSetStateFinished(processKey, string.Empty, "Operácia 'Predkontovať' sa skončila s chybami", state: LongOperationState.Done, reportId: reportId);
               return success;
           }

           if (dokl.RzpDennik && !dokl.UctDennik)
           {
               PredkontujDokladRzp(dokl, out chybneDokladyRzp);
               if (!chybneDokladyRzp.Any())
               {
                   msg = "Predkontácia do rozpočtu bola úspešne vykonaná.";
                   success = true;
               }
           }
           else if (!dokl.RzpDennik && dokl.UctDennik)
           {
               PredkontujDokladUct(dokl, out chybneDokladyUct);
               if (!chybneDokladyUct.Any())
               {
                   msg = "Predkontácia do účtovníctva bola úspešne vykonaná.";
                   success = true;
               }
           }
           else if (dokl.RzpDennik && dokl.UctDennik)
           {
               PredkontujDokladRzp(dokl, out chybneDokladyRzp);
               PredkontujDokladUct(dokl, out chybneDokladyUct);

               if (!chybneDokladyRzp.Any() && !chybneDokladyUct.Any())
               {
                   msg = "Predkontácia bola úspešne vykonaná.";
                   success = true;
               }
               else if (!chybneDokladyRzp.Any())
               {
                   msg = "Predkontácia do rozpočtu bola úspešne vykonaná.";
               }
               else if (!chybneDokladyUct.Any())
               {
                   msg = "Predkontácia do účtovníctva bola úspešne vykonaná.";
               }
           }
           else
           {
               msg = "Nebola zvolená žiadna predkontácia.";
           }

           if (chybneDokladyUct.Any() || chybneDokladyRzp.Any())
           {
               reportId = Guid.NewGuid().ToString();
               using var ms = new MemoryStream();
               TextWriter tw = new StreamWriter(ms);

               if (chybneDokladyRzp.Any())
               {
                   tw.WriteLine("Rozpočtový denník");
                   tw.WriteLine();
               }

               foreach (var dkl in chybneDokladyRzp.GroupBy(x => x.D_BiznisEntita_Id))
               {
                   tw.WriteLine($"Doklad '{doklady.FirstOrDefault(x => x.D_BiznisEntita_Id == dkl.Key)?.CisloInterne}':");
                   tw.WriteLine(string.Join(Environment.NewLine, dkl.Select(x => x.Chyba)));
                   tw.WriteLine();
               }

               if (chybneDokladyUct.Any())
               {
                   tw.WriteLine("Účtovný denník");
                   tw.WriteLine();
               }

               foreach (var dkl in chybneDokladyUct.GroupBy(x => x.D_BiznisEntita_Id))
               {
                   tw.WriteLine($"Doklad '{doklady.FirstOrDefault(x => x.D_BiznisEntita_Id == dkl.Key)?.CisloInterne}':");
                   tw.WriteLine(string.Join(Environment.NewLine, dkl.Select(x => x.Chyba)));
                   tw.WriteLine();
               }

               tw.Flush();
               ms.Position = 0;

               var ret = new RendererResult
               {
                   DocumentBytes = ms.ToArray(),
                   DocumentName = "ChybyPredkontacie-" + ((TypBiznisEntityEnum)doklady.First().C_TypBiznisEntity_Id).ToString() + DateTime.Now.ToString("_yyyyMMdd_HHmm"),
                   Extension = "txt"
               };

               SetToCache(string.Concat("Report:", reportId), ret, new TimeSpan(8, 0, 0), useGzipCompression: true);
           }

           if (string.IsNullOrEmpty(reportId))
           {
               LongOperationSetStateFinished(processKey, string.Empty, msg, state: LongOperationState.Done);
           }
           else
           {
               LongOperationSetStateFinished(processKey, string.Empty, "Operácia 'Predkontovať' sa skončila s chybami", state: LongOperationState.Done, reportId: reportId);
           }

           return success;
       }

       public void PredkontujDokladUct(PredkontovatDokladDto request, out List<(long D_BiznisEntita_Id, string Chyba)> chybneDoklady)
       {
           // Key 1 - nevyhovujuce, 2- viacnasobne, 3 - cent vyr.
           var nevyhovujucePolozky = new List<(long D_BiznisEntita_Id, int Typ, int Poradie)>();
           chybneDoklady = new List<(long D_BiznisEntita_Id, string Chyba)>();

           using (var transaction = BeginTransaction())
           {

               var biznisEntity = GetList(Db.From<BiznisEntitaView>().
                   Where(x => Sql.In(x.D_BiznisEntita_Id, request.D_BiznisEntita_Ids)));

               short tbe = biznisEntity.First().C_TypBiznisEntity_Id;
               bool uhr = (tbe == (short)TypBiznisEntityEnum.BAN ||
                           tbe == (short)TypBiznisEntityEnum.PDK ||
                           tbe == (short)TypBiznisEntityEnum.IND);

               if (!uhr && tbe != (short)TypBiznisEntityEnum.DFA && tbe != (short)TypBiznisEntityEnum.OFA)
               {
                   foreach (var id in request.D_BiznisEntita_Ids)
                   {
                       chybneDoklady.Add((id, $"Predkontácia do účtovníctva nie je pre typ dokladu '{ biznisEntity.First().TypBiznisEntityNazov }' možná."));
                   }
                   return;
               }

               List<UctDennik> uctDennikList = new List<UctDennik>();
               List<UctDennikViewHelper> uctDennikSdkFA = new List<UctDennikViewHelper>();
               List<DokladBANPolViewHelper> dokladBanPol = null;
               List<UhradaParovanieViewHelper> uhradaParovanie = null;
               List<BiznisEntita_ZalohaView> zalohyFaktury = null;

               // nacitanie nastavenia "UctovatPolozkovite"
               bool uctovatPolozkovite = GetTypBiznisEntityNastavView().Where(x => x.C_TypBiznisEntity_Id == tbe).FirstOrDefault()?.UctovatPolozkovite ?? false;

               SqlExpression<PredkontaciaUctViewHelper> sqlExp = Db.From<PredkontaciaUctViewHelper>().
                   Where(p => Sql.In(p.C_Predkontacia_Id, biznisEntity.Select(m => m.C_Predkontacia_Id).Distinct()));

               var predkontacieUctAll = uctovatPolozkovite ?
                   GetList(sqlExp).OrderBy(p => p.Polozka).ThenBy(p => p.Poradie) :
                   GetList(sqlExp).OrderBy(p => p.Poradie);

               List<UctRozvrh> ucty = GetList(Db.From<UctRozvrh>().
                   SelectDistinct(x => new
                   {
                       x.C_UctRozvrh_Id,
                       x.VyzadovatStredisko,
                       x.VyzadovatProjekt,
                       x.SDK,
                       x.VyzadovatUctKluc1,
                       x.VyzadovatUctKluc2,
                       x.VyzadovatUctKluc3
                   }).
                   Where(x => Sql.In(x.C_UctRozvrh_Id, predkontacieUctAll.Select(u => u.C_UctRozvrh_Id_MD).
                                                 Union(predkontacieUctAll.Select(u => u.C_UctRozvrh_Id_Dal)).Distinct())));

               if (request.VymazatZaznamy)
               {
                   // zmazat zaznamy v uctDenniku. Púšťať predkontáciu cez roky asi nikto nebude, takže rok môžem zobrať z prvého
                   Db.Delete<UctDennik>(e => Sql.In(e.D_BiznisEntita_Id, request.D_BiznisEntita_Ids) && e.Rok == biznisEntity.First().Rok);
               }

               if (tbe == (short)TypBiznisEntityEnum.BAN)
               {
                   //Dávam VIEW aby som mal ošetrené zmazané záznamy
                   dokladBanPol = GetList(Db.From<DokladBANPolViewHelper>().Where(e => Sql.In(e.D_BiznisEntita_Id, request.D_BiznisEntita_Ids) && e.Rok == biznisEntity.First().Rok));
               }

               if (tbe == (short)TypBiznisEntityEnum.DFA || tbe == (short)TypBiznisEntityEnum.OFA)
               {
                   //Dávam VIEW aby som mal ošetrené zmazané záznamy
                   zalohyFaktury = GetList(Db.From<BiznisEntita_ZalohaView>().Where(e => Sql.In(e.D_BiznisEntita_Id_FA, request.D_BiznisEntita_Ids) && e.Rok == biznisEntity.First().Rok));
               }


               if (uhr)
               {
                   //Dávam VIEW aby som mal ošetrené zmazané záznamy
                   uhradaParovanie = GetList(Db.From<UhradaParovanieViewHelper>()
                                             .Where(e => Sql.In(e.D_BiznisEntita_Id_Uhrada, request.D_BiznisEntita_Ids) && e.Rok == biznisEntity.First().Rok));

                   uctDennikSdkFA = GetList(Db.From<UctDennikViewHelper>()
                                    .Where(e => e.U &&
                                                e.SDK != null &&
                                                Sql.In(e.D_BiznisEntita_Id, uhradaParovanie.Where(x => x.D_BiznisEntita_Id_Predpis != null)
                                                                                          .Select(u => u.D_BiznisEntita_Id_Predpis))));

                   var eSAMStart = GetNastavenieD("reg", "eSAMStart");
                   if (eSAMStart != null)
                   {
                       //pridám ešte SDK zápisy z importu počiatočného stavu
                       var sSql = @$"SELECT d.D_UctDennik_Id, d.D_Tenant_Id, d.D_BiznisEntita_Id, d.SDK, d.SU, d.C_UctRozvrh_Id, d.VS, d.D_Osoba_Id,
                                            d.SumaMD, d.SumaDal, d.C_Stredisko_Id, d.C_Projekt_Id,
                                            d.C_UctKluc_Id1, d.C_UctKluc_Id2, d.C_UctKluc_Id3, d.C_Typ_Id
                                   FROM uct.V_UctDennik d
                                     JOIN fin.D_UhradaParovanie up ON up.VS = d.VS AND up.D_Osoba_Id = d.D_Osoba_Id AND ABS(up.DM_Cena + up.DM_Rozdiel) = ABS(d.SumaMD + d.SumaDal)
                                     JOIN reg.D_BiznisEntita be ON be.D_BiznisEntita_Id = up.D_BiznisEntita_Id_Predpis AND be.PS = 1
                                   WHERE d.DatumDokladu < @DatStart
                                         AND d.U = 1
                                         AND d.SDK IS NOT NULL
                                         AND d.D_Osoba_Id IS NOT NULL
                                         AND d.C_TypBiznisEntity_Id = 1
                                         AND up.D_BiznisEntita_Id_Uhrada IN (@IDs)
                                   ORDER BY d.VS";

                       uctDennikSdkFA.AddRange(Db.Select<UctDennikViewHelper>(sSql, new { DatStart = eSAMStart, IDs = request.D_BiznisEntita_Ids }));
                   }

                   ucty.AddRange(GetList(Db.From<UctRozvrh>().
                   SelectDistinct(x => new
                   {
                       x.C_UctRozvrh_Id,
                       x.VyzadovatStredisko,
                       x.VyzadovatProjekt,
                       x.SDK,
                       x.VyzadovatUctKluc1,
                       x.VyzadovatUctKluc2,
                       x.VyzadovatUctKluc3
                   }).
                   Where(x => Sql.In(x.C_UctRozvrh_Id, uctDennikSdkFA.Select(u => u.C_UctRozvrh_Id).Distinct()))));
               }

               try
               {
                   foreach (var be in biznisEntity)
                   {
                       long? projektId = be.C_Projekt_Id;
                       int? strediskoId = be.C_Stredisko_Id;
                       int? pokladnicaId = (tbe == (short)TypBiznisEntityEnum.PDK) ? be.C_Pokladnica_Id : null;
                       int? bankaUcetId = (tbe == (short)TypBiznisEntityEnum.BAN) ? be.C_BankaUcet_Id : null;
                       long? osobaId = (tbe != (short)TypBiznisEntityEnum.BAN && tbe != (short)TypBiznisEntityEnum.IND) ? be.D_Osoba_Id : null;
                       int kniha = be.C_TypBiznisEntity_Kniha_Id;

                       string VS = be.VS;
                       string SS = null;
                       string KS = null;
                       decimal DM_SumaKUhr = 0;
                       decimal DM_Kredit = 0;
                       decimal DM_Debet = 0;
                       DateTime? datSplat = be.DatumSplatnosti;

                       var predkontacieUct = predkontacieUctAll.Where(k => k.C_TypBiznisEntity_Kniha_Id == null || k.C_TypBiznisEntity_Kniha_Id == kniha).ToList();

                       switch ((TypBiznisEntityEnum)be.C_TypBiznisEntity_Id)
                       {
                           case TypBiznisEntityEnum.DFA:
                           case TypBiznisEntityEnum.OFA:
                               string typBe1 = ((TypBiznisEntityEnum)be.C_TypBiznisEntity_Id).ToString();
                               var data1 = Db.Select<(string, string, decimal)>($@"SELECT SS AS Item1, KS AS Item2, DM_SumaKUhr AS Item3
                                                               FROM crm.V_Doklad{typBe1} 
                                                               WHERE D_Tenant_Id = '{Session.TenantId}' AND D_Doklad{typBe1}_Id = {be.D_BiznisEntita_Id} AND Rok = {be.Rok}").First();
                               SS = data1.Item1;
                               KS = data1.Item2;
                               DM_SumaKUhr = data1.Item3;
                               break;


                           case TypBiznisEntityEnum.BAN:
                               var data3 = Db.Select<(decimal, decimal)>($@"SELECT DM_Kredit AS Item1, DM_Debet AS Item2 
                                                                               FROM fin.D_DokladBAN
                                                                               WHERE D_Tenant_Id = '{Session.TenantId}' AND D_DokladBAN_Id = {be.D_BiznisEntita_Id} AND Rok = {be.Rok}").First();
                               DM_Kredit = data3.Item1;
                               DM_Debet = data3.Item2;
                               VS = ""; //Bankový výpis nemá VS v hlavičke
                               KS = "";
                               SS = "";
                               datSplat = be.DatumDokladu; //Banka nemá dátum splatnosti
                               projektId = null;
                               break;

                           case TypBiznisEntityEnum.PDK:
                               VS = ""; //Bankový výpis nemá VS v hlavičke
                               KS = "";
                               SS = "";
                               strediskoId = null;
                               datSplat = be.DatumDokladu; //Pokladňa nemá dátum splatnosti
                               break;

                           default:
                               break;
                       }

                       //Zaúčtovanie sumárnych riadkov
                       foreach (var defGrp in predkontacieUct.Where(p => p.C_Predkontacia_Id == be.C_Predkontacia_Id && !p.Polozka &&
                             !((p.C_Stredisko_Id != null && strediskoId != p.C_Stredisko_Id) ||
                               (p.C_Pokladnica_Id != null && pokladnicaId != p.C_Pokladnica_Id) ||
                               (p.C_BankaUcet_Id != null && bankaUcetId != p.C_BankaUcet_Id) ||
                               (p.C_Projekt_Id != null && projektId != p.C_Projekt_Id) ||
                               (p.D_Osoba_Id != null && osobaId != p.D_Osoba_Id) ||
                               (p.KS != null && KS != p.KS) ||
                               (p.SS != null && SS != p.SS) ||
                               (p.VS != null && VS != p.VS) ||
                               (p.C_Lokalita_Id != null && be.C_Lokalita_Id != p.C_Lokalita_Id) ||
                               (p.C_OsobaTyp_Id != null && be.C_OsobaTyp_Id != p.C_OsobaTyp_Id)))
                           .GroupBy(x => x.C_Typ_Id))
                       {
                           var tmp = new List<(long D_BiznisEntita_Id, int Typ, int Poradie)>(); //Nebudeme informovať o duplicite pri sumačných typoch
                           List<PredkontaciaUctViewHelper> predkonGrp;

                           predkonGrp = defGrp.Where(x => x.C_UctRozvrh_Id_MD != null).ToList();
                           VyberPodlaPriorityUct(ref tmp, predkonGrp, 2, 0, be.D_BiznisEntita_Id);
                           CreateSumacneZau(ref nevyhovujucePolozky, tbe, uctDennikList, uctovatPolozkovite, ucty, be, strediskoId, projektId, osobaId, kniha, ref VS, DM_SumaKUhr, DM_Kredit, DM_Debet, datSplat, predkonGrp, zalohyFaktury);

                           predkonGrp = defGrp.Where(x => x.C_UctRozvrh_Id_Dal != null).ToList();
                           VyberPodlaPriorityUct(ref tmp, predkonGrp, 2, 0, be.D_BiznisEntita_Id);
                           CreateSumacneZau(ref nevyhovujucePolozky, tbe, uctDennikList, uctovatPolozkovite, ucty, be, strediskoId, projektId, osobaId, kniha, ref VS, DM_SumaKUhr, DM_Kredit, DM_Debet, datSplat, predkonGrp, zalohyFaktury);

                           predkonGrp = defGrp.Where(x => x.C_UctRozvrh_Id_MD == null && x.C_UctRozvrh_Id_Dal == null).ToList();
                           VyberPodlaPriorityUct(ref tmp, predkonGrp, 2, 0, be.D_BiznisEntita_Id);
                           CreateSumacneZau(ref nevyhovujucePolozky, tbe, uctDennikList, uctovatPolozkovite, ucty, be, strediskoId, projektId, osobaId, kniha, ref VS, DM_SumaKUhr, DM_Kredit, DM_Debet, datSplat, predkonGrp, zalohyFaktury);

                       }

                       //Zaúčtovanie položiek (aktuálne máme len položky BAN)
                       if (dokladBanPol != null)
                       {
                           foreach (var banPol in dokladBanPol.Where(b => b.D_BiznisEntita_Id == be.D_BiznisEntita_Id).OrderBy(x => x.Poradie))
                           {
                               //Vyfiltruj riadky predkontácie, ktoré vyhovujú a všetky vygeneruj
                               var predkontBanPolozky = predkontacieUct.Where(x => (x.Polozka && x.C_Predkontacia_Id == be.C_Predkontacia_Id && x.C_Typ_Id == banPol.C_Typ_Id &&
                                                                                 (x.C_BankaUcet_Id == null || x.C_BankaUcet_Id == bankaUcetId) &&
                                                                                 (x.C_Projekt_Id == null || x.C_Projekt_Id == banPol.C_Projekt_Id) &&
                                                                                 (x.KS == null || x.KS == banPol.KS) &&
                                                                                 (x.SS == null || x.SS == banPol.SS) &&
                                                                                 (x.VS == null || x.VS == banPol.VS)
                                                                                )
                                                                             );
                               if (predkontBanPolozky.Count() > 0)
                               {
                                   foreach (var def2 in predkontBanPolozky)
                                   {
                                       uctDennikList.Add(CreateUctDennikSingleRow(ucty, be, banPol, null, def2, tbe, kniha, strediskoId, projektId, osobaId, null, be.DatumDokladu, Math.Abs(banPol.Suma), banPol.Poradie, Math.Abs(banPol.Suma) != banPol.Suma, false, false, null, false, ref nevyhovujucePolozky, null));
                                   }
                               }
                               else
                               {
                                   if (banPol.C_Typ_Id != (int)TypEnum.UhradaPohZav)
                                   {
                                       uctDennikList.Add(CreateUctDennikSingleRow(ucty, be, banPol, null, null, tbe, kniha, strediskoId, projektId, osobaId, null, be.DatumDokladu, Math.Abs(banPol.Suma), banPol.Poradie, Math.Abs(banPol.Suma) != banPol.Suma, false, false, null, true, ref nevyhovujucePolozky, null));
                                   }
                               }

                               //Párovanie úhrad jednej položky:
                               foreach (UhradaParovanieViewHelper uhrPar in uhradaParovanie.Where(b => b.D_DokladBANPol_Id == banPol.D_DokladBANPol_Id).OrderBy(x => x.Poradie))
                               {
                                   CreateUctDennikFromParovanieUhrad(uctDennikList, ucty, predkontacieUct, uctDennikSdkFA, ref nevyhovujucePolozky, be, banPol, uhrPar, kniha, tbe, strediskoId, bankaUcetId, pokladnicaId, projektId, osobaId, false);
                                   CreateUctDennikFromParovanieUhrad(uctDennikList, ucty, predkontacieUct, uctDennikSdkFA, ref nevyhovujucePolozky, be, banPol, uhrPar, kniha, tbe, strediskoId, bankaUcetId, pokladnicaId, projektId, osobaId, true);
                               }

                           }
                       }
                       else if (uhradaParovanie != null) //Pokladňa a Vzájomné zápočty IND
                       {
                           foreach (var uhrPar in uhradaParovanie.Where(b => b.D_BiznisEntita_Id_Uhrada == be.D_BiznisEntita_Id).OrderBy(x => x.Poradie))
                           {
                               CreateUctDennikFromParovanieUhrad(uctDennikList, ucty, predkontacieUct, uctDennikSdkFA, ref nevyhovujucePolozky, be, null, uhrPar, kniha, tbe, strediskoId, bankaUcetId, pokladnicaId, projektId, osobaId, false);
                               CreateUctDennikFromParovanieUhrad(uctDennikList, ucty, predkontacieUct, uctDennikSdkFA, ref nevyhovujucePolozky, be, null, uhrPar, kniha, tbe, strediskoId, bankaUcetId, pokladnicaId, projektId, osobaId, true);
                           }
                       }

                       //finalne zoradenie
                       uctDennikList = uctDennikList.OrderBy(d => d.Poradie).ToList();
                       for (int i = 0; i < uctDennikList.Count; i++)
                       {
                           uctDennikList[i].Poradie = i + 1;
                       }

                       // vlozenie do DB
                       foreach (var dennik in uctDennikList)
                       {
                           Create(dennik);
                       }
                       uctDennikList.Clear();
                   }

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


           foreach (var dokladPol in nevyhovujucePolozky.GroupBy(x => x.D_BiznisEntita_Id))
           {
               int pocet = dokladPol.Count(x => x.Typ == 1);
               if (pocet > 0)
               {

                   if (pocet == 1) chybneDoklady.Add((dokladPol.Key, $"Riadok {nevyhovujucePolozky.Where(x => x.Typ == 1 && x.D_BiznisEntita_Id == dokladPol.First().D_BiznisEntita_Id).Select(x => x.Poradie).Join(", ")} bol predkontovaný bez účtu lebo nevyhovuje žiadnej definícii. "));
                   if (pocet > 1) chybneDoklady.Add((dokladPol.Key, $"Riadky {nevyhovujucePolozky.Where(x => x.Typ == 1 && x.D_BiznisEntita_Id == dokladPol.First().D_BiznisEntita_Id).Select(x => x.Poradie).Join(", ")} boli predkontované bez účtu lebo nevyhovujú žiadnej definícii. "));
               }

               pocet = dokladPol.Count(x => x.Typ == 4);
               if (pocet > 0)
               {
                   if (pocet == 1) chybneDoklady.Add((dokladPol.Key, $"Centové vyrovnanie na riadku {nevyhovujucePolozky.Where(x => x.Typ == 4 && x.D_BiznisEntita_Id == dokladPol.First().D_BiznisEntita_Id).Select(x => x.Poradie).Join(", ")} bol predkontované do účtovného denníka bez zaevidovania účtu. "));
                   if (pocet > 1) chybneDoklady.Add((dokladPol.Key, $"Centové vyrovnanie na riadkoch {nevyhovujucePolozky.Where(x => x.Typ == 4 && x.D_BiznisEntita_Id == dokladPol.First().D_BiznisEntita_Id).Select(x => x.Poradie).Join(", ")} bolo predkontované do účtovného denníka bez zaevidovania účtu. "));
               }

               pocet = dokladPol.Count(x => x.Typ == 2);
               if (pocet > 0)
               {
                   if (pocet == 1) chybneDoklady.Add((dokladPol.Key, $"Riadok {nevyhovujucePolozky.Where(x => x.Typ == 2 && x.D_BiznisEntita_Id == dokladPol.First().D_BiznisEntita_Id).Select(x => x.Poradie).Join(", ")} bol viacnásobne predkontovaný do účtovného denníka, keďže viacero definícií vyhovuje účtovanému záznamu. "));
                   if (pocet > 1) chybneDoklady.Add((dokladPol.Key, $"Riadky {nevyhovujucePolozky.Where(x => x.Typ == 2 && x.D_BiznisEntita_Id == dokladPol.First().D_BiznisEntita_Id).Select(x => x.Poradie).Join(", ")} boli viacnásobne predkontované do účtovného denníka, keďže viacero definícií vyhovuje účtovanému záznamu. "));
               }

               pocet = dokladPol.Count(x => x.Typ == 3);
               if (pocet > 0)
               {
                   if (pocet == 1) chybneDoklady.Add((dokladPol.Key, $"Na riadku {nevyhovujucePolozky.Where(x => x.Typ == 3 && x.D_BiznisEntita_Id == dokladPol.First().D_BiznisEntita_Id).Select(x => x.Poradie).Join(", ")} bolo viacnásobne predkontované do účtovného denníka centové vyrovnanie, keďže viacero definícií vyhovuje účtovanému záznamu. "));
                   if (pocet > 1) chybneDoklady.Add((dokladPol.Key, $"Na riadkoch {nevyhovujucePolozky.Where(x => x.Typ == 3 && x.D_BiznisEntita_Id == dokladPol.First().D_BiznisEntita_Id).Select(x => x.Poradie).Join(", ")} boli viacnásobne predkontované do účtovného denníka centové vyrovnania, keďže viacero definícií vyhovuje účtovanému záznamu. "));
               }

               pocet = dokladPol.Count(x => x.Typ == 5);
               if (pocet > 0)
               {
                   if (pocet == 1) chybneDoklady.Add((dokladPol.Key, $"Na riadku {nevyhovujucePolozky.Where(x => x.Typ == 5 && x.D_BiznisEntita_Id == dokladPol.First().D_BiznisEntita_Id).Select(x => x.Poradie).Join(", ")} je úhrada faktúry, ktorej nebol v zaúčtovaní nájdený saldokontný účet. "));
                   if (pocet > 1) chybneDoklady.Add((dokladPol.Key, $"Na riadkoch {nevyhovujucePolozky.Where(x => x.Typ == 5 && x.D_BiznisEntita_Id == dokladPol.First().D_BiznisEntita_Id).Select(x => x.Poradie).Join(", ")} sú úhrady faktúr, ktorým nebol v zaúčtovaní nájdený saldokontný účet. "));
               }
           }
       }

       private static void CreateSumacneZau(ref List<(long D_BiznisEntita_Id, int Typ, int Poradie)> nevyhovujucePolozky,
                                    short tbe,
                                    List<UctDennik> uctDennikList,
                                    bool uctovatPolozkovite,
                                    List<UctRozvrh> ucty,
                                    BiznisEntitaView be,
                                    int? strediskoId,
                                    long? projektId,
                                    long? osobaId,
                                    int kniha,
                                    ref string VS,
                                    decimal DM_SumaKUhr,
                                    decimal DM_Kredit,
                                    decimal DM_Debet,
                                    DateTime? datSplat,
                                    List<PredkontaciaUctViewHelper> predkonGrp,
                                    List<BiznisEntita_ZalohaView> zalohyFaktury)
       {
           foreach (PredkontaciaUctViewHelper def1 in predkonGrp)
           {
               decimal hodnota = def1.C_Typ_Id switch
               {
                   (int)TypEnum.SumaDokladu => be.DM_Suma,
                   (int)TypEnum.SumaKUhrade => DM_SumaKUhr,
                   (int)TypEnum.SumaDebet => DM_Debet,
                   (int)TypEnum.SumaKredit => DM_Kredit,
                   (int)TypEnum.ZakladDPH => def1.SadzbaDph_Id switch
                   {
                       0 => be.DM_Zak0,
                       1 => be.DM_Zak1,
                       2 => be.DM_Zak2,
                       _ => be.DM_Zak0 + be.DM_Zak1 + be.DM_Zak2
                   },
                   (int)TypEnum.DPH => def1.SadzbaDph_Id switch
                   {
                       1 => be.DM_DPH1,
                       2 => be.DM_DPH2,
                       _ => be.DM_DPH1 + be.DM_DPH2
                   },
                   (int)TypEnum.CentVyrovnanieHLA => be.DM_CV,
                   _ => 0
               };

               if (def1.C_Typ_Id == (int)TypEnum.ZalohaVSDokladu && zalohyFaktury.Count > 0)
               {
                   decimal suma = zalohyFaktury.Sum(x => x.DM_Cena);
                   if (suma != 0)
                   {
                       uctDennikList.Add(CreateUctDennikSingleRow(ucty, be, null, null, def1, tbe, kniha, strediskoId, projektId, osobaId, VS, datSplat, suma,
                           def1.Poradie, false, false, false, null, false, ref nevyhovujucePolozky, null));
                   }
               }
               else if (def1.C_Typ_Id == (int)TypEnum.ZalohaVSZalohy && zalohyFaktury.Count > 0)
               {
                   //VS + sumár z gridu záloh
                   foreach (var zf in zalohyFaktury)
                   {
                       uctDennikList.Add(CreateUctDennikSingleRow(ucty, be, null, null, def1, tbe, kniha, strediskoId, projektId, osobaId, zf.VS, datSplat, zf.DM_Cena,
                           def1.Poradie, false, false, false, null, false, ref nevyhovujucePolozky, zf.Popis));
                   }

               }
               else if (hodnota != 0)
               {
                   //Pri položkovitej predkontácii chcem mať sumárne typy na začiatku zoradené podľa predkontácie a potom až položkové
                   uctDennikList.Add(CreateUctDennikSingleRow(ucty, be, null, null, def1, tbe, kniha, strediskoId, projektId, osobaId, VS, datSplat, hodnota,
                       (uctovatPolozkovite ? -10000 : 0) + def1.Poradie, false, false, false, null, false, ref nevyhovujucePolozky, null));
               }
           }
       }

       public void PredkontujDokladRzp(PredkontovatDokladDto request, out List<(long D_BiznisEntita_Id, string Chyba)> chybneDoklady)
       {
           // Key 1 - nevyhovujuce, 2- viacnasobne, 3 - cent vyr.
           var nevyhovujucePolozky = new List<(long D_BiznisEntita_Id, int Typ, int Poradie)>();
           chybneDoklady = new List<(long D_BiznisEntita_Id, string Chyba)>();

           using (var transaction = BeginTransaction())
           {
               List<RzpDennik> rzpDennikList = new List<RzpDennik>();
               List<RzpDennikViewHelper> rzpDennikPredbezne = null;
               List<DokladBANPolViewHelper> dokladBanPol = null;
               List<UhradaParovanieViewHelper> uhradaParovanie = null;

               var biznisEntity = GetList(Db.From<BiznisEntitaView>().
                   Where(x => Sql.In(x.D_BiznisEntita_Id, request.D_BiznisEntita_Ids)));

               short tbe = biznisEntity.First().C_TypBiznisEntity_Id;

               // nacitanie nastavenia "UctovatPolozkovite"
               bool uctovatPolozkovite = GetTypBiznisEntityNastavView().Where(x => x.C_TypBiznisEntity_Id == tbe).FirstOrDefault()?.UctovatPolozkovite ?? true;

               SqlExpression<PredkontaciaRzpViewHelper> sqlExp = Db.From<PredkontaciaRzpViewHelper>().
                   Where(p => Sql.In(p.C_Predkontacia_Id, biznisEntity.Select(m => m.C_Predkontacia_Id).Distinct()));

               var predkontacieRzpAll = uctovatPolozkovite ?
                   GetList(sqlExp).OrderBy(p => p.Polozka).ThenBy(p => p.Poradie) :
                   GetList(sqlExp).OrderBy(p => p.Poradie);

               List<RzpPol> rzpPolozky = GetList(Db.From<RzpPol>().
                   Select(x => new
                   {
                       x.C_RzpPol_Id,
                       x.Stredisko,
                       x.Projekt,
                       x.OpacnaStrana,
                       x.PrijemVydaj
                   }).
                   Where(x => Sql.In(x.C_RzpPol_Id, predkontacieRzpAll.Select(u => u.C_RzpPol_Id).Distinct())));

               if (request.VymazatZaznamy)
               {
                   // zmazat zaznamy v uctDenniku. Púšťať predkontáciu cez roky asi nikto nebude, takže rok môžem zobrať z prvého
                   Db.Delete<RzpDennik>(e => Sql.In(e.D_BiznisEntita_Id, request.D_BiznisEntita_Ids) && e.Rok == biznisEntity.First().Rok);
               }

               if (tbe == (short)TypBiznisEntityEnum.BAN ||
                   tbe == (short)TypBiznisEntityEnum.PDK ||
                   tbe == (short)TypBiznisEntityEnum.IND)
               {
                   if (tbe == (short)TypBiznisEntityEnum.BAN)
                   {
                       //Dávam VIEW aby som mal ošetrené zmazané záznamy
                       dokladBanPol = GetList(Db.From<DokladBANPolViewHelper>()
                           .Where(e => Sql.In(e.D_BiznisEntita_Id, request.D_BiznisEntita_Ids) && e.Rok == biznisEntity.First().Rok &&
                           (e.RzpDefinicia != -1 || e.C_Typ_Id == (int)TypEnum.UhradaPohZav)));//-1 = Nerozpočtovať  (RzpDefinicia != -1 OR C_Typ_Id = 130)
                   }

                   //Dávam VIEW aby som mal ošetrené zmazané záznamy
                   uhradaParovanie = GetList(Db.From<UhradaParovanieViewHelper>()
                       .Where(e => Sql.In(e.D_BiznisEntita_Id_Uhrada, request.D_BiznisEntita_Ids) && e.Rok == biznisEntity.First().Rok && e.RzpDefinicia != -1));//-1 = Nerozpočtovať

                   rzpDennikPredbezne = GetList(Db.From<RzpDennikViewHelper>()
                       .Where(e => e.R &&
                                   Sql.In(e.D_BiznisEntita_Id, uhradaParovanie.Where(x => x.D_BiznisEntita_Id_Predpis != null)
                                                                              .Select(x => x.D_BiznisEntita_Id_Predpis)
                                                                              .Distinct())));
                   rzpPolozky.AddRange(GetList(Db.From<RzpPol>().
                       Select(x => new
                       {
                           x.C_RzpPol_Id,
                           x.Stredisko,
                           x.Projekt,
                           x.OpacnaStrana,
                           x.PrijemVydaj
                       }).
                       Where(x => Sql.In(x.C_RzpPol_Id, rzpDennikPredbezne.Select(u => u.C_RzpPol_Id).Distinct()))));
               }

               try
               {
                   foreach (var be in biznisEntity)
                   {
                       int? strediskoId = be.C_Stredisko_Id;
                       long? projektId = be.C_Projekt_Id;
                       long? osobaId = (tbe != (short)TypBiznisEntityEnum.BAN && tbe != (short)TypBiznisEntityEnum.IND) ? be.D_Osoba_Id : null;
                       int kniha = be.C_TypBiznisEntity_Kniha_Id;

                       decimal DM_SumaKUhr = 0;

                       var predkontacieRzp = predkontacieRzpAll.Where(k => k.C_TypBiznisEntity_Kniha_Id == null || k.C_TypBiznisEntity_Kniha_Id == kniha).ToList();

                       switch ((TypBiznisEntityEnum)be.C_TypBiznisEntity_Id)
                       {
                           case TypBiznisEntityEnum.DFA:
                           case TypBiznisEntityEnum.OFA:
                           case TypBiznisEntityEnum.OZF:
                           case TypBiznisEntityEnum.DZF:
                           case TypBiznisEntityEnum.DOB:
                           case TypBiznisEntityEnum.OOB:
                           case TypBiznisEntityEnum.DZM:
                           case TypBiznisEntityEnum.OZM:
                               string typBe1 = ((TypBiznisEntityEnum)be.C_TypBiznisEntity_Id).ToString();
                               string fldKUhr = (typBe1 == "DFA" || typBe1 == "OFA") ? "DM_SumaKUhr" : "DM_Suma";
                               DM_SumaKUhr = Db.Scalar<decimal>($@"SELECT {fldKUhr} FROM crm.V_Doklad{typBe1} 
                                                                   WHERE D_Tenant_Id = '{Session.TenantId}' AND D_Doklad{typBe1}_Id = {be.D_BiznisEntita_Id} AND Rok = {be.Rok}");
                               break;

                           default:
                               break;
                       }

                       //Zaúčtovanie sumárnych riadkov

                       foreach (var defGrp in predkontacieRzp.Where(p => p.C_Predkontacia_Id == be.C_Predkontacia_Id && !p.Polozka &&
                             !((p.C_Stredisko_Id != null && strediskoId != p.C_Stredisko_Id) ||
                               (p.C_Projekt_Id != null && projektId != p.C_Projekt_Id) ||
                               (p.D_Osoba_Id != null && osobaId != p.D_Osoba_Id) ||
                               (p.C_OsobaTyp_Id != null && be.C_OsobaTyp_Id != p.C_OsobaTyp_Id)))
                           .GroupBy(x => x.C_Typ_Id))
                       {
                           var predkonGrp = defGrp.ToList();

                           var tmp = new List<(long D_BiznisEntita_Id, int Typ, int Poradie)>(); //Nebudeme informovať o duplicite pri sumačných typoch
                           VyberPodlaPriorityRzp(ref tmp, predkonGrp, 2, 0, be.D_BiznisEntita_Id);

                           foreach (PredkontaciaRzpViewHelper def1 in predkonGrp)
                           {
                               decimal hodnota = def1.C_Typ_Id switch
                               {
                                   (int)TypEnum.SumaDokladu => be.DM_Suma,
                                   (int)TypEnum.SumaKUhrade => DM_SumaKUhr,
                                   //(int)TypEnum.SumaDebet => DM_Debet, --Nerozpočtovaný typ
                                   //(int)TypEnum.SumaKredit => DM_Kredit, --Nerozpočtovaný typ
                                   //(int)TypEnum.ZalohaVSDokladu => DM_SumaZal, --Nerozpočtovaný typ
                                   //(int)TypEnum.ZalohaVSZalohy => DM_SumaZal,--Nerozpočtovaný typ
                                   (int)TypEnum.CentVyrovnanieHLA => be.DM_CV,
                                   _ => 0
                               };

                               if (hodnota != 0)
                               {
                                   //Pri položkovitej predkontácii chcem mať sumárne typy na začiatku zoradené podľa predkontácie a potom až položkové
                                   //Pri sumárnej predkontácii chcem mať poradie z predkontácie zachované
                                   rzpDennikList.Add(CreateRzpDennikSingleRow(rzpPolozky, be, strediskoId, projektId, null, null, def1, null, hodnota, (uctovatPolozkovite ? -10000 : 0) + def1.Poradie));
                               }
                           }
                       }

                       //Zaúčtovanie položiek (aktuálne máme len položky BAN)
                       if (dokladBanPol != null)
                       {
                           foreach (var banPol in dokladBanPol.Where(b => b.D_BiznisEntita_Id == be.D_BiznisEntita_Id).OrderBy(x => x.Poradie))
                           {
                               //Vyfiltruj riadky predkontácie, ktoré vyhovujú a všetky vygeneruj
                               var predkontBanPolozky = predkontacieRzp.Where(x => (x.Polozka && x.C_Typ_Id == banPol.C_Typ_Id && x.C_Predkontacia_Id == be.C_Predkontacia_Id &&
                                                                                    (x.C_Projekt_Id == null || x.C_Projekt_Id == banPol.C_Projekt_Id)
                                                                                   )
                                                                             );
                               if (predkontBanPolozky.Count() > 0)
                               {
                                   foreach (var def2 in predkontBanPolozky)
                                   {
                                       rzpDennikList.Add(CreateRzpDennikSingleRow(rzpPolozky, be, strediskoId, projektId, banPol, null, def2, null, Math.Abs(banPol.Suma), banPol.Poradie));
                                   }
                               }
                               else
                               {
                                   if (banPol.C_Typ_Id != (int)TypEnum.UhradaPohZav)
                                   {
                                       nevyhovujucePolozky.AddIfNotExists((be.D_BiznisEntita_Id, 1, banPol.Poradie));
                                       //Pridám riadok ale bez rzp. položky a programu
                                       rzpDennikList.Add(CreateRzpDennikSingleRow(rzpPolozky, be, strediskoId, projektId, banPol, null, null, null, Math.Abs(banPol.Suma), banPol.Poradie));
                                   }
                               }

                               //Párovanie úhrad jednej položky:
                               foreach (UhradaParovanieViewHelper uhrPar in uhradaParovanie.Where(b => b.D_DokladBANPol_Id == banPol.D_DokladBANPol_Id).OrderBy(x => x.Poradie))
                               {
                                   CreateRzpDennikFromParovanieUhrad(rzpDennikList, kniha, predkontacieRzp, rzpPolozky, rzpDennikPredbezne, be, strediskoId, projektId, osobaId, banPol, uhrPar, ref nevyhovujucePolozky);
                               }

                           }
                       }
                       else if (uhradaParovanie != null) //Pokladňa a Vzájomné zápočty IND
                       {
                           foreach (var uhrPar in uhradaParovanie.Where(b => b.D_BiznisEntita_Id_Uhrada == be.D_BiznisEntita_Id).OrderBy(x => x.Poradie))
                           {
                               CreateRzpDennikFromParovanieUhrad(rzpDennikList, kniha, predkontacieRzp, rzpPolozky, rzpDennikPredbezne, be, strediskoId, projektId, osobaId, null, uhrPar, ref nevyhovujucePolozky);
                           }
                       }

                       //finalne zoradenie
                       rzpDennikList = rzpDennikList.OrderBy(d => d.Poradie).ToList();
                       for (int i = 0; i < rzpDennikList.Count; i++)
                       {
                           rzpDennikList[i].Poradie = i + 1;
                       }

                       // vlozenie do DB
                       foreach (var dennik in rzpDennikList)
                       {
                           Create(dennik);
                       }
                       rzpDennikList.Clear();
                   }

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

           foreach (var dokladPol in nevyhovujucePolozky.GroupBy(x => x.D_BiznisEntita_Id))
           {
               int pocet = dokladPol.Count(x => x.Typ == 1);
               if (pocet > 0)
               {

                   if (pocet == 1) chybneDoklady.Add((dokladPol.Key, $"Riadok {dokladPol.Where(x => x.Typ == 1 && x.D_BiznisEntita_Id == dokladPol.First().D_BiznisEntita_Id).Select(x => x.Poradie).Join(", ")} bol predkontovaný do rozpočtového denníka bez zaevidovania rozpočtovej položky a prípadného programu. "));
                   if (pocet > 1) chybneDoklady.Add((dokladPol.Key, $"Riadky {dokladPol.Where(x => x.Typ == 1 && x.D_BiznisEntita_Id == dokladPol.First().D_BiznisEntita_Id).Select(x => x.Poradie).Join(", ")} boli predkontované do rozpočtového denníka bez zaevidovania rozpočtovej položky a prípadného programu. "));
               }

               pocet = dokladPol.Count(x => x.Typ == 2);
               if (pocet > 0)
               {
                   if (pocet == 1) chybneDoklady.Add((dokladPol.Key, $"Riadok {dokladPol.Where(x => x.Typ == 2 && x.D_BiznisEntita_Id == dokladPol.First().D_BiznisEntita_Id).Select(x => x.Poradie).Join(", ")} bol viacnásobne predkontovaný do rozpočtového denníka, keďže viacero definícií vyhovuje účtovanému záznamu. "));
                   if (pocet > 1) chybneDoklady.Add((dokladPol.Key, $"Riadky {dokladPol.Where(x => x.Typ == 2 && x.D_BiznisEntita_Id == dokladPol.First().D_BiznisEntita_Id).Select(x => x.Poradie).Join(", ")} boli viacnásobne predkontované do rozpočtového denníka, keďže viacero definícií vyhovuje účtovanému záznamu. "));
               }

               pocet = dokladPol.Count(x => x.Typ == 3);
               if (pocet > 0)
               {
                   if (pocet == 1) chybneDoklady.Add((dokladPol.Key, $"Na riadku {dokladPol.Where(x => x.Typ == 3 && x.D_BiznisEntita_Id == dokladPol.First().D_BiznisEntita_Id).Select(x => x.Poradie).Join(", ")} bolo viacnásobne predkontované do rozpočtového denníka centové vyrovnanie, keďže viacero definícií vyhovuje účtovanému záznamu. "));
                   if (pocet > 1) chybneDoklady.Add((dokladPol.Key, $"Na riadkoch {dokladPol.Where(x => x.Typ == 3 && x.D_BiznisEntita_Id == dokladPol.First().D_BiznisEntita_Id).Select(x => x.Poradie).Join(", ")} boli viacnásobne predkontované do rozpočtového denníka centové vyrovnania, keďže viacero definícií vyhovuje účtovanému záznamu. "));
               }
           }
       }

       private static void CreateUctDennikFromParovanieUhrad(List<UctDennik> uctDennikList, List<UctRozvrh> ucty,
                                                             List<PredkontaciaUctViewHelper> predkontacieUct,
                                                             List<UctDennikViewHelper> uctDennikSdkFA,
                                                             //List<UctDennikViewHelper> uctDennikDap,
                                                             ref List<(long D_BiznisEntita_Id, int Typ, int Poradie)> nevyhovujucePolozky,
                                                             BiznisEntitaView be, DokladBANPolViewHelper banPol,
                                                             UhradaParovanieViewHelper uhrPar, int kniha, short tbe,
                                                             int? strediskoId, int? bankaUcetId, int? pokladnicaId,
                                                             long? projektId,
                                                             long? osobaId, bool rozdiel)
       {
           List<PredkontaciaUctViewHelper> predkontacieUctRow;
           short rokBE = be.Rok;
           short rokPredpis = uhrPar.Rok_Predpis;
           decimal val = rozdiel ? uhrPar.DM_Rozdiel : uhrPar.DM_Cena;
           decimal roz = uhrPar.DM_Rozdiel;

           bool otoceneZnamienko;
           bool zisk = false;

           if (val == 0) return;

           if (kniha != (short)TypBiznisEntity_KnihaEnum.Vydajove_pokladnicne_doklady)
           {
               if (uhrPar.C_Typ_Id == (int)TypEnum.UhradaDFA || uhrPar.C_Typ_Id == (int)TypEnum.UhradaDZF ||
                   uhrPar.C_Typ_Id == (int)TypEnum.DobropisDFA || uhrPar.C_Typ_Id == (int)TypEnum.ZalohyPoskytnute)
               {
                   val *= (-1);
                   roz *= (-1);
               }
           }

           //Vyfiltruj riadky predkontácie, ktoré vyhovujú a všetky vygeneruj
           if (!rozdiel)
           {
               otoceneZnamienko = val != uhrPar.DM_Cena;
               predkontacieUctRow = predkontacieUct.Where(x => x.Polozka && x.C_Predkontacia_Id == be.C_Predkontacia_Id && x.C_Typ_Id == uhrPar.C_Typ_Id &&
                   (x.C_BankaUcet_Id == null || tbe != (short)TypBiznisEntityEnum.BAN || x.C_BankaUcet_Id == bankaUcetId) &&
                   (x.C_Pokladnica_Id == null || tbe != (short)TypBiznisEntityEnum.PDK || x.C_Pokladnica_Id == pokladnicaId) &&
                   (x.C_Stredisko_Id == null || x.C_Stredisko_Id == (uhrPar?.C_Stredisko_Id ?? banPol?.C_Stredisko_Id ?? strediskoId)) &&
                   (x.C_Projekt_Id == null || x.C_Projekt_Id == (uhrPar?.C_Projekt_Id ?? banPol?.C_Projekt_Id ?? projektId)) &&
                   (x.C_Lokalita_Id == null || x.C_Lokalita_Id == be.C_Lokalita_Id) && //Beriem z BE
                   (x.C_Projekt_Id == null || x.C_Projekt_Id == (uhrPar.C_Projekt_Id ?? banPol?.C_Projekt_Id)) &&
                   (x.D_Osoba_Id == null || x.D_Osoba_Id == (uhrPar.D_Osoba_Id ?? osobaId)) &&
                   (x.C_OsobaTyp_Id == null || x.C_OsobaTyp_Id == uhrPar.C_OsobaTyp_Id) &&
                   (x.C_Druh_Id == null || x.C_Druh_Id == uhrPar.C_Druh_Id) &&
                   (x.C_Kod_Id == null || x.C_Kod_Id == uhrPar.C_Kod_Id) &&
                   (x.C_Odsek_Id == null || x.C_Odsek_Id == uhrPar.C_Odsek_Id) &&
                   (x.KS == null || x.KS == banPol?.KS) && //beriem z bankovej položky
                   (x.SS == null || x.SS == banPol?.SS) && //beriem z bankovej položky
                   (x.VS == null || x.VS == uhrPar.VS) &&
                   (string.IsNullOrEmpty(x.DapRok) || x.DapRok == "N" ||
                   (x.DapRok == "A" && rokBE == rokPredpis) ||
                   (x.DapRok == "M" && rokBE > rokPredpis) ||
                   x.DapRok == rokPredpis.ToString())
               ).ToList();
           }
           else
           {
               val = Math.Abs(uhrPar.DM_Rozdiel);
               //Zisti či sa jedná o (9  - Cent.vyr.preplatok) alebo nedoplatok (10 - Cent.vyr.nedoplatok)
               zisk = kniha == (int)TypBiznisEntity_KnihaEnum.Vydajove_pokladnicne_doklady ? uhrPar.DM_Rozdiel > 0 : uhrPar.DM_Rozdiel < 0;
               otoceneZnamienko = val != uhrPar.DM_Rozdiel;

               int typ = zisk ? (int)TypEnum.CentVyrovnaniePreplatok : (int)TypEnum.CentVyrovnanieNedoplatok;

               predkontacieUctRow = predkontacieUct.Where(x => !x.Polozka && x.C_Predkontacia_Id == be.C_Predkontacia_Id && x.C_Typ_Id == typ &&
                   (x.C_BankaUcet_Id == null || tbe != (short)TypBiznisEntityEnum.BAN || x.C_BankaUcet_Id == bankaUcetId) &&
                   (x.C_Pokladnica_Id == null || tbe != (short)TypBiznisEntityEnum.PDK || x.C_Pokladnica_Id == pokladnicaId) &&
                   (x.C_Stredisko_Id == null || tbe == (short)TypBiznisEntityEnum.BAN || x.C_Stredisko_Id == (uhrPar?.C_Stredisko_Id ?? banPol?.C_Stredisko_Id ?? strediskoId)) &&
                   (x.C_Lokalita_Id == null || x.C_Lokalita_Id == be.C_Lokalita_Id) && //Beriem z BE
                   (x.C_Projekt_Id == null || x.C_Projekt_Id == (uhrPar.C_Projekt_Id ?? banPol?.C_Projekt_Id ?? projektId)) &&
                   (x.D_Osoba_Id == null || x.D_Osoba_Id == (uhrPar.D_Osoba_Id ?? osobaId)) &&
                   (x.C_OsobaTyp_Id == null || x.C_OsobaTyp_Id == uhrPar.C_OsobaTyp_Id) &&
                   (x.C_Druh_Id == null || x.C_Druh_Id == uhrPar.C_Druh_Id) &&
                   (x.C_Kod_Id == null || x.C_Kod_Id == uhrPar.C_Kod_Id) &&
                   (x.C_Odsek_Id == null || x.C_Odsek_Id == uhrPar.C_Odsek_Id) &&
                   (x.KS == null || x.KS == banPol?.KS) && //beriem z bankovej položky
                   (x.SS == null || x.SS == banPol?.SS) && //beriem z bankovej položky
                   (x.VS == null || x.VS == uhrPar.VS) &&
                   (string.IsNullOrEmpty(x.DapRok) || x.DapRok == "N" ||
                   (x.DapRok == "A" && rokBE == rokPredpis) ||
                   (x.DapRok == "M" && rokBE > rokPredpis) ||
                   x.DapRok == rokPredpis.ToString())
               ).ToList();
           }

           if (predkontacieUctRow.Count() > 0)
           {
               VyberPodlaPriorityUct(ref nevyhovujucePolozky, predkontacieUctRow, rozdiel ? 3 : 2, uhrPar.Poradie, be.D_BiznisEntita_Id);

               foreach (PredkontaciaUctViewHelper def3 in predkontacieUctRow)
               {
                   uctDennikList.Add(CreateUctDennikSingleRow(ucty, be, banPol, uhrPar, def3, tbe, kniha, strediskoId, projektId, osobaId, null, banPol?.DatumPohybu ?? be.DatumDokladu, val, banPol?.Poradie ?? uhrPar.Poradie, otoceneZnamienko, rozdiel, zisk, uctDennikSdkFA, true, ref nevyhovujucePolozky, null));
                   if (!rozdiel && roz != 0)
                   {
                       uctDennikList.Add(CreateUctDennikSingleRow(ucty, be, banPol, uhrPar, def3, tbe, kniha, strediskoId, projektId, osobaId, null, banPol?.DatumPohybu ?? be.DatumDokladu, roz, banPol?.Poradie ?? uhrPar.Poradie, otoceneZnamienko, rozdiel, zisk, uctDennikSdkFA, false, ref nevyhovujucePolozky, null));
                   }
               }
           }
           else
           {
               //Pridám riadok ale bez vyplneného účtu
               uctDennikList.Add(CreateUctDennikSingleRow(ucty, be, banPol, uhrPar, null, tbe, kniha, strediskoId, projektId, osobaId, null, banPol?.DatumPohybu ?? be.DatumDokladu, val, banPol?.Poradie ?? uhrPar.Poradie, otoceneZnamienko, rozdiel, zisk, uctDennikSdkFA, true, ref nevyhovujucePolozky, null));
               if (!rozdiel && roz != 0)
               {
                   uctDennikList.Add(CreateUctDennikSingleRow(ucty, be, banPol, uhrPar, null, tbe, kniha, strediskoId, projektId, osobaId, null, banPol?.DatumPohybu ?? be.DatumDokladu, roz, banPol?.Poradie ?? uhrPar.Poradie, otoceneZnamienko, rozdiel, zisk, uctDennikSdkFA, false, ref nevyhovujucePolozky, null));
               }
           }
       }

       private static void VyberPodlaPriorityUct(ref List<(long D_BiznisEntita_Id, int Typ, int Poradie)> nevyhovujucePolozky, List<PredkontaciaUctViewHelper> predkontSource, int typ, int poradie, long D_BiznisEntita_Id)
       {
           while (predkontSource.Count > 1)
           {
               if (predkontSource.Any(x => string.IsNullOrEmpty(x.DapRok) || x.DapRok == "N") && predkontSource.Any(x => !string.IsNullOrEmpty(x.DapRok) && x.DapRok != "N"))
               {
                   predkontSource.RemoveAll(x => string.IsNullOrEmpty(x.DapRok) || x.DapRok == "N");
                   continue;
               }

               if (predkontSource.Any(x => x.C_Druh_Id == null) && predkontSource.Any(x => x.C_Druh_Id != null))
               {
                   predkontSource.RemoveAll(x => x.C_Druh_Id == null);
                   continue;
               }

               if (predkontSource.Any(x => x.C_Kod_Id == null) && predkontSource.Any(x => x.C_Kod_Id != null))
               {
                   predkontSource.RemoveAll(x => x.C_Kod_Id == null);
                   continue;
               }

               if (predkontSource.Any(x => x.C_Odsek_Id == null) && predkontSource.Any(x => x.C_Odsek_Id != null))
               {
                   predkontSource.RemoveAll(x => x.C_Odsek_Id == null);
                   continue;
               }

               if (predkontSource.Any(x => x.D_Osoba_Id == null) && predkontSource.Any(x => x.D_Osoba_Id != null))
               {
                   predkontSource.RemoveAll(x => x.D_Osoba_Id == null);
                   continue;
               }

               if (predkontSource.Any(x => x.C_OsobaTyp_Id == null) && predkontSource.Any(x => x.C_OsobaTyp_Id != null))
               {
                   predkontSource.RemoveAll(x => x.C_OsobaTyp_Id == null);
                   continue;
               }

               if (predkontSource.Any(x => x.VS == null) && predkontSource.Any(x => x.VS != null))
               {
                   predkontSource.RemoveAll(x => x.VS == null);
                   continue;
               }

               if (predkontSource.Any(x => x.SS == null) && predkontSource.Any(x => x.SS != null))
               {
                   predkontSource.RemoveAll(x => x.SS == null);
                   continue;
               }

               if (predkontSource.Any(x => x.KS == null) && predkontSource.Any(x => x.KS != null))
               {
                   predkontSource.RemoveAll(x => x.KS == null);
                   continue;
               }

               if (predkontSource.Any(x => x.C_TypBiznisEntity_Kniha_Id == null) && predkontSource.Any(x => x.C_TypBiznisEntity_Kniha_Id != null))
               {
                   predkontSource.RemoveAll(x => x.C_TypBiznisEntity_Kniha_Id == null);
                   continue;
               }

               if (predkontSource.Count() > 1)
               {
                   nevyhovujucePolozky.AddIfNotExists((D_BiznisEntita_Id, typ, poradie));
               }
               break;
           }
       }

       private static void CreateRzpDennikFromParovanieUhrad(List<RzpDennik> rzpDennikList, int kniha,
                                                             List<PredkontaciaRzpViewHelper> predkontacieRzp,
                                                             List<RzpPol> rzpPolozky,
                                                             List<RzpDennikViewHelper> rzpDennikPredbezne,
                                                             BiznisEntitaView be, int? strediskoId, long? projektId, long? osobaId,
                                                             DokladBANPolViewHelper banPol,
                                                             UhradaParovanieViewHelper uhrPar,
                                                             ref List<(long D_BiznisEntita_Id, int Typ, int Poradie)> nevyhovujucePolozky)
       {
           short rokBE = be.Rok;
           short rokPredpis = uhrPar.Rok_Predpis;

           //Vyfiltruj riadky predkontácie, ktoré vyhovujú a všetky vygeneruj
           var predkontPolPar = predkontacieRzp.Where(x => x.Polozka && x.C_Predkontacia_Id == be.C_Predkontacia_Id && x.C_Typ_Id == uhrPar.C_Typ_Id &&
                       (x.C_Stredisko_Id == null || x.C_Stredisko_Id == (uhrPar?.C_Stredisko_Id ?? banPol?.C_Stredisko_Id ?? strediskoId)) &&
                       (x.C_Projekt_Id == null || x.C_Projekt_Id == (uhrPar.C_Projekt_Id ?? banPol?.C_Projekt_Id ?? projektId)) &&
                       (x.D_Osoba_Id == null || x.D_Osoba_Id == (uhrPar.D_Osoba_Id ?? osobaId)) &&
                       (x.C_OsobaTyp_Id == null || x.C_OsobaTyp_Id == uhrPar.C_OsobaTyp_Id) &&
                       (x.C_Druh_Id == null || x.C_Druh_Id == uhrPar.C_Druh_Id) &&
                       (x.C_Kod_Id == null || x.C_Kod_Id == uhrPar.C_Kod_Id) &&
                       (x.C_Odsek_Id == null || x.C_Odsek_Id == uhrPar.C_Odsek_Id) &&
                       (string.IsNullOrEmpty(x.DapRok) || x.DapRok == "N" ||
                        (x.DapRok == "A" && rokBE == rokPredpis) ||
                        (x.DapRok == "M" && rokBE > rokPredpis) ||
                        x.DapRok == rokPredpis.ToString())
                       ).ToList();

           decimal val = uhrPar.DM_Cena + uhrPar.DM_Rozdiel;

           if (kniha != (short)TypBiznisEntity_KnihaEnum.Vydajove_pokladnicne_doklady)
           {
               if (uhrPar.C_Typ_Id == (int)TypEnum.UhradaDFA || uhrPar.C_Typ_Id == (int)TypEnum.UhradaDZF ||
                   uhrPar.C_Typ_Id == (int)TypEnum.DobropisDFA || uhrPar.C_Typ_Id == (int)TypEnum.ZalohyPoskytnute)
               {
                   val *= -1;
               }
           }

           //Vyfiltruj riadky predbežného čerpania/plnenia
           var predbezneCP = rzpDennikPredbezne.Where(x => x.D_BiznisEntita_Id == uhrPar.D_BiznisEntita_Id_Predpis);

           if (predbezneCP.Any())
           {
               decimal rzpTotal = predbezneCP.Sum(x => x.Suma); //* (x.PrijemVydaj == 2 ? (-1) : 1)
               if (rzpTotal == val)
               {
                   //postačí iba 1:1 natiahnuť údaje bez zložitého rozgenerovania ako je potrebné pre čiastkové úhrady
                   foreach (RzpDennikViewHelper predbCPRow in predbezneCP)
                   {
                       rzpDennikList.Add(CreateRzpDennikSingleRow(rzpPolozky, be, predbCPRow.C_Stredisko_Id ?? strediskoId, predbCPRow.C_Projekt_Id ?? projektId, banPol, uhrPar, null, predbCPRow, predbCPRow.Suma, banPol?.Poradie ?? uhrPar.Poradie));
                   }
               }
               else
               {
                   var rzpZvysok = val;
                   int count = 1;
                   foreach (RzpDennikViewHelper predbCPRow in predbezneCP)
                   {
                       decimal alikvotnaHodnota;
                       if (predbezneCP.Count() == count)
                       {
                           alikvotnaHodnota = rzpZvysok;
                       }
                       else
                       {
                           count += 1;
                           alikvotnaHodnota = Math.Round(val * predbCPRow.Suma / rzpTotal, 2, MidpointRounding.AwayFromZero);
                           rzpZvysok = Math.Round(rzpZvysok - alikvotnaHodnota, 2, MidpointRounding.AwayFromZero);
                           alikvotnaHodnota *= (val >= 0 && alikvotnaHodnota < 0) || (val < 0 && alikvotnaHodnota >= 0) ? -1 : 1;
                       }
                       rzpDennikList.Add(CreateRzpDennikSingleRow(rzpPolozky, be, predbCPRow.C_Stredisko_Id ?? strediskoId, predbCPRow.C_Projekt_Id ?? projektId, banPol, uhrPar, null, predbCPRow, alikvotnaHodnota, banPol?.Poradie ?? uhrPar.Poradie));
                   }
               }
           }
           else
           {
               if (predkontPolPar.Count() > 0)
               {
                   VyberPodlaPriorityRzp(ref nevyhovujucePolozky, predkontPolPar, 2, uhrPar.Poradie, be.D_BiznisEntita_Id);

                   foreach (PredkontaciaRzpViewHelper def3 in predkontPolPar)
                   {
                       rzpDennikList.Add(CreateRzpDennikSingleRow(rzpPolozky, be, strediskoId, projektId, banPol, uhrPar, def3, null, val, banPol?.Poradie ?? uhrPar.Poradie));
                   }
               }
               else
               {
                   nevyhovujucePolozky.AddIfNotExists((be.D_BiznisEntita_Id, 1, uhrPar.Poradie));
                   //Pridám riadok ale bez predkontácie
                   rzpDennikList.Add(CreateRzpDennikSingleRow(rzpPolozky, be, strediskoId, projektId, banPol, uhrPar, null, null, val, banPol?.Poradie ?? uhrPar.Poradie));
               }
           }
       }

       private static void VyberPodlaPriorityRzp(ref List<(long D_BiznisEntita_Id, int Typ, int Poradie)> nevyhovujucePolozky, List<PredkontaciaRzpViewHelper> predkontSource, int typ, int poradie, long D_BiznisEntita_Id)
       {
           while (predkontSource.Count > 1)
           {
               if (predkontSource.Any(x => string.IsNullOrEmpty(x.DapRok) || x.DapRok == "N") && predkontSource.Any(x => !string.IsNullOrEmpty(x.DapRok) && x.DapRok != "N"))
               {
                   predkontSource.RemoveAll(x => string.IsNullOrEmpty(x.DapRok) || x.DapRok == "N");
                   continue;
               }

               if (predkontSource.Any(x => x.C_Druh_Id == null) && predkontSource.Any(x => x.C_Druh_Id != null))
               {
                   predkontSource.RemoveAll(x => x.C_Druh_Id == null);
                   continue;
               }

               if (predkontSource.Any(x => x.C_Kod_Id == null) && predkontSource.Any(x => x.C_Kod_Id != null))
               {
                   predkontSource.RemoveAll(x => x.C_Kod_Id == null);
                   continue;
               }

               if (predkontSource.Any(x => x.C_Odsek_Id == null) && predkontSource.Any(x => x.C_Odsek_Id != null))
               {
                   predkontSource.RemoveAll(x => x.C_Odsek_Id == null);
                   continue;
               }

               if (predkontSource.Any(x => x.D_Osoba_Id == null) && predkontSource.Any(x => x.D_Osoba_Id != null))
               {
                   predkontSource.RemoveAll(x => x.D_Osoba_Id == null);
                   continue;
               }

               if (predkontSource.Any(x => x.C_OsobaTyp_Id == null) && predkontSource.Any(x => x.C_OsobaTyp_Id != null))
               {
                   predkontSource.RemoveAll(x => x.C_OsobaTyp_Id == null);
                   continue;
               }

               if (predkontSource.Any(x => x.C_TypBiznisEntity_Kniha_Id == null) && predkontSource.Any(x => x.C_TypBiznisEntity_Kniha_Id != null))
               {
                   predkontSource.RemoveAll(x => x.C_TypBiznisEntity_Kniha_Id == null);
                   continue;
               }

               if (predkontSource.Count() > 1)
               {
                   nevyhovujucePolozky.AddIfNotExists((D_BiznisEntita_Id, typ, poradie));
               }

               break;
           }
       }

       private static UctDennik CreateUctDennikSingleRow(List<UctRozvrh> ucty,
                                                         BiznisEntitaView be,
                                                         DokladBANPolViewHelper banPol,
                                                         UhradaParovanieViewHelper uhrPar,
                                                         PredkontaciaUctViewHelper predkontRow,
                                                         short tbe,
                                                         int kniha,
                                                         int? strediskoId,
                                                         long? projektId,
                                                         long? osobaId,
                                                         string vs,
                                                         DateTime? splat,
                                                         decimal val,
                                                         int poradie,
                                                         bool otoceneZnamienko,
                                                         bool rozdiel,
                                                         bool rozdielIsZisk,
                                                         List<UctDennikViewHelper> uctDennikSdkFA,
                                                         //List<UctDennikViewHelper> uctDennikDap,
                                                         bool pridajNevyhovujuce,
                                                         ref List<(long D_BiznisEntita_Id, int Typ, int Poradie)> nevyhovujucePolozky, string explicitPopis)
       {
           bool md = true;
           bool hladajSdkUcet = (uhrPar?.C_Typ_Id == (int)TypEnum.UhradaOFA || uhrPar?.C_Typ_Id == (int)TypEnum.UhradaDFA ||
                                 uhrPar?.C_Typ_Id == (int)TypEnum.DobropisOFA || uhrPar?.C_Typ_Id == (int)TypEnum.DobropisDFA) && !rozdiel;
           bool SdkUcetNajdeny = false;

           if (predkontRow == null || (predkontRow.C_UctRozvrh_Id_MD == null && predkontRow.C_UctRozvrh_Id_Dal == null)) //navrhni stranu
           {
               if ((TypBiznisEntity_KnihaEnum)kniha == TypBiznisEntity_KnihaEnum.Prijmove_pokladnicne_doklady)
               {
                   md = uhrPar.C_Typ_Id == (int)TypEnum.UhradaDFA || uhrPar.C_Typ_Id == (int)TypEnum.UhradaDZF ||
                        uhrPar.C_Typ_Id == (int)TypEnum.DobropisDFA || uhrPar.C_Typ_Id == (int)TypEnum.ZalohyPoskytnute;
               }
               else if ((TypBiznisEntity_KnihaEnum)kniha == TypBiznisEntity_KnihaEnum.Vydajove_pokladnicne_doklady)
               {
                   md = uhrPar.C_Typ_Id != (int)TypEnum.UhradaOFA && uhrPar.C_Typ_Id != (int)TypEnum.UhradaOZF &&
                        uhrPar.C_Typ_Id != (int)TypEnum.DobropisOFA || uhrPar.C_Typ_Id == (int)TypEnum.ZalohyPrijate;
               }
               else if ((TypBiznisEntityEnum)tbe == TypBiznisEntityEnum.OFA)
               {
                   md = predkontRow.C_Typ_Id == (int)TypEnum.SumaDokladu || predkontRow.C_Typ_Id == (int)TypEnum.ZalohaVSZalohy;
               }
               else if ((TypBiznisEntityEnum)tbe == TypBiznisEntityEnum.DFA)
               {
                   md = predkontRow.C_Typ_Id != (int)TypEnum.SumaDokladu && predkontRow.C_Typ_Id != (int)TypEnum.ZalohaVSZalohy;
               }
               else if ((TypBiznisEntityEnum)tbe == TypBiznisEntityEnum.IND ||
                        (TypBiznisEntityEnum)tbe == TypBiznisEntityEnum.BAN)
               {
                   md = otoceneZnamienko;
               }
               else
               {
                   md = true;
               }

               if (rozdiel)
               {
                   md = !rozdielIsZisk;
               }
           }
           else
           {
               md = (predkontRow.C_UctRozvrh_Id_MD != null);
               //!rozdiel <alebo> predkontRow.C_Typ_Id != (int)TypEnum.CentVyrovnaniePreplatok && predkontRow.C_Typ_Id != (int)TypEnum.CentVyrovnanieNedoplatok
               if (banPol != null && !rozdiel)
               {
                   //Debetné položky BAN sa účtujú na MD kladnou hodnotou; na DAL zápornou hodnotou
                   //Kreditné položky BAN sa účtujú na MD zápornou hodnotou; na DAL kladnou hodnotou
                   if (md && !otoceneZnamienko || !md && otoceneZnamienko)
                   {
                       otoceneZnamienko = !otoceneZnamienko;
                       val *= (-1);
                   }
               }
           }

           UctRozvrh ucet = null;

           //Nepodporujeme zápis na obe strany
           if (ucty != null && predkontRow != null && (predkontRow.C_UctRozvrh_Id_MD != null || predkontRow.C_UctRozvrh_Id_Dal != null))
           {
               ucet = ucty.Where(x => x.C_UctRozvrh_Id == ((md) ? predkontRow.C_UctRozvrh_Id_MD : predkontRow.C_UctRozvrh_Id_Dal)).First();
           }

           UctDennikViewHelper uctDennikSdkFARow = null;
           if (hladajSdkUcet && uctDennikSdkFA != null && uctDennikSdkFA.Any())
           {
               //Hľadám SDK účet na opačnej strane a s rovnakým SÚ. Resp. ak účet v predkontácii nie je, tak akýkoľvek
               uctDennikSdkFARow = uctDennikSdkFA.Where(x => x.D_BiznisEntita_Id == uhrPar.D_BiznisEntita_Id_Predpis &&
                                                            (x.SU == ucet?.SU || ucet == null) &&
                                                            (md ? x.SumaDal : x.SumaMD) != 0
                                                       ).FirstOrDefault();

               if (uctDennikSdkFARow == null && ucet != null)
               {
                   //Nenašla sa presne tá istá SU, zoberiem hocijaku
                   uctDennikSdkFARow = uctDennikSdkFA.Where(x => x.D_BiznisEntita_Id == uhrPar.D_BiznisEntita_Id_Predpis &&
                                                                (md ? x.SumaDal : x.SumaMD) != 0
                                                           ).FirstOrDefault();
               }

               if (uctDennikSdkFARow == null)
               {
                   //Ak ide o PS doklad, tak idem hľadať cez VS, OBP, Suma a SDK účet
                   uctDennikSdkFARow = uctDennikSdkFA.Where(x => x.VS == uhrPar.VS && x.D_Osoba_Id == uhrPar.D_Osoba_Id && Math.Abs(x.SumaDal + x.SumaMD) == Math.Abs(uhrPar.DM_Cena + uhrPar.DM_Rozdiel) &&
                                                                (x.SU == ucet?.SU || ucet == null) &&
                                                                (md ? x.SumaDal : x.SumaMD) != 0
                                                           ).FirstOrDefault();
               }

               if (uctDennikSdkFARow == null && ucet != null)
               {
                   //Nenašla sa presne tá istá SU, zoberiem hocijaku
                   uctDennikSdkFARow = uctDennikSdkFA.Where(x => x.VS == uhrPar.VS && x.D_Osoba_Id == uhrPar.D_Osoba_Id && Math.Abs(x.SumaDal + x.SumaMD) == Math.Abs(uhrPar.DM_Cena + uhrPar.DM_Rozdiel) &&
                                                                (md ? x.SumaDal : x.SumaMD) != 0
                                                           ).FirstOrDefault();
               }


               if (uctDennikSdkFARow != null)
               {
                   //NAŠIEL SOM - zmením účet na ten z FA
                   ucet = ucty.Where(x => x.C_UctRozvrh_Id == uctDennikSdkFARow.C_UctRozvrh_Id).First();
                   SdkUcetNajdeny = true;
               }
           }

           //UctDennikViewHelper uctDennikDapRow = null;
           //if (uctDennikDap != null && uctDennikDap.Any())
           //{
           //    //Hľadám SDK účet na opačnej strane a s rovnakým SÚ. Resp. ak účet v predkontácii nie je, tak akýkoľvek
           //    uctDennikDapRow = uctDennikDap.Where(x => x.D_VymerPol_Id == uhrPar.D_VymerPol_Id &&
           //                                                 (x.SU == ucet?.SU || ucet == null) &&
           //                                                 (md ? x.SumaDal : x.SumaMD) != 0
           //                                            ).FirstOrDefault();

           //    if (uctDennikDapRow == null && ucet != null)
           //    {
           //        //Nenašla sa presne tá istá SU, zoberiem hocijaku
           //        uctDennikDapRow = uctDennikDap.Where(x => x.D_VymerPol_Id == uhrPar.D_VymerPol_Id &&
           //                                                     (md ? x.SumaDal : x.SumaMD) != 0
           //                                                ).FirstOrDefault();
           //    }

           //    if (uctDennikDapRow != null)
           //    {
           //        //NAŠIEL SOM - zmením účet na ten z ID-DaP
           //        ucet = ucty.Where(x => x.C_UctRozvrh_Id == uctDennikDapRow.C_UctRozvrh_Id).First();
           //    }
           //}

           if (predkontRow != null)
           {
               val = Math.Round((decimal)(val * predkontRow.Percento / 100), 2, MidpointRounding.AwayFromZero);
           }

           if (hladajSdkUcet && SdkUcetNajdeny)
           {
               //Je to v poriadku
               //Nie je dôležité či som mal predkontačný riadok. Ak som aj nemal a účet sa potiahne z FA - nevypisuj chybu lebo je to OK
           }
           else if (hladajSdkUcet && !SdkUcetNajdeny)
           {
               //Ide o úhradový riadok a nenašiel som SDK účet - treba to oznámiť, aj keď možno v predkontácii úhrady nejaký je
               nevyhovujucePolozky.AddIfNotExists((be.D_BiznisEntita_Id, 5, uhrPar.Poradie));
           }
           else if (pridajNevyhovujuce && ucet == null)
           {
               nevyhovujucePolozky.AddIfNotExists((be.D_BiznisEntita_Id, rozdiel ? 4 : 1, uhrPar?.Poradie ?? banPol?.Poradie ?? 0)); //Iba WARNING - záznam bude vygenerovaný ale bez uctu
           }

           return new UctDennik()
           {
               D_BiznisEntita_Id = be.D_BiznisEntita_Id,
               Rok = be.Rok,
               C_UctRozvrh_Id = ucet?.C_UctRozvrh_Id,
               SumaMD = (md) ? val : 0,
               SumaDal = (!md) ? val : 0,
               C_Stredisko_Id = (ucet?.VyzadovatStredisko ?? true) ? (uctDennikSdkFARow?.C_Stredisko_Id ?? uhrPar?.C_Stredisko_Id ?? banPol?.C_Stredisko_Id ?? strediskoId) : null,
               C_Projekt_Id = (ucet?.VyzadovatProjekt ?? true) ? (uctDennikSdkFARow?.C_Projekt_Id ?? uhrPar?.C_Projekt_Id ?? banPol?.C_Projekt_Id ?? projektId) : null,
               D_Osoba_Id = (ucet?.SDK == "P" || ucet?.SDK == "Z") || ucet == null ? (uhrPar?.D_Osoba_Id ?? osobaId) : null,
               VS = (ucet?.SDK == "P" || ucet?.SDK == "Z") || ucet == null ? (uhrPar?.VS ?? banPol?.VS ?? vs) : null, //VS úhrady, VS položky BAN, VS z hlavičky dokladu
               DatumSplatnosti = (ucet?.SDK == "P" && md || ucet?.SDK == "Z" && !md) || ucet == null ? splat : null, //Ak sa to náhodou stane, tak dám "Dátum dokladu"
               Poradie = poradie,
               DatumUctovania = be.DatumDokladu,

               C_UctKluc_Id1 = (ucet?.VyzadovatUctKluc1 ?? true) ? (uctDennikSdkFARow?.C_UctKluc_Id1 ?? uhrPar?.C_UctKluc_Id1 ?? banPol?.C_UctKluc_Id1) : null,
               C_UctKluc_Id2 = (ucet?.VyzadovatUctKluc2 ?? true) ? (uctDennikSdkFARow?.C_UctKluc_Id2 ?? uhrPar?.C_UctKluc_Id2 ?? banPol?.C_UctKluc_Id2) : null,
               C_UctKluc_Id3 = (ucet?.VyzadovatUctKluc3 ?? true) ? (uctDennikSdkFARow?.C_UctKluc_Id3 ?? uhrPar?.C_UctKluc_Id3 ?? banPol?.C_UctKluc_Id3) : null,
               Popis = explicitPopis ?? uhrPar?.Popis ?? banPol?.Popis ?? predkontRow?.Nazov ?? be.Popis,
               D_DokladBANPol_Id = uhrPar?.D_DokladBANPol_Id ?? banPol?.D_DokladBANPol_Id,
               D_UhradaParovanie_Id = uhrPar?.D_UhradaParovanie_Id
           };
       }

       private static RzpDennik CreateRzpDennikSingleRow(List<RzpPol> rzpPolozky, BiznisEntitaView be, int? strediskoId, long? projektId,
           DokladBANPolViewHelper banPol, UhradaParovanieViewHelper uhrPar, PredkontaciaRzpViewHelper predkontRow, RzpDennikViewHelper predbezneCPRow, decimal val, int poradie)
       {
           var pol = rzpPolozky.Where(x => x.C_RzpPol_Id == (predkontRow?.C_RzpPol_Id ?? predbezneCPRow?.C_RzpPol_Id)).FirstOrDefault();

           if (predkontRow != null && predbezneCPRow == null)
           {
               val = Math.Round((decimal)(val * predkontRow.Percento / 100), 2, MidpointRounding.AwayFromZero);
           }

           if (banPol != null && predkontRow != null && uhrPar == null)
           {
               if (banPol.Suma < 0 && predkontRow.PrijemVydaj == 1 ||   //Debetná položka výpisu na príjmovú rzp. položku
                   banPol.Suma > 0 && predkontRow.PrijemVydaj == 2)     //Kreditná položka výpisu na výdajovú rzp. položku
               {
                   val *= (-1);
               }
           }

           return new RzpDennik()
           {
               D_BiznisEntita_Id = be.D_BiznisEntita_Id,
               D_DokladBANPol_Id = banPol?.D_DokladBANPol_Id,
               D_UhradaParovanie_Id = uhrPar?.D_UhradaParovanie_Id,
               Rok = be.Rok,
               C_RzpPol_Id = predbezneCPRow?.C_RzpPol_Id ?? predkontRow?.C_RzpPol_Id,
               D_Program_Id = predbezneCPRow?.D_Program_Id ?? predkontRow?.D_Program_Id,
               Suma = val,
               Pocet = 1, //Kým nemáme evidenciu položiek a v nich atribút "Počet", tak dávam 1.
               C_Stredisko_Id = (pol?.Stredisko ?? true) ? (predbezneCPRow?.C_Stredisko_Id ?? uhrPar?.C_Stredisko_Id ?? banPol?.C_Stredisko_Id ?? strediskoId) : null,
               C_Projekt_Id = (pol?.Projekt ?? true) ? (predbezneCPRow?.C_Projekt_Id ?? uhrPar?.C_Projekt_Id ?? banPol?.C_Projekt_Id ?? projektId) : null,
               Poradie = poradie,
               Popis = predbezneCPRow?.Popis ?? uhrPar?.Popis ?? banPol?.Popis ?? predkontRow?.Nazov ?? be.Popis
           };
       }

       private List<(long D_BiznisEntita_Id, string Chyba)> SkontrolovatZauctovanieDokladu(short idTBE, List<BiznisEntita> doklady, int idNewState, bool rzpZauctovanie, bool rzpOductovanie, bool uctZauctovanie, bool uctOductovanie, string processKey, out string reportId)
       {
           var chybneDoklady = new List<(long D_BiznisEntita_Id, string Chyba)>();
           var eSAMStart = GetNastavenieD("reg", "eSAMStart");
           reportId = null;

           if (uctZauctovanie)
           {
               LongOperationSetStateMessage(processKey, "Prebieha kontrola účtovania v účtovnom denníku");
               var uctovneDenniky = GetList(Db.From<UctDennik>().Where(x => Sql.In(x.D_BiznisEntita_Id, doklady.Select(x => x.D_BiznisEntita_Id))).And(Filter.NotDeleted().ToString()));
               var uctovneRozvrhy = GetList(Db.From<UctRozvrh>().Where(x => Sql.In(x.C_UctRozvrh_Id, uctovneDenniky.Select(x => x.C_UctRozvrh_Id).Distinct())).And(Filter.NotDeleted().ToString()));
               var strediska = GetList(Db.From<StrediskoCis>().Where(x => Sql.In(x.C_Stredisko_Id, uctovneDenniky.Select(x => x.C_Stredisko_Id).Distinct())).And(Filter.NotDeleted().ToString()));

               //CHECK-51
               foreach (var bezStr in uctovneDenniky
                   .Where(x => !x.C_Stredisko_Id.HasValue && uctovneRozvrhy.SingleOrDefault(z => x.C_UctRozvrh_Id == z.C_UctRozvrh_Id)?.VyzadovatStredisko == true)
                   .GroupBy(x => x.D_BiznisEntita_Id))
               {
                   chybneDoklady.Add((bezStr.Key, $"Záznamy účtovného denníka (pč: { bezStr.Select(x => x.Poradie).Join(", ")}) nemajú vyplnené stredisko pri použitom strediskovom účte"));
               }

               //CHECK-52
               foreach (var bezPrj in uctovneDenniky
                   .Where(x => !x.C_Projekt_Id.HasValue && uctovneRozvrhy.SingleOrDefault(z => x.C_UctRozvrh_Id == z.C_UctRozvrh_Id)?.VyzadovatProjekt == true)
                   .GroupBy(x => x.D_BiznisEntita_Id))
               {
                   chybneDoklady.Add((bezPrj.Key, $"Záznamy účtovného denníka (pč: { bezPrj.Select(x => x.Poradie).Join(", ")}) nemajú vyplnený projekt pri použitom projektovom účte"));
               }

               foreach (var bezUctKluc1 in uctovneDenniky
                   .Where(x => !x.C_UctKluc_Id1.HasValue && uctovneRozvrhy.SingleOrDefault(z => x.C_UctRozvrh_Id == z.C_UctRozvrh_Id)?.VyzadovatUctKluc1 == true)
                   .GroupBy(x => x.D_BiznisEntita_Id))
               {
                   var klucS = GetNastavenieS("uct", "UctKluc1Nazov");
                   chybneDoklady.Add((bezUctKluc1.Key, $"Záznamy účtovného denníka (pč: { bezUctKluc1.Select(x => x.Poradie).Join(", ")}) nemajú vyplnené '{klucS}' pri použitom účte"));
               }

               foreach (var bezUctKluc2 in uctovneDenniky
                   .Where(x => !x.C_UctKluc_Id2.HasValue && uctovneRozvrhy.SingleOrDefault(z => x.C_UctRozvrh_Id == z.C_UctRozvrh_Id)?.VyzadovatUctKluc2 == true)
                   .GroupBy(x => x.D_BiznisEntita_Id))
               {
                   var klucS = GetNastavenieS("uct", "UctKluc2Nazov");
                   chybneDoklady.Add((bezUctKluc2.Key, $"Záznamy účtovného denníka (pč: { bezUctKluc2.Select(x => x.Poradie).Join(", ")}) nemajú vyplnené '{klucS}' pri použitom účte"));
               }

               foreach (var bezUctKluc3 in uctovneDenniky
                   .Where(x => !x.C_UctKluc_Id3.HasValue && uctovneRozvrhy.SingleOrDefault(z => x.C_UctRozvrh_Id == z.C_UctRozvrh_Id)?.VyzadovatUctKluc3 == true)
                   .GroupBy(x => x.D_BiznisEntita_Id))
               {
                   var klucS = GetNastavenieS("uct", "UctKluc3Nazov");
                   chybneDoklady.Add((bezUctKluc3.Key, $"Záznamy účtovného denníka (pč: { bezUctKluc3.Select(x => x.Poradie).Join(", ")}) nemajú vyplnené '{klucS}' pri použitom účte"));
               }

               //CHECK-82
               foreach (var uctDenNulove in uctovneDenniky
                   .Where(x => x.SumaMD == 0 && x.SumaDal == 0)
                   .GroupBy(x => x.D_BiznisEntita_Id))
               {
                   chybneDoklady.Add((uctDenNulove.Key, $"Záznamy účtovného denníka (pč: { uctDenNulove.Select(x => x.Poradie).Join(", ")}) majú nulovú stranu Má dať aj Dal"));
               }

               //CHECK-83
               foreach (var syntUcet in uctovneDenniky
                   .Where(x => uctovneRozvrhy.SingleOrDefault(z => x.C_UctRozvrh_Id == z.C_UctRozvrh_Id)?.Ucet?.Length == 3)
                   .GroupBy(x => x.D_BiznisEntita_Id))
               {
                   chybneDoklady.Add((syntUcet.Key, $"Na syntetický účet nie je možné účtovať - je potrebné navoliť analytický účet (pč: { syntUcet.Select(x => x.Poradie).Join(", ")})"));
               }

               //CHECK-84
               foreach (var bezUctu in uctovneDenniky
                   .Where(x => !x.C_UctRozvrh_Id.HasValue)
                   .GroupBy(x => x.D_BiznisEntita_Id))
               {
                   chybneDoklady.Add((bezUctu.Key, $"Nie je zadaný účet (pč: { bezUctu.Select(x => x.Poradie).Join(", ")})"));
               }

               //CHECK-88
               foreach (var MdDalRozdiel in uctovneDenniky
                   .GroupBy(x => x.D_BiznisEntita_Id)
                   .Select(
                   g => new
                   {
                       g.Key,
                       Rozdiel = g.Sum(x => x.SumaMD) - g.Sum(x => x.SumaDal)
                   })
                   .Where(x => x.Rozdiel != 0)
                   )
               {
                   chybneDoklady.Add((MdDalRozdiel.Key, $"Strana 'Má dať' sa nerovná strane 'Dal' (Rozdiel: { MdDalRozdiel.Rozdiel })"));
               }

               //CHECK-89, CHECK-90
               foreach (var sdkRiadok in uctovneDenniky
                   .Where(x => uctovneRozvrhy.SingleOrDefault(z => x.C_UctRozvrh_Id == z.C_UctRozvrh_Id)?.SDK != null)
                   .GroupBy(x => new { x.D_BiznisEntita_Id, uctovneRozvrhy.Single(z => x.C_UctRozvrh_Id == z.C_UctRozvrh_Id).SDK }))
               {
                   var dokl = doklady.FirstOrDefault(x => x.D_BiznisEntita_Id == sdkRiadok.Key.D_BiznisEntita_Id);
                   bool checkSdk = !dokl.PS && (eSAMStart == null || eSAMStart <= dokl.DatumDokladu);

                   if (checkSdk)
                   {
                       var stranaVznikuBezVS = sdkRiadok.Where(x => (sdkRiadok.Key.SDK == "P" && x.SumaMD != 0 && string.IsNullOrEmpty(x.VS)) || (sdkRiadok.Key.SDK == "Z" && x.SumaDal != 0 && string.IsNullOrEmpty(x.VS)));
                       var stranaVznikuBezOsoba = sdkRiadok.Where(x => (sdkRiadok.Key.SDK == "P" && x.SumaMD != 0 && !x.D_Osoba_Id.HasValue) || (sdkRiadok.Key.SDK == "Z" && x.SumaDal != 0 && !x.D_Osoba_Id.HasValue));
                       var stranaVznikuBezDatSpl = sdkRiadok.Where(x => (sdkRiadok.Key.SDK == "P" && x.SumaMD != 0 && !x.DatumSplatnosti.HasValue) || (sdkRiadok.Key.SDK == "Z" && x.SumaDal != 0 && !x.DatumSplatnosti.HasValue));

                       if (stranaVznikuBezVS.Any())
                       {
                           chybneDoklady.Add((sdkRiadok.Key.D_BiznisEntita_Id, $"Doklad obsahuje saldokontné zápisy bez vyplneného variabilného symbolu (pč: { stranaVznikuBezVS.Select(x => x.Poradie).Join(", ")})"));
                       }

                       if (stranaVznikuBezOsoba.Any())
                       {
                           chybneDoklady.Add((sdkRiadok.Key.D_BiznisEntita_Id, $"Doklad obsahuje saldokontné zápisy bez vyplnenej osoby (pč: { stranaVznikuBezOsoba.Select(x => x.Poradie).Join(", ")})"));
                       }

                       if (stranaVznikuBezDatSpl.Any())
                       {
                           chybneDoklady.Add((sdkRiadok.Key.D_BiznisEntita_Id, $"Doklad obsahuje saldokontné zápisy bez vyplneného dátumu splatnosti (pč: { stranaVznikuBezDatSpl.Select(x => x.Poradie).Join(", ")})"));
                       }

                       var stranaUhradyBezVS = sdkRiadok.Where(x => (sdkRiadok.Key.SDK == "P" && x.SumaDal != 0 && string.IsNullOrEmpty(x.VS)) || (sdkRiadok.Key.SDK == "Z" && x.SumaMD != 0 && string.IsNullOrEmpty(x.VS)));
                       var stranaUhradyBezOsoba = sdkRiadok.Where(x => (sdkRiadok.Key.SDK == "P" && x.SumaDal != 0 && !x.D_Osoba_Id.HasValue) || (sdkRiadok.Key.SDK == "Z" && x.SumaMD != 0 && !x.D_Osoba_Id.HasValue));

                       if (stranaUhradyBezVS.Any())
                       {
                           chybneDoklady.Add((sdkRiadok.Key.D_BiznisEntita_Id, $"Doklad obsahuje saldokontné zápisy bez vyplneného variabilného symbolu (pč: { stranaUhradyBezVS.Select(x => x.Poradie).Join(", ")})"));
                       }

                       if (stranaUhradyBezOsoba.Any())
                       {
                           chybneDoklady.Add((sdkRiadok.Key.D_BiznisEntita_Id, $"Doklad obsahuje saldokontné zápisy bez vyplnenej osoby (pč: { stranaUhradyBezOsoba.Select(x => x.Poradie).Join(", ")})"));
                       }
                   }
               }

               //CHECK-94
               foreach (var uctDenPlatnost in uctovneDenniky
                   .Where(x =>
                   uctovneRozvrhy.SingleOrDefault(z => x.C_UctRozvrh_Id == z.C_UctRozvrh_Id) != null &&
                   (uctovneRozvrhy.Single(z => x.C_UctRozvrh_Id == z.C_UctRozvrh_Id).PlatnostOd > doklady.Single(d => d.D_BiznisEntita_Id == x.D_BiznisEntita_Id).DatumDokladu ||
                   uctovneRozvrhy.Single(z => x.C_UctRozvrh_Id == z.C_UctRozvrh_Id).PlatnostDo < doklady.Single(d => d.D_BiznisEntita_Id == x.D_BiznisEntita_Id).DatumDokladu))
                   .GroupBy(x => x.D_BiznisEntita_Id))
               {
                   chybneDoklady.Add((uctDenPlatnost.Key, $"Záznamy účtovného denníka (pč: { uctDenPlatnost.Select(x => x.Poradie).Join(", ")}) majú použité účty, ktoré nie su platné k dátumu dokladu."));
               }

               //CHECK-96
               foreach (var pol in uctovneDenniky
                   .Where(x => x.C_Stredisko_Id.HasValue &&
                          uctovneRozvrhy.SingleOrDefault(u => x.C_UctRozvrh_Id == u.C_UctRozvrh_Id)?.PodnCinn == true &&
                          strediska.SingleOrDefault(s => x.C_Stredisko_Id == s.C_Stredisko_Id)?.PodnCinn == false)
                   .GroupBy(x => x.D_BiznisEntita_Id))
               {
                   chybneDoklady.Add((pol.Key, $"Záznamy účtovného denníka (pč: { pol.Select(x => x.Poradie).Join(", ")}) majú zvolené nepodnikateľské stredisko pri použitom podnikateľskom účte."));
               }

               foreach (var dokl in doklady)
               {
                   //CHECK-81
                   if ((dokl.C_TypBiznisEntity_Id == (int)TypBiznisEntityEnum.IND && !uctovneDenniky.Any(x => x.D_BiznisEntita_Id == dokl.D_BiznisEntita_Id)) ||
                       (dokl.C_TypBiznisEntity_Id != (int)TypBiznisEntityEnum.IND && !uctovneDenniky.Any(x => x.D_BiznisEntita_Id == dokl.D_BiznisEntita_Id) && !dokl.PS && dokl.DM_Suma != 0))
                   {
                       chybneDoklady.Add((dokl.D_BiznisEntita_Id, $"Doklad nie je možné zaúčtovať, pretože denník neobsahuje žiadne záznamy"));
                   }

                   if (dokl.C_TypBiznisEntity_Id != (int)TypBiznisEntityEnum.IND && uctovneDenniky.Any(x => x.D_BiznisEntita_Id == dokl.D_BiznisEntita_Id) && dokl.PS)
                   {
                       chybneDoklady.Add((dokl.D_BiznisEntita_Id, $"Doklad „počiatočného stavu“ nie je možné zaúčtovať, pretože denník obsahuje záznamy"));
                   }

                   //CHECK-85
                   var uctDenVymerPol = uctovneDenniky.Where(x => x.D_BiznisEntita_Id == dokl.D_BiznisEntita_Id && x.D_VymerPol_Id.HasValue);
                   if (dokl.C_TypBiznisEntity_Kniha_Id == (int)TypBiznisEntity_KnihaEnum.Externe_doklady_DaP && uctDenVymerPol.Any())
                   {
                       var vymerPolozky = GetList(Db
                           .From<ServiceModel.Office.Types.Dap.VymerPolViewHelper>()
                           .Where(x => Sql.In(x.D_VymerPol_Id, uctDenVymerPol.Select(x => x.D_VymerPol_Id)))
                           .Select(x => new { x.D_VymerPol_Id, x.ZauctovanieSuma, x.Suma }));

                       var chybnePolSuma = vymerPolozky.Where(x => uctDenVymerPol.Single(z => x.D_VymerPol_Id == z.D_VymerPol_Id).SumaMD != x.Suma - x.ZauctovanieSuma.GetValueOrDefault());
                       if (chybnePolSuma.Any())
                       {
                           chybneDoklady.Add((dokl.D_BiznisEntita_Id, $"Suma 'Má dať' v účtovnom denníku (pč: { uctDenVymerPol.Where(x => chybnePolSuma.Any(z => z.D_VymerPol_Id == x.D_VymerPol_Id)).Select(x => x.Poradie).Join(", ")}) nezodpovedá sume položky rozhodnutia (skontrolujte hodnotu položky rozhodnutia a tiež prípadné duplicitné zaúčtovanie v inom ID-DaP)."));
                       }
                   }


                   if (dokl.C_TypBiznisEntity_Kniha_Id == (int)TypBiznisEntity_KnihaEnum.Externe_doklady_DaP)
                   {
                       //CHECK-86
                       var uctDenDanVynos = uctovneDenniky.Where(x => x.D_BiznisEntita_Id == dokl.D_BiznisEntita_Id && x.SumaMD != 0 &&
                                                                     (x.C_Typ_Id == (int)TypEnum.DanovyVynosDAN ||
                                                                      x.C_Typ_Id == (int)TypEnum.DanovyVynosONE ||
                                                                      x.C_Typ_Id == (int)TypEnum.DanovyVynosPEN ||
                                                                      x.C_Typ_Id == (int)TypEnum.DanovyVynosPOK ||
                                                                      x.C_Typ_Id == (int)TypEnum.DanovyVynosDOD ||
                                                                      x.C_Typ_Id == (int)TypEnum.DanovyVynosURO
                                                                      ) //x.C_Typ_Id == (int)TypEnum.DanovyVynosODP - dvojaké účtovanie, preto nekontrolujem
                                                                );
                       if (uctDenDanVynos.Any())
                       {
                           var typy = GetList(Db.From<TypView>().Where(x => Sql.In(x.C_Typ_Id, uctDenDanVynos.Select(z => z.C_Typ_Id).Distinct())));
                           foreach (var typGrp in uctDenDanVynos.GroupBy(x => x.C_Typ_Id))
                           {
                               chybneDoklady.Add((dokl.D_BiznisEntita_Id, $"Pre typ '{typy.Single(x => x.C_Typ_Id == typGrp.Key).Nazov}' musí byť suma 'Má dať' rovná nule (pč: { typGrp.Select(x => x.Poradie).Join(", ")})"));
                           }
                       }

                       //CHECK-87
                       var uctDenDalCheck = uctovneDenniky.Where(x => x.D_BiznisEntita_Id == dokl.D_BiznisEntita_Id && x.SumaDal != 0 &&
                                                                     (x.C_Typ_Id == (int)TypEnum.DAN_Dan ||
                                                                      x.C_Typ_Id == (int)TypEnum.ONE_PokutaZaOneskorenie ||
                                                                      x.C_Typ_Id == (int)TypEnum.PEN_UrokZOmeskania ||
                                                                      x.C_Typ_Id == (int)TypEnum.POK_Pokuta ||
                                                                      x.C_Typ_Id == (int)TypEnum.DOD_PokutaZaDodatocnePodanie ||
                                                                      x.C_Typ_Id == (int)TypEnum.URO_UrokZOdlozeniaSplatok
                                                                      ) //x.C_Typ_Id == (int)TypEnum.ODP_OdpisPohladavky - dvojaké účtovanie, preto nekontrolujem
                                                                 );
                       if (uctDenDalCheck.Any())
                       {
                           var typy = GetList(Db.From<TypView>().Where(x => Sql.In(x.C_Typ_Id, uctDenDalCheck.Select(z => z.C_Typ_Id).Distinct())));
                           foreach (var typGrp in uctDenDalCheck.GroupBy(x => x.C_Typ_Id))
                           {
                               chybneDoklady.Add((dokl.D_BiznisEntita_Id, $"Pre typ '{typy.Single(x => x.C_Typ_Id == typGrp.Key).Nazov}' musí byť suma 'Dal' rovná nule (pč: { typGrp.Select(x => x.Poradie).Join(", ")})"));
                           }
                       }
                   }
               }
           }

           if (rzpZauctovanie)
           {
               LongOperationSetStateMessage(processKey, "Prebieha kontrola účtovania v rozpočtovom denníku");
               var rozpoctovyDennik = GetList(Db.From<RzpDennik>().Where(x => Sql.In(x.D_BiznisEntita_Id, doklady.Select(x => x.D_BiznisEntita_Id))).And(Filter.NotDeleted().ToString()));
               var rozpoctovePolozky = GetList(Db.From<RzpPol>().Where(x => Sql.In(x.C_RzpPol_Id, rozpoctovyDennik.Select(x => x.C_RzpPol_Id).Distinct())).And(Filter.NotDeleted().ToString()));
               var rzpComb = Db.Select<(long C_RzpPol_Id, long? D_Program_Id, int Rok)>("SELECT DISTINCT C_RzpPol_Id, D_Program_Id, Rok FROM rzp.V_RzpCombinations cmb WHERE cmb.C_RzpPol_Id IN (@zrpPolId) AND cmb.Rok IN (@rok)",
                                     new { zrpPolId = rozpoctovePolozky.Select(z => z.C_RzpPol_Id).Distinct(), rok = doklady.Select(x => x.Rok) });

               //CHECK-71
               foreach (var bezRzpPol in rozpoctovyDennik.Where(x => !x.C_RzpPol_Id.HasValue).GroupBy(x => x.D_BiznisEntita_Id))
               {
                   chybneDoklady.Add((bezRzpPol.Key, $"Na záznamoch (pč: {bezRzpPol.Select(x => x.Poradie).Join(", ")}) nie je vyplnená rozpočtová položka"));
               }

               //CHECK-72
               if (GetNastavenieB("rzp", "VydProgrRzp"))
               {
                   foreach (var bezPrg in rozpoctovyDennik
                   .Where(x => !x.D_Program_Id.HasValue && rozpoctovePolozky.SingleOrDefault(z => x.C_RzpPol_Id == z.C_RzpPol_Id)?.PrijemVydaj == 2)
                   .GroupBy(x => x.D_BiznisEntita_Id))
                   {
                       chybneDoklady.Add((bezPrg.Key, $"Rozpočtový denník obsahuje záznamy výdaja rozpočtu (pč: { bezPrg.Select(x => x.Poradie).Join(", ")}), ktoré nemajú vyplnený program"));
                   }
               }

               //CHECK-51
               foreach (var bezStr in rozpoctovyDennik
                   .Where(x => !x.C_Stredisko_Id.HasValue && rozpoctovePolozky.SingleOrDefault(z => x.C_RzpPol_Id == z.C_RzpPol_Id)?.Stredisko == true)
                   .GroupBy(x => x.D_BiznisEntita_Id))
               {
                   chybneDoklady.Add((bezStr.Key, $"Záznamy rozpočtového denníka (pč: { bezStr.Select(x => x.Poradie).Join(", ")}) nemajú vyplnené stredisko, pri použitej strediskovej rozpočtovej položke"));
               }

               //CHECK-52
               foreach (var bezPrj in rozpoctovyDennik
                   .Where(x => !x.C_Projekt_Id.HasValue && rozpoctovePolozky.SingleOrDefault(z => x.C_RzpPol_Id == z.C_RzpPol_Id)?.Projekt == true)
                   .GroupBy(x => x.D_BiznisEntita_Id))
               {
                   chybneDoklady.Add((bezPrj.Key, $"Záznamy rozpočtového denníka (pč: { bezPrj.Select(x => x.Poradie).Join(", ")}) nemajú vyplnený projekt, pri použitej projektovej rozpočtovej položke"));
               }

               //CHECK-73A
               foreach (var rzpDenGrp in rozpoctovyDennik.Where(x => x.C_RzpPol_Id != null).GroupBy(x => x.D_BiznisEntita_Id))
               {
                   var doklad = doklady.Single(z => z.D_BiznisEntita_Id == rzpDenGrp.Key);
                   var chybPrijPol = new List<int>();
                   var chybVydPol = new List<int>();

                   foreach (var rzpDen in rzpDenGrp)
                   {
                       var rzpPol = rozpoctovePolozky.Single(z => rzpDen.C_RzpPol_Id == z.C_RzpPol_Id);
                       if (rzpPol.PrijemVydaj == 1)
                       {
                           if (!rzpComb.Any(x => x.Rok == doklad.Rok && x.C_RzpPol_Id == rzpDen.C_RzpPol_Id))
                           {
                               chybPrijPol.Add(rzpDen.Poradie);
                           }
                       }
                       else if (rzpDen.D_Program_Id != null) //Nevyplnený program na výdajovom riadku sa rieši vyššie a tu už riešim iba s vyplneným
                       {
                           if (!rzpComb.Any(x => x.Rok == doklad.Rok && x.D_Program_Id == rzpDen.D_Program_Id && x.C_RzpPol_Id == rzpDen.C_RzpPol_Id))
                           {
                               var be = doklady.Single(d => d.D_BiznisEntita_Id == rzpDen.D_BiznisEntita_Id);
                               //PS doklady za minulé roky nemusia byť v zozname kombinácií
                               if (!be.PS || eSAMStart == null || be.Rok >= eSAMStart.Value.Year)
                               {
                                   chybVydPol.Add(rzpDen.Poradie);
                               }
                           }
                       }
                   }

                   if (chybPrijPol.Any())
                   {
                       chybneDoklady.Add((rzpDenGrp.Key, $"Príjmové rzp. položky (pč: { chybPrijPol.Join(", ")}) sa nenachádzajú v aktuálnom rozpočte"));
                   }

                   if (chybVydPol.Any())
                   {
                       chybneDoklady.Add((rzpDenGrp.Key, $"Kombinácia výdajovej rzp. položky a programu (pč: { chybVydPol.Join(", ")}) sa nenachádza v aktuálnom rozpočte"));
                   }

               }

               //CHECK-76
               if (idTBE == (short)TypBiznisEntityEnum.BAN || idTBE == (short)TypBiznisEntityEnum.PDK)
               {
                   foreach (var be in doklady)
                   {
                       var chybPol = new List<string>();

                       void SkontrolujZauctovaniePolozky(TypBiznisEntity_KnihaEnum tbek, IEnumerable<RzpDennik> rzpDen, decimal hodnotaPol, string popis)
                       {
                           if (rzpDen.Any())
                           {
                               decimal rzpSuma = 0;
                               foreach (var rzpDenRow in rzpDen)
                               {
                                   if (rzpDenRow.C_RzpPol_Id != null)
                                   {
                                       var rzpPol = rozpoctovePolozky.SingleOrDefault(z => rzpDenRow.C_RzpPol_Id == z.C_RzpPol_Id);
                                       rzpSuma += (rzpPol.PrijemVydaj == 1 ? 1 : -1) * rzpDenRow.Suma;
                                   }
                               }

                               if (tbek == TypBiznisEntity_KnihaEnum.Vydajove_pokladnicne_doklady)
                               {
                                   rzpSuma *= (-1);
                               }

                               if (hodnotaPol != rzpSuma && rzpSuma != 0)
                               {
                                   chybPol.Add($" - {popis}\r\n\t- hodnota na zaúčtovanie: { hodnotaPol }\r\n\t- zaúčtovaná hodnota: {rzpSuma}\r\n\t- rozdiel: {hodnotaPol - rzpSuma}");
                               }
                           }
                           else
                           {
                               if (!be.PS)
                               {
                                   chybPol.Add($" - {popis} nie je zaúčtovaný");
                               }
                           }
                       }

                       var uhradaParovanie = Db.Select(Db.From<UhradaParovanieViewHelper>().Where(x => x.D_BiznisEntita_Id_Uhrada == be.D_BiznisEntita_Id)
                                                                                           .Select(x => new { x.D_UhradaParovanie_Id, x.C_TypBiznisEntity_Kniha_Id, x.RzpDefinicia, x.DM_Cena, x.DM_Rozdiel, x.Poradie }));
                       if (uhradaParovanie.Any())
                       {
                           foreach (var uhr in uhradaParovanie.Where(x => x.RzpDefinicia != -1).OrderBy(x => x.Poradie))
                           {
                               IEnumerable<RzpDennik> rzpDenForUhr = rozpoctovyDennik.Where(x => x.D_UhradaParovanie_Id == uhr.D_UhradaParovanie_Id);
                               SkontrolujZauctovaniePolozky((TypBiznisEntity_KnihaEnum)uhr.C_TypBiznisEntity_Kniha_Id, rzpDenForUhr, uhr.DM_Cena + uhr.DM_Rozdiel, $"Riadok párovania úhrad {uhr.Poradie}");
                           }
                       }

                       if (idTBE == (short)TypBiznisEntityEnum.BAN)
                       {
                           var banPolozka = Db.Select(Db.From<DokladBANPolViewHelper>().Where(x => x.D_BiznisEntita_Id == be.D_BiznisEntita_Id)
                                                                                       .Select(x => new { x.D_DokladBANPol_Id, x.C_TypBiznisEntity_Kniha_Id, x.RzpDefinicia, x.Suma, x.Poradie }));
                           if (banPolozka.Any())
                           {
                               foreach (var banPol in banPolozka.Where(x => x.RzpDefinicia != -1).OrderBy(x => x.Poradie))
                               {
                                   IEnumerable<RzpDennik> rzpDenForBanPol = rozpoctovyDennik.Where(x => x.D_DokladBANPol_Id == banPol.D_DokladBANPol_Id);
                                   SkontrolujZauctovaniePolozky((TypBiznisEntity_KnihaEnum)banPol.C_TypBiznisEntity_Kniha_Id, rzpDenForBanPol, banPol.Suma, $"Položka bankového výpisu {banPol.Poradie}");
                               }
                           }
                       }

                       if (chybPol.Any())
                       {
                           chybneDoklady.Add((be.D_BiznisEntita_Id, $"Na uvedených riadkoch nie je predkontovaná správna hodnota:\r\n{chybPol.Join(Environment.NewLine)}"));
                       }
                   }
               }

               //CHECK-77
               foreach (var rzpDenGrp in rozpoctovyDennik
                   .Where(x => rozpoctovePolozky.SingleOrDefault(z => x.C_RzpPol_Id == z.C_RzpPol_Id) != null)
                   .GroupBy(x => x.D_BiznisEntita_Id))
               {
                   var prijmove = rzpDenGrp.Where(x => rozpoctovePolozky.Any(z => x.C_RzpPol_Id == z.C_RzpPol_Id && z.PrijemVydaj == 1)).Sum(x => x.Suma);
                   var vydajove = rzpDenGrp.Where(x => rozpoctovePolozky.Any(z => x.C_RzpPol_Id == z.C_RzpPol_Id && z.PrijemVydaj == 2)).Sum(x => x.Suma);
                   decimal suma = 0;
                   var dokl = doklady.Single(x => x.D_BiznisEntita_Id == rzpDenGrp.Key);
                   var kontrolovanaSuma = dokl.DM_Suma;

                   switch (dokl.C_TypBiznisEntity_Id)
                   {
                       case (int)TypBiznisEntityEnum.OZF:
                       case (int)TypBiznisEntityEnum.OZM:
                       case (int)TypBiznisEntityEnum.OOB:
                       case (int)TypBiznisEntityEnum.OCP:
                       case (int)TypBiznisEntityEnum.OFA:
                           suma = prijmove - vydajove;
                           break;

                       case (int)TypBiznisEntityEnum.DZF:
                       case (int)TypBiznisEntityEnum.DZM:
                       case (int)TypBiznisEntityEnum.DOB:
                       case (int)TypBiznisEntityEnum.DCP:
                       case (int)TypBiznisEntityEnum.DFA:
                           suma = vydajove - prijmove;
                           break;
                       default: //BAN, PDK
                           kontrolovanaSuma = 0; //Priradenie iba z dovodu aby kontrola nezahucala
                           break;
                   }

                   if (suma != 0 || kontrolovanaSuma != 0)
                   {
                       if (dokl.C_TypBiznisEntity_Id == (int)TypBiznisEntityEnum.DFA || dokl.C_TypBiznisEntity_Id == (int)TypBiznisEntityEnum.OFA)
                       {
                           var sumaZal = Db.Single<decimal>($"SELECT TOP 1 DM_SumaZal FROM crm.V_Doklad{(dokl.C_TypBiznisEntity_Id == (int)TypBiznisEntityEnum.DFA ? "D" : "O")}FA WHERE D_BiznisEntita_Id = {dokl.D_BiznisEntita_Id}");
                           kontrolovanaSuma -= sumaZal;
                       }

                       if (suma != kontrolovanaSuma)
                       {
                           chybneDoklady.Add((rzpDenGrp.Key, $"Suma zaúčtovania do rozpočtu sa nerovná sume dokladu. Rozdiel {suma - kontrolovanaSuma} €"));
                       }
                   }
               }

               foreach (var dokl in doklady)
               {
                   //CHECK-78
                   if (!rozpoctovyDennik.Any(x => x.D_BiznisEntita_Id == dokl.D_BiznisEntita_Id) && dokl.DM_Suma != 0 && dokl.C_TypBiznisEntity_Kniha_Id != (int)TypBiznisEntity_KnihaEnum.Externe_doklady_DaP)
                   {
                       if (dokl.PS && (dokl.C_TypBiznisEntity_Id == (int)TypBiznisEntityEnum.PDK ||
                                       dokl.C_TypBiznisEntity_Id == (int)TypBiznisEntityEnum.BAN))
                       {
                           //OK - PS doklady BAN a PDK nesmú mať záznamy
                       }
                       else if (dokl.PS && (dokl.C_TypBiznisEntity_Id == (int)TypBiznisEntityEnum.DFA ||
                                       dokl.C_TypBiznisEntity_Id == (int)TypBiznisEntityEnum.DZF ||
                                       dokl.C_TypBiznisEntity_Id == (int)TypBiznisEntityEnum.OFA ||
                                       dokl.C_TypBiznisEntity_Id == (int)TypBiznisEntityEnum.OZF) &&
                                       eSAMStart != null && dokl.Rok < eSAMStart.Value.Year)
                       {
                           //OK - Povolenie potvrdenia rozpočtu dokladov minulých rokov aj bez záznamov
                       }
                       else
                       {
                           if (dokl.C_TypBiznisEntity_Id == (int)TypBiznisEntityEnum.DFA || dokl.C_TypBiznisEntity_Id == (int)TypBiznisEntityEnum.OFA)
                           {
                               var sumaZal = Db.Single<decimal>($"SELECT TOP 1 DM_SumaZal FROM crm.V_Doklad{(dokl.C_TypBiznisEntity_Id == (int)TypBiznisEntityEnum.DFA ? "D" : "O")}FA WHERE D_BiznisEntita_Id = {dokl.D_BiznisEntita_Id}");
                               if (dokl.DM_Suma == sumaZal)
                               {
                                   continue; //Prípad, kedy tiež nepotrebujem žiadne záznamy v rzpDenniku
                               }
                           }

                           chybneDoklady.Add((dokl.D_BiznisEntita_Id, $"Na doklade nie je možné potvrdiť rozpočet, pretože rozpočtový denník neobsahuje žiadne záznamy"));
                       }
                   }
               }

           }

           if (rzpOductovanie)
           {
               LongOperationSetStateMessage(processKey, "Prebieha kontrola pred zrušením potvrdenia rozpočtom");

               //CHECK-93
               CheckUhradyPriZmeneStavu(doklady, ref chybneDoklady, "potvrdenie rozpočtom");
           }

           if (chybneDoklady.Any())
           {
               reportId = Guid.NewGuid().ToString();
               using var ms = new MemoryStream();
               TextWriter tw = new StreamWriter(ms);

               foreach (var dkl in chybneDoklady.GroupBy(x => x.D_BiznisEntita_Id))
               {
                   tw.WriteLine($"Doklad '{doklady.FirstOrDefault(x => x.D_BiznisEntita_Id == dkl.Key)?.CisloInterne}':");
                   tw.WriteLine(string.Join(Environment.NewLine, dkl.Select(x => x.Chyba)));
                   tw.WriteLine();
               }

               tw.Flush();
               ms.Position = 0;

               var ret = new RendererResult
               {
                   DocumentBytes = ms.ToArray(),
                   DocumentName = "ChybyZauctovania-" + ((TypBiznisEntityEnum)doklady.First().C_TypBiznisEntity_Id).ToString() + DateTime.Now.ToString("_yyyyMMdd_HHmm"),
                   Extension = "txt"
               };

               SetToCache(string.Concat("Report:", reportId), ret, new TimeSpan(8, 0, 0), useGzipCompression: true);
           }

           return chybneDoklady;
       }

       private void CheckUhradyPriZmeneStavu(List<BiznisEntita> doklady, ref List<(long D_BiznisEntita_Id, string Chyba)> chybneDoklady, string akcia)
       {
           foreach (var dokl in doklady.GroupBy(x => x.C_TypBiznisEntity_Id))
           {
               var fakturySql = "SELECT D_BiznisEntita_Id FROM crm.{0} WHERE D_BiznisEntita_Id IN ({1}) AND DatumUhrady IS NOT NULL";
               var fakturyUhrada = new List<long>();
               switch ((TypBiznisEntityEnum)dokl.Key)
               {
                   case TypBiznisEntityEnum.DFA:
                       fakturyUhrada.AddRange(Db.Select<long>(string.Format(fakturySql, "V_DokladDFA", dokl.Select(x => x.D_BiznisEntita_Id).Join(","))));
                       break;
                   case TypBiznisEntityEnum.OFA:
                       fakturyUhrada.AddRange(Db.Select<long>(string.Format(fakturySql, "V_DokladOFA", dokl.Select(x => x.D_BiznisEntita_Id).Join(","))));
                       break;
                   case TypBiznisEntityEnum.DZF:
                       fakturyUhrada.AddRange(Db.Select<long>(string.Format(fakturySql, "V_DokladDZF", dokl.Select(x => x.D_BiznisEntita_Id).Join(","))));
                       break;
                   case TypBiznisEntityEnum.OZF:
                       fakturyUhrada.AddRange(Db.Select<long>(string.Format(fakturySql, "V_DokladOZF", dokl.Select(x => x.D_BiznisEntita_Id).Join(","))));
                       break;
               }

               if (fakturyUhrada.Any())
               {
                   var uhrady = GetList(Db
                       .From<UhradaParovanieViewHelper>()
                       .Where(x => Sql.In(x.D_BiznisEntita_Id_Predpis, fakturyUhrada))
                       .Select(x => new { x.D_BiznisEntita_Id_Predpis, x.CisloInterne }));

                   foreach (var uhr in uhrady.GroupBy(x => x.D_BiznisEntita_Id_Predpis))
                   {
                       chybneDoklady.Add((uhr.Key.Value, $"Na doklad je už zaevidovaná úhrada ({ uhr.Select(x => string.Concat("'", x.CisloInterne, "'")).Join(", ")}). Nie je preto možné zrušiť {akcia}. Skontrolujte záložku úhrady."));
                   }
               }
           }
       }

       public void SkontrolujZauctovanie(SkontrolovatZauctovanieDto dokl, string processKey)
       {
           var biznisEntita = GetList(Db.From<BiznisEntita>().Where(x => Sql.In(x.D_BiznisEntita_Id, dokl.D_BiznisEntita_Ids)));
           var chybneDoklady = SkontrolovatZauctovanieDokladu(biznisEntita.First().C_TypBiznisEntity_Id, biznisEntita, 1, dokl.RzpDennik, false, dokl.UctDennik, false, processKey, out string reportId);

           LongOperationSetStateFinished(processKey, string.Empty, $"Operácia 'Skontrolovať zaúčtovanie' sa skončila {(!chybneDoklady.Any() ? "úspešne" : "neúspešne")}.", state: LongOperationState.Done, reportId: reportId);
       }

       public void DoposlanieUhrad(string kodPolozky, string processKey)
       {
           if (!kodPolozky.StartsWith("fin-bnk-ban") && !kodPolozky.StartsWith("fin-pok-pdk") && !kodPolozky.StartsWith("all-evi-intd"))
           {
               throw new WebEasValidationException(null, "Nepovolený typ položky!");
           }

           List<BiznisEntita> biznisEntita;

           if (kodPolozky.StartsWith("fin-pok-pdk"))
           {
               biznisEntita = GetList(Db.From<BiznisEntita>().Join<BiznisEntita, DokladPDK>((be, pdk) => be.D_BiznisEntita_Id == pdk.D_DokladPDK_Id && pdk.DCOM == false && be.C_StavEntity_Id > (int)StavEntityEnum.NOVY));
           }
           else if (kodPolozky.StartsWith("fin-bnk-ban"))
           {
               biznisEntita = GetList(Db.From<BiznisEntita>().Join<BiznisEntita, DokladBAN>((be, ban) => be.D_BiznisEntita_Id == ban.D_DokladBAN_Id && ban.DCOM == false && be.C_StavEntity_Id > (int)StavEntityEnum.NOVY));
           }
           else
           {
               biznisEntita = GetList(Db.From<BiznisEntita>().Join<BiznisEntita, DokladIND>((be, ind) => be.D_BiznisEntita_Id == ind.D_DokladIND_Id && ind.DCOM == false && be.C_StavEntity_Id > (int)StavEntityEnum.NOVY));
           }

           if (!biznisEntita.Any())
           {
               LongOperationSetStateFinished(processKey, string.Empty, "Neboli nájdené žiadne doklady na odoslanie.", state: LongOperationState.Done);
               return;
           }

           //tu sa posiela idNewState 0, lebo taky stav v StavEntityEnum nemame a vtedy sa berie stav z dokladu
           SpracovatZauctovatDoklad(biznisEntita, default, false, false, processKey, out string msgNeodoslanePolozky, doposlanieUhrad: true);

           var msg = biznisEntita.Count > 1 ? "Doklady boli úspešne odoslané." : "Doklad bol úspešne odoslaný.";
           LongOperationSetStateFinished(processKey, string.Empty, msg, state: LongOperationState.Done);
       }

       public void SpracujDoklad(SpracovatDokladDto dokl, string processKey, out string reportId, bool finishOperation = true)
       {
           #region Úvodná kontrola stavov

           var biznisEntita = GetList(Db.From<BiznisEntita>().Where(x => Sql.In(x.D_BiznisEntita_Id, dokl.Ids)));
           if (!biznisEntita.Any())
           {
               throw new WebEasValidationException(null, "Neboli nájdené žiadne doklady na spracovanie.");
           }

           if (biznisEntita.Any(x => x.C_StavEntity_Id != (int)StavEntityEnum.NOVY && x.C_StavEntity_Id != (int)StavEntityEnum.SPRACOVANY))
           {
               throw new WebEasValidationException(null, "Výber obsahuje doklady, ktoré nemajú stav 'Nový' alebo 'Spracovaný'. Operáciu 'Spracovať' nie je možné vykonať!");
           }

           var stavEntity = biznisEntita.First().C_StavEntity_Id;
           if (biznisEntita.Any(x => x.C_StavEntity_Id != stavEntity))
           {
               throw new WebEasValidationException(null, "Výber obsahuje doklady, ktoré majú rôzne stavy. Operáciu 'Spracovať' nie je možné vykonať!");
           }

           #endregion

           int idNewState = (biznisEntita.First().C_StavEntity_Id == (int)StavEntityEnum.NOVY) ?
               (int)StavEntityEnum.SPRACOVANY :
               (int)StavEntityEnum.NOVY;
           var idTBE = biznisEntita.First().C_TypBiznisEntity_Id;
           var valTBE = (TypBiznisEntityEnum)biznisEntita.First().C_TypBiznisEntity_Id;
           var chybneDoklady = new List<(long D_BiznisEntita_Id, string Chyba)>();
           reportId = null;
           string msgNeodoslanePolozky = null;

           // Kontroly pri spracovaní
           if (idNewState == (int)StavEntityEnum.SPRACOVANY)
           {
               #region CHECK-13

               var osobyDoklad = Db.Select(Db
                      .From<OsobaView>()
                      .Where(x => Sql.In(x.D_Osoba_Id, biznisEntita.Where(x => x.D_Osoba_Id.HasValue).Select(z => z.D_Osoba_Id)))
                      .Select(x => x.D_Osoba_Id));

               var uhrady = Db.Select(Db
                      .From<UhradaParovanieViewHelper>()
                      .Where(x => Sql.In(x.D_BiznisEntita_Id_Uhrada, biznisEntita.Select(z => z.D_BiznisEntita_Id))));

               var osobyUhrady = Db.Select(Db
                      .From<OsobaView>()
                      .Where(x => Sql.In(x.D_Osoba_Id, uhrady.Where(x => x.D_Osoba_Id.HasValue).Select(z => z.D_Osoba_Id)))
                      .Select(x => x.D_Osoba_Id));

               var polozkyBanHrady = new List<DokladBANPolViewHelper>();
               if (uhrady.Any(x => x.D_DokladBANPol_Id.HasValue))
               {
                   polozkyBanHrady = GetList(Db
                       .From<DokladBANPolViewHelper>()
                       .Where(x => Sql.In(x.D_DokladBANPol_Id, uhrady.Where(x => x.D_DokladBANPol_Id.HasValue).Select(x => x.D_DokladBANPol_Id)))
                       .Select(x => new { x.Poradie, x.D_DokladBANPol_Id }));
               }


               foreach (var be in biznisEntita)
               {
                   if (be.D_Osoba_Id.HasValue && !osobyDoklad.Any(x => x.D_Osoba_Id == be.D_Osoba_Id.Value))
                   {
                       chybneDoklady.Add((be.D_BiznisEntita_Id, "Hlavička má vyplnenú neplatnú osobu"));
                   }
               }

               //kontrola na osoby
               foreach (var uhrOsoba in uhrady
                   .Where(y => y.D_Osoba_Id.HasValue && !osobyUhrady.Any(x => x.D_Osoba_Id == y.D_Osoba_Id.Value))
                   .GroupBy(x => x.D_BiznisEntita_Id_Uhrada))
               {
                   var typBiznisEntityId = biznisEntita.Single(x => x.D_BiznisEntita_Id == uhrOsoba.Key).C_TypBiznisEntity_Id;

                   if (typBiznisEntityId == (short)TypBiznisEntityEnum.IND)
                   {
                       chybneDoklady.Add((uhrOsoba.Key, $"V položke preúčtovania úhrad (pč. { uhrOsoba.Select(x => x.Poradie).Join(",")}) je neplatná osoba"));
                   }

                   if (typBiznisEntityId == (short)TypBiznisEntityEnum.PDK)
                   {
                       chybneDoklady.Add((uhrOsoba.Key, $"V položke dokladu (pč. { uhrOsoba.Select(x => x.Poradie).Join(",")}) je neplatná osoba"));
                   }

                   if (typBiznisEntityId == (short)TypBiznisEntityEnum.BAN)
                   {
                       foreach (var polBan in polozkyBanHrady.Where(x => uhrOsoba.Select(x => x.D_DokladBANPol_Id).Contains(x.D_DokladBANPol_Id)))
                       {
                           chybneDoklady.Add((uhrOsoba.Key, $"V položke výpisu (pč. { polBan.Poradie }) a položke párovania úhrad (pč. { uhrOsoba.Where(x => x.D_DokladBANPol_Id == polBan.D_DokladBANPol_Id).Select(x => x.Poradie).Join(",")}) je neplatná osoba"));
                       }
                   }
               }

               foreach (var uhrDap in uhrady
                   .Where(x => (x.C_Typ_Id >= 1001 && x.C_Typ_Id <= 1007 && (!x.D_VymerPol_Id.HasValue || string.IsNullOrEmpty(x.VS))) ||
                   ((x.C_Typ_Id == (int)TypEnum.UhradaOFA ||
                   x.C_Typ_Id == (int)TypEnum.UhradaOZF ||
                   x.C_Typ_Id == (int)TypEnum.UhradaDFA ||
                   x.C_Typ_Id == (int)TypEnum.UhradaDZF ||
                   x.C_Typ_Id == (int)TypEnum.DobropisDFA ||
                   x.C_Typ_Id == (int)TypEnum.DobropisOFA
                   ) && (!x.D_BiznisEntita_Id_Predpis.HasValue || string.IsNullOrEmpty(x.VS))))
                   .GroupBy(x => x.D_BiznisEntita_Id_Uhrada))
               {
                   var typBiznisEntityId = biznisEntita.Single(x => x.D_BiznisEntita_Id == uhrDap.Key).C_TypBiznisEntity_Id;
                   var bezPredpisu = uhrDap.Where(x => (x.C_Typ_Id == (int)TypEnum.UhradaOFA ||
                   x.C_Typ_Id == (int)TypEnum.UhradaOZF ||
                   x.C_Typ_Id == (int)TypEnum.UhradaDFA ||
                   x.C_Typ_Id == (int)TypEnum.UhradaDZF ||
                   x.C_Typ_Id == (int)TypEnum.DobropisDFA ||
                   x.C_Typ_Id == (int)TypEnum.DobropisOFA
                   ) && !x.D_BiznisEntita_Id_Predpis.HasValue);

                   var bezVymeru = uhrDap.Where(x => x.C_Typ_Id >= 1001 && x.C_Typ_Id <= 1007 && !x.D_VymerPol_Id.HasValue);
                   var bezVs = uhrDap.Where(x => string.IsNullOrEmpty(x.VS));

                   if (typBiznisEntityId == (short)TypBiznisEntityEnum.IND)
                   {
                       if (bezVymeru.Any())
                       {
                           chybneDoklady.Add((uhrDap.Key, $"V položke preúčtovania úhrad (pč. { bezVymeru.Select(x => x.Poradie).Join(",")}) chýba položka rozhodnutia"));
                       }

                       if (bezPredpisu.Any())
                       {
                           chybneDoklady.Add((uhrDap.Key, $"V položke preúčtovania úhrad (pč. { bezPredpisu.Select(x => x.Poradie).Join(",")}) chýba položka úhrady"));
                       }

                       if (bezVs.Any())
                       {
                           chybneDoklady.Add((uhrDap.Key, $"V položke preúčtovania úhrad (pč. { bezVs.Select(x => x.Poradie).Join(",")}) chýba VS"));
                       }
                   }

                   if (typBiznisEntityId == (short)TypBiznisEntityEnum.PDK)
                   {
                       if (bezVymeru.Any())
                       {
                           chybneDoklady.Add((uhrDap.Key, $"V položke dokladu (pč. { bezVymeru.Select(x => x.Poradie).Join(",")}) chýba položka rozhodnutia"));
                       }

                       if (bezPredpisu.Any())
                       {
                           chybneDoklady.Add((uhrDap.Key, $"V položke dokladu (pč. { bezPredpisu.Select(x => x.Poradie).Join(",")}) chýba položka úhrady"));
                       }

                       if (bezVs.Any())
                       {
                           chybneDoklady.Add((uhrDap.Key, $"V položke dokladu (pč. { bezVymeru.Select(x => x.Poradie).Join(",")}) chýba VS"));
                       }
                   }

                   if (typBiznisEntityId == (short)TypBiznisEntityEnum.BAN)
                   {
                       foreach (var polBan in polozkyBanHrady.Where(x => uhrDap.Select(x => x.D_DokladBANPol_Id).Contains(x.D_DokladBANPol_Id)))
                       {
                           if (bezVymeru.Any())
                           {
                               chybneDoklady.Add((uhrDap.Key, $"V položke výpisu (pč. { polBan.Poradie }) a položke párovania úhrad (pč. { bezVymeru.Where(x => x.D_DokladBANPol_Id == polBan.D_DokladBANPol_Id).Select(x => x.Poradie).Join(",")}) chýba položka rozhodnutia"));
                           }

                           if (bezPredpisu.Any())
                           {
                               chybneDoklady.Add((uhrDap.Key, $"V položke výpisu (pč. { polBan.Poradie }) a položke párovania úhrad (pč. { bezPredpisu.Where(x => x.D_DokladBANPol_Id == polBan.D_DokladBANPol_Id).Select(x => x.Poradie).Join(",")}) chýba položka úhrady"));
                           }

                           if (bezVs.Any())
                           {
                               chybneDoklady.Add((uhrDap.Key, $"V položke výpisu (pč. { polBan.Poradie }) a položke párovania úhrad (pč. { bezVs.Where(x => x.D_DokladBANPol_Id == polBan.D_DokladBANPol_Id).Select(x => x.Poradie).Join(",")}) chýba VS"));
                           }
                       }
                   }
               }

               #endregion

               if (idTBE == (short)TypBiznisEntityEnum.BAN)
               {
                   #region Načítanie hlavičiek a položiek BAN so všetkými stĺpcami potrebnými na nasledovné kontroly

                   var sqlExp = Db.From<DokladBANView>();
                   foreach (var rok in biznisEntita.GroupBy(x => x.Rok))
                   {
                       sqlExp.Or(x => x.Rok == rok.Key && Sql.In(x.D_BiznisEntita_Id, rok.Select(z => z.D_BiznisEntita_Id)));
                   }

                   var doklBan = GetList(sqlExp.Select(x => new { x.D_BiznisEntita_Id, x.DM_Debet, x.DM_Kredit, x.PocetPol, x.DatumDokladu }));

                   var polozkyBan = GetList(Db.From<DokladBANPolViewHelper>()
                       .Where(x => Sql.In(x.D_BiznisEntita_Id, biznisEntita.Select(z => z.D_BiznisEntita_Id)))
                       .Select(x => new { x.D_BiznisEntita_Id, x.D_DokladBANPol_Id, x.Poradie, x.DatumPohybu, x.C_Typ_Id, x.Suma }));

                   #endregion

                   #region CHECK-10  Kontrola na pocet poloziek

                   foreach (var ban in doklBan)
                   {
                       if (ban.PocetPol != default)
                       {
                           var pocetPoloziek = polozkyBan.Where(p => p.D_BiznisEntita_Id == ban.D_BiznisEntita_Id).Count();

                           if (pocetPoloziek != ban.PocetPol)
                           {
                               chybneDoklady.Add((ban.D_BiznisEntita_Id, "Počet položiek uvedený v hlavičke výpisu sa nezhoduje s počtom zaevidovaných položiek."));
                           }
                       }
                   }

                   #endregion

                   #region CHECK-5, CHECK-6  Kontrola Datumu Uhrady

                   foreach (var ban in doklBan)
                   {
                       foreach (var uhrada in polozkyBan.Where(p => p.D_BiznisEntita_Id == ban.D_BiznisEntita_Id))
                       {
                           if (uhrada.DatumPohybu > ban.DatumDokladu) // neviem, ktory datum z uhrady
                           {
                               chybneDoklady.Add((ban.D_BiznisEntita_Id, $"Dátum úhrady {uhrada.DatumPohybu.ToShortDateString()} na {uhrada.Poradie}. riadku je väčší ako dátum dokladu ({ban.DatumDokladu.ToShortDateString()})"));
                           }

                           if (uhrada.DatumPohybu.Month != ban.DatumDokladu.Month) // neviem, ktory datum z uhrady
                           {
                               chybneDoklady.Add((ban.D_BiznisEntita_Id, $"Dátum úhrady {uhrada.DatumPohybu.ToShortDateString()} na {uhrada.Poradie}. riadku má rozdielny mesiac ako dátum dokladu ({ban.DatumDokladu.ToShortDateString()})"));
                           }
                       }
                   }

                   #endregion

                   #region CHECK-08 Kontrola na konzistenciu medzi tabuľkami fin.V_DokladBANPol a D_UhradaParovanie

                   var chybnePolozky = new List<(long, int)>();
                   var chybnePolozkySuma = new List<(long, int)>();

                   var naparovaneUhrady = Db.Select<(long D_DokladBANPol_Id, decimal DmCenaSum)>(Db
                      .From<UhradaParovanieViewHelperDCOM>()
                      .Where(x => Sql.In(x.D_BiznisEntita_Id_Uhrada, biznisEntita.Select(z => z.D_BiznisEntita_Id)))
                      .Select(x => new { x.D_DokladBANPol_Id, DmCenaSum = Sql.Sum(x.DM_Cena) }) //Sem nedávam Cena+Rozdiel !!
                      .GroupBy(x => x.D_DokladBANPol_Id));

                   foreach (var pol in polozkyBan)
                   {
                       if (pol.C_Typ_Id == (int)TypEnum.UhradaPohZav)
                       {
                           if (!naparovaneUhrady.Any(x => x.D_DokladBANPol_Id == pol.D_DokladBANPol_Id) ||
                             naparovaneUhrady.Single(x => x.D_DokladBANPol_Id == pol.D_DokladBANPol_Id).DmCenaSum != pol.Suma)
                           {
                               chybnePolozkySuma.Add((pol.D_BiznisEntita_Id, pol.Poradie));
                           }
                       }
                       else
                       {
                           if (naparovaneUhrady.Any(x => x.D_DokladBANPol_Id == pol.D_DokladBANPol_Id))
                           {
                               chybnePolozky.Add((pol.D_BiznisEntita_Id, pol.Poradie));
                           }
                       }
                   }

                   if (chybnePolozkySuma.Any() || chybnePolozky.Any())
                   {
                       var msg = string.Empty;

                       if (chybnePolozkySuma.Any())
                       {
                           foreach (var pol in chybnePolozkySuma.GroupBy(x => x.Item1))
                           {
                               chybneDoklady.Add((pol.Key, $"Suma úhrady na ({pol.Select(x => x.Item2).Join(",")}) položke bankového výpisu sa nezhoduje so sumou na záznamoch párovania. "));
                           }
                       }

                       if (chybnePolozky.Any())
                       {
                           foreach (var pol in chybnePolozkySuma.GroupBy(x => x.Item1))
                           {
                               chybneDoklady.Add((pol.Key, $"({pol.Select(x => x.Item2).Join(",")}) položka bankového výpisu nesmie mať zaevidované žiadne záznamy v párovaní úhrad"));
                           }
                       }
                   }

                   #endregion

                   #region CHECK-9  Kontrola DM_Suma a rozdiel

                   foreach (var ban in doklBan)
                   {
                       if ((ban.DM_Kredit - ban.DM_Debet) != biznisEntita.Single(x => x.D_BiznisEntita_Id == ban.D_BiznisEntita_Id).DM_Suma)
                       {
                           chybneDoklady.Add((ban.D_BiznisEntita_Id, "Hodnota 'kredit - debet' na doklade sa nezhoduje so sumou napočítanou z položiek bankového výpisu."));
                       }
                   }

                   #endregion

               }
           }

           // CHECK-11  Kontrola pri odspracovani odkladu
           if (idNewState == (int)StavEntityEnum.NOVY)
           {
               CheckUhradyPriZmeneStavu(biznisEntita, ref chybneDoklady, "spracovanie dokladu"); //Využijem tiež túto metódu, ale nepoužijem naplnené chyby, lebo nie je podpora pre výpis do súboru
           }

           if (chybneDoklady.Any())
           {
               reportId = Guid.NewGuid().ToString();
               using var ms = new MemoryStream();
               TextWriter tw = new StreamWriter(ms);

               foreach (var dkl in chybneDoklady.GroupBy(x => x.D_BiznisEntita_Id))
               {
                   tw.WriteLine($"Doklad '{biznisEntita.FirstOrDefault(x => x.D_BiznisEntita_Id == dkl.Key)?.CisloInterne}':");
                   tw.WriteLine(string.Join(Environment.NewLine, dkl.Select(x => x.Chyba)));
                   tw.WriteLine();
               }

               tw.Flush();
               ms.Position = 0;

               var ret = new RendererResult
               {
                   DocumentBytes = ms.ToArray(),
                   DocumentName = "ChybySpracovania-" + ((TypBiznisEntityEnum)biznisEntita.First().C_TypBiznisEntity_Id).ToString() + DateTime.Now.ToString("_yyyyMMdd_HHmm"),
                   Extension = "txt"
               };

               SetToCache(string.Concat("Report:", reportId), ret, new TimeSpan(8, 0, 0), useGzipCompression: true);
           }
           else
           {
               SpracovatZauctovatDoklad(biznisEntita, idNewState, false, false, processKey, out msgNeodoslanePolozky);
           }


           if (!string.IsNullOrEmpty(reportId))
           {
               LongOperationSetStateFinished(processKey, string.Empty, $"Operácia 'Spracovať' sa skončila neúspešne.", state: LongOperationState.Done, reportId: reportId);
           }
           else
           {
               // A koniec long time
               if (idNewState == (int)StavEntityEnum.SPRACOVANY)
               {
                   var msg = biznisEntita.Count > 1 ? "Doklady boli úspešne spracované. " : "Doklad bol úspešne spracovaný. ";
                   if (!string.IsNullOrEmpty(msgNeodoslanePolozky))
                   {
                       msg = string.Concat(msg, "Avšak ", msgNeodoslanePolozky);
                   }

                   if (finishOperation)
                   {
                       LongOperationSetStateFinished(processKey, string.Empty, msg, state: LongOperationState.Done);
                   }
                   else
                   {
                       LongOperationSetStateMessage(processKey, msg);
                   }
               }
               else
               {
                   var msg = biznisEntita.Count > 1 ? "Na dokladoch bolo úspešne zrušené spracovanie. " : "Na doklade bolo úspešne zrušené spracovanie. ";
                   if (!string.IsNullOrEmpty(msgNeodoslanePolozky))
                   {
                       msg = string.Concat(msg, "Avšak ", msgNeodoslanePolozky);
                   }

                   if (finishOperation)
                   {
                       LongOperationSetStateFinished(processKey, string.Empty, msg, state: LongOperationState.Done);
                   }
                   else
                   {
                       LongOperationSetStateMessage(processKey, msg);
                   }
               }
           }
       }

       */

        [DataContract]
        [Schema("fin")]
        [Alias("V_UhradaParovanie")]
        private class UhradaParovanieViewHelperDCOM
        {
            public long D_BiznisEntita_Id_Uhrada { get; set; }
            public long? D_VymerPol_Id { get; set; }
            public long D_UhradaParovanie_Id { get; set; }
            public long? D_VymerPol_Id_Externe { get; set; }
            public decimal DmCenaRozdiel { get; set; } //Tento fild vo VIEW-e nie je. Asi to nevadi.
            public decimal DM_Cena { get; set; }
            public Guid? D_Osoba_Id_Externe { get; set; }
            public string VS { get; set; }
            public DateTime? DatumValuta { get; set; }
            public long? D_DokladBANPol_Id { get; set; }
        }
        /*
        private void SpracovatZauctovatDoklad(List<BiznisEntita> biznisEntita, int idNewState, bool uctZauctovanie, bool uctOductovanie, string processKey, out string msgNeodoslanePolozky, bool doposlanieUhrad = false)
        {
            var prepojenePolozky = new List<UhradaParovanieViewHelperDCOM>();
            var pokladnica = new List<PokladnicaView>();
            var bankaUcet = new List<BankaUcetView>();
            var stredisko = new List<StrediskoView>();
            msgNeodoslanePolozky = string.Empty;
            var dcomRezim = GetNastavenieI("reg", "eSAMRezim") == 1;
            int idOldState = default;
            bool uctZmenaZauctovania = uctZauctovanie || uctOductovanie;

            if (biznisEntita.GroupBy(x => x.C_StavEntity_Id).Count() > 1 && !doposlanieUhrad)
            {
                throw new WebEasValidationException(null, "Výber obsahuje doklady, ktoré majú rôzne stavy. Operáciu nie je možné vykonať!");
            }

            if (biznisEntita.GroupBy(x => x.C_TypBiznisEntity_Id).Count() > 1)
            {
                throw new WebEasValidationException(null, "Výber obsahuje doklady, ktoré majú rôzne typy. Operáciu nie je možné vykonať!");
            }

            // kontrola stavu staci iba na jednej entite za stav. priestor
            if (!doposlanieUhrad)
            {
                foreach (var state in biznisEntita.GroupBy(x => x.C_StavovyPriestor_Id))
                {
                    idOldState = state.First().C_StavEntity_Id;
                    var stateInfo = CheckPossibleState(idOldState, idNewState, state.Key);
                }
            }

            var idTBE = biznisEntita.First().C_TypBiznisEntity_Id;

            if (dcomRezim)
            {
                var sendPlaToDcom = idTBE == (short)TypBiznisEntityEnum.BAN ||
                                    idTBE == (short)TypBiznisEntityEnum.PDK ||
                                    idTBE == (short)TypBiznisEntityEnum.IND;

                var dcomPlatbyDapOd = Db.Single<DateTime?>("select reg.F_NastavenieD('fin','DCOMPlatbyDaPOd',null)");

                if (sendPlaToDcom && dcomPlatbyDapOd.HasValue)
                {
                    if (uctZmenaZauctovania)
                    {
                        LongOperationSetStateMessage(processKey, "Identifikácia úhrad určených na zaslanie zaúčtovania do DCOM-u");
                    }
                    else
                    {
                        LongOperationSetStateMessage(processKey, "Identifikácia úhrad potrebných pre zaslanie do DCOM-u");
                    }

                    prepojenePolozky = Db.Select<UhradaParovanieViewHelperDCOM>(
                        "SELECT D_BiznisEntita_Id_Uhrada, D_VymerPol_Id, D_UhradaParovanie_Id, D_VymerPol_Id_Externe, DM_Cena, (DM_Cena + DM_Rozdiel) as DmCenaRozdiel, D_Osoba_Id_Externe, VS, DatumValuta, D_DokladBANPol_Id " +
                        "FROM fin.V_UhradaParovanie " +
                        "WHERE D_Tenant_Id = @tenantId AND D_BiznisEntita_Id_Uhrada IN (@beId) AND " +
                        "     (C_Typ_Id BETWEEN 1001 AND 1010) AND " +
                        "     DatumDokladu BETWEEN @dcomPlatbyDapOd AND " +
                        "                   ISNULL(reg.F_NastavenieD('fin','DCOMPlatbyDaPDo',null), '2100-01-01')", new { tenantId = Session.TenantId, dcomPlatbyDapOd, beId = biznisEntita.Select(x => x.D_BiznisEntita_Id) });

                    if (doposlanieUhrad)
                    {
                        prepojenePolozky.RemoveAll(x => !x.D_VymerPol_Id.HasValue);
                    }

                    if (idNewState == (int)StavEntityEnum.SPRACOVANY)
                    {
                        if (prepojenePolozky.Any(x => !x.D_VymerPol_Id.HasValue))
                        {
                            if (biznisEntita.Count == 1)
                            {
                                throw new WebEasValidationException(null, "Doklad nemá niektoré položky korektne prepojené na položku dane/poplatku!");
                            }
                            else
                            {
                                throw new WebEasValidationException(null, "Výber obsahuje doklady, ktoré nemajú niektoré položky korektne prepojené na položku dane/poplatku!");
                            }
                        }

                        if (prepojenePolozky.Any())
                        {
                            pokladnica = GetList(Db.From<PokladnicaView>().Select(z => new { z.C_Pokladnica_Id, z.Kod, z.Nazov, z.MenaKod }).Where(x => (x.DCOM == null || x.DCOM == false) && biznisEntita.Where(z => prepojenePolozky.Any(t => t.D_BiznisEntita_Id_Uhrada == z.D_BiznisEntita_Id)).Select(y => y.C_Pokladnica_Id).Contains(x.C_Pokladnica_Id)));
                            bankaUcet = GetList(Db.From<BankaUcetView>().Select(z => new { z.C_BankaUcet_Id, z.IBAN, z.MenaKod, z.Nazov, z.BIC }).Where(x => (x.DCOM == null || x.DCOM == false) && biznisEntita.Where(z => prepojenePolozky.Any(t => t.D_BiznisEntita_Id_Uhrada == z.D_BiznisEntita_Id)).Select(y => y.C_BankaUcet_Id).Contains(x.C_BankaUcet_Id)));
                            stredisko = GetList(Db.From<StrediskoView>().Select(z => new { z.C_Stredisko_Id, z.Kod, z.Nazov }).Where(x => (x.DCOM == null || x.DCOM == false) && biznisEntita.Where(z => prepojenePolozky.Any(t => t.D_BiznisEntita_Id_Uhrada == z.D_BiznisEntita_Id)).Select(y => y.C_Stredisko_Id).Contains(x.C_Stredisko_Id)));
                        }
                    }

                    if (idTBE == (short)TypBiznisEntityEnum.PDK)
                    {
                        foreach (var pdk in prepojenePolozky.GroupBy(x => x.D_BiznisEntita_Id_Uhrada))
                        {
                            if (pdk.GroupBy(x => x.D_Osoba_Id_Externe).Count() > 1)
                            {
                                throw new WebEasValidationException(null, "Výber obsahuje pokladničné doklady, ktoré obsahujú úhrady rozhodnutí pre viacerých daňovníkov. Pokladničný doklad musí mať položky iba jednej osoby! (Takúto úhradu vykonajte viacerými PDK)");
                            }
                        }
                    }
                }

                //kontrola kodov pokladnice a ich konverzie na cislo
                foreach (var pokl in pokladnica)
                {
                    KontrolaPokladnica(pokl.Kod, dcomRezim);
                }

                //kontrola kodov strediska a ich konverzie na cislo
                foreach (var str in stredisko)
                {
                    KontrolaStredisko(str.Kod, dcomRezim);
                }

                if (idTBE == (short)TypBiznisEntityEnum.BAN && prepojenePolozky.Any())
                {
                    //kontrola ban. vypisov a ich konverzie na cisla vypisu na int
                    foreach (var be in biznisEntita.Where(x => prepojenePolozky.Select(z => z.D_BiznisEntita_Id_Uhrada).Contains(x.D_BiznisEntita_Id)))
                    {
                        if (!int.TryParse(be.CisloExterne, out int cislo))
                        {
                            throw new WebEasValidationException(null, $"Číslo výpisu '{be.CisloExterne}' musí byť kvôli odoslaniu do DCOM-u skonvertovateľné na číslo!");
                        }
                    }
                }

                if (prepojenePolozky.Any() || bankaUcet.Any() || pokladnica.Any() || stredisko.Any())
                {
                    using var client = new PlatbyClient();
                    var dcmHeader = new DcomWs.IsoPla.DcmHeader
                    {
                        tenantId = Session.D_Tenant_Id_Externe?.ToString() ?? throw new ArgumentException("nie je zadefinovane D_Tenant_Id_Externe"),
                        isoId = Session.IsoId,
                        requestId = WebEas.Context.Current.CurrentCorrelationID.ToString()
                    };

                    if (pokladnica.Any() || idTBE == (short)TypBiznisEntityEnum.PDK)
                    {
                        try
                        {
                            #region Zaslanie pokladnice do DCOMu

                            UpdatePokladnicaDcom(processKey, pokladnica, client, ref dcmHeader);

                            #endregion

                            #region Zaslanie pokladničného dokladu do DCOM-u

                            var paymentsRecords = new List<SetPaymentRecord>();
                            var dokladyPdk = GetList(
                                Db.From<DokladPDKView>()
                                .Select(z => new { z.D_BiznisEntita_Id, z.C_TypBiznisEntity_Kniha_Id, z.DM_Suma, z.Popis, z.DatumDokladu, z.C_Pokladnica_Id, z.Cislo, z.DCOM })
                                .Where(x => prepojenePolozky.Select(z => z.D_BiznisEntita_Id_Uhrada).Contains(x.D_BiznisEntita_Id)));

                            foreach (var dokladPdk in dokladyPdk)
                            {
                                var be = biznisEntita.Single(x => x.D_BiznisEntita_Id == dokladPdk.D_BiznisEntita_Id);
                                var prvaPolozka = prepojenePolozky.First(x => x.D_BiznisEntita_Id_Uhrada == dokladPdk.D_BiznisEntita_Id);

                                bool? typBiznisEntityKniha = dokladPdk.C_TypBiznisEntity_Kniha_Id == (int)TypBiznisEntity_KnihaEnum.Prijmove_pokladnicne_doklady
                                    ? true
                                    : (dokladPdk.C_TypBiznisEntity_Kniha_Id == (int)TypBiznisEntity_KnihaEnum.Vydajove_pokladnicne_doklady ? false : (bool?)null);

                                var paymentRecord = new SetPaymentRecord
                                {
                                    amount = dokladPdk.DM_Suma,
                                    cashReceiptPurpose = dokladPdk.Popis,
                                    dateCashReceipt = dokladPdk.DatumDokladu,
                                    dateCashReceiptSpecified = true,
                                    entryDescriptionBankStatement = dokladPdk.Popis,
                                    idCashBook = "ES-" + dokladPdk.C_Pokladnica_Id,
                                    idCashReceipt = "ES-" + be.D_BiznisEntita_Id,
                                    isIncomeCashReceipt = typBiznisEntityKniha,
                                    isIncomeCashReceiptSpecified = typBiznisEntityKniha.HasValue,
                                    numberCashReceipt = dokladPdk.Cislo,
                                    numberCashReceiptSpecified = true,
                                    idPerson = prvaPolozka.D_Osoba_Id_Externe.ToString(),
                                    paymentType = PaymentTypeEnum.PO,
                                    payerReference = string.Concat("VS", (prvaPolozka.VS ?? string.Empty).PadLeft(10, '0'), "/SS/KS"),
                                    typeOfOperation = idNewState == (int)StavEntityEnum.NOVY ? TypeOfOperationEnum.Delete : (dokladPdk.DCOM ? TypeOfOperationEnum.Update : TypeOfOperationEnum.Create)
                                };

                                var paymentItems = new List<SetPaymentItem>();
                                foreach (var uhrParovanie in prepojenePolozky.Where(x => x.D_BiznisEntita_Id_Uhrada == dokladPdk.D_BiznisEntita_Id))
                                {
                                    paymentItems.Add(new SetPaymentItem
                                    {
                                        amount = uhrParovanie.DmCenaRozdiel,
                                        accounted = idNewState == (int)StavEntityEnum.ZAUCTOVANY || idNewState == (int)StavEntityEnum.ZAUCTOVANY_UCT,
                                        idCashReceiptEntry = uhrParovanie.D_UhradaParovanie_Id.ToString(),
                                        idPerson = uhrParovanie.D_Osoba_Id_Externe.ToString(),
                                        taxAssessmentItemID = uhrParovanie.D_VymerPol_Id_Externe.Value,
                                        typeOfOperation = idNewState == (int)StavEntityEnum.NOVY ? TypeOfOperationEnum.Delete : (dokladPdk.DCOM ? TypeOfOperationEnum.Update : TypeOfOperationEnum.Create)
                                    });
                                }
                                paymentRecord.paymentItems = paymentItems.ToArray();
                                paymentsRecords.Add(paymentRecord);
                            }

                            if (paymentsRecords.Any())
                            {
                                if (uctZmenaZauctovania)
                                {
                                    LongOperationSetStateMessage(processKey, "Posielam informáciu o zaúčtovaní úhrad do DCOM-u");
                                }
                                else
                                {
                                    var msg = idNewState == (int)StavEntityEnum.SPRACOVANY ? "Posielam pokladničný doklad do DCOM-u." : "Posielam odstránenie pokladničného dokladu do DCOM-u";
                                    LongOperationSetStateMessage(processKey, msg);
                                }

                                client.setPayments(ref dcmHeader, new ReqSetPaymentsList { requestRecords = paymentsRecords.ToArray(), recordCount = paymentsRecords.Count });

                                foreach (var dokladPdk in GetList<DokladPDK>(x => prepojenePolozky.Select(z => z.D_BiznisEntita_Id_Uhrada).Contains(x.D_DokladPDK_Id)))
                                {
                                    dokladPdk.DCOM = idNewState != (int)StavEntityEnum.NOVY;
                                    UpdateData(dokladPdk);
                                }
                            }
                            #endregion
                        }
                        catch (Exception e)
                        {
                            //Ak to spadne, vrátim chybu "Do DCOM-u sa nepodarilo zaslať pokladnicu, a teda aj následné párovanie úhrad." – vyskočím a vynechám aj krok 3. , vykonám iba 4.
                            if (idNewState == (int)StavEntityEnum.NOVY)
                            {
                                throw;
                            }
                            else
                            {
                                Log.Error(e);
                                msgNeodoslanePolozky = "do DCOM-u sa nepodarilo zaslať pokladnicu, a teda aj následné párovanie úhrad.";
                            }
                        }
                    }

                    if (bankaUcet.Any() || idTBE == (short)TypBiznisEntityEnum.BAN)
                    {
                        try
                        {
                            #region Zaslanie bankového účtu do DCOM-u

                            UpdateBankaUcetDcom(processKey, bankaUcet, client, ref dcmHeader);

                            #endregion

                            #region Zaslanie bankového výpisu do DCOM-u

                            var bankStatementRecords = new List<SetBankStatementRecord>();
                            var banDoklad = GetList(Db.From<DokladBAN>().Select(s => new { s.D_DokladBAN_Id, s.DCOM }).Where(x => prepojenePolozky.Select(z => z.D_BiznisEntita_Id_Uhrada).Contains(x.D_DokladBAN_Id)));

                            foreach (var be in biznisEntita.Where(x => prepojenePolozky.Select(z => z.D_BiznisEntita_Id_Uhrada).Contains(x.D_BiznisEntita_Id)))
                            {
                                var typeOfOperationEnum = idNewState == (int)StavEntityEnum.NOVY ? TypeOfOperationEnum.Delete : (banDoklad.Single(x => x.D_DokladBAN_Id == be.D_BiznisEntita_Id).DCOM ? TypeOfOperationEnum.Update : TypeOfOperationEnum.Create);
                                bankStatementRecords.Add(new SetBankStatementRecord
                                {
                                    idBankStatement = "ES-" + be.D_BiznisEntita_Id,
                                    idBankAccount = "ES-" + be.C_BankaUcet_Id,
                                    numberBankStatement = int.Parse(be.CisloExterne),
                                    dateBankStatement = be.DatumDokladu,
                                    descriptionBankStatement = be.Popis ?? be.CisloInterne,
                                    typeOfOperation = typeOfOperationEnum
                                });
                            }

                            if (bankStatementRecords.Any())
                            {
                                client.setBankStatements(ref dcmHeader, new ReqSetBankStatementsList { requestRecords = bankStatementRecords.ToArray(), recordCount = bankStatementRecords.Count });

                                var paymentRecords = new List<SetPaymentRecord>();
                                var dokladyBanPol = GetList(
                                Db.From<DokladBANPolViewHelper>()
                                    .Select(z => new { z.D_BiznisEntita_Id, z.VS, z.SS, z.KS, z.Suma, z.DatumValuta, z.DatumPohybu, z.Popis, z.D_DokladBANPol_Id, z.Poradie })
                                    .Where(x => prepojenePolozky.Select(z => z.D_BiznisEntita_Id_Uhrada)
                                    .Contains(x.D_BiznisEntita_Id)));

                                foreach (var dokladBanPol in dokladyBanPol)
                                {
                                    if (prepojenePolozky.Any(x => x.D_DokladBANPol_Id == dokladBanPol.D_DokladBANPol_Id))
                                    {
                                        var be = biznisEntita.Single(x => x.D_BiznisEntita_Id == dokladBanPol.D_BiznisEntita_Id);
                                        var typeOfOperationEnum = idNewState == (int)StavEntityEnum.NOVY ? TypeOfOperationEnum.Delete : (banDoklad.Single(x => x.D_DokladBAN_Id == be.D_BiznisEntita_Id).DCOM ? TypeOfOperationEnum.Update : TypeOfOperationEnum.Create);
                                        string vs = string.Empty, ss = string.Empty, ks = string.Empty;

                                        vs = (dokladBanPol.VS ?? string.Empty).PadLeft(10, '0');

                                        if (!string.IsNullOrEmpty(dokladBanPol.SS))
                                        {
                                            ss = dokladBanPol.SS.PadLeft(10, '0');
                                        }

                                        if (!string.IsNullOrEmpty(dokladBanPol.KS))
                                        {
                                            ks = dokladBanPol.KS.PadLeft(4, '0');
                                        }

                                        var paymentRecord = new SetPaymentRecord
                                        {
                                            amount = Math.Abs(dokladBanPol.Suma),
                                            dateBankStatementEntrySettlement = dokladBanPol.DatumValuta,
                                            dateBankStatementEntrySettlementSpecified = true,
                                            entryDateBankStatement = dokladBanPol.DatumPohybu,
                                            entryDateBankStatementSpecified = true,
                                            entryDescriptionBankStatement = dokladBanPol.Popis,
                                            idBankAccount = "ES-" + be.C_BankaUcet_Id,
                                            idBankStatement = "ES-" + be.D_BiznisEntita_Id,
                                            idBankStatementEntry = "ES-" + dokladBanPol.D_DokladBANPol_Id,
                                            isCreditBankStatementEntry = dokladBanPol.Suma > 0,
                                            isCreditBankStatementEntrySpecified = true,
                                            numberBankStatementEntry = dokladBanPol.Poradie,
                                            numberBankStatementEntrySpecified = true,
                                            idPerson = Guid.Empty.ToString(),
                                            paymentType = PaymentTypeEnum.BA,
                                            payerReference = string.Concat("VS", vs, "/SS", ss, "/KS", ks),
                                            typeOfOperation = typeOfOperationEnum
                                        };

                                        var paymentItems = new List<SetPaymentItem>();
                                        foreach (var uhrParovanie in prepojenePolozky.Where(x => x.D_DokladBANPol_Id == dokladBanPol.D_DokladBANPol_Id))
                                        {
                                            paymentItems.Add(new SetPaymentItem
                                            {
                                                amount = Math.Abs(uhrParovanie.DmCenaRozdiel),
                                                accounted = idNewState == (int)StavEntityEnum.ZAUCTOVANY || idNewState == (int)StavEntityEnum.ZAUCTOVANY_UCT,
                                                idBankPaymentItem = uhrParovanie.D_UhradaParovanie_Id.ToString(),
                                                idPerson = uhrParovanie.D_Osoba_Id_Externe.ToString(),
                                                taxAssessmentItemID = uhrParovanie.D_VymerPol_Id_Externe.Value,
                                                typeOfOperation = typeOfOperationEnum
                                            });
                                        }

                                        paymentRecord.paymentItems = paymentItems.ToArray();
                                        paymentRecords.Add(paymentRecord);
                                    }
                                }

                                if (uctZmenaZauctovania)
                                {
                                    LongOperationSetStateMessage(processKey, "Posielam informáciu o zaúčtovaní úhrad do DCOM-u");
                                }
                                else
                                {
                                    var msg = idNewState == (int)StavEntityEnum.SPRACOVANY ? "Posielam bankový výpis do DCOM-u" : "Posielam odstránenie bankového výpisu do DCOM-u";
                                    LongOperationSetStateMessage(processKey, msg);
                                }

                                client.setPayments(ref dcmHeader, new ReqSetPaymentsList { requestRecords = paymentRecords.ToArray(), recordCount = paymentRecords.Count });

                                foreach (var dokladBan in GetList<DokladBAN>(x => prepojenePolozky.Select(z => z.D_BiznisEntita_Id_Uhrada).Contains(x.D_DokladBAN_Id)))
                                {
                                    dokladBan.DCOM = idNewState != (int)StavEntityEnum.NOVY;
                                    UpdateData(dokladBan);
                                }
                            }
                            #endregion
                        }
                        catch (Exception e)
                        {
                            //Ak to spadne, vrátim chybu "Do DCOM-u sa nepodarilo zaslať bankový účet, a teda aj následné párovanie úhrad." – vyskočím a vynechám aj krok 3., vykonám iba 4.
                            if (idNewState == (int)StavEntityEnum.NOVY)
                            {
                                throw;
                            }
                            else
                            {
                                Log.Error(e);
                                msgNeodoslanePolozky = "do DCOM-u sa nepodarilo zaslať bankový účet, a teda aj následné párovanie úhrad.";
                            }
                        }
                    }

                    if (stredisko.Any() || idTBE == (short)TypBiznisEntityEnum.IND)
                    {
                        try
                        {
                            #region Zaslanie kníh interných dokladov do DCOMu

                            UpdateStrediskoDcom(processKey, stredisko, client, ref dcmHeader);

                            #endregion

                            #region Zaslanie interného dokladu do DCOM-u

                            var paymentsRecords = new List<SetPaymentRecord>();
                            var dokladyInd = GetList(
                                Db.From<DokladINDView>()
                                .Select(z => new { z.D_BiznisEntita_Id, z.DM_Suma, z.DatumDokladu, z.Popis, z.C_Stredisko_Id, z.Cislo, z.DCOM })
                                .Where(x => prepojenePolozky.Select(z => z.D_BiznisEntita_Id_Uhrada).Contains(x.D_BiznisEntita_Id)));

                            foreach (var dokladInd in dokladyInd)
                            {
                                var be = biznisEntita.Single(x => x.D_BiznisEntita_Id == dokladInd.D_BiznisEntita_Id);
                                var prvaPolozka = prepojenePolozky.First(x => x.D_BiznisEntita_Id_Uhrada == dokladInd.D_BiznisEntita_Id);
                                var typeOfOperationEnum = idNewState == (int)StavEntityEnum.NOVY ? TypeOfOperationEnum.Delete : (dokladInd.DCOM ? TypeOfOperationEnum.Update : TypeOfOperationEnum.Create);

                                var paymentRecord = new SetPaymentRecord
                                {
                                    amount = dokladInd.DM_Suma,
                                    dateInternalDocumentIssuance = dokladInd.DatumDokladu,
                                    dateInternalDocumentIssuanceSpecified = true,
                                    entryDescriptionBankStatement = dokladInd.Popis,
                                    idInternalDocument = "ES-" + be.D_BiznisEntita_Id,
                                    idInternalDocumentsBook = "ES-" + dokladInd.C_Stredisko_Id,
                                    numberInternalDocument = dokladInd.Cislo,
                                    numberInternalDocumentSpecified = true,
                                    idPerson = prvaPolozka.D_Osoba_Id_Externe.ToString(),
                                    paymentType = PaymentTypeEnum.ID,
                                    payerReference = string.Concat("VS", (prvaPolozka.VS ?? string.Empty).PadLeft(10, '0'), "/SS/KS"),
                                    typeOfOperation = typeOfOperationEnum
                                };

                                var paymentItems = new List<SetPaymentItem>();
                                foreach (var uhrParovanie in prepojenePolozky.Where(x => x.D_BiznisEntita_Id_Uhrada == dokladInd.D_BiznisEntita_Id))
                                {
                                    paymentItems.Add(new SetPaymentItem
                                    {
                                        amount = uhrParovanie.DmCenaRozdiel,
                                        accounted = idNewState == (int)StavEntityEnum.ZAUCTOVANY || idNewState == (int)StavEntityEnum.ZAUCTOVANY_UCT,
                                        idInternalDocumentEntry = uhrParovanie.D_UhradaParovanie_Id.ToString(),
                                        dateOriginalPayment = uhrParovanie.DatumValuta,
                                        dateOriginalPaymentSpecified = uhrParovanie.DatumValuta.HasValue,
                                        idPerson = uhrParovanie.D_Osoba_Id_Externe.ToString(),
                                        taxAssessmentItemID = uhrParovanie.D_VymerPol_Id_Externe.Value,
                                        typeOfOperation = typeOfOperationEnum
                                    });
                                }
                                paymentRecord.paymentItems = paymentItems.ToArray();
                                paymentsRecords.Add(paymentRecord);
                            }

                            if (paymentsRecords.Any())
                            {
                                if (uctZmenaZauctovania)
                                {
                                    LongOperationSetStateMessage(processKey, "Posielam informáciu o zaúčtovaní úhrad do DCOM-u");
                                }
                                else
                                {
                                    var msg = idNewState == (int)StavEntityEnum.SPRACOVANY ? "Posielam interný doklad do DCOM-u." : "Posielam odstránenie interného dokladu do DCOM-u";
                                    LongOperationSetStateMessage(processKey, msg);
                                }

                                client.setPayments(ref dcmHeader, new ReqSetPaymentsList { requestRecords = paymentsRecords.ToArray(), recordCount = paymentsRecords.Count });

                                foreach (var dokladInd in GetList<DokladIND>(x => prepojenePolozky.Select(z => z.D_BiznisEntita_Id_Uhrada).Contains(x.D_DokladIND_Id)))
                                {
                                    dokladInd.DCOM = idNewState != (int)StavEntityEnum.NOVY;
                                    UpdateData(dokladInd);
                                }
                            }
                            #endregion
                        }
                        catch (Exception e)
                        {
                            //Ak to spadne, vrátim chybu "Do DCOM-u sa nepodarilo zaslať pokladnicu, a teda aj následné párovanie úhrad." – vyskočím a vynechám aj krok 3. , vykonám iba 4.
                            if (idNewState == (int)StavEntityEnum.NOVY)
                            {
                                throw;
                            }
                            else
                            {
                                Log.Error(e);
                                msgNeodoslanePolozky = "do DCOM-u sa nepodarilo zaslať stredisko, a teda aj následné párovanie úhrad.";
                            }
                        }
                    }
                }

                #region Zasielanie (za/od)účtovanosti predpisov daní do DCOM

                if (uctZmenaZauctovania && biznisEntita.First().C_TypBiznisEntity_Kniha_Id == (int)TypBiznisEntity_KnihaEnum.Externe_doklady_DaP)
                {
                    using (var danePoplatkyClient = new DanePoplatkyClient())
                    {
                        var dapHeader = new DcomWs.IsoDap.DcmHeader
                        {
                            tenantId = Session.D_Tenant_Id_Externe?.ToString() ?? throw new ArgumentException("nie je zadefinovane D_Tenant_Id_Externe"),
                            isoId = Session.IsoId,
                            requestId = WebEas.Context.Current.CurrentCorrelationID.ToString()
                        };

                        var taxAssessmentRecordStatusRecords = new List<TaxAssessmentRecordStatusRecord>();

                        foreach (var vymerIdExterne in Db.Select<long>("SELECT DISTINCT D_Vymer_Id_Externe FROM uct.V_UctDennik WHERE D_BiznisEntita_Id IN (@ids) AND D_Vymer_Id_Externe IS NOT NULL", new { ids = biznisEntita.Select(z => z.D_BiznisEntita_Id) }))
                        {
                            taxAssessmentRecordStatusRecords.Add(new TaxAssessmentRecordStatusRecord
                            {
                                decisionIDSpecified = true,
                                decisionID = vymerIdExterne,
                                accounted = idNewState == (int)StavEntityEnum.ZAUCTOVANY || idNewState == (int)StavEntityEnum.ZAUCTOVANY_UCT
                            });
                        }

                        if (taxAssessmentRecordStatusRecords.Any())
                        {
                            danePoplatkyClient.setTaxAssessmentsRecordStatus(ref dapHeader, new ReqSetTaxAssessmentsRecordStatusList { requestRecords = taxAssessmentRecordStatusRecords.ToArray(), recordCount = taxAssessmentRecordStatusRecords.Count });
                            foreach (var doklad in GetList<DokladIND>(x => biznisEntita.Select(z => z.D_BiznisEntita_Id).Contains(x.D_DokladIND_Id)))
                            {
                                doklad.DCOM = idNewState == (int)StavEntityEnum.ZAUCTOVANY || idNewState == (int)StavEntityEnum.ZAUCTOVANY_UCT;
                                UpdateData(doklad);
                            }
                        }
                    }
                }

                #endregion
            }

            if (!doposlanieUhrad)
            {
                ZmenaStavuDokladov(biznisEntita, idNewState, processKey);
            }
        }

        public void UpdateStrediskoDcom(string processKey, List<StrediskoView> stredisko, PlatbyClient client, ref DcomWs.IsoPla.DcmHeader dcmHeader, bool delete = false)
        {
            var bookOfInternalAccountingDocuments = new List<SetBookOfInternalAccountingDocumentsRecord>();
            foreach (var str in stredisko)
            {
                bookOfInternalAccountingDocuments.Add(new SetBookOfInternalAccountingDocumentsRecord
                {
                    idIADBook = "ES-" + str.C_Stredisko_Id,
                    numberIADBook = int.Parse(str.Kod),
                    titleIADBook = str.Nazov,
                    yearIADBook = DateTime.Today.Year.ToString(),
                    typeOfOperation = delete ? TypeOfOperationEnum.Delete : (str.DCOM.GetValueOrDefault() ? TypeOfOperationEnum.Update : TypeOfOperationEnum.Create)
                });
            }

            if (bookOfInternalAccountingDocuments.Any())
            {
                LongOperationSetStateMessage(processKey, "Zaslanie nových stredísk/kníh interných dokladov do DCOM-u");
                client.setBooksOfInternalAccountingDocuments(ref dcmHeader, new ReqSetBooksOfInternalAccountingDocumentsList { requestRecords = bookOfInternalAccountingDocuments.ToArray(), recordCount = bookOfInternalAccountingDocuments.Count });

                foreach (var str in GetList<StrediskoCis>(x => stredisko.Select(z => z.C_Stredisko_Id).Contains(x.C_Stredisko_Id)))
                {
                    str.DCOM = true;
                    UpdateData(str);
                }
            }
        }

        public void UpdateBankaUcetDcom(string processKey, List<BankaUcetView> bankaUcet, PlatbyClient client, ref DcomWs.IsoPla.DcmHeader dcmHeader, bool delete = false)
        {
            var bankAccountRecords = new List<SetBankAccountRecord>();
            foreach (var ban in bankaUcet)
            {
                bankAccountRecords.Add(new SetBankAccountRecord
                {
                    idBankAccount = "ES-" + ban.C_BankaUcet_Id,
                    ibanBankAccount = ban.IBAN,
                    currencyBankAccount = ban.MenaKod,
                    titleBankAccount = ban.Nazov,
                    bicBankAccount = ban.BIC,
                    typeOfOperation = delete ? TypeOfOperationEnum.Delete : (ban.DCOM.GetValueOrDefault() ? TypeOfOperationEnum.Update : TypeOfOperationEnum.Create)
                });
            }


            if (bankAccountRecords.Any())
            {
                LongOperationSetStateMessage(processKey, "Zaslanie nových bankových účtov do DCOM-u");
                client.setBankAccounts(ref dcmHeader, new ReqSetBankAccountsList { requestRecords = bankAccountRecords.ToArray(), recordCount = bankAccountRecords.Count });

                foreach (var banucet in GetList<BankaUcetCis>(x => bankaUcet.Select(z => z.C_BankaUcet_Id).Contains(x.C_BankaUcet_Id)))
                {
                    banucet.DCOM = true;
                    UpdateData(banucet);
                }
            }
        }

        public void UpdatePokladnicaDcom(string processKey, List<PokladnicaView> pokladnica, PlatbyClient client, ref DcomWs.IsoPla.DcmHeader dcmHeader, bool delete = false)
        {
            var cashBookRecords = new List<SetCashBookRecord>();
            foreach (var pokl in pokladnica)
            {
                cashBookRecords.Add(new SetCashBookRecord
                {
                    idCashBook = "ES-" + pokl.C_Pokladnica_Id,
                    numberCashBook = int.Parse(pokl.Kod),
                    titleCashBook = pokl.Nazov,
                    currencyCashBook = pokl.MenaKod,
                    yearCashBook = DateTime.Today.Year.ToString(),
                    typeOfOperation = delete ? TypeOfOperationEnum.Delete : (pokl.DCOM.GetValueOrDefault() ? TypeOfOperationEnum.Update : TypeOfOperationEnum.Create)
                });
            }

            if (cashBookRecords.Any())
            {
                LongOperationSetStateMessage(processKey, "Zaslanie nových pokladníc do DCOM-u");
                client.setCashBooks(ref dcmHeader, new ReqSetCashBooksList { requestRecords = cashBookRecords.ToArray(), recordCount = cashBookRecords.Count });

                foreach (var pokl in GetList<Pokladnica>(x => pokladnica.Select(z => z.C_Pokladnica_Id).Contains(x.C_Pokladnica_Id)))
                {
                    pokl.DCOM = true;
                    UpdateData(pokl);
                }
            }
        }

        public bool ZauctujDoklad(ZauctovatDokladDto dokl, string processKey, out string reportId, bool finishOperation = true)
        {
            var biznisEntita = GetList(Db.From<BiznisEntita>().Where(x => Sql.In(x.D_BiznisEntita_Id, dokl.Ids)));
            reportId = null;

            if (!biznisEntita.Any())
            {
                LongOperationSetStateFinished(processKey, string.Empty, "Neboli nájdené žiadne doklady na zaúčtovanie/odúčtovanie.", state: LongOperationState.Done);
                return false;
            }

            if (biznisEntita.Any(
                x =>
                x.C_StavEntity_Id != (int)StavEntityEnum.SPRACOVANY &&
                x.C_StavEntity_Id != (int)StavEntityEnum.ZAUCTOVANY &&
                x.C_StavEntity_Id != (int)StavEntityEnum.ZAUCTOVANY_RZP &&
                x.C_StavEntity_Id != (int)StavEntityEnum.ZAUCTOVANY_UCT))
            {
                throw new WebEasValidationException(null, "Výber obsahuje doklady, ktoré nemajú povolený stav na zaúčtovanie/odúčtovanie. Operáciu nie je možné vykonať!");
            }

            var idOldState = biznisEntita.First().C_StavEntity_Id;
            var idTBE = biznisEntita.First().C_TypBiznisEntity_Id;
            var idNewState = dokl.IdNewState;
            int spracovane = 0, nespracovane = 0;
            bool rzpZauctovanie;
            bool rzpOductovanie;
            bool uctZauctovanie = false;
            bool uctOductovanie = false;

            bool oductovanie;

            rzpZauctovanie = (idOldState == (int)StavEntityEnum.SPRACOVANY && (idNewState == (int)StavEntityEnum.ZAUCTOVANY || idNewState == (int)StavEntityEnum.ZAUCTOVANY_RZP)) ||
                             (idOldState == (int)StavEntityEnum.ZAUCTOVANY_UCT && idNewState == (int)StavEntityEnum.ZAUCTOVANY);

            rzpOductovanie = (idNewState == (int)StavEntityEnum.SPRACOVANY && (idOldState == (int)StavEntityEnum.ZAUCTOVANY || idOldState == (int)StavEntityEnum.ZAUCTOVANY_RZP)) ||
                             (idNewState == (int)StavEntityEnum.ZAUCTOVANY_UCT && idOldState == (int)StavEntityEnum.ZAUCTOVANY);

            if (idTBE == (short)TypBiznisEntityEnum.BAN || idTBE == (short)TypBiznisEntityEnum.PDK || idTBE == (short)TypBiznisEntityEnum.IND ||
                idTBE == (short)TypBiznisEntityEnum.DFA || idTBE == (short)TypBiznisEntityEnum.OFA)
            {
                uctZauctovanie = (idOldState == (int)StavEntityEnum.SPRACOVANY && (idNewState == (int)StavEntityEnum.ZAUCTOVANY || idNewState == (int)StavEntityEnum.ZAUCTOVANY_UCT)) ||
                                 (idOldState == (int)StavEntityEnum.ZAUCTOVANY_RZP && idNewState == (int)StavEntityEnum.ZAUCTOVANY);

                uctOductovanie = (idNewState == (int)StavEntityEnum.SPRACOVANY && (idOldState == (int)StavEntityEnum.ZAUCTOVANY || idOldState == (int)StavEntityEnum.ZAUCTOVANY_UCT)) ||
                                 (idNewState == (int)StavEntityEnum.ZAUCTOVANY_RZP && idOldState == (int)StavEntityEnum.ZAUCTOVANY);
            }

            oductovanie = rzpOductovanie || uctOductovanie;

            var chybneDoklady = SkontrolovatZauctovanieDokladu(idTBE, biznisEntita, idNewState, rzpZauctovanie, rzpOductovanie, uctZauctovanie, uctOductovanie, processKey, out reportId);

            foreach (var be in biznisEntita.ToList())
            {
                if (!chybneDoklady.Any(x => x.D_BiznisEntita_Id == be.D_BiznisEntita_Id))
                {
                    spracovane++;
                }
                else
                {
                    biznisEntita.Remove(be);
                    nespracovane++;
                }
            }


            if ((idTBE == (short)TypBiznisEntityEnum.BAN ||
                 idTBE == (short)TypBiznisEntityEnum.PDK ||
                 idTBE == (short)TypBiznisEntityEnum.IND ||
                 idTBE == (short)TypBiznisEntityEnum.DFA ||
                 idTBE == (short)TypBiznisEntityEnum.OFA) &&
                (idNewState == (int)StavEntityEnum.ZAUCTOVANY ||
                 idNewState == (int)StavEntityEnum.ZAUCTOVANY_UCT))
            {
                bool podlaDatumuPravoplatnosti = false;
                var vymerPolozky = new List<ServiceModel.Office.Types.Dap.VymerPolViewHelper>();

                var uctovneDenniky = GetList(Db
                    .From<UctDennik>()
                    .Select(x => new { x.D_UctDennik_Id, x.DatumUctovania, x.D_BiznisEntita_Id, x.D_VymerPol_Id })
                    .Where(x => Sql.In(x.D_BiznisEntita_Id, biznisEntita.Select(x => x.D_BiznisEntita_Id))).And(Filter.NotDeleted().ToString()));

                if (uctovneDenniky.Any(x => x.D_VymerPol_Id.HasValue))
                {
                    podlaDatumuPravoplatnosti = GetTypBiznisEntityNastav(TypBiznisEntityEnum.DAP, LokalitaEnum.TU) == "DatumPravoplatnosti";
                    vymerPolozky = GetList(Db
                                .From<ServiceModel.Office.Types.Dap.VymerPolViewHelper>()
                                .Where(x => Sql.In(x.D_VymerPol_Id, uctovneDenniky.Where(x => x.D_VymerPol_Id.HasValue).Select(x => x.D_VymerPol_Id)))
                                .Select(x => new { x.D_VymerPol_Id, x.DatumPravoplatnosti, x.DatumVyrubenia }));
                }

                foreach (var uctDen in uctovneDenniky)
                {
                    ServiceModel.Office.Types.Dap.VymerPolViewHelper vymerPol = null;
                    if (uctDen.D_VymerPol_Id.HasValue)
                    {
                        vymerPol = vymerPolozky.Single(x => x.D_VymerPol_Id == uctDen.D_VymerPol_Id);
                    }

                    uctDen.DatumUctovania = vymerPol != null ? (podlaDatumuPravoplatnosti ? (vymerPol.DatumPravoplatnosti ?? biznisEntita.Single(x => x.D_BiznisEntita_Id == uctDen.D_BiznisEntita_Id).DatumDokladu) : vymerPol.DatumVyrubenia) : biznisEntita.Single(x => x.D_BiznisEntita_Id == uctDen.D_BiznisEntita_Id).DatumDokladu;
                }

                UpdateAllData(uctovneDenniky);
            }

            if (biznisEntita.Any())
            {
                SpracovatZauctovatDoklad(biznisEntita, idNewState, uctZauctovanie, uctOductovanie, processKey, out string msgNeodoslanePolozky);
            }

            string msg;
            string op = oductovanie ? "odúčtova" : "zaúčtova";
            bool allOK;

            allOK = nespracovane == 0;
            if (spracovane > 0 && nespracovane > 0)
            {
                msg = $"Výber obsahuje doklady, ktoré sa nepodarilo {op}ť";
            }
            else
            {
                msg = $"{(spracovane > 1 || nespracovane > 1 ? "Doklady" : "Doklad") } {(spracovane == 1 ? $"bol úspešne {op}ný" : (spracovane > 1 ? $"boli úspešne {op}né" : $"sa nepodarilo {op}ť"))}";
            }

            if (finishOperation)
            {
                LongOperationSetStateFinished(processKey, string.Empty, msg, state: LongOperationState.Done, reportId: reportId);
            }
            else
            {
                LongOperationSetStateMessage(processKey, msg, reportId: reportId);
            }

            return allOK;
        }
        */

        public static void CopyProperties(object fromObject, object toObject)
        {
            PropertyInfo[] toObjectProperties = toObject.GetType().GetProperties();
            foreach (PropertyInfo propTo in toObjectProperties)
            {
                PropertyInfo propFrom = fromObject.GetType().GetProperty(propTo.Name);
                if (propFrom != null && propFrom.CanWrite && propTo.CanWrite)
                {
                    // Debug.WriteLine("-> " + propFrom.Name);
                    propTo.SetValue(toObject, propFrom.GetValue(fromObject, null), null);
                }
            }
        }

        #endregion

        #region GetRowDefaultValues

        public override object GetRowDefaultValues(string code, string masterCode, string masterRowId)
        {
            //Odkomentovať keď to chcem použiť
            var root = RenderModuleRootNode(code);
            var node = root.TryFindNode(code);
            //HierarchyNode masternode = null;
            //if (!masterCode.IsNullOrEmpty()) //Používať iba ak je modul z code a mastercode rovnaký
            //{
            //    masternode = root.TryFindNode(masterCode);
            //}

            #region BiznisEntita pre IND - rzp aj uct
            if (node != null && (node.ModelType == typeof(DokladINDView)))
            {
                int? firstPredkont = null;
                int kniha;

                if (code == "uct-evi-exd-dap") kniha = (int)TypBiznisEntity_KnihaEnum.Externe_doklady_DaP;
                else if (code == "uct-evi-exd-mjt") kniha = (int)TypBiznisEntity_KnihaEnum.Externe_doklady_majetok;
                else if (code == "uct-evi-exd-mzd") kniha = (int)TypBiznisEntity_KnihaEnum.Externe_doklady_mzdy;
                else if (code == "uct-evi-exd-skl") kniha = (int)TypBiznisEntity_KnihaEnum.Externe_doklady_sklad;
                else kniha = (int)TypBiznisEntity_KnihaEnum.Interne_doklady;

                //Nastavenie defaultnej predkontácie
                firstPredkont = Db.Single<int>(Db.From<PredkontaciaCombo>().Select(x => new { x.C_Predkontacia_Id }).
                    Where(x => x.C_TypBiznisEntity_Kniha_Id == kniha).OrderBy(x => x.Poradie).Take(1));

                return new
                {
                    D_User_Id_DokladVyhotovil = Session.UserIdGuid,
                    C_Predkontacia_Id = firstPredkont
                };
            }
            #endregion

            return base.GetRowDefaultValues(code, masterCode, masterRowId);
        }

        #endregion

        #region Reports

        /// <summary>
        /// Renders the multi PDF.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public void RenderMultiPdf(string processKey, RendererFormatType format, List<IReportData> data, string nazovSuboru = "")
        {
            Hashtable deviceInfo = null;
            if (data == null || data.Count == 0)
            {
                throw new ArgumentNullException("Report data cannot be empty");
            }

            using var book = new Telerik.Reporting.ReportBook();
            int i = 0;
            do
            {
                IReportData item = data[i];
                Type t = item.GetType();
                var list = new List<IReportData> { item };

                while (((i + 1) < data.Count) && (t == data[i + 1].GetType()))
                {
                    i++;
                    list.Add(data[i]);
                }
                i++;
                var rpts = GetReport(t, list);
                foreach (var rpt in rpts)
                {
                    var irs = new Telerik.Reporting.InstanceReportSource
                    {
                        ReportDocument = rpt.TelerikReport
                    };
                    // book.DocumentName = rpt.DocumentName; JP: mozme pouzit v buducnosti ak by nam chybalo meno
                    book.ReportSources.Add(irs);

                    deviceInfo = new Hashtable();
                    if (rpts.Count > 0)
                    {
                        if (rpts[0] is IReportInfo info)
                        {
                            var sAuthor = info.DocumentAuthor;
                            string sDocName;
                            if (sAuthor.IsNullOrEmpty()) sAuthor = Session.TenantName;
                            deviceInfo.Add("DocumentAuthor", RemoveDiacritics(sAuthor));
                            deviceInfo.Add("DocumentSubject", RemoveDiacritics(info.DocumentSubject));
                            deviceInfo.Add("DocumentKeywords", RemoveDiacritics(info.DocumentKeywords));
                            if (format.ToString() == "Csv")
                            {
                                deviceInfo.Add("WriteBOM", true);
                                deviceInfo.Add("NoHeader", true);
                            }
                            if (nazovSuboru != "")
                            {
                                sDocName = nazovSuboru;
                            }
                            else if (book.ReportSources.Count == 1)
                            {
                                sDocName = info.DocumentTitle;
                            }
                            else
                            {
                                sDocName = "Hromadná tlač";
                            }
                            deviceInfo.Add("DocumentTitle", RemoveDiacritics(sDocName));
                            book.DocumentName = RemoveDiacritics(sDocName).Replace(" ", "_").Replace("?", "_") + DateTime.Now.ToString("_yyyyMMdd_HHmm");
                            // deviceInfo.Add("ComplianceLevel", "PDF/A-1b");
                        }
                    }
                }
            }
            while (i < data.Count);

            var result = Render(format, book, deviceInfo);
            var reportId = Guid.NewGuid().ToString();
            SetToCache(string.Concat("Report:", reportId), result, new TimeSpan(8, 0, 0), useGzipCompression: true);
            LongOperationSetStateFinished(processKey, string.Empty, $"Zostava '{book.DocumentName}' bola úspešne vygenerovaná", state: LongOperationState.Done, reportId: reportId);
        }

        public string RemoveDiacritics(string sText)
        {
            if (sText is null) return null;
            return Encoding.ASCII.GetString(Encoding.GetEncoding(1251).GetBytes(sText)); // JP: nasa 1250 by to neodstranila
        }

        // chceli sme pouzit na diakritiku v PDF properties ale nezabralo, mozme pouzit v buducnosti pre .FDF sablony
        private string ToPdfString(string sCo)
        {
            string sInStr, sTmpStr = default;
            sInStr = @"()\<>‰€ÁáÄäČčĎďÉéĚěËëÍíĹĺĽľŇňÓóÔôÖöŐőŔŕŘřßŠšŤťÚúÜüŮůŰűÝýŽž¦§«»µ×÷–—‘’‚""¨´°";

            List<string> mapTable = new List<string>() { @"\(", @"\)", @"\\", @"\210", @"\211", @"\213", @"\240", @"\301", @"\341", @"\304", @"\344", @"\225", @"\226", @"\230", @"\232", @"\311", @"\351", @"\233", @"\234", @"\313", @"\353", @"\315", @"\355", @"\241", @"\242", @"\245", @"\251", @"\252", @"\254", @"\323", @"\363", @"\324", @"\364", @"\326", @"\366", @"\266", @"\271", @"\203", @"\261", @"\277", @"\272", @"\337", @"\227", @"\235", @"\222", @"\243", @"\332", @"\372", @"\334", @"\374", @"\262", @"\263", @"\201", @"\202", @"\335", @"\375", @"\231", @"\236", @"\246", @"\247", @"\253", @"\273", @"\265", @"\327", @"\267", @"\205", @"\204", @"\217", @"\220", @"\221", @"\215", @"\216", @"\250", @"\264", @"\260" };

            for (int i = 0; i < sCo.Length; i++)
            {
                int j = sInStr.IndexOf(sCo[i]);
                if (j >= 0)
                {
                    sTmpStr += mapTable[j];
                }
                else
                {
                    sTmpStr += sCo[i];
                }
            }
            return sTmpStr;
        }

        /// <summary>
        /// Renders the excel.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public RendererResult RenderExcel(LayoutExportData data)
        {
            if (data == null)
            {
                data = new LayoutExportData
                {
                    CityName = "Litava",
                    UserName = "Starosta Litava",
                    CityLogo = null,
                    Id = 1,
                    DocumentTitle = "Export dokumentu",
                    Items = new List<LayoutExportData.LayoutExportItem>
                        {
                            new LayoutExportData.LayoutExportItem
                            {
                                Name = "eDemokracia > Evidencia > Pripomienky > Všetky pripomienky",
                                FilterText = "FilterText",
                                Filter = " aaa>0 <a súčasne> bbb=1",
                                ViewName = "Standardny Pohlad",
                                ColumnData = new List<LayoutExportData.LayoutExportColumnData>
                                {
                                    new LayoutExportData.LayoutExportColumnData
                                    {
                                        Caption = "Iden",
                                        ColumnName = "Id",
                                        Width = 10
                                    }, new LayoutExportData.LayoutExportColumnData
                                       {
                                           Caption = "Nazov",
                                           ColumnName = "Name",
                                           Width = 255
                                       },
                                       new LayoutExportData.LayoutExportColumnData
                                       {
                                           Caption = "Pozn",
                                           ColumnName = "Poznamka",
                                           Width = 255
                                       }
                                },
                                PageNumber = "10/20",
                                Data = new List<Dictionary<string, object>>
                                {
                                    new Dictionary<string, object>
                                    {
                                        { "Name", "Text" }, { "Poznamka", "Ahoj" }, { "Id", 10 }
                                    },

                                    //new { Id = 15, Name = "Text 2", Poznamka = " s 52" }, new { Id = 35, Name = "Text 4", Poznamka = "dsf 55" }
                                }
                            }
                        }
                };
            }

            var result = new RendererResult
            {
                DocumentName = data.DocumentTitle ?? "Export",
                Encoding = Encoding.Unicode.WebName,
                Extension = "xlsx",
                MimeType = "application/vnd.ms-excel"
            };

            using (var wb = new XLWorkbook(XLEventTracking.Disabled))
            {
                IXLStyle defaultStyle = wb.Style;
                defaultStyle.Font.FontSize = 12;
                defaultStyle.Font.FontName = "Calibri";

                int itemsCount = 0;

                if (data.Items != null)
                {
                    foreach (LayoutExportData.LayoutExportItem item in data.Items)
                    {
                        itemsCount++;
                        if (string.IsNullOrEmpty(item.ViewName))
                        {
                            item.ViewName = $"Sheet {itemsCount}";
                        }
                        if (wb.Worksheets.Any(nav => nav.Name == item.ViewName))
                        {
                            string newName;
                            int cnt = 1;
                            do
                            {
                                cnt++;
                                newName = $"{item.ViewName} {cnt}";
                            }
                            while (wb.Worksheets.Any(nav => nav.Name == newName));
                            item.ViewName = newName;
                        }
                        IXLWorksheet sheet = wb.Worksheets.Add(item.ViewName);
                        sheet.Author = data.UserName;
                        sheet.Row(1).Height = 38;
                        sheet.Cell(1, 1).Value = item.ViewName;
                        sheet.Range(1, 1, 1, item.ColumnData.Count).Merge().AddToNamed("Titles");
                        sheet.SheetView.FreezeRows(1);

                        int rowNumber = 2;
                        sheet.Cell(rowNumber, 1).Value = $"Názov obce: {data.CityName}";
                        sheet.Range(rowNumber, 1, 2, item.ColumnData.Count).Merge().AddToNamed("City");

                        if (!string.IsNullOrEmpty(item.Name))
                        {
                            rowNumber++;
                            sheet.Cell(rowNumber, 1).Value = $"Názov položky v strome: {item.Name}";
                            sheet.Range(rowNumber, 1, 3, item.ColumnData.Count).Merge().AddToNamed("ViewName");
                        }

                        if (!string.IsNullOrEmpty(item.FilterText))
                        {
                            rowNumber++;
                            sheet.Cell(rowNumber, 1).Value = $"Aktívne filtre: {item.FilterText}";
                            sheet.Range(rowNumber, 1, 4, item.ColumnData.Count).Merge().AddToNamed("FilterText");
                        }


                        int count = 0;
                        int row = 0;
                        rowNumber += 2;
                        if (item.ColumnData == null)
                        {
                            throw new ArgumentNullException("Stĺpce nie sú definované");
                        }
                        foreach (LayoutExportData.LayoutExportColumnData col in item.ColumnData.Where(x => x.ColumnName != null))
                        {
                            row = rowNumber;
                            IXLCell cell = sheet.Cell(row++, ++count);
                            cell.Value = col.Caption;
                            sheet.Column(count).Width = col.Width;
                            cell.AddToNamed("Header");
                            if (item.Data != null)
                            {
                                foreach (Dictionary<string, object> rowData in item.Data)
                                {
                                    cell = sheet.Cell(row++, count);
                                    if (row % 2 == 0)
                                    {
                                        //cell.AddToNamed("DataPair");
                                        cell.Style.Font.FontSize = 12;
                                        cell.Style.Font.FontName = "Calibri";
                                        cell.Style.Fill.BackgroundColor = XLColor.WhiteSmoke;
                                    }
                                    else
                                    {
                                        //cell.AddToNamed("DataClean");
                                        cell.Style.Font.FontSize = 12;
                                        cell.Style.Font.FontName = "Calibri";
                                        cell.Style.Fill.BackgroundColor = XLColor.White;
                                    }

                                    if (rowData.ContainsKey(col.ColumnName))
                                    {
                                        switch (col.Align)
                                        {
                                            case LayoutExportData.LayoutExportColumnData.AligmentType.Left:
                                                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                                                break;
                                            case LayoutExportData.LayoutExportColumnData.AligmentType.Right:
                                                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                                                break;
                                            case LayoutExportData.LayoutExportColumnData.AligmentType.Center:
                                                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                                break;
                                            case LayoutExportData.LayoutExportColumnData.AligmentType.Justify:
                                                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Justify;
                                                break;
                                        }

                                        switch (col.ColType)
                                        {
                                            case LayoutExportData.LayoutExportColumnData.DataType.Boolean:

                                                cell.DataType = XLDataType.Boolean;
                                                if (rowData[col.ColumnName] != null && rowData[col.ColumnName] is string)
                                                {
                                                    if (bool.TryParse((string)rowData[col.ColumnName], out bool val))
                                                    {
                                                        cell.Value = val;
                                                    }
                                                    else
                                                    {
                                                        cell.Value = rowData[col.ColumnName];
                                                    }
                                                }
                                                else
                                                {
                                                    cell.Value = rowData[col.ColumnName];
                                                }
                                                break;
                                            case LayoutExportData.LayoutExportColumnData.DataType.Date:
                                                cell.DataType = XLDataType.DateTime;
                                                cell.Style.DateFormat.SetFormat("dd.MM.yyyy");
                                                if (rowData[col.ColumnName] != null && rowData[col.ColumnName] is string)
                                                {
                                                    DateTime val;
                                                    if (DateTime.TryParse((string)rowData[col.ColumnName], out val))
                                                    {
                                                        cell.Value = val;
                                                    }
                                                    else
                                                    {
                                                        cell.Value = rowData[col.ColumnName];
                                                    }
                                                }
                                                else
                                                {
                                                    cell.Value = rowData[col.ColumnName];
                                                }
                                                break;
                                            case LayoutExportData.LayoutExportColumnData.DataType.DateTime:
                                                cell.DataType = XLDataType.DateTime;
                                                cell.Style.DateFormat.SetFormat("dd.MM.yyyy HH:mm");
                                                if (rowData[col.ColumnName] != null && rowData[col.ColumnName] is string)
                                                {
                                                    DateTime val;
                                                    if (DateTime.TryParse((string)rowData[col.ColumnName], out val))
                                                    {
                                                        cell.Value = val;
                                                    }
                                                    else
                                                    {
                                                        cell.Value = rowData[col.ColumnName];
                                                    }
                                                }
                                                else
                                                {
                                                    cell.Value = rowData[col.ColumnName];
                                                }
                                                break;
                                            case LayoutExportData.LayoutExportColumnData.DataType.Time:
                                                cell.DataType = XLDataType.DateTime;
                                                cell.Style.DateFormat.SetFormat("HH:mm:ss");
                                                if (rowData[col.ColumnName] != null && rowData[col.ColumnName] is string)
                                                {
                                                    if (DateTime.TryParse((string)rowData[col.ColumnName], out DateTime val))
                                                    {
                                                        cell.Value = val;
                                                    }
                                                    else
                                                    {
                                                        cell.Value = rowData[col.ColumnName];
                                                    }
                                                }
                                                else
                                                {
                                                    cell.Value = rowData[col.ColumnName];
                                                }
                                                break;
                                            case LayoutExportData.LayoutExportColumnData.DataType.Number:
                                                cell.DataType = XLDataType.Number;
                                                if (rowData[col.ColumnName] != null && rowData[col.ColumnName] is string)
                                                {
                                                    var cdata = rowData[col.ColumnName] as string;
                                                    if (cdata.Contains('.') || cdata.Contains(','))
                                                    {

                                                        if (CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.Equals(","))
                                                        {
                                                            cdata = cdata.Replace('.', ',');
                                                        }
                                                        else
                                                        {
                                                            cdata = cdata.Replace(',', '.');
                                                        }

                                                        if (double.TryParse(cdata, out double output))
                                                        {
                                                            cell.Value = output;
                                                        }
                                                        else
                                                        {
                                                            cell.Value = rowData[col.ColumnName];
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (long.TryParse(cdata, out long val))
                                                        {
                                                            cell.Value = val;
                                                        }
                                                        else
                                                        {
                                                            cell.Value = rowData[col.ColumnName];
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    cell.Value = rowData[col.ColumnName];
                                                }
                                                break;
                                            default:
                                                if (long.TryParse(rowData[col.ColumnName].ToString(), out long num))
                                                {
                                                    cell.Value = num;
                                                    cell.DataType = XLDataType.Number;
                                                    cell.Style.NumberFormat.Format = "0";
                                                }
                                                else
                                                    cell.Value = rowData[col.ColumnName];
                                                break;
                                        }
                                    }
                                }
                            }
                        }

                        IXLCell cel = sheet.Cell(++row, 1);
                        cel.Value = $"Dátum vytvorenia: {DateTime.Now:dd.MM.yyyy HH:mm}";
                        cel = sheet.Cell(row, count);
                        cel.Value = $"Vytvoril: {data.UserName}";

                        // Format all titles in one shot
                        IXLStyle titleStyle = wb.NamedRanges.NamedRange("Titles").Ranges.Style;

                        titleStyle.Font.Bold = true;
                        titleStyle.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        titleStyle.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        titleStyle.Font.FontSize = 18;
                        titleStyle.Font.FontName = "Arial";

                        IXLStyle headerStyle = wb.NamedRanges.NamedRange("Header").Ranges.Style;

                        headerStyle.Font.Bold = true;
                        headerStyle.Font.FontSize = 10;
                        headerStyle.Font.FontName = "Arial";
                        headerStyle.Fill.BackgroundColor = XLColor.SkyBlue;

                        IXLNamedRange dataClean = wb.NamedRanges.NamedRange("DataClean");
                        if (dataClean != null)
                        {
                            IXLStyle dataStyle = dataClean.Ranges.Style;

                            dataStyle.Font.FontSize = 12;
                            dataStyle.Font.FontName = "Calibri";
                            dataStyle.Fill.BackgroundColor = XLColor.White;
                        }
                        IXLNamedRange dataPair = wb.NamedRanges.NamedRange("DataPair");
                        if (dataPair != null)
                        {
                            IXLStyle dataPairStyle = dataPair.Ranges.Style;

                            dataPairStyle.Font.FontSize = 12;
                            dataPairStyle.Font.FontName = "Calibri";
                            dataPairStyle.Fill.BackgroundColor = XLColor.WhiteSmoke;
                        }
                        sheet.Columns(1, item.ColumnData.Count).AdjustToContents(5, item.Data == null ? 5 : (item.Data.Count + 5));
                    }
                }

                using (var memStream = new MemoryStream())
                {
                    wb.SaveAs(memStream);
                    result.DocumentBytes = memStream.ToArray();
                }
            }

            return result;
        }

        public void RenderCsv(string processKey, List<KeyValuePair<string, string>> listFileData, Encoding enc = null)
        {
            var reportIds = new List<string>();

            foreach (var item in listFileData)
            {
                var result = new RendererResult
                {
                    DocumentName = item.Key ?? "Export",
                    Encoding = Encoding.Unicode.WebName,
                    Extension = "csv",
                    MimeType = "application/csv"
                };
                enc ??= Encoding.Default;
                result.DocumentBytes = enc.GetBytes(item.Value);
                var reportId = Guid.NewGuid().ToString();
                SetToCache(string.Concat("Report:", reportId), result, new TimeSpan(8, 0, 0), useGzipCompression: true);
                reportIds.Add(reportId);
            }
            var multiple = listFileData.Count() > 1;
            LongOperationSetStateFinished(processKey, string.Empty, $"Csv súbor{(multiple ? "y" : string.Empty)} '{listFileData.Select(x => x.Key + ".csv").Join(", ")}' bol{(multiple ? "i" : string.Empty)} úspešne vygenerovan{(multiple ? "é" : "ý")}", state: LongOperationState.Done, reportId: reportIds.Join(","));
        }

        public virtual List<EsamReport> GetReport<T>(Type type, List<T> data) where T : IReportData
        {
            var ret = new List<EsamReport>();

            if (type == typeof(ZostavaUctDoklad))
            {
                var rpt = GetTelerikReport(ReportsEnum.UctDokladReport);
                rpt.SetDocumentProperties("Účtovný doklad", "účet, doklad");
                var ds = data.OfType<ZostavaUctDoklad>().First();
                rpt.TelerikReport.DataSource = ds;

                //SubReport subRpt = rpt.TelerikReport.Items.Find("subReport1", true).First() as SubReport;
                //var x = subRpt.ReportSource as UriReportSource;
                //x.Uri = "bin/Reports/" + x.Uri;
                ret.Add(rpt);
            }

            if (ret.Count == 0) // len nech zahuci pokial nemas zadefinovany report
            {
                throw new NotImplementedException(string.Format("Type {0} is not implemented", type));
            }
            return ret;
        }

        public List<IReportData> PrepareReportUctDoklad(ReportKnihaDto rptParams)
        {
            var rptData = new List<IReportData>();
            var zostava = new ZostavaUctDoklad();
            zostava.Hlavicky = new List<ZostavaUctDokladHla>();
            RptSetOwner(zostava);

            zostava.ViacZaznamov = (rptParams.Ids.Count() > 1);
            foreach (var id in rptParams.Ids)
            {
                // musim si zavolat info z BE lebo ak by nemali riadky tak nevytvorim ani hlavicku a tu cheme v kazdom pripade zobrazit
                var be = GetById<BiznisEntitaView>(id);
                // hlavicka
                var h = new ZostavaUctDokladHla();
                h.Datum = be.DatumDokladu;
                h.DokladCaption = be.TypBiznisEntityNazov;
                h.StrediskoCaption = zostava.StrediskoCaption;
                h.ViacZaznamov = zostava.ViacZaznamov;
                h.Doklad = be.BiznisEntitaPopis;
                h.Dodavatel = be.FormatMenoSort;
                h.Stredisko = be.StrediskoNazov;
                h.Projekt = be.ProjektNazov;
                h.Ucel = be.Popis;
                h.Suma = be.DM_Suma;
                switch (be.C_TypBiznisEntity_Id)
                {
                    case (int)TypBiznisEntityEnum.DFA:
                    case (int)TypBiznisEntityEnum.DOB:
                    case (int)TypBiznisEntityEnum.DZM:
                    case (int)TypBiznisEntityEnum.DZF:
                        h.DodavatelCaption = "Dodávateľ";
                        break;
                    case (int)TypBiznisEntityEnum.OFA:
                    case (int)TypBiznisEntityEnum.OOB:
                    case (int)TypBiznisEntityEnum.OZM:
                    case (int)TypBiznisEntityEnum.OZF:
                        h.DodavatelCaption = "Dodávateľ";
                        break;
                    default:  // BAN, PDK, IND
                        h.DodavatelCaption = "Meno/Názov";
                        h.NoDataMsg = "  (nezaúčtovaný)";
                        break;
                }
                h.UctPolozky = new List<ZostavaUctDennikPol>();
                h.RzpPolozky = new List<ZostavaRzpDennikPol>();
                // Uct polozky
                var filter = new Filter();
                filter.AndEq(nameof(UctDennikRptHelper.D_BiznisEntita_Id), id);
                filter.AndEq(nameof(UctDennikRptHelper.U), true); // + Zauctovane
                var data1 = GetList<UctDennikRptHelper>(filter);
                foreach (var row in data1)
                {
                    var r = new ZostavaUctDennikPol()
                    {
                        Suv = row.D_UctDennik_Id < 0,
                        VS = row.VS,
                        DatumUctovania = row.DatumUctovania,
                        BiznisEntitaPopis = row.BiznisEntitaPopis,
                        Poradie = row.Poradie,
                        RozvrhUcet = row.RozvrhUcet,
                        SumaMD = row.SumaMD,
                        SumaDal = row.D_UctDennik_Id < 0 ? null : (decimal?)row.SumaDal,
                        Popis = row.Popis,
                        StrediskoNazov = row.StrediskoNazov,
                        ProjektNazov = row.ProjektNazov
                    };
                    h.UctPolozky.Add(r);
                };
                // Rzp polozky
                filter = new Filter();
                filter.AndEq(nameof(UctDennikRptHelper.D_BiznisEntita_Id), id);
                filter.AndEq(nameof(RzpDennikViewHelper.R), true); // + Zauctovane
                var data2 = GetList<RzpDennikViewHelper>(filter);
                foreach (var row in data2)
                {
                    var r = new ZostavaRzpDennikPol()
                    {
                        Suv = row.D_RzpDennik_Id < 0,
                        DatumUctovania = row.DatumDokladu,
                        BiznisEntitaPopis = row.BiznisEntitaPopis,
                        Poradie = row.Poradie,
                        PV = row.PrijemVydajText,
                        ZD = row.ZdrojKod,
                        FK = row.FK,
                        EK = row.EK,
                        A1 = row.A1,
                        A2 = row.A2,
                        A3 = row.A3,
                        NazovPolozky = row.RzpPolNazov,
                        ProgramFull = row.ProgramFull,
                        Suma = row.D_RzpDennik_Id < 0 ? null : (decimal?)row.Suma,
                        Popis = row.Popis,
                        StrediskoNazov = row.StrediskoNazov,
                        ProjektNazov = row.ProjektNazov
                    };
                    h.RzpPolozky.Add(r);
                };
                // finalize
                zostava.Hlavicky.Add(h);
            }

            rptData.Add(zostava);

            return rptData;
        }

        public EsamReport GetTelerikReport(ReportsEnum report)
        {
            //zatial takto, neskor budeme tahat z DMS
            var reportPackager = new Telerik.Reporting.ReportPackager();
            using var sourceStream = System.IO.File.OpenRead(System.Web.Hosting.HostingEnvironment.MapPath("~/bin/Reports/" + report.ToString() + ".trdp"));
            return new EsamReport((Telerik.Reporting.Report)reportPackager.UnpackageDocument(sourceStream));
        }

        /// <summary>
        /// Renders the specified format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        internal RendererResult Render(RendererFormatType format, Telerik.Reporting.IReportDocument report, Hashtable deviceInfo = null)
        {
            var reportProcessor = new Telerik.Reporting.Processing.ReportProcessor();
            var reportSource = new Telerik.Reporting.InstanceReportSource
            {
                ReportDocument = report
            };

            if (deviceInfo == null)
            {
                deviceInfo = new Hashtable();
            }

            if (!deviceInfo.ContainsKey("DocumentAuthor"))
            {
                deviceInfo.Add("DocumentAuthor", RemoveDiacritics(Session.TenantName));
            }
            if (!deviceInfo.ContainsKey("DocumentCreator"))
            {
                deviceInfo.Add("DocumentCreator", RemoveDiacritics(Session.DisplayName)); // treba doriesit diakritiku
            }
            if (!deviceInfo.ContainsKey("DocumentProducer"))
            {
                deviceInfo.Add("DocumentProducer", "eSAM");
            }

            // JP: deviceInfo nesmie obsahovat hodnotu s NULL parametrom inac to padne
            var output = reportProcessor.RenderReport(format.ToString().ToUpper(), reportSource, deviceInfo);
            var documentBytes = output.DocumentBytes;

            var result = new RendererResult
            {
                DocumentBytes = documentBytes,
                DocumentName = output.DocumentName,
                Encoding = output.Encoding?.WebName,
                Errors = output.Errors,
                Extension = output.Extension,
                MimeType = output.MimeType
            };

            return result;
        }

        /// <summary>
        /// Rozparsuje text oddeleny '&' a vrati nam list
        /// </summary>
        /// <param name="data">String na parsovanie</param>
        /// <returns></returns>
        public List<ZostavaFilter> RptSetFiltre(string data, bool toOneLine)
        {
            var flt = new List<ZostavaFilter>();
            ZostavaFilter fp;

            if (!data.IsNullOrEmpty())
            {
                if (toOneLine)
                {
                    string sTmp = data.Replace("&", "    ");
                    fp = new ZostavaFilter();
                    fp.Filter = sTmp.Trim();
                    flt.Add(fp);
                }
                else
                {
                    string[] ar = data.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < ar.Length; i++)
                    {
                        fp = new ZostavaFilter();
                        fp.Filter = ar[i].Trim();
                        flt.Add(fp);
                    }
                }
            }
            return flt;
        }

        public void RptSetOwner(RptHead zostava)
        {
            TenantSimpleView ti = GetList(Db.From<TenantSimpleView>().Select().Where(tu => tu.D_Tenant_Id == Session.TenantIdGuid)).FirstOrDefault();
            zostava.Nazov = ti.MenoObchodne;
            zostava.ICO = ti.ICO;
            zostava.Obec = ti.AdresaObec;
            zostava.Ulica = ti.AdresaUlicaCislo;
            zostava.OrganizaciaTyp_Id = ti.C_OrganizaciaTyp_Id;
            zostava.OrganizaciaTyp = ti.OrganizaciaTypNazov;
            zostava.PSC = ti.AdresaPSC;
            zostava.Vytlacil = Session.DisplayName;
            zostava.StrediskoCaption = GetNastavenieS("reg", "OrjNazovJC");
        }

        public void SetRptText(string textBox, string sText, Telerik.Reporting.Report rpt)
        {
            var txtObj = rpt.Items.Find(textBox, true);
            if (txtObj.Any())
            {
                (txtObj.First() as Telerik.Reporting.TextBox).Value = sText;
            }
        }

        #endregion

        #region SetPredkontacia
        public void SetPredkontacia()
        {
            List<PredkontaciaCis> data = new List<PredkontaciaCis>();
            try
            {

                var reader = Db.ExecuteReader($@"SELECT DISTINCT k.SkupinaPredkont_Id
                                                    FROM reg.V_TypBiznisEntity_Kniha k
                                                             JOIN reg.V_TypBiznisEntityNastav tben ON tben.C_TypBiznisEntity_Id = k.C_TypBiznisEntity_Id
                                                        LEFT JOIN uct.C_Predkontacia p ON p.SkupinaPredkont_Id = k.SkupinaPredkont_Id
                                                    WHERE tben.EvidenciaSystem = 1 AND tben.Uctovany = 1 AND k.SkupinaPredkont_Id IS NOT NULL AND p.SkupinaPredkont_Id IS NULL");

                data = reader.Parse<PredkontaciaCis>().ToList();
                reader.Close();
            }
            catch (Exception ex)
            {
                throw new WebEasException("Nastala chyba pri volaní SetPredkontacia - identifikácia chýbajúcich základných predkontácií", ex.Message, ex);
            }

            if (data.Count > 0)
            {
                using var transaction = BeginTransaction();
                try
                {
                    foreach (var d in data)
                    {
                        var zaklNazov = d.SkupinaPredkont_Id switch
                        {
                            (int)SkupinaPredkontEnum.Bankove_vypisy => "Predkontácia úhrad",
                            (int)SkupinaPredkontEnum.ExtDoklady_DaP => "Predkontácia predpisu",
                            _ => "Základná predkontácia",
                        };
                        string sql = $@"INSERT INTO uct.C_Predkontacia (D_Tenant_Id, SkupinaPredkont_Id, Nazov, Poradie)
                                            VALUES ('{Session.TenantIdGuid}', {d.SkupinaPredkont_Id}, '{zaklNazov}', 1)";
                        Db.ExecuteSql(sql);
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new WebEasException("Nastala chyba pri volaní SetPredkontacia - pridávanie základných predkontácií", ex.Message, ex);
                }
            }
        }
        #endregion

        #region SetCislovanie
        public void SetCislovanie()
        {
            const string interneCislo = "[RR].{MM}/<C,4>"; // Vuyžívané pre VBÚ (Kód VBÚ odporúčame zadávať manuálne), IND
            const string interneCisloORJ = "[ORJ2]-[RR].{MM}/<C,4>";
            const string interneCisloPOK = "[POK2]-[RR].{MM}/<C,4>";
            const string vsmask = "[RR]{MM}<C,4>";

            this.RemoveFromCacheByRegexOptimizedTenant("GetCislovanieDefinition:.*");

            DateTime now = DateTime.Now;
            List<CislovanieCis> source;
            List<CislovanieCis> mergedData;
            List<CislovanieCis> destination = new List<CislovanieCis>();

            var reader = Db.ExecuteReader($@"SELECT DISTINCT n.D_Tenant_Id, n.CislovanieJedno,  k.C_TypBiznisEntity_Kniha_Id, s.C_Stredisko_Id, b.C_BankaUcet_Id, p.C_Pokladnica_Id,
                                                    CASE WHEN n.KodORS = 'VBU' OR n.C_TypBiznisEntity_Id = 1 THEN '{interneCislo}'
                                                         WHEN n.KodORS = 'ORJ' THEN '{interneCisloORJ}' 
                                                         WHEN n.KodORS = 'POK' THEN '{interneCisloPOK}' ELSE '' END AS CisloInterneMask,
                                                    CASE WHEN KodDokladu IN ('OFA', 'OZF') THEN '{vsmask}' END as VSMask, null AS Popis, 
                                                    COALESCE((SELECT DATEADD(dd, 1, MAX(PlatnostDo)) FROM reg.V_Cislovanie tmp 
                                                            WHERE   tmp.C_TypBiznisEntity_Kniha_Id = k.C_TypBiznisEntity_Kniha_Id AND
                                                                    tmp.CislovanieJedno = n.CislovanieJedno AND 
                                                                    ISNULL(tmp.C_Stredisko_Id, 0)  = ISNULL(s.C_Stredisko_Id, 0) AND 
                                                                    ISNULL(tmp.C_BankaUcet_Id, 0)  = ISNULL(b.C_BankaUcet_Id, 0) AND
                                                                    ISNULL(tmp.C_Pokladnica_Id, 0) = ISNULL(p.C_Pokladnica_Id, 0)),
                                                            p.PlatnostOd,
                                                            b.PlatnostOd,
                                                            s.PlatnostOd,
                                                            '20000101') AS PlatnostOd, 
                                                    null AS PlatnostDo
                                                 FROM reg.V_TypBiznisEntityNastav AS n
                                                         JOIN reg.V_TypBiznisEntity_Kniha k ON k.C_TypBiznisEntity_Id = n.C_TypBiznisEntity_Id
                                                    LEFT JOIN reg.V_Stredisko s  ON s.D_Tenant_Id = n.D_Tenant_Id AND n.KodORS = 'ORJ' AND n.CislovanieJedno = 0
                                                    LEFT JOIN reg.V_BankaUcet b  ON b.D_Tenant_Id = n.D_Tenant_Id AND n.KodORS = 'VBU' AND n.CislovanieJedno = 0
                                                    LEFT JOIN reg.V_Pokladnica p ON p.D_Tenant_Id = n.D_Tenant_Id AND n.KodORS = 'POK' AND n.CislovanieJedno = 0
                                                 WHERE n.EvidenciaSystem = 1 AND n.KodORS IN ('ORJ', 'VBU', 'POK')");

            source = reader.Parse<CislovanieCis>().ToList();
            reader.Close();

            mergedData = GetList<CislovanieCis>(x => x.PlatnostDo == null).ToList();

            using (var transaction = BeginTransaction())
            {
                try
                {
                    foreach (var s in source)
                    {
                        var row = mergedData.Where(x => x.C_TypBiznisEntity_Kniha_Id == s.C_TypBiznisEntity_Kniha_Id &&
                                                        x.CislovanieJedno == s.CislovanieJedno &&
                                                        x.C_Stredisko_Id == s.C_Stredisko_Id &&
                                                        x.C_BankaUcet_Id == s.C_BankaUcet_Id &&
                                                        x.C_Pokladnica_Id == s.C_Pokladnica_Id).FirstOrDefault();
                        if (row == null)
                            destination.Add(s);
                        else
                            mergedData.Remove(row);
                    }

                    if (mergedData.Count > 0)
                        Db.ExecuteSql($"UPDATE reg.C_Cislovanie SET PlatnostDo = '{now.ToString("yyyy-MM-dd")}' WHERE C_Cislovanie_Id in ({string.Join(",", mergedData.Select(x => x.C_Cislovanie_Id).ToList())})");

                    if (destination.Count > 0)
                    {
                        foreach (var d in destination)
                        {
                            string c_Stredisko_Id = (d.C_Stredisko_Id.HasValue) ? d.C_Stredisko_Id.Value.ToString() : "NULL";
                            string c_BankaUcet_Id = (d.C_BankaUcet_Id.HasValue) ? d.C_BankaUcet_Id.Value.ToString() : "NULL";
                            string c_Pokladnica_Id = (d.C_Pokladnica_Id.HasValue) ? d.C_Pokladnica_Id.Value.ToString() : "NULL";

                            string sql = $@"INSERT INTO reg.C_Cislovanie (D_Tenant_Id, CislovanieJedno, C_TypBiznisEntity_Kniha_Id, C_Stredisko_Id, C_BankaUcet_Id, C_Pokladnica_Id, CisloInterneMask, VSMask, Popis, PlatnostOd, PlatnostDo)
                                            VALUES ('{Session.TenantIdGuid}', '{d.CislovanieJedno}', {d.C_TypBiznisEntity_Kniha_Id}, {c_Stredisko_Id}, {c_BankaUcet_Id}, {c_Pokladnica_Id}, '{d.CisloInterneMask}', '{d.VSMask}', NULL, '{d.PlatnostOd.ToString("yyyy-MM-dd")}', NULL)";
                            Db.ExecuteSql(sql);
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new WebEasException("Nastala chyba pri volaní SetCislovanie", ex.Message, ex);
                }
            }
        }
        #endregion

        #region Parovanie

        public void SetParovanieIdentifikatorRok(BiznisEntita_Parovanie data)
        {
            long id = 0;

            int parTyp = Db.Scalar<int>(Db.From<TypBiznisEntity_ParovanieDef>().Select(x => new { x.ParovanieTyp }).Where(x => x.C_TypBiznisEntity_ParovanieDef_Id == data.C_TypBiznisEntity_ParovanieDef_Id));
            switch (parTyp)
            {
                case (int)ParovanieTypEnum.par_1_N:
                    id = Db.Scalar<long>(Db.From<BiznisEntita_Parovanie>().Select(x => new { Identifikator = Sql.Max(x.Identifikator) }).Where(x => x.C_TypBiznisEntity_ParovanieDef_Id == data.C_TypBiznisEntity_ParovanieDef_Id && x.D_BiznisEntita_Id_1 == data.D_BiznisEntita_Id_1));
                    break;
                case (int)ParovanieTypEnum.par_N_1:
                    id = Db.Scalar<long>(Db.From<BiznisEntita_Parovanie>().Select(x => new { Identifikator = Sql.Max(x.Identifikator) }).Where(x => x.C_TypBiznisEntity_ParovanieDef_Id == data.C_TypBiznisEntity_ParovanieDef_Id && x.D_BiznisEntita_Id_2 == data.D_BiznisEntita_Id_2));
                    break;
                case (int)ParovanieTypEnum.par_N_M:
                    id = Db.Scalar<long>(Db.From<BiznisEntita_Parovanie>().Select(x => new { Identifikator = Sql.Max(x.Identifikator) }).Where(x => x.C_TypBiznisEntity_ParovanieDef_Id == data.C_TypBiznisEntity_ParovanieDef_Id && (x.D_BiznisEntita_Id_1 == data.D_BiznisEntita_Id_1 || x.D_BiznisEntita_Id_2 == data.D_BiznisEntita_Id_2)));
                    break;
                default:  // 1_1
                    break;
            }
            if (id == 0)
            {  // Najdi maximalne pouzite REF_ID
                id = Db.Scalar<long>(Db.From<BiznisEntita_Parovanie>().Select(x => new { Identifikator = Sql.Max(x.Identifikator) }));
                id += 1;
            }
            data.Identifikator = id;
            data.Rok = Db.Scalar<short>(Db.From<BiznisEntita>().Select(x => new { x.Rok }).Where(x => x.D_BiznisEntita_Id == data.D_BiznisEntita_Id_2));
        }

        #endregion

        #region Modul

        /// <summary>
        /// Generate tree node Správa modulu / Konfigurácia parametrov, História zmien stavov
        /// </summary>
        public HierarchyNode GenerateNodeSpravaModulu(string code, Type updateNastavenie)
        {
            var additionalFilter = code == "reg" ? (Session.AdminLevel == AdminLevel.SysAdmin ? null : new Filter(FilterElement.NotEq("Modul", "sys"))) : new Filter("Modul", code);
            var sm = new HierarchyNode("sm", "Module management")
            {
                Children = new List<HierarchyNode>
                    {
                        new HierarchyNode("mset", "Module management", typeof(NastavenieView), code == "reg" ? null : new Filter("Modul", code), null, HierarchyNodeIconCls.Settings, PfeSelection.Single, true)
                        {
                            Actions = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.UpdateNastavenie, updateNastavenie)
                            }
                        }
                    }
            };

            if (code != "cfe" && code != "dms" && code != "dap")
            {
                sm.AddChild(
                    new HierarchyNode("hzs", "History of state changes", typeof(EntitaHistoriaStavovView), additionalFilter, null, HierarchyNodeIconCls.History, PfeSelection.Single, true));
            }

            return sm;

        }

        public List<HierarchyNode> GenerateRzpModuleReports(Type rzpDennik, Type prhRozpoctu)
        {
            return new List<HierarchyNode>()
            {
                new HierarchyNode("rzpd", "Rozpočtový denník", rzpDennik, icon: HierarchyNodeIconCls.Book, crossModulItem: true)
                {
                    DialogTyp = DialogTypEnum.RzpDennik.ToString(),
                    SelectionMode = PfeSelection.Single,
                    Actions = new List<NodeAction>
                    {
                        // new NodeAction(NodeActionType.Change),
                        // new NodeAction(NodeActionType.Update, typeof(UpdateRzpDennik)),
                        new NodeAction(NodeActionType.MenuButtonsAll)
                        {
                            Caption = "Zostavy",
                            ActionIcon = NodeActionIcons.Zostavy,
                            MenuButtons = new List<NodeAction>()
                            {
                                new NodeAction(NodeActionType.ReportRzpDennik) { Url = $"/office/rzp/long/ReportRzpDennikPdf", GroupType = "ReportFilter" },
                                new NodeAction(NodeActionType.ViewReportRzpDennik) { Url = $"rzp/RzpDennikReport.trdp", GroupType = "ReportViewer" },
                                new NodeAction(NodeActionType.PrintReportRzpDennik) { Url = $"/office/rzp/long/ReportRzpDennikPdf", GroupType = "ReportViewer" }
                            }
                        }
                    }
                },
                new HierarchyNode("kmpl", "Prehľad rozpočtu", prhRozpoctu, icon: HierarchyNodeIconCls.MoneyBillAlt, crossModulItem: true)
                {
                    DialogTyp = DialogTypEnum.PrehladRzp.ToString(),
                    Actions = new List<NodeAction>
                    {
                        new NodeAction(NodeActionType.MenuButtonsAll)
                        {
                            Caption = "Zostavy",
                            ActionIcon = NodeActionIcons.Zostavy,
                            MenuButtons = new List<NodeAction>
                            {
                                new NodeAction(NodeActionType.ExportRzpRissam) { Url = $"/office/rzp/long/ReportRzpRissam" }
                            }
                        }
                    },
                    DefaultValues = new List<NodeFieldDefaultValue>
                    {
                        new NodeFieldDefaultValue("ObdobieText", DateTime.Today.Month)
                    },
                    LayoutDependencies = new List<LayoutDependency>
                    {
                        LayoutDependency.OneToMany("rzp-def-prij", "PrijemVydaj;C_RzpPol_Id;C_FRZdroj_Id;C_FREK_Id;A1;A2;A3", "Príjmové rzp. pol."),
                        LayoutDependency.OneToMany("rzp-def-vyd",  "PrijemVydaj;C_RzpPol_Id;C_FRZdroj_Id;C_FRFK_Id;C_FREK_Id;A1;A2;A3", "Výdajové rzp. pol."),
                        LayoutDependency.OneToOne("rzp-def-prs",   "D_Program_Id", "Progr.rzp.-sumárne"),
                        LayoutDependency.OneToMany("all-prh-rzpd",      "Rok;D_Program_Id;C_RzpPol_Id;PrijemVydaj;C_FRZdroj_Id;C_FRFK_Id;C_FREK_Id;A1;A2;A3;C_Stredisko_Id;C_Projekt_Id;ObdobieOd;ObdobieDo;DatumOd;DatumDo", "Rzp.denník"),
                        LayoutDependency.OneToMany("rzp-evi-navrh-pol", "Rok;D_Program_Id;C_RzpPol_Id;PrijemVydaj;C_FRZdroj_Id;C_FRFK_Id;C_FREK_Id;A1;A2;A3;C_Stredisko_Id;C_Projekt_Id", "Návrh rzp."),
                        LayoutDependency.OneToMany("rzp-evi-zmena-pol", "Rok;D_Program_Id;C_RzpPol_Id;PrijemVydaj;C_FRZdroj_Id;C_FRFK_Id;C_FREK_Id;A1;A2;A3;C_Stredisko_Id;C_Projekt_Id;ObdobieOd;ObdobieDo;DatumOd;DatumDo", "Zmeny rzp.")
                    }
                }
            };
        }

        public List<HierarchyNode> GenerateUctModuleReports(Type uctDennik, Type uctHlavnaKniha)
        {
            return new List<HierarchyNode>()
            {
                new HierarchyNode("uctd", "Účtovný denník", uctDennik, icon: HierarchyNodeIconCls.Book, crossModulItem: true)
                {
                    DialogTyp = DialogTypEnum.UctDennik.ToString(),
                    SelectionMode = PfeSelection.Single,
                    Actions = new List<NodeAction>
                    {
                        new NodeAction(NodeActionType.MenuButtonsAll)
                        {
                            Caption = "Zostavy", // Účtovný denník
                            ActionIcon = NodeActionIcons.Zostavy,
                            MenuButtons = new List<NodeAction>()
                            {
                                new NodeAction(NodeActionType.ReportUctDennik) { Url = $"/office/uct/long/ReportUctDennikPdf", GroupType = "ReportFilter"},
                                new NodeAction(NodeActionType.ViewReportUctDennik) { Url = $"uct/UctDennikReport.trdp", GroupType = "ReportViewer"},
                                new NodeAction(NodeActionType.PrintReportUctDennik) { Url = $"/office/uct/long/ReportUctDennikPdf", GroupType = "ReportViewer"}
                            }
                        }
                    }
                },
                new HierarchyNode("hlk", "Hlavná kniha", uctHlavnaKniha, icon: HierarchyNodeIconCls.Book, crossModulItem: true)
                {
                    DialogTyp = DialogTypEnum.HlavnaKniha.ToString(),
                    SelectionMode = PfeSelection.Multi,
                    Actions = new List<NodeAction>
                    {
                        new NodeAction(NodeActionType.MenuButtonsAll)
                        {
                            Caption = "Zostavy",
                            ActionIcon = NodeActionIcons.Zostavy,
                            MenuButtons = new List<NodeAction>()
                            {
                                new NodeAction(NodeActionType.ReportHlaKniha) { Url = $"/office/uct/long/ReportHlaKnihaPdf", GroupType = "ReportFilter"},
                                new NodeAction(NodeActionType.ViewReportHlaKniha) { Url = $"uct/HlaKnihaReport.trdp", GroupType = "ReportViewer"},
                                new NodeAction(NodeActionType.PrintReportHlaKniha) { Url = $"/office/uct/long/ReportHlaKnihaPdf", GroupType = "ReportViewer"}
                            }
                        }
                    },
                    LayoutDependencies = new List<LayoutDependency>
                    {
                        LayoutDependency.OneToMany("all-prh-uctd", "D_Hlk_Guid", "Účt.denník"),
                        LayoutDependency.OneToMany("all-prh-rzpd", "D_Hlk_Guid", "Rozp.denník")
                    }
                },
                new HierarchyNode<DummyCombo>("obu", "Obraty účtov *", null, icon: HierarchyNodeIconCls.ChartBar, crossModulItem: true)
                {
                }
            };
        }


        #region RZP Akcie

        /// <summary>
        /// Generuje zoznam akcii pre Report
        /// </summary>
        /// <returns></returns>
        public NodeAction ReportAkcieF112()
        {
            NodeAction akcia = new NodeAction(NodeActionType.MenuButtonsAll)
            {
                Caption = "Zostavy",
                ActionIcon = NodeActionIcons.Zostavy
            };

            akcia.MenuButtons = new List<NodeAction>
            {
                new NodeAction(NodeActionType.ReportVykazF112) { Url = $"/office/rzp/long/Report_vykaz_fin_1_12_pdf" },
                //new NodeAction(NodeActionType.ViewReportVykazF112) { Url = $"rzp/VykazF112Report.trdp", GroupType = "ReportViewer" },
                //new NodeAction(NodeActionType.PrintReportVykazF112) { Url = $"/office/rzp/long/{OperationsList.Report_vykaz_fin_1_12_pdf}" },
                new NodeAction(NodeActionType.ExportFinRissam) { Url = $"/office/rzp/long/ReportFinRissam" },
                new NodeAction(NodeActionType.ExportRzpRissam) { Url = $"/office/rzp/long/ReportRzpRissam" }
            };

            return akcia;
        }

        /// <summary>
        /// Generuje zoznam akcii pre Report
        /// </summary>
        /// <returns></returns>
        public NodeAction HistoriaAkcieF112()
        {
            NodeAction akcia = new NodeAction(NodeActionType.MenuButtonsAll)
            {
                Caption = "História",
                ActionIcon = NodeActionIcons.History
            };

            akcia.MenuButtons = new List<NodeAction>
            {
                new NodeAction(NodeActionType.SaveToHistory) { SelectionMode = PfeSelection.Single, Url = "/office/vyk/SaveToHistory" },
            };

            return akcia;
        }

        #endregion

        #endregion

        #region GetCisloDokladu

        public string OneMatch(string mask, bool raiseValidityError, string pattern, string kod, string stredisko, string pokladnica, string bankUcet, string replaceStr, ref string likeSql, string msg)
        {
            if (mask.IsEmpty())
                return "";

            try
            {
                var match = Regex.Match(mask, pattern);
                if (kod.IsEmpty())
                    kod = match.Groups[1].Value;

                if (kod.IsEmpty())
                    return mask;

                int charLen = 0;
                int charCount = 0;


                switch (kod)
                {
                    case "SKL":
                    case "ORJ":
                        charLen = int.Parse(match.Groups[2].Value);
                        if (raiseValidityError && stredisko == null)
                        {
                            return "<zvoľte stredisko a zatlačte refresh>";
                        }
                        replaceStr = stredisko.Substring(0, charLen);
                        break;
                    case "POK":
                        charLen = int.Parse(match.Groups[2].Value);
                        if (raiseValidityError && pokladnica == null)
                        {
                            return "<zvoľte pokladnicu a zatlačte refresh>";
                        }
                        replaceStr = pokladnica.Substring(0, charLen);
                        break;
                    case "VBU":
                        charLen = int.Parse(match.Groups[2].Value);
                        if (raiseValidityError && bankUcet == null)
                        {
                            return "<zvoľte bank. účet a zatlačte refresh>";
                        }
                        replaceStr = bankUcet.Substring(0, charLen);
                        break;
                    case "ZAM":
                        charLen = int.Parse(match.Groups[2].Value);
                        replaceStr = Session.EvidCisloZam.Substring(0, charLen);
                        break;
                    case "R":
                        charLen = 1;
                        replaceStr = replaceStr.Substring(3);
                        break;
                    case "RR":
                        charLen = 2;
                        replaceStr = replaceStr.Substring(2);
                        break;
                    case "RRRR":
                        charLen = 4;
                        // replaceStr = ponechám celý rok
                        break;
                    case "PV":
                        charLen = 2;
                        charCount = 1;
                        break;
                    case "MM":
                        charLen = 2;
                        break;
                    default:
                        break;
                }

                if (charCount == 0)
                    charCount = charLen;

                if (match.Groups[0].Value.Contains("["))
                {
                    likeSql = likeSql.Replace($"[{kod}{charLen}]", replaceStr);
                    likeSql = likeSql.Replace($"[{kod}]", replaceStr);
                }
                else if (match.Groups[0].Value.Contains("{"))
                {
                    likeSql = likeSql.Replace(match.Groups[0].Value, "_".PadLeft(charCount, '_'));
                }

                if (!match.Groups[0].Value.IsEmpty())
                    return mask.Replace(match.Groups[0].Value, replaceStr);

                return mask;
            }
            catch (Exception e)
            {
                throw new WebEasValidationException(null, msg, e);
            }
        }

        private class CislovanieDefinitionHelper
        {
            public string CisloInterneMask { get; set; }
            public string VSMask { get; set; }
            public string Str { get; set; }
            public string Pok { get; set; }
            public string Bu { get; set; }
        }

        public int GetCisloDokladu(DateTime datumDokladu, BiznisEntita biznisEntita, string NumberChar, out string newCisloInterne, out string newVS)
        {
            bool ok = GetDefCisloDokladu(datumDokladu, biznisEntita, NumberChar, out newCisloInterne, out newVS, out string likeSql, out string filterRok);
            string txt;
            char paddingChar;
            int newCounter;
            if (!string.IsNullOrEmpty(NumberChar))
            {
                newCounter = 0;
                paddingChar = Convert.ToChar(NumberChar);
                txt = NumberChar;
            }
            else
            {
                likeSql = likeSql.Replace(Regex.Match(likeSql, @"<C,(\d*)>").Groups[0].Value,
                    "_".PadLeft(int.Parse(Regex.Match(likeSql, @"<C,(\d*)>").Groups[1].Value), '_'));

                newCounter = Db.Scalar<int>($@"SELECT TOP(1) d.Cislo FROM reg.D_BiznisEntita as d 
                                                   WHERE d.DatumPlatnosti IS NULL AND 
                                                         d.C_TypBiznisEntity_Id = {biznisEntita.C_TypBiznisEntity_Id} AND
                                                         {filterRok}
                                                         d.CisloInterne like '{likeSql}' 
                                                   ORDER BY d.Cislo DESC") + 1;
                paddingChar = '0';
                txt = newCounter.ToString();
            }

            if (!string.IsNullOrEmpty(NumberChar) && !ok)
            {
                //Pri novom doklade ešte nemám prvok ORŠ zvolený takže vraciam iba text, že treba zvoliť ORJ, POK, VBU
                return 0;
            }
            else
            {
                newCisloInterne = newCisloInterne.Replace(Regex.Match(newCisloInterne, @"<.,(\d*)>").Groups[0].Value, txt.PadLeft(int.Parse(Regex.Match(newCisloInterne, @"<.,(\d*)>").Groups[1].Value), paddingChar));

                if (!newVS.IsEmpty())
                    newVS = newVS.Replace(Regex.Match(newVS, @"<.,(\d*)>").Groups[0].Value, txt.PadLeft(int.Parse(Regex.Match(newVS, @"<.,(\d*)>").Groups[1].Value), paddingChar));

                return newCounter;
            }
        }

        public List<(int Cislo, string VS, string CisloDokladu)> GetChybajuceCisloDokladu(DateTime datumDokladu, BiznisEntita biznisEntita)
        {
            var chybajuceCislaDokladov = new List<(int Cislo, string VS, string CisloDokladu)>();

            if (!GetDefCisloDokladu(datumDokladu, biznisEntita, "chýb.", out string newCisloInterne, out string newVS, out string likeSql, out string filterRok))
            {
                chybajuceCislaDokladov.Add((0, newVS, newCisloInterne));
                return chybajuceCislaDokladov;
            };

            char paddingChar = '0';

            likeSql = likeSql.Replace(Regex.Match(likeSql, @"<C,(\d*)>").Groups[0].Value,
                    "_".PadLeft(int.Parse(Regex.Match(likeSql, @"<C,(\d*)>").Groups[1].Value), '_'));

            var cisla = Db.Select<int>($@"SELECT d.Cislo FROM reg.D_BiznisEntita as d 
                                                   WHERE d.DatumPlatnosti IS NULL AND 
                                                         d.C_TypBiznisEntity_Id = {biznisEntita.C_TypBiznisEntity_Id} AND
                                                         {filterRok}
                                                         d.CisloInterne like '{likeSql}'");

            if (cisla.Count > 0)
            {
                for (int counter = 1; counter <= cisla.Max(); counter++)
                {
                    if (!cisla.Contains(counter))
                    {
                        var cisloDokladu = newCisloInterne.Replace(Regex.Match(newCisloInterne, @"<.,(\d*)>").Groups[0].Value, counter.ToString().PadLeft(int.Parse(Regex.Match(newCisloInterne, @"<.,(\d*)>").Groups[1].Value), paddingChar));
                        string vs = null;
                        if (!newVS.IsEmpty())
                            vs = newVS.Replace(Regex.Match(newVS, @"<.,(\d*)>").Groups[0].Value, counter.ToString().PadLeft(int.Parse(Regex.Match(newVS, @"<.,(\d*)>").Groups[1].Value), paddingChar));

                        chybajuceCislaDokladov.Add((counter, vs, cisloDokladu));
                    }
                }
            }

            if (chybajuceCislaDokladov.Count == 0)
            {
                chybajuceCislaDokladov.Add((0, "", "<žiadne chýbajúce čísla v rade>"));
            };
            return chybajuceCislaDokladov;
        }

        private bool GetDefCisloDokladu(DateTime datumDokladu, BiznisEntita biznisEntita, string NumberChar, out string newCisloInterne, out string newVS, out string likeSql, out string filterRok)
        {
            // Zistenie masky
            var data = GetCacheOptimizedTenant($"GetCislovanieDefinition:{biznisEntita.C_TypBiznisEntity_Kniha_Id}:{datumDokladu:yyyy-MM-dd}-{biznisEntita.C_Stredisko_Id.GetValueOrDefault()}-{biznisEntita.C_Pokladnica_Id.GetValueOrDefault()}-{biznisEntita.C_BankaUcet_Id.GetValueOrDefault()}", () =>
            {
                string sSql;
                sSql = $@"SELECT cc.CisloInterneMask, cc.VSMask, ORJ.Kod AS Str, POK.Kod AS Pok, VBU.Kod AS Bu
                        FROM reg.V_Cislovanie as cc
                            LEFT JOIN reg.C_Stredisko  ORJ ON cc.KodORS = 'ORJ' AND ORJ.C_Stredisko_Id  = {biznisEntita.C_Stredisko_Id.GetValueOrDefault()}
                            LEFT JOIN reg.C_Pokladnica POK ON cc.KodORS = 'POK' AND POK.C_Pokladnica_Id = {biznisEntita.C_Pokladnica_Id.GetValueOrDefault()}
                            LEFT JOIN reg.C_BankaUcet  VBU ON cc.KodORS = 'VBU' AND VBU.C_BankaUcet_Id  = {biznisEntita.C_BankaUcet_Id.GetValueOrDefault()}
                        WHERE cc.C_TypBiznisEntity_Kniha_Id = {biznisEntita.C_TypBiznisEntity_Kniha_Id} AND 
                                '{datumDokladu:yyyy-MM-dd}' BETWEEN cc.PlatnostOd AND ISNULL(cc.PlatnostDo, '2100-01-01') AND
                                (cc.CislovanieJedno = 1 OR 
                                cc.KodORS = 'ORJ' AND cc.C_Stredisko_Id  = {biznisEntita.C_Stredisko_Id.GetValueOrDefault()}  OR
                                cc.KodORS = 'POK' AND cc.C_Pokladnica_Id = {biznisEntita.C_Pokladnica_Id.GetValueOrDefault()} OR
                                cc.KodORS = 'VBU' AND cc.C_BankaUcet_Id  = {biznisEntita.C_BankaUcet_Id.GetValueOrDefault()})";
                return Db.Select<CislovanieDefinitionHelper>(sSql);
            });
            bool raiseValidityError = !string.IsNullOrEmpty(NumberChar);
            if (!data.Any())
            {
                if (raiseValidityError && (biznisEntita.C_BankaUcet_Id == null && biznisEntita.C_TypBiznisEntity_Id == (short)TypBiznisEntityEnum.BAN ||
                                           biznisEntita.C_Pokladnica_Id == null && biznisEntita.C_TypBiznisEntity_Id == (short)TypBiznisEntityEnum.PDK ||
                                           biznisEntita.C_Stredisko_Id == null && biznisEntita.C_TypBiznisEntity_Id != (short)TypBiznisEntityEnum.BAN &&
                                                                                  biznisEntita.C_TypBiznisEntity_Id != (short)TypBiznisEntityEnum.PDK
                                          ))
                {
                    string ors;
                    if (biznisEntita.C_TypBiznisEntity_Id == (short)TypBiznisEntityEnum.BAN)
                    {
                        ors = "bank. účet";
                    }
                    else if (biznisEntita.C_TypBiznisEntity_Id == (short)TypBiznisEntityEnum.PDK)
                    {
                        ors = "pokladnicu";
                    }
                    else
                    {
                        ors = "stredisko";
                    }

                    newCisloInterne = $"<zvoľte {ors} a zatlačte refresh>";
                    newVS = "";
                    likeSql = "";
                    filterRok = "";
                    return false;
                }
                throw new WebEasValidationException(null, "K uvedenému dátumu dokladu nie je zadefinované žiadne platné číslovanie!");
            }

            newCisloInterne = data[0].CisloInterneMask;
            newVS = data[0].VSMask;

            string stredisko = data[0].Str;
            string pokladnica = data[0].Pok;
            string bankUcet = data[0].Bu;

            likeSql = newCisloInterne.Replace("_", "[_]");
            string likeSqlTmp = "";
            filterRok = "";

            //ORJ SKL POK VBU ZAM
            newCisloInterne = OneMatch(newCisloInterne, raiseValidityError, @"[[{](ORJ|SKL|POK|VBU|ZAM)(\d*)[]}]", "", stredisko, pokladnica, bankUcet, "", ref likeSql, "Nepodarilo sa vygenerovať číslo dokladu. Skontrolujte nastavenie číselného radu (kód ORŠ - strediska, bank.účtu alebo pokladnice)!");
            newVS = OneMatch(newVS, raiseValidityError, @"[[{](ORJ|SKL|POK|VBU|ZAM)(\d*)[]}]", "", stredisko, pokladnica, bankUcet, "", ref likeSqlTmp, "Nepodarilo sa vygenerovať variabilný symbol. Skontrolujte nastavenie číselného radu (kód ORŠ - strediska, bank.účtu alebo pokladnice)!");

            //Mesiac
            newCisloInterne = OneMatch(newCisloInterne, raiseValidityError, @"[[{](MM)(\d*)[]}]", "MM", "", "", "", datumDokladu.Month.ToString().PadLeft(2, '0'), ref likeSql, "Nepodarilo sa vygenerovať číslo dokladu. Skontrolujte nastavenie číselného radu (mesiac)!");
            newVS = OneMatch(newVS, raiseValidityError, @"[[{](MM)(\d*)[]}]", "MM", "", "", "", datumDokladu.Month.ToString().PadLeft(2, '0'), ref likeSqlTmp, "Nepodarilo sa vygenerovať variabilný symbol. Skontrolujte nastavenie číselného radu (mesiac)!");

            //Rok
            if (newCisloInterne.Contains("[R]") || newCisloInterne.Contains("[RR]") || newCisloInterne.Contains("[RRRR]"))
            {
                filterRok = $"Rok = {datumDokladu.Year} AND ";
            }
            newCisloInterne = OneMatch(newCisloInterne, raiseValidityError, @"[[{](R|RR|RRRR)(\d*)[]}]", "", "", "", "", datumDokladu.Year.ToString(), ref likeSql, "Nepodarilo sa vygenerovať číslo dokladu. Skontrolujte nastavenie číselného radu (rok)!");
            newVS = OneMatch(newVS, raiseValidityError, @"[[{](R|RR|RRRR)(\d*)[]}]", "", "", "", "", datumDokladu.Year.ToString(), ref likeSqlTmp, "Nepodarilo sa vygenerovať variabilný symbol. Skontrolujte nastavenie číselného radu (rok)!");

            //PV
            newCisloInterne = OneMatch(newCisloInterne, raiseValidityError, @"[[{](PV)(\d*)[]}]", "PV", "", "", "", (biznisEntita.C_TypBiznisEntity_Kniha_Id == (int)TypBiznisEntity_KnihaEnum.Prijmove_pokladnicne_doklady) ? "P" : "V", ref likeSql, "Nepodarilo sa vygenerovať číslo dokladu. Skontrolujte nastavenie číselného radu (príjem/výdaj)!");
            newVS = OneMatch(newVS, raiseValidityError, @"[[{](PV)(\d*)[]}]", "PV", "", "", "", (biznisEntita.C_TypBiznisEntity_Kniha_Id == (int)TypBiznisEntity_KnihaEnum.Prijmove_pokladnicne_doklady) ? "P" : "V", ref likeSqlTmp, "Nepodarilo sa vygenerovať variabilný symbol. Skontrolujte nastavenie číselného radu (príjem/výdaj)!");

            if (newCisloInterne.StartsWith("<zvoľte"))
            {
                return false;
            }
            return true;
        }
        #endregion

        #region Kontroly
        public void KontrolaPokladnica(string kodPokladnice, bool dcomRezim)
        {
            if (dcomRezim)
            {
                if (!int.TryParse(kodPokladnice, out _))
                {
                    throw new WebEasValidationException(null, $"Kód pokladnice '{kodPokladnice}' musí byť kvôli odoslaniu do DCOM-u skonvertovateľný na číslo!");
                }
            }
        }

        public void KontrolaStredisko(string kodStrediska, bool dcomRezim)
        {
            if (dcomRezim)
            {
                if (!int.TryParse(kodStrediska, out _))
                {
                    throw new WebEasValidationException(null, $"Kód strediska '{kodStrediska}' musí byť kvôli odoslaniu do DCOM-u skonvertovateľný na číslo!");
                }
            }
        }
        #endregion

        #region Formátovanie stringov

        public string FormatujUcet(string ucet, string fmt)
        {
            if (ucet == null || ucet.Length <= 3) return ucet;
            string nazovUctu = "";
            if (ucet.Contains(" - "))
            {
                nazovUctu = ucet.Substring(ucet.IndexOf(" - ")); //Formatujem: Ucet + " - " + Nazov
                ucet = ucet.Substring(0, ucet.IndexOf(" - ")).Trim();
            }

            if (ucet.IndexOf('E') == 3 || ucet.IndexOf('F') == 3 || ucet.IndexOf('Z') == 3)
            {
                //Nevyplnené S1-6 iba Zdroj, EK, FK - neformátujem
                return ucet.Substring(0, 3) + ' ' + ucet.Substring(3);
            }
            string suffix = "";
            if (ucet.Contains('|'))
            {
                suffix = ucet.Substring(ucet.IndexOf('|'));
                ucet = ucet.Substring(0, ucet.IndexOf('|')).Trim();
            }
            char[] characters = (ucet + "".PadLeft(50, ' ')).ToCharArray();
            object[] args = Array.ConvertAll(characters, x => (object)x);
            ucet = string.Format(fmt, args).TrimEnd(' ', '.', '-', '/');

            return (ucet + ' ' + suffix + nazovUctu).Trim();
        }

        public void OdstranitFormatovanieUctuFiltra(string ucet, ref Filter filter)
        {
            var filterElement = filter.GetFilterElementByKeyName(ucet);
            if (filterElement != null)
            {
                filterElement.Value = filterElement.Value.ToString().Replace(" ", "").Replace(".", "").Replace("-", "").Replace("/", "");
            }

            var filterElementInputSearch = filter.GetFilterElementByKeyName("InputSearchField");
            if (filterElementInputSearch != null)
            {
                filterElementInputSearch.Value = filterElementInputSearch.Value.ToString().Replace(" ", "").Replace(".", "").Replace("-", "").Replace("/", "");
            }
        }

        #endregion

    }
}