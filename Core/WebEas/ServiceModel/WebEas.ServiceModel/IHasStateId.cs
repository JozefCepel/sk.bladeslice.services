using System;
using System.Linq;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// Pouziva ak potrebujeme filtrovat na tenanta
    /// </summary>
    public interface IHasStateId
    {
        /// <summary>
        /// Gets or sets the C_StavEntity_Id.
        /// </summary>
        /// <value>The C_StavEntity_Id.</value>
        int C_StavEntity_Id { get; set; }
        
    }
}