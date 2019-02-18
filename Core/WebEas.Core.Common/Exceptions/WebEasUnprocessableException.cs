using System;
using System.Collections.Generic;
using System.Linq;

namespace WebEas
{
    public class WebEasUnprocessableException : WebEasValidationException
    {
        public WebEasUnprocessableException(string message) : this(message, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasValidationException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="faultCode">The fault code.</param>
        public WebEasUnprocessableException(string message, FaultCodeType faultCode) : this(message)
        {
            this.FaultCode = faultCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasValidationException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public WebEasUnprocessableException(string message, System.Exception innerException) : this(message, null, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasValidationException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="messageUser">The message user.</param>
        public WebEasUnprocessableException(string message, string messageUser) : this(message, messageUser, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasValidationException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="messageUser">The message user.</param>
        /// <param name="innerException">The inner exception.</param>
        public WebEasUnprocessableException(string message, string messageUser, System.Exception innerException) : base(message, messageUser, innerException)
        {
            this.StatusCode = (System.Net.HttpStatusCode)422;
            this.Errors = new List<string>();
        }
    }
}