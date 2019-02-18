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
        /// Gets or sets the STS thumb print.
        /// </summary>
        /// <value>The STS thumb print.</value>
        string StsThumbPrint { get; set; }

        /// <summary>
        /// Gets the session.
        /// </summary>
        /// <value>The session.</value>
        IWebEasSession Session { get; }
    }
}