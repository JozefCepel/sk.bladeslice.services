using ServiceStack;
using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Types.Reg;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Reg.Dto
{
    // Create
    [Route("/CreateTypBiznisEntity_Kniha", "POST")]
    [Api("TypBiznisEntity_Kniha")]
    [DataContract]
    public class CreateTypBiznisEntity_Kniha : TypBiznisEntity_KnihaDto, IReturn<TypBiznisEntity_KnihaView> { }

    // Update
    [Route("/UpdateTypBiznisEntity_Kniha", "PUT")]
    [Api("TypBiznisEntity_Kniha")]
    [DataContract]
    public class UpdateTypBiznisEntity_Kniha : TypBiznisEntity_KnihaDto, IReturn<TypBiznisEntity_KnihaView>
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public int C_TypBiznisEntity_Kniha_Id { get; set; }
    }

    // Delete
    [Route("/DeleteTypBiznisEntity_Kniha", "DELETE")]
    [Api("TypBiznisEntity_Kniha")]
    [DataContract]
    public class DeleteTypBiznisEntity_Kniha
    {
        [DataMember(IsRequired = true)]
        public int[] C_TypBiznisEntity_Kniha_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class TypBiznisEntity_KnihaDto : BaseDto<TypBiznisEntity_Kniha>
    {
        [DataMember]
        [StringLength(6)]
        public string Kod { get; set; }

        [DataMember]
        public short C_TypBiznisEntity_Id { get; set; }

        [DataMember]
        [StringLength(100)]
        public string Nazov { get; set; }

        [DataMember]
        public byte Poradie { get; set; }

        [DataMember]
        public int? SkupinaPredkont_Id { get; set; }

        [DataMember]
        [StringLength(40)]
        public string SkupinaPredkont_Popis { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(TypBiznisEntity_Kniha data)
        {
            data.Kod = Kod;
            data.C_TypBiznisEntity_Id = C_TypBiznisEntity_Id;
            data.Nazov = Nazov;
            data.Poradie = Poradie;
            data.SkupinaPredkont_Id = SkupinaPredkont_Id;
            data.SkupinaPredkont_Popis = SkupinaPredkont_Popis;
        }
    }
    #endregion
}
