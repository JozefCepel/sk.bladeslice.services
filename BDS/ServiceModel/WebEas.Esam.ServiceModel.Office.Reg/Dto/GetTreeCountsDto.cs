using ServiceStack;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Reg.Dto
{
    [DataContract]
    [Route("/treecounts", "POST")]
    public class GetTreeCountsDto : BaseGetTreeCounts
    {
    }
}
