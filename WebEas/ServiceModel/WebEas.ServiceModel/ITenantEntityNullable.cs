using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    public interface ITenantEntityNullable
    {
        /// <summary>
        /// Gets or sets the d_ tenant_ id.
        /// </summary>
        /// <value>The d_ tenant_ id.</value>
        [IgnoreDataMember]
        Guid? D_Tenant_Id { get; set; }
    }
}