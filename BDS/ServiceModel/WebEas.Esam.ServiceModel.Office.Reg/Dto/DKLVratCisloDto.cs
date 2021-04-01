using ServiceStack;
using System.Collections.Generic;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Types.Reg;

namespace WebEas.Esam.ServiceModel.Office.Reg.Dto
{
    [Route("/DKLVratCislo", "POST")]
    [Api("DKLVratCislo")]
    [DataContract]
    public class DKLVratCisloDto : IReturn<List<DKLVratCisloResponseDto>>
    {
        [DataMember(IsRequired = true)]
        public BiznisEntita Be { get; set; }

        [DataMember(IsRequired = true)]
        public string NumberChar { get; set; }
    }

    [DataContract]
    public class DKLVratCisloResponseDto
    {
        [DataMember]
        public int? Cislo { get; set; }

        [DataMember]
        public string VS { get; set; }

        [DataMember]
        public string CisloDokladu { get; set; }
    }
}
