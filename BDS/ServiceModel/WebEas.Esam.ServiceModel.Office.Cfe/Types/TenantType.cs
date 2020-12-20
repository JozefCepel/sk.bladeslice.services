using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [Schema("cfe")]
    [Alias("C_TenantType")]
    [DataContract]
    public class TenantType : BaseEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]        
        public byte C_TenantType_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Typ", Mandatory = true)]
        public string Typ { get; set; }

        [DataMember]
        [PfeColumn(Text = "Description")]        
        public string Popis { get; set; }

    }
}
