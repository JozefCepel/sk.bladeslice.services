using ServiceStack;
using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Types.Reg;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Reg.Dto
{
    // Create
    [Route("/CreateTypBiznisEntity_ParovanieDef", "POST")]
    [Api("TypBiznisEntity_ParovanieDef")]
    [DataContract]
    public class CreateTypBiznisEntity_ParovanieDef : TypBiznisEntity_ParovanieDefDto, IReturn<TypBiznisEntity_ParovanieDefView> { }

    // Update
    [Route("/UpdateTypBiznisEntity_ParovanieDef", "PUT")]
    [Api("TypBiznisEntity_ParovanieDef")]
    [DataContract]
    public class UpdateTypBiznisEntity_ParovanieDef : TypBiznisEntity_ParovanieDefDto, IReturn<TypBiznisEntity_ParovanieDefView>
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public int C_TypBiznisEntity_ParovanieDef_Id { get; set; }
    }

    // Delete
    [Route("/DeleteTypBiznisEntity_ParovanieDef", "DELETE")]
    [Api("TypBiznisEntity_ParovanieDef")]
    [DataContract]
    public class DeleteTypBiznisEntity_ParovanieDef
    {
        [DataMember(IsRequired = true)]
        public int[] C_TypBiznisEntity_ParovanieDef_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class TypBiznisEntity_ParovanieDefDto : BaseDto<TypBiznisEntity_ParovanieDef>
    {
        [DataMember]
        public short C_TypBiznisEntity_Id_1 { get; set; }

        [DataMember]
        public short C_TypBiznisEntity_Id_2 { get; set; }

        [DataMember]
        public byte ParovanieTyp { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(TypBiznisEntity_ParovanieDef data)
            {
                data.C_TypBiznisEntity_Id_1 = C_TypBiznisEntity_Id_1;
                data.C_TypBiznisEntity_Id_2 = C_TypBiznisEntity_Id_2;
                data.ParovanieTyp = ParovanieTyp;
                data.Nazov = Nazov;
        }
    }
    #endregion
}
