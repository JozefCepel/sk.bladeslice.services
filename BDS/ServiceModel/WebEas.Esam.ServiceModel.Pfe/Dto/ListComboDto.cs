using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Pfe.Dto
{
    [DataContract]
    
    [Api("Hodnoty číselnika")]
    [Route("/combo/{KodPolozky}/{Column}", "GET")]
    [Route("/combo/{KodPolozky}/{Column}/{RequiredField*}", "GET")]
    public class ListComboDto : BaseListComboDto
    {
    }
}