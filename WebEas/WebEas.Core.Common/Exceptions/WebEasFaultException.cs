using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.Exceptions
{
    [DataContract]
    public class WebEasFaultException
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        /// <value>The error code.</value>
        [DataMember]
        public FaultCodeType ErrorCode { get; set; }
    }
}