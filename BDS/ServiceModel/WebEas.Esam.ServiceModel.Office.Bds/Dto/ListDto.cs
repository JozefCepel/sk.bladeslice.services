using ServiceStack;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Dto
{
    [DataContract]
    [Route("/list/{KodPolozky}", "GET")]
    [Route("/list/{Code}/{KodPolozky}", "GET")]
    public class ListDto : BaseListDto
    {
    }
}
