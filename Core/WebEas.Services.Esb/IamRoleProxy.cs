using System;
using System.Diagnostics;
using System.Linq;
using WebEas.Services.Esb.Iam.Role;

namespace WebEas.Services.Esb
{
    /// <summary>
    /// Iam Role Proxy
    /// </summary>
    /// <example>
    /// using(IamRoleProxy proxy = new IamRoleProxy())
    /// {
    /// }
    /// </example>
    public class IamRoleProxy : ProxyBase, IDisposable
    {
        private const string ServiceUrl = "https://lbsoa.intra.dcom.sk/soa/iam/1.1/roleservice";

        private Iam.Role.RolePortChannel proxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="IamRoleProxy" /> class.
        /// </summary>
        public IamRoleProxy(string stsThumbprint)
            : base(stsThumbprint)
        {
            this.proxy = WebEas.Core.Sts.SecurityTokenServiceHelper.CreateChannelProxy<Iam.Role.RolePortChannel>(ServiceUrl, stsThumbprint);            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IamRoleProxy" /> class.
        /// </summary>
        public IamRoleProxy(string openAmSessionIs, string tenantId, string stsThumbprint)
            : base(stsThumbprint)
        {
            this.proxy = WebEas.Core.Sts.SecurityTokenServiceHelper.CreateChannelProxy<Iam.Role.RolePortChannel>(ServiceUrl, openAmSessionIs, tenantId, stsThumbprint);
        }

        /// <summary>
        /// Gets the user roles.
        /// </summary>
        /// <param name="userGuid">The user GUID.</param>
        /// <param name="tenantId">The tenant id.</param>
        /// <returns></returns>
        public string[] GetUserRoles(string userGuid, string tenantId)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                if (String.IsNullOrEmpty(userGuid))
                {
                    throw new ArgumentNullException("User Guid not defined");
                }
                if (String.IsNullOrEmpty(tenantId))
                {
                    throw new ArgumentNullException("TenantId not defined");
                }

                userGuid = userGuid.ToLower();
                tenantId = tenantId.ToLower();

                var rolesInType = new Iam.Role.GetUserRolesInType();
                rolesInType.userGuid = userGuid;
                rolesInType.tenantId = tenantId;
                var request = new Iam.Role.getUserRolesRequest(rolesInType);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                Iam.Role.getUserRolesResponse response = this.proxy.getUserRoles(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.roles;
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetUserRoles", ex, userGuid, tenantId);
            }
        }

        /// <summary>
        /// Gets the role users.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <param name="tenantId">The tenant id.</param>
        /// <returns></returns>
        public WebEas.Services.Esb.Iam.Role.UserType[] GetRoleUsers(string roleName, string tenantId)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                if (String.IsNullOrEmpty(tenantId))
                {
                    throw new ArgumentNullException("TenantId not defined");
                }

                tenantId = tenantId.ToLower();

                var request = new getRoleUsersRequest(new GetRoleUsersInType { roleName = roleName, tenantId = tenantId });
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                getRoleUsersResponse response = this.proxy.getRoleUsers(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.users;
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetRoleUsers", ex, roleName, tenantId);
            }
        }

        /// <summary>
        /// Determines whether [is user in role] [the specified user GUID].
        /// </summary>
        /// <param name="userGuid">The user GUID.</param>
        /// <param name="roleName">Name of the role.</param>
        /// <param name="tenantId">The tenant id.</param>
        /// <returns></returns>
        public bool IsUserInRole(string userGuid, string roleName, string tenantId)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                if (String.IsNullOrEmpty(userGuid))
                {
                    throw new ArgumentNullException("User Guid not defined");
                }
                if (String.IsNullOrEmpty(tenantId))
                {
                    throw new ArgumentNullException("TenantId not defined");
                }

                userGuid = userGuid.ToLower();
                tenantId = tenantId.ToLower();

                var request = new isUserInRoleRequest(new IsUserInRoleInType { userGuid = userGuid, roleName = roleName, tenantId = tenantId });
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                isUserInRoleResponse response = this.proxy.isUserInRole(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.value;
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in isUserInRole", ex, userGuid, roleName, tenantId);
            }
        }

        /// <summary>
        /// Gets the user tenants.
        /// </summary>
        /// <param name="userGuid">The user GUID.</param>
        /// <returns>list of tenants (GUIDs)</returns>
        public string[] GetUserTenants(string userGuid)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                if (String.IsNullOrEmpty(userGuid))
                {
                    throw new ArgumentNullException("User Guid not defined");
                }

                var request = new Iam.Role.getUserTenantsRequest(userGuid.ToLower());
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                Iam.Role.getUserTenantsResponse response = this.proxy.getUserTenants(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.tenants;
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetUserTenants", ex, userGuid);
            }
        }

        /// <summary>
        /// Gets GetUnionRolesForMuniciality.
        /// </summary>
        /// <param name="userGuid">The municipality GUID.</param>
        /// <returns>list of UnionRoleType</returns>
        public WebEas.Services.Esb.Iam.Role.UnionRoleType[] GetUnionRolesForMunicipality(string municipalityGuid)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                if (String.IsNullOrEmpty(municipalityGuid))
                {
                    throw new ArgumentNullException("Municiality Guid not defined");
                }

                var request = new Iam.Role.getUnionRolesForMunicipalityRequest(municipalityGuid.ToLower());
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                Iam.Role.getUnionRolesForMunicipalityResponse response = this.proxy.getUnionRolesForMunicipality(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.unionRoles;
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetUserTGetUnionRolesForMunicialityenants", ex, municipalityGuid);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (this.proxy != null && this.proxy is IDisposable)
                {
                    if (this.proxy.State == System.ServiceModel.CommunicationState.Faulted)
                        this.proxy.Abort();

                    ((IDisposable)this.proxy).Dispose();
                }
                this.proxy = null;
            }
            catch (Exception ex)
            {
                throw new WebEasException("Error in dispose", ex);
            }
        }
    }
}