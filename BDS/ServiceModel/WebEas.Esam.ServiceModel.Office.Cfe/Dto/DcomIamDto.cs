using ServiceStack;
using System.Runtime.Serialization;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Dto
{
    // Create
    [Route("/SynchronizeDcomUsers", "POST")]
    [Api("DcomIam")]
    [DataContract]
    public class SynchronizeDcomUsersDto
    {
    }
}
