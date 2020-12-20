using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Cfe.Types;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Types;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Dto
{
    // Create
    [Route("/CreateUserTenant", "POST")]
    [Api("UserTenant")]
    [DataContract]
    public class CreateUserTenant : UserTenantDto, IReturn<UserTenantView> { }

    // Update
    [Route("/UpdateUserTenant", "PUT")]
    [Api("UserTenant")]
    [DataContract]
    public class UpdateUserTenant : UserTenantDto, IReturn<UserTenantView>
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public int D_UserTenant_Id { get; set; }
    }

    // Delete
    [Route("/DeleteUserTenant", "DELETE")]
    [Api("UserTenant")]
    [DataContract]
    public class DeleteUserTenant
    {
        [DataMember(IsRequired = true)]
        public int[] D_UserTenant_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class UserTenantDto : BaseDto<UserTenant>
    {
        [DataMember]
        [NotEmptyOrDefault]
        public Guid D_User_Id { get; set; }

        [DataMember]
        [Required]
        public Guid D_Tenant_Id { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(UserTenant data)
        {
            data.D_User_Id = D_User_Id;
            data.D_Tenant_Id = D_Tenant_Id;
        }
    }
    #endregion
}