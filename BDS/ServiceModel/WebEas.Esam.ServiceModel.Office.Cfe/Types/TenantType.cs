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
        [PfeColumn(Text = "Názov", Mandatory = true)]
        public string Typ { get; set; }

        [DataMember]
        [PfeColumn(Text = "Code")]
        public string Kod { get; set; }

        [DataMember]
        [PfeColumn(Text = "Description", Mandatory = true)]
        public string Popis { get; set; }

    }
}
