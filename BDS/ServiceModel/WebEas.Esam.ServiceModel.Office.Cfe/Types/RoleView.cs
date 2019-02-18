using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [Schema("cfe")]
    [Alias("C_Role")]
    [DataContract]
    public class RoleView : BaseEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]        
        public int C_Role_Id { get; set; }                     

        [DataMember]
        [PfeColumn(Text = "Názov")]        
        public string Nazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Nadradená rola")]
        public string C_Roles_Id_Parent { get; set; }

        [DataMember]
        [PfeColumn(Text = "Nadradená rola")]
        [PfeCombo(typeof(RoleView), NameColumn = "C_Roles_Id_Parent")]
        public string RoleParent { get; set; }

    }
}
