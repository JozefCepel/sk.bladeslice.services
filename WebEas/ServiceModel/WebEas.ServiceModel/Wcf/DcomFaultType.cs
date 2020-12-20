using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace WebEas.ServiceModel
{
    [DataContract(Namespace = "http://schemas.dcom.sk/fault/1.0", Name = "dcomFault")]
    public class DcomFaultType
    {
        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        /// <value>The error code.</value>
        [DataMember(Name = "faultCode")]
        public int ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        [DataMember(Name = "faultMessage")]
        public string Message { get; set; }
     
        /// <summary>
        /// Gets or sets the cause.
        /// </summary>
        /// <value>The cause.</value>
        [DataMember(Name = "faultCause")]
        public string Cause { get; set; }

        /// <summary>
        /// Gets or sets the invalid parameters.
        /// </summary>
        /// <value>The invalid parameters.</value>
        [DataMember(Name = "invalidParameter")]
        public List<InvalidParameterType> InvalidParameters { get; set; }

        /// <summary>
        /// Throws the fault exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="errorCodeRate">The error code rate.</param>
        /// <returns></returns>
        public static FaultException<DcomFaultType> GetFaultException(Exception ex, int errorCodeRate = 26000)
        {
            if (ex is WebEasException)
            {
                int code = errorCodeRate;
                var exx = (WebEasException)ex;
                switch (exx.FaultCode)
                {
                    case FaultCodeType.NotFound:
                        code += 1;
                        break;
                    case FaultCodeType.NotExists:
                        code += 2;
                        break;
                    case FaultCodeType.AlreadyExists:
                        code += 3;
                        break;
                    case FaultCodeType.Duplicate:
                        code += 4;
                        break;
                    default:
                        code += 9;
                        break; 
                }
                return new FaultException<WebEas.ServiceModel.DcomFaultType>(new ServiceModel.DcomFaultType { Message = exx.MessageUser ?? exx.Message, ErrorCode = code }, exx.MessageUser ?? exx.Message);
            }
            else
            {
                int code = errorCodeRate + 9;
                return new FaultException<WebEas.ServiceModel.DcomFaultType>(new ServiceModel.DcomFaultType { Message = ex.Message, ErrorCode = code }, "Nastala interná chyba");
            }
        }
    }

    [DataContract(Namespace = "http://schemas.dcom.sk/fault/1.0", Name = "InvalidParameterType")]
    public class InvalidParameterType
    {
        [DataMember(Name = "parameterName")]
        public string ParameterName { get; set; }

        [DataMember(Name = "errorMessage")]
        public string ErrorMessage { get; set; }
    }
}