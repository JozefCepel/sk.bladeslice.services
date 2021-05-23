using ServiceStack;
using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Dto;
using WebEas.Esam.ServiceModel.Office.Types.Reg;

namespace WebEas.Esam.ServiceModel.Office.Reg.Dto
{
    // Create
    [Route("/CreateBiznisEntita_Zaloha", "POST")]
    [Api("BiznisEntita_Zaloha")]
    [DataContract]
    public class CreateBiznisEntita_Zaloha : BiznisEntita_ZalohaDto, IReturn<BiznisEntita_ZalohaView> { }

    // Update
    [Route("/UpdateBiznisEntita_Zaloha", "PUT")]
    [Api("BiznisEntita_Zaloha")]
    [DataContract]
    public class UpdateBiznisEntita_Zaloha : BiznisEntita_ZalohaDto, IReturn<BiznisEntita_ZalohaView>
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public long D_BiznisEntita_Zaloha_Id { get; set; }
    }

    // Delete
    [Route("/DeleteBiznisEntita_Zaloha", "DELETE")]
    [Api("BiznisEntita_Zaloha")]
    [DataContract]
    public class DeleteBiznisEntita_Zaloha
    {
        [DataMember(IsRequired = true)]
        public long[] D_BiznisEntita_Zaloha_Id { get; set; }
    }
}
