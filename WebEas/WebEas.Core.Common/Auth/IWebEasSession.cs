using ServiceStack.Auth;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebEas
{
    public interface IWebEasSession: IAuthSession
    {
        /// <summary>
        /// Gets or sets the tenant id.
        /// </summary>
        /// <value>The tenant id.</value>
        [DataMember]
        string TenantId { get; set; }

        /// <summary>
        /// Gets the tenant id GUID.
        /// </summary>
        /// <value>The tenant id GUID.</value
        [IgnoreDataMember]
        Guid? TenantIdGuid { get; }

        /// <summary>
        /// Tenant Name
        /// </summary>
        [DataMember]
        string TenantName { get; set; }

        /// <summary>
        /// Tenanti priradeny danemu uzivatelovi
        /// </summary>
        List<Guid> TenantIds { get; set; }

        /// <summary>
        /// Pouzivatel Id
        /// </summary>
        /// <value>The User id.</value>
        [DataMember]
        string UserId { get; set; }

        /// <summary>
        /// Gets the User id GUID.
        /// </summary>
        /// <value>The DCOM id GUID.</value>
        [IgnoreDataMember]
        Guid? UserIdGuid { get; }

        /// <summary>
        /// Admin Level
        /// </summary>
        [DataMember]
        AdminLevel AdminLevel { get; set; }

        /// <summary>
        /// Externé Id Tenanta
        /// </summary>
        [DataMember]
        Guid? D_Tenant_Id_Externe { get; set; }

        /// <summary>
        /// Gets or sets the ORS Permissions.
        /// </summary>
        /// <value>ORS Permissions - id_ORSElement = string index</value>
        [DataMember]
        string OrsPermissions { get; set; }

        [DataMember]
        Dictionary<string, string> OrsElementPermisions { get; set; }

        /// <summary>
        /// Evidencne cislo zamestnanca
        /// </summary>
        [DataMember]
        string EvidCisloZam { get; set; }

        /// <summary>
        /// Determines whether the specified role has role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        bool HasRole(string role);

        #region DCOM

        /// <summary>
        /// Dcom Token
        /// </summary>
        string IamDcomToken { get; set; }

        /// <summary>
        /// Identifikácia ISO dodávateľa
        /// </summary>
        string IsoId { get; set; }

        #endregion
    }
}