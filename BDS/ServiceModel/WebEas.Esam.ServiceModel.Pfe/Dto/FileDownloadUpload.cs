using WebEas.ServiceModel.Pfe.Types;
using ServiceStack;
using ServiceStack.DataAnnotations;
using ServiceStack.FluentValidation;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Net.Mime;

namespace WebEas.Esam.ServiceModel.Pfe.Dto
{
    [DataContract]
    [Route("/exportgridtoxls", HttpMethods.Post)]
    [Api("Export grid to XLS")]
    
    public class ExportGridToXLS
    {
        [DataMember]
        public string Xml { get; set; }

        [DataMember]
        public string Title { get; set; }
    }

    [DataContract]
    [Route("/getreport/{ReportId}", HttpMethods.Get)]
    [Api("Get report")]
    public class GetReportDto
    {
        [DataMember(IsRequired = true)]
        public string ReportId { get; set; }
    }

    [DataContract]
    [Route("/fileupload", HttpMethods.Post)]
    [Api("File upload")]
    [Description("File upload pre prilohy")]
    
    public class FileUpload : IReturn<List<FileUploadResponse>>
    {
        
    }

    [DataContract]
    public class FileUploadResponse
    {
        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string TempId { get; set; }

        [DataMember]
        public bool success { get; set; }
    }
}
