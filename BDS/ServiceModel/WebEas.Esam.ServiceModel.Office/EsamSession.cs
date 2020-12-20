using ServiceStack;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebEas.Esam.ServiceModel.Office
{
    [DataContract]
    public class EsamSession : AuthUserSession, IWebEasSession
    {
        private Guid? userIdGuid;
        private Guid? tenantIdGuid;
        private string tenantId;
        private string userId;

        /// <summary>
        /// PouzivatelId
        /// </summary>
        /// <value>The DCOM id.</value>
        [DataMember]
        public string UserId
        {
            get => userId;
            set
            {
                userIdGuid = null;
                userId = value;
            }
        }

        /// <summary>
        /// Gets the DCOM id GUID.
        /// </summary>
        /// <value>The DCOM id GUID.</value>
        [IgnoreDataMember]
        public Guid? UserIdGuid
        {
            get
            {
                if (string.IsNullOrEmpty(UserId))
                {
                    return null;
                }

                if (userIdGuid.HasValue)
                {
                    return userIdGuid;
                }

                userIdGuid = new Guid(UserId);
                return userIdGuid;
            }
        }

        [DataMember]
        public string TenantId
        {
            get => tenantId;
            set
            {
                tenantIdGuid = null;
                tenantId = value;
            }
        }

        [IgnoreDataMember]
        public Guid? TenantIdGuid
        {
            get
            {
                if (string.IsNullOrEmpty(TenantId))
                {
                    return null;
                }

                if (tenantIdGuid.HasValue)
                {
                    return tenantIdGuid;
                }

                tenantIdGuid = new Guid(TenantId);
                return tenantIdGuid;
            }
        }

        /// <summary>
        /// Tenant Name
        /// </summary>
        [DataMember]
        public string TenantName { get; set; }

        [DataMember]
        public List<Guid> TenantIds { get; set; }

        /// <summary>
        /// Determines whether the specified role has role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        public bool HasRole(string role)
        {
            return Roles?.Contains(role) ?? false;
        }

        [DataMember]
        public AdminLevel AdminLevel { get; set; }

        [DataMember]
        public Guid? D_Tenant_Id_Externe { get; set; }

        [DataMember]
        public string OrsPermissions { get; set; }

        [DataMember]
        public Dictionary<string, string> OrsElementPermisions { get; set; }

        [DataMember]
        public string EvidCisloZam { get; set; }

        #region DCOM

        /// <summary>
        /// Dcom Token
        /// </summary>
        [DataMember]
        public string IamDcomToken { get; set; }

        /// <summary>
        /// Identifikácia ISO dodávateľa
        /// </summary>
        [DataMember]
        public string IsoId { get; set; }

        #endregion
    }
}
