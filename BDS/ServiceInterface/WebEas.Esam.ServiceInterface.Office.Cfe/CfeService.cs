using ServiceStack;
using ServiceStack.OrmLite;
using System.Collections.Generic;
using System.Linq;
using WebEas.Esam.ServiceModel.Office.Cfe.Dto;
using WebEas.Esam.ServiceModel.Office.Cfe.Types;
using WebEas.ServiceModel.Types;

namespace WebEas.Esam.ServiceInterface.Office.Cfe
{
    public partial class CfeService : ServiceBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CfeService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public CfeService(ICfeRepository repository)
            : base(repository)
        {
        }

        /// <summary>
        /// Gets or sets the repository.
        /// </summary>
        /// <value>The repository.</value>
        public new ICfeRepository Repository
        {
            get
            {
                return (ICfeRepository)repository;
            }
        }

        #region Globalne, ale musia byt v moduloch

        public object Get(AppStatus.HealthCheckDto request)
        {
            return GetHealthCheck(request);
        }

        public object Any(ListDto request)
        {
            return base.GetList(request);
        }

        public object Get(ListComboDto request)
        {
            var comboList = this.Repository.GetListCombo(request);

            // ak nie je rola ESAM_ADMIN, vyber tenanta obmedzit na moj tenant
            if (request.KodPolozky == "cfe-def-ten-users" && Repository.Session.AdminLevel != AdminLevel.SysAdmin)
            {
                var myTenants = Repository.GetMyTenantsIDs().Select(t => t.ToString());

                if (request.Column == "tenantname")
                {
                    comboList = comboList.Where(cl => (cl as IEnumerable<KeyValuePair<string, object>>).Any(kp => kp.Key == "id" && myTenants.Contains(kp.Value.ToString()))).ToList();
                }
                if (request.Column == "username")
                {
                    var usersOnMyTenannts = Repository.GetMyTenantsUsersIDs().Select(t => t.ToString());
                    comboList = comboList.Where(cl => (cl as IEnumerable<KeyValuePair<string, object>>).Any(kp => kp.Key == "id" && usersOnMyTenannts.Contains(kp.Value.ToString()))).ToList();
                }
            }

            if (request.KodPolozky == "cfe-admin-users" && Repository.Session.AdminLevel != AdminLevel.SysAdmin)
            {
                if (request.Column == "parentfullname")
                {
                    var myTenantUsers = Repository.GetMyTenantsUsersIDs().Select(t => t.ToString());

                    comboList = comboList.Where(cl => (cl as IEnumerable<KeyValuePair<string, object>>).Any(kp => kp.Key == "id" && myTenantUsers.Contains(kp.Value.ToString()))).ToList();
                }
            }
            return comboList;
        }

        public void Post(ChangeStateDto request)
        {
            this.Repository.ChangeState(request);
        }


        public object Post(GetTreeCountsDto request)
        {
            return base.GetTreeCounts(request);
        }

        public object Get(DataChangesRequest request)
        {
            return Repository.GetTableLogging(request.Code, request.RowId);
        }

        #endregion

        #region Globalne, Long operations

        public object Post(LongOperationStartDto request)
        {
            return Repository.LongOperationStart(request);
        }

        public object Post(LongOperationRestartDto request)
        {
            return Repository.LongOperationRestart(request.ProcessKey);
        }

        public object Get(LongOperationProgressDto request)
        {
            return Repository.LongOperationStatus(request.ProcessKey);
        }

        public object Get(LongOperationResultDto request)
        {
            return Repository.LongOperationResult(request.ProcessKey);
        }

        public object Post(LongOperationCancelDto request)
        {
            return Repository.LongOperationCancel(request.ProcessKey, request.NotRemove);
        }

        public object Get(LongOperationListDto request)
        {
            return Repository.LongOperationList(request.PerTenant, request.Skip, request.Take);
        }

        #endregion

        #region Default row values

        public object Any(RowDefaultValues request)
        {
            return Repository.GetRowDefaultValues(request.code, request.masterCode, request.masterRowId);
        }

        #endregion

        #region Nastavenie

        public WebEas.ServiceModel.Types.NastavenieView Any(UpdateNastavenie request)
        {
            return Repository.UpdateNastavenie(request);
        }

        #endregion

        // ----------------------------------------------------------

        #region Tenant

        public TenantView Any(CreateTenant request)
        {
            return Repository.CreateTenant(request); // specialita:  GUID ako primary key

        }

        public TenantView Any(UpdateTenant request)
        {
            return Repository.UpdateTenant(request);
        }

        public void Any(DeleteTenant request)
        {
            Repository.Delete<Tenant>(request.D_Tenant_Id);
        }

        #endregion

        #region User

        public UserView Any(CreateUser request)
        {
            return Repository.CreateUser(request);  // specialita:  GUID ako primary key
        }

        public UserView Any(UpdateUser request)
        {
            return Repository.Update<UserView>(request);
        }

        public object Any(ChangePassword request)
        {
            if (!string.IsNullOrEmpty(request.newPassword))
            {
                var passwordHasher = HostContext.TryResolve<ServiceStack.Auth.IPasswordHasher>();
                //zatial takto
                var user = Repository.GetList<User>(x => x.D_User_Id == request.D_User_Id).FirstOrDefault();

                if (user != null)
                {
                    if (request.D_User_Id ==  Repository.Session.UserIdGuid.Value) //Ak je D_User_Id zhodné s prihláseným userom, tak musí staré heslo súhlasiť.
                    {
                        var successful = passwordHasher.VerifyPassword(user.LoginPswd, request.oldPassword, out bool needsRehash);
                        if (!successful)
                        {
                            throw new WebEasException(null, "Zadané pôvodné heslo sa nezhoduje s heslom uloženým v databáze. Ak heslo nepoznáte, kontaktujte svojho administrátora modulu 'Administrácia'.");
                        }
                    }
                    else
                    {
                        //Ak je user rôzny od prihláseného, explicitne overiť či je CFE - ADMIN = TRUE
                        if (Repository.Session.AdminLevel != AdminLevel.CfeAdmin && Repository.Session.AdminLevel != AdminLevel.SysAdmin)
                        {
                            throw new WebEasException(null, "Zmenu hesla iného používatela môže vykonávať iba administrátor modulu 'Administrácia'");
                        }
                    }
                    user.LoginPswd = passwordHasher.HashPassword(request.newPassword);
                    Repository.Update(user);
                    return true;
                }
                else
                {
                    throw new WebEasException(null, "Používateľ nenájdený");
                }
            }
            else
            {
                throw new WebEasException(null, "Nové heslo musí byť vyplnené");
            }
        }

        public void Any(BlockUser request)
        {
            Repository.BlockUser(request);
        }

        public void Any(CopyUserPermissions request)
        {
            Repository.CopyUserPermissions(request);
        }

        public void Any(DeleteUser request)
        {
            Repository.Delete<User>(request.D_User_Id);
            Repository.InvalidateTreeCountsForPath("cfe-admin-users");
        }

        #endregion

        #region Role

        public RoleView Any(CreateRole request)
        {
            var res =  Repository.Create<RoleView>(request);
            Repository.InvalidateTreeCountsForPath("cfe-admin-role");
            return res;
        }

        public RoleView Any(UpdateRole request)
        {
            return Repository.Update<RoleView>(request);
        }

        public void Any(DeleteRole request)
        {
            Repository.Delete<Role>(request.C_Role_Id);
            Repository.InvalidateTreeCountsForPath("cfe-admin-role");
        }

        #endregion

        #region UserRole

        public UserRoleView Any(CreateUserRole request)
        {
            return Repository.Create<UserRoleView>(request);
        }

        public UserRoleView Any(UpdateUserRole request)
        {
            return Repository.Update<UserRoleView>(request);
        }

        public void Any(DeleteUserRole request)
        {
            Repository.Delete<UserRole>(request.D_UserRole_Id);
        }

        #endregion

        #region UserTenant

        internal void RefreshSessionAndClearCache(System.Guid userId)
        {
            var sessionPattern = IdUtils.CreateUrn<ServiceStack.Auth.IAuthSession>(string.Empty); //= urn:iauthsession:
            var sessionKeys = Cache.GetKeysStartingWith(sessionPattern).ToList();
            if (sessionKeys.Any())
            {
                var allSessions = Cache.GetAll<ServiceModel.Office.EsamSession>(sessionKeys);
                foreach (var ses in allSessions.Where(x => x.Value.UserIdGuid == userId))
                {
                    if (ses.Value.AdminLevel != AdminLevel.SysAdmin)
                    {
                        Request.RemoveSession(ses.Value.Id);
                    }
                    
                    Repository.Cache.Remove($"sessions:{ses.Value.Id}:pfe:UserTenants");

                    //TODO: ak budeme cez server events poslat na FE info o zmene tenantovi, pouzit tento kod na zmenu session
                    /*
                    
                    var actualuserTenants = Repository.GetList<UserTenantView>(x => x.D_User_Id == userId).Select(x => x.D_Tenant_Id).ToList();
                    var session = ses.Value;
                    if (!actualuserTenants.Any())
                    {
                        Request.RemoveSession(session.Id);
                        continue;
                    }

                    if (actualuserTenants.Any() && !actualuserTenants.Contains(session.TenantIdGuid.Value))
                    {
                        session.TenantId = actualuserTenants.First().ToString();
                    }

                    EsamAppHostBase.CustomCredentialsAuthProvider.SetUserTenantSession(ref session, this);
                    Request.SaveSession(session);

                    Repository.Cache.Remove($"sessions:{session.Id}:pfe:UserTenants");*/
                }
            }
        }

        public UserTenantView Any(CreateUserTenant request)
        {
            var res = Repository.Create<UserTenantView>(request);
            RefreshSessionAndClearCache(request.D_User_Id);
            return res;
        }

        public UserTenantView Any(UpdateUserTenant request)
        {
            var res = Repository.UpdateTenantUsers(request);  // specialita: UPDATE D_Tenant_Id
            RefreshSessionAndClearCache(request.D_User_Id);
            return res;
        }

        public void Any(DeleteUserTenant request)
        {
            var users = Repository.GetList(Db.From<UserTenant>().Where(x => request.D_UserTenant_Id.Contains(x.D_UserTenant_Id)).Select(x => x.D_User_Id));
            Repository.Delete<UserTenant>(request.D_UserTenant_Id);
            foreach (var user in users)
            {
                RefreshSessionAndClearCache(user.D_User_Id);
            }
        }

        #endregion

        #region OrsElementTypePermission

        public void Any(OETSetRightNo request)
        {
            Repository.UpdateOrsElementTypePermissions(request.IDs, 0);
        }

        public void Any(OETSetRightRead request)
        {
            Repository.UpdateOrsElementTypePermissions(request.IDs, 1);
        }

        public void Any(OETSetRightUpdate request)
        {
            Repository.UpdateOrsElementTypePermissions(request.IDs, 2);
        }

        public void Any(OETSetRightFull request)
        {
            Repository.UpdateOrsElementTypePermissions(request.IDs, 3);
        }

        #endregion

        #region OrsElementPermission
        public void Any(OESetRightNo request)
        {
            Repository.UpdateOrsElementPermissions(request.IDs, 0);
        }

        public void Any(OESetRightRead request)
        {
            Repository.UpdateOrsElementPermissions(request.IDs, 1);
        }

        public void Any(OESetRightUpdate request)
        {
            Repository.UpdateOrsElementPermissions(request.IDs, 2);
        }

        public void Any(OESetRightFull request)
        {
            Repository.UpdateOrsElementPermissions(request.IDs, 3);
        }
        #endregion

        #region OrsElementType
        public void Any(ObnovitZoznamORS request)
        {
            //Repository.Update<OrsElementTypeView>(request);
            Repository.UpdateORSElement(request);
        }
        #endregion

        #region OrsElement

        //public OrsElementView Any(CreateOrsElement request)
        //{
        //    Repository.Create<OrsElementView>(request);
        //}

        //public OrsElementView Any(UpdateOrsElement request)
        //{
        //    return Repository.Update<OrsElementView>(request);
        //}

        //public void Any(DeleteOrsElement request)
        //{
        //    Repository.Delete<OrsElementTypeView>(request.C_OrsElementType_Id);
        //}

        #endregion

        #region RightPermission

        public void Any(AddRightPermission request)
        {
            Repository.AddRightPermissions(request.IDs);
        }

        public void Any(RemoveRightPermission request)
        {
            Repository.RemoveRightPermissions(request.IDs);
        }

        #endregion

        #region Tree

        public void Any(RefreshModuleTree request)
        {
            Repository.RefreshModuleTree(request.IDs);
        }

        //public void Any(DeleteTree request)
        //{
        //    Repository.Delete<TreeView>(request.C_Tree_Id);
        //}

        #endregion

        #region TreePermission
        public void Any(TreeSetRightNo request)
        {
            Repository.UpdateTreePermissions(request.IDs, 0);
        }

        public void Any(TreeSetRightRead request)
        {
            Repository.UpdateTreePermissions(request.IDs, 1);
        }

        public void Any(TreeSetRightUpdate request)
        {
            Repository.UpdateTreePermissions(request.IDs, 2);
        }

        public void Any(TreeSetRightFull request)
        {
            Repository.UpdateTreePermissions(request.IDs, 3);
        }

        #endregion

        #region DCOM

        public void Post(SynchronizeDcomUsersDto request)
        {
            //Repository.SynchronizeDcomUsers(request);
        }

        #endregion
    }
}
