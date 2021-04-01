using ServiceStack;
using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Types.Reg;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Reg.Dto
{
    // Create
    [Route("/CreateBiznisEntita_Parovanie", "POST")]
    [Api("BiznisEntita_Parovanie")]
    [DataContract]
    public class CreateBiznisEntita_Parovanie : BiznisEntita_ParovanieDto, IReturn<BiznisEntita_ParovanieView> { }

    // Update
    [Route("/UpdateBiznisEntita_Parovanie", "PUT")]
    [Api("BiznisEntita_Parovanie")]
    [DataContract]
    public class UpdateBiznisEntita_Parovanie : BiznisEntita_ParovanieDto, IReturn<BiznisEntita_ParovanieView>
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public long D_BiznisEntita_Parovanie_Id { get; set; }
    }

    // Delete
    [Route("/DeleteBiznisEntita_Parovanie", "DELETE")]
    [Api("BiznisEntita_Parovanie")]
    [DataContract]
    public class DeleteBiznisEntita_Parovanie
    {
        [DataMember(IsRequired = true)]
        public long[] D_BiznisEntita_Parovanie_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class BiznisEntita_ParovanieDto : BaseDto<BiznisEntita_Parovanie>
    {
        [DataMember]
        public int C_TypBiznisEntity_ParovanieDef_Id { get; set; }

        [DataMember]
        public long D_BiznisEntita_Id_1 { get; set; }

        [DataMember]
        public long D_BiznisEntita_Id_2 { get; set; }

        [DataMember]
        public decimal Plnenie { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(BiznisEntita_Parovanie data)
            {
            data.C_TypBiznisEntity_ParovanieDef_Id = C_TypBiznisEntity_ParovanieDef_Id;
            data.D_BiznisEntita_Id_1 = D_BiznisEntita_Id_1;
            data.D_BiznisEntita_Id_2 = D_BiznisEntita_Id_2;
            // data.Identifikator = Identifikator;
            data.Plnenie = Plnenie;
        }
    }
    #endregion
}
