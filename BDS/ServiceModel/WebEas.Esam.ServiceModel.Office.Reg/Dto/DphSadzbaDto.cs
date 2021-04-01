using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Types.Reg;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Reg.Dto
{
    // Create
    [Route("/CreateDphSadzba", "POST")]
    [Api("DphSadzba")]
    [DataContract]
    public class CreateDphSadzba : DphSadzbaDto, IReturn<DphSadzbaView> { }

    // Update
    [Route("/UpdateDphSadzba", "PUT")]
    [Api("DphSadzba")]
    [DataContract]
    public class UpdateDphSadzba : DphSadzbaDto, IReturn<DphSadzbaView>
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public int C_DphSadzba_Id { get; set; }
    }

    // Delete
    [Route("/DeleteDphSadzba", "DELETE")]
    [Api("DphSadzba")]
    [DataContract]
    public class DeleteDphSadzba
    {
        [DataMember(IsRequired = true)]
        public int[] C_DphSadzba_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class DphSadzbaDto : BaseDto<DphSadzba>
    {
        [DataMember]
        public byte TypId { get; set; }

        [DataMember]
        public byte DPH { get; set; }

        [DataMember]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        public DateTime? PlatnostDo { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(DphSadzba data)
        {
            data.TypId = TypId;
            data.DPH = DPH;
            data.PlatnostOd = PlatnostOd;
            data.PlatnostDo = PlatnostDo;
        }
    }
    #endregion
}
