using System;
using System.Collections.Generic;
using System.Linq;

namespace WebEas
{
    /// <summary>
    /// Web Eas Validation Exception
    /// </summary>
    public class WebEasValidationException : WebEasException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasValidationException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public WebEasValidationException(string message)
            : this(message, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasValidationException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="faultCode">The fault code.</param>
        public WebEasValidationException(string message, FaultCodeType faultCode)
            : this(message)
        {
            FaultCode = faultCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasValidationException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public WebEasValidationException(string message, System.Exception innerException)
            : this(message, null, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasValidationException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="messageUser">The message user.</param>
        public WebEasValidationException(string message, string messageUser)
            : this(message, messageUser, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasValidationException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="messageUser">The message user.</param>
        /// <param name="innerException">The inner exception.</param>
        public WebEasValidationException(string message, string messageUser, System.Exception innerException)
            : this(message, messageUser, innerException, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasValidationException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="messageUser">The message user.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="parameters">The parameters.</param>
        public WebEasValidationException(string message, string messageUser, System.Exception innerException, params object[] parameters)
            : base(message, messageUser, innerException, parameters)
        {
            this.StatusCode = System.Net.HttpStatusCode.BadRequest;
            this.Errors = new List<string>();
        }

        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        /// <value>The errors.</value>
        public List<string> Errors { get; set; }

        /// <summary>
        /// Creates validation exception 'Entity not found by name'
        /// </summary>
        /// <param name="entityName">User friendly name of the not found entity.</param>
        /// <param name="value">Value of name search criteria</param>
        /// <value>The errors.</value>
        public static WebEasValidationException NotFoundByName(string entityName, string value)
        {
            return new WebEasValidationException(null, string.Format("Údaj '{0}' s hodnotou '{1}' nebol nájdený", entityName, value));
        }
    }
}