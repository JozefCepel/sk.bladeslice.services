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
    [Route("/CreateRoleUsers", "POST")]
    [Api("RoleUsers")]
    [DataContract]
    public class CreateRoleUsers : RoleUsersDto { }

    // Update
    [WebEasRequiresAnyRole(RolesDefinition.Bds.Roles.BdsMember)]
    [Route("/UpdateRoleUsers", "PUT")]
    [Api("RoleUsers")]
    [DataContract]
    public class UpdateRoleUsers : RoleUsersDto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public int D_RoleUsers_Id { get; set; }
    }

    // Delete
    [WebEasRequiresAnyRole(RolesDefinition.Cfe.Roles.CfeMember)]
    [Route("/DeleteRoleUsers", "DELETE")]
    [Api("RoleUsers")]
    [DataContract]
    public class DeleteRoleUsers
    {
        [DataMember(IsRequired = true)]
        public int D_RoleUsers_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class RoleUsersDto : BaseDto<RoleUsers>
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
        protected override void BindToEntity(RoleUsers data)
        {
            data.D_User_Id = D_User_Id;
            data.C_Role_Id = C_Role_Id;
            data.PlatnostOd = PlatnostOd;
            data.PlatnostDo = PlatnostDo;
        }
    }
    #endregion
}