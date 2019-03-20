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
    [Route("/CreateRightPermission", "POST")]
    [Api("RightPermission")]
    [DataContract]
    public class CreateRightPermission : RightPermissionDto { }

    // Update
    [WebEasRequiredRole(Roles.Admin)]
    [Route("/UpdateRightPermission", "PUT")]
    [Api("RightPermission")]
    [DataContract]
    public class UpdateRightPermission : RightPermissionDto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public long D_RightPermission_Id { get; set; }
    }

    // Delete
    [WebEasRequiresAnyRole(RolesDefinition.Cfe.Roles.CfeMember)]
    [Route("/DeleteRightPermission", "DELETE")]
    [Api("RightPermission")]
    [DataContract]
    public class DeleteRightPermission
    {
        [DataMember(IsRequired = true)]
        public long D_RightPermission_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class RightPermissionDto : BaseDto<RightPermission>
    {
        [DataMember]
        public int C_Right_Id { get; set; }

        [DataMember]
        public int? C_Role_Id { get; set; }

        [DataMember]
        public Guid? D_User_Id { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(RightPermission data)
        {
            data.C_Right_Id = C_Right_Id;
            data.C_Role_Id = C_Role_Id;
            data.D_User_Id = D_User_Id;
        }
    }
    #endregion
}
