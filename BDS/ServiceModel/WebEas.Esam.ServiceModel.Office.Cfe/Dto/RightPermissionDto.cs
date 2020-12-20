using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Cfe.Types;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Types;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Dto
{
    // AddRightPermission
    [Route("/AddRightPermission", "POST")]
    [Api("RightPermission")]
    [DataContract]
    public class AddRightPermission : RightPermissionDto
    {
        [DataMember(IsRequired = true)]
        public string IdField { get; set; }

        [DataMember(IsRequired = true)]
        public string[] IDs { get; set; }
    }

    // RemoveRightPermission
    [Route("/RemoveRightPermission", "POST")]
    [Api("RightPermission")]
    [DataContract]
    public class RemoveRightPermission
    {
        [DataMember(IsRequired = true)]
        public string IdField { get; set; }

        [DataMember(IsRequired = true)]
        public string[] IDs { get; set; }
    }

    #region DTO
    [DataContract]
    public class RightPermissionDto : BaseDto<RightPermission>
    {
        [DataMember]
        public int C_Right_Id { get; set; }

        [DataMember]
        public int? C_Role_Id { get; set; }

        [DataMember]
        public Guid? D_User_Id { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(RightPermission data)
        {
            data.C_Right_Id = C_Right_Id;
            data.C_Role_Id = C_Role_Id;
            data.D_User_Id = D_User_Id;
        }
    }
    #endregion
}
