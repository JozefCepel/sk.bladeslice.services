using ServiceStack;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Dto
{
    [DataContract]
    [Route("/list/{KodPolozky}", "GET")]
    [Route("/list/{Code}/{KodPolozky}", "GET")]
    [WebEasRequiredRole(RolesDefinition.Bds.Roles.BdsMember)]
    public class ListDto : BaseListDto
    {
    }
}
