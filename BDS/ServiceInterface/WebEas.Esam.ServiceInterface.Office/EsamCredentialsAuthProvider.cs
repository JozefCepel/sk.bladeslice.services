using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Dapper;
using ServiceStack.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using WebEas.Esam.ServiceModel.Office;
using WebEas.ServiceModel.Types;

namespace WebEas.Esam.ServiceInterface.Office
{
    public class EsamCredentialsAuthProvider : CredentialsAuthProviderSync
    {
        public const string TenantId = "TenantId";

        private const string ActorDcomId = "Actor.DcomID";
        private const string Upn = "upn";
        private const string ActorEmail = "Actor.Email";
        private const string ActorFormattedName = "Actor.FormattedName";
        private const string ActorFirstName = "Actor.FirstName";
        private const string ActorLastName = "Actor.LastName";
        private const string ActorIdsector = "ActorIDSector";
        private const string HasMultipleTenants = "HasMultipleTenants";
#if TEST
            private const string IamDcomTokenName = "IAMDCOMToken_TEST";
#else
        private const string IamDcomTokenName = "IAMDCOMToken";
#endif

#if DEBUG || DEVELOP
        private readonly bool dcomRezim = false;
#elif TEST || PROD
        private readonly bool dcomRezim = true;
#endif

        public override bool TryAuthenticate(ServiceStack.IServiceBase authService,
            string userName, string password)
        {
            var service = authService.ResolveService<ServiceBase>();
            var passwordHasher = HostContext.TryResolve<IPasswordHasher>();

            if (dcomRezim && userName != "IsoNotifyTechUser")
            {
                if (!service.Request.Cookies.ContainsKey(IamDcomTokenName))
                {
                    throw new WebEasUnauthorizedAccessException(null, "Token nebol najdený");
                }

                var tokenCookie = service.Request.Cookies[IamDcomTokenName];

                // kontrola na token z cookie
                if (tokenCookie == null || string.IsNullOrEmpty(tokenCookie.Value))
                {
                    throw new WebEasUnauthorizedAccessException(null, "Token nebol najdený");
                }

                if (service.Request.Headers[ActorIdsector] != "DCOM")
                {
                    throw new WebEasUnauthorizedAccessException(null, "DCOM header nie je nastavený");
                }

                return true;
            }

            var user = service.Db.Single<User>("SELECT top 1 * FROM cfe.D_User where (DatumPlatnosti is null or DatumPlatnosti > getdate()) AND LoginName = @login", new { login = userName });

            if (user == null)
            {
                throw new WebEasUnauthorizedAccessException(null, "Nesprávne meno alebo heslo");
            }

            if (user.PlatnostOd.Date > DateTime.Today || DateTime.Today > (user.PlatnostDo?.Date ?? DateTime.Today))
            {
                throw new WebEasUnauthorizedAccessException(null, "Účet má ukončenú platnosť. Kontaktuje prosím administrátora.");
            }

            var successful = passwordHasher.VerifyPassword(user.LoginPswd, password, out bool needsRehash);

            if (needsRehash)
            {
                user.LoginPswd = passwordHasher.HashPassword(password);
                service.Db.UpdateOnly(user, onlyFields: p => p.LoginPswd, where: p => p.D_User_Id == user.D_User_Id);
            }

            if (!successful)
            {
                throw new WebEasUnauthorizedAccessException(null, "Nesprávne meno alebo heslo");
            }

            return successful;
        }

        public override IHttpResult OnAuthenticated(ServiceStack.IServiceBase authService,
            IAuthSession session, IAuthTokens tokens,
            Dictionary<string, string> authInfo)
        {
            var service = authService.ResolveService<ServiceBase>();
            var ses = session as EsamSession;
            User user;

            if (dcomRezim && ses.UserAuthName != "IsoNotifyTechUser")
            {
                //Log.Warn("OnAuthenticated Headers:" + service.Request.Headers.ToDictionary().ToJson());

                var tokenCookie = service.Request.Cookies[IamDcomTokenName];
                string token = tokenCookie == null || string.IsNullOrEmpty(tokenCookie.Value) ? string.Empty : tokenCookie.Value;

                ses.IamDcomToken = token;
                ses.UserAuthName = service.Request.Headers[Upn];

                var dcomUserEmail = service.Request.Headers[ActorEmail] ?? string.Empty;
                var dcomUserFirstName = service.Request.Headers[ActorFirstName];
                var dcomUserLastName = service.Request.Headers[ActorLastName];
                var dcomUserDisplayName = service.Request.Headers[ActorFormattedName];
                var userDcomId = service.Request.Headers[ActorDcomId];
                var tenantIdFromHeader = service.Request.Headers[TenantId];
                var hasMultipleTenants = service.Request.Headers[HasMultipleTenants];

                if (!string.IsNullOrEmpty(tenantIdFromHeader))
                {
                    tenantIdFromHeader = tenantIdFromHeader.ToUpper();
                }

                CheckIsNullOrEmpty(nameof(token), token);
                CheckIsNullOrEmpty(nameof(ses.UserAuthName), ses.UserAuthName);
                CheckIsNullOrEmpty(nameof(dcomUserFirstName), dcomUserFirstName);
                CheckIsNullOrEmpty(nameof(dcomUserLastName), dcomUserLastName);
                CheckIsNullOrEmpty(nameof(userDcomId), userDcomId);
                CheckIsNullOrEmpty(nameof(tenantIdFromHeader), tenantIdFromHeader);


                ses.D_Tenant_Id_Externe = Guid.Parse(tenantIdFromHeader);
                ses.TenantId = service.Db.Single<string>("SELECT D_Tenant_Id FROM cfe.D_Tenant WHERE D_Tenant_Id_Externe = @tenantId", new { tenantId = ses.D_Tenant_Id_Externe });

                if (ses.TenantId == null)
                {
                    throw new WebEasUnauthorizedAccessException(null, $"Ext. TenantId {tenantIdFromHeader} nebol najdený");
                }

                user = service.Db.Single<User>("SELECT * FROM cfe.D_User WHERE (DatumPlatnosti is null or DatumPlatnosti > getdate()) AND D_User_Id_Externe = @userIdExt", new { userIdExt = userDcomId });

                if (user != null)
                {
                    if (!string.IsNullOrEmpty(dcomUserEmail) && dcomUserEmail != user.Email)
                    {
                        service.Db.UpdateOnly(user, onlyFields: p => p.Email, where: p => p.D_User_Id == user.D_User_Id);
                    }

                    if (!string.IsNullOrEmpty(dcomUserFirstName) && dcomUserFirstName != user.FirstName)
                    {
                        service.Db.UpdateOnly(user, onlyFields: p => p.FirstName, where: p => p.D_User_Id == user.D_User_Id);
                    }

                    if (!string.IsNullOrEmpty(dcomUserLastName) && dcomUserLastName != user.LastName)
                    {
                        service.Db.UpdateOnly(user, onlyFields: p => p.LastName, where: p => p.D_User_Id == user.D_User_Id);
                    }
                }
                else
                {
                    //zatial vytvarame pod userom ktory spracovava notifikacie
                    var newUser = new User
                    {
                        D_User_Id = Guid.NewGuid(),
                        DatumVytvorenia = DateTime.Now,
                        DatumZmeny = DateTime.Now,
                        PlatnostOd = DateTime.Now,
                        D_User_Id_Externe = Guid.Parse(userDcomId),
                        Email = dcomUserEmail,
                        FirstName = dcomUserFirstName,
                        LastName = dcomUserLastName,
                        LoginName = ses.UserAuthName,
                        EC = "ESAM",
                        Vytvoril = Guid.Parse("00000000-0000-0000-0000-000000000002")
                    };
                    CreateOrUpdateDcomUser(service.Db, newUser, ses.TenantIdGuid.Value);
                }
            }

            user = service.Db.Single<User>("SELECT * FROM cfe.D_User WHERE (DatumPlatnosti is null or DatumPlatnosti > getdate()) AND LoginName = @login", new { login = ses.UserAuthName });
            if (ses == null)
            {
                throw new WebEasUnauthorizedAccessException(null, $"Login {ses.UserAuthName} nebol najdený");
            }


            ses.UserId = user.D_User_Id.ToString();
            ses.FirstName = user.FirstName;
            ses.LastName = user.LastName;
            ses.Email = user.Email;
            ses.DisplayName = user.FullName;
            ses.FullName = user.FullName;
            ses.EvidCisloZam = user.EC;

            if (ses.UserAuthName == "IsoNotifyTechUser")
            {
                var authRequest = (Authenticate)authService.Request.Dto;
                ses.D_Tenant_Id_Externe = Guid.Parse(authRequest.Meta["TenantId"]);
                ses.TenantId = service.Db.Single<string>("SELECT D_Tenant_Id FROM cfe.D_Tenant WHERE D_Tenant_Id_Externe = @tenantId", new { tenantId = ses.D_Tenant_Id_Externe });
                SetUserTenantSession(ref ses, service, true);
                return base.OnAuthenticated(authService, session, tokens, authInfo);
            }

            SetUserTenantSession(ref ses, service);

            user.LastLogin = DateTime.Now;
            service.Db.UpdateOnly(user, onlyFields: p => p.LastLogin, where: p => p.D_User_Id == user.D_User_Id);

            //Call base method to Save Session and fire Auth/Session callbacks:
            return base.OnAuthenticated(authService, session, tokens, authInfo);
        }

        public override object Logout(ServiceStack.IServiceBase service, Authenticate request)
        {
            if (service is Service srv)
            {
                srv.Cache.Remove($"sessions:{srv.GetSessionId()}:pfe:UserTenants");
            }

            return base.Logout(service, request);
        }

        public static void CreateOrUpdateDcomUser(System.Data.IDbConnection dbConnection, User user, Guid tenantId, bool setContext = true)
        {
            using (var tran = dbConnection.OpenTransaction())
            {
                try
                {
                    if (setContext)
                    {
                        dbConnection.ExecuteNonQuery("set context_info 0x10101010101010101010101010101010");
                    }

                    var vytvorilGuid = user.Vytvoril;

                    // aj take sa stalo, namiesto emailu dame login
                    if (string.IsNullOrEmpty(user.Email))
                    {
                        user.Email = user.LoginName;
                    }

                    if (!dbConnection.Exists<User>(x => x.D_User_Id == user.D_User_Id))
                    {
                        if (!string.IsNullOrEmpty(user.LoginPswd))
                        {
                            var passwordHasher = HostContext.TryResolve<IPasswordHasher>();
                            user.LoginPswd = passwordHasher.HashPassword(user.LoginPswd);
                        }

                        dbConnection.Insert(user);
                    }
                    else
                    {
                        dbConnection.Update(user);
                    }

                    if (!dbConnection.Exists<UserTenant>(x => x.D_User_Id == user.D_User_Id && x.D_Tenant_Id == tenantId))
                    {
                        var tenantUser = new UserTenant()
                        {
                            D_User_Id = user.D_User_Id,
                            D_Tenant_Id = tenantId,
                            DatumVytvorenia = DateTime.Now,
                            DatumZmeny = DateTime.Now,
                            Vytvoril = vytvorilGuid
                        };

                        dbConnection.Insert(tenantUser);

                        foreach (var rightId in dbConnection.Select<int>("SELECT C_Right_Id FROM cfe.V_Right WHERE Kod = 'MEMBER'"))
                        {
                            dbConnection.Insert(new RightPermission()
                            {
                                C_Right_Id = rightId,
                                D_User_Id = user.D_User_Id,
                                D_Tenant_Id = tenantId,
                                DatumVytvorenia = DateTime.Now,
                                DatumZmeny = DateTime.Now,
                                Vytvoril = vytvorilGuid
                            });
                        }

                        foreach (var (modulId, kod) in dbConnection.Select<(int modulId, string kod)>("SELECT C_Modul_Id, Kod FROM cfe.V_Modul"))
                        {
                            dbConnection.Insert(new TreePermission()
                            {
                                Kod = kod,
                                C_Modul_Id = modulId,
                                Pravo = 3,
                                D_User_Id = user.D_User_Id,
                                D_Tenant_Id = tenantId,
                                DatumVytvorenia = DateTime.Now,
                                DatumZmeny = DateTime.Now,
                                Vytvoril = vytvorilGuid
                            });
                        }

                        foreach (var orsElementTypeId in dbConnection.Select<int>("SELECT C_OrsElementType_Id FROM cfe.V_OrsElementType"))
                        {
                            dbConnection.Insert(new OrsElementTypePermission()
                            {
                                C_OrsElementType_Id = orsElementTypeId,
                                Pravo = 3,
                                D_User_Id = user.D_User_Id,
                                D_Tenant_Id = tenantId,
                                DatumVytvorenia = DateTime.Now,
                                DatumZmeny = DateTime.Now,
                                Vytvoril = vytvorilGuid
                            });
                        }
                    }

                    tran.Commit();
                }
                catch (Exception e)
                {
                    tran.Rollback();
                    Core.Log.WebEasNLogExtensions.ExecuteUsingLogicalContext(() => { Log.Error("Chyba volania CreateDcomUser", e); }, request: user);
                    throw;
                }
            }
        }

        public static void SetUserTenantSession(ref EsamSession ses, Service service, bool techUser = false)
        {
            var userId = Guid.Parse(ses.UserId);

            if (!techUser)
            {
                var tenants = service.Db.Select<Guid>("SELECT D_Tenant_Id FROM cfe.D_UserTenant WHERE (DatumPlatnosti is null or DatumPlatnosti > getdate()) AND D_User_Id = @userid ORDER BY D_UserTenant_Id", new { userid = userId });

                if (!tenants.Any())
                {
                    throw new WebEasValidationException(null, $"User {ses.DisplayName} doesn't have access to any company  !");
                }

                if (string.IsNullOrEmpty(ses.TenantId))
                {
                    var lastUsedTenant = service.Cache.Get<string>($"LastUsedTenant:{ses.UserId}");
                    if (!string.IsNullOrEmpty(lastUsedTenant) && tenants.Contains(Guid.Parse(lastUsedTenant)))
                    {
                        ses.TenantId = lastUsedTenant;
                    }
                    else
                    {
                        ses.TenantId = tenants.First().ToString();
                    }
                }
                else
                {
                    if (!tenants.Contains(ses.TenantIdGuid.Value))
                    {
                        throw new WebEasValidationException(null, $"User {ses.DisplayName} doesn't have access to login!");
                    }
                }

                service.Cache.Set($"LastUsedTenant:{ses.UserId}", ses.TenantId);
                ses.TenantIds = tenants;
            }

            if (!string.IsNullOrEmpty(ses.TenantId))
            {
                ses.TenantId = ses.TenantId.ToUpper();
                var isoId = service.Db.Single<string>("SELECT IsoId FROM cfe.D_Tenant WHERE D_Tenant_Id = @tenantId", new { tenantId = ses.TenantId });
                ses.IsoId = isoId;

                byte[] tenant = Guid.Parse(ses.TenantId).ToByteArray();
                byte[] endpoint = new byte[] { (byte)Context.Current.CurrentEndpoint };
                byte[] dcomId = userId.ToByteArray();
                byte[] rola = "U".ToAsciiBytes();
                byte[] context = tenant.Concat(endpoint).Concat(dcomId).Concat(rola).ToArray();

                var cmd = service.Db.CreateCommand();
                cmd.CommandText = "SET CONTEXT_INFO @context";
                cmd.AddParam("context", context, System.Data.ParameterDirection.Input, System.Data.DbType.Binary);
                cmd.ExecuteNonQuery();

                if (!techUser)
                {
                    var permissions = service.Db.Select<string>(@"SELECT DISTINCT CONCAT(UPPER(r.ModulKod), '_', r.kod) as ModulePermissionCode
                                                                FROM cfe.V_RightUser r 
                                                                WHERE HasRight = 1 AND D_User_Id=@userid", new { userid = userId });
                    ses.Roles = permissions;

                    // SysAdmin budeme nastavovat tu, treba zabezpecit aby sa nedal nastavit z aplikacie ?
                    ses.AdminLevel = ses.Roles.Any(x => x.Contains(AdminLevel.SysAdmin.ToDescription())) ?
                        AdminLevel.SysAdmin :
                        ses.Roles.Any(x => x.Contains("CFE_" + AdminLevel.CfeAdmin.ToDescription())) ?
                        AdminLevel.CfeAdmin :
                        AdminLevel.User;

                    var (D_Tenant_Id_Externe, Nazov) = service.Db.Single<(Guid? D_Tenant_Id_Externe, string Nazov)>("SELECT D_Tenant_Id_Externe, Nazov FROM cfe.D_Tenant WHERE D_Tenant_Id = @tenantId", new { tenantId = ses.TenantId });
                    ses.D_Tenant_Id_Externe = D_Tenant_Id_Externe;
                    ses.TenantName = Nazov;
                    ses.OrsPermissions = service.Db.Query<string>("EXEC [cfe].[PR_GetOrsReadPermissions]").Join("");
                    ses.OrsElementPermisions = new Dictionary<string, string>();

                    var elPrava = service.Db.Query<Tuple<int, int, byte>>($@"SELECT C_OrsElementType_Id as Item1, IdValue as Item2, PravoReal as Item3 
                                                                             FROM [cfe].V_OrsElementUser 
                                                                             WHERE IsElementPravo = 1 AND PravoReal > 0 AND D_User_Id = '{userId}'");
                    foreach (var elPravo in elPrava)
                    {
                        string dKey = $"ORS_{elPravo.Item1}_"; // {((elPravo.Item3 == 3)? "F" : ((elPravo.Item3 == 2)? "W" : "R"))}";

                        string oldString = "";
                        if (elPravo.Item3 == 3)
                        {
                            ses.OrsElementPermisions.TryGetValue(dKey + "F", out oldString);
                            if (string.IsNullOrEmpty(oldString))
                                oldString = ",";

                            ses.OrsElementPermisions[dKey + "F"] = oldString + elPravo.Item2 + ",";
                        }

                        if (elPravo.Item3 >= 2)
                        {
                            ses.OrsElementPermisions.TryGetValue(dKey + "W", out oldString);
                            if (string.IsNullOrEmpty(oldString))
                                oldString = ",";

                            ses.OrsElementPermisions[dKey + "W"] = oldString + elPravo.Item2 + ",";
                        }

                        if (elPravo.Item3 >= 1)
                        {
                            ses.OrsElementPermisions.TryGetValue(dKey + "R", out oldString);
                            if (string.IsNullOrEmpty(oldString))
                                oldString = ",";

                            ses.OrsElementPermisions[dKey + "R"] = oldString + elPravo.Item2 + ",";
                        }
                    }
                }
            }
        }

        private void CheckIsNullOrEmpty(string name, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new WebEasUnauthorizedAccessException(null, $"{name} nebol najdený");
            }
        }
    }
}
