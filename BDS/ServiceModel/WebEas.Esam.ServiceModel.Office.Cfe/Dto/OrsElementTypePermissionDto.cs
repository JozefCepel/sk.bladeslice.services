using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Cfe.Types;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Types;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Dto
{
    [Route("/OETSetRightNo", "POST")]
    [Api("OrsElementType")]
    [DataContract]
    public class OETSetRightNo
    {
        [DataMember(IsRequired = true)]
        public string[] IDs { get; set; }
    }

    [Route("/OETSetRightRead", "POST")]
    [Api("OrsElementType")]
    [DataContract]
    public class OETSetRightRead
    {
        [DataMember(IsRequired = true)]
        public string[] IDs { get; set; }
    }

    [Route("/OETSetRightUpdate", "POST")]
    [Api("OrsElementType")]
    [DataContract]
    public class OETSetRightUpdate
    {
        [DataMember(IsRequired = true)]
        public string[] IDs { get; set; }
    }

    [Route("/OETSetRightFull", "POST")]
    [Api("OrsElementType")]
    [DataContract]
    public class OETSetRightFull
    {
        [DataMember(IsRequired = true)]
        public string[] IDs { get; set; }
    }

    #region DTO
    [DataContract]
    public class OrsElementTypePermissionDto : BaseDto<OrsElementTypePermission>
    {
        [DataMember]
        public int C_OrsElementType_Id { get; set; }

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
        protected override void BindToEntity(OrsElementTypePermission data)
        {
            data.C_OrsElementType_Id = C_OrsElementType_Id;
            data.C_Role_Id = C_Role_Id;
            data.D_User_Id = D_User_Id;
            data.Pravo = Pravo;
        }
    }
    #endregion
}
