using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Cfe.Types;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Dto
{
    // Create ors
    [WebEasRequiresAnyRole(RolesDefinition.Cfe.Roles.CfeWriter)]
    [Route("/CreateOrsElementType", "POST")]
    [Api("OrsElementType")]
    [DataContract]
    public class CreateOrsElementType : OrsElementTypeDto { }

    // Update
    [WebEasRequiresAnyRole(RolesDefinition.Cfe.Roles.CfeWriter)]
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
    [WebEasRequiresAnyRole(RolesDefinition.Cfe.Roles.CfeWriter)]
    [Route("/DeleteOrsElementType", "DELETE")]
    [Api("OrsElementType")]
    [DataContract]
    public class DeleteOrsElementType
    {
        [DataMember(IsRequired = true)]
        public long C_OrsElementType_Id { get; set; }
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
