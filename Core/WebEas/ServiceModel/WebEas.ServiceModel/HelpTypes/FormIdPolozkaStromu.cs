using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel.HelpTypes
{
    [DataContract]
    public class FormIdPolozkaStromu
    {
        [DataMember]
        public string FormId { get; set; }

        [DataMember]
        public string PolozkaStromu { get; set; }
    }
}