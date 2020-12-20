using System;
using System.Linq;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITenantFilter
    {
        string TenantId { get; set; }
    }
}