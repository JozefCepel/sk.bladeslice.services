using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack;

namespace WebEas.ServiceModel
{
    public interface IGetDataChanges
    {
        long RowId { get; set; }
        string Code { get; set; }
    }
}

namespace WebEas.ServiceModel.Dto
{
    //[Route("/datachanges/{Code}/{RowId}", "GET")]
    [DataContract]
    public abstract class DataChangesRequestBase : IGetDataChanges
    {
        [DataMember]
        public long RowId { get; set; }

        [DataMember]
        public string Code { get; set; }
    }
}
