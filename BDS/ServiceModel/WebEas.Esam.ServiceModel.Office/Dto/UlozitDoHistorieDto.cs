using ServiceStack;
using System.Runtime.Serialization;

namespace WebEas.Esam.ServiceModel.Office.Dto
{
    [Route("/SaveToHistory", "POST")]
    [Api("Vyk")]
    [DataContract]
    public class UlozitDoHistorieDto
    {
        [DataMember(IsRequired = true)]
        public string KodPolozky { get; set; }

        [DataMember(IsRequired = true)]
        public short Rok { get; set; }

        [DataMember(IsRequired = true)]
        public short Obdobie { get; set; }

        [DataMember]
        public string Popis { get; set; }

        [DataMember]
        public int C_VykazDruh_Id { get; set; }  // Id typu reportu, ked vieme zadame tento urychlime zbytocne hladanie KodPolozky cez DB

        [DataMember]
        public bool RISSAM { get; set; } // nastav priznak exportovane
    }
}