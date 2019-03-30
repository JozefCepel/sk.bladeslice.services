using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Bds.Types;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Dto
{
    // Create
    [WebEasRequiresAnyRole(RolesDefinition.Bds.Roles.BdsWriter)]
    [Route("/CreateD_PRI_1", "POST")]
    [Api("D_PRI_1")]
    [DataContract]
    public class CreateD_PRI_1 : D_PRI_1Dto { }

    // Update
    [WebEasRequiresAnyRole(RolesDefinition.Bds.Roles.BdsWriter)]
    [Route("/UpdateD_PRI_1", "PUT")]
    [Api("D_PRI_1")]
    [DataContract]
    public class UpdateD_PRI_1 : D_PRI_1Dto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public int D_PRI_1 { get; set; }
    }

    // Delete
    [WebEasRequiresAnyRole(RolesDefinition.Bds.Roles.BdsWriter)]
    [Route("/DeleteD_PRI_1", "DELETE")]
    [Api("D_PRI_1")]
    [DataContract]
    public class DeleteD_PRI_1
    {
        [DataMember(IsRequired = true)]
        public int D_PRI_1 { get; set; }
    }

    #region DTO
    [DataContract]
    public class D_PRI_1Dto : BaseDto<tblD_PRI_1>
    {
        [DataMember]
        public int? D_PRI_0 { get; set; }

        [DataMember]
        public int? K_TSK_0 { get; set; }

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

        [DataMember]
        public string EAN { get; set; }

        [DataMember]
        public int WARRANTY { get; set; }

        [DataMember]
        public string SN { get; set; }

        [DataMember]
        public string LOCATION { get; set; }

        [DataMember]
        public string KOD_EXT { get; set; }

        [DataMember]
        public string NAZOV_EXT { get; set; }

        [DataMember]
        public string SARZA { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(tblD_PRI_1 data)
        {
            data.D_PRI_0 = D_PRI_0;
            data.K_TSK_0 = K_TSK_0;
            data.K_TYP_0 = 2; //K_TYP_0;
            data.KOD = KOD;
            data.NAZOV = string.IsNullOrEmpty(NAZOV) ? "" : NAZOV;
            data.POC_KS = POC_KS;
            data.BAL_KS = POC_KS; //BAL_KS;
            data.MJ = MJ;
            data.D_CENA = D_CENA;
            data.Z_CENA = Z_CENA;
            data.RANK = RANK;
            data.EAN = EAN;
            data.WARRANTY = WARRANTY;
            data.SN = SN;
            data.LOCATION = LOCATION;
            data.KOD_EXT = KOD_EXT;
            data.NAZOV_EXT = NAZOV_EXT;
            data.SARZA = SARZA;
        }
    }
    #endregion
}