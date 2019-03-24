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
    [Route("/CreateRole", "POST")]
    [Api("Role")]
    [DataContract]
    public class CreateRole : RoleDto { }

    // Update
    [WebEasRequiresAnyRole(RolesDefinition.Bds.Roles.BdsMember)]
    [Route("/UpdateRole", "PUT")]
    [Api("Role")]
    [DataContract]
    public class UpdateRole : RoleDto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public int C_Role_Id { get; set; }
    }

    // Delete
    [WebEasRequiresAnyRole(RolesDefinition.Cfe.Roles.CfeMember)]
    [Route("/DeleteRole", "DELETE")]
    [Api("Role")]
    [DataContract]
    public class DeleteRole
    {
        [DataMember(IsRequired = true)]
        public int C_Role_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class RoleDto : BaseDto<Role>
    {
        [DataMember]
        [NotEmptyOrDefault]
        public string Nazov { get; set; }

        [DataMember]
        public int? C_Roles_Id_Parent { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(Role data)
        {
            data.Nazov = Nazov;
            data.C_Roles_Id_Parent = C_Roles_Id_Parent;
        }
    }
    #endregion
}