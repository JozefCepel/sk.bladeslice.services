using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Rzp.Types;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Rzp.Dto
{
    // Create
    [WebEasRequiresAnyRole(RolesDefinition.Rzp.Roles.RzpMember)]
    [Route("/CreateCiel", "POST")]
    [Api("Ciel")]
    [DataContract]
    public class CreateCiel : CielDto { }

    // Update
    [WebEasRequiredRole(Roles.Admin)]
    [Route("/UpdateCiel", "PUT")]
    [Api("Ciel")]
    [DataContract]
    public class UpdateCiel : CielDto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public long D_PRCiel_Id { get; set; }
    }

    // Delete
    [WebEasRequiresAnyRole(RolesDefinition.Rzp.Roles.RzpMember)]
    [Route("/DeleteCiel", "DELETE")]
    [Api("Ciel")]
    [DataContract]
    public class DeleteCiel
    {
        [DataMember(IsRequired = true)]
        public long D_PRCiel_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class CielDto : BaseDto<Ciel>
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
        protected override void BindToEntity(Ciel data)
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