using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Dto
{
    [DataContract]
    public class ReportKnihaDto
    {
        [DataMember]
        public long[] Ids { get; set; }

        [DataMember]
        public PfeFilterUrl[] Filters { get; set; }

        [DataMember]
        public string FilterText { get; set; }
    }
}
