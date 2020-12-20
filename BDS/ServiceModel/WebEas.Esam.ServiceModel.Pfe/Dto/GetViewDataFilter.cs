using System.Runtime.Serialization;
using ServiceStack;

namespace WebEas.Esam.ServiceModel.Pfe.Dto
{
    [Route("/viewdata/{ItemCode}", "POST")]
    [Api("RS 5 - Zobrazenie údajov")]
    
    public class GetViewDataFilter
    {
        [DataMember(Name = "itemcode", IsRequired = true)]
        public string ItemCode { get; set; }

        [DataMember(Name = "filters")]
        public string Filters { get; set; }

        [DataMember(Name = "page")]
        public string Page { get; set; }

        [DataMember(Name = "start")]
        public string Start { get; set; }

        [DataMember(Name = "limit")]
        public string Limit { get; set; }
    }
}