using ServiceStack;
using System.Runtime.Serialization;

namespace WebEas.Esam.ServiceModel.Office.Dto
{
    [DataContract]
    public class ReportGenerationStartBaseDto
    {
        [DataMember]
        [ApiMember(Name = "Identifiers", Description = "Identifikatory")]
        public long[] Identifiers { get; set; }

        [DataMember]
        [ApiMember(Name = "KodPolozky", Description = "Kód položky")]
        public string KodPolozky { get; set; }

        [DataMember]
        [ApiMember(Name = "ReportName", Description = "Názov reportu", DataType = "string", ParameterType = "path", IsRequired = true)]
        public string ReportName { get; set; }

        [DataMember]
        [ApiMember(Name = "ReportParameters", Description = "Parametre reportu v Base64", DataType = "string")]
        public string ReportParameters { get; set; }
    }

    [DataContract]
    public class ReportGenerationProgressBaseDto
    {
        [DataMember]
        [ApiMember(Name = "ProcessKey", Description = "identifikator procesu", DataType = "string", IsRequired = true, ParameterType = "path")]
        public string ProcessKey { get; set; }

        [DataMember]
        [ApiMember(Name = "ReportName", Description = "Nazov reportu", DataType = "string", ParameterType = "path", IsRequired = true)]
        public string ReportName { get; set; }
    }

    [DataContract]
    public class ReportGenerationCancelBaseDto
    {
        [DataMember]
        [ApiMember(Name = "ProcessKey", Description = "identifikator procesu", DataType = "string", IsRequired = true, ParameterType = "path")]
        public string ProcessKey { get; set; }

        [DataMember]
        [ApiMember(Name = "ReportName", Description = "Nazov reportu", DataType = "string", ParameterType = "path", IsRequired = true)]
        public string ReportName { get; set; }
    }

    [DataContract]
    public class ReportGenerationResultBaseDto
    {
        [DataMember]
        [ApiMember(Name = "ProcessKey", Description = "identifikator procesu", DataType = "string", IsRequired = true, ParameterType = "path")]
        public string ProcessKey { get; set; }

        [DataMember]
        [ApiMember(Name = "ReportName", Description = "Nazov reportu", DataType = "string", ParameterType = "path", IsRequired = true)]
        public string ReportName { get; set; }
    }
}
