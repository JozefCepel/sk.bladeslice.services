using System.Runtime.Serialization;
using ServiceStack;

namespace WebEas.Esam.ServiceModel.Pfe.Dto
{

    /// <summary>
    /// Generate merge script for all global views
    /// </summary>
    [Route("/generatescriptglobalviews", "GET")]
    [Api("Prenos globálnych pohľadov")]
    [WebEasAuthenticate]
    [DataContract]
    public class MergeScriptGlobalViews { }

    [Route("/CachedValue")]
    [WebEasAuthenticate]
    public class CachedValueReq : IReturn<string>
    {
        [ApiMember(Name = "Key", DataType = "string")]
        [DataMember]
        public string Key { get; set; }

        [ApiMember(Name = "Value", DataType = "string")]
        [DataMember]
        public string Value { get; set; }
    }

    [Route("/LogRequestDuration", "POST")]
    [Api("Log Request Duration from FE")]
    [WebEasAuthenticate]
    public class LogRequestDurationReq
    {
        [DataMember]
        [ApiMember(Name = "ElapsedMilliseconds", DataType = "long", IsRequired = true)]
        public long ElapsedMilliseconds { get; set; }

        [DataMember]
        [ApiMember(Name = "ServiceUrl", DataType = "string", IsRequired = true)]
        public string ServiceUrl { get; set; }

        [DataMember]
        [ApiMember(Name = "Operation", DataType = "string", IsRequired = true)]
        public string Operation { get; set; }
    }
}