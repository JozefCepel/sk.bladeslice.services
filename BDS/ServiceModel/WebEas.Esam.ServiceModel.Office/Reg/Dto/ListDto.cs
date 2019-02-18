using ServiceStack;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Reg.Dto
{
    [DataContract]
    [Route("/list/{KodPolozky}", "GET")]
    [Route("/list/{Code}/{KodPolozky}", "GET")]
    [WebEasRequiredRole(WebEas.ServiceModel.Office.Egov.Reg.Roles.RegMember)]
    public class ListDto : BaseListDto
    {
    }
}
