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
    [Route("/CreateNavrhyRzpVal", "POST")]
    [Api("NavrhyRzpVal")]
    [DataContract]
    public class CreateNavrhyRzpVal : NavrhyRzpValDto { }

    // Update
    [WebEasRequiredRole(Roles.Admin)]
    [Route("/UpdateNavrhyRzpVal", "PUT")]
    [Api("NavrhyRzpVal")]
    [DataContract]
    public class UpdateNavrhyRzpVal : NavrhyRzpValDto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public long D_NavrhyRzpVal_Id { get; set; }
    }

    // Delete
    [WebEasRequiresAnyRole(RolesDefinition.Rzp.Roles.RzpMember)]
    [Route("/DeleteNavrhyRzpVal", "DELETE")]
    [Api("NavrhyRzpVal")]
    [DataContract]
    public class DeleteNavrhyRzpVal
    {
        [DataMember(IsRequired = true)]
        public long D_NavrhyRzpVal_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class NavrhyRzpValDto : BaseDto<NavrhyRzpVal>
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
        protected override void BindToEntity(NavrhyRzpVal data)
        {
            data.D_NavrhZmenyRzp_Id = D_NavrhZmenyRzp_Id;
            data.D_Program_Id = D_Program_Id;
            data.C_Projekt_Id = C_Projekt_Id;
            data.C_Stredisko_Id = C_Stredisko_Id;
            data.C_RzpPolozky_Id = C_RzpPolozky_Id;
            data.SchvalenyRzp = SchvalenyRzp;
            data.NavrhRzp1 = NavrhRzp1;
            data.NavrhRzp2 = NavrhRzp2;
        }
    }
    #endregion
}
