using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Cfe.Types;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Dto
{
    // Create
    [Route("/CreateUserRole", "POST")]
    [Api("UserRole")]
    [DataContract]
    public class CreateUserRole : UserRoleDto, IReturn<UserRoleView> { }

    // Update
    [Route("/UpdateUserRole", "PUT")]
    [Api("UserRole")]
    [DataContract]
    public class UpdateUserRole : UserRoleDto, IReturn<UserRoleView>
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public int D_UserRole_Id { get; set; }
    }

    // Delete
    [Route("/DeleteUserRole", "DELETE")]
    [Api("UserRole")]
    [DataContract]
    public class DeleteUserRole
    {
        [DataMember(IsRequired = true)]
        public int[] D_UserRole_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class UserRoleDto : BaseDto<UserRole>
    {
        [DataMember]
        [NotEmptyOrDefault]
        public Guid D_User_Id { get; set; }

        [DataMember]
        [NotEmptyOrDefault]
        public int C_Role_Id { get; set; }

        [DataMember]
        [NotEmptyOrDefault]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        public DateTime? PlatnostDo { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(UserRole data)
        {
            data.D_User_Id = D_User_Id;
            data.C_Role_Id = C_Role_Id;
            data.PlatnostOd = PlatnostOd;
            data.PlatnostDo = PlatnostDo;
        }
    }
    #endregion
}