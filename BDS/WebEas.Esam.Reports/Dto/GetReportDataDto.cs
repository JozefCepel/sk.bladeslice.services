namespace WebEas.Esam.Reports.Dto
{
    public abstract class ReportDataRequestDto
    {
        public string ReportId { get; set; }

        public string ReportParameters { get; set; }

        public string SessionId { get; set; }
    }

    public class UctReportDataRequestDto : ReportDataRequestDto
    {

    }

    public class RzpReportDataRequestDto : ReportDataRequestDto
    {

    }
}
