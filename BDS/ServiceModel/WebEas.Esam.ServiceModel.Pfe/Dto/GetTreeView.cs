using System.Runtime.Serialization;
using ServiceStack;
using ServiceStack.DataAnnotations;

namespace WebEas.Esam.ServiceModel.Pfe.Dto
{
    /// <summary>
    /// RS 2 - Strom vybraného modulu
    /// </summary>
    [Route("/treeview/{SkratkaModulu}", "GET")]
    [Api("RS 2 - Strom vybraného modulu")]
    [WebEasAuthenticate]
    [DataContract]
    public class GetTreeView
    {
        [ApiMember(Name = "SkratkaModulu", Description = "Skratka vybraného modulu", DataType = "string", IsRequired = true)]
        [DataMember]
        public string SkratkaModulu { get; set; }
    }
}
