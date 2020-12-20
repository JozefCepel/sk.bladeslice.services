using ServiceStack;
using System.Runtime.Serialization;

namespace WebEas.Esam.ServiceModel.Office.Dto
{
    // Spracovat
    [Route("/ZauctovatDoklad", "POST")]
    [Api("Doklad")]
    [DataContract]
    public class ZauctovatDokladDto
    {
        [DataMember(IsRequired = true)]
        public long[] Ids { get; set; }

        [DataMember(IsRequired = true)]
        public int IdNewState { get; set; }
    }
}
