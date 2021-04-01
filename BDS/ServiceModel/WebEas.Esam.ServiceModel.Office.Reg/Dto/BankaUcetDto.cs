using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Types.Reg;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Reg.Dto
{
    // Create
    [Route("/CreateBankaUcet", "POST")]
    [Api("BankaUcet")]
    [DataContract]
    public class CreateBankaUcet : BankaUcetDto, IReturn<BankaUcetView> { }

    // Update
    [Route("/UpdateBankaUcet", "PUT")]
    [Api("BankaUcet")]
    [DataContract]
    public class UpdateBankaUcet : BankaUcetDto, IReturn<BankaUcetView>
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public int C_BankaUcet_Id { get; set; }
    }

    // Delete
    [Route("/DeleteBankaUcet", "DELETE")]
    [Api("BankaUcet")]
    [DataContract]
    public class DeleteBankaUcet
    {
        [DataMember(IsRequired = true)]
        public int[] C_BankaUcet_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class BankaUcetDto : BaseDto<BankaUcetCis>
    {
        [DataMember]
        public string C_BankaUcet_Id_Externe { get; set; }

        [DataMember]
        [Required]
        public string Nazov { get; set; }

        [DataMember]
        [Required]
        public string Kod { get; set; }

        [DataMember]
        [Required]
        public string IBAN { get; set; }

        [DataMember]
        [Required]
        public string BIC { get; set; }

        [DataMember]
        [Required]
        public byte Poradie { get; set; }

        [DataMember]
        [NotEmptyOrDefault]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        public DateTime? PlatnostDo { get; set; }

        [DataMember]
        [Required]
        public short C_Mena_Id { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(BankaUcetCis data)
        {
            data.Nazov = Nazov;
            data.Kod = Kod;
            data.IBAN = IBAN;
            data.BIC = BIC;
            data.Poradie = Poradie;
            data.PlatnostOd = PlatnostOd;
            data.PlatnostDo = PlatnostDo;
            data.C_Mena_Id = C_Mena_Id;
        }
    }
    #endregion
}
