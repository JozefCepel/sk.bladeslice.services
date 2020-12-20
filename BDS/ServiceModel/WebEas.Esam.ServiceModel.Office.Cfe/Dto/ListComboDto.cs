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
    [Api("Hodnoty číselnika")]
    [Route("/combo/{KodPolozky}/{Column}", "GET")]
    [Route("/combo/{KodPolozky}/{Column}/{RequiredField*}", "GET")]
    public class ListComboDto : BaseListComboDto
    {
    }
}
