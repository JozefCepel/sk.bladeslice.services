using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Bds.Types;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Dto
{
    // Create
    [Route("/CreateK_SKL_0", "POST")]
    [Api("K_SKL_0")]
    [DataContract]
    public class CreateK_SKL_0 : K_SKL_0Dto { }

    // Update
    [Route("/UpdateK_SKL_0", "PUT")]
    [Api("K_SKL_0")]
    [DataContract]
    public class UpdateK_SKL_0 : K_SKL_0Dto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public int K_SKL_0 { get; set; }
    }

    // Delete
    [Route("/DeleteK_SKL_0", "DELETE")]
    [Api("K_SKL_0")]
    [DataContract]
    public class DeleteK_SKL_0
    {
        [DataMember(IsRequired = true)]
        public int[] K_SKL_0 { get; set; }
    }

    #region DTO
    [DataContract]
    public class K_SKL_0Dto : BaseDto<tblK_SKL_0>
    {
        [DataMember]
        public string SKL { get; set; }

        [DataMember]
        public string POZN { get; set; }

        //[DataMember]
        //public short? ROZSAH { get; set; }

        [DataMember]
        public byte SO { get; set; }

        [DataMember]
        public string KOD { get; set; }

        [DataMember]
        public string SKL_GRP { get; set; }

        [DataMember]
        public bool SKL_MINUS { get; set; }

        [DataMember]
        public bool? SHOW_VYD_ZERO_SKL { get; set; }

        [DataMember]
        public bool CHECK_IN_SKL_MIN_MAX { get; set; }

        [DataMember]
        public bool CHECK_IN_REZ { get; set; }

        [DataMember]
        public int Serial_No { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(tblK_SKL_0 data)
        {
            data.SKL = SKL;
            data.POZN = string.IsNullOrEmpty(POZN) ? "" : POZN;
            data.ROZSAH = 1; //ROZSAH;
            data.SO = SO;
            data.KOD = KOD;
            data.SKL_GRP = string.IsNullOrEmpty(SKL_GRP) ? "" : SKL_GRP;
            data.SKL_MINUS = SKL_MINUS;
            data.SHOW_VYD_ZERO_SKL = SHOW_VYD_ZERO_SKL;
            data.CHECK_IN_SKL_MIN_MAX = CHECK_IN_SKL_MIN_MAX;
            data.CHECK_IN_REZ = CHECK_IN_REZ;
            data.Serial_No = Serial_No;
        }
    }
    #endregion
}