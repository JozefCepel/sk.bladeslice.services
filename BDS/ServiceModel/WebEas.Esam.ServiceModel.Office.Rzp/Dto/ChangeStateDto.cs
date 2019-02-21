using ServiceStack;
using System.Runtime.Serialization;
using WebEas.ServiceModel.Dto;

namespace WebEas.Esam.ServiceModel.Office.Rzp.Dto
{
    [DataContract]
    [Route("/changestate", "POST")]
    [Api("Zmena stavu")]
    [WebEasRequiresAnyRole(RolesDefinition.Rzp.Roles.RzpWriter)]
    public class ChangeStateDto : BaseChangeStateDto
    {
        [DataMember]
        public string VyjadrenieSpracovatela { get; set; }
    }
}
