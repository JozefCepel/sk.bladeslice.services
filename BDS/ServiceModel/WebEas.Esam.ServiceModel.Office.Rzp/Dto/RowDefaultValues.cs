using ServiceStack;

namespace WebEas.Esam.ServiceModel.Office.Rzp.Dto
{
    /// <summary>
    /// Gets Row Default Values
    /// </summary>
    [Route("/GetRowDefaultValues", "GET")]
    [Api("GetRowDefaultValues")]
    [WebEasRequiredRole(RolesDefinition.Rzp.Roles.RzpMember)]
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
