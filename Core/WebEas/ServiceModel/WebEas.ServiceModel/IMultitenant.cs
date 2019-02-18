using System;
using System.Linq;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// Tenant je povinny
    /// </summary>
    public interface IMultitenant
    {
        /// <summary>
        /// Gets or sets the d_ tenant id.
        /// </summary>
        /// <value>The d_ tenant id.</value>
        long D_TenantId { get; set; }
    }
}