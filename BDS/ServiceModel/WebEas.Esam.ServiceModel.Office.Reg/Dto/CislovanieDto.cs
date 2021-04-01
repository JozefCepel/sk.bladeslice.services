using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Types.Reg;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Reg.Dto
{
    // Update
    [Route("/UpdateCislovanie", "PUT")]
    [Api("Cislovanie")]
    [DataContract]
    public class UpdateCislovanie : CislovanieDto, IReturn<CislovanieView>
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public int C_Cislovanie_Id { get; set; }
    }

    // Delete
    [Route("/DeleteCislovanie", "DELETE")]
    [Api("Cislovanie")]
    [DataContract]
    public class DeleteCislovanie
    {
        [DataMember(IsRequired = true)]
        public int[] C_Cislovanie_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class CislovanieDto : BaseDto<CislovanieCis>
    {
        [DataMember]
        public bool CislovanieJedno { get; set; }

        [DataMember]
        public int C_TypBiznisEntity_Kniha_Id { get; set; }

        [DataMember]
        public int? C_Stredisko_Id { get; set; }

        [DataMember]
        public int? C_BankaUcet_Id { get; set; }

        [DataMember]
        public int? C_Pokladnica_Id { get; set; }

        [DataMember]
        public string CisloInterneMask { get; set; }

        [DataMember]
        public string VSMask { get; set; }

        [DataMember]
        public string Popis { get; set; }

        [DataMember]
        public string UT { get; set; }

        [DataMember]
        public string ZT { get; set; }

        [DataMember]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        public DateTime? PlatnostDo { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(CislovanieCis data)
        {
            data.CislovanieJedno = CislovanieJedno;
            data.C_TypBiznisEntity_Kniha_Id = C_TypBiznisEntity_Kniha_Id;
            data.C_Stredisko_Id = C_Stredisko_Id;
            data.C_BankaUcet_Id = C_BankaUcet_Id;
            data.C_Pokladnica_Id = C_Pokladnica_Id;
            data.CisloInterneMask = CisloInterneMask;
            data.VSMask = VSMask;
            data.Popis = Popis;
            data.UT = UT;
            data.ZT = ZT;
            data.PlatnostOd = PlatnostOd;
            data.PlatnostDo = PlatnostDo;
        }
    }
    #endregion
}
