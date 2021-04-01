using System.Collections.Generic;

namespace WebEas.Esam.Reports.Dto
{
    public class ReportDataResponseDto
    {
        public List<IReportData> Data { get; set; }

        public string Error { get; set; }
    }
}
