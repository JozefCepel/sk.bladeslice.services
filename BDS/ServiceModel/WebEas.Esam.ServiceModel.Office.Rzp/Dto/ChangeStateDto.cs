using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WebEas.ServiceModel.Dto;

namespace WebEas.Esam.ServiceModel.Office.Rzp.Dto
{
    [DataContract]
    [Route("/changestate", "POST")]
    [Api("Zmena stavu")]
    [WebEasRequiresAnyRole(RolesDefinition.Rzp.Roles.RzpWriter)]
    public class ChangeStateDto : BaseChangeStateDto
    {
    }
}
