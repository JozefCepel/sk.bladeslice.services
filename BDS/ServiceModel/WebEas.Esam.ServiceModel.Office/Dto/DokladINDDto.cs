using ServiceStack;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Types.Uct;
using WebEas.ServiceModel.Office.Egov.Reg.Types;

namespace WebEas.Esam.ServiceModel.Office.Dto
{
    // Create
    [Route("/CreateDokladIND", "POST")]
    [Api("DokladIND")]
    [DataContract]
    public class CreateDokladINDDto : DokladDto, IReturn<DokladINDView>
    {
        public new long D_BiznisEntita_Id { get; set; }

        public override short C_TypBiznisEntity_Id => (short)TypBiznisEntityEnum.IND;
    }

    // Update
    [Route("/UpdateDokladIND", "PUT")]
    [Api("DokladIND")]
    [DataContract]
    public class UpdateDokladINDDto : DokladINDDto, IReturn<DokladINDView> { }

    // Delete
    [Route("/DeleteDokladIND", "DELETE")]
    [Api("DokladIND")]
    [DataContract]
    public class DeleteDokladINDDto
    {
        [DataMember(IsRequired = true)]
        [NotEmptyOrDefault]
        public long[] D_BiznisEntita_Id { get; set; }
    }

    [DataContract]
    public class DokladINDDto : DokladDto
    {
    }
}