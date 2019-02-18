using System;
using System.Linq;

namespace WebEas.Core
{
    /// <summary>
    /// Web Eas Session Provider
    /// </summary>
    public interface IWebEasSessionProvider
    {
        /// <summary>
        /// Gets the session.
        /// </summary>
        /// <returns></returns>
        IWebEasSession GetSession();

        /// <summary>
        /// Sets the background session.
        /// </summary>
        /// <param name="uniqueKey">The unique key.</param>
        void SetBackgroundSession(string uniqueKey);
    }
}