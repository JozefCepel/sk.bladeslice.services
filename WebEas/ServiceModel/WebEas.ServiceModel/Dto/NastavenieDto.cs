using System;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel.Dto
{
    // Update
    [DataContract]
    public class UpdateNastavenieBase
    {
        [DataMember(IsRequired = true)]
        public string Nazov { get; set; }

        [DataMember]
        public string sHodn { get; set; }

        [DataMember]
        [NotEmptyOrDefault]
        public long iHodn { get; set; }

        [DataMember]
        public bool? bHodn { get; set; }

        [DataMember]
        public DateTime? dHodn { get; set; }

        [DataMember]
        public DateTime? tHodn { get; set; }

        [DataMember]
        [NotEmptyOrDefault]
        public decimal nHodn { get; set; }
    }
}
