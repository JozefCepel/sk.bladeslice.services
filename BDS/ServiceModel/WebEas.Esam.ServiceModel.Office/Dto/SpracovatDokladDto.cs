using ServiceStack;
using System.Runtime.Serialization;

namespace WebEas.Esam.ServiceModel.Office.Dto
{
    // Spracovat
    [Route("/SpracovatDoklad", "POST")]
    [Api("Doklad")]
    [DataContract]
    public class SpracovatDokladDto
    {
        [DataMember(IsRequired = true)]
        public long[] Ids { get; set; }
    }
}
