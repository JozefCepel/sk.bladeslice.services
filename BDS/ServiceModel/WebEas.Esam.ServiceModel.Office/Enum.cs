using ServiceStack.DataAnnotations;
using System;

namespace WebEas.Esam.ServiceModel.Office
{
    [Flags]
    public enum StavEntityEnum
    {
        STORNOVANY = -2,
        VRATENY_DOD = -1,
        NOVY = 1,
        SCHVALENY = 2,
        NESCHVALENY = 3,
        EXPORTOVANY = 4,
        ODOSLANY = 5,
        UPRAVENY = 6,
        SPRACOVANY = 7,
        VYBAVENY = 8,
        VSCHVALOVANI = 9,
        ZAUCTOVANY = 10,
        PLANOVANA = 11,
        AKTUALNA = 12,
        EXPIROVANA = 13,
        ZAUCTOVANY_RZP = 14,
        ZAUCTOVANY_UCT = 15,
        NEZAUCTOVANY = 16,
        CIASTOCNE_ZAU = 17,
        SILNAZHODA = 18,
        SLABAZHODA = 19,
        PREPLATOK = 20,
        RUCNE = 21
    }

    // TypBiznisEntityEnum sa tusim nedal?

    [Flags]
    public enum TypBiznisEntity_KnihaEnum
    {
        Interne_doklady = 1,
        Dodavatelske_faktury = 2,
        Odberatelske_faktury = 3,
        Prijemky = 5,
        Vydajky = 6,
        Prevodky = 7,
        Bankove_vypisy = 8,
        Odberatelske_dopyty = 9,
        Dodavatelske_dopyty = 10,
        Odberatelske_cenove_ponuky = 11,
        Dodavatelske_cenove_ponuky = 12,
        Odberatelske_objednavky = 13,
        Dodavatelske_objednavky = 14,
        Odberatelske_zmluvy = 15,
        Dodavatelske_zmluvy = 16,
        Odberatelske_zalohove_faktury = 17,
        Dodavatelske_zalohove_faktury = 18,
        Platobne_prikazy = 19,
        Dodacie_listy = 20,
        Prijmove_pokladnicne_doklady = 51,
        Vydajove_pokladnicne_doklady = 52,
        Externe_doklady_DaP = 61,
        Externe_doklady_majetok = 62,
        Externe_doklady_mzdy = 63,
        Externe_doklady_sklad = 64
    }

    [Flags]
    public enum SkupinaPredkontEnum
    {
        Interne_doklady = 1,
        Dodavatelia = 2,
        Odberatelia = 3,
        Bankove_vypisy = 8,
        Pokladnicne_doklady = 50,
        ExtDoklady_DaP = 61,
        ExtDoklady_MJT = 62,
        ExtDoklady_MZD = 63,
        ExtDoklady_SKL = 64,
    }

    [Flags]
    public enum MenaEnum
    {
        Unknown = -999,

        EUR = 978,
        XXX = 999, // ziadna mena
    }

    [Flags]
    public enum OsobaTypEnum
    {
        Fyzicka_osoba = 1,
        Podnikatel = 2,
    }

    // len tie kt sa pouzivaju
    public enum TypEnum
    {
        SumaDokladu = 1,
        SumaKUhrade = 2,
        SumaDebet = 3,
        SumaKredit = 4,
        ZakladDPH = 5,
        DPH = 6,
        ZalohaVSDokladu = 7,
        ZalohaVSZalohy = 8,
        CentVyrovnaniePreplatok = 9,
        CentVyrovnanieNedoplatok = 10,
        CentVyrovnanieHLA = 11,
        Manualne = 12,
        Debet = 101,
        Kredit = 102,
        UhradaDFA = 103,
        UhradaOFA = 104,
        UhradaDZF = 105,
        UhradaOZF = 106,
        DobropisDFA = 107,
        DobropisOFA = 108,
        ZalohyPoskytnute = 115,
        ZalohyPrijate = 116,
        Text = 121,
        UhradaPohZav = 130,
        DanovyVynosDAN = 129,
        DanovyVynosPEN = 131,
        DanovyVynosPOK = 132,
        DanovyVynosURO = 133,
        DanovyVynosODP = 134,
        DanovyVynosONE = 135,
        DanovyVynosDOD = 136,
        DaPUhradaVsetky = 1000,
        DaPUhradaDane = 1001,
        DaPUhradaPokutyZaOneskorenie = 1002,
        DaPUhradaUrokuZOmeskania = 1004,
        DaPUhradaPokuty = 1005,
        DaPUhradaPokutyZaDodatocnePodanie = 1006,
        DaPUhradaUrokuZOdlozeniaSplatok = 1007,
        DAN_Dan = 2001,
        ONE_PokutaZaOneskorenie = 2002,
        PEN_UrokZOmeskania = 2004,
        POK_Pokuta = 2005,
        DOD_PokutaZaDodatocnePodanie = 2006,
        URO_UrokZOdlozeniaSplatok = 2007,
        ODP_OdpisPohladavky = 2010
    }

    [Flags]
    public enum LokalitaEnum
    {
        TU = 1,
        TUS = 2,
        EU = 3,
        DV = 4
    }

    [Flags]
    public enum OrganizaciaTypEnum
    {
        ROZPOCT_ORG_Obec_Mesto = 1, //11-Rozpočtová organizácia; 22-Obec; 23-Mesto
        PRISP_ORG = 2,              //02-Príspevková organizácia
        NEZISK_ORG = 3,             //31-Nezisková organizácia
        PODNIKATEL = 4              //  -Podnikateľ
    }

    [Flags]
    public enum FakturaciaVztahEnum
    {
        NEURCENE = 0,
        DOD = 1,
        ODB = 2,
        DOD_ODB = 3
    }

    public enum DialogTypEnum
    {
        [Description("-")] //(bez dialógu)
        BezDialogu = 0,
        [Description("ReportVykazF112")]
        ReportVykazF112 = 1,
        [Description("VykUct1")] //"Obdobie od" je disablované
        VykUct1 = 2,
        [Description("VykUct2")] //"Obdobie od" je enablované
        VykUct2 = 3,
        [Description("VykUct3")] //"Obdobie od-do" je skryté. Ráta sa od 1-12 za celý rok
        VykUct3 = 4,
        [Description("UctDennik")]
        UctDennik = 11,
        [Description("RzpDennik")]
        RzpDennik = 12,
        [Description("HlavnaKniha")]
        HlavnaKniha = 13,
        [Description("PrehladRzp")]
        PrehladRzp = 14,
        [Description("Dph")]
        Dph = 51,
    }
}
