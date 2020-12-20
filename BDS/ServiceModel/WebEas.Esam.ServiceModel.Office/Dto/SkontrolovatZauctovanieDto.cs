using ServiceStack;
using System.Runtime.Serialization;

namespace WebEas.Esam.ServiceModel.Office.Dto
{
    // Predkontovat
    [Route("/SkontrolovatZauctovanie", "POST")]
    [Api("Doklad")]
    [DataContract]
    public class SkontrolovatZauctovanieDto
    {
        [DataMember(IsRequired = true)]
        [NotEmptyOrDefault]
        public long[] D_BiznisEntita_Ids { get; set; }

        [DataMember]
        public bool UctDennik { get; set; }

        [DataMember]
        public bool RzpDennik { get; set; }
    }
}
