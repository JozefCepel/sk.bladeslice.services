using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Types.Reg;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Reg.Dto
{
    // Create
    [Route("/CreatePokladnica", "POST")]
    [Api("Pokladnica")]
    [DataContract]
    public class CreatePokladnica : PokladnicaDto, IReturn<PokladnicaView> { }

    // Update
    [Route("/UpdatePokladnica", "PUT")]
    [Api("Pokladnica")]
    [DataContract]
    public class UpdatePokladnica : PokladnicaDto, IReturn<PokladnicaView>
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public int C_Pokladnica_Id { get; set; }
    }

    // Delete
    [Route("/DeletePokladnica", "DELETE")]
    [Api("Pokladnica")]
    [DataContract]
    public class DeletePokladnica
    {
        [DataMember(IsRequired = true)]
        public int[] C_Pokladnica_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class PokladnicaDto : BaseDto<Pokladnica>
    {
        [DataMember]
        public string C_Pokladnica_Id_Externe { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        public byte Poradie { get; set; }

        [DataMember]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        public DateTime? PlatnostDo { get; set; }

        [DataMember]
        public short C_Mena_Id { get; set; }

        [DataMember]
        public Guid? D_User_Id_Podpisal { get; set; }

        [DataMember]
        public byte Terminalova { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(Pokladnica data)
        {
            data.C_Pokladnica_Id_Externe = C_Pokladnica_Id_Externe;
            data.Nazov = Nazov;
            data.Kod = Kod;
            data.Poradie = Poradie;
            data.PlatnostOd = PlatnostOd;
            data.PlatnostDo = PlatnostDo;
            data.C_Mena_Id = C_Mena_Id;
            data.D_User_Id_Podpisal = D_User_Id_Podpisal;
            data.Terminalova = Terminalova;
        }
    }
    #endregion
}
