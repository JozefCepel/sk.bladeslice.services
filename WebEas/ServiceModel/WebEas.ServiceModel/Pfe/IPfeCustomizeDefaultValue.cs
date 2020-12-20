using System;
using System.Linq;

namespace WebEas.ServiceModel
{
    public interface IPfeCustomizeDefaultValue
    {
        /// <summary>
        /// Customizes the default value.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="node">The node.</param>
        void CustomizeDefaultValue(PfeColumnAttribute column, IWebEasRepositoryBase repository, HierarchyNode node);
    }
}