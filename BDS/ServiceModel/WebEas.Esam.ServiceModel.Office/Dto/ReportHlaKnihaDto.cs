using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Dto
{
    [DataContract]
    public class ReportHlaKnihaDto
    {
        [DataMember]
        public Guid[] Ids { get; set; }

        [DataMember]
        public PfeFilterUrl[] Filters { get; set; }

        [DataMember]
        public string FilterText { get; set; }
    }
}
