using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WebEas.Esam.ServiceModel.Office
{
    [DataContract]
    public class EsamSession : IWebEasSession
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string language;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Guid? dcomIdGuid;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Guid? subjectDcomIdGuid;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Guid? tenantIdGuid;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private HashSet<string> roles = new HashSet<string>(StringComparer.InvariantCulture);
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private WebEasUserType userType = WebEasUserType.Anonym;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private WebEasAccessType accessType = WebEasAccessType.Json;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<string> techRoles = new List<string>();

        /// <summary>
        /// Session Key
        /// </summary>
        /// <value>The DCOM token.</value>
        [DataMember]
        public string IamDcomToken { get; set; }

        /// <summary>
        /// Typ používateľa pristupujúceho do aplikácie
        /// </summary>
        /// <value>The type of the user.</value>
        [DataMember]
        public WebEasUserType UserType
        {
            get
            {
                return this.userType;
            }
            set
            {
                this.userType = value;
            }
        }

        /// <summary>
        /// Typ pristupu do aplikacie
        /// </summary>
        /// <value>The type of the access.</value>
        [DataMember]
        public WebEasAccessType AccessType
        {
            get
            {
                return this.accessType;
            }
            set
            {
                this.accessType = value;
            }
        }

        /// <summary>
        /// Gets the unique key.
        /// </summary>
        /// <value>The unique key.</value>
        [IgnoreDataMember]
        public string UniqueKey
        {
            get
            {
                return string.Format("ses:{0}:{1}:{2}:{3}:{4}", this.GetValue(TenantId), this.GetValue(DcomId), this.GetValue(SubjectDcomId), String.IsNullOrEmpty(this.IamDcomToken) ? 0 : this.IamDcomToken.GetHashCode(), Language);
                //return string.Format("ses:{0}", String.IsNullOrEmpty(this.IamDcomToken) ? String.IsNullOrEmpty(this.DcomId) && string.IsNullOrEmpty(this.TenantId) ? null : string.Format("{0}${1}", this.DcomId, this.TenantId) : string.Format("{0}${1}${2}", this.IamDcomToken, this.TenantId, string.IsNullOrEmpty(this.SubjectDcomId) ? this.DcomId : this.SubjectDcomId));
            }
        }

        /// <summary>
        /// Gets the has unique key.
        /// </summary>
        /// <value>The has unique key.</value>
        [IgnoreDataMember]
        public bool HasUniqueKey
        {
            get
            {
                return !string.IsNullOrEmpty(this.UniqueKey);
            }
        }

        /// <summary>
        /// PouzivatelId
        /// </summary>
        /// <value>The DCOM id.</value>
        [DataMember]
        public string DcomId { get; set; }

        /// <summary>
        /// Gets the DCOM id GUID.
        /// </summary>
        /// <value>The DCOM id GUID.</value>
        [IgnoreDataMember]
        public Guid? DcomIdGuid
        {
            get
            {
                if (string.IsNullOrEmpty(this.DcomId))
                {
                    return null;
                }
                if (this.dcomIdGuid.HasValue)
                {
                    return this.dcomIdGuid;
                }
                this.dcomIdGuid = new Guid(this.DcomId);
                return this.dcomIdGuid;
            }
        }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        [DataMember]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        [DataMember]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        [DataMember]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the name of the formatted.
        /// </summary>
        /// <value>The name of the formatted.</value>
        //[DataMember]
        //public string FormattedName { get; set; }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>The language.</value>
        [DataMember]
        public string Language
        {
            get
            {
                if (string.IsNullOrEmpty(this.language))
                {
                    return "sk";
                }
                return this.language;
            }
            set
            {
                this.language = value;
            }
        }

        /// <summary>
        /// Gets the has language.
        /// </summary>
        /// <value>The has language.</value>
        [IgnoreDataMember]
        public bool HasLanguage
        {
            get
            {
                return !String.IsNullOrEmpty(this.language);
            }
        }

        /// <summary>
        /// Gets or sets the tenant id.
        /// </summary>
        /// <value>The tenant id.</value>
        [DataMember]
        public string TenantId { get; set; }

        /// <summary>
        /// Gets or sets the tenant id.
        /// </summary>
        /// <value>The tenant id.</value>
        [IgnoreDataMember]
        public string QueryTenantId { get; set; }

        /// <summary>
        /// Gets or sets the actor ID sector.
        /// </summary>
        /// <value>The actor ID sector.</value>
        [DataMember]
        public string ActorIDSector { get; set; }

        /// <summary>
        /// Gets or sets the subject D COM id.
        /// </summary>
        /// <value>The subject D COM id.</value>
        [DataMember]
        public string SubjectDcomId { get; set; }

        /// <summary>
        /// Gets the subject DCOM id GUID.
        /// </summary>
        /// <value>The subject DCOM id GUID.</value>
        [IgnoreDataMember]
        public Guid? SubjectDcomIdGuid
        {
            get
            {
                if (String.IsNullOrEmpty(this.SubjectDcomId))
                {
                    return null;
                }
                if (this.subjectDcomIdGuid.HasValue)
                {
                    return this.subjectDcomIdGuid;
                }
                this.subjectDcomIdGuid = new Guid(this.SubjectDcomId);
                return this.subjectDcomIdGuid;
            }
        }

        /// <summary>
        /// Gets or sets the tech DCOM id.
        /// </summary>
        /// <value>The tech DCOM id.</value>
        [DataMember]
        public string TechDcomId { get; set; }

        /// <summary>
        /// Gets or sets the upn.
        /// </summary>
        /// <value>The upn.</value>
        [DataMember]
        public string Upn { get; set; }

        /// <summary>
        /// Gets the tenant id GUID.
        /// </summary>
        /// <value>The tenant id GUID.</value
        [IgnoreDataMember]
        public Guid? TenantIdGuid
        {
            get
            {
                if (String.IsNullOrEmpty(this.TenantId))
                {
                    return null;
                }
                if (this.tenantIdGuid.HasValue)
                {
                    return this.tenantIdGuid;
                }
                this.tenantIdGuid = new Guid(this.TenantId);
                return this.tenantIdGuid;
            }
        }

        /// <summary>
        /// Gets or sets the tech roles. Wcf
        /// </summary>
        /// <value>The tech roles.</value>
        [DataMember]
        public List<string> TechRoles
        {
            get
            {
                return this.techRoles;
            }
            set
            {
                this.techRoles = value;
            }
        }

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>The roles.</value>
        [DataMember]
        public HashSet<string> Roles
        {
            get
            {
                return this.roles;
            }
            set
            {
                this.roles = value;
            }
        }

        /// <summary>
        /// Gets the is authorized.
        /// </summary>
        /// <value>The is authorized.</value>
        [IgnoreDataMember]
        public bool IsAuthorized
        {
            get
            {
                return !string.IsNullOrEmpty(this.DcomId) || !string.IsNullOrEmpty(this.TenantId);
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("Access: {0}, User: {1}, DcomId: {2}, TenantId: {3}, Actor: {4}", this.AccessType, this.UserType, this.DcomId, this.TenantId, this.SubjectDcomId);
        }

        /// <summary>
        /// Determines whether the specified role has role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        public bool HasRole(string role)
        {
            return this.Roles.Contains(role);
        }

        private string GetValue(string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                return "-";
            }
            else
            {
                return val;
            }
        }
    }
}
