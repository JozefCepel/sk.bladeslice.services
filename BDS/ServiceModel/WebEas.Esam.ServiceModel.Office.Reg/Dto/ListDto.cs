using ServiceStack;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Reg.Dto
{
    [DataContract]
    [Route("/list/{KodPolozky}", "GET,POST")]
    [Route("/list/{Code}/{KodPolozky}", "GET,POST")]
    public class ListDto : BaseListDto
    {
    }
}
