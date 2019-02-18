using System;
using System.Data;
using System.Linq;
using Ninject;
using ServiceStack.Data;
using ServiceStack.Logging;
using ServiceStack.OrmLite;
using System.Configuration;
using ServiceStack;
using ServiceStack.Redis;

namespace WebEas.Core.Base
{
    public abstract class WebEasCoreRepositoryBase : LogicBase, IWebEasCoreRepositoryBase
    {
        protected static readonly ILog Log = LogManager.GetLogger(typeof(WebEasCoreRepositoryBase));

        private IDbConnection db;

        private IWebEasSession session = null;

        /// <summary>
        /// Gets or sets the STS thumb print.
        /// </summary>
        /// <value>The STS thumb print.</value>
        public string StsThumbPrint { get; set; }

        /// <summary>
        /// Gets the db.
        /// </summary>
        /// <value>The db.</value>
        public new IDbConnection Db
        {
            get
            {
                if (this.db == null)
                {
                    this.db = this.DbFactory.OpenDbConnection();
                    #if DEBUG
                    Log.Debug("Connection opened..");
                    #endif
                    SetDbContextInfo(ref db);
                }
                if (this.db.State == ConnectionState.Closed)
                {
                    this.db.Close();
                    #if DEBUG
                    Log.Warn("Need to reopen closed connection - Still wrong");
                    #endif
                    this.db.Open();
                    SetDbContextInfo(ref db);
                }

                return this.db;
            }
            set
            {
                if (value != null && this.db != value)
                {
                    if (this.db != null)
                    {
                        #if DEBUG
                        Log.Warn("Changing connection - closing");
                        #endif
                        this.db.Close();
                        this.db = null;
                    }
                    this.db = value;
                    this.SetDbContextInfo(ref db);
                }
            }
        }

        /// <summary>
        /// Gets db connection with context info
        /// </summary>
        /// <param name="namedConnection">optional connection name</param>
        /// <returns>IDbConnection</returns>
        public IDbConnection GetNewDb(string namedConnection)
        {
            var db = DbFactory.OpenDbConnection(namedConnection);
            SetDbContextInfo(ref db);
            return db;
        }

        public IDbConnection GetNewDb()
        {
            var db = DbFactory.OpenDbConnection();
            SetDbContextInfo(ref db);
            return db;
        }

        /// <summary>
        /// Gets or sets the db factory.
        /// </summary>
        /// <value>The db factory.</value>
        [Inject]
        public override IDbConnectionFactory DbFactory { get; set; }

        [Inject]
        public override IRedisClientsManager RedisManager { get; set; }

        /// <summary>
        /// Sets the session if is empty or null context
        /// </summary>
        /// <param name="session">The session.</param>
        protected void SetSession(IWebEasSession session)
        {
            if (!Context.Current.HasHttpContext || this.session == null)
            {
                if (this.session != session)
                {
                    Log.Info(string.Format("Setting session to Key {0}", session.UniqueKey));
                    this.session = session;                    
                }
                Context.Current.SetBackgroundProcessSessionUniqueKey(session.UniqueKey);
            }
            else
            {
                throw new NotSupportedException("Only not setted session is allowed to set");
            }
        }

        /// <summary>
        /// Gets the session.
        /// </summary>
        /// <value>The session.</value>
        public virtual IWebEasSession Session
        {
            get
            {
                if (this.session == null)
                {
                    #pragma warning disable 0618
                    this.session = Context.Current.Session;
                    #pragma warning restore 0618
                }
                return this.session;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            string lastsql = string.Empty;
            string corid = string.Empty;
            if (this.db != null)
            {
                #if DEBUG
                Log.Debug("Disposing connection");
                #endif

                lastsql = db.GetLastSql();
                corid = Context.Current.CurrentCorrelationID.ToString().ToLower();

                this.db.Dispose();
                this.db = null;
                //   Log.Debug("Connection opened..");
                // System.Data.SqlClient.SqlConnection.ClearAllPools();
            }
            this.session = null;

            if (!string.IsNullOrEmpty(lastsql) && !string.IsNullOrEmpty(corid))
            {
                try
                {
                    if (NLog.LogManager.Configuration?.FindTargetByName("database") != null)
                    {
                        if (NLog.LogManager.Configuration.FindTargetByName("database") is NLog.Targets.DatabaseTarget target)
                        {
                            if (target.ConnectionString is NLog.Layouts.SimpleLayout connStringSimpleLayout)
                            {
                                if (!string.IsNullOrEmpty(connStringSimpleLayout?.Text) && DbFactory != null)
                                {
                                    using (var dbLog = DbFactory.OpenDbConnectionString(connStringSimpleLayout.Text))
                                    {
                                        if (dbLog != null)
                                        {
                                            if (dbLog.State == ConnectionState.Closed)
                                            {
                                                dbLog.Close();
                                                dbLog.Open();
                                            }
                                            dbLog.ExecuteNonQuery("set context_info 0x50006F00730074004400650070006C006F007900");
                                            var id = dbLog.Scalar<long?>("select D_Log_Id from reg.D_Log where CorrId = @corid", new { corid });
                                            if (id != null)
                                            {
                                                dbLog.ExecuteNonQuery("update reg.D_Log set LastSql = @sql where D_Log_Id = @id", new { sql = lastsql, id });
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("LastSql Log Error", ex);
                }
            }

            base.Dispose();
        }

        /// <summary>
        /// Sets the db context info.
        /// </summary>
        internal void SetDbContextInfo(ref IDbConnection db)
        {
            #if DEBUG
            Log.Debug("Setting context info");
            #endif
            if (this.Session.TenantIdGuid.HasValue || this.Session.DcomIdGuid.HasValue || Context.Current.CurrentEndpoint != Context.EndpointType.Unknown)
            {
                byte[] tenant = this.Session.TenantIdGuid.HasValue ? this.Session.TenantIdGuid.Value.ToByteArray() : Guid.Empty.ToByteArray();

                //Ak mam v Query atribut tenantId a jedna sa o Public endpoint, tak pouzijem na context info tohoto tenanta
                if (Context.Current.CurrentEndpoint == Context.EndpointType.Public)
                {
                    if (!string.IsNullOrEmpty(this.Session.QueryTenantId))
                    {
                        tenant = new Guid(this.Session.QueryTenantId).ToByteArray();
                    }
                }

                byte[] endpoint = new byte[] { (byte)Context.Current.CurrentEndpoint };
                byte[] dcomId = this.Session.SubjectDcomIdGuid.HasValue ? this.Session.SubjectDcomIdGuid.Value.ToByteArray() : this.Session.DcomIdGuid.HasValue ? this.Session.DcomIdGuid.Value.ToByteArray() : Guid.Empty.ToByteArray();

                byte[] context = tenant.Concat(endpoint).Concat(dcomId).ToArray();
                IDbCommand cmd = db.CreateCommand();
                cmd.CommandText = "SET CONTEXT_INFO @context";
                cmd.AddParam("context", context, ParameterDirection.Input, DbType.Binary);
                cmd.ExecuteNonQuery();
                #if DEBUG
                Log.Debug(string.Format("SET CONTEXT_INFO {0};", BitConverter.ToString(context)));
                Log.Debug(string.Format("Value - tenant:           {0};", this.Session.TenantId));
                Log.Debug(string.Format("Value - (string)endpoint: {0};", Context.Current.CurrentEndpoint));
                Log.Debug(string.Format("Value - DcomId:           {0};", this.Session.DcomId));
                Log.Debug(string.Format("Value - SubjectDcomId:    {0};", this.Session.SubjectDcomId));
                Log.Debug(string.Format("Value - UserType:         {0};", this.Session.UserType));
                Log.Debug(string.Format("Value - QueryTenantId:    {0};", this.Session.QueryTenantId));
                #endif
            }
            else
            {
                ClearDbContextInfo(ref db);
            }
        }

        /// <summary>
        /// Clears the db context info.
        /// </summary>
        internal void ClearDbContextInfo(ref IDbConnection db)
        {
            #if DEBUG
            Log.Debug("Clearing context info");
            #endif
            db.ExecuteNonQuery("SET CONTEXT_INFO 0x");
        }
    }
}