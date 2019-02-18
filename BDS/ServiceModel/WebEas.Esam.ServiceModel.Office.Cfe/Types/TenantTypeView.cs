using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [Schema("cfe")]
    [Alias("V_TenantType")]
    [DataContract]
    public class TenantTypeView : BaseEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]        
        public short D_TenantType_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Typ")]
        public string Typ { get; set; }

        [DataMember]
        [PfeColumn(Text = "Popis")]        
        public string Popis { get; set; }

    }
}
