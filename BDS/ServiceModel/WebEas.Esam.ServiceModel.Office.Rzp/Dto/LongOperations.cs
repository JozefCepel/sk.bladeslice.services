using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WebEas.ServiceModel.Dto;

namespace WebEas.Esam.ServiceModel.Office.Bds.Dto
{
    public static class OperationsList
    {
        public const string InternyPoplatok = "InternyPoplatok";
    }

    [WebEasRequiresAnyRole(RolesDefinition.Bds.Roles.BdsMember, RolesDefinition.Bds.Roles.BdsAdmin)]
    [Route("/long/{OperationName}/_start", "POST")]
    [Api("Bds")]
    [DataContract]
    public class LongOperationStartDto : LongOperationStartDtoBase
    {
    }

    [WebEasRequiresAnyRole(RolesDefinition.Bds.Roles.BdsMember, RolesDefinition.Bds.Roles.BdsAdmin)]
    [Route("/long/restart/{ProcessKey}", "POST")]
    [Api("Bds")]
    [DataContract]
    public class LongOperationRestartDto : LongOperationRestartDtoBase
    {
    }

    [WebEasRequiresAnyRole(RolesDefinition.Bds.Roles.BdsMember, RolesDefinition.Bds.Roles.BdsAdmin)]
    [Route("/long/{OperationName}/_progress/{ProcessKey}", "GET")]
    [Api("Bds")]
    [DataContract]
    public class LongOperationProgressDto : LongOperationProgressDtoBase
    {
    }

    [WebEasRequiresAnyRole(RolesDefinition.Bds.Roles.BdsMember, RolesDefinition.Bds.Roles.BdsAdmin)]
    [Route("/long/{OperationName}/_result/{ProcessKey}", "GET")]
    [Api("Bds")]
    [DataContract]
    public class LongOperationResultDto : LongOperationResultDtoBase
    {
    }

    [WebEasRequiresAnyRole(RolesDefinition.Bds.Roles.BdsMember, RolesDefinition.Bds.Roles.BdsAdmin)]
    [Route("/long/{OperationName}/_cancel/{ProcessKey}", "POST")]
    [Api("Bds")]
    [DataContract]
    public class LongOperationCancelDto : LongOperationCancelDtoBase
    {
    }

    [WebEasRequiresAnyRole(RolesDefinition.Bds.Roles.BdsMember, RolesDefinition.Bds.Roles.BdsAdmin)]
    [Route("/long/list", "GET")]
    [Api("Bds")]
    [DataContract]
    public class LongOperationListDto : LongOperationListDtoBase
    {
    }
}
