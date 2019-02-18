using System;
using System.Linq;

namespace WebEas
{
    /// <summary>
    /// Proxy exception
    /// </summary>
    public class WebEasProxyException : WebEasException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasProxyException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public WebEasProxyException(string message)
            : this(message, null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasProxyException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="parameters">The parameters.</param>
        public WebEasProxyException(string message, System.Exception innerException, params object[] parameters)
            : this(message, null, innerException, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasProxyException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="messageUser">The message user.</param>
        /// <param name="parameters">The parameters.</param>
        public WebEasProxyException(string message, string messageUser, params object[] parameters)
            : this(message, messageUser, null, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasProxyException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="messageUser">The message user.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="parameters">The parameters.</param>
        public WebEasProxyException(string message, string messageUser, System.Exception innerException, params object[] parameters)
            : base(message, messageUser, innerException)
        {
            this.StatusCode = System.Net.HttpStatusCode.Conflict;
            this.Parameters = parameters;
        }
    }
}