using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WebEas.Esam.ServiceModel.Pfe.Dto
{
    [DataContract]
    [Route("/getcontextuser/{ModuleShortcut}", "GET")]
    [Route("/getcontextuser", "GET")]
    [Api("ziskanie mena prihlaseneho pouzivatela")]
    
    public class GetContextUser
    {
        [ApiMember(Name = "ModuleShortcut", Description = "Skratka modulu", DataType = "string")]
        [DataMember]
        public string ModuleShortcut { get; set; }
    }
}
