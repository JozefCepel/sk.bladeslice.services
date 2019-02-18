using System.Runtime.Serialization;
using ServiceStack;
using ServiceStack.DataAnnotations;

namespace WebEas.Esam.ServiceModel.Pfe.Dto
{
    /// <summary>
    /// RS 1 - Zoznam Modulov
    /// </summary>
    [Route("/ListModul", "GET,OPTIONS")]
    [Api("RS 1 - Zoznam Modulov")]
    [Description("RS 1 - Zoznam Modulov")]
    [WebEasAuthenticate]
    [DataContract]
    public class ListModul
    {
    }

    /// <summary>
    /// RS 1 - Zoznam Modulov aj s epo, bpm, oso
    /// </summary>
    [Route("/ListAllModules", "GET,OPTIONS")]
    [Api("RS 2 - Zoznam Modulov")]
    [Description("RS 2 - Zoznam Modulov")]
    [WebEasAuthenticate]
    [DataContract]
    public class ListAllModules
    {
    }

    [DataContract]
    public class ListAllModulesResponse
    {
        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        [DataMember]
        public string Url { get; set; }

        [DataMember]
        public bool Separator { get; set; }
    }
}