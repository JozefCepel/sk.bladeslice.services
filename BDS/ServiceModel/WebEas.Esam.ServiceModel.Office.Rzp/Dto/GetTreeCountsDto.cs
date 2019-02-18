using ServiceStack;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Rzp.Dto
{
    [DataContract]
    [Route("/treecounts", "POST")]
    [WebEasRequiredRole(RolesDefinition.Rzp.Roles.RzpMember)]
    public class GetTreeCountsDto : BaseGetTreeCounts
    {
    }
}
