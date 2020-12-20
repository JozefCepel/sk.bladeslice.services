using System;
using System.Linq;
using System.Net;

namespace WebEas
{
    /// <summary>
    /// Web Eas Exception
    /// </summary>
    public class WebEasException : Exception
    {
        private string messageUser;
        private HttpStatusCode statusCode = HttpStatusCode.Conflict;
        private FaultCodeType faultCode = FaultCodeType.InternalError;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public WebEasException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public WebEasException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public WebEasException(string message, FaultCodeType faultCode) : base(message)
        {
            this.FaultCode = faultCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public WebEasException(string message, System.Exception innerException, FaultCodeType faultCode) : base(message, innerException)
        {
            this.FaultCode = faultCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="messageUser">The message user.</param>
        public WebEasException(string message, string messageUser) : base(string.IsNullOrEmpty(message) ? messageUser : message)
        {
            this.MessageUser = messageUser;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="messageUser">The message user.</param>
        /// <param name="innerException">The inner exception.</param>
        public WebEasException(string message, string messageUser, System.Exception innerException) : base(string.IsNullOrEmpty(message) ? messageUser : message, innerException)
        {
            this.MessageUser = messageUser;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="messageUser">The message user.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="parameters">The parameters.</param>
        public WebEasException(string message, string messageUser, System.Exception innerException, params object[] parameters) : base(string.IsNullOrEmpty(message) ? messageUser : message, innerException)
        {
            this.MessageUser = messageUser;
            this.Parameters = parameters;
        }

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        public object[] Parameters { get; protected set; }

        /// <summary>
        /// Gets or sets the caption.
        /// </summary>
        /// <value>The caption.</value>
        public string Caption { get; set; }

        /// <summary>
        /// Gets the has caption.
        /// </summary>
        /// <value>The has caption.</value>
        public bool HasCaption
        {
            get
            {
                return !string.IsNullOrEmpty(this.Caption);
            }
        }

        /// <summary>
        /// Gets or sets the message user.
        /// </summary>
        /// <value>The message user.</value>
        public string MessageUser
        {
            get
            {
                if (this.HasInnerMessageUser)
                {
                    if (string.IsNullOrEmpty(this.messageUser))
                    {
                        return this.InnerMessageUser;
                    }

                    string delimiter = string.Empty;

                    this.messageUser = this.messageUser.TrimEnd(' '); 
                    string last = this.messageUser.Substring(this.messageUser.Length - 1);
                    switch(last)
                    {
                        case ".":
                            delimiter = " ";
                            break;
                        case "!":
                            delimiter = " ";
                            break;
                        default:
                            delimiter = ". ";
                            break;                        
                    }

                    return string.Format("{0}{1}{2}", this.messageUser, delimiter, this.InnerMessageUser);
                }
                else
                {
                    return this.messageUser;
                }
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.messageUser = value;
                }
                else
                {
                    string delimiter = string.Empty;
                    this.messageUser = value.TrimEnd(' ');
                    string last = this.messageUser.Substring(this.messageUser.Length - 1);
                    switch (last)
                    {
                        case ".":
                            delimiter = " ";
                            break;
                        case "!":
                            delimiter = " ";
                            break;
                        default:
                            delimiter = ". ";
                            break;
                    }
                    this.messageUser = string.Format("{0}{1}", this.messageUser, delimiter);
                }
            }
        }

        /// <summary>
        /// Gets the has message user.
        /// </summary>
        /// <value>The has message user.</value>
        public bool HasMessageUser
        {
            get
            {
                return !string.IsNullOrEmpty(this.messageUser) || this.HasInnerMessageUser;
            }
        }

        /// <summary>
        /// Gets or sets the detail message.
        /// </summary>
        /// <value>The detail message.</value>
        public string DetailMessage { get; set; }

        /// <summary>
        /// Gets the has detail message.
        /// </summary>
        /// <value>The has detail message.</value>
        public bool HasDetailMessage
        {
            get
            {
                return !string.IsNullOrEmpty(this.DetailMessage);
            }
        }

        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        /// <value>The status code.</value>
        public HttpStatusCode StatusCode
        {
            get
            {
                return this.statusCode;
            }
            set
            {
                this.statusCode = value;
            }
        }

        /// <summary>
        /// Gets or sets the fault code.
        /// </summary>
        /// <value>The fault code.</value>
        public FaultCodeType FaultCode
        {
            get
            {
                return this.faultCode;
            }
            protected set
            {
                this.faultCode = value;
            }
        }

        /// <summary>
        /// Gets the inner message user.
        /// </summary>
        /// <value>The inner message user.</value>
        private string InnerMessageUser
        {
            get
            {
                return GetInnerMessageUser(this);
            }
        }

        /// <summary>
        /// Gets the has inner message user.
        /// </summary>
        /// <value>The has inner message user.</value>
        private bool HasInnerMessageUser
        {
            get
            {
                return CheckInnerMessageUser(this);
            }
        }

        /// <summary>
        /// Gets the inner message user.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        private static string GetInnerMessageUser(Exception ex)
        {
            if (CheckInnerMessageUser(ex))
            {
                if (ex.InnerException is WebEasException)
                {
                    return ((WebEasException)ex.InnerException).MessageUser;
                }
                else
                {
                    return GetInnerMessageUser(ex.InnerException);
                }
            }
            return null;
        }

        /// <summary>
        /// Checks the inner message user.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        private static bool CheckInnerMessageUser(Exception ex)
        {
            if (ex.InnerException != null)
            {
                if (ex.InnerException is WebEasException && ((WebEasException)ex.InnerException).HasMessageUser)
                {
                    return true;
                }
                else
                {
                    return CheckInnerMessageUser(ex.InnerException);
                }
            }
            return false;
        }
    }
}