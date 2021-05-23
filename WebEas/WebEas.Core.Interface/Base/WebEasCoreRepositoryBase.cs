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
        private IWebEasSession session;

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
                    SetDbContextInfo();
                }
                if (this.db.State == ConnectionState.Closed)
                {
                    this.db.Close();
                    #if DEBUG
                    Log.Warn("Need to reopen closed connection - Still wrong");
                    #endif
                    this.db.Open();
                    SetDbContextInfo();
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
                    this.SetDbContextInfo();
                }
            }
        }

        /// <summary>
        /// Gets db connection with context info
        /// </summary>
        /// <param name="namedConnection">optional connection name</param>
        /// <returns>IDbConnection</returns>
        public IDbConnection GetNewDb(string sConnString, int nastavenieISOZdroj)
        {
            if (nastavenieISOZdroj == 1)      // KORWIN
                sConnString = sConnString.Replace("%heslo%", "Dtln19!");
            else if (nastavenieISOZdroj == 2) // URBIS
                sConnString = sConnString.Replace("%heslo%", "p4LMtre£99");
            var db = DbFactory.OpenDbConnectionString(sConnString);
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

        [Inject]
        public IServerEvents ServerEvents { get; set; }

        /// <summary>
        /// Gets the session.
        /// </summary>
        /// <value>The session.</value>

        public virtual IWebEasSession Session
        {
            get
            {
                return session;
            }
            set
            {
                if (session == null)
                {
                    session = value;
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            if (this.db != null)
            {
                #if DEBUG
                Log.Debug("Disposing connection");
                #endif

                this.db.Dispose();
                this.db = null;
                //   Log.Debug("Connection opened..");
                // System.Data.SqlClient.SqlConnection.ClearAllPools();
            }
            base.Dispose();
        }

        /// <summary>
        /// Sets the db context info.
        /// </summary>
        public void SetDbContextInfo()
        {
            #if DEBUG
            Log.Debug("Setting context info");
            #endif
            if (this.Session.TenantIdGuid.HasValue || this.Session.UserIdGuid.HasValue || Context.Current.CurrentEndpoint != Context.EndpointType.Unknown)
            {
                byte[] tenant = this.Session.TenantIdGuid.HasValue ? this.Session.TenantIdGuid.Value.ToByteArray() : Guid.Empty.ToByteArray();

                byte[] endpoint = new byte[] { (byte)Context.Current.CurrentEndpoint };
                byte[] userId = this.Session.UserIdGuid.HasValue ? this.Session.UserIdGuid.Value.ToByteArray() : Guid.Empty.ToByteArray();
                byte[] rola = (this.Session.AdminLevel == AdminLevel.SysAdmin) ? "A".ToAsciiBytes() : ((this.Session.AdminLevel == AdminLevel.CfeAdmin) ? "M".ToAsciiBytes() : "U".ToAsciiBytes());

                byte[] context = tenant.Concat(endpoint).Concat(userId).Concat(rola).ToArray();
                if (Session.OrsPermissions != null)
                    context = context.Concat((new string('.', 16)).ToAsciiBytes()).Concat(Session.OrsPermissions.ToAsciiBytes()).ToArray();

                IDbCommand cmd = db.CreateCommand();
                cmd.CommandText = "SET CONTEXT_INFO @context";
                cmd.AddParam("context", context, ParameterDirection.Input, DbType.Binary);
                cmd.ExecuteNonQuery();
                #if DEBUG
                Log.Debug(string.Format("SET CONTEXT_INFO {0};", BitConverter.ToString(context)));
                Log.Debug(string.Format("Value - tenant:           {0};", this.Session.TenantId));
                Log.Debug(string.Format("Value - (string)endpoint: {0};", Context.Current.CurrentEndpoint));
                Log.Debug(string.Format("Value - DcomId:           {0};", this.Session.UserId));
                #endif

                if(Session.OrsElementPermisions != null)
                {
                    foreach(var elPerm in Session.OrsElementPermisions)
                    {
                        cmd = db.CreateCommand();
                        cmd.CommandText = $"EXEC sys.sp_set_session_context @key = N'{elPerm.Key}', @value = '{elPerm.Value}'";
                        cmd.ExecuteNonQuery();
#if DEBUG
                        Log.Debug(cmd.CommandText);
#endif
                    }
                }
            }
            else
            {
                ClearDbContextInfo();
            }
        }

        /// <summary>
        /// Clears the db context info.
        /// </summary>
        public void ClearDbContextInfo()
        {
            #if DEBUG
            Log.Debug("Clearing context info");
            #endif
            db.ExecuteNonQuery("SET CONTEXT_INFO 0x");
        }
    }
}