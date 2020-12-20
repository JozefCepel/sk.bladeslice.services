using ServiceStack;
using System.Runtime.Serialization;
using WebEas.ServiceModel.Dto;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Dto
{
    // Update
    [Route("/UpdateNastavenie", "PUT")]
    [Api("Nastavenie")]
    [DataContract]
    public class UpdateNastavenie : UpdateNastavenieBase
    {
    }
}
