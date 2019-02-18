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
    [Route("/CreateRzpPolozky", "POST")]
    [Api("RzpPolozky")]
    [DataContract]
    public class CreateRzpPolozky : RzpPolozkyDto { }

    // Update
    [WebEasRequiredRole(Roles.Admin)]
    [Route("/UpdateRzpPolozky", "PUT")]
    [Api("RzpPolozky")]
    [DataContract]
    public class UpdateRzpPolozky : RzpPolozkyDto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public long C_RzpPolozky_Id { get; set; }
    }

    // Delete
    [WebEasRequiresAnyRole(RolesDefinition.Rzp.Roles.RzpMember)]
    [Route("/DeleteRzpPolozky", "DELETE")]
    [Api("RzpPolozky")]
    [DataContract]
    public class DeleteRzpPolozky
    {
        [DataMember(IsRequired = true)]
        public long C_RzpPolozky_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class RzpPolozkyDto : BaseDto<RzpPolozky>
    {
        [DataMember]
        [NotEmptyOrDefault]
        public string RzpNazov { get; set; }

        [DataMember]
        [NotEmptyOrDefault]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        [NotEmptyOrDefault]
        public byte PrijemVydaj { get; set; }

        [DataMember]
        [Required]
        public bool Stredisko { get; set; }

        [DataMember]
        [Required]
        public bool Projekt { get; set; }

        [DataMember]
        [Required]
        public bool OpacnaStrana { get; set; }

        [DataMember]
        public int? C_FRZdroj_Id { get; set; }

        [DataMember]
        public int? C_FREK_Id { get; set; }

        [DataMember]
        public int? C_FRFK_Id { get; set; }

        [DataMember]
        public string A1 { get; set; }

        [DataMember]
        public string A2 { get; set; }

        [DataMember]
        public string A3 { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(RzpPolozky data)
        {
            data.RzpNazov = RzpNazov;
            data.PlatnostOd = PlatnostOd;
            data.PrijemVydaj = PrijemVydaj;
            data.Stredisko = Stredisko;
            data.Projekt = Projekt;
            data.OpacnaStrana = OpacnaStrana;
            data.C_FRZdroj_Id = C_FRZdroj_Id;
            data.C_FREK_Id = C_FREK_Id;
            data.C_FRFK_Id = C_FRFK_Id;
            data.A1 = A1;
            data.A2 = A2;
            data.A3 = A3;
        }
    }
    #endregion
}