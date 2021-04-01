using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Reg.Types;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Reg.Dto
{
    // Create
    [Route("/CreateTextacia", "POST")]
    [Api("Textacia")]
    [DataContract]
    public class CreateTextacia : TextaciaDto, IReturn<TextaciaView> { }

    // Update
    [Route("/UpdateTextacia", "PUT")]
    [Api("Textacia")]
    [DataContract]
    public class UpdateTextacia : TextaciaDto, IReturn<TextaciaView>
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public int C_Textacia_Id { get; set; }
    }

    // Delete
    [Route("/DeleteTextacia", "DELETE")]
    [Api("Textacia")]
    [DataContract]
    public class DeleteTextacia
    {
        [DataMember(IsRequired = true)]
        public int[] C_Textacia_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class TextaciaDto : BaseDto<Textacia>
    {
        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        [DataMember]
        public string Popis { get; set; }

        [DataMember]
        public short? C_TypBiznisEntity_Id { get; set; }

        [DataMember]
        public int? C_TypBiznisEntity_Kniha_Id { get; set; }

        [DataMember]
        public long? C_Druh_Id { get; set; }

        [DataMember]
        public short RokOd { get; set; }

        [DataMember]
        public short? RokDo { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(Textacia data)
        {
            data.Kod = Kod;
            data.Nazov = Nazov;
            data.Popis = Popis;
            data.C_TypBiznisEntity_Id = C_TypBiznisEntity_Id;
            data.C_TypBiznisEntity_Kniha_Id = C_TypBiznisEntity_Kniha_Id;
            data.C_Druh_Id = C_Druh_Id;
            data.RokOd = RokOd;
            data.RokDo = RokDo;
        }
    }
    #endregion
}
