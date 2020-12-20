using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Cfe.Types;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Types;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Dto
{
    [Route("/TreeSetRightNo", "POST")]
    [Api("OrsElementType")]
    [DataContract]
    public class TreeSetRightNo
    {
        [DataMember(IsRequired = true)]
        public string[] IDs { get; set; }
    }

    [Route("/TreeSetRightRead", "POST")]
    [Api("OrsElementType")]
    [DataContract]
    public class TreeSetRightRead
    {
        [DataMember(IsRequired = true)]
        public string[] IDs { get; set; }
    }

    [Route("/TreeSetRightUpdate", "POST")]
    [Api("OrsElementType")]
    [DataContract]
    public class TreeSetRightUpdate
    {
        [DataMember(IsRequired = true)]
        public string[] IDs { get; set; }
    }

    [Route("/TreeSetRightFull", "POST")]
    [Api("OrsElementType")]
    [DataContract]
    public class TreeSetRightFull
    {
        [DataMember(IsRequired = true)]
        public string[] IDs { get; set; }
    }

    #region DTO
    [DataContract]
    public class TreePermissionDto : BaseDto<TreePermission>
    {
        [DataMember]
        public int C_Modul_Id { get; set; }

        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        public byte Pravo { get; set; }

        [DataMember]
        public int? C_Role_Id { get; set; }

        [DataMember]
        public Guid? D_User_Id { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(TreePermission data)
        {
            data.C_Modul_Id = C_Modul_Id;
            data.Kod = Kod;
            data.Pravo = Pravo;
            data.C_Role_Id = C_Role_Id;
            data.D_User_Id = D_User_Id;
        }
    }
    #endregion
}
