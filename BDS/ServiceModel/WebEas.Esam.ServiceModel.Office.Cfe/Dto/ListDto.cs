using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Dto
{
    [DataContract]
    [Route("/list/{KodPolozky}", "GET,POST")]
    [Route("/list/{Code}/{KodPolozky}", "GET,POST")]
    public class ListDto : BaseListDto
    {
    }
}
