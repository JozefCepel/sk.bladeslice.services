using System;
using System.Linq;

namespace WebEas
{
    /// <summary>
    /// 
    /// </summary>
    public class WebEasSoapException : WebEasException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasSoapException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public WebEasSoapException(string message) : this(message, null, null,null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasSoapException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="parameters">The parameters.</param>
        public WebEasSoapException(string message, System.Exception innerException, params object[] parameters) : this(message, null, innerException, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasSoapException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="messageUser">The message user.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="parameters">The parameters.</param>
        public WebEasSoapException(string message, string messageUser, System.Exception innerException, params object[] parameters) : base(message, messageUser, innerException)
        {
            this.StatusCode = System.Net.HttpStatusCode.Conflict;
            this.Parameters = parameters;
        }
    }
}