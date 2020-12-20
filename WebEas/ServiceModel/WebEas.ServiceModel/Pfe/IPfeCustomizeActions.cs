using System;
using System.Collections.Generic;
using System.Linq;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPfeCustomizeActions
    {
        /// <summary>
        /// Customizes the actions.
        /// </summary>
        /// <param name="actions">The actions.</param>
        /// <param name="repository">The repository.</param>
        /// <returns></returns>
        List<NodeAction> CustomizeActions(List<NodeAction> actions, IWebEasRepositoryBase repository);
    }
}