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
    [Route("/CreateIntDoklad", "POST")]
    [Api("IntDoklad")]
    [DataContract]
    public class CreateIntDoklad : IntDokladDto { }

    // Update
    [WebEasRequiredRole(Roles.Admin)]
    [Route("/UpdateIntDoklad", "PUT")]
    [Api("IntDoklad")]
    [DataContract]
    public class UpdateIntDoklad : IntDokladDto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public long D_IntDoklad_Id { get; set; }
    }

    // Delete
    [WebEasRequiresAnyRole(RolesDefinition.Bds.Roles.BdsMember)]
    [Route("/DeleteIntDoklad", "DELETE")]
    [Api("IntDoklad")]
    [DataContract]
    public class DeleteIntDoklad
    {
        [DataMember(IsRequired = true)]
        public long D_IntDoklad_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class IntDokladDto : BaseDto<IntDoklad>
    {
        [DataMember]
        [NotEmptyOrDefault]
        public long D_BiznisEntita_Id { get; set; }

        [DataMember]
        public string Popis { get; set; }

        [DataMember]
        public decimal? Suma { get; set; }

        [DataMember]
        public int? C_BdsPredkontacia_Id { get; set; }

        [DataMember]
        public long? C_Stredisko_Id { get; set; }

        [DataMember]
        public string CisloDokladu { get; set; }

        [DataMember]
        public DateTime DatumDokladu { get; set; }

        [DataMember]
        public string VS { get; set; }

        [DataMember]
        public byte? UO { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(IntDoklad data)
        {
            data.D_BiznisEntita_Id = D_BiznisEntita_Id;
            data.Popis = Popis;
            data.Suma = Suma;
            data.C_BdsPredkontacia_Id = C_BdsPredkontacia_Id;
        }
    }
    #endregion
}
