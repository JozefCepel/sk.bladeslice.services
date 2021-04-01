using ServiceStack;
using System.Runtime.Serialization;
using WebEas.ServiceModel.Dto;

namespace WebEas.Esam.ServiceModel.Office.Reg.Dto
{
    // Update
    [Route("/UpdateNastavenie", "PUT")]
    [Api("Nastavenie")]
    [DataContract]
    public class UpdateNastavenie : UpdateNastavenieBase
    {
    }

    [DataContract]
    public class GetNastavenieBase
    {
        [DataMember(IsRequired = true)]
        public string Modul { get; set; }

        [DataMember(IsRequired = true)]
        public string Kod { get; set; }
    }


    [Route("/GetNastavenieB", "GET")]
    [Api("GetNastavenie")]
    [DataContract]
    public class GetNastavenieB : GetNastavenieBase
    {
    }

    [Route("/GetNastavenieI", "GET")]
    [Api("GetNastavenie")]
    [DataContract]
    public class GetNastavenieI : GetNastavenieBase
    {
    }

    [Route("/GetNastavenieS", "GET")]
    [Api("GetNastavenie")]
    [DataContract]
    public class GetNastavenieS : GetNastavenieBase
    {
    }

    [Route("/GetNastavenieD", "GET")]
    [Api("GetNastavenie")]
    [DataContract]
    public class GetNastavenieD : GetNastavenieBase
    {
    }
}
