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
    [Route("/CreateDennikRzp", "POST")]
    [Api("DennikRzp")]
    [DataContract]
    public class CreateDennikRzp : DennikRzpDto { }

    // Update
    [WebEasRequiredRole(Roles.Admin)]
    [Route("/UpdateDennikRzp", "PUT")]
    [Api("DennikRzp")]
    [DataContract]
    public class UpdateDennikRzp : DennikRzpDto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public long D_DennikRzp_Id { get; set; }
    }

    // Delete
    [WebEasRequiresAnyRole(RolesDefinition.Rzp.Roles.RzpMember)]
    [Route("/DeleteDennikRzp", "DELETE")]
    [Api("DennikRzp")]
    [DataContract]
    public class DeleteDennikRzp
    {
        [DataMember(IsRequired = true)]
        public long D_DennikRzp_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class DennikRzpDto : BaseDto<DennikRzp>
    {
        [DataMember]
        public long? D_IntDoklad_Id { get; set; }

        [DataMember]
        [NotEmptyOrDefault]
        public long C_RzpPolozky_Id { get; set; }

        [DataMember]
        public long? D_Program_Id { get; set; }

        [DataMember]
        public long? C_Stredisko_Id { get; set; }

        [DataMember]
        public long? C_Projekt_Id { get; set; }

        [DataMember]
        public string Popis { get; set; }

        [DataMember]
        [NotEmptyOrDefault]
        public decimal Suma { get; set; }

        [DataMember]
        [NotEmptyOrDefault]
        public decimal Pocet { get; set; }

        [DataMember]
        public int Poradie { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(DennikRzp data)
        {
            data.D_IntDoklad_Id = D_IntDoklad_Id;
            data.C_RzpPolozky_Id = C_RzpPolozky_Id;
            data.D_Program_Id = D_Program_Id;
            data.C_Stredisko_Id = C_Stredisko_Id;
            data.C_Projekt_Id = C_Projekt_Id;
            data.Popis = Popis;
            data.Suma = Suma;
            data.Pocet = Pocet;
            data.Poradie = Poradie;
        }
    }
    #endregion
}
