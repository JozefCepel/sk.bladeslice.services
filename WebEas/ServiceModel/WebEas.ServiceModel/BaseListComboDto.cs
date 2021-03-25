using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [DataContract]
    public abstract class BaseListComboDto : IListComboDto
    {
        [DataMember]
        public string KodPolozky { get; set; }

        [DataMember]
        public string Column { get; set; }

        [DataMember]
        public string RequiredField { get; set; }
        
        [DataMember]
        public string Value { get; set; }

        [DataMember(Name = "af")]
        public string AdditionalFields { get; set; }
    }
}