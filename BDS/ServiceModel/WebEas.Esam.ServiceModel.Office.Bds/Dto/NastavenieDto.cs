using ServiceStack;
using System.Runtime.Serialization;
using WebEas.ServiceModel.Dto;

namespace WebEas.Esam.ServiceModel.Office.Bds.Dto
{
    // Update
    [Route("/UpdateNastavenie", "PUT")]
    [Api("Nastavenie")]
    [DataContract]
    public class UpdateNastavenie : UpdateNastavenieBase
    {
    }
}
