using System;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [Schema("cfe")]
    [Alias("D_TenantUsers")]
    [DataContract]
    public class TenantUsers : BaseEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]        
        public int D_TenantUsers_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Užívateľ")]
        public Guid D_User_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Tenant")]
        public Guid D_Tenant_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Oddelenie")]
        public int? D_Department_Id { get; set; }
    }
}
