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
    [Route("/CreateRzpDennik", "POST")]
    [Api("RzpDennik")]
    [DataContract]
    public class CreateRzpDennik : RzpDennikDto { }

    // Update
    [WebEasRequiredRole(Roles.Admin)]
    [Route("/UpdateRzpDennik", "PUT")]
    [Api("RzpDennik")]
    [DataContract]
    public class UpdateRzpDennik : RzpDennikDto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public long D_RzpDennik_Id { get; set; }
    }

    // Delete
    [WebEasRequiresAnyRole(RolesDefinition.Rzp.Roles.RzpMember)]
    [Route("/DeleteRzpDennik", "DELETE")]
    [Api("RzpDennik")]
    [DataContract]
    public class DeleteRzpDennik
    {
        [DataMember(IsRequired = true)]
        public long D_RzpDennik_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class RzpDennikDto : BaseDto<RzpDennik>
    {
        [DataMember]
        public long? D_BiznisEntita_Id { get; set; }

        [DataMember]
        [NotEmptyOrDefault]
        public long C_RzpPol_Id { get; set; }

        [DataMember]
        public long? D_Program_Id { get; set; }

        [DataMember]
        public int? C_Stredisko_Id { get; set; }

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
        protected override void BindToEntity(RzpDennik data)
        {
            data.D_BiznisEntita_Id = D_BiznisEntita_Id;
            data.C_RzpPol_Id = C_RzpPol_Id;
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
