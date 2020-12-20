using Telerik.Reporting;

namespace WebEas.Esam.ServiceModel.Office
{
    public class EsamReport : Reports.IReportInfo
    {
        public EsamReport(Report report)
        {
            TelerikReport = report;
        }

        public void SetDocumentProperties(string title, string keywords)
        {
            DocumentTitle = title;
            DocumentSubject = DocumentTitle;
            DocumentKeywords = keywords;
        }

        public Report TelerikReport { get; }
        public string DocumentAuthor { get; set; }
        public string DocumentSubject { get; set; }
        public string DocumentKeywords { get; set; }
        public string DocumentTitle { get; set; }
    }
}