using ServiceStack;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Dto
{
    [DataContract]
    [Route("/treecounts", "POST")]
    public class GetTreeCountsDto : BaseGetTreeCounts
    {
    }
}
