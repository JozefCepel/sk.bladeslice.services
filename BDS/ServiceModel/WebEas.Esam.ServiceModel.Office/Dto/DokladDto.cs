using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Types.Reg;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Dto
{
    #region DTO
    [DataContract]
    public abstract class DokladDto : BaseDto<BiznisEntita>
    {
        [DataMember(IsRequired = true)]
        [NotEmptyOrDefault]
        public long D_BiznisEntita_Id { get; set; }

        public virtual short C_TypBiznisEntity_Id { get { return default; } }

        //[DataMember]
        //public long? D_BiznisEntita_Id_Externe { get; set; } - neriesime cez FE

        [DataMember]
        public int C_TypBiznisEntity_Kniha_Id { get; set; }

        [DataMember]
        public int? C_Stredisko_Id { get; set; }

        [DataMember]
        public int? C_BankaUcet_Id { get; set; }

        [DataMember]
        public int? C_Pokladnica_Id { get; set; }

        [DataMember]
        public long? C_Projekt_Id { get; set; }

        [DataMember]
        public DateTime DatumDokladu { get; set; }

        [DataMember]
        public DateTime? DatumPrijatia { get; set; }

        [DataMember]
        public DateTime? DatumVystavenia { get; set; }

        [DataMember]
        public DateTime? DatumSplatnosti { get; set; }

        [DataMember]
        public DateTime? DatumDodania { get; set; }

        [DataMember]
        public DateTime? DatumVDP { get; set; }

        [DataMember]
        public string CisloInterne { get; set; }

        [DataMember]
        public string CisloExterne { get; set; }

        [DataMember]
        public string VS { get; set; }

        [DataMember]
        public long? D_Osoba_Id { get; set; }

        [DataMember]
        public decimal DM_CV { get; set; }

        [DataMember]
        public decimal DM_Zak0 { get; set; }

        [DataMember]
        public decimal DM_Zak1 { get; set; }

        [DataMember]
        public decimal DM_Zak2 { get; set; }

        [DataMember]
        public decimal DM_DPH1 { get; set; }

        [DataMember]
        public decimal DM_DPH2 { get; set; }

        [DataMember]
        public decimal CM_CV { get; set; }

        [DataMember]
        public decimal CM_Zak0 { get; set; }

        [DataMember]
        public decimal CM_Zak1 { get; set; }

        [DataMember]
        public decimal CM_Zak2 { get; set; }

        [DataMember]
        public decimal CM_DPH1 { get; set; }

        [DataMember]
        public decimal CM_DPH2 { get; set; }

        [DataMember]
        public bool PS { get; set; }

        [DataMember]
        public byte C_Lokalita_Id { get; set; }

        [DataMember]
        public string Popis { get; set; }

        [DataMember]
        public long? C_Predkontacia_Id { get; set; }

        [DataMember]
        public long? D_OsobaKontakt_Id_Komu { get; set; }

        [DataMember]
        public string OsobaKontaktKomu { get; set; }

        [DataMember]
        public long? D_ADR_Adresa_Id { get; set; }

        [DataMember]
        public int Cislo { get; set; }

        [DataMember]
        public bool T { get; set; }

        /// <summary>
        /// Binds to BiznisEntita.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(BiznisEntita data)
        {
            data.C_Stredisko_Id = C_Stredisko_Id;
            data.C_BankaUcet_Id = C_BankaUcet_Id;
            data.C_Pokladnica_Id = C_Pokladnica_Id;
            data.C_Projekt_Id = C_Projekt_Id;
            data.DatumDokladu = DatumDokladu;
            data.DatumPrijatia = DatumPrijatia;
            data.DatumVystavenia = DatumVystavenia;
            data.DatumSplatnosti = DatumSplatnosti;
            data.DatumDodania = DatumDodania;
            data.DatumVDP = DatumVDP;
            data.Cislo = Cislo;
            data.CisloInterne = CisloInterne;
            data.CisloExterne = CisloExterne;
            data.VS = VS;
            data.D_BiznisEntita_Id = D_BiznisEntita_Id;
            data.C_TypBiznisEntity_Id = C_TypBiznisEntity_Id;
            data.D_Osoba_Id = D_Osoba_Id;
            data.DM_CV = DM_CV;
            data.DM_DPH1 = DM_DPH1;
            data.DM_DPH2 = DM_DPH2;
            data.DM_Zak0 = DM_Zak0;
            data.DM_Zak1 = DM_Zak1;
            data.DM_Zak2 = DM_Zak2;
            data.CM_CV = CM_CV;
            data.CM_Zak0 = CM_Zak0;
            data.CM_Zak1 = CM_Zak1;
            data.CM_Zak2 = CM_Zak2;
            data.CM_DPH1 = CM_DPH1;
            data.CM_DPH2 = CM_DPH2;
            data.C_Lokalita_Id = C_Lokalita_Id;
            data.PS = PS;
            data.C_TypBiznisEntity_Kniha_Id = C_TypBiznisEntity_Kniha_Id;
            data.Popis = Popis;
            data.C_Predkontacia_Id = C_Predkontacia_Id;
            data.D_OsobaKontakt_Id_Komu = D_OsobaKontakt_Id_Komu;
            data.OsobaKontaktKomu = OsobaKontaktKomu;
            data.D_ADR_Adresa_Id = D_ADR_Adresa_Id;
            data.T = T;
        }
    }
    #endregion
}
