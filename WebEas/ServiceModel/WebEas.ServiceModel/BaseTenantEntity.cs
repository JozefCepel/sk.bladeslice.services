using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// Pouziva pri filtrovani a zadavani tenanta
    /// </summary>
    [DataContract]
    [Serializable]
    public abstract class BaseTenantEntity : BaseEntity, IBaseTenantEntity
    {
        /// <summary>
        /// Gets or sets the d_ tenant_ id.
        /// </summary>
        /// <value>The d_ tenant_ id.</value>
        [IgnoreDataMember]
        public Guid D_Tenant_Id { get; set; }
    }
}