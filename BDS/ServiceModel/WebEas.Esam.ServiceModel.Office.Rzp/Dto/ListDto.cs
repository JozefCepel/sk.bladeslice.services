using ServiceStack;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Rzp.Dto
{
    [DataContract]
    [Route("/list/{KodPolozky}", "GET")]
    [Route("/list/{Code}/{KodPolozky}", "GET")]
    [WebEasRequiredRole(RolesDefinition.Rzp.Roles.RzpMember)]
    public class ListDto : BaseListDto
    {
    }
}
