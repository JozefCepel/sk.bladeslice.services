using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Types.Reg;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Reg.Dto
{
    // Create
    [Route("/CreateStredisko", "POST")]
    [Api("Stredisko")]
    [DataContract]
    public class CreateStredisko : StrediskoDto, IReturn<StrediskoView> { }

    // Update
    [Route("/UpdateStredisko", "PUT")]
    [Api("Stredisko")]
    [DataContract]
    public class UpdateStredisko : StrediskoDto, IReturn<StrediskoResult>
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public int C_Stredisko_Id { get; set; }
    }

    // Delete
    [Route("/DeleteStredisko", "DELETE")]
    [Api("Stredisko")]
    [DataContract]
    public class DeleteStredisko : IReturn<StrediskoResult>
    {
        [DataMember(IsRequired = true)]
        public int[] C_Stredisko_Id { get; set; }
    }


    [Route("/GetListStredisko", "GET")]
    [Api("Stredisko")]
    [DataContract]
    public class GetListStredisko : IReturn<List<StrediskoView>>
    {
    }

    [DataContract]
    public class StrediskoResult
    {
        [DataMember]
        public StrediskoView Record { get; set; }

        [DataMember]
        public string DcomError { get; set; }
    }


    #region DTO
    [DataContract]
    public class StrediskoDto : BaseDto<StrediskoCis>
    {
        [DataMember]
        public int? C_Stredisko_Id_Parent { get; set; }

        [DataMember]
        public string C_Stredisko_Id_Externe { get; set; }

        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        public short Poradie { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        [DataMember]
        public string Popis { get; set; }

        [DataMember]
        public bool PodnCinn { get; set; }

        [DataMember]
        [NotEmptyOrDefault]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        public DateTime? PlatnostDo { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(StrediskoCis data)
        {
            data.C_Stredisko_Id_Parent = C_Stredisko_Id_Parent;
            data.C_Stredisko_Id_Externe = C_Stredisko_Id_Externe;
            data.Kod = Kod;
            data.Poradie = Poradie;
            data.Nazov = Nazov;
            data.Popis = Popis;
            data.PodnCinn = PodnCinn;
            data.PlatnostOd = PlatnostOd;
            data.PlatnostDo = PlatnostDo;
        }
    }
    #endregion
}