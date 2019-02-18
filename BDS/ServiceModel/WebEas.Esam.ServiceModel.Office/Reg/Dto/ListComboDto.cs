using ServiceStack;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Reg.Dto
{
    [DataContract]
    [WebEasRequiredRole(WebEas.ServiceModel.Office.Egov.Reg.Roles.RegMember)]
    [Api("Hodnoty číselnika")]
    [Route("/combo/{KodPolozky}/{Column}", "GET")]
    [Route("/combo/{KodPolozky}/{Column}/{RequiredField*}", "GET")]
    public class ListComboDto : BaseListComboDto
    {
    }
}