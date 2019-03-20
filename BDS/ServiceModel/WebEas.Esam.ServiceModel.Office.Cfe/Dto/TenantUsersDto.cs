using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Cfe.Types;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Dto
{
    // Create
    [WebEasRequiresAnyRole(RolesDefinition.Cfe.Roles.CfeMember)]
    [Route("/CreateTenantUsers", "POST")]
    [Api("TenantUsers")]
    [DataContract]
    public class CreateTenantUsers : TenantUsersDto { }

    // Update
    [WebEasRequiredRole(Roles.Admin)]
    [Route("/UpdateTenantUsers", "PUT")]
    [Api("TenantUsers")]
    [DataContract]
    public class UpdateTenantUsers : TenantUsersDto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public int D_TenantUsers_Id { get; set; }
    }

    // Delete
    [WebEasRequiresAnyRole(RolesDefinition.Cfe.Roles.CfeMember)]
    [Route("/DeleteTenantUsers", "DELETE")]
    [Api("TenantUsers")]
    [DataContract]
    public class DeleteTenantUsers
    {
        [DataMember(IsRequired = true)]
        public int D_TenantUsers_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class TenantUsersDto : BaseDto<TenantUsers>
    {
        [DataMember]
        [NotEmptyOrDefault]
        public Guid D_User_Id { get; set; }

        [DataMember]
        [NotEmptyOrDefault]
        public Guid D_Tenant_Id { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(TenantUsers data)
        {
            data.D_User_Id = D_User_Id;
            data.D_Tenant_Id = D_Tenant_Id;
        }
    }
    #endregion
}