using System;
using System.Linq;

namespace WebEas.ServiceModel
{
    public interface IHybridMultitenant
    {
        /// <summary>
        /// Gets or sets the d_ tenant_ id.
        /// </summary>
        /// <value>The d_ tenant_ id.</value>
        long? D_Tenant_Id { get; set; }
    }
}