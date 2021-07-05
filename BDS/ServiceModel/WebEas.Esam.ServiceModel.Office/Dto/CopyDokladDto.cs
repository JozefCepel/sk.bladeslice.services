using ServiceStack;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Dto
{
    [Route("/CopyDoklad", "POST")]
    [Api("Doklad")]
    [DataContract]
    public class CopyDokladDto
    {
        [DataMember]
        public long[] D_BiznisEntita_Id { get; set; }

        [DataMember]
        public short? CielTyp { get; set; }

        [DataMember]
        public short? RefLeft { get; set; }

        [DataMember]
        public short? RefRight { get; set; }
    }
}
