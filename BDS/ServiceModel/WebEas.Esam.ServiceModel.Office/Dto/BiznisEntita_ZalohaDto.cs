using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Types.Reg;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Dto
{
    #region DTO
    [DataContract]
    public class BiznisEntita_ZalohaDto : BaseDto<BiznisEntita_Zaloha>
    {
        [DataMember]
        public long D_BiznisEntita_Id_FA { get; set; }

        // virtualny, hodnota z neho sa rozdeluje do D_BiznisEntita_Id_ZF / D_DokladBANPol_Id / D_UhradaParovanie_Id
        [DataMember]
        public long? Zaloha_Id { get; set; }

        [DataMember]
        public long? D_BiznisEntita_Id_ZF { get; set; }

        [DataMember]
        public long? D_DokladBANPol_Id { get; set; }

        [DataMember]
        public long? D_UhradaParovanie_Id { get; set; }

        [DataMember]
        public int C_Typ_Id { get; set; }

        [DataMember]
        public string VS { get; set; }

        [DataMember]
        public decimal DM_Cena { get; set; }

        [DataMember]
        public decimal DM_Rozdiel { get; set; }

        [DataMember]
        public decimal CM_Cena { get; set; }

        [DataMember]
        public decimal Kurz { get; set; }

        [DataMember]
        public DateTime DatumUhrady { get; set; }

        [DataMember]
        public string Popis { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(BiznisEntita_Zaloha data)
        {
            data.D_BiznisEntita_Id_FA = D_BiznisEntita_Id_FA;
            switch (C_Typ_Id)
            {
                case (int)TypEnum.UhradaOZF:
                case (int)TypEnum.UhradaDZF:
                    data.D_BiznisEntita_Id_ZF = D_BiznisEntita_Id_ZF;
                    data.D_DokladBANPol_Id = null;
                    data.D_UhradaParovanie_Id = Zaloha_Id;
                    break;
                case (int)TypEnum.ZalohyPoskytnute:
                case (int)TypEnum.ZalohyPrijate:
                    data.D_BiznisEntita_Id_ZF = null;
                    // aby sme zistili od kadial prislo mame otocene znamienko (nestastne riesenie, ale ohybanie by bolo este zlozitejsie)
                    data.D_DokladBANPol_Id = (Zaloha_Id < 0) ? - Zaloha_Id : null;
                    data.D_UhradaParovanie_Id = (Zaloha_Id > 0) ? Zaloha_Id : null;
                    break;
                default: // Manualne
                    data.D_BiznisEntita_Id_ZF = null;
                    data.D_DokladBANPol_Id = null;
                    data.D_UhradaParovanie_Id = null;
                    break;
            }
            data.C_Typ_Id = C_Typ_Id;
            data.VS = VS;
            data.DM_Cena = DM_Cena;
            data.DM_Rozdiel = DM_Rozdiel;
            data.CM_Cena = CM_Cena;
            data.Kurz = Kurz;
            data.DatumUhrady = DatumUhrady;
            data.Popis = Popis;
            // Rok sa riesi inde
        }
    }
    #endregion
}
