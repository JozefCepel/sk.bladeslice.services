using System;
using System.Linq;

namespace WebEas
{
    /// <summary>
    /// Web Eas Authentication Exception
    /// </summary>
    public class WebEasAuthenticationException : WebEasException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasAuthenticationException" /> class.
        /// </summary>
        public WebEasAuthenticationException() : this("Authentication - User is not authenticated!")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasAuthenticationException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public WebEasAuthenticationException(string message) : this(message, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasAuthenticationException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public WebEasAuthenticationException(string message, System.Exception innerException) : this(message, null, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasAuthenticationException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="messageUser">The message user.</param>
        public WebEasAuthenticationException(string message, string messageUser) : this(message, messageUser, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasAuthenticationException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="messageUser">The message user.</param>
        /// <param name="innerException">The inner exception.</param>
        public WebEasAuthenticationException(string message, string messageUser, System.Exception innerException) : base(message, messageUser, innerException)
        {
            this.StatusCode = System.Net.HttpStatusCode.Unauthorized;
        }
    }
}