using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Bds.Types;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Dto
{
    // Create
    [WebEasRequiresAnyRole(RolesDefinition.Bds.Roles.BdsMember)]
    [Route("/CreateD_PRI_0", "POST")]
    [Api("D_PRI_0")]
    [DataContract]
    public class CreateD_PRI_0 : D_PRI_0Dto { }

    // Update
    [WebEasRequiresAnyRole(RolesDefinition.Bds.Roles.BdsMember)]
    [Route("/UpdateD_PRI_0", "PUT")]
    [Api("D_PRI_0")]
    [DataContract]
    public class UpdateD_PRI_0 : D_PRI_0Dto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public int D_PRI_0 { get; set; }
    }

    // Delete
    [WebEasRequiresAnyRole(RolesDefinition.Bds.Roles.BdsMember)]
    [Route("/DeleteD_PRI_0", "DELETE")]
    [Api("D_PRI_0")]
    [DataContract]
    public class DeleteD_PRI_0
    {
        [DataMember(IsRequired = true)]
        public int D_PRI_0 { get; set; }
    }

    #region DTO
    [DataContract]
    public class D_PRI_0Dto : BaseDto<tblD_PRI_0>
    {
        [DataMember]
        public int? K_OBP_0 { get; set; }

        [DataMember]
        public int K_SKL_0 { get; set; }

        //[DataMember]
        //public int K_MEN_0 { get; set; }

        [DataMember]
        public int K_POH_0 { get; set; }

        [DataMember]
        public int K_ORJ_0 { get; set; }

        //[DataMember]
        //public decimal? KURZ { get; set; }

        [DataMember]
        public bool PS { get; set; }

        [DataMember]
        public string DKL_C { get; set; }

        [DataMember]
        public string DL_C { get; set; }

        [DataMember]
        public bool V { get; set; }

        [DataMember]
        public bool Z { get; set; }

        [DataMember]
        public DateTime DAT_DKL { get; set; }

        [DataMember]
        public string POZN { get; set; }

        [DataMember]
        public decimal Z_SUMA { get; set; }

        [DataMember]
        public decimal D_SUMA { get; set; }

        [DataMember]
        public decimal D_DPH1 { get; set; }

        [DataMember]
        public decimal D_DPH2 { get; set; }

        [DataMember]
        public byte SO { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(tblD_PRI_0 data)
        {
            data.K_OBP_0 = K_OBP_0;
            data.K_SKL_0 = K_SKL_0;
            data.K_MEN_0 = 1; //K_MEN_0;
            data.K_POH_0 = K_POH_0;
            data.K_ORJ_0 = K_ORJ_0;
            data.KURZ = 1;   //KURZ;
            data.PS = PS;
            data.DKL_C = DKL_C;
            data.DL_C = DL_C;
            data.V = V;
            data.Z = Z;
            data.DAT_DKL = DAT_DKL;
            data.POZN = string.IsNullOrEmpty(POZN) ? "" : POZN;
            data.Z_SUMA = Z_SUMA;
            data.D_SUMA = D_SUMA;
            data.D_DPH1 = D_DPH1;
            data.D_DPH2 = D_DPH2;
            data.SO = SO;
        }
    }
    #endregion

    [WebEasRequiresAnyRole(RolesDefinition.Bds.Roles.BdsWriter)]
    [Route("/GetVybavDokladyPri", "POST")]
    [Api("Doklady")]
    [DataContract]
    public class GetVybavDokladyPriReq : IReturn<List<long>>
    {
        [DataMember]
        public long[] D_PRI_0 { get; set; }
    }

    [WebEasRequiresAnyRole(RolesDefinition.Bds.Roles.BdsWriter)]
    [Route("/GetOdvybavDokladyPri", "POST")]
    [Api("Doklady")]
    [DataContract]
    public class GetOdvybavDokladyPriReq : IReturn<List<long>>
    {
        [DataMember]
        public long[] D_PRI_0 { get; set; }
    }
}