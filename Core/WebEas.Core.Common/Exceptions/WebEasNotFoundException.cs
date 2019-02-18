using System;
using System.Linq;

namespace WebEas
{
    /// <summary>
    /// 
    /// </summary>
    public class WebEasNotFoundException : WebEasException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasNotFoundException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public WebEasNotFoundException(string message) : this(message, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasNotFoundException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="faultCode">The fault code.</param>
        public WebEasNotFoundException(string message, FaultCodeType faultCode) : this(message)
        {
            this.FaultCode = faultCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasNotFoundException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public WebEasNotFoundException(string message, System.Exception innerException) : this(message, null, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasNotFoundException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="messageUser">The message user.</param>
        public WebEasNotFoundException(string message, string messageUser) : this(message, messageUser, null)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasNotFoundException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="messageUser">The message user.</param>
        /// <param name="innerException">The inner exception.</param>
        public WebEasNotFoundException(string message, string messageUser, System.Exception innerException) : base(message, messageUser, innerException)
        {
            this.StatusCode = System.Net.HttpStatusCode.NotFound;            
        }
    }
}