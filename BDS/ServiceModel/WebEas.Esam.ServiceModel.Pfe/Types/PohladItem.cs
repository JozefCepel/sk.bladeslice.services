using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel.Pfe.Types
{
    [Schema("pfe")]
    [Alias("D_Pohlad")]
    [TenantUpdatable]
    [DataContract]
    public class PohladItem
    {
        [AutoIncrement]
        [PrimaryKey]
        [Alias("D_Pohlad_Id")]
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        [DataMember]
        public string TypAkcie { get; set; }
    }
}