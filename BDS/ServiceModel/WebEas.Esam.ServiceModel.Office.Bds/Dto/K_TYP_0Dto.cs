using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Bds.Types;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Dto
{
    // Create
    [Route("/CreateK_TYP_0", "POST")]
    [Api("K_TYP_0")]
    [DataContract]
    public class CreateK_TYP_0 : K_TYP_0Dto { }

    // Update
    [Route("/UpdateK_TYP_0", "PUT")]
    [Api("K_TYP_0")]
    [DataContract]
    public class UpdateK_TYP_0 : K_TYP_0Dto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public int K_TYP_0 { get; set; }
    }

    // Delete
    [Route("/DeleteK_TYP_0", "DELETE")]
    [Api("K_TYP_0")]
    [DataContract]
    public class DeleteK_TYP_0
    {
        [DataMember(IsRequired = true)]
        public int[] K_TYP_0 { get; set; }
    }

    #region DTO
    [DataContract]
    public class K_TYP_0Dto : BaseDto<tblK_TYP_0>
    {
        [DataMember]
        public string NAZOV { get; set; }

        [DataMember]
        public bool? DPH_0 { get; set; }

        [DataMember]
        public bool? DPH_1 { get; set; }

        [DataMember]
        public bool? DPH_2 { get; set; }

        [DataMember]
        public bool? UCT_DKL { get; set; }

        [DataMember]
        public bool? HLASENIE_DPH { get; set; }

        [DataMember]
        public bool? KAT_MAT { get; set; }

        [DataMember]
        public string CUST_NAZOV { get; set; }

        [DataMember]
        public bool IS_HIDDEN { get; set; }

        [DataMember]
        public bool IS_CUST { get; set; }

        [DataMember]
        public int? TYP_RANK { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(tblK_TYP_0 data)
        {
            data.NAZOV = NAZOV;
            data.DPH_0 = DPH_0;
            data.DPH_1 = DPH_1;
            data.DPH_2 = DPH_2;
            data.UCT_DKL = UCT_DKL;
            data.HLASENIE_DPH = HLASENIE_DPH;
            data.KAT_MAT = KAT_MAT;
            data.CUST_NAZOV = CUST_NAZOV;
            data.IS_HIDDEN = IS_HIDDEN;
            data.IS_CUST = IS_CUST;
            data.TYP_RANK = TYP_RANK;
        }
    }
    #endregion
}