using System;
using System.Linq;

namespace WebEas.ServiceModel
{ 
    /// <summary>
    /// Pouziva ak potrebujeme filtrovat na tenanta
    /// </summary>
    public interface IBaseTenantEntityNullable : IBaseEntity, ITenantEntityNullable
    {
    }
}