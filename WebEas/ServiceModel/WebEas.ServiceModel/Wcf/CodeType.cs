using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract(Namespace = "http://schemas.dcom.sk/private/egov/office/1.0")]
    public enum CodeType : int
    {
        [EnumMember]
        NotFound = 1,
        [EnumMember]
        NotExists = 2,
        [EnumMember]
        AlreadyExists = 3,
        [EnumMember]
        Duplicate = 4,
        [EnumMember]        
        InternalError = 99,        
    }
}