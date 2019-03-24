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
    [Route("/CreateOrsElementPermission", "POST")]
    [Api("OrsElementPermission")]
    [DataContract]
    public class CreateOrsElementPermission : OrsElementPermissionDto { }

    // Update
    [WebEasRequiresAnyRole(RolesDefinition.Bds.Roles.BdsMember)]
    [Route("/UpdateOrsElementPermission", "PUT")]
    [Api("OrsElementPermission")]
    [DataContract]
    public class UpdateOrsElementPermission : OrsElementPermissionDto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public long D_OrsElementPermission_Id { get; set; }
    }

    // Delete
    [WebEasRequiresAnyRole(RolesDefinition.Cfe.Roles.CfeMember)]
    [Route("/DeleteOrsElementPermission", "DELETE")]
    [Api("OrsElementPermission")]
    [DataContract]
    public class DeleteOrsElementPermission
    {
        [DataMember(IsRequired = true)]
        public long D_OrsElementPermission_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class OrsElementPermissionDto : BaseDto<OrsElementPermission>
    {
        [DataMember]
        public int C_OrsElement_Id { get; set; }

        [DataMember]
        public int? C_Role_Id { get; set; }

        [DataMember]
        public Guid? D_User_Id { get; set; }

        [DataMember]
        public byte Pravo { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(OrsElementPermission data)
        {
            data.C_OrsElement_Id = C_OrsElement_Id;
            data.C_Role_Id = C_Role_Id;
            data.D_User_Id = D_User_Id;
            data.Pravo = Pravo;
        }
    }
    #endregion
}
