using ServiceStack;
using System.Runtime.Serialization;
using WebEas.ServiceModel.Dto;

namespace WebEas.Esam.ServiceModel.Office.Reg.Dto
{
    public enum OperationsList
    {
        //Operacie
        SpracovatDoklad,
        DoposlanieUhradDoDcomu,
        PredkontovatDoklad,
        SkontrolovatZauctovanie,
        ZauctovatDoklad,
        MigraciaPociatocnehoStavu

        //Zostavy
    }

    [Route("/long/{OperationName}/_start", "POST")]
    [Api("Reg")]
    [DataContract]
    public class RegLongOperationStartDto : LongOperationStartDtoBase
    {
    }

    [Route("/long/restart/{ProcessKey}", "POST")]
    [Api("Reg")]
    [DataContract]
    public class LongOperationRestartDto : LongOperationRestartDtoBase
    {
    }

    [Route("/long/{OperationName}/_progress/{ProcessKey}", "GET")]
    [Api("Reg")]
    [DataContract]
    public class LongOperationProgressDto : LongOperationProgressDtoBase
    {
    }

    [Route("/long/{OperationName}/_result/{ProcessKey}", "GET")]
    [Api("Reg")]
    [DataContract]
    public class LongOperationResultDto : LongOperationResultDtoBase
    {
    }

    [Route("/long/{OperationName}/_cancel/{ProcessKey}", "POST")]
    [Api("Reg")]
    [DataContract]
    public class LongOperationCancelDto : LongOperationCancelDtoBase
    {
    }

    [Route("/long/list", "GET")]
    [Api("Reg")]
    [DataContract]
    public class LongOperationListDto : LongOperationListDtoBase
    {
    }
}
