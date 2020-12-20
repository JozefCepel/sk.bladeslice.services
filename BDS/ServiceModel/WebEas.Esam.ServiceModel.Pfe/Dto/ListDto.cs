using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Pfe.Dto
{
    [DataContract]
    [Route("/list/{KodPolozky}", "GET,POST")]
    [Route("/list/{Code}/{KodPolozky}", "GET,POST")]
    
    public class ListDto : BaseListDto
    {
    }
}