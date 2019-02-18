using ServiceStack;
using System.Runtime.Serialization;
using WebEas.ServiceModel.Dto;

namespace WebEas.Esam.ServiceModel.Office.Reg.Dto
{
    [DataContract]
    [Route("/changestate", "POST")]
    [Api("Zmena stavu")]
    [WebEasRequiresAnyRole(new string[] { RolesDefinition.Rzp.Roles.RzpWriter })]
    public class ChangeStateDto : BaseChangeStateDto
    {
    }
}