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
    public static class OperationsList
    {
        public const string InternyPoplatok = "InternyPoplatok";
    }

    [WebEasRequiresAnyRole(RolesDefinition.Rzp.Roles.RzpMember, RolesDefinition.Rzp.Roles.RzpAdmin)]
    [Route("/long/{OperationName}/_start", "POST")]
    [Api("Rzp")]
    [DataContract]
    public class LongOperationStartDto : LongOperationStartDtoBase
    {
    }

    [WebEasRequiresAnyRole(RolesDefinition.Rzp.Roles.RzpMember, RolesDefinition.Rzp.Roles.RzpAdmin)]
    [Route("/long/restart/{ProcessKey}", "POST")]
    [Api("Rzp")]
    [DataContract]
    public class LongOperationRestartDto : LongOperationRestartDtoBase
    {
    }

    [WebEasRequiresAnyRole(RolesDefinition.Rzp.Roles.RzpMember, RolesDefinition.Rzp.Roles.RzpAdmin)]
    [Route("/long/{OperationName}/_progress/{ProcessKey}", "GET")]
    [Api("Rzp")]
    [DataContract]
    public class LongOperationProgressDto : LongOperationProgressDtoBase
    {
    }

    [WebEasRequiresAnyRole(RolesDefinition.Rzp.Roles.RzpMember, RolesDefinition.Rzp.Roles.RzpAdmin)]
    [Route("/long/{OperationName}/_result/{ProcessKey}", "GET")]
    [Api("Rzp")]
    [DataContract]
    public class LongOperationResultDto : LongOperationResultDtoBase
    {
    }

    [WebEasRequiresAnyRole(RolesDefinition.Rzp.Roles.RzpMember, RolesDefinition.Rzp.Roles.RzpAdmin)]
    [Route("/long/{OperationName}/_cancel/{ProcessKey}", "POST")]
    [Api("Rzp")]
    [DataContract]
    public class LongOperationCancelDto : LongOperationCancelDtoBase
    {
    }

    [WebEasRequiresAnyRole(RolesDefinition.Rzp.Roles.RzpMember, RolesDefinition.Rzp.Roles.RzpAdmin)]
    [Route("/long/list", "GET")]
    [Api("Rzp")]
    [DataContract]
    public class LongOperationListDto : LongOperationListDtoBase
    {
    }
}
