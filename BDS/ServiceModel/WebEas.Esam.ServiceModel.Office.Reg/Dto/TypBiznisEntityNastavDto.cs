using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Reg.Types;
using WebEas.Esam.ServiceModel.Office.Types.Reg;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Reg.Dto
{
    // Create
    //[Route("/CreateTypBiznisEntityNastav", "POST")]
    //[Api("CreateTypBiznisEntityNastav")]
    //[DataContract]
    //public class CreateTypBiznisEntityNastav : TypBiznisEntityNastavDto { }

    // Update
    [Route("/UpdateTypBiznisEntityNastav", "PUT")]
    [Api("UpdateTypBiznisEntityNastav")]
    [DataContract]
    public class UpdateTypBiznisEntityNastav : TypBiznisEntityNastavDto, IReturn<TypBiznisEntityNastavView>
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public long D_TypBiznisEntityNastav_Id { get; set; }
    }

    [Route("/RefreshDefault", "POST")]
    [Api("TypBiznisEntityNastav")]
    [DataContract]
    public class RefreshDefault
    {
        [DataMember(IsRequired = true)]
        public long[] IDs { get; set; }
    }

    [Route("/GetTypBiznisEntityNastavView", "GET")]
    [Api("TypBiznisEntityNastav")]
    public class GetTypBiznisEntityNastavViewDto
    {
        public short? C_TypBiznisEntity_Id { get; set; }
    }

    // Delete
    //[Route("/DeleteTypBiznisEntityNastav", "DELETE")]
    //[Api("DeleteTypBiznisEntityNastav")]
    //[DataContract]
    //public class DeleteTypBiznisEntityNastav
    //{
    //    [DataMember(IsRequired = true)]
    //    public long D_TypBiznisEntityNastav_Id { get; set; }
    //}

    #region DTO
    [DataContract]
    public class TypBiznisEntityNastavDto : BaseDto<TypBiznisEntityNastav>
    {
        [DataMember]
        public short C_TypBiznisEntity_Id { get; set; }

        [DataMember]
        public bool? StrediskoNaPolozke { get; set; }

        [DataMember]
        public bool? ProjektNaPolozke { get; set; }

        [DataMember]
        public bool? UctKluc1NaPolozke { get; set; }

        [DataMember]
        public bool? UctKluc2NaPolozke { get; set; }

        [DataMember]
        public bool? UctKluc3NaPolozke { get; set; }

        [DataMember]
        public bool? EvidenciaDMS { get; set; }

        [DataMember]
        public bool? EvidenciaSystem { get; set; }

        [DataMember]
        public bool? CislovanieJedno { get; set; }

        [DataMember]
        public string DatumDokladuTU { get; set; }

        [DataMember]
        public string DatumDokladuEU { get; set; }

        [DataMember]
        public string DatumDokladuDV { get; set; }

        [DataMember]
        public bool? UctovatPolozkovite { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(TypBiznisEntityNastav data)
        {
            data.C_TypBiznisEntity_Id = C_TypBiznisEntity_Id;
            data.StrediskoNaPolozke = StrediskoNaPolozke;
            data.ProjektNaPolozke = ProjektNaPolozke;
            data.UctKluc1NaPolozke = UctKluc1NaPolozke;
            data.UctKluc2NaPolozke = UctKluc2NaPolozke;
            data.UctKluc3NaPolozke = UctKluc3NaPolozke;
            data.EvidenciaDMS = EvidenciaDMS;
            data.EvidenciaSystem = EvidenciaSystem;
            data.CislovanieJedno = CislovanieJedno;
            //data.DatumDokladuTU = DatumDokladuTU;
            //data.DatumDokladuEU = DatumDokladuEU;
            //data.DatumDokladuDV = DatumDokladuDV;
            data.UctovatPolozkovite = UctovatPolozkovite;
        }
    }
    #endregion
}
