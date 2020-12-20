using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack;

namespace WebEas.Exceptions
{
    [DataContract]
    public class WebEasResponseStatus
    {
        /// <summary>
        /// Gets or sets the caption.
        /// </summary>
        /// <value>The caption.</value>
        [DataMember(Order = 1)]
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        [DataMember(Order = 2)]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the detail message.
        /// </summary>
        /// <value>The detail message.</value>
        [DataMember(Order = 3)]
        public string DetailMessage { get; set; }

        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        /// <value>The errors.</value>
        [DataMember(Order = 4)]
        public List<ResponseError> Errors { get; set; }

        /// <summary>
        /// Gets or sets the stack trace.
        /// </summary>
        /// <value>The stack trace.</value>
        [DataMember(Order = 5)]
        public string StackTrace { get; set; }

        /// <summary>
        /// Shoulds the serialize stack trace.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeStackTrace()
        {
            return !string.IsNullOrEmpty(this.StackTrace);
        }

        /// <summary>
        /// Shoulds the serialize errors.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeErrors()
        {
            return this.Errors != null && this.Errors.Count > 0;
        }

        /// <summary>
        /// Shoulds the serialize detail message.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeDetailMessage()
        {
            return !String.IsNullOrEmpty(this.DetailMessage);
        }
    }
}