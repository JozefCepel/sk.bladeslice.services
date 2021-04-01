using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Types.Reg;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Reg.Dto
{
    // Create
    [Route("/CreateTyp", "POST")]
    [Api("Typ")]
    [DataContract]
    public class CreateTyp : TypDto, IReturn<TypView> { }

    // Update
    [Route("/UpdateTyp", "PUT")]
    [Api("Typ")]
    [DataContract]
    public class UpdateTyp : TypDto, IReturn<TypView>
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public int C_Typ_Id { get; set; }
    }

    // Delete
    [Route("/DeleteTyp", "DELETE")]
    [Api("Typ")]
    [DataContract]
    public class DeleteTyp
    {
        [DataMember(IsRequired = true)]
        public int[] C_Typ_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class TypDto : BaseDto<Typ>
    {
        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        [DataMember]
        public bool Polozka { get; set; }

        [DataMember]
        public int RzpDefinicia { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(Typ data)
        {
            data.Kod = Kod;
            data.Nazov = Nazov;
            data.Polozka = Polozka;
            data.RzpDefinicia = RzpDefinicia;
        }
    }
    #endregion
}
