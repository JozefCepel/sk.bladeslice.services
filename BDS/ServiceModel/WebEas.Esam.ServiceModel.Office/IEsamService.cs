using System;
using System.Linq;
using WebEas.Core;

namespace WebEas.Esam.ServiceModel.Office
{
    public interface IEsamService : IWebEasCoreServiceBase
    {
        /// <summary>
        /// Gets or sets the repository.
        /// </summary>
        /// <value>The repository.</value>
        new IRepositoryBase Repository { get; }
    }
}