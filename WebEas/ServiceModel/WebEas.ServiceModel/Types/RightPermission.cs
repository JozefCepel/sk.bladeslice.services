using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel.Types
{
    [DataContract]
    [Schema("cfe")]
    [Alias("D_RightPermission")]
    public class RightPermission : BaseTenantEntity
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        public int D_RightPermission_Id { get; set; }
        
        [DataMember]
        [PfeColumn(Text = "_C_Right_Id")]
        public int C_Right_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Role")]
        public int? C_Role_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_User_Id")]
        public Guid? D_User_Id { get; set; }
    }
}
