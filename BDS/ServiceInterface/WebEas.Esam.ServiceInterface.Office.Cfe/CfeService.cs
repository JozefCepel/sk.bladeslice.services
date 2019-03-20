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

        public object Get(ListDto request)
        {
            return base.GetList(request);
        }

        public object Get(ListComboDto request)
        {
            return this.Repository.GetListCombo(request);
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
            return Repository.LongOperationStart(request.OperationName, request.OperationParameters, request.OperationInfo);
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

        // ----------------------------------------------------------

        #region Tenant

        public object Any(UpdateTenant request)
        {
            return Repository.Update<TenantView>(request);
        }

        #endregion

        #region User

        public void Any(CreateUser request)
        {
            Repository.CreateUser(request);  // specialita:  GUID ako primary key
        }

        public object Any(UpdateUser request)
        {
            return Repository.Update<UserView>(request);
        }

        public void Any(DeleteUser request)
        {
            Repository.Delete<User>(request.D_User_Id);
        }

        #endregion

        #region Role

        public object Any(CreateRole request)
        {
            return Repository.Create<RoleView>(request);
        }

        public object Any(UpdateRole request)
        {
            return Repository.Update<RoleView>(request);
        }

        public void Any(DeleteRole request)
        {
            Repository.Delete<Role>(request.C_Role_Id);
        }

        #endregion

        #region RoleUsers

        public object Any(CreateRoleUsers request)
        {
            return Repository.Create<RoleUsersView>(request);
        }

        public object Any(UpdateRoleUsers request)
        {
            return Repository.Update<RoleUsersView>(request);
        }

        public void Any(DeleteRoleUsers request)
        {
            Repository.Delete<RoleUsers>(request.D_RoleUsers_Id);
        }

        #endregion

        #region TenantUsers

        public object Any(CreateTenantUsers request)
        {
            return Repository.Create<TenantUsersView>(request);
        }

        public object Any(UpdateTenantUsers request)
        {
            return Repository.UpdateTenantUsers(request);  // specialita: UPDATE D_Tenant_Id
        }

        public void Any(DeleteTenantUsers request)
        {
            Repository.Delete<TenantUsers>(request.D_TenantUsers_Id);
        }

        #endregion
       
    }
}
