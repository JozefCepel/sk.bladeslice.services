using System;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [Schema("cfe")]
    [Alias("D_TenantUsers")]
    [DataContract]
    public class TenantUsersView : BaseEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]        
        public int D_TenantUsers_Id { get; set; }

        [DataMember]
        public Guid D_User_Id { get; set; }

        [DataMember]
        public Guid D_Tenant_Id { get; set; }

        [DataMember]
        public int D_Department_Id { get; set; }
    }
}
