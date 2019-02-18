using System;
using System.Collections.Generic;
using System.Linq;
using WebEas.ServiceModel;

namespace WebEas.ServiceInterface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IWebEasServiceInterface
    {
        /// <summary>
        /// Gets or sets the root node.
        /// </summary>
        /// <value>The root node.</value>
        HierarchyNode RootNode { get; }

        /// <summary>
        /// Gets the code.
        /// </summary>
        /// <value>The code.</value>
        string Code { get; }
    }
}