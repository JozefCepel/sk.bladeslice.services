using ServiceStack;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel.Pfe.Dto
{
    #region Translations

    [DataContract]
    [Route("/listtranslated", "GET")]
    [Route("/listtranslated/{KodPolozky}", "GET")]
    [Api("Zoznam preložených/prekladaných výrazov")]
    [WebEasRequiredRole(Roles.RegWriter)]
    public class ListTranslatedExpressions : BaseListDto
    {
        //[DataMember]
        //[ApiMember(Name = "KodPolozky", IsRequired = false)]
        //public string KodPolozky { get; set; }
    }

    [DataContract]
    [Route("/listtranslatedTest/{UniqueKey}", "GET")]
    [Api("Zoznam preložených/prekladaných výrazov")]
    [WebEasRequiredRole (Roles.RegWriter)]
    public class ListTranslatedExpressionsTest
    {
        [DataMember]
        [ApiMember(Name = "UniqueKey", IsRequired = true)]
        public string UniqueKey { get; set; }
    }

    [DataContract]
    [Route("/listtranslationcolumns", "GET")]
    [Route("/listtranslationcolumns/{KodPolozky}", "GET")]
    [Api("Zoznam prekladaných stĺpcov")]
    [WebEasRequiredRole(Roles.RegWriter)]
    public class ListTranslationColumns
    {
        [DataMember]
        [ApiMember(Name = "KodPolozky", IsRequired = false)]
        public string KodPolozky { get; set; }
    }

    #endregion
}