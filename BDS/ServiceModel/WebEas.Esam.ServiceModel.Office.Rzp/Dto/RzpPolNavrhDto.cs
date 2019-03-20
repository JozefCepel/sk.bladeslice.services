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
    [Route("/CreateRzpPolNavrh", "POST")]
    [Api("RzpPolNavrh")]
    [DataContract]
    public class CreateRzpPolNavrh : RzpPolNavrhDto { }

    // Update
    [WebEasRequiredRole(Roles.Admin)]
    [Route("/UpdateRzpPolNavrh", "PUT")]
    [Api("RzpPolNavrh")]
    [DataContract]
    public class UpdateRzpPolNavrh : RzpPolNavrhDto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public long D_RzpPolNavrh_Id { get; set; }
    }

    // Delete
    [WebEasRequiresAnyRole(RolesDefinition.Rzp.Roles.RzpMember)]
    [Route("/DeleteRzpPolNavrh", "DELETE")]
    [Api("RzpPolNavrh")]
    [DataContract]
    public class DeleteRzpPolNavrh
    {
        [DataMember(IsRequired = true)]
        public long D_RzpPolNavrh_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class RzpPolNavrhDto : BaseDto<RzpPolNavrh>
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

        [DataMember(IsRequired = true)]
        public decimal SchvalenyRzp { get; set; }

        [DataMember(IsRequired = true)]
        public decimal NavrhRzp1 { get; set; }

        [DataMember(IsRequired = true)]
        public decimal NavrhRzp2 { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(RzpPolNavrh data)
        {
            data.D_Rzp_Id = D_Rzp_Id;
            data.D_Program_Id = D_Program_Id;
            data.C_Projekt_Id = C_Projekt_Id;
            data.C_Stredisko_Id = C_Stredisko_Id;
            data.C_RzpPol_Id = C_RzpPol_Id;
            data.SchvalenyRzp = SchvalenyRzp;
            data.NavrhRzp1 = NavrhRzp1;
            data.NavrhRzp2 = NavrhRzp2;
        }
    }
    #endregion
}
