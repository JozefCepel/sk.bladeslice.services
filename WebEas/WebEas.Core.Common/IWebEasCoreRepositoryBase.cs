using System;
using System.Linq;
using ServiceStack;

namespace WebEas.Core
{
    /// <summary>
    /// Web Eas Repository Base
    /// </summary>
    public interface IWebEasCoreRepositoryBase : ILogic, IDisposable
    {
        /// <summary>
        /// Gets the session.
        /// </summary>
        /// <value>The session.</value>
        IWebEasSession Session { get; set; }
    }
}