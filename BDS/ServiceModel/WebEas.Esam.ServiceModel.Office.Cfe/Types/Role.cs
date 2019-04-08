using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [Schema("cfe")]
    [Alias("C_Role")]
    [DataContract]
    public class Role : BaseEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]        
        public int C_Role_Id { get; set; }                     

        [DataMember]
        [PfeColumn(Text = "Name", Mandatory = true)]        
        public string Nazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Nadradená rola")]
        public int? C_Roles_Id_Parent { get; set; }
    }
}
