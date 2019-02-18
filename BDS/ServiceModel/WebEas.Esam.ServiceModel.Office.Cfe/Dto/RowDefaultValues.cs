using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Dto
{
    /// <summary>
    /// Gets Row Default Values
    /// </summary>
    [Route("/GetRowDefaultValues", "GET")]
    [Api("GetRowDefaultValues")]
    [WebEasRequiredRole(RolesDefinition.Cfe.Roles.CfeMember)]
    public class RowDefaultValues
    {
        [ApiMember(Name = "Code", Description = "kod aktualnej polozky v strome", DataType = "string", IsRequired = true)]
        public string code { get; set; }

        [ApiMember(Name = "masterCode", Description = "kod master polozky v strome", DataType = "string", IsRequired = true)]
        public string masterCode { get; set; }

        [ApiMember(Name = "masterRowId", Description = "Hodnota id stlpca aktualneho zaznamu v master", DataType = "string", IsRequired = true)]
        public string masterRowId { get; set; }
    }
}
