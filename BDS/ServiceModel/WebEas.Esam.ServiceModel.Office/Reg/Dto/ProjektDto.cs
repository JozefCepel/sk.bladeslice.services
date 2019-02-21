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
    [Route("/CreateProjekt", "POST")]
    [Api("Projekt")]
    [DataContract]
    public class CreateProjekt : ProjektDto { }

    // Update
    [WebEasRequiredRole(Roles.Admin)]
    [Route("/UpdateProjekt", "PUT")]
    [Api("Projekt")]
    [DataContract]
    public class UpdateProjekt : ProjektDto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public long C_Projekt_Id { get; set; }
    }

    // Delete
    [WebEasRequiresAnyRole(RolesDefinition.Bds.Roles.BdsMember)]
    [Route("/DeleteProjekt", "DELETE")]
    [Api("Projekt")]
    [DataContract]
    public class DeleteProjekt
    {
        [DataMember(IsRequired = true)]
        public long C_Projekt_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class ProjektDto : BaseDto<Projekt>
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

        [DataMember]
        public int? C_TypBds_Id { get; set; }

        [DataMember]
        public DateTime? Datum { get; set; }

        [DataMember]
        public Guid? Poskytovatel { get; set; }

        [DataMember]
        public decimal? Suma { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(Projekt data)
        {
            data.Kod = Kod;
            data.Nazov = Nazov;
            data.Popis = Popis;
            data.PlatnostOd = PlatnostOd;
            data.PlatnostDo = PlatnostDo;
            data.C_TypBds_Id = C_TypBds_Id;
            data.Datum = Datum;
            data.Poskytovatel = Poskytovatel;
            data.Suma = Suma;
        }
    }
    #endregion
}
