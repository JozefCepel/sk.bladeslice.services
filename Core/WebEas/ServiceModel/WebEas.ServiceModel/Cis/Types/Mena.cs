using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel.Office.Egov.Cis.Types
{
    [Schema("cfe")]
    [Alias("C_Mena")]
    [DataContract]
    [Dial(DialType.Static, DialKindType.ARadio)]
    public class Mena
    {
        [PrimaryKey]
        [DataMember]
        [Alias("ID")]
        public int Id { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        [DataMember]
        [PfeValueColumn]
        public string Kod { get; set; }
    }
}
