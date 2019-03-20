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
    [Route("/CreateTreePermission", "POST")]
    [Api("TreePermission")]
    [DataContract]
    public class CreateTreePermission : TreePermissionDto { }

    // Update
    [WebEasRequiredRole(Roles.Admin)]
    [Route("/UpdateTreePermission", "PUT")]
    [Api("TreePermission")]
    [DataContract]
    public class UpdateTreePermission : TreePermissionDto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public long D_TreePermission_Id { get; set; }
    }

    // Delete
    [WebEasRequiresAnyRole(RolesDefinition.Cfe.Roles.CfeMember)]
    [Route("/DeleteTreePermission", "DELETE")]
    [Api("TreePermission")]
    [DataContract]
    public class DeleteTreePermission
    {
        [DataMember(IsRequired = true)]
        public long D_TreePermission_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class TreePermissionDto : BaseDto<TreePermission>
    {
        [DataMember]
        public int C_Modul_Id { get; set; }

        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        public byte Pravo { get; set; }

        [DataMember]
        public int? C_Role_Id { get; set; }

        [DataMember]
        public Guid? D_User_Id { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(TreePermission data)
        {
            data.C_Modul_Id = C_Modul_Id;
            data.Kod = Kod;
            data.Pravo = Pravo;
            data.C_Role_Id = C_Role_Id;
            data.D_User_Id = D_User_Id;
        }
    }
    #endregion
}
