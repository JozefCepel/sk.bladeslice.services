using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Types.Reg;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Reg.Dto
{
    // Create
    [Route("/CreateBanka", "POST")]
    [Api("Banka")]
    [DataContract]
    public class CreateBanka : BankaDto, IReturn<BankaView> { }

    // Update
    [Route("/UpdateBanka", "PUT")]
    [Api("Banka")]
    [DataContract]
    public class UpdateBanka : BankaDto, IReturn<BankaView>
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public short C_Banka_Id { get; set; }
    }

    // Delete
    [Route("/DeleteBanka", "DELETE")]
    [Api("Banka")]
    [DataContract]
    public class DeleteBanka
    {
        [DataMember(IsRequired = true)]
        public short[] C_Banka_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class BankaDto : BaseDto<Banka>
    {
        [DataMember]
        public short C_Stat_Id { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        public string BIC { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(Banka data)
        {
            data.C_Stat_Id = C_Stat_Id;
            data.Nazov = Nazov;
            data.Kod = Kod;
            data.BIC = BIC;
        }
    }
    #endregion
}
