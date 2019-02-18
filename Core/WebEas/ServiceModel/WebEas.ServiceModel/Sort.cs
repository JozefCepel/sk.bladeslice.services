using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [DataContract]
    public class Sort
    {
        [DataMember(Name = "f")]
        public string Field { get; set; }

        [DataMember(Name = "s")]
        public string SortDirection { get; set; }
    }
}