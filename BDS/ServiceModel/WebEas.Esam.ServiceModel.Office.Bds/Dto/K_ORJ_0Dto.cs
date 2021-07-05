using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Bds.Types;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Dto
{
    // Create
    [Route("/CreateK_ORJ_0", "POST")]
    [Api("K_ORJ_0")]
    [DataContract]
    public class CreateK_ORJ_0 : K_ORJ_0Dto { }

    // Update
    [Route("/UpdateK_ORJ_0", "PUT")]
    [Api("K_ORJ_0")]
    [DataContract]
    public class UpdateK_ORJ_0 : K_ORJ_0Dto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public int K_ORJ_0 { get; set; }
    }

    // Delete
    [Route("/DeleteK_ORJ_0", "DELETE")]
    [Api("K_ORJ_0")]
    [DataContract]
    public class DeleteK_ORJ_0
    {
        [DataMember(IsRequired = true)]
        public int[] K_ORJ_0 { get; set; }
    }

    #region DTO
    [DataContract]
    public class K_ORJ_0Dto : BaseDto<tblK_ORJ_0>
    {
        [DataMember]
        public string ORJ { get; set; }

        [DataMember]
        public string POZN { get; set; }

        [DataMember]
        public string KOD { get; set; }

        [DataMember]
        public int Serial_No { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(tblK_ORJ_0 data)
        {
            data.ORJ = ORJ;
            data.POZN = string.IsNullOrEmpty(POZN) ? "" : POZN;
            data.KOD = KOD;
            data.Serial_No = Serial_No;
        }
    }
    #endregion
}