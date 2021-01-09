using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Bds.Types;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Dto
{
    // Create
    [Route("/CreateD_SIM_0", "POST")]
    [Api("D_SIM_0")]
    [DataContract]
    public class CreateD_SIM_0 : D_SIM_0Dto { }

    // Update
    [Route("/UpdateD_SIM_0", "PUT")]
    [Api("D_SIM_0")]
    [DataContract]
    public class UpdateD_SIM_0 : D_SIM_0Dto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public int D_SIM_0 { get; set; }
    }

    // DeleteR
    [Route("/DeleteD_SIM_0", "DELETE")]
    [Api("D_SIM_0")]
    [DataContract]
    public class DeleteD_SIM_0
    {
        [DataMember(IsRequired = true)]
        public int D_SIM_0 { get; set; }
    }

    [Route("/GetSimData", "GET")]
    [Api("D_SIM_0")]
    [DataContract]
    public class GetSimData : D3dGraphic2.D3DSource
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public int D_VYD_1 { get; set; }
    }


    #region DTO
    [DataContract]
    public class D_SIM_0Dto : BaseDto<tblD_SIM_0>
    {
        [DataMember]
        public int? D_PRI_0 { get; set; }

        [DataMember]
        public int? D_PRI_1 { get; set; }

        [DataMember]
        public int? D_VYD_0 { get; set; }

        [DataMember]
        public int? D_VYD_1 { get; set; }

        [DataMember]
        public int? RANK { get; set; }

        [DataMember]
        public byte PV { get; set; }

        [DataMember]
        public string SN { get; set; }

        [DataMember]
        public int A1 { get; set; }

        [DataMember]
        public int A2 { get; set; }

        [DataMember]
        public int B1 { get; set; }

        [DataMember]
        public int B2 { get; set; }

        [DataMember]
        public int D1 { get; set; }

        [DataMember]
        public int D2 { get; set; }

        [DataMember]
        public int L1 { get; set; }

        [DataMember]
        public int L2 { get; set; }

        [DataMember]
        public int POC_KS { get; set; }

        [DataMember]
        public decimal? POC_KS_VYREZ { get; set; }

        [DataMember]
        public decimal? POC_KS_ZVYSOK { get; set; }

        [DataMember]
        public bool ZVYSOK_SPOTREBA { get; set; }

        [DataMember]
        public string POZN { get; set; }

        [DataMember]
        public string SARZA { get; set; }

        [DataMember]
        public string LOCATION { get; set; }

        [DataMember]
        public decimal SKL_CENA { get; set; }

        [DataMember]
        public decimal POC_KS_PLT { get; set; }

        [DataMember]
        public string OUTER_SIZE { get; set; }

        [DataMember]
        public string OUTER_SIZE_FINAL { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(tblD_SIM_0 data)
        {
            data.D_PRI_0 = D_PRI_0;
            data.D_PRI_1 = D_PRI_1;
            data.D_VYD_0 = D_VYD_0;
            data.D_VYD_1 = D_VYD_1;
            data.RANK = RANK;
            data.PV = PV;
            data.SN = SN;
            data.A1 = A1;
            data.A2 = A2;
            data.B1 = B1;
            data.B2 = B2;
            data.D1 = D1;
            data.D2 = D2;
            data.L1 = L1;
            data.L2 = L2;
            data.POC_KS = POC_KS;
            data.POC_KS_VYREZ = POC_KS_VYREZ;
            data.POC_KS_ZVYSOK = POC_KS_ZVYSOK;
            data.ZVYSOK_SPOTREBA = ZVYSOK_SPOTREBA;
            data.POZN = string.IsNullOrEmpty(POZN) ? "" : POZN;
            data.SARZA = SARZA;
            data.LOCATION = LOCATION;
            data.SKL_CENA = SKL_CENA;
            data.POC_KS_PLT = POC_KS_PLT;
            data.OUTER_SIZE = OUTER_SIZE;
            data.OUTER_SIZE_FINAL = OUTER_SIZE_FINAL;
        }
    }
    #endregion
}