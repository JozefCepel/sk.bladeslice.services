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
    [Route("/CreateRzpPolZmena", "POST")]
    [Api("RzpPolZmena")]
    [DataContract]
    public class CreateRzpPolZmena : RzpPolZmenaDto { }

    // Update
    [WebEasRequiredRole(Roles.Admin)]
    [Route("/UpdateRzpPolZmena", "PUT")]
    [Api("RzpPolZmena")]
    [DataContract]
    public class UpdateRzpPolZmena : RzpPolZmenaDto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public long D_RzpPolZmena_Id { get; set; }
    }

    // Delete
    [WebEasRequiresAnyRole(RolesDefinition.Rzp.Roles.RzpMember)]
    [Route("/DeleteRzpPolZmena", "DELETE")]
    [Api("RzpPolZmena")]
    [DataContract]
    public class DeleteRzpPolZmena
    {
        [DataMember(IsRequired = true)]
        public long D_RzpPolZmena_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class RzpPolZmenaDto : BaseDto<RzpPolZmena>
    {
        [DataMember]
        public long D_Rzp_Id { get; set; }

        [DataMember]
        public long? D_Program_Id { get; set; }

        [DataMember]
        public long? C_Projekt_Id { get; set; }

        [DataMember]
        public long? C_Stredisko_Id { get; set; }

        [DataMember]
        public long? C_RzpPol_Id { get; set; }

        [DataMember]
        [NotEmptyOrDefault]
        public decimal SumaZmeny { get; set; }

        [DataMember]
        [NotEmptyOrDefault]
        public string Popis { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(RzpPolZmena data)
        {
            data.D_Rzp_Id = D_Rzp_Id;
            data.D_Program_Id = D_Program_Id;
            data.C_Projekt_Id = C_Projekt_Id;
            data.C_Stredisko_Id = C_Stredisko_Id;
            data.C_RzpPol_Id = C_RzpPol_Id;
            data.Popis = Popis;
            data.SumaZmeny = SumaZmeny;
        }
    }
    #endregion
}
