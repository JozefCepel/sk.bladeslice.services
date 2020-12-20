using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Cfe.Types;
using WebEas.Esam.ServiceModel.Office.Types.Cfe;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Dto
{
    //// Create
    //[Route("/CreateOrsElement", "POST")]
    //[Api("OrsElement")]
    //[DataContract]
    //public class CreateOrsElement : OrsElementDto { }

    // Update
    [Route("/UpdateOrsElement", "PUT")]
    [Api("OrsElement")]
    [DataContract]
    public class UpdateOrsElement : OrsElementDto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public int C_OrsElement_Id { get; set; }
    }

    //// Delete
    //[Route("/DeleteOrsElement", "DELETE")]
    //[Api("OrsElement")]
    //[DataContract]
    //public class DeleteOrsElement
    //{
    //    [DataMember(IsRequired = true)]
    //    public long C_OrsElement_Id { get; set; }
    //}

    #region DTO
    [DataContract]
    public class OrsElementDto : BaseDto<OrsElement>
    {
        [DataMember]
        public int C_OrsElementType_Id { get; set; }

        [DataMember]
        public int? IdValue { get; set; }

        [DataMember]
        public string ListValue { get; set; }

        [DataMember]
        public bool Deleted { get; set; }

        [DataMember]
        public DateTime? DeletedDate { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(OrsElement data)
        {
            data.C_OrsElementType_Id = C_OrsElementType_Id;
            data.IdValue = IdValue;
            data.ListValue = ListValue;
            data.Deleted = (DeletedDate != null)? ((DeletedDate < DateTime.Now)? true: false) : Deleted;
        }
    }
    #endregion
}
