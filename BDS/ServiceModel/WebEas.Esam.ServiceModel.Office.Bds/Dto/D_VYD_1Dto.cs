using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Bds.Types;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Dto
{
    // Create
    [Route("/CreateD_VYD_1", "POST")]
    [Api("D_VYD_1")]
    [DataContract]
    public class CreateD_VYD_1 : D_VYD_1Dto { }

    // Update
    [Route("/UpdateD_VYD_1", "PUT")]
    [Api("D_VYD_1")]
    [DataContract]
    public class UpdateD_VYD_1 : D_VYD_1Dto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public int D_VYD_1 { get; set; }
    }

    // Delete
    [Route("/DeleteD_VYD_1", "DELETE")]
    [Api("D_VYD_1")]
    [DataContract]
    public class DeleteD_VYD_1
    {
        [DataMember(IsRequired = true)]
        public int D_VYD_1 { get; set; }
    }

    #region DTO
    [DataContract]
    public class D_VYD_1Dto : BaseDto<tblD_VYD_1>
    {
        
        [DataMember]
        public int D_VYD_0 { get; set; }

        [DataMember]
        public int K_TSK_0 { get; set; }

        //[DataMember]
        //public int K_TYP_0 { get; set; }

        [DataMember]
        public string KOD { get; set; }

        [DataMember]
        public string NAZOV { get; set; }

        [DataMember]
        public decimal? POC_KS { get; set; }

        [DataMember]
        public string MJ { get; set; }

        [DataMember]
        public decimal? D_CENA { get; set; }

        [DataMember]
        public decimal? Z_CENA { get; set; }

        [DataMember]
        public int? RANK { get; set; }

        //[DataMember]
        //public decimal? BAL_KS { get; set; }

        //[DataMember]
        //public decimal? BAL_KS1 { get; set; }

        [DataMember]
        public string EAN { get; set; }

        [DataMember]
        public int WARRANTY { get; set; }

        [DataMember]
        public string SN { get; set; }

        [DataMember]
        public string LOCATION { get; set; }

        [DataMember]
        public string LOCATION_DEST { get; set; }

        [DataMember]
        public string KOD_EXT { get; set; }

        [DataMember]
        public string NAZOV_EXT { get; set; }

        [DataMember]
        public string SARZA { get; set; }

        [DataMember]
        public int D3D_A { get; set; }

        [DataMember]
        public int D3D_B { get; set; }

        [DataMember]
        public int D3D_L { get; set; }

        [DataMember]
        public int D3D_D1 { get; set; }

        [DataMember]
        public int D3D_D2 { get; set; }

        [DataMember]
        public int D3D_POC_KS { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(tblD_VYD_1 data)
        {
            data.D_VYD_0 = D_VYD_0;
            data.K_TSK_0 = K_TSK_0;
            data.K_TYP_0 = 2; //K_TYP_0;
            data.KOD = KOD;
            data.NAZOV = string.IsNullOrEmpty(NAZOV) ? "" : NAZOV;
            data.POC_KS = POC_KS ?? 0;
            data.MJ = MJ;
            data.D_CENA = D_CENA ?? 0;
            data.Z_CENA = Z_CENA ?? 0;
            data.RANK = RANK;
            data.BAL_KS = POC_KS ?? 0; //BAL_KS;
            data.BAL_KS1 = 1; // BAL_KS1;
            data.EAN = EAN;
            data.WARRANTY = WARRANTY;
            data.SN = SN;
            data.LOCATION = LOCATION;
            data.LOCATION_DEST = LOCATION_DEST;
            data.KOD_EXT = KOD_EXT;
            data.NAZOV_EXT = NAZOV_EXT;
            data.SARZA = SARZA;
            data.D3D_A = D3D_A;
            data.D3D_B = D3D_B;
            data.D3D_L = D3D_L;
            data.D3D_D1 = D3D_D1;
            data.D3D_D2 = D3D_D2;
            data.D3D_POC_KS = D3D_POC_KS;
        }
    }
    #endregion
}