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
    [Route("/CreateCieleUkaz", "POST")]
    [Api("CieleUkaz")]
    [DataContract]
    public class CreateCieleUkaz : CieleUkazDto { }

    // Update
    [WebEasRequiredRole(Roles.Admin)]
    [Route("/UpdateCieleUkaz", "PUT")]
    [Api("CieleUkaz")]
    [DataContract]
    public class UpdateCieleUkaz : CieleUkazDto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public long D_PRCieleUkaz_Id { get; set; }
    }

    // Delete
    [WebEasRequiresAnyRole(RolesDefinition.Rzp.Roles.RzpMember)]
    [Route("/DeleteCieleUkaz", "DELETE")]
    [Api("CieleUkaz")]
    [DataContract]
    public class DeleteCieleUkaz
    {
        [DataMember(IsRequired = true)]
        public long D_PRCieleUkaz_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class CieleUkazDto : BaseDto<CieleUkaz>
    {
        [DataMember]
        [NotEmptyOrDefault]
        public long D_PRCiele_Id { get; set; }

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
        protected override void BindToEntity(CieleUkaz data)
        {
            data.D_PRCiele_Id = D_PRCiele_Id;
            data.Typ = Typ;
            data.D_User_Id_Zodp = D_User_Id_Zodp;
            data.Nazov = Nazov;
            data.Popis = Popis;
            data.Komentar = Komentar;
        }
    }
    #endregion
}