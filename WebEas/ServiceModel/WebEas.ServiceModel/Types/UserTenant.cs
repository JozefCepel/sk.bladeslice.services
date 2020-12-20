using System;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel.Types
{
    [Schema("cfe")]
    [Alias("D_UserTenant")]
    [DataContract]
    public class UserTenant : BaseEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]        
        public int D_UserTenant_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Užívateľ")]
        public Guid D_User_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Tenant")]
        public Guid D_Tenant_Id { get; set; }
    }
}
