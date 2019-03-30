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
    [Route("/CreateK_SKL_2", "POST")]
    [Api("K_SKL_2")]
    [DataContract]
    public class CreateK_SKL_2 : K_SKL_2Dto { }

    // Update
    [WebEasRequiresAnyRole(RolesDefinition.Bds.Roles.BdsWriter)]
    [Route("/UpdateK_SKL_2", "PUT")]
    [Api("K_SKL_2")]
    [DataContract]
    public class UpdateK_SKL_2 : K_SKL_2Dto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public int K_SKL_2 { get; set; }
    }

    // Delete
    [WebEasRequiresAnyRole(RolesDefinition.Bds.Roles.BdsWriter)]
    [Route("/DeleteK_SKL_2", "DELETE")]
    [Api("K_SKL_2")]
    [DataContract]
    public class DeleteK_SKL_2
    {
        [DataMember(IsRequired = true)]
        public int K_SKL_2 { get; set; }
    }

    #region DTO
    [DataContract]
    public class K_SKL_2Dto : BaseDto<tblK_SKL_2>
    {
        [DataMember]
        public int K_SKL_0 { get; set; }

        [DataMember]
        public string LOCATION { get; set; }

        [DataMember]
        public string POPIS { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(tblK_SKL_2 data)
        {
            data.K_SKL_0 = K_SKL_0;
            data.LOCATION = LOCATION;
            data.POPIS = POPIS;
        }
    }
    #endregion
}