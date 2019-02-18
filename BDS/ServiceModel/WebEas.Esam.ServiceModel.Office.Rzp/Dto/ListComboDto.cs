using ServiceStack;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Rzp.Dto
{
    [DataContract]
    [WebEasRequiredRole(RolesDefinition.Rzp.Roles.RzpMember)]
    [Api("Hodnoty číselnika")]
    [Route("/combo/{KodPolozky}/{Column}", "GET")]
    [Route("/combo/{KodPolozky}/{Column}/{RequiredField*}", "GET")]
    public class ListComboDto : BaseListComboDto
    {
    }
}
