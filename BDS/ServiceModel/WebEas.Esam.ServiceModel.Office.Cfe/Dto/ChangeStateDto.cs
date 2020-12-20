using ServiceStack;
using System.Runtime.Serialization;
using WebEas.ServiceModel.Dto;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Dto
{
    [DataContract]
    [Route("/changestate", "POST")]
    [Api("Zmena stavu")]
    public class ChangeStateDto : BaseChangeStateDto
    {
    }
}
