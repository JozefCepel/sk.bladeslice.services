using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack;

namespace WebEas.ServiceModel.Dto
{
    //[Route("/sadzbyintpoplatku", "GET")]
    [DataContract]
    public abstract class SadzbyRequestBase
    {
        [DataMember]
        public long EntityId { get; set; }

        [DataMember]
        public short Rok { get; set; }
    }

    //[Route("/internypoplatok", "GET")]
    [DataContract]
    public abstract class InternyPoplatokRequestBase
    {
        [DataMember]
        public long EntityId { get; set; }
    }
}
