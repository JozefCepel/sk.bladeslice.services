using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas
{
    public interface IWebEasSession
    {
        /// <summary>
        /// Session Key
        /// </summary>
        /// <value>The DCOM token.</value>
        [DataMember]
        string IamDcomToken { get; set; }

        /// <summary>
        /// Typ používateľa pristupujúceho do aplikácie
        /// </summary>
        /// <value>The type of the user.</value>
        [DataMember]
        WebEasUserType UserType { get; set; }
        
        /// <summary>
        /// Typ pristupu do aplikacie
        /// </summary>
        /// <value>The type of the access.</value>
        [DataMember]
        WebEasAccessType AccessType { get; set; }

        /// <summary>
        /// Gets the unique key.
        /// </summary>
        /// <value>The unique key.</value>
        [IgnoreDataMember]
        string UniqueKey { get; }

        /// <summary>
        /// Gets the has unique key.
        /// </summary>
        /// <value>The has unique key.</value>
        [IgnoreDataMember]
        bool HasUniqueKey { get; }
        
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        [DataMember]
        string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        [DataMember]
        string LastName { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        [DataMember]
        string Email { get; set; }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>The language.</value>
        [DataMember]
        string Language { get; set; }

        /// <summary>
        /// Gets the has language.
        /// </summary>
        /// <value>The has language.</value>
        [IgnoreDataMember]
        bool HasLanguage { get; }

        /// <summary>
        /// Gets or sets the tenant id.
        /// </summary>
        /// <value>The tenant id.</value>
        [DataMember]
        string TenantId { get; set; }

        /// <summary>
        /// Gets or sets the tenant id.
        /// </summary>
        /// <value>The tenant id.</value>
        [IgnoreDataMember]
        string QueryTenantId { get; set; }

        /// <summary>
        /// Gets the tenant id GUID.
        /// </summary>
        /// <value>The tenant id GUID.</value
        [IgnoreDataMember]
        Guid? TenantIdGuid { get; }


        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>The roles.</value>
        [DataMember]
        HashSet<string> Roles { get; set; }

        /// <summary>
        /// Gets the is authorized.
        /// </summary>
        /// <value>The is authorized.</value>
        [IgnoreDataMember]
        bool IsAuthorized { get; }

        /// <summary>
        /// Determines whether the specified role has role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        bool HasRole(string role);

        /// <summary>
        /// PouzivatelId
        /// </summary>
        /// <value>The DCOM id.</value>
        [DataMember]
        string DcomId { get; set; }

        /// <summary>
        /// Gets the DCOM id GUID.
        /// </summary>
        /// <value>The DCOM id GUID.</value>
        [IgnoreDataMember]
        Guid? DcomIdGuid { get; }

        /// <summary>
        /// Gets the subject DCOM id GUID.
        /// </summary>
        /// <value>The subject DCOM id GUID.</value>
        [IgnoreDataMember]
        Guid? SubjectDcomIdGuid { get; }

        /// <summary>
        /// Gets or sets the subject D COM id.
        /// </summary>
        /// <value>The subject D COM id.</value>
        [DataMember]
        string SubjectDcomId { get; set; }
    }
}