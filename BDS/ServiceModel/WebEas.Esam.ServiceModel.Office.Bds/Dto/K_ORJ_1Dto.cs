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
    [Route("/CreateK_ORJ_1", "POST")]
    [Api("K_ORJ_1")]
    [DataContract]
    public class CreateK_ORJ_1 : K_ORJ_1Dto { }

    // Update
    [WebEasRequiresAnyRole(RolesDefinition.Bds.Roles.BdsWriter)]
    [Route("/UpdateK_ORJ_1", "PUT")]
    [Api("K_ORJ_1")]
    [DataContract]
    public class UpdateK_ORJ_1 : K_ORJ_1Dto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public int K_ORJ_1 { get; set; }
    }

    // Delete
    [WebEasRequiresAnyRole(RolesDefinition.Bds.Roles.BdsWriter)]
    [Route("/DeleteK_ORJ_1", "DELETE")]
    [Api("K_ORJ_1")]
    [DataContract]
    public class DeleteK_ORJ_1
    {
        [DataMember(IsRequired = true)]
        public int K_ORJ_1 { get; set; }
    }

    #region DTO
    [DataContract]
    public class K_ORJ_1Dto : BaseDto<tblK_ORJ_1>
    {
        [DataMember]
        public int K_ORJ_0 { get; set; }

        [DataMember]
        public int K_SKL_0 { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(tblK_ORJ_1 data)
        {
            data.K_ORJ_0 = K_ORJ_0;
            data.K_SKL_0 = K_SKL_0;
        }
    }
    #endregion
}