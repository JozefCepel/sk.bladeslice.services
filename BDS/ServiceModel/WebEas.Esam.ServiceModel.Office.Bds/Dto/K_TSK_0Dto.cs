using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Bds.Types;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Dto
{
    // Create
    [WebEasRequiresAnyRole(RolesDefinition.Bds.Roles.BdsMember)]
    [Route("/CreateK_TSK_0", "POST")]
    [Api("K_TSK_0")]
    [DataContract]
    public class CreateK_TSK_0 : K_TSK_0Dto { }

    // Update
    [WebEasRequiredRole(Roles.Admin)]
    [Route("/UpdateK_TSK_0", "PUT")]
    [Api("K_TSK_0")]
    [DataContract]
    public class UpdateK_TSK_0 : K_TSK_0Dto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public long K_TSK_0 { get; set; }
    }

    // Delete
    [WebEasRequiresAnyRole(RolesDefinition.Bds.Roles.BdsMember)]
    [Route("/DeleteK_TSK_0", "DELETE")]
    [Api("K_TSK_0")]
    [DataContract]
    public class DeleteK_TSK_0
    {
        [DataMember(IsRequired = true)]
        public long K_TSK_0 { get; set; }
    }

    #region DTO
    [DataContract]
    public class K_TSK_0Dto : BaseDto<tblK_TSK_0>
    {
        [DataMember]
        public string TSK { get; set; }

        [DataMember]
        public string POZN { get; set; }

        [DataMember]
        public bool? TOVAR { get; set; }

        [DataMember]
        public bool? MATERIAL { get; set; }

        [DataMember]
        public bool? OSIVO { get; set; }

        [DataMember]
        public decimal? PREP_K { get; set; }

        [DataMember]
        public decimal? PREP_M { get; set; }

        [DataMember]
        public bool? BALENIE { get; set; }

        [DataMember]
        public bool? VYROBOK { get; set; }

        [DataMember]
        public decimal? OM1 { get; set; }

        [DataMember]
        public decimal? OM2 { get; set; }

        [DataMember]
        public decimal? OM3 { get; set; }

        [DataMember]
        public decimal? OM4 { get; set; }

        [DataMember]
        public decimal? OM5 { get; set; }

        [DataMember]
        public byte? PREP_M1 { get; set; }

        [DataMember]
        public byte SKL_SIMULATION { get; set; }

        [DataMember]
        public decimal OM6 { get; set; }

        [DataMember]
        public decimal OM7 { get; set; }

        [DataMember]
        public decimal OM8 { get; set; }

        [DataMember]
        public decimal OM9 { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(tblK_TSK_0 data)
        {
            data.TSK = TSK;
            data.POZN = POZN;
            data.TOVAR = TOVAR;
            data.MATERIAL = MATERIAL;
            data.OSIVO = OSIVO;
            data.PREP_K = PREP_K;
            data.PREP_M = PREP_M;
            data.BALENIE = BALENIE;
            data.VYROBOK = VYROBOK;
            data.OM1 = OM1;
            data.OM2 = OM2;
            data.OM3 = OM3;
            data.OM4 = OM4;
            data.OM5 = OM5;
            data.PREP_M1 = PREP_M1;
            data.SKL_SIMULATION = SKL_SIMULATION;
            data.OM6 = OM6;
            data.OM7 = OM7;
            data.OM8 = OM8;
            data.OM9 = OM9;
        }
    }
    #endregion
}