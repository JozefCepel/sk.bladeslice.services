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
    [Route("/CreateRight", "POST")]
    [Api("Right")]
    [DataContract]
    public class CreateRight : RightDto { }

    // Update
    [WebEasRequiredRole(Roles.Admin)]
    [Route("/UpdateRight", "PUT")]
    [Api("Right")]
    [DataContract]
    public class UpdateRight : RightDto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public long C_Right_Id { get; set; }
    }

    // Delete
    [WebEasRequiresAnyRole(RolesDefinition.Cfe.Roles.CfeMember)]
    [Route("/DeleteRight", "DELETE")]
    [Api("Right")]
    [DataContract]
    public class DeleteRight
    {
        [DataMember(IsRequired = true)]
        public long C_Right_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class RightDto : BaseDto<Right>
    {
        [DataMember]
        public int C_Modul_Id { get; set; }

        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(Right data)
        {
            data.C_Modul_Id = C_Modul_Id;
            data.Kod = Kod;
            data.Nazov = Nazov;
        }
    }
    #endregion
}
