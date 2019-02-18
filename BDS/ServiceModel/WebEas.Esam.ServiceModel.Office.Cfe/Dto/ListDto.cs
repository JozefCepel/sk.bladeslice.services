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
    [Route("/list/{KodPolozky}", "GET")]
    [Route("/list/{Code}/{KodPolozky}", "GET")]
    [WebEasRequiredRole(RolesDefinition.Cfe.Roles.CfeMember)]
    public class ListDto : BaseListDto
    {
    }
}
