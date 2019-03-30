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
    [Route("/CreateK_MAT_0", "POST")]
    [Api("K_MAT_0")]
    [DataContract]
    public class CreateK_MAT_0 : K_MAT_0Dto { }

    // Update
    [WebEasRequiresAnyRole(RolesDefinition.Bds.Roles.BdsWriter)]
    [Route("/UpdateK_MAT_0", "PUT")]
    [Api("K_MAT_0")]
    [DataContract]
    public class UpdateK_MAT_0 : K_MAT_0Dto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public int K_MAT_0 { get; set; }
    }

    // Delete
    [WebEasRequiresAnyRole(RolesDefinition.Bds.Roles.BdsWriter)]
    [Route("/DeleteK_MAT_0", "DELETE")]
    [Api("K_MAT_0")]
    [DataContract]
    public class DeleteK_MAT_0
    {
        [DataMember(IsRequired = true)]
        public int K_MAT_0 { get; set; }
    }

    #region DTO
    [DataContract]
    public class K_MAT_0Dto : BaseDto<tblK_MAT_0>
    {
        [DataMember]
        public int K_TSK_0 { get; set; }

        [DataMember]
        public string KOD { get; set; }

        [DataMember]
        public string NAZOV { get; set; }

        //[DataMember]
        //public decimal DPH { get; set; }

        [DataMember]
        public string MJ { get; set; }

        [DataMember]
        public string POZN { get; set; }

        [DataMember]
        public decimal N_CENA { get; set; }

        [DataMember]
        public decimal PC1 { get; set; }

        [DataMember]
        public decimal PC2 { get; set; }

        [DataMember]
        public decimal PC3 { get; set; }

        [DataMember]
        public decimal PC4 { get; set; }

        [DataMember]
        public decimal PC5 { get; set; }

        [DataMember]
        public decimal MIN_MN { get; set; }

        [DataMember]
        public decimal MAX_MN { get; set; }

        [DataMember]
        public string EAN { get; set; }

        [DataMember]
        public int WARRANTY { get; set; }

        //[DataMember]
        //public decimal VRB_INE { get; set; }

        [DataMember]
        public decimal WT { get; set; }

        [DataMember]
        public string WT_MJ { get; set; }

        [DataMember]
        public string IST { get; set; }

        [DataMember]
        public string KOD_EXT { get; set; }

        [DataMember]
        public string NAZOV_EXT { get; set; }

        [DataMember]
        public DateTime? VALID_TO { get; set; }

        [DataMember]
        public decimal HUST { get; set; }

        [DataMember]
        public decimal PC6 { get; set; }

        [DataMember]
        public decimal PC7 { get; set; }

        [DataMember]
        public decimal PC8 { get; set; }

        [DataMember]
        public decimal PC9 { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(tblK_MAT_0 data)
        {
            data.K_TSK_0 = K_TSK_0;
            data.KOD = KOD;
            data.NAZOV = NAZOV;
            data.DPH = 0; //DPH;
            data.MJ = string.IsNullOrEmpty(MJ) ? "" : MJ;
            data.POZN = string.IsNullOrEmpty(POZN) ? "" : POZN;
            data.N_CENA = N_CENA;
            data.PC1 = PC1;
            data.PC2 = PC2;
            data.PC3 = PC3;
            data.PC4 = PC4;
            data.PC5 = PC5;
            data.PC6 = PC6;
            data.PC7 = PC7;
            data.PC8 = PC8;
            data.PC9 = PC9;
            data.MIN_MN = MIN_MN;
            data.MAX_MN = MAX_MN;
            data.EAN = EAN;
            data.WARRANTY = WARRANTY;
            data.VRB_INE = 0; // VRB_INE;
            data.WT = WT;
            data.WT_MJ = WT_MJ;
            data.IST = IST;
            data.KOD_EXT = KOD_EXT;
            data.NAZOV_EXT = NAZOV_EXT;
            data.VALID_TO = VALID_TO;
            data.HUST = HUST;
        }
    }
    #endregion
}