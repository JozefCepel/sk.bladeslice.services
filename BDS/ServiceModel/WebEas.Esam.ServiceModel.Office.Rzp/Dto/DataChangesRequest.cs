using ServiceStack;
using System.Runtime.Serialization;
using WebEas.ServiceModel.Dto;

namespace WebEas.Esam.ServiceModel.Office.Rzp.Dto
{
    [DataContract]
    [Route("/datachanges/{Code}/{RowId}", "GET")]
    [WebEasRequiresAnyRole(RolesDefinition.Rzp.Roles.RzpMember)]
    public class DataChangesRequest : DataChangesRequestBase
    {
    }
}
