using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack;

namespace WebEas.Esam.ServiceModel.Pfe.Dto
{
    [Route("/actualtoken", "GET")]
    [Api("Info")]
    public class ActualToken
    {
    }

    [Route("/lllj/{Identifier}", "GET")]
    [Api("Info")]
    #if DEBUG || INT || DEVELOP
    [WebEasAuthenticate]
    #else
    [WebEasRequiredRole(Roles.Admin)]
    #endif
    public class LogViewDto
    {
        [DataMember]
        [ApiMember(Name = "Identifier", DataType = "string", IsRequired = true) ]
        public string Identifier { get; set; }
    }

    /*COMMENTED 29.12
    #if DEBUG || INT || DEVELOP || TEST
    [WebEasAuthenticate]
    #else
    [WebEasRequiredRole(Roles.Admin)]
    #endif
    */

    [Route("/lll/{Identifier}", "GET")]
    [Api("Info")]
    [WebEasAuthenticate]
    public class LogViewRawDto
    {
        [DataMember]
        [ApiMember(Name = "Identifier", DataType = "string", IsRequired = true)]
        public string Identifier { get; set; }
    }

    [Route("/corrid/{Identifier}", "GET")]
    [Api("Info")]
#if DEBUG || INT || DEVELOP || TEST
    [WebEasAuthenticate]
#else
    [WebEasRequiredRole(Roles.Admin)]
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
 