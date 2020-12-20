using System.Runtime.Serialization;
using ServiceStack;

namespace WebEas.Esam.ServiceModel.Pfe.Dto
{

    /// <summary>
    /// Generate merge script for all global views
    /// </summary>
    [Route("/generatescriptglobalviews", "GET")]
    [Route("/generatescriptglobalviews/{FW}", "GET")]
    [Api("Prenos globálnych pohľadov")]
    
    [DataContract]
    public class MergeScriptGlobalViews
    {
        [ApiMember(Name = "FW", Description = "ak ide iba o framworkové pohľady, tak TRUE", DataType = "bool", IsRequired = false)]
        [DataMember]
        public bool FW { get; set; }
    }

    [Route("/CachedValue")]
    
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