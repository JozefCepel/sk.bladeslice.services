using ServiceStack;
using System.Runtime.Serialization;

namespace WebEas.Esam.ServiceModel.Office.Dto
{
    // Predkontovat
    [Route("/PredkontovatDoklad", "POST")]
    [Api("Doklad")]
    [DataContract]
    public class PredkontovatDokladDto
    {
        [DataMember(IsRequired = true)]
        [NotEmptyOrDefault]
        public long[] D_BiznisEntita_Ids { get; set; }

        [DataMember]
        public bool UctDennik { get; set; }

        [DataMember]
        public bool RzpDennik { get; set; }

        [DataMember]
        public bool VymazatZaznamy { get; set; }
    }
}
