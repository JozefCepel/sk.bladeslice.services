using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel.Office.Egov.Reg.Types
{
    [Schema("reg")]
    [Alias("V_EpodFormTextation")]
    [DataContract]
    public class EpodFormTextationView
    {
        [DataMember]
        public long textationId { get; set; }

        [DataMember]
        public string url { get; set; }

        [DataMember]
        public string nazov { get; set; }
    }
}