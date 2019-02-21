using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Reg.Types;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Reg.Dto
{
    // Create
    [WebEasRequiresAnyRole(RolesDefinition.Bds.Roles.BdsMember)]
    [Route("/CreateStredisko", "POST")]
    [Api("Stredisko")]
    [DataContract]
    public class CreateStredisko : StrediskoDto { }

    // Update
    [WebEasRequiredRole(Roles.Admin)]
    [Route("/UpdateStredisko", "PUT")]
    [Api("Stredisko")]
    [DataContract]
    public class UpdateStredisko : StrediskoDto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public long C_Stredisko_Id { get; set; }
    }

    // Delete
    [WebEasRequiresAnyRole(RolesDefinition.Bds.Roles.BdsMember)]
    [Route("/DeleteStredisko", "DELETE")]
    [Api("Stredisko")]
    [DataContract]
    public class DeleteStredisko
    {
        [DataMember(IsRequired = true)]
        public long C_Stredisko_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class StrediskoDto : BaseDto<Stredisko>
    {
        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        [DataMember]
        public string Popis { get; set; }

        [DataMember]
        [NotEmptyOrDefault]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        public DateTime? PlatnostDo { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(Stredisko data)
        {
            data.Kod = Kod;
            data.Nazov = Nazov;
            data.Popis = Popis;
            data.PlatnostOd = PlatnostOd;
            data.PlatnostDo = PlatnostDo;
        }
    }
    #endregion
}
