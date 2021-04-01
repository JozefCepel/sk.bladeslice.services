using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Reg.Types;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Reg.Dto
{
    // Create
    [Route("/CreateMenaKurz", "POST")]
    [Api("MenaKurz")]
    [DataContract]
    public class CreateMenaKurz : MenaKurzDto, IReturn<MenaKurzView> { }

    // Update
    [Route("/UpdateMenaKurz", "PUT")]
    [Api("MenaKurz")]
    [DataContract]
    public class UpdateMenaKurz : MenaKurzDto, IReturn<MenaKurzView>
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public long C_MenaKurz_Id { get; set; }
    }

    // Delete
    [Route("/DeleteMenaKurz", "DELETE")]
    [Api("MenaKurz")]
    [DataContract]
    public class DeleteMenaKurz
    {
        [DataMember(IsRequired = true)]
        public long[] C_MenaKurz_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class MenaKurzDto : BaseDto<MenaKurz>
    {
        [DataMember]
        public short C_Mena_Id { get; set; }

        [DataMember]
        public decimal Kurz { get; set; }

        [DataMember]
        public DateTime PlatnostOd { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(MenaKurz data)
        {
            data.C_Mena_Id = C_Mena_Id;
            data.Kurz = Kurz;
            data.PlatnostOd = PlatnostOd;
        }
    }
    #endregion
}
