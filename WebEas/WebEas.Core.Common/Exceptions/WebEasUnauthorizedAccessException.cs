using System;
using System.Linq;

namespace WebEas
{
    /// <summary>
    /// Web Eas Unathorized Access Exception
    /// </summary>
    public class WebEasUnauthorizedAccessException : WebEasException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasUnauthorizedAccessException" /> class.
        /// </summary>
        public WebEasUnauthorizedAccessException() : this("Forbidden - User is not allowed to call this operation!")
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasUnauthorizedAccessException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public WebEasUnauthorizedAccessException(string message) : this(message, null, null)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasUnauthorizedAccessException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public WebEasUnauthorizedAccessException(string message, System.Exception innerException) : this(message, null, innerException)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasUnauthorizedAccessException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="messageUser">The message user.</param>
        public WebEasUnauthorizedAccessException(string message, string messageUser) : this(message, messageUser, null)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasUnauthorizedAccessException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="messageUser">The message user.</param>
        /// <param name="innerException">The inner exception.</param>
        public WebEasUnauthorizedAccessException(string message, string messageUser, System.Exception innerException) : base(message, messageUser, innerException)
        {
            this.StatusCode = System.Net.HttpStatusCode.Forbidden;
        }
    }
}