using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel.Office.Egov.Cis.Types
{
    [Schema("cfe")]
    [Alias("C_Jazyk")]
    [DataContract]
    [Dial(DialType.Static, DialKindType.BackOffice)]
    public class Jazyk
    {
        [PrimaryKey]
        [DataMember]
        [Alias("ID")]
        public int Id { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        [DataMember]
        public string Kod { get; set; }
    }
}
