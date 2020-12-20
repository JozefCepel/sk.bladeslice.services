using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Cfe.Types;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Dto
{
    [Route("/OESetRightNo", "POST")]
    [Api("OrsElementType")]
    [DataContract]
    public class OESetRightNo
    {
        [DataMember(IsRequired = true)]
        public string[] IDs { get; set; }
    }

    [Route("/OESetRightRead", "POST")]
    [Api("OrsElementType")]
    [DataContract]
    public class OESetRightRead
    {
        [DataMember(IsRequired = true)]
        public string[] IDs { get; set; }
    }

    [Route("/OESetRightUpdate", "POST")]
    [Api("OrsElementType")]
    [DataContract]
    public class OESetRightUpdate
    {
        [DataMember(IsRequired = true)]
        public string[] IDs { get; set; }
    }

    [Route("/OESetRightFull", "POST")]
    [Api("OrsElementType")]
    [DataContract]
    public class OESetRightFull
    {
        [DataMember(IsRequired = true)]
        public string[] IDs { get; set; }
    }

    #region DTO
    [DataContract]
    public class OrsElementPermissionDto : BaseDto<OrsElementPermission>
    {
        [DataMember]
        public int C_OrsElement_Id { get; set; }

        [DataMember]
        public int? C_Role_Id { get; set; }

        [DataMember]
        public Guid? D_User_Id { get; set; }

        [DataMember]
        public byte Pravo { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(OrsElementPermission data)
        {
            data.C_OrsElement_Id = C_OrsElement_Id;
            data.C_Role_Id = C_Role_Id;
            data.D_User_Id = D_User_Id;
            data.Pravo = Pravo;
        }
    }
    #endregion
}
