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
    [Route("/CreateK_OBP_0", "POST")]
    [Api("K_OBP_0")]
    [DataContract]
    public class CreateK_OBP_0 : K_OBP_0Dto { }

    // Update
    [WebEasRequiresAnyRole(RolesDefinition.Bds.Roles.BdsMember)]
    [Route("/UpdateK_OBP_0", "PUT")]
    [Api("K_OBP_0")]
    [DataContract]
    public class UpdateK_OBP_0 : K_OBP_0Dto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public int K_OBP_0 { get; set; }
    }

    // Delete
    [WebEasRequiresAnyRole(RolesDefinition.Bds.Roles.BdsMember)]
    [Route("/DeleteK_OBP_0", "DELETE")]
    [Api("K_OBP_0")]
    [DataContract]
    public class DeleteK_OBP_0
    {
        [DataMember(IsRequired = true)]
        public int K_OBP_0 { get; set; }
    }

    #region DTO
    [DataContract]
    public class K_OBP_0Dto : BaseDto<tblK_OBP_0>
    {
        //[DataMember]
        //public int K_TOB_0 { get; set; }

        //[DataMember]
        //public int K_PRF_0 { get; set; }

        //[DataMember]
        //public int K_OPC_0 { get; set; }

        //[DataMember]
        //public int? K_OPK_0 { get; set; }
        
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
        public int? DEALER { get; set; }

        [DataMember]
        public short? SPLAT { get; set; }

        [DataMember]
        public decimal? ZLAVA { get; set; }

        [DataMember]
        public decimal? PENALE { get; set; }

        [DataMember]
        public int KREDIT { get; set; }

        [DataMember]
        public string OSOBA { get; set; }

        [DataMember]
        public string FUNKCIA { get; set; }

        [DataMember]
        public string TEL1 { get; set; }

        [DataMember]
        public string TEL2 { get; set; }

        [DataMember]
        public string FAX { get; set; }

        [DataMember]
        public string MOBIL { get; set; }

        [DataMember]
        public string EMAIL { get; set; }

        [DataMember]
        public string WWW { get; set; }

        [DataMember]
        public string POZN { get; set; }

        [DataMember]
        public string IC_DPH { get; set; }

        [DataMember]
        public string NICKNAME { get; set; }

        [DataMember]
        public string OBP_C { get; set; }

        [DataMember]
        public int OBP_POTENCIAL { get; set; }

        [DataMember]
        public string DOPL_TEXT { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(tblK_OBP_0 data)
        {
            data.K_TOB_0 = 1;  //K_TOB_0;
            data.K_PRF_0 = 1;  //K_PRF_0;
            data.K_OPC_0 = 1;  //K_OPC_0;
            data.K_OPK_0 = 1;  //K_OPK_0;
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
            data.DEALER = DEALER;
            data.SPLAT = SPLAT;
            data.ZLAVA = ZLAVA;
            data.PENALE = PENALE;
            data.KREDIT = KREDIT;
            data.OSOBA = OSOBA;
            data.FUNKCIA = FUNKCIA;
            data.TEL1 = TEL1;
            data.TEL2 = TEL2;
            data.FAX = FAX;
            data.MOBIL = MOBIL;
            data.EMAIL = EMAIL;
            data.WWW = WWW;
            data.POZN = string.IsNullOrEmpty(POZN) ? "" : POZN;
            data.IC_DPH = IC_DPH;
            data.NICKNAME = NICKNAME;
            data.OBP_C = OBP_C;
            data.OBP_POTENCIAL = OBP_POTENCIAL;
            data.DOPL_TEXT = DOPL_TEXT;
        }
    }
    #endregion
}