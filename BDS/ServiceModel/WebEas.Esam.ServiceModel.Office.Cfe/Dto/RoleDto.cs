using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Cfe.Types;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Dto
{
    // Create
    [Route("/CreateRole", "POST")]
    [Api("Role")]
    [DataContract]
    public class CreateRole : RoleDto, IReturn<RoleView> { }

    // Update
    [Route("/UpdateRole", "PUT")]
    [Api("Role")]
    [DataContract]
    public class UpdateRole : RoleDto, IReturn<RoleView>
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public int C_Role_Id { get; set; }
    }

    // Delete
    [Route("/DeleteRole", "DELETE")]
    [Api("Role")]
    [DataContract]
    public class DeleteRole
    {
        [DataMember(IsRequired = true)]
        public int[] C_Role_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class RoleDto : BaseDto<Role>
    {
        [DataMember]
        [NotEmptyOrDefault]
        public string Nazov { get; set; }

        [DataMember]
        public int? C_Role_Id_Parent { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(Role data)
        {
            data.Nazov = Nazov;
            data.C_Role_Id_Parent = C_Role_Id_Parent;
        }
    }
    #endregion
}