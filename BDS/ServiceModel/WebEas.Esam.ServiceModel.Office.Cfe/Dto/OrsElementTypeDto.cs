using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Cfe.Types;
using WebEas.Esam.ServiceModel.Office.Types.Cfe;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Dto
{
    // Create ors
    [Route("/CreateOrsElementType", "POST")]
    [Api("OrsElementType")]
    [DataContract]
    public class CreateOrsElementType : OrsElementTypeDto { }

    // Update
    [Route("/UpdateOrsElementType", "PUT")]
    [Api("OrsElementType")]
    [DataContract]
    public class UpdateOrsElementType : OrsElementTypeDto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public long C_OrsElementType_Id { get; set; }
    }

    // Delete
    [Route("/DeleteOrsElementType", "DELETE")]
    [Api("OrsElementType")]
    [DataContract]
    public class DeleteOrsElementType
    {
        [DataMember(IsRequired = true)]
        public long C_OrsElementType_Id { get; set; }
    }

    // Obnov ORS Elementy
    [Route("/ObnovitZoznamORS", "POST")]
    [Api("OrsElementType")]
    [DataContract]
    public class ObnovitZoznamORS
    {
        [DataMember(IsRequired = true)]
        public int[] IDs { get; set; }
    }

    #region DTO
    [DataContract]
    public class OrsElementTypeDto : BaseDto<OrsElementType>
    {
        [DataMember]
        public int C_Modul_Id { get; set; }

        [DataMember]
        public string DbSchema { get; set; }

        [DataMember]
        public string DbView { get; set; }

        [DataMember]
        public string DbIdField { get; set; }

        [DataMember]
        public string DbListField { get; set; }

        [DataMember]
        public string DbDeletedField { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(OrsElementType data)
        {
            data.C_Modul_Id = C_Modul_Id;
            data.DbSchema = DbSchema;
            data.DbView = DbView;
            data.DbIdField = DbIdField;
            data.DbListField = DbListField;
            data.DbDeletedField = DbDeletedField;
        }
    }
    #endregion
}
