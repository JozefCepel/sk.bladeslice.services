using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Bds.Types;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Dto
{
    // Create
    [WebEasRequiresAnyRole(RolesDefinition.Bds.Roles.BdsMember)]
    [Route("/CreateCiele", "POST")]
    [Api("Ciele")]
    [DataContract]
    public class CreateCiele : CieleDto { }

    // Update
    [WebEasRequiredRole(Roles.Admin)]
    [Route("/UpdateCiele", "PUT")]
    [Api("Ciele")]
    [DataContract]
    public class UpdateCiele : CieleDto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public long D_PRCiele_Id { get; set; }
    }

    // Delete
    [WebEasRequiresAnyRole(RolesDefinition.Bds.Roles.BdsMember)]
    [Route("/DeleteCiele", "DELETE")]
    [Api("Ciele")]
    [DataContract]
    public class DeleteCiele
    {
        [DataMember(IsRequired = true)]
        public long D_PRCiele_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class CieleDto : BaseDto<Ciele>
    {
        [DataMember]
        [Required]
        [NotEmptyOrDefault]
        public long D_Program_Id { get; set; }

        [DataMember]
        public byte? Typ { get; set; }

        [DataMember]
        public Guid? D_User_Id_Zodp { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        [DataMember]
        public string Popis { get; set; }

        [DataMember]
        public string Komentar { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(Ciele data)
        {
            data.D_Program_Id = D_Program_Id;
            data.Typ = Typ;
            data.D_User_Id_Zodp = D_User_Id_Zodp;
            data.Nazov = Nazov;
            data.Popis = Popis;
            data.Komentar = Komentar;
        }
    }
    #endregion
}