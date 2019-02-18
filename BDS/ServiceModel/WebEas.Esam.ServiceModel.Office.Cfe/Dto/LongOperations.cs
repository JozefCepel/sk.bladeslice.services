using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WebEas.ServiceModel.Dto;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Dto
{
    public static class OperationsList
    {
        public const string InternyPoplatok = "InternyPoplatok";
    }

    [WebEasRequiresAnyRole(RolesDefinition.Cfe.Roles.CfeMember, RolesDefinition.Cfe.Roles.CfeAdmin)]
    [Route("/long/{OperationName}/_start", "POST")]
    [Api("Cfe")]
    [DataContract]
    public class LongOperationStartDto : LongOperationStartDtoBase
    {
    }

    [WebEasRequiresAnyRole(RolesDefinition.Cfe.Roles.CfeMember, RolesDefinition.Cfe.Roles.CfeAdmin)]
    [Route("/long/restart/{ProcessKey}", "POST")]
    [Api("Cfe")]
    [DataContract]
    public class LongOperationRestartDto : LongOperationRestartDtoBase
    {
    }

    [WebEasRequiresAnyRole(RolesDefinition.Cfe.Roles.CfeMember, RolesDefinition.Cfe.Roles.CfeAdmin)]
    [Route("/long/{OperationName}/_progress/{ProcessKey}", "GET")]
    [Api("Cfe")]
    [DataContract]
    public class LongOperationProgressDto : LongOperationProgressDtoBase
    {
    }

    [WebEasRequiresAnyRole(RolesDefinition.Cfe.Roles.CfeMember, RolesDefinition.Cfe.Roles.CfeAdmin)]
    [Route("/long/{OperationName}/_result/{ProcessKey}", "GET")]
    [Api("Cfe")]
    [DataContract]
    public class LongOperationResultDto : LongOperationResultDtoBase
    {
    }

    [WebEasRequiresAnyRole(RolesDefinition.Cfe.Roles.CfeMember, RolesDefinition.Cfe.Roles.CfeAdmin)]
    [Route("/long/{OperationName}/_cancel/{ProcessKey}", "POST")]
    [Api("Cfe")]
    [DataContract]
    public class LongOperationCancelDto : LongOperationCancelDtoBase
    {
    }

    [WebEasRequiresAnyRole(RolesDefinition.Cfe.Roles.CfeMember, RolesDefinition.Cfe.Roles.CfeAdmin)]
    [Route("/long/list", "GET")]
    [Api("Cfe")]
    [DataContract]
    public class LongOperationListDto : LongOperationListDtoBase
    {
    }
}
