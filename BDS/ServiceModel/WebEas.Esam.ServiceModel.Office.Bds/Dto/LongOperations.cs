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

    [Route("/long/{OperationName}/_start", "POST")]
    [Api("Bds")]
    [DataContract]
    public class BdsLongOperationStartDto : LongOperationStartDtoBase
    {
    }

    [Route("/long/restart/{ProcessKey}", "POST")]
    [Api("Bds")]
    [DataContract]
    public class LongOperationRestartDto : LongOperationRestartDtoBase
    {
    }

    [Route("/long/{OperationName}/_progress/{ProcessKey}", "GET")]
    [Api("Bds")]
    [DataContract]
    public class LongOperationProgressDto : LongOperationProgressDtoBase
    {
    }

    [Route("/long/{OperationName}/_result/{ProcessKey}", "GET")]
    [Api("Bds")]
    [DataContract]
    public class LongOperationResultDto : LongOperationResultDtoBase
    {
    }

    [Route("/long/{OperationName}/_cancel/{ProcessKey}", "POST")]
    [Api("Bds")]
    [DataContract]
    public class LongOperationCancelDto : LongOperationCancelDtoBase
    {
    }

    [Route("/long/list", "GET")]
    [Api("Bds")]
    [DataContract]
    public class LongOperationListDto : LongOperationListDtoBase
    {
    }
}
