using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack;

namespace WebEas.Esam.ServiceModel.Pfe.Dto
{
    [Route("/lllj/{Identifier}", "GET")]
    [Api("Info")]
    #if DEBUG || INT || DEVELOP
    
    #else
    #endif
    public class LogViewDto
    {
        [DataMember]
        [ApiMember(Name = "Identifier", DataType = "string", IsRequired = true) ]
        public string Identifier { get; set; }
    }

    /*COMMENTED 29.12
    #if DEBUG || INT || DEVELOP || TEST
    
    #else
    #endif
    */

    [Route("/lll/{Identifier}", "GET")]
    [Api("Info")]
    
    public class LogViewRawDto
    {
        [DataMember]
        [ApiMember(Name = "Identifier", DataType = "string", IsRequired = true)]
        public string Identifier { get; set; }
    }

    [Route("/corrid/{Identifier}", "GET")]
    [Api("Info")]
#if DEBUG || INT || DEVELOP || TEST
    
#else
#endif
    public class LogViewCorIdRawDto
    {
        [DataMember]
        [ApiMember(Name = "Identifier", DataType = "string", IsRequired = true)]
        public string Identifier { get; set; }
    }

    [Route("/listinfo", "GET")]
    [Api("Info")]
    public class ListInfo
    {
    }

    [Route("/appinfo", "GET")]
    [Api("Info")]
    public class AppInfo
    {
    }
}
 