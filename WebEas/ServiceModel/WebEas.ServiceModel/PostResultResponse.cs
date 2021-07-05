using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [DataContract]
    public class PostResultResponse<T>
    {
        [DataMember]
        public string Success { get; set; }

        [DataMember]
        public string Warning { get; set; }

        [DataMember]
        public bool Refresh { get; set; }

        [DataMember]
        public string UrlLink { get; set; }

        [DataMember]
        public string ReportId { get; set; }

        [DataMember]
        public T[] Records { get; set; }

        public bool ShouldSerializeSuccess()
        {
            return !string.IsNullOrEmpty(Success);
        }

        public bool ShouldSerializeWarning()
        {
            return !string.IsNullOrEmpty(Warning);
        }

        public bool ShouldSerializeRefresh()
        {
            return Refresh;
        }

        public bool ShouldSerializeUrlLink()
        {
            return !string.IsNullOrEmpty(UrlLink);
        }

        public bool ShouldSerializeRecords()
        {
            return Records != null && Records.Length > 0;
        }

        public bool ShouldSerializeReportId()
        {
            return !string.IsNullOrEmpty(ReportId);
        }

    }
}
