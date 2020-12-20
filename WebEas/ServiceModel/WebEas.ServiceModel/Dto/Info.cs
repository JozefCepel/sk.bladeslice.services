using System;
using System.Linq;
using ServiceStack;

namespace WebEas.ServiceModel.Dto
{
    [Route("/sessioninfo", "GET")]
    [Api("Info")]
    public class SessionInfo
    {
    }

    [Route("/getcontexttenants", "GET")]
    [Api("Info")]
    public class GetContextTenants
    {
    }

    [Route("/checkContext/{TenantId}", "GET")]
    [Api("Info")]
    public class CheckContext
    {
        [ApiMember(Name = "TenantId", Description = "Id tenanta", DataType = "string")]
        public Guid TenantId { get; set; }
    }
}