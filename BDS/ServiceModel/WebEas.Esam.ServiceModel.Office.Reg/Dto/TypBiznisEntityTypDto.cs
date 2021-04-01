using ServiceStack;
using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Types.Reg;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Reg.Dto
{
    // Create
    [Route("/CreateTypBiznisEntity_Typ", "POST")]
    [Api("TypBiznisEntity_Typ")]
    [DataContract]
    public class CreateTypBiznisEntityTyp : TypBiznisEntityTypDto, IReturn<TypBiznisEntityTypView> { }

    // Update
    [Route("/UpdateTypBiznisEntity_Typ", "PUT")]
    [Api("TypBiznisEntity_Typ")]
    [DataContract]
    public class UpdateTypBiznisEntityTyp : TypBiznisEntityTypDto, IReturn<TypBiznisEntityTypView>
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public int C_TypBiznisEntity_Typ_Id { get; set; }
    }

    // Delete
    [Route("/DeleteTypBiznisEntity_Typ", "DELETE")]
    [Api("TypBiznisEntity_Typ")]
    [DataContract]
    public class DeleteTypBiznisEntityTyp
    {
        [DataMember(IsRequired = true)]
        public int[] C_TypBiznisEntity_Typ_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class TypBiznisEntityTypDto : BaseDto<TypBiznisEntityTyp>
    {
        [DataMember]
        public int C_Typ_Id { get; set; }

        [DataMember]
        public short C_TypBiznisEntity_Id { get; set; }

        [DataMember]
        public int? C_TypBiznisEntity_Kniha_Id { get; set; }

        [DataMember]
        public short Poradie { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(TypBiznisEntityTyp data)
        {
            data.C_Typ_Id = C_Typ_Id;
            data.C_TypBiznisEntity_Id = C_TypBiznisEntity_Id;
            data.C_TypBiznisEntity_Kniha_Id = C_TypBiznisEntity_Kniha_Id;
            data.Poradie = Poradie;
        }
    }
    #endregion
}
