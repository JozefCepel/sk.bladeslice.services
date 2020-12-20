using ServiceStack;
using System;
using System.Runtime.Serialization;

namespace WebEas.Esam.ServiceModel.Office.Dto
{
    [DataContract]
    public class MigraciaStavovDto
    {
        [ApiMember(Name = "TypImportu", Description = "Typy importu", DataType = "string")]
        [DataMember]
        public string TypImportu { get; set; }

        [ApiMember(Name = "PolozkyMigracie", Description = "Položky migrácie", DataType = "array")]
        [DataMember]
        public string[] PolozkyMigracie { get; set; }

        [ApiMember(Name = "ObdobieMesiac", Description = "Obdobie mesiac", DataType = "integer", Format = "int32")]
        [DataMember]
        public int ObdobieMesiac { get; set; }

        [ApiMember(Name = "ObdobieRok", Description = "Obdobie rok", DataType = "integer", Format = "int32")]
        [DataMember]
        public int ObdobieRok { get; set; }

        [ApiMember(Name = "DatumDo", Description = "Importovať k dátumu", DataType = "string", Format = "date-time")]
        [DataMember]
        public DateTime? DatumDo { get; set; }

    }
}
