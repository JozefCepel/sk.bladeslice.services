using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Bds.Types;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Dto
{
    // Update
    [WebEasRequiresAnyRole(RolesDefinition.Bds.Roles.BdsWriter)]
    [Route("/UpdateK_OWN_0", "PUT")]
    [Api("K_OWN_0")]
    [DataContract]
    public class UpdateK_OWN_0 : K_OWN_0Dto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public int K_OWN_0 { get; set; }
    }

    #region DTO
    [DataContract]
    public class K_OWN_0Dto : BaseDto<tblK_OWN_0>
    {
        [DataMember]
        public string ICO { get; set; }

        [DataMember]
        public string DRC { get; set; }

        [DataMember]
        public string NAZOV1 { get; set; }

        [DataMember]
        public string NAZOV2 { get; set; }

        [DataMember]
        public string ULICA_S { get; set; }

        [DataMember]
        public string PSC_S { get; set; }

        [DataMember]
        public string OBEC_S { get; set; }

        [DataMember]
        public string NAZOV1_P { get; set; }

        [DataMember]
        public string NAZOV2_P { get; set; }

        [DataMember]
        public string ULICA_P { get; set; }

        [DataMember]
        public string PSC_P { get; set; }

        [DataMember]
        public string OBEC_P { get; set; }

        [DataMember]
        public string STAT { get; set; }

        [DataMember]
        public string ROK { get; set; }

        [DataMember]
        public string TEL1 { get; set; }

        [DataMember]
        public string TEL2 { get; set; }

        [DataMember]
        public string MOBIL { get; set; }

        [DataMember]
        public string FAX { get; set; }

        [DataMember]
        public string EMAIL { get; set; }

        [DataMember]
        public string WWW { get; set; }

        [DataMember]
        public bool? PLATIC_DPH { get; set; }

        [DataMember]
        public bool? MES_DPH { get; set; }

        [DataMember]
        public bool? FIFO { get; set; }

        [DataMember]
        public string REGISTER1 { get; set; }

        [DataMember]
        public string REGISTER2 { get; set; }

        [DataMember]
        public string IC_DPH { get; set; }

        [DataMember]
        public decimal? INDEX_DPH { get; set; }

        [DataMember]
        public byte? START_UCT { get; set; }

        [DataMember]
        public bool? SKL_SO_UP { get; set; }

        [DataMember]
        public bool? DPH_UCT_DIFF_UO { get; set; }

        [DataMember]
        public bool? VRB_ONLINE { get; set; }

        [DataMember]
        public bool? SKL_CHRONOL { get; set; }

        [DataMember]
        public string STAT_P { get; set; }

        [DataMember]
        public bool? VRB_DIRECT { get; set; }

        [DataMember]
        public string KONCERN_NO { get; set; }

        [DataMember]
        public string SK_NACE { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(tblK_OWN_0 data)
        {
            data.ICO = ICO;
            data.DRC = DRC;
            data.NAZOV1 = NAZOV1;
            data.NAZOV2 = NAZOV2;
            data.ULICA_S = ULICA_S;
            data.PSC_S = PSC_S;
            data.OBEC_S = OBEC_S;
            data.NAZOV1_P = NAZOV1_P;
            data.NAZOV2_P = NAZOV2_P;
            data.ULICA_P = ULICA_P;
            data.PSC_P = PSC_P;
            data.OBEC_P = OBEC_P;
            data.STAT = STAT;
            data.ROK = ROK;
            data.TEL1 = TEL1;
            data.TEL2 = TEL2;
            data.MOBIL = MOBIL;
            data.FAX = FAX;
            data.EMAIL = EMAIL;
            data.WWW = WWW;
            data.PLATIC_DPH = PLATIC_DPH;
            data.MES_DPH = MES_DPH;
            data.FIFO = FIFO;
            data.REGISTER1 = REGISTER1;
            data.REGISTER2 = REGISTER2;
            data.IC_DPH = IC_DPH;
            data.INDEX_DPH = INDEX_DPH;
            data.START_UCT = START_UCT;
            data.SKL_SO_UP = SKL_SO_UP;
            data.DPH_UCT_DIFF_UO = DPH_UCT_DIFF_UO;
            data.VRB_ONLINE = VRB_ONLINE;
            data.SKL_CHRONOL = SKL_CHRONOL;
            data.STAT_P = STAT_P;
            data.VRB_DIRECT = VRB_DIRECT;
            data.KONCERN_NO = string.IsNullOrEmpty(KONCERN_NO) ? "" : KONCERN_NO;
            data.SK_NACE = string.IsNullOrEmpty(SK_NACE) ? "" : SK_NACE;
        }
    }
    #endregion
}