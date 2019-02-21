using ServiceStack;
using System.Runtime.Serialization;

namespace WebEas.Esam.ServiceModel.Pfe.Dto
{
    /// <summary>
    /// Porovna tenanta s tenantom v session
    /// </summary>
    [Route("/poller/receive/{TenantId}", "GET")]
    [Api("")]
    [WebEasAuthenticate]
    [DataContract]
    public class PollerReceive
    {
        [ApiMember(Name = "TenantId", Description = "Prihlaseny tenant", DataType = "string")]
        [DataMember]
        public string TenantId { get; set; }
    }

    public class PollerReceiveResponse
    {
        public bool Changed { get; set; }
        public string TenantId { get; set; }
    }
}
