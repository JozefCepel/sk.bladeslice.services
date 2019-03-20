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
    [Route("/CreateCielUkaz", "POST")]
    [Api("CielUkaz")]
    [DataContract]
    public class CreateCielUkaz : CielUkazDto { }

    // Update
    [WebEasRequiredRole(Roles.Admin)]
    [Route("/UpdateCielUkaz", "PUT")]
    [Api("CielUkaz")]
    [DataContract]
    public class UpdateCielUkaz : CielUkazDto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public long D_PRCielUkaz_Id { get; set; }
    }

    // Delete
    [WebEasRequiresAnyRole(RolesDefinition.Rzp.Roles.RzpMember)]
    [Route("/DeleteCielUkaz", "DELETE")]
    [Api("CielUkaz")]
    [DataContract]
    public class DeleteCielUkaz
    {
        [DataMember(IsRequired = true)]
        public long D_PRCielUkaz_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class CielUkazDto : BaseDto<CielUkaz>
    {
        [DataMember]
        [NotEmptyOrDefault]
        public long D_PRCiel_Id { get; set; }

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
        protected override void BindToEntity(CielUkaz data)
        {
            data.D_PRCiel_Id = D_PRCiel_Id;
            data.Typ = Typ;
            data.D_User_Id_Zodp = D_User_Id_Zodp;
            data.Nazov = Nazov;
            data.Popis = Popis;
            data.Komentar = Komentar;
        }
    }
    #endregion
}