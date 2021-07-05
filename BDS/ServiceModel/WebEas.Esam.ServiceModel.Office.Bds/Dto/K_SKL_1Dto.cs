using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Bds.Types;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Dto
{
    // Create
    [Route("/CreateK_SKL_1", "POST")]
    [Api("K_SKL_1")]
    [DataContract]
    public class CreateK_SKL_1 : K_SKL_1Dto { }

    // Update
    [Route("/UpdateK_SKL_1", "PUT")]
    [Api("K_SKL_1")]
    [DataContract]
    public class UpdateK_SKL_1 : K_SKL_1Dto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public int K_SKL_1 { get; set; }
    }

    // Delete
    [Route("/DeleteK_SKL_1", "DELETE")]
    [Api("K_SKL_1")]
    [DataContract]
    public class DeleteK_SKL_1
    {
        [DataMember(IsRequired = true)]
        public int[] K_SKL_1 { get; set; }
    }

    #region DTO
    [DataContract]
    public class K_SKL_1Dto : BaseDto<tblK_SKL_1>
    {
        [DataMember]
        public int K_SKL_0 { get; set; }

        [DataMember]
        public int K_TSK_0 { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(tblK_SKL_1 data)
        {
            data.K_SKL_0 = K_SKL_0;
            data.K_TSK_0 = K_TSK_0;
        }
    }
    #endregion
}