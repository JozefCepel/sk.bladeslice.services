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
    [Route("/CreateZmenyRzpVal", "POST")]
    [Api("ZmenyRzpVal")]
    [DataContract]
    public class CreateZmenyRzpVal : ZmenyRzpValDto { }

    // Update
    [WebEasRequiredRole(Roles.Admin)]
    [Route("/UpdateZmenyRzpVal", "PUT")]
    [Api("ZmenyRzpVal")]
    [DataContract]
    public class UpdateZmenyRzpVal : ZmenyRzpValDto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public long D_ZmenyRzpVal_Id { get; set; }
    }

    // Delete
    [WebEasRequiresAnyRole(RolesDefinition.Rzp.Roles.RzpMember)]
    [Route("/DeleteZmenyRzpVal", "DELETE")]
    [Api("ZmenyRzpVal")]
    [DataContract]
    public class DeleteZmenyRzpVal
    {
        [DataMember(IsRequired = true)]
        public long D_ZmenyRzpVal_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class ZmenyRzpValDto : BaseDto<ZmenyRzpVal>
    {
        [DataMember]
        public long D_NavrhZmenyRzp_Id { get; set; }

        [DataMember]
        public long? D_Program_Id { get; set; }

        [DataMember]
        public long? C_Projekt_Id { get; set; }

        [DataMember]
        public long? C_Stredisko_Id { get; set; }

        [DataMember]
        public long? C_RzpPolozky_Id { get; set; }

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
        protected override void BindToEntity(ZmenyRzpVal data)
        {
            data.D_NavrhZmenyRzp_Id = D_NavrhZmenyRzp_Id;
            data.D_Program_Id = D_Program_Id;
            data.C_Projekt_Id = C_Projekt_Id;
            data.C_Stredisko_Id = C_Stredisko_Id;
            data.C_RzpPolozky_Id = C_RzpPolozky_Id;
            data.Popis = Popis;
            data.SumaZmeny = SumaZmeny;
        }
    }
    #endregion
}
