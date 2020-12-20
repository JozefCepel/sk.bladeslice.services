using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel.Types
{
    [DataContract]
    [Schema("reg")]
    [Alias("V_Log")]
    public class LogView
    {
        [DataMember]
        public long D_Log_Id { get; set; }

        [DataMember]
        public string Application { get; set; }

        [DataMember]
        public string Version { get; set; }

        [DataMember]
        public DateTime Time_Stamp { get; set; }

        [DataMember]
        public string Machine { get; set; }

        [DataMember]
        public string Log_Level { get; set; }

        [DataMember]
        public string Logger { get; set; }

        [DataMember]
        public string Verb { get; set; }

        [DataMember]
        public string ErrorType { get; set; }

        [DataMember]
        public string ErrorIdentifier { get; set; }

        [DataMember]
        public string CorrId { get; set; }

        [DataMember]
        public string RequestUrl { get; set; }

        [DataMember]
        public string JsonSession { get; set; }

        [DataMember]
        public string JsonRequest { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public string LastSoapRequestMessage { get; set; }

        [DataMember]
        public string LastSoapResponseMessage { get; set; }

        [DataMember]
        public string LastSql { get; set; }
    }
}
