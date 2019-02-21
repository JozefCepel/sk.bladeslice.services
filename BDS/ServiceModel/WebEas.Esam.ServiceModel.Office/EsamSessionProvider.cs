using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Web;
using System.Web.Configuration;
using System.Xml;
using Ninject;
using ServiceStack;
using ServiceStack.Caching;
using ServiceStack.Logging;
using ServiceStack.Redis;
using WebEas.Auth;
using WebEas.Core;
using WebEas.Ninject;

namespace WebEas.Esam.ServiceModel.Office
{
    public class EsamSessionProvider : IWebEasSessionProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(EsamSessionProvider));

        private const string EsbActorIDSector = "ActorIDSector";
        private const string EsbTechDcomId = "TechID";
        private const string EsbUserDcomId = "UserID";
        private const string EsbTenantId = "TenantID";
        private const string EsbTechRoles = "TechRoles";
        private const string EsbUserSessionId = "UserSessionID";

        private const string ActorIdsector = "ActorIDSector";
        private const string ActorDcomId = "Actor.DcomID";
        private const string ActorEmail = "Actor.Email";
        private const string ActorFormattedName = "Actor.FormattedName";
        private const string ActorFirstName = "Actor.FirstName";
        private const string ActorLastName = "Actor.LastName";
        private const string ActorPreferredLanguage = "Actor.PreferredLanguage";
        private const string ActorPreferredTenant = "Actor.PreferredTenant";
        private const string ActorTenant = "Actor.TenantId";

        private const string SubjectDcomId = "Subject.DcomID";
        private const string SubjectIdentityType = "Subject.identityType";
        private const string SubjectFirstName = "Subject.firstName";
        private const string SubjectFormattedName = "Subject.formattedName";
        private const string SubjectLastName = "Subject.lastName";

        private const string TenantId = "TenantId";
        private const string Upn = "Upn";

        private static string iamDcomToken = null;

        /// <summary>
        /// Gets the iam DCOM token.
        /// </summary>
        /// <value>The iam DCOM token.</value>
        private string IamDcomToken
        {
            get
            {
                if (string.IsNullOrEmpty(iamDcomToken))
                {
                    iamDcomToken = string.IsNullOrEmpty(WebConfigurationManager.AppSettings["IamDcomTokenName"]) ? "IAMDCOMToken" : WebConfigurationManager.AppSettings["IamDcomTokenName"];
                }
                return iamDcomToken;
            }
        }

        private static List<Role> roleList = null;

        /// <summary>
        /// Gets the cert thumbprint.
        /// </summary>
        /// <value>The cert thumbprint.</value>
        private static string DebugTokenId
        {
            get
            {
#if DEBUG
                log.Debug("Loading token..");
#endif
                if (String.IsNullOrEmpty(WebConfigurationManager.AppSettings["DebugTokenId"]))
                {
                    return null;
                }
                return WebConfigurationManager.AppSettings["DebugTokenId"];
            }
        }

        /// <summary>
        /// Gets the debug tenant id.
        /// </summary>
        /// <value>The debug tenant id.</value>
        private static string DebugTenantId
        {
            get
            {
#if DEBUG
                log.Debug("Loading debug tenantId..");
#endif
                if (String.IsNullOrEmpty(WebConfigurationManager.AppSettings["DebugTenantId"]))
                {
                    return null;
                }
                return WebConfigurationManager.AppSettings["DebugTenantId"];
            }
        }

        /// <summary>
        /// Gets the debug load roles.
        /// </summary>
        /// <value>The debug load roles.</value>
        private static bool DebugLoadRoles
        {
            get
            {
                if (String.IsNullOrEmpty(WebConfigurationManager.AppSettings["DebugLoadRoles"]))
                {
                    return false;
                }
                return WebConfigurationManager.AppSettings["DebugLoadRoles"] == "1";
            }
        }

        /// <summary>
        /// Gets the debug DCOM id.
        /// </summary>
        /// <value>The debug DCOM id.</value>
        private static string DebugDcomId
        {
            get
            {
#if DEBUG
                log.Debug("Loading debug dcom id..");
#endif
                if (String.IsNullOrEmpty(WebConfigurationManager.AppSettings["DebugDcomId"]))
                {
                    return null;
                }
                return WebConfigurationManager.AppSettings["DebugDcomId"];
            }
        }

        /// <summary>
        /// Gets the debug DCOM id.
        /// </summary>
        /// <value>The debug DCOM id.</value>
        private static string DebugSubjectDcomId
        {
            get
            {
#if DEBUG
                log.Debug("Loading debug subject dcom id..");
#endif
                if (String.IsNullOrEmpty(WebConfigurationManager.AppSettings["DebugSubjectDcomId"]))
                {
                    return null;
                }
                return WebConfigurationManager.AppSettings["DebugSubjectDcomId"];
            }
        }

        private static readonly object lockObject = new Object();

        /// <summary>
        /// Gets the role list.
        /// </summary>
        /// <value>The role list.</value>
        private static List<Role> RoleList
        {
            get
            {
                if (roleList == null)
                {
                    lock (lockObject)
                    {
                        if (roleList == null)
                        {
                            var listRoles = new List<Role>();

                            try
                            {
#if DEBUG
                                log.Info("Loading role list..");
#endif
                                List<IRoleList> list = NinjectServiceLocator.Kernel.GetAll<IRoleList>().ToList();

#if DEBUG
                                log.Info(string.Format("modules {0}", list.Count));
#endif
                                foreach (IRoleList roles in list)
                                {
                                    listRoles.AddRange(roles.RoleList);
                                }
                            }
                            catch (Exception ex)
                            {
                                throw new WebEasException("Error in getting role list", ex);
                            }
                            var admin = new Role("BDS_ADMIN", "Admin modulu rozpočet");
                            admin.SubRoles = new List<Role>();
                            admin.SubRoles.AddRange(listRoles);
                            listRoles.Add(admin);

#if DEBUG
                            log.Info(string.Format("loaded {0} roles.", listRoles.Count));
#endif
                            roleList = listRoles;
                        }
                    }
                }
                return roleList;
            }
        }

        /// <summary>
        /// Gets the cache.
        /// </summary>
        /// <value>The cache.</value>
        private static ICacheClient Cache
        {
            get
            {
                var redisMan = NinjectServiceLocator.Kernel.Get<IRedisClientsManager>();
                return redisMan != null ? redisMan.GetCacheClient() : WebEas.Context.Current.LocalMachineCache;
            }
        }

        private string uniqueKey;

        /// <summary>
        /// Sets the background session.
        /// </summary>
        /// <param name="uniqueKey">The unique key.</param>
        public void SetBackgroundSession(string uniqueKey)
        {
            this.uniqueKey = uniqueKey;
        }

        /// <summary>
        /// Gets the session.
        /// </summary>
        /// <returns></returns>
        public IWebEasSession GetSession()
        {
            try
            {
#if DEBUG || DEVELOP
                if (System.Web.HttpContext.Current == null && IsServiceStack() && string.IsNullOrEmpty(this.uniqueKey))
                {
                    //log.Debug("Setting debug context");
                    var ses = new EsamSession();
                    ses = LoadDebugData(ses);
                    LoadRolesForUser(ses, !DebugLoadRoles);
                    return ses;
                }
#endif

                #region Specialny pripad background procesu

                if (System.Web.HttpContext.Current == null)
                {
                    if (string.IsNullOrEmpty(this.uniqueKey))
                    {
                        throw new WebEasException("No context");
                    }

                    var session = Cache.Get<EsamSession>(this.uniqueKey);
                    if (session == null)
                    {
                        throw new ArgumentNullException(string.Format("Session by {0} not found in cache", this.uniqueKey));
                    }
                    return session;
                }

                #endregion

                // Data v requeste aby sa netahali z cache
                if (System.Web.HttpContext.Current.Items["EsamSession"] == null)
                {
                    EsamSession ses = null;
                    if (IsServiceStack())
                    {
                        HttpRequest req = System.Web.HttpContext.Current.Request;
                        HttpCookie tokenCookie = req.Cookies[IamDcomToken];
                        string token = tokenCookie == null || string.IsNullOrEmpty(tokenCookie.Value) ? DebugTokenId : tokenCookie.Value;

                        ses = LoadHeadersFromServiceStack(req, token);
                    }
                    else
                    {
                        ses = LoadHeadersFromWcf();
                    }

#if DEBUG || DEVELOP

                    if ((String.IsNullOrEmpty(ses.DcomId) && String.IsNullOrEmpty(ses.TenantId)) && !string.IsNullOrEmpty(DebugTokenId))
                    {
                        ses = LoadDebugData(ses);
                    }

#endif

                    if (ses.HasUniqueKey && (WebEas.Context.Current.CurrentEndpoint == Context.EndpointType.Office || WebEas.Context.Current.CurrentEndpoint == Context.EndpointType.Unknown))
                    {
                        var cached = Cache.Get<EsamSession>(ses.UniqueKey);
                        if (cached == null) //nenaslo sa v cache
                        {
                            // ulozenie do requestu
                            System.Web.HttpContext.Current.Items["EsamSession"] = ses;
                            LoadRolesForUser(ses);

                            Cache.Set(ses.UniqueKey, ses, new TimeSpan(1, 0, 0));
                        }
                        else
                        {
                            // ulozenie do requestu                            
                            ses = cached;
                        }
                    }

                    // ulozenie do requestu
                    System.Web.HttpContext.Current.Items["EsamSession"] = ses;

                    return ses;
                }
                else
                {
                    // ziskanie z requestu
                    return (EsamSession)System.Web.HttpContext.Current.Items["EsamSession"];
                }
            }
            catch (Exception ex)
            {
                throw new WebEasException("", "Nepodarilo sa identifikovať používateľa", ex);
            }
        }

        /// <summary>
        /// Adds the role.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="role">The role.</param>
        private static void AddRole(EsamSession session, string role)
        {
            if (RoleList.Any(nav => nav.Name == role))
            {
                Role ro = RoleList.First(nav => nav.Name == role);
                foreach (string r in ro.Roles)
                {
                    session.Roles.Add(r);
                }
            }
            else
            {
                session.Roles.Add(role);
            }
        }

        /// <summary>
        /// When exists , remove role from array of roles (strings).
        /// </summary>
        /// <param name="role">The role.</param>
        private static string[] RemoveRole(string role, string[] roles)
        {
            var roleIndex = Array.IndexOf(roles, role);
            if (roleIndex != -1)
            {
                return roles.Where(w => w != roles[roleIndex]).ToArray();
            }
            return roles;
        }

        /// <summary>
        /// Loads the roles for user.
        /// </summary>
        /// <param name="session">The session.</param>
        private static void LoadRolesForUser(EsamSession session, bool debug = false)
        {
            try
            {
                if (WebEas.Context.Current.CurrentEndpoint == Context.EndpointType.Public)
                {
                    return;
                }

#if DEBUG || DEVELOP
                debug = !DebugLoadRoles;
#endif

                if (debug)
                {
                    log.Debug("Asign debug roles");
                    AddRole(session, "BDS_ADMIN");
                    AddRole(session, "DMS_ADMIN");
                    AddRole(session, "CFE_ADMIN");
                    return;
                }

                if (String.IsNullOrEmpty(session.DcomId) || String.IsNullOrEmpty(session.TenantId))
                {
                    if (String.IsNullOrEmpty(session.DcomId))
                    {
                        log.Debug("Dcom Id is not defined in role loading");
                    }
                    return;
                }
#if DEBUG
                log.Debug("Loading roles..");
#endif
                var roles = new string[] { };

                foreach (string role in roles)
                {
                    AddRole(session, role.ToUpper());
                }
#if DEBUG
                log.Debug(string.Format("Loaded roles {0} and extended to {1}..", roles.Length, session.Roles.Count));
#endif
            }
            catch (Exception ex)
            {
                throw new WebEasException(string.Format("Error in loading roles for user {0}", session.DcomId), "Nepodarilo sa získať privilégia pre používateľa", ex);
            }
        }

        /// <summary>
        /// Loads the headers from WCF.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        private static EsamSession LoadHeadersFromWcf()
        {
            try
            {
                //  log.Debug("Loading wcf header..");
                var session = new EsamSession();
                session.AccessType = WebEasAccessType.Soap;
                if (OperationContext.Current != null)
                {
                    System.ServiceModel.Channels.MessageHeaders headers = OperationContext.Current.IncomingMessageHeaders;

                    for (int i = 0; i < headers.Count; i++)
                    {
                        switch (headers[i].Name)
                        {
                            case EsbActorIDSector:
                                session.ActorIDSector = headers.GetHeader<string>(i);
                                break;
                            case EsbTenantId:
                                session.TenantId = headers.GetHeader<string>(i);
                                break;
                            case EsbUserDcomId:
                                session.DcomId = headers.GetHeader<string>(i);
                                break;
                            case EsbTechDcomId:
                                session.TechDcomId = headers.GetHeader<string>(i);
                                break;
                            case EsbTechRoles:
                                session.TechRoles.Add(headers.GetHeader<string>(i));
                                break;
                            case EsbUserSessionId:
                                session.IamDcomToken = headers.GetHeader<string>(i);
                                break;
                        }
                    }

                    // Pre ISOGateway
                    if (session.TenantId == null)
                    {
                        for (int i = 0; i < headers.Count; i++)
                        {
                            switch (headers[i].Name)
                            {
                                case "DcmHeader":
                                    XmlReader reader = headers.GetReaderAtHeader(i);
                                    if (reader.ReadToDescendant("tenantId", headers[i].Namespace) && session.TenantId == null)
                                    {
                                        session.TenantId = reader.ReadElementContentAsString();
                                        // DCOMPREV - 976 : zatial takto takyto header ma iba IsoGW
                                        session.AccessType = WebEasAccessType.IsoGw;
                                    }
                                    break;
                            }
                        }
                    }
                }

                // Urcenie typu pristupu
                if (string.IsNullOrEmpty(session.DcomId))
                {
                    if (!string.IsNullOrEmpty(session.TechDcomId))
                    {
                        session.UserType = WebEasUserType.TechUser;
                        session.DcomId = session.TechDcomId;
                    }
                    else
                    {
                        session.UserType = WebEasUserType.Anonym;
                    }
                }
                else
                {
                    session.UserType = WebEasUserType.Clerk;
                }

                return session;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in loading data from wcf header", ex);
            }
        }

        /// <summary>
        /// Loads the headers from service stack.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        private static EsamSession LoadHeadersFromServiceStack(System.Web.HttpRequest request, string token)
        {
            try
            {

                // log.Debug("Loading ss header..");
                var session = new EsamSession();
                session.AccessType = WebEasAccessType.Json;
                //token
                session.IamDcomToken = token;
                // subject
                session.SubjectDcomId = request.Headers[SubjectDcomId];
                // sector
                session.ActorIDSector = request.Headers[ActorIdsector];
                //dcomId
                session.DcomId = request.Headers[ActorDcomId];
                if (!string.IsNullOrEmpty(session.DcomId))
                {
                    session.DcomId = session.DcomId.ToUpper();
                }

                if ((session.ActorIDSector == "DCOM" && !string.IsNullOrEmpty(session.DcomId)) || !string.IsNullOrEmpty(session.SubjectDcomId))
                {
                    //language
                    session.Language = request.Headers[ActorPreferredLanguage];


                    // Rozdelenie na typ prihlaseneho
                    if (session.ActorIDSector == "DCOM")
                    {
                        // Uradnik Dcom
                        session.UserType = WebEasUserType.Clerk;
                        session.Upn = request.Headers[Upn];
                        session.Email = request.Headers[ActorEmail];
                        session.FirstName = request.Headers[ActorFirstName];
                        //session.FormattedName = request.Headers[ActorFormattedName];
                        session.LastName = request.Headers[ActorLastName];

                        //tenant - obcan neobsahuje tenanta
                        session.TenantId = request.Headers[TenantId];

                        if (string.IsNullOrEmpty(session.TenantId))
                        {
                            session.TenantId = request.Headers[ActorTenant];

                            if (string.IsNullOrEmpty(session.TenantId))
                            {
                                session.TenantId = request.Headers[ActorPreferredTenant];
                            }
                        }
                        if (!string.IsNullOrEmpty(session.TenantId))
                        {
                            session.TenantId = session.TenantId.ToUpper();
                        }
                    }
                    else
                    {
                        //Obcan
                        session.UserType = request.Headers[SubjectIdentityType] == "2" ? WebEasUserType.UserPo : WebEasUserType.UserFo;
                        session.FirstName = request.Headers[SubjectFirstName];
                        //session.FormattedName = request.Headers[SubjectFormattedName];
                        session.LastName = request.Headers[SubjectLastName];
                    }
                }
                else
                {
                    session.UserType = WebEasUserType.Anonym;
                }

                AssignId(session, request);

                return session;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in loading data from servicestack header", ex);
            }
        }

        /// <summary>
        /// Assigns the tenant id.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="request">The request.</param>
        private static void AssignId(EsamSession session, HttpRequest request)
        {
            IDictionary<string, string> queryString = null;

            #region Tenant

            //Zistenie tenanta z Query - pre Public endpoint potom pouzijem tento bez ohladu na to ci sa jedna o Obcana alebo uradnika

            const string TenantId = "tenantid";
            session.QueryTenantId = "";
            queryString = request.QueryString.AllKeys.ToDictionary(k => k.ToLowerInvariant(), k => request.QueryString[k]);

            if (queryString.ContainsKey(TenantId))
            {
                session.QueryTenantId = queryString[TenantId];
            }
            if (session.QueryTenantId.IsEmpty())
            {
                session.QueryTenantId = GetValueFromRequest(TenantId);
            }

            if (string.IsNullOrEmpty(session.TenantId) && !string.IsNullOrEmpty(session.QueryTenantId))
            {
                session.TenantId = session.QueryTenantId;
            }

            #endregion

            #region Subjekt

            if (string.IsNullOrEmpty(session.SubjectDcomId))
            {
                if (queryString == null)
                {
                    queryString = request.QueryString.AllKeys.ToDictionary(k => k.ToLowerInvariant(), k => request.QueryString[k]);
                }
                const string SubjektId = "subjektid";
                if (queryString.ContainsKey(SubjektId))
                {
                    session.SubjectDcomId = queryString[SubjektId];
                }
                else if (string.IsNullOrEmpty(session.SubjectDcomId))
                {
                    session.SubjectDcomId = GetValueFromRequest(SubjektId);
                }
            }

            #endregion

            #region Language - locale

            //if (!session.HasLanguage)
            //{
            if (queryString == null)
            {
                queryString = request.QueryString.AllKeys.ToDictionary(k => k.ToLowerInvariant(), k => request.QueryString[k]);
            }
            const string Locale = "locale";
            if (queryString.ContainsKey(Locale))
            {
                session.Language = queryString[Locale];
            }
            else if (!session.HasLanguage)
            {
                session.Language = GetValueFromRequest(Locale);
            }
            //}

            #endregion
        }

        /// <summary>
        /// Gets the value from request.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        private static string GetValueFromRequest(string name)
        {
            object dto = Context.Current.CurrentDto;
            if (dto != null && dto.GetType().GetProperties().Any(nav => nav.Name.ToLower() == name))
            {
                PropertyInfo prop = dto.GetType().GetProperties().First(nav => nav.Name.ToLower() == name);
                object val = prop.GetValue(dto);
                if (val is string)
                {
                    return val as string;
                }
                else if (val != null)
                {
                    return val.ToString();
                }
            }
            return null;
        }

        /// <summary>
        /// Loads the debug data.
        /// </summary>
        /// <param name="session">The session.</param>
        private static EsamSession LoadDebugData(EsamSession session)
        {
            var debugSessionFilePath = WebConfigurationManager.AppSettings["DebugSessionFilePath"];
            if (!session.IsAuthorized && (DebugTokenId != null || debugSessionFilePath != null))
            {
                //  log.Debug("Loading debug data..");
                session.AccessType = WebEasAccessType.Debug;
                session.UserType = WebEasUserType.Clerk;
                session.SubjectDcomId = DebugSubjectDcomId ?? "D68D8CB1-2945-46D0-A7B7-DF36DFADF433";
                session.IamDcomToken = DebugTokenId;
                session.DcomId = DebugDcomId ?? "00000000-0000-0000-0000-000000000000".ToUpper();
                session.Email = "Local test@datalan.sk";
                session.FirstName = "Local test";
                //session.FormattedName = "Test starosta Litava";
                session.Language = "sk";
                session.LastName = "Local test";             
                //session.TenantId = "B0F6447C-BDDB-4942-8498-881DAE108803".ToUpper();
                //session.TenantId = "9FF9C399-6FF3-4EFD-891A-7FFAFC5C02B4".ToUpper();
                session.TenantId = DebugTenantId ?? "00000000-0000-0000-0000-000000000000".ToUpper();

                if (!string.IsNullOrEmpty(debugSessionFilePath) && false)
                {
                    if (System.IO.File.Exists(debugSessionFilePath))
                    {
                        var json = ServiceStack.Text.JsonObject.Parse(System.IO.File.ReadAllText(debugSessionFilePath));
                        if (json.ContainsKey("Session"))
                        {
                            var ses = ServiceStack.Text.JsonSerializer.DeserializeFromString<EsamSession>(json.GetUnescaped("Session"));
                            session.SubjectDcomId = ses.SubjectDcomId;
                            session.IamDcomToken = ses.IamDcomToken;
                            session.DcomId = ses.DcomId;
                            session.TenantId = ses.TenantId;
                        }
                    }
                }

            }
            else
            {
                session.ActorIDSector = "Anonymous";
            }
            return session;
        }

        /// <summary>
        /// Determines whether [is service stack].
        /// </summary>
        /// <returns></returns>
        private static bool IsServiceStack()
        {
            return HostContext.AppHost != null;
        }
    }
}