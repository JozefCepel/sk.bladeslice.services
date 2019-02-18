using System;
using System.Linq;

namespace WebEas.Core
{
    public interface IWebEasCoreServiceBase
    {
        /// <summary>
        /// Gets or sets the repository.
        /// </summary>
        /// <value>The repository.</value>
        IWebEasCoreRepositoryBase Repository { get; }
    }
}