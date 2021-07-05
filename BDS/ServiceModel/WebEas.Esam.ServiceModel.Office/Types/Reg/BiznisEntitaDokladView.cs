using ServiceStack;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Types.Crm;
using WebEas.Esam.ServiceModel.Office.Types.Osa;
using WebEas.Esam.ServiceModel.Office.Types.Uct;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Office.Egov.Reg.Types;
using WebEas.ServiceModel.Reg.Types;

namespace WebEas.Esam.ServiceModel.Office.Types.Reg
{
    /// <summary>
    /// Base doklad - nepouzivat samostane na select
    /// </summary>
    [DataContract]
    public class BiznisEntitaDokladView : BiznisEntita, IPfeCustomize, IOrsPravo, IBeforeGetList
    {
        [DataMember]
        [PfeColumn(Text = "Typ dokladu kód", Hidden = true, Editable = false)]
        public string TypBiznisEntityKod { get; set; }

        [DataMember]
        [PfeColumn(Text = "Typ dokladu názov", Hidden = true, Editable = false)]
        public string TypBiznisEntityNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kniha", RequiredFields = new[] { nameof(C_TypBiznisEntity_Id) })]
        [PfeCombo(typeof(TypBiznisEntity_KnihaView),
            IdColumn = nameof(C_TypBiznisEntity_Kniha_Id),
            ComboDisplayColumn = nameof(TypBiznisEntity_KnihaView.Kod),
            FilterByOrsPravo = true,
            AdditionalWhereSql = "C_TypBiznisEntity_Id = @C_TypBiznisEntity_Id AND C_TypBiznisEntity_Kniha_Id NOT IN (61,62,63,64)")]
        public string Kniha { get; set; }

        [DataMember]
        [PfeColumn(Text = "Doklad", ReadOnly = true, Xtype = PfeXType.Link)] //LoadWhenVisible = false - musí byť zobrazené kvôli radeniu v gride
        public string BiznisEntitaPopis { get; set; }

        [DataMember]
        [PfeColumn(Text = "_URL", ReadOnly = true)] //Natvrdo v kóde hľadá k PfeXType.Link - field URL
        public string URL { get; set; }

        [DataMember]
        [PfeColumn(Text = "Stav", Editable = false)]
        [PfeCombo(typeof(StavEntityView), IdColumn = nameof(C_StavEntity_Id), AdditionalWhereSql = "bude nastavené neskôr v ComboCustomize")]
        public string StavNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "S", Editable = false, ReadOnly = true, Tooltip = "Spracované")]
        public bool S { get; set; }

        [DataMember]
        [PfeColumn(Text = "R", Editable = false, ReadOnly = true)]
        public bool R { get; set; }

        [DataMember]
        [PfeColumn(Text = "Ú", Editable = false, ReadOnly = true, Tooltip = "Zaúčtované do účtovníctva")]
        public bool U { get; set; }

        [DataMember]
        [PfeColumn(Text = "Konečný stav", Editable = false, ReadOnly = true)]
        public bool JeKoncovyStav { get; set; }

        [DataMember]
        [PfeColumn(Text = "Stavový priestor", Editable = false)]
        public string StavovyPriestorNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "_SK")]
        public string StrediskoKod { get; set; }

        [DataMember]
        [PfeColumn(Text = "_SKN")]
        public string StrediskoKodNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Stredisko", RequiredFields = new[] { nameof(DatumDokladu) })] //ISNULL(@DatumDokladu, GetDate()) - kvôli ribbon filtru
        [PfeCombo(typeof(StrediskoView), IdColumn = nameof(C_Stredisko_Id), ComboDisplayColumn = nameof(StrediskoView.Nazov), AdditionalWhereSql = "ISNULL(@DatumDokladu, GetDate()) BETWEEN PlatnostOd AND ISNULL(PlatnostDo, '2100-01-01')")]
        public string StrediskoNazov { get; set; }

        [DataMember] //Ponechávam combo ale nie kvôli BAN
        [PfeColumn(Text = "Bank. účet", RequiredFields = new[] { nameof(DatumDokladu) })] //ISNULL(@DatumDokladu, GetDate()) - kvôli ribbon filtru
        [PfeCombo(typeof(BankaUcetView),
            IdColumn = nameof(C_BankaUcet_Id),
            ComboDisplayColumn = nameof(BankaUcetView.Nazov),
            AdditionalWhereSql = "ISNULL(@DatumDokladu, GetDate()) BETWEEN PlatnostOd AND ISNULL(PlatnostDo, '2100-01-01')",
            AdditionalFields = new[] { nameof(BankaUcetView.DatumZostatok), nameof(BankaUcetView.DM_Zostatok) },
            Tpl = "{value};t:n,a:r{DM_Zostatok};t:d{DatumZostatok}")]
        public string BankaUcetNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Pokladnica", ReadOnly = true, Editable = false)]
        public string PokladnicaNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "_PokladnicaKod", Hidden = true, ReadOnly = true, Editable = false)]
        public string PokladnicaKod { get; set; }

        [DataMember]
        [PfeColumn(Text = "Projekt", RequiredFields = new[] { nameof(DatumDokladu) })]
        [PfeCombo(typeof(ProjektView), IdColumn = nameof(C_Projekt_Id), ComboDisplayColumn = nameof(ProjektView.Nazov),
            AdditionalWhereSql = "ISNULL(@DatumDokladu, GetDate()) BETWEEN PlatnostOd AND ISNULL(PlatnostDo, '2100-01-01')")]
        public string ProjektNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Mena", Mandatory = true)]
        [PfeCombo(typeof(MenaView), IdColumn = nameof(C_Mena_Id))]
        public string MenaKod { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_OsobaTyp_Id")]
        public short? C_OsobaTyp_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Typ osoby", ReadOnly = true)]
        public string OsobaTypKod { get; set; }

        [DataMember]
        [PfeColumn(Text = "RČ / IČO", ReadOnly = true)]
        public string Identifikator { get; set; }

        [DataMember]
        [PfeColumn(Text = "Osoba", Xtype = PfeXType.SearchFieldSS, Mandatory = true)]
        [PfeCombo(typeof(OsobaView), ComboDisplayColumn = nameof(OsobaView.IdFormatMeno), IdColumn = nameof(D_Osoba_Id), MinCharSearch = 3,
            ComboIdColumn = nameof(OsobaView.D_Osoba_Id),
            Tpl = "{value};{AdresaTPSidlo}",
            AdditionalFields = new[] { nameof(OsobaView.FakturaciaSplatnost), nameof(OsobaView.AdresaTPSidlo) })]
        public string IdFormatMeno { get; set; } //Atribúty "Tpl" a "AdditionalFields" sú pre PDK zmenené v OsobaView.(public void ComboCustomize)

        // virtualny stlpec, ID by malo byt zhodne s D_Osoba_Id, vyuziva sa na druhe combo, v DTo sa to este kontroluje ci su rovnake
        [DataMember]
        [Ignore]
        [PfeColumn(Text = "_D_Osoba_Id_Fake")]
        public long? D_Osoba_Id_Fake => !string.IsNullOrEmpty(AdresaTPSidlo) ? D_Osoba_Id : null; // Init, preberieme hodnotu

        [DataMember]
        [PfeColumn(Text = "TP / Sídlo", Xtype = PfeXType.SearchFieldSS )]
        [PfeCombo(typeof(OsobaView), ComboDisplayColumn = nameof(OsobaView.AdresaTPSidlo), IdColumn = nameof(D_Osoba_Id_Fake),
                  ComboIdColumn = nameof(OsobaView.D_Osoba_Id), Tpl = "{value};{IdFormatMeno}", MinCharSearch = 2,
                  AdditionalFields = new[] { nameof(OsobaView.FakturaciaSplatnost), nameof(OsobaView.IdFormatMeno) })]
        public string AdresaTPSidlo { get; set; }

        [DataMember]
        [PfeColumn(Text = "Meno / Názov", Editable = false, ReadOnly = true)]
        public string FormatMenoSort { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_FO_StavExistencny_Id", ReadOnly = true)]
        public short? C_FO_StavExistencny_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Lokalita")]
        [PfeCombo(typeof(Lokalita), IdColumn = nameof(C_Lokalita_Id))]
        public string LokalitaNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zaúčtoval", Editable = false, ReadOnly = true)]
        public string Zauctoval { get; set; }

        [DataMember]
        [PfeColumn(Text = "_OrsPravo", Hidden = true, Editable = false, ReadOnly = true)]
        public byte OrsPravo { get; set; }

        [DataMember]
        [PfeColumn(Text = "Fakturačná adresa", RequiredFields = new[] { nameof(D_Osoba_Id) })]
        [PfeCombo(typeof(AdresaComboView), IdColumn = nameof(D_ADR_Adresa_Id), ComboDisplayColumn = nameof(AdresaComboView.AdresaCombo),
            AdditionalWhereSql = "D_Osoba_Id = @D_Osoba_Id",
            CustomSortSqlExp = nameof(AdresaComboView.C_ADR_Typ_Id) + " ASC, " + nameof(AdresaComboView.AdresaCombo) + " ASC")]
        public string Adresa { get; set; }

        [DataMember]
        [PfeColumn(Text = "Predkontácia", Mandatory = true, RequiredFields = new[] { nameof(C_TypBiznisEntity_Kniha_Id) })]
        [PfeCombo(typeof(PredkontaciaCombo), IdColumn = nameof(C_Predkontacia_Id), ComboDisplayColumn = nameof(PredkontaciaCombo.Nazov))]
        public string Predkontacia { get; set; }

        [DataMember]
        [PfeColumn(Text = "DMS súbor", Editable = false)]
        public string SuborNazov { get; set; }

        public void CustomizeModel(PfeDataModel model, IWebEasRepositoryBase repository, HierarchyNode node, string filter, HierarchyNode masterNode)
        {
            const int Dni14 = 14;
            var vztah = FakturaciaVztahEnum.NEURCENE;

            if (model?.Fields == null || node == null || node.TypBiznisEntity == null) return;

            if (node.TypBiznisEntity.Count() > 1)
            {
                throw new NotImplementedException($"C_TypBiznisEntity_Id {C_TypBiznisEntity_Id} is not implemented for multiple entities"); ;
            }

            var typBiznisEntity = node.TypBiznisEntity.First();
            var typBiznisEntityKnihaIntExt = node.TypBiznisEntityKnihaIntExt;

            #region DefaultValue spolocne

            model.Fields.First(p => p.Name == nameof(DatumDokladu)).DefaultValue = DateTime.Today;
            model.Fields.First(p => p.Name == nameof(Rok)).DefaultValue = DateTime.Today.Year;
            model.Fields.First(p => p.Name == nameof(UOMesiac)).DefaultValue = DateTime.Today.Month;
            model.Fields.First(p => p.Name == nameof(C_Mena_Id)).DefaultValue = (short)MenaEnum.EUR;
            model.Fields.First(p => p.Name == nameof(C_OsobaTyp_Id)).DefaultValue = (short)OsobaTypEnum.Podnikatel;
            //model.Fields.First(p => p.Name == nameof(DatumVDP)).Text = "_DatumVDP";
            #endregion

            #region Skryvanie, zmena textu, povinnost, default hodnoty stlpcov jednotlivo

            #region Skryvanie - atribúty OSOBY
            if (typBiznisEntity == TypBiznisEntityEnum.IND || typBiznisEntity == TypBiznisEntityEnum.BAN || typBiznisEntity == TypBiznisEntityEnum.PPP)
            {
                model.Fields.First(p => p.Name == nameof(OsobaTypKod)).Text = "_OsobaTypKod";
                model.Fields.First(p => p.Name == nameof(Identifikator)).Text = "_Identifikator";
                model.Fields.First(p => p.Name == nameof(IdFormatMeno)).Text = "_IdFormatMeno";
                model.Fields.First(p => p.Name == nameof(AdresaTPSidlo)).Text = "_AdresaTPSidlo";
                model.Fields.First(p => p.Name == nameof(FormatMenoSort)).Text = "_FormatMenoSort";

                model.Fields.First(p => p.Name == nameof(D_Osoba_Id)).Mandatory = false; //Mandatory v combe ide na ID pole

            }
            else if (typBiznisEntity == TypBiznisEntityEnum.DFA || typBiznisEntity == TypBiznisEntityEnum.DZF ||
                     typBiznisEntity == TypBiznisEntityEnum.DZM || typBiznisEntity == TypBiznisEntityEnum.DOB ||
                     typBiznisEntity == TypBiznisEntityEnum.DCP || typBiznisEntity == TypBiznisEntityEnum.DDP)
            {
                model.Fields.First(p => p.Name == nameof(IdFormatMeno)).Text = "Dodávateľ";
                vztah = FakturaciaVztahEnum.DOD;
            }
            else if (typBiznisEntity == TypBiznisEntityEnum.OFA || typBiznisEntity == TypBiznisEntityEnum.OZF || typBiznisEntity == TypBiznisEntityEnum.DOL ||
                    typBiznisEntity == TypBiznisEntityEnum.OZM || typBiznisEntity == TypBiznisEntityEnum.OOB ||
                    typBiznisEntity == TypBiznisEntityEnum.OCP || typBiznisEntity == TypBiznisEntityEnum.ODP)
            {
                model.Fields.First(p => p.Name == nameof(IdFormatMeno)).Text = "Odberateľ";
                vztah = FakturaciaVztahEnum.ODB;
            }

            #endregion

            #region Skryvanie Z, R, UOMesiac
            if (!(typBiznisEntity == TypBiznisEntityEnum.IND || typBiznisEntity == TypBiznisEntityEnum.BAN || typBiznisEntity == TypBiznisEntityEnum.PDK ||
                 typBiznisEntity == TypBiznisEntityEnum.DFA || typBiznisEntity == TypBiznisEntityEnum.OFA))
            {
                model.Fields.First(p => p.Name == nameof(U)).Text = "_Ú";
                if (!(typBiznisEntity == TypBiznisEntityEnum.DCP || typBiznisEntity == TypBiznisEntityEnum.OCP ||
                      typBiznisEntity == TypBiznisEntityEnum.DZF || typBiznisEntity == TypBiznisEntityEnum.OZF ||
                      typBiznisEntity == TypBiznisEntityEnum.DZM || typBiznisEntity == TypBiznisEntityEnum.OZM ||
                      typBiznisEntity == TypBiznisEntityEnum.OOB || typBiznisEntity == TypBiznisEntityEnum.DOB))
                {
                    //Mali by to byť doklady: DDP, ODP, DOL, PPP
                    model.Fields.First(p => p.Name == nameof(R)).Text = "_R";
                    model.Fields.First(p => p.Name == nameof(UOMesiac)).Text = "_UOMesiac";
                }
            }
            if (typBiznisEntity == TypBiznisEntityEnum.IND || typBiznisEntity == TypBiznisEntityEnum.BAN || typBiznisEntity == TypBiznisEntityEnum.PDK)
            {
                model.Fields.First(p => p.Name == nameof(R)).Tooltip = "Zaúčtované do rozpočtu";
            }
            else
            {
                model.Fields.First(p => p.Name == nameof(R)).Tooltip = "Potvrdený rozpočet";
            }

            #endregion

            #region Nastavenie - VS
            if (typBiznisEntity == TypBiznisEntityEnum.DFA || typBiznisEntity == TypBiznisEntityEnum.DZF)
            {
                model.Fields.First(p => p.Name == nameof(VS)).Mandatory = true;
                model.Fields.First(p => p.Name == nameof(VS)).ReadOnly = false;
                model.Fields.First(p => p.Name == nameof(VS)).Editable = true;
            }
            else if (typBiznisEntity == TypBiznisEntityEnum.OFA || typBiznisEntity == TypBiznisEntityEnum.OZF)
            {
                //model.Fields.First(p => p.Name == nameof(VS)).ReadOnly = true; //V prípade PS to potrebujem na BE posielať
                model.Fields.First(p => p.Name == nameof(VS)).Editable = false;
            }
            else
            {
                model.Fields.First(p => p.Name == nameof(VS)).Text = "_VS";
            }
            #endregion

            #region Nastavenie - ProjektNazov
            if (typBiznisEntity == TypBiznisEntityEnum.PPP || typBiznisEntity == TypBiznisEntityEnum.BAN)
            {
                //Projekt - nezobrazené
                model.Fields.First(p => p.Name == nameof(ProjektNazov)).Text = "_PN";
                model.Fields.First(p => p.Name == nameof(ProjektNazov)).LoadWhenVisible = true;
            }
            #endregion

            #region Nastavenie - BankaUcetNazov, PokladnicaNazov
            if (typBiznisEntity == TypBiznisEntityEnum.OFA || typBiznisEntity == TypBiznisEntityEnum.OZF ||
                typBiznisEntity == TypBiznisEntityEnum.PPP)
            {
                //Bank.účet - zobrazené a povinné
                model.Fields.First(p => p.Name == nameof(C_BankaUcet_Id)).Mandatory = true;
                model.Fields.First(p => p.Name == nameof(BankaUcetNazov)).Mandatory = true; //asi zbytocne, berie sa z ID
                //Pokladnica - nezobrazené
                model.Fields.First(p => p.Name == nameof(PokladnicaNazov)).Text = "_PokladnicaNazov";
            }
            else if (typBiznisEntity == TypBiznisEntityEnum.BAN)
            {
                //Bank.účet - zobrazené a povinné
                model.Fields.First(p => p.Name == nameof(C_BankaUcet_Id)).Mandatory = true;
                model.Fields.First(p => p.Name == nameof(BankaUcetNazov)).Mandatory = true; //asi zbytocne, berie sa z ID
                model.Fields.First(p => p.Name == nameof(BankaUcetNazov)).Editable = false; //Bank.výpis nemá mať combo
                model.Fields.First(p => p.Name == nameof(BankaUcetNazov)).ReadOnly = true;
                model.Fields.First(p => p.Name == nameof(BankaUcetNazov)).Xtype = PfeXType.Textfield;
                //Pokladnica - nezobrazené
                model.Fields.First(p => p.Name == nameof(PokladnicaNazov)).Text = "_PokladnicaNazov";
            }
            else if (typBiznisEntity == TypBiznisEntityEnum.PDK)
            {
                //Bank.účet - nezobrazené
                model.Fields.First(p => p.Name == nameof(BankaUcetNazov)).Text = "_BankaUcetNazov";
                //Pokladnica - Zobrazené a povinné
                model.Fields.First(p => p.Name == nameof(C_Pokladnica_Id)).Mandatory = true;
                model.Fields.First(p => p.Name == nameof(PokladnicaNazov)).Mandatory = true;  //Editable = false ale nevadí, že nastavujem aj Mandatory
            }
            else if (typBiznisEntity == TypBiznisEntityEnum.DOB)
            {
                //Bank.účet - Zobrazené a nepovinné - DEFAULT
                //Pokladnica - nezobrazené
                model.Fields.First(p => p.Name == nameof(PokladnicaNazov)).Text = "_PokladnicaNazov";
                model.Fields.First(p => p.Name == nameof(PokladnicaNazov)).LoadWhenVisible = true;
            }
            else
            {
                //Bank.účet, Pokladnica - nezobrazené
                model.Fields.First(p => p.Name == nameof(BankaUcetNazov)).Text = "_BankaUcetNazov";
                model.Fields.First(p => p.Name == nameof(PokladnicaNazov)).Text = "_PokladnicaNazov";
                model.Fields.First(p => p.Name == nameof(BankaUcetNazov)).LoadWhenVisible = true;
                model.Fields.First(p => p.Name == nameof(PokladnicaNazov)).LoadWhenVisible = true;
            }
            #endregion

            #region Nastavenie - StrediskoNazov
            if (typBiznisEntity == TypBiznisEntityEnum.BAN || typBiznisEntity == TypBiznisEntityEnum.PPP)
            {
                model.Fields.First(p => p.Name == nameof(StrediskoNazov)).Text = "_StrediskoNazov";
                model.Fields.First(p => p.Name == nameof(StrediskoNazov)).LoadWhenVisible = true;
            }
            else if (typBiznisEntity == TypBiznisEntityEnum.PDK)
            {
                //Stredisko - Zobrazené a nepovinné - DEFAULT
                model.Fields.First(p => p.Name == nameof(StrediskoNazov)).Text = repository.GetNastavenieS("reg", "OrjNazovJC");
            }
            else
            {
                model.Fields.First(p => p.Name == nameof(C_Stredisko_Id)).Mandatory = true;
                model.Fields.First(p => p.Name == nameof(StrediskoNazov)).Mandatory = true; //asi zbytocne, berie sa z ID
                model.Fields.First(p => p.Name == nameof(StrediskoNazov)).Text = repository.GetNastavenieS("reg", "OrjNazovJC");
            }
            #endregion

            #region Nastavenie - Adresa
            //OUT: DZM, DOB, DDP, OFA, DOL, OZF, OCP
            // IN: DFA, DZF, DCP, OZM, OOB, ODP
            if (typBiznisEntity == TypBiznisEntityEnum.BAN || typBiznisEntity == TypBiznisEntityEnum.IND)
            {
                model.Fields.First(p => p.Name == nameof(Adresa)).Text = "_Adresa";
            }
            else if (typBiznisEntity == TypBiznisEntityEnum.PDK)
            {
                model.Fields.First(p => p.Name == nameof(Adresa)).Text = "Adresa";
            }
            else if (typBiznisEntity == TypBiznisEntityEnum.DOL || typBiznisEntity == TypBiznisEntityEnum.ODP)
            {
                model.Fields.First(p => p.Name == nameof(Adresa)).Text = "Adresa dodania";
            }
            else if (typBiznisEntity == TypBiznisEntityEnum.OFA || typBiznisEntity == TypBiznisEntityEnum.OZF || typBiznisEntity == TypBiznisEntityEnum.OCP ||
                     typBiznisEntity == TypBiznisEntityEnum.DZM || typBiznisEntity == TypBiznisEntityEnum.DOB || typBiznisEntity == TypBiznisEntityEnum.DDP)
            {
                model.Fields.First(p => p.Name == nameof(Adresa)).Text = "Korešp. adresa";
            }
            else
            {
                //IN doklady - viď hore
                //Default nastavená v modeli je "Fakturačná adresa"
            }
            #endregion

            #region DMS Subor

            if (typBiznisEntity != TypBiznisEntityEnum.BAN && typBiznisEntity != TypBiznisEntityEnum.PPP)
            {
                model.Fields.First(p => p.Name == nameof(SuborNazov)).Text = "_DMS súbor";
            }

            #endregion

            switch (typBiznisEntity)
            {
                case TypBiznisEntityEnum.Unknown:
                    break;
                case TypBiznisEntityEnum.IND:
                    #region Skryvanie
                    if (typBiznisEntityKnihaIntExt == (int)TypBiznisEntity_KnihaEnum.Externe_doklady_DaP || typBiznisEntityKnihaIntExt == (int)TypBiznisEntity_KnihaEnum.Externe_doklady_majetok ||
                        typBiznisEntityKnihaIntExt == (int)TypBiznisEntity_KnihaEnum.Externe_doklady_mzdy || typBiznisEntityKnihaIntExt == (int)TypBiznisEntity_KnihaEnum.Externe_doklady_sklad)
                    {
                        model.Fields.First(p => p.Name == nameof(Kniha)).Text = "_Kniha";
                        model.Fields.First(p => p.Name == nameof(Kniha)).Xtype = PfeXType.Textfield;
                        //ReadOnly zostáva TRUE
                    }

                    model.Fields.First(p => p.Name == nameof(DatumVystavenia)).Text = "_DatumVystavenia";
                    model.Fields.First(p => p.Name == nameof(DatumPrijatia)).Text = "_DatumPrijatia";
                    model.Fields.First(p => p.Name == nameof(DatumSplatnosti)).Text = "_DatumSplatnosti";
                    model.Fields.First(p => p.Name == nameof(DatumDodania)).Text = "_DatumDodania";
                    model.Fields.First(p => p.Name == nameof(CisloInterne)).Text = "Číslo dokladu";
                    model.Fields.First(p => p.Name == nameof(CisloExterne)).Text = "_CisloExterne";

                    #endregion

                    #region Default hodnoty
                    if (typBiznisEntityKnihaIntExt.HasValue)
                    {
                        model.Fields.First(p => p.Name == nameof(C_TypBiznisEntity_Kniha_Id)).DefaultValue = typBiznisEntityKnihaIntExt.Value;
                    }
                    #endregion
                    break;
                case TypBiznisEntityEnum.DFA:
                    #region Skryvanie
                    //model.Fields.First(p => p.Name == nameof(DatumDokladu)).Text = "_DatumDokladu";
                    model.Fields.First(p => p.Name == nameof(DatumDokladu)).Tooltip = "Referenčný dátum dokladu, zaúčtovania";
                    #endregion

                    #region Zmena textu
                    model.Fields.First(p => p.Name == nameof(CisloExterne)).Text = "Číslo faktúry";
                    #endregion

                    #region Povinnosti
                    model.Fields.First(p => p.Name == nameof(DatumPrijatia)).Mandatory = true;
                    model.Fields.First(p => p.Name == nameof(DatumVystavenia)).Mandatory = true;
                    model.Fields.First(p => p.Name == nameof(DatumSplatnosti)).Mandatory = true;
                    model.Fields.First(p => p.Name == nameof(DatumDodania)).Mandatory = true;

                    model.Fields.First(p => p.Name == nameof(D_Osoba_Id)).Mandatory = true; //Mandatory v combe ide na ID pole

                    model.Fields.First(p => p.Name == nameof(CisloExterne)).Mandatory = true;
                    #endregion

                    #region Default hodnoty
                    model.Fields.First(p => p.Name == nameof(DatumPrijatia)).DefaultValue = DateTime.Today;
                    model.Fields.First(p => p.Name == nameof(DatumVystavenia)).DefaultValue = DateTime.Today;
                    model.Fields.First(p => p.Name == nameof(DatumSplatnosti)).DefaultValue = DateTime.Today.AddDays(Dni14);
                    model.Fields.First(p => p.Name == nameof(DatumDodania)).DefaultValue = DateTime.Today;
                    model.Fields.First(p => p.Name == nameof(C_TypBiznisEntity_Kniha_Id)).DefaultValue = (int)TypBiznisEntity_KnihaEnum.Dodavatelske_faktury;
                    #endregion

                    break;
                case TypBiznisEntityEnum.OFA:
                    #region Skryvanie
                    //model.Fields.First(p => p.Name == nameof(DatumDokladu)).Text = "_DatumDokladu";
                    model.Fields.First(p => p.Name == nameof(DatumDokladu)).Tooltip = "Referenčný dátum dokladu, zaúčtovania";
                    model.Fields.First(p => p.Name == nameof(DatumPrijatia)).Text = "_DatumPrijatia";

                    model.Fields.First(p => p.Name == nameof(CisloExterne)).Text = "_CisloExterne";
                    #endregion

                    #region Zmena textu
                    model.Fields.First(p => p.Name == nameof(DatumDodania)).Text = "Dátum dodania/úhrady";
                    #endregion

                    #region Povinnosti
                    model.Fields.First(p => p.Name == nameof(DatumVystavenia)).Mandatory = true;
                    model.Fields.First(p => p.Name == nameof(DatumSplatnosti)).Mandatory = true;
                    model.Fields.First(p => p.Name == nameof(DatumDodania)).Mandatory = true;

                    model.Fields.First(p => p.Name == nameof(D_Osoba_Id)).Mandatory = true; //Mandatory v combe ide na ID pole
                    #endregion

                    #region Default hodnoty
                    model.Fields.First(p => p.Name == nameof(DatumVystavenia)).DefaultValue = DateTime.Today;
                    model.Fields.First(p => p.Name == nameof(DatumSplatnosti)).DefaultValue = DateTime.Today.AddDays(Dni14);
                    model.Fields.First(p => p.Name == nameof(DatumDodania)).DefaultValue = DateTime.Today;
                    model.Fields.First(p => p.Name == nameof(C_TypBiznisEntity_Kniha_Id)).DefaultValue = (int)TypBiznisEntity_KnihaEnum.Odberatelske_faktury;
                    #endregion
                    break;
                case TypBiznisEntityEnum.PDK:
                    #region Skryvanie
                    model.Fields.First(p => p.Name == nameof(DatumPrijatia)).Text = "_DatumPrijatia";
                    model.Fields.First(p => p.Name == nameof(DatumVystavenia)).Text = "_DatumVystavenia";
                    model.Fields.First(p => p.Name == nameof(DatumSplatnosti)).Text = "_DatumSplatnosti";
                    model.Fields.First(p => p.Name == nameof(DatumDodania)).Text = "_DatumDodania";
                    model.Fields.First(p => p.Name == nameof(D_Osoba_Id)).Mandatory = false; //Mandatory v combe ide na ID pole
                    #endregion

                    #region Zmena textu
                    model.Fields.First(p => p.Name == nameof(CisloExterne)).Text = "Číslo PD";
                    model.Fields.First(p => p.Name == nameof(Popis)).Text = "Účel";
                    //model.Fields.First(p => p.Name == nameof(OsobaKontaktKomu)).Text = ; //Mení sa validátorom, v GRIDE je default: "Kontaktná osoba"
                    #endregion

                    #region Default hodnoty
                    model.Fields.First(p => p.Name == nameof(C_TypBiznisEntity_Kniha_Id)).DefaultValue = (int)TypBiznisEntity_KnihaEnum.Prijmove_pokladnicne_doklady;
                    model.Fields.First(p => p.Name == nameof(C_OsobaTyp_Id)).DefaultValue = (short)OsobaTypEnum.Fyzicka_osoba;
                    #endregion

                    break;
                case TypBiznisEntityEnum.PRI:
                    #region Default hodnoty
                    model.Fields.First(p => p.Name == nameof(C_TypBiznisEntity_Kniha_Id)).DefaultValue = (int)TypBiznisEntity_KnihaEnum.Prijemky;
                    #endregion
                    break;
                case TypBiznisEntityEnum.VYD:
                    #region Default hodnoty
                    model.Fields.First(p => p.Name == nameof(C_TypBiznisEntity_Kniha_Id)).DefaultValue = (int)TypBiznisEntity_KnihaEnum.Vydajky;
                    #endregion
                    break;
                case TypBiznisEntityEnum.PRE:
                    #region Default hodnoty
                    model.Fields.First(p => p.Name == nameof(C_TypBiznisEntity_Kniha_Id)).DefaultValue = (int)TypBiznisEntity_KnihaEnum.Prevodky;
                    #endregion
                    break;
                case TypBiznisEntityEnum.BAN:
                    #region Skryvanie
                    model.Fields.First(p => p.Name == nameof(Kniha)).Text = "_Kniha";
                    model.Fields.First(p => p.Name == nameof(LokalitaNazov)).Text = "_LokalitaNazov";
                    model.Fields.First(p => p.Name == nameof(DatumPrijatia)).Text = "_DatumPrijatia";
                    model.Fields.First(p => p.Name == nameof(DatumVystavenia)).Text = "_DatumVystavenia";
                    model.Fields.First(p => p.Name == nameof(DatumSplatnosti)).Text = "_DatumSplatnosti";
                    model.Fields.First(p => p.Name == nameof(DatumDodania)).Text = "_DatumDodania";

                    #endregion

                    #region Zmena textu
                    model.Fields.First(p => p.Name == nameof(CisloExterne)).Text = "Číslo výpisu";
                    #endregion

                    #region Povinnosti
                    model.Fields.First(p => p.Name == nameof(CisloExterne)).Mandatory = true;

                    #endregion

                    #region Default hodnoty
                    model.Fields.First(p => p.Name == nameof(C_TypBiznisEntity_Kniha_Id)).DefaultValue = (int)TypBiznisEntity_KnihaEnum.Bankove_vypisy;
                    #endregion
                    break;
                case TypBiznisEntityEnum.ODP:
                    #region Skryvanie
                    model.Fields.First(p => p.Name == nameof(DatumDokladu)).Text = "_DatumDokladu";
                    model.Fields.First(p => p.Name == nameof(DatumSplatnosti)).Text = "_DatumSplatnosti";
                    model.Fields.First(p => p.Name == nameof(Predkontacia)).Text = "_Predkontacia";
                    #endregion

                    #region Zmena textu
                    model.Fields.First(p => p.Name == nameof(DatumDodania)).Text = "Požadovaný dátum dodania";
                    model.Fields.First(p => p.Name == nameof(CisloExterne)).Text = "Číslo dopytu";
                    #endregion

                    #region Default hodnoty
                    model.Fields.First(p => p.Name == nameof(DatumPrijatia)).DefaultValue = DateTime.Today;
                    model.Fields.First(p => p.Name == nameof(DatumVystavenia)).DefaultValue = DateTime.Today;
                    model.Fields.First(p => p.Name == nameof(DatumDodania)).DefaultValue = DateTime.Today;
                    model.Fields.First(p => p.Name == nameof(C_TypBiznisEntity_Kniha_Id)).DefaultValue = (int)TypBiznisEntity_KnihaEnum.Odberatelske_dopyty;
                    #endregion

                    #region Povinnosti
                    model.Fields.First(p => p.Name == nameof(DatumPrijatia)).Mandatory = true;
                    model.Fields.First(p => p.Name == nameof(DatumVystavenia)).Mandatory = true;
                    model.Fields.First(p => p.Name == nameof(DatumDodania)).Mandatory = true;

                    model.Fields.First(p => p.Name == nameof(D_Osoba_Id)).Mandatory = true; //Mandatory v combe ide na ID pole
                    model.Fields.First(p => p.Name == nameof(CisloExterne)).Mandatory = true;
                    #endregion
                    break;
                case TypBiznisEntityEnum.DDP:
                    #region Skryvanie
                    model.Fields.First(p => p.Name == nameof(DatumDokladu)).Text = "_DatumDokladu";
                    model.Fields.First(p => p.Name == nameof(DatumPrijatia)).Text = "_DatumPrijatia";
                    model.Fields.First(p => p.Name == nameof(DatumSplatnosti)).Text = "_DatumSplatnosti";
                    model.Fields.First(p => p.Name == nameof(Predkontacia)).Text = "_Predkontacia";
                    model.Fields.First(p => p.Name == nameof(CisloExterne)).Text = "_CisloExterne";
                    #endregion

                    #region Zmena textu
                    model.Fields.First(p => p.Name == nameof(DatumDodania)).Text = "Požadovaný dátum dodania";
                    #endregion

                    #region Default hodnoty
                    model.Fields.First(p => p.Name == nameof(DatumVystavenia)).DefaultValue = DateTime.Today;
                    model.Fields.First(p => p.Name == nameof(DatumDodania)).DefaultValue = DateTime.Today;
                    model.Fields.First(p => p.Name == nameof(C_TypBiznisEntity_Kniha_Id)).DefaultValue = (int)TypBiznisEntity_KnihaEnum.Dodavatelske_dopyty;
                    #endregion

                    #region Povinnosti
                    model.Fields.First(p => p.Name == nameof(DatumVystavenia)).Mandatory = true;
                    model.Fields.First(p => p.Name == nameof(DatumDodania)).Mandatory = true;

                    model.Fields.First(p => p.Name == nameof(D_Osoba_Id)).Mandatory = true; //Mandatory v combe ide na ID pole
                    #endregion
                    break;
                case TypBiznisEntityEnum.OCP:
                    #region Skryvanie
                    model.Fields.First(p => p.Name == nameof(DatumDokladu)).Text = "_DatumDokladu";
                    model.Fields.First(p => p.Name == nameof(DatumPrijatia)).Text = "_DatumPrijatia";
                    model.Fields.First(p => p.Name == nameof(DatumSplatnosti)).Text = "_DatumSplatnosti";

                    model.Fields.First(p => p.Name == nameof(CisloExterne)).Text = "_CisloExterne";
                    #endregion

                    #region Zmena textu
                    model.Fields.First(p => p.Name == nameof(DatumVystavenia)).Text = "Dátum vypracovania";
                    model.Fields.First(p => p.Name == nameof(DatumDodania)).Text = "Platná do";
                    #endregion

                    #region Default hodnoty
                    model.Fields.First(p => p.Name == nameof(DatumVystavenia)).DefaultValue = DateTime.Today;
                    model.Fields.First(p => p.Name == nameof(DatumDodania)).DefaultValue = DateTime.Today;
                    model.Fields.First(p => p.Name == nameof(C_TypBiznisEntity_Kniha_Id)).DefaultValue = (int)TypBiznisEntity_KnihaEnum.Odberatelske_cenove_ponuky;
                    #endregion

                    #region Povinnosti
                    model.Fields.First(p => p.Name == nameof(DatumVystavenia)).Mandatory = true;
                    model.Fields.First(p => p.Name == nameof(DatumDodania)).Mandatory = true;

                    model.Fields.First(p => p.Name == nameof(D_Osoba_Id)).Mandatory = true; //Mandatory v combe ide na ID pole
                    #endregion
                    break;
                case TypBiznisEntityEnum.DCP:
                    #region Skryvanie
                    model.Fields.First(p => p.Name == nameof(DatumSplatnosti)).Text = "_DatumSplatnosti";
                    #endregion

                    #region Zmena textu
                    model.Fields.First(p => p.Name == nameof(DatumDodania)).Text = "Platná do";
                    model.Fields.First(p => p.Name == nameof(CisloExterne)).Text = "Číslo cenovej ponuky";
                    #endregion

                    #region Default hodnoty
                    model.Fields.First(p => p.Name == nameof(DatumPrijatia)).DefaultValue = DateTime.Today;
                    model.Fields.First(p => p.Name == nameof(DatumVystavenia)).DefaultValue = DateTime.Today;
                    model.Fields.First(p => p.Name == nameof(DatumDodania)).DefaultValue = DateTime.Today;
                    model.Fields.First(p => p.Name == nameof(C_TypBiznisEntity_Kniha_Id)).DefaultValue = (int)TypBiznisEntity_KnihaEnum.Dodavatelske_cenove_ponuky;
                    #endregion

                    #region Povinnosti
                    model.Fields.First(p => p.Name == nameof(DatumPrijatia)).Mandatory = true;
                    model.Fields.First(p => p.Name == nameof(DatumVystavenia)).Mandatory = true;
                    model.Fields.First(p => p.Name == nameof(DatumDodania)).Mandatory = true;

                    model.Fields.First(p => p.Name == nameof(D_Osoba_Id)).Mandatory = true; //Mandatory v combe ide na ID pole
                    model.Fields.First(p => p.Name == nameof(CisloExterne)).Mandatory = true;
                    #endregion
                    break;
                case TypBiznisEntityEnum.OOB:
                    #region Skryvanie
                    model.Fields.First(p => p.Name == nameof(DatumDokladu)).Text = "_DatumDokladu";
                    model.Fields.First(p => p.Name == nameof(DatumSplatnosti)).Text = "_DatumSplatnosti";
                    #endregion

                    #region Zmena textu
                    model.Fields.First(p => p.Name == nameof(DatumDodania)).Text = "Požadovaný dátum dodania";
                    model.Fields.First(p => p.Name == nameof(CisloExterne)).Text = "Číslo objednávky";
                    #endregion

                    #region Default hodnoty
                    model.Fields.First(p => p.Name == nameof(DatumPrijatia)).DefaultValue = DateTime.Today;
                    model.Fields.First(p => p.Name == nameof(DatumVystavenia)).DefaultValue = DateTime.Today;
                    model.Fields.First(p => p.Name == nameof(DatumDodania)).DefaultValue = DateTime.Today;
                    model.Fields.First(p => p.Name == nameof(C_TypBiznisEntity_Kniha_Id)).DefaultValue = (int)TypBiznisEntity_KnihaEnum.Odberatelske_objednavky;
                    #endregion

                    #region Povinnosti
                    model.Fields.First(p => p.Name == nameof(DatumPrijatia)).Mandatory = true;
                    model.Fields.First(p => p.Name == nameof(DatumVystavenia)).Mandatory = true;
                    model.Fields.First(p => p.Name == nameof(DatumDodania)).Mandatory = true;

                    model.Fields.First(p => p.Name == nameof(D_Osoba_Id)).Mandatory = true; //Mandatory v combe ide na ID pole
                    model.Fields.First(p => p.Name == nameof(CisloExterne)).Mandatory = true;
                    #endregion
                    break;
                case TypBiznisEntityEnum.DOB:
                    #region Skryvanie
                    model.Fields.First(p => p.Name == nameof(DatumDokladu)).Text = "_DatumDokladu";
                    model.Fields.First(p => p.Name == nameof(DatumPrijatia)).Text = "_DatumPrijatia";
                    model.Fields.First(p => p.Name == nameof(DatumSplatnosti)).Text = "_DatumSplatnosti";

                    model.Fields.First(p => p.Name == nameof(CisloExterne)).Text = "_CisloExterne";
                    #endregion

                    #region Zmena textu
                    model.Fields.First(p => p.Name == nameof(DatumDodania)).Text = "Požadovaný dátum dodania";
                    #endregion

                    #region Default hodnoty
                    model.Fields.First(p => p.Name == nameof(DatumVystavenia)).DefaultValue = DateTime.Today;
                    model.Fields.First(p => p.Name == nameof(DatumDodania)).DefaultValue = DateTime.Today;
                    model.Fields.First(p => p.Name == nameof(C_TypBiznisEntity_Kniha_Id)).DefaultValue = (int)TypBiznisEntity_KnihaEnum.Dodavatelske_objednavky;
                    #endregion

                    #region Povinnosti
                    model.Fields.First(p => p.Name == nameof(DatumVystavenia)).Mandatory = true;
                    model.Fields.First(p => p.Name == nameof(DatumDodania)).Mandatory = true;

                    model.Fields.First(p => p.Name == nameof(D_Osoba_Id)).Mandatory = true; //Mandatory v combe ide na ID pole
                    #endregion
                    break;
                case TypBiznisEntityEnum.OZM:
                    #region Skryvanie
                    model.Fields.First(p => p.Name == nameof(DatumDokladu)).Text = "_DatumDokladu";
                    #endregion

                    #region Zmena textu
                    model.Fields.First(p => p.Name == nameof(DatumVystavenia)).Text = "Dátum vypracovania";
                    model.Fields.First(p => p.Name == nameof(DatumSplatnosti)).Text = "Dátum podpísania";
                    model.Fields.First(p => p.Name == nameof(DatumDodania)).Text = "Platná od";
                    model.Fields.First(p => p.Name == nameof(CisloExterne)).Text = "Číslo zmluvy";
                    #endregion

                    #region Default hodnoty
                    model.Fields.First(p => p.Name == nameof(DatumPrijatia)).DefaultValue = DateTime.Today;
                    model.Fields.First(p => p.Name == nameof(DatumVystavenia)).DefaultValue = DateTime.Today;
                    model.Fields.First(p => p.Name == nameof(DatumSplatnosti)).DefaultValue = DateTime.Today;
                    model.Fields.First(p => p.Name == nameof(DatumDodania)).DefaultValue = DateTime.Today;
                    model.Fields.First(p => p.Name == nameof(C_TypBiznisEntity_Kniha_Id)).DefaultValue = (int)TypBiznisEntity_KnihaEnum.Odberatelske_zmluvy;
                    #endregion

                    #region Povinnosti
                    model.Fields.First(p => p.Name == nameof(DatumPrijatia)).Mandatory = true;
                    model.Fields.First(p => p.Name == nameof(DatumVystavenia)).Mandatory = true;
                    model.Fields.First(p => p.Name == nameof(DatumDodania)).Mandatory = true;

                    model.Fields.First(p => p.Name == nameof(D_Osoba_Id)).Mandatory = true; //Mandatory v combe ide na ID pole
                    model.Fields.First(p => p.Name == nameof(CisloExterne)).Mandatory = true;
                    #endregion
                    break;
                case TypBiznisEntityEnum.DZM:
                    #region Skryvanie
                    model.Fields.First(p => p.Name == nameof(DatumDokladu)).Text = "_DatumDokladu";
                    model.Fields.First(p => p.Name == nameof(DatumPrijatia)).Text = "_DatumPrijatia";

                    model.Fields.First(p => p.Name == nameof(CisloExterne)).Text = "_CisloExterne";
                    #endregion

                    #region Zmena textu
                    model.Fields.First(p => p.Name == nameof(DatumVystavenia)).Text = "Dátum vypracovania";
                    model.Fields.First(p => p.Name == nameof(DatumSplatnosti)).Text = "Dátum podpísania";
                    model.Fields.First(p => p.Name == nameof(DatumDodania)).Text = "Platná od";
                    #endregion

                    #region Default hodnoty
                    model.Fields.First(p => p.Name == nameof(DatumVystavenia)).DefaultValue = DateTime.Today;
                    model.Fields.First(p => p.Name == nameof(DatumSplatnosti)).DefaultValue = DateTime.Today;
                    model.Fields.First(p => p.Name == nameof(DatumDodania)).DefaultValue = DateTime.Today;
                    model.Fields.First(p => p.Name == nameof(C_TypBiznisEntity_Kniha_Id)).DefaultValue = (int)TypBiznisEntity_KnihaEnum.Dodavatelske_zmluvy;
                    #endregion

                    #region Povinnosti
                    model.Fields.First(p => p.Name == nameof(DatumVystavenia)).Mandatory = true;
                    model.Fields.First(p => p.Name == nameof(DatumDodania)).Mandatory = true;

                    model.Fields.First(p => p.Name == nameof(D_Osoba_Id)).Mandatory = true; //Mandatory v combe ide na ID pole
                    #endregion
                    break;
                case TypBiznisEntityEnum.OZF:
                    #region Skryvanie
                    model.Fields.First(p => p.Name == nameof(DatumDokladu)).Text = "_DatumDokladu";
                    model.Fields.First(p => p.Name == nameof(DatumPrijatia)).Text = "_DatumPrijatia";
                    model.Fields.First(p => p.Name == nameof(DatumDodania)).Text = "_DatumDodania";

                    model.Fields.First(p => p.Name == nameof(CisloExterne)).Text = "_CisloExterne";
                    #endregion

                    #region Default hodnoty
                    model.Fields.First(p => p.Name == nameof(DatumVystavenia)).DefaultValue = DateTime.Today;
                    model.Fields.First(p => p.Name == nameof(DatumSplatnosti)).DefaultValue = DateTime.Today.AddDays(Dni14);
                    model.Fields.First(p => p.Name == nameof(C_TypBiznisEntity_Kniha_Id)).DefaultValue = (int)TypBiznisEntity_KnihaEnum.Odberatelske_zalohove_faktury;
                    #endregion

                    #region Povinnosti
                    model.Fields.First(p => p.Name == nameof(DatumVystavenia)).Mandatory = true;
                    model.Fields.First(p => p.Name == nameof(DatumSplatnosti)).Mandatory = true;

                    model.Fields.First(p => p.Name == nameof(D_Osoba_Id)).Mandatory = true; //Mandatory v combe ide na ID pole
                    #endregion
                    break;
                case TypBiznisEntityEnum.DZF:
                    #region Skryvanie
                    model.Fields.First(p => p.Name == nameof(DatumDokladu)).Text = "_DatumDokladu";
                    model.Fields.First(p => p.Name == nameof(DatumDodania)).Text = "_DatumDodania";
                    #endregion

                    #region Zmena textu
                    model.Fields.First(p => p.Name == nameof(CisloExterne)).Text = "Číslo zálohy";
                    #endregion

                    #region Default hodnoty
                    model.Fields.First(p => p.Name == nameof(DatumPrijatia)).DefaultValue = DateTime.Today;
                    model.Fields.First(p => p.Name == nameof(DatumVystavenia)).DefaultValue = DateTime.Today;
                    model.Fields.First(p => p.Name == nameof(DatumSplatnosti)).DefaultValue = DateTime.Today.AddDays(Dni14);
                    model.Fields.First(p => p.Name == nameof(C_TypBiznisEntity_Kniha_Id)).DefaultValue = (int)TypBiznisEntity_KnihaEnum.Dodavatelske_zalohove_faktury;
                    #endregion

                    #region Povinnosti
                    model.Fields.First(p => p.Name == nameof(DatumPrijatia)).Mandatory = true;
                    model.Fields.First(p => p.Name == nameof(DatumVystavenia)).Mandatory = true;
                    model.Fields.First(p => p.Name == nameof(DatumSplatnosti)).Mandatory = true;

                    model.Fields.First(p => p.Name == nameof(D_Osoba_Id)).Mandatory = true; //Mandatory v combe ide na ID pole
                    model.Fields.First(p => p.Name == nameof(CisloExterne)).Mandatory = true;
                    #endregion

                    break;
                case TypBiznisEntityEnum.PPP:
                    #region Skryvanie
                    model.Fields.First(p => p.Name == nameof(Kniha)).Text = "_Kniha";
                    model.Fields.First(p => p.Name == nameof(LokalitaNazov)).Text = "_LokalitaNazov";
                    model.Fields.First(p => p.Name == nameof(DatumPrijatia)).Text = "_DatumPrijatia";
                    model.Fields.First(p => p.Name == nameof(DatumVystavenia)).Text = "_DatumVystavenia";
                    model.Fields.First(p => p.Name == nameof(DatumDodania)).Text = "_DatumDodania";
                    model.Fields.First(p => p.Name == nameof(CisloExterne)).Text = "_CisloExterne";
                    #endregion

                    #region Default hodnoty
                    model.Fields.First(p => p.Name == nameof(C_TypBiznisEntity_Kniha_Id)).DefaultValue = (int)TypBiznisEntity_KnihaEnum.Platobne_prikazy;
                    #endregion

                    #region Povinnosti
                    model.Fields.First(p => p.Name == nameof(DatumSplatnosti)).Mandatory = true;
                    #endregion
                    break;
                case TypBiznisEntityEnum.DOL:
                    #region Skryvanie
                    model.Fields.First(p => p.Name == nameof(DatumDokladu)).Text = "_DatumDokladu";
                    model.Fields.First(p => p.Name == nameof(DatumPrijatia)).Text = "_DatumPrijatia";
                    model.Fields.First(p => p.Name == nameof(DatumSplatnosti)).Text = "_DatumSplatnosti";
                    model.Fields.First(p => p.Name == nameof(Predkontacia)).Text = "_Predkontacia";
                    model.Fields.First(p => p.Name == nameof(CisloExterne)).Text = "_CisloExterne";
                    #endregion

                    #region Zmena textu
                    model.Fields.First(p => p.Name == nameof(OsobaKontaktKomu)).Text = "Prevzal";
                    #endregion

                    #region Default hodnoty
                    model.Fields.First(p => p.Name == nameof(DatumVystavenia)).DefaultValue = DateTime.Today;
                    model.Fields.First(p => p.Name == nameof(DatumDodania)).DefaultValue = DateTime.Today;
                    model.Fields.First(p => p.Name == nameof(C_TypBiznisEntity_Kniha_Id)).DefaultValue = (int)TypBiznisEntity_KnihaEnum.Dodacie_listy;
                    #endregion

                    #region Povinnosti
                    model.Fields.First(p => p.Name == nameof(DatumVystavenia)).Mandatory = true;
                    model.Fields.First(p => p.Name == nameof(DatumDodania)).Mandatory = true;

                    model.Fields.First(p => p.Name == nameof(D_Osoba_Id)).Mandatory = true; //Mandatory v combe ide na ID pole
                    #endregion
                    break;
                default:
                    throw new NotImplementedException($"C_TypBiznisEntity_Id {C_TypBiznisEntity_Id} is not implemented"); ;
            }

            #endregion

            if (node.KodPolozky.StartsWith("dms"))
            {
                // zobrazoval to ako "Korespondecnu adresu" z BE aj ked je to z DMS a je to skryte (2 polia s rovnakym nazvom)
                model.Fields.First(p => p.Name == nameof(Adresa)).Text = "_Adresa";
                node.Actions.RemoveAll(x => x.ActionType == NodeActionType.MenuButtonsAll && x.MenuButtons.Any(z => z.ActionType == NodeActionType.FullTextSearch));
            }
            else
            {
                #region Validatory

                #region Disable polí - na základe stavu

                List<string> enblField = new List<string>() { "Predkontacia", "Popis", "Poznamka",
                                                              "OsobaKontaktKomu", "Web", "DovodUkoncenia",
                                                              "CisloOBJ", "CisloDOL", "CisloZML", "CisloFAK",
                                                              "PodpisalMeno", "PodpisalFunkcia" };

                foreach (PfeColumnAttribute col in model.Fields.Where(f => !f.Text.StartsWith("_") && f.Editable && !enblField.Contains(f.Name) &&
                                                                          (!f.ReadOnly || f.Xtype == PfeXType.Combobox || f.Xtype == PfeXType.SearchFieldSS || f.Xtype == PfeXType.SearchFieldMS)))
                {
                    col.Validator ??= new PfeValidator { Rules = new List<PfeRule>() };

                    col.Validator.Rules.Add(new PfeRule
                    {
                        ValidatorType = PfeValidatorType.Disable,
                        Condition = new List<PfeFilterAttribute>
                    {
                        new PfeFilterAttribute
                        {
                            Field = nameof(C_StavEntity_Id),
                            ComparisonOperator = "ne",
                            Value = (int)StavEntityEnum.NOVY
                        }
                    }
                    });
                }

                List<string> disblFieldZau = new() { "Predkontacia", "Popis", "OsobaKontaktKomu", "PodpisalMeno", "PodpisalFunkcia" };

                foreach (PfeColumnAttribute col in model.Fields.Where(f => !f.Text.StartsWith("_") && f.Editable && disblFieldZau.Contains(f.Name) &&
                                                                          (!f.ReadOnly || f.Xtype == PfeXType.Combobox || f.Xtype == PfeXType.SearchFieldSS || f.Xtype == PfeXType.SearchFieldMS)))
                {
                    col.Validator ??= new PfeValidator { Rules = new List<PfeRule>() };

                    col.Validator.Rules.Add(new PfeRule
                    {
                        ValidatorType = PfeValidatorType.Disable,
                        Condition = new List<PfeFilterAttribute>
                    {
                        new PfeFilterAttribute
                        {
                            Field = nameof(C_StavEntity_Id),
                            ComparisonOperator = "gt",
                            Value = (int)StavEntityEnum.SPRACOVANY //hlavne sa jedná o stavy: Vybavený, Zaúčtovaný, Zaúčtovaný RZP, Zaúčtovaný ÚČT
                        }
                    }
                    });
                }

                #endregion

                #region Disable - Kniha

                if (typBiznisEntity != TypBiznisEntityEnum.PDK)
                {
                    var kniha = model.Fields.FirstOrDefault(p => p.Name == nameof(Kniha));
                    if (kniha != null)
                    {
                        kniha.Validator ??= new PfeValidator { Rules = new List<PfeRule>() };
                        kniha.Validator.Rules.Add(new PfeRule
                        {
                            ValidatorType = PfeValidatorType.Disable,
                            Condition = new List<PfeFilterAttribute>
                            {
                                new PfeFilterAttribute
                                {
                                    Field = nameof(DatumVytvorenia),
                                    ComparisonOperator = "ne",
                                    Value = null,
                                    LogicOperator = "AND"
                                }
                            }
                        });
                    }
                }

                #endregion

                #region Disable - ORS na nie novom zázname

                var orsFieldName = typBiznisEntity switch
                {
                    TypBiznisEntityEnum.PPP => nameof(BankaUcetNazov),
                    TypBiznisEntityEnum.BAN => "",//nameof(BankaUcetNazov); //Pole je disablované, takže validátor netreba
                    TypBiznisEntityEnum.PDK => "",//nameof(PokladnicaNazov); //Pole je disablované, takže validátor netreba
                    _ => nameof(StrediskoNazov),
                };

                PfeColumnAttribute orsField = null;
                if (!string.IsNullOrEmpty(orsFieldName))
                {
                    orsField = model.Fields.FirstOrDefault(p => p.Name == orsFieldName);
                }

                if (orsField != null)
                {
                    bool? jednoCislovanie = ((IRepositoryBase)repository).GetTypBiznisEntityNastavView().Where(x => x.C_TypBiznisEntity_Id == (int)node.TypBiznisEntity.First()).FirstOrDefault()?.CislovanieJedno;
                    if (jednoCislovanie == false)
                    {
                        orsField.Validator ??= new PfeValidator { Rules = new List<PfeRule>() };

                        orsField.Validator.Rules.Add(new PfeRule
                        {
                            ValidatorType = PfeValidatorType.Disable,
                            Condition = new List<PfeFilterAttribute>
                        {
                            new PfeFilterAttribute
                            {
                                Field = nameof(DatumVytvorenia),
                                ComparisonOperator = "ne",
                                Value = null,
                                LogicOperator = "AND",
                            }
                        }
                        });
                    }
                }

                #endregion

                #region Zmena referečného dátumu na doklade počas editácie

                if (typBiznisEntity != TypBiznisEntityEnum.IND &&
                    typBiznisEntity != TypBiznisEntityEnum.BAN &&
                    typBiznisEntity != TypBiznisEntityEnum.PPP &&
                    typBiznisEntity != TypBiznisEntityEnum.PDK
                    )
                {
                    var beDatum = model.Fields.FirstOrDefault(p => p.Name == nameof(DatumDokladu));

                    if (beDatum != null)
                    {
                        var tbeNastavenie = ((IRepositoryBase)repository).GetTypBiznisEntityNastavView().Where(x => x.C_TypBiznisEntity_Id == (int)typBiznisEntity).FirstOrDefault();
                        var tbeNastavenieList = new List<(LokalitaEnum lokalita, string datum)>()
                    {
                        (LokalitaEnum.TU, tbeNastavenie.DatumDokladuTU),
                        (LokalitaEnum.TUS, tbeNastavenie.DatumDokladuTU),
                        (LokalitaEnum.EU, tbeNastavenie.DatumDokladuEU),
                        (LokalitaEnum.DV, tbeNastavenie.DatumDokladuDV),
                    };

                        foreach (var tbeNastavDatum in tbeNastavenieList.Where(x => x.datum != nameof(DatumDokladu)).GroupBy(x => x.datum))
                        {
                            beDatum.Validator ??= new PfeValidator();
                            beDatum.Validator.Rules ??= new List<PfeRule>();

                            var rule = new PfeRule
                            {
                                ValidatorType = PfeValidatorType.SetValue,
                                Value = "<" + tbeNastavDatum.Key + ">"
                            };

                            if (tbeNastavDatum.Count() < 4)
                            {
                                rule.Condition = new List<PfeFilterAttribute>();
                                foreach (var (lokalita, datum) in tbeNastavDatum)
                                {
                                    rule.Condition.Add(
                                    new PfeFilterAttribute
                                    {
                                        Field = nameof(C_Lokalita_Id),
                                        ComparisonOperator = "eq",
                                        Value = (int)lokalita,
                                        LogicOperator = "OR",
                                    });
                                }
                            }

                            beDatum.Validator.Rules.Add(rule);
                        }
                    }
                }

                #endregion

                #endregion

                #region SearchFieldDefinition

                model.Fields.FirstOrDefault(p => p.Name == nameof(IdFormatMeno)).SearchFieldDefinition = Helpers.AddIdFormatMeno_SearchFieldDefinition(vztah);
                model.Fields.FirstOrDefault(p => p.Name == nameof(AdresaTPSidlo)).SearchFieldDefinition = Helpers.AddAdresaTPSidlo_SearchFieldDefinition(vztah);

                #endregion

                #region Akcie

                var menuButtAdd = node.Actions.FirstOrDefault(x => x.ActionType == NodeActionType.MenuButtonsAll && x.Caption == "Add");
                if (menuButtAdd != null)
                {
                    menuButtAdd.MenuButtons.Add(new NodeAction(NodeActionType.DKL_NastavPS) { Url = "/office/reg/DKLNastavPS" });

                    if (repository.Session.HasRole("REG_UPRAVA_CISLOVANIA"))
                    {
                        menuButtAdd.MenuButtons.Add(new NodeAction(NodeActionType.DKL_NastavCislo) { Url = "/office/reg/DKLNastavCislo" });
                        menuButtAdd.MenuButtons.Add(new NodeAction(NodeActionType.DKL_VyberCislo) { Url = "/office/reg/DKLVyberCislo" });
                    }

                }

                #endregion

                #region Synchronizacia

                var isoZdroj = repository.GetNastavenieI("reg", "ISOZdroj");
                var isoZdrojNazov = repository.GetNastavenieS("reg", "ISOZdrojNazov");
                if (isoZdroj > 0 && repository.Session.Roles.Where(w => w.Contains("REG_MIGRATOR")).Any() && model.Type != PfeModelType.Form)
                {
                    //node.Actions.RemoveAll(x => x.ActionType == NodeActionType.MenuButtonsAll && x.Caption == "Pridať"); 21.2. - akcie chcem mať
                    if (node.Actions.Any(x => x.ActionType == NodeActionType.MenuButtonsAll))
                    {
                        var polozkaMenuAll = node.Actions.Where(x => x.ActionType == NodeActionType.MenuButtonsAll && x.Caption == "ISO").FirstOrDefault();
                        if (polozkaMenuAll != null)
                        {
                            polozkaMenuAll.Caption = isoZdrojNazov;
                        }
                        //node.Actions.RemoveAll(x => x.ActionType == NodeActionType.Change);
                        //node.Actions.RemoveAll(x => x.ActionType == NodeActionType.Update); Tuto akciu ponechám
                        //node.Actions.RemoveAll(x => x.ActionType == NodeActionType.Delete);
                    }
                }
                else
                {
                    node.Actions.RemoveAll(x => x.ActionType == NodeActionType.MenuButtonsAll && x.Caption == "ISO");
                }

                #endregion

            }

            #region DphRezim a sumačné stĺpce

            if (typBiznisEntity == TypBiznisEntityEnum.IND || typBiznisEntity == TypBiznisEntityEnum.BAN || typBiznisEntity == TypBiznisEntityEnum.PPP)
            {
                model.Fields.First(p => p.Name == nameof(DM_CV)).Text = "_DM_CV";
                model.Fields.First(p => p.Name == nameof(DM_Zak0)).Text = "_DM_Zak0";
                model.Fields.First(p => p.Name == nameof(DM_Zak1)).Text = "_DM_Zak1";
                model.Fields.First(p => p.Name == nameof(DM_Zak2)).Text = "_DM_Zak2";
                model.Fields.First(p => p.Name == nameof(DM_DPH1)).Text = "_DM_DPH1";
                model.Fields.First(p => p.Name == nameof(DM_DPH2)).Text = "_DM_DPH2";

                model.Fields.First(p => p.Name == nameof(CM_CV)).Text = "_CM_CV";
                model.Fields.First(p => p.Name == nameof(CM_Zak0)).Text = "_CM_Zak0";
                model.Fields.First(p => p.Name == nameof(CM_Zak1)).Text = "_CM_Zak1";
                model.Fields.First(p => p.Name == nameof(CM_Zak2)).Text = "_CM_Zak2";
                model.Fields.First(p => p.Name == nameof(CM_DPH1)).Text = "_CM_DPH1";
                model.Fields.First(p => p.Name == nameof(CM_DPH2)).Text = "_CM_DPH2";

                model.Fields.First(p => p.Name == nameof(DM_CV)).ReadOnly = true;
                model.Fields.First(p => p.Name == nameof(DM_Zak0)).ReadOnly = true;
                model.Fields.First(p => p.Name == nameof(DM_Zak1)).ReadOnly = true;
                model.Fields.First(p => p.Name == nameof(DM_Zak2)).ReadOnly = true;
                model.Fields.First(p => p.Name == nameof(DM_DPH1)).ReadOnly = true;
                model.Fields.First(p => p.Name == nameof(DM_DPH2)).ReadOnly = true;

                model.Fields.First(p => p.Name == nameof(CM_CV)).ReadOnly = true;
                model.Fields.First(p => p.Name == nameof(CM_Zak0)).ReadOnly = true;
                model.Fields.First(p => p.Name == nameof(CM_Zak1)).ReadOnly = true;
                model.Fields.First(p => p.Name == nameof(CM_Zak2)).ReadOnly = true;
                model.Fields.First(p => p.Name == nameof(CM_DPH1)).ReadOnly = true;
                model.Fields.First(p => p.Name == nameof(CM_DPH2)).ReadOnly = true;
            }
            else if (typBiznisEntity == TypBiznisEntityEnum.PDK)
            {
                //Needitovateľné a počítané v UpdateDmSumaDokl
                model.Fields.First(p => p.Name == nameof(DM_Suma)).ReadOnly = true;
                model.Fields.First(p => p.Name == nameof(DM_Zak0)).ReadOnly = true;
                model.Fields.First(p => p.Name == nameof(CM_Suma)).ReadOnly = true;
                model.Fields.First(p => p.Name == nameof(CM_Zak0)).ReadOnly = true;

                model.Fields.First(p => p.Name == nameof(DM_Suma)).Editable = false;
                model.Fields.First(p => p.Name == nameof(DM_Zak0)).Editable = false;
                model.Fields.First(p => p.Name == nameof(CM_Suma)).Editable = false;
                model.Fields.First(p => p.Name == nameof(CM_Zak0)).Editable = false;

                //Dočane editovateľné
                model.Fields.First(p => p.Name == nameof(DM_Zak1)).ReadOnly = false;
                model.Fields.First(p => p.Name == nameof(DM_Zak2)).ReadOnly = false;
                model.Fields.First(p => p.Name == nameof(CM_Zak1)).ReadOnly = false;
                model.Fields.First(p => p.Name == nameof(CM_Zak2)).ReadOnly = false;

                //editovateľné
                model.Fields.First(p => p.Name == nameof(DM_CV)).ReadOnly = false;
                model.Fields.First(p => p.Name == nameof(DM_DPH1)).ReadOnly = false;
                model.Fields.First(p => p.Name == nameof(DM_DPH2)).ReadOnly = false;

                model.Fields.First(p => p.Name == nameof(CM_CV)).ReadOnly = false;
                model.Fields.First(p => p.Name == nameof(CM_DPH1)).ReadOnly = false;
                model.Fields.First(p => p.Name == nameof(CM_DPH2)).ReadOnly = false;
            }
            else
            {
                //Needitovateľné a počítané v UpdateDmSumaDokl
                model.Fields.First(p => p.Name == nameof(DM_Suma)).ReadOnly = true;
                model.Fields.First(p => p.Name == nameof(DM_Zak0)).ReadOnly = true;
                model.Fields.First(p => p.Name == nameof(DM_Zak1)).ReadOnly = true;
                model.Fields.First(p => p.Name == nameof(DM_Zak2)).ReadOnly = true;

                model.Fields.First(p => p.Name == nameof(DM_Suma)).Editable = false;
                model.Fields.First(p => p.Name == nameof(DM_Zak0)).Editable = false;
                model.Fields.First(p => p.Name == nameof(DM_Zak1)).Editable = false;
                model.Fields.First(p => p.Name == nameof(DM_Zak2)).Editable = false;

                model.Fields.First(p => p.Name == nameof(CM_Suma)).ReadOnly = true;
                model.Fields.First(p => p.Name == nameof(CM_Zak0)).ReadOnly = true;
                model.Fields.First(p => p.Name == nameof(CM_Zak1)).ReadOnly = true;
                model.Fields.First(p => p.Name == nameof(CM_Zak2)).ReadOnly = true;

                model.Fields.First(p => p.Name == nameof(CM_Suma)).Editable = false;
                model.Fields.First(p => p.Name == nameof(CM_Zak0)).Editable = false;
                model.Fields.First(p => p.Name == nameof(CM_Zak1)).Editable = false;
                model.Fields.First(p => p.Name == nameof(CM_Zak2)).Editable = false;

                //editovateľné
                model.Fields.First(p => p.Name == nameof(DM_CV)).ReadOnly = false;
                model.Fields.First(p => p.Name == nameof(DM_DPH1)).ReadOnly = false;
                model.Fields.First(p => p.Name == nameof(DM_DPH2)).ReadOnly = false;

                model.Fields.First(p => p.Name == nameof(CM_CV)).ReadOnly = false;
                model.Fields.First(p => p.Name == nameof(CM_DPH1)).ReadOnly = false;
                model.Fields.First(p => p.Name == nameof(CM_DPH2)).ReadOnly = false;
            }

            int dphRezim = (int)repository.GetNastavenieI("reg", "RezimDph");

            if (dphRezim.In(0, 1))
            {
                var hideColumns = typBiznisEntity switch
                {
                    TypBiznisEntityEnum.DFA or TypBiznisEntityEnum.DZF or
                    TypBiznisEntityEnum.ODP or TypBiznisEntityEnum.OOB or TypBiznisEntityEnum.OZM or
                    TypBiznisEntityEnum.DCP or
                    TypBiznisEntityEnum.PDK => dphRezim != 1,
                    _ => true,
                };

                if (hideColumns)
                {
                    model.Fields.First(p => p.Name == nameof(DM_Suma)).Text = "Suma celkom";
                    model.Fields.First(p => p.Name == nameof(DM_Zak0)).Text = "Suma";
                    model.Fields.First(p => p.Name == nameof(DM_Zak1)).Text = "_DM_Zak1";
                    model.Fields.First(p => p.Name == nameof(DM_Zak2)).Text = "_DM_Zak2";
                    model.Fields.First(p => p.Name == nameof(DM_DPH1)).Text = "_DM_DPH1";
                    model.Fields.First(p => p.Name == nameof(DM_DPH2)).Text = "_DM_DPH2";

                    model.Fields.First(p => p.Name == nameof(DM_Zak1)).ReadOnly = true;
                    model.Fields.First(p => p.Name == nameof(DM_Zak2)).ReadOnly = true;
                    model.Fields.First(p => p.Name == nameof(DM_DPH1)).ReadOnly = true;
                    model.Fields.First(p => p.Name == nameof(DM_DPH2)).ReadOnly = true;
                }
            }

            #endregion
        }

        public void ApplyOrsPravoToAccesFlags()
        {
            if (OrsPravo < (int)Pravo.Upravovat) AccessFlag &= (long)(~(NodeActionFlag.Update | NodeActionFlag.Change | NodeActionFlag.ZmenaStavu));
            if (OrsPravo < (int)Pravo.Full) AccessFlag &= (long)(~NodeActionFlag.Delete);
        }

        public void SetAccessFlags(Pravo PravoKniha, Pravo? PravoStredisko = null, Pravo? PravoPokladnica = null, Pravo? PravoBankaUcet = null)
        {
            if (PravoKniha < Pravo.Upravovat || (PravoStredisko != null && PravoStredisko < Pravo.Upravovat) || (PravoPokladnica != null && PravoPokladnica < Pravo.Upravovat) || (PravoBankaUcet != null && PravoBankaUcet < Pravo.Upravovat))
            {
                var atList = Enum.GetValues(typeof(NodeActionType)).Cast<NodeActionType>().Where(nat => nat.GetType().GetField(nat.ToString()).FirstAttribute<PfeRightAttribute>().Right == (int)Pravo.Upravovat).ToList();

                NodeAction na = new NodeAction(NodeActionType.AddRight);
                var nn = na.GetNodeActionFlag(NodeActionType.ObnovitZoznamORS);


            }
            else if (PravoKniha < Pravo.Full || (PravoStredisko != null && PravoStredisko < Pravo.Full) || (PravoPokladnica != null && PravoPokladnica < Pravo.Full) || (PravoBankaUcet != null && PravoBankaUcet < Pravo.Full))
            {

            }

            if (C_Stredisko_Id != null)
            {
                if (PravoStredisko == null || PravoStredisko < Pravo.Upravovat) AccessFlag &= (long)(~(NodeActionFlag.Update | NodeActionFlag.Change | NodeActionFlag.ZmenaStavu));
                if (PravoStredisko == null || PravoStredisko < Pravo.Full) AccessFlag &= (long)(~NodeActionFlag.Delete);
            }

            if (C_Pokladnica_Id != null)
            {
                if (PravoPokladnica == null || PravoPokladnica < Pravo.Upravovat) AccessFlag &= (long)(~(NodeActionFlag.Update | NodeActionFlag.Change | NodeActionFlag.ZmenaStavu));
                if (PravoPokladnica == null || PravoPokladnica < Pravo.Full) AccessFlag &= (long)(~NodeActionFlag.Delete);
            }

            if (C_BankaUcet_Id != null)
            {
                if (PravoBankaUcet == null || PravoBankaUcet < Pravo.Upravovat) AccessFlag &= (long)(~(NodeActionFlag.Update | NodeActionFlag.Change | NodeActionFlag.ZmenaStavu));
                if (PravoBankaUcet == null || PravoBankaUcet < Pravo.Full) AccessFlag &= (long)(~NodeActionFlag.Delete);
            }

            if (PravoKniha < Pravo.Upravovat) AccessFlag &= (long)(~(NodeActionFlag.Update | NodeActionFlag.Change | NodeActionFlag.ZmenaStavu));
            if (PravoKniha < Pravo.Full) AccessFlag &= (long)(~NodeActionFlag.Delete);
        }

        public void BeforeGetList(IWebEasRepositoryBase repository, HierarchyNode node, ref string sql, ref Filter filter, ref string sqlFromAlias, string sqlOrderPart)
        {
            if (filter != null)
            {
                //Špeciálny filter na úhrady - treba z filtra odstrániť polia z daní
                filter.ChangeFilterCondition("C_VymerTyp_Id =", "99 =");
            }
        }
    }
}
