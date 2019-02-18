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
    [Route("/treecounts", "POST")]
    [WebEasRequiredRole(RolesDefinition.Cfe.Roles.CfeMember)]
    public class GetTreeCountsDto : BaseGetTreeCounts
    {
    }
}
