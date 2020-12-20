using ServiceStack;
using System.Runtime.Serialization;
using WebEas.ServiceModel.Dto;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Dto
{
    public static class OperationsList
    {
        public const string InternyPoplatok = "InternyPoplatok";
    }

    [Route("/long/{OperationName}/_start", "POST")]
    [Api("Cfe")]
    [DataContract]
    public class LongOperationStartDto : LongOperationStartDtoBase
    {
    }

    [Route("/long/restart/{ProcessKey}", "POST")]
    [Api("Cfe")]
    [DataContract]
    public class LongOperationRestartDto : LongOperationRestartDtoBase
    {
    }

    [Route("/long/{OperationName}/_progress/{ProcessKey}", "GET")]
    [Api("Cfe")]
    [DataContract]
    public class LongOperationProgressDto : LongOperationProgressDtoBase
    {
    }

    [Route("/long/{OperationName}/_result/{ProcessKey}", "GET")]
    [Api("Cfe")]
    [DataContract]
    public class LongOperationResultDto : LongOperationResultDtoBase
    {
    }

    [Route("/long/{OperationName}/_cancel/{ProcessKey}", "POST")]
    [Api("Cfe")]
    [DataContract]
    public class LongOperationCancelDto : LongOperationCancelDtoBase
    {
    }

    [Route("/long/list", "GET")]
    [Api("Cfe")]
    [DataContract]
    public class LongOperationListDto : LongOperationListDtoBase
    {
    }
}
