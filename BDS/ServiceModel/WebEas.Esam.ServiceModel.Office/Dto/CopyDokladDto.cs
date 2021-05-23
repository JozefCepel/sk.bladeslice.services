using ServiceStack;
using System.Runtime.Serialization;

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
    }
}
