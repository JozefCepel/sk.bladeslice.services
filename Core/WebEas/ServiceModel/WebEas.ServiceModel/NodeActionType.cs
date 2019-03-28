using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [DataContract(Name = "Type")]
    public enum NodeActionType
    {
        [EnumMember(Value = "ReadList")]
        [PfeCaption("Načítať")]
        ReadList,
        [EnumMember(Value = "Create")]
        [PfeCaption("Nový záznam")]
        [NodeActionIcon(NodeActionIcons.Plus)]
        Create,
        [EnumMember(Value = "Read")]
        [PfeCaption("Načítať")]
        Read,

        [EnumMember(Value = "Update")]
        [PfeCaption("Uložiť <br/> zmeny")]
        [NodeActionIcon(NodeActionIcons.FloppyO)]
        Update,

        [EnumMember(Value = "Delete")]
        [PfeCaption("Zmazať")]
        [NodeActionIcon(NodeActionIcons.Trash)]
        Delete,
        [PfeCaption("Upraviť")]
        [NodeActionIcon(NodeActionIcons.Edit)]
        Change,
        [PfeCaption("Zmena stavu <br/> podania")]
        [NodeActionIcon(NodeActionIcons.Share)]
        ZmenaStavuPodania,
        [PfeCaption("Spis")]
        [NodeActionIcon(NodeActionIcons.Folder)]
        ZobrazSpis,
        [PfeCaption("Asistované podanie")]
        [NodeActionIcon(NodeActionIcons.UserPlus)]
        AsistovanePodanie,
        [PfeCaption("Detail <br/> záznamu")]
        ShowRowDetail,
        [PfeCaption("Rozhodnutie")]
        ZobrazRozhodnutie,
        [PfeCaption("Podanie")]
        [NodeActionIcon(NodeActionIcons.FileTextO)]
        ZobrazEPodPodanie,

        [PfeCaption("Zobraziť")]
        MenuButtons,

        [PfeCaption("MenuButtonsAll")]
        MenuButtonsAll,

        [PfeCaption("Zobraziť <br/> v akciách")]
        ShowInActions,

        [EnumMember(Value = "MultiPdfDownload")]
        [PfeCaption("Stiahnuť <br/> súbor")]
        [NodeActionIcon(NodeActionIcons.FilePdfO)]
        MultiPdfDownload,

        [PfeCaption("Opraviť a uložiť")]
        [NodeActionIcon(NodeActionIcons.FloppyO)]
        ZobrazPodanie,

        [PfeCaption("Zrušiť")]
        [NodeActionIcon(NodeActionIcons.Reply)]
        ZrusitPodanie,

        [PfeCaption("Rozhodnutie")]
        [NodeActionIcon(NodeActionIcons.FileTextO)]
        ZobrazEPodRozhodnutie,

        [PfeCaption("Formulár podania")]
        ZobrazEFormPodanie,

        [PfeCaption("Formulár rozhodnutia")]
        ZobrazEFormRozhodnutie,

        [PfeCaption("Nepublikovať <br/> zmenu")]
        CancelPublish,

        [PfeCaption("Publikovať <br/> zmenu")]
        SubmitPublish,

        [PfeCaption("Uzavrieť požiadavku <br/> na publikovanie")]
        ClosePublish,

        [PfeCaption("História zmien")]
        HistoriaZmien,

        [PfeCaption("Kopírovať")]
        [NodeActionIcon(NodeActionIcons.FilesO)]
        Copy,

        [PfeCaption("Detail žiadateľa")]
        [NodeActionIcon(NodeActionIcons.Folder)]
        ZobrazOsobu,

        [EnumMember(Value = "Custom")]
        Custom,

        [PfeCaption("Tlač <br/> oznámenia")]
        [NodeActionIcon(NodeActionIcons.Print)]
        TlacOznamenia,

        #region Informovanie a poradenstvo

        [PfeCaption("Manuálne <br/> zverejnenie")]
        [NodeActionIcon(NodeActionIcons.Plus)]
        ManualneZverejnenie,

        [PfeCaption("Na portáli")]
        [NodeActionIcon(NodeActionIcons.Globe)]
        ZverejniNaPortali,
        [PfeCaption("Na úradnej tabuli")]
        [NodeActionIcon(NodeActionIcons.Clipboard)]
        ZverejniNaUradnejTabuli,
        [PfeCaption("V lokálnej tlači")]
        [NodeActionIcon(NodeActionIcons.NewspaperO)]
        ZverejniVLokalnejTlaci,
        [PfeCaption("V obecnom rozhlase")]
        [NodeActionIcon(NodeActionIcons.Bullhorn)]
        ZverejniVObecnomRozhlase,
        [PfeCaption("Na facebooku")]
        [NodeActionIcon(NodeActionIcons.FacebookOfficial)]
        ZverejniNaFacebooku,

        #endregion

        #region Licencovanie a povoľovanie

        [PfeCaption("Oznámenie <br/> pre FS")]
        [NodeActionIcon(NodeActionIcons.FileExcelO)]
        OznameniePreFs,

        #endregion
        
        [PfeCaption("Vyrubiť <br/> poplatok")]
        [NodeActionIcon(NodeActionIcons.Euro)]
        VyrubitPoplatok,

        //[PfeCaption("Zrušiť <br/> poplatok")]
        [PfeCaption("Stornovať <br/> poplatok")]
        [NodeActionIcon(NodeActionIcons.Close)]
        ZrusitPoplatok,

        [PfeCaption("Zisti údaje <br/> vozidla")]
        [NodeActionIcon(NodeActionIcons.Car)]
        ZistitUdajeVozidla,

        [PfeCaption("List vlastníctva")]
        [NodeActionIcon(NodeActionIcons.ListAlt)]
        LvPozemku,

        #region Dane a poplatky

        [PfeCaption("Spracovať <br/> priznanie")]
        [NodeActionIcon(NodeActionIcons.CheckSquare)]
        SpracovatPriznanie,
        [PfeCaption("Rozhodnutie")]
        [NodeActionIcon(NodeActionIcons.File)]
        VytvoritRozhodnutie,
        [PfeCaption("Hromadné generovanie základných rozhodnutí")]
        HromadneGenerovanieRozhodnuti,
        [PfeCaption("Odoslať (vyrubiť) <br/> rozhodnutie")]
        [NodeActionIcon(NodeActionIcons.EnvelopeO)]
        OdoslatRozhodnutie,
        [PfeCaption("Vytlačiť")]
        Vytlacit,
        [PfeCaption("Zadať dátum doručenia")]
        ZadatDatumDorucenia,
        [PfeCaption("Úprava splátkového kalendára")]
        UpravaSplatkovehoKalendara,
        [PfeCaption("Vygenerovať predpis")]
        [NodeActionIcon(NodeActionIcons.Euro)]
        VygenerovatPredpis,
        [PfeCaption("Výzva")]
        Vyzva,
        [PfeCaption("Pokuta")]
        Pokuta,
        [PfeCaption("Úroky z omeškania")]
        UrokyZOmeskania,
        [PfeCaption("Daňové exekučné konanie")]
        DanoveExekucneKonanie,
        [PfeCaption("Nevymožiteľné")]
        Nevymozitelne,
        [PfeCaption("Prepočítať")]
        [NodeActionIcon(NodeActionIcons.Calculator)]
        Prepocitat,
        [PfeCaption("Detail priznania")]
        [NodeActionIcon(NodeActionIcons.FileText)]
        ZobrazPriznanie,
        [PfeCaption("Skontrolovať")]
        [NodeActionIcon(NodeActionIcons.CheckSquareO)]
        Skontrolovat,
        [PfeCaption("Detail")]
        [NodeActionIcon(NodeActionIcons.InfoCircle)]
        RozhodnutieDetail,
        [PfeCaption("Detail priznania")]
        [NodeActionIcon(NodeActionIcons.FileText)]
        ZobrazPriznanie4Dan,
        [PfeCaption("Detail prílohy")]
        [NodeActionIcon(NodeActionIcons.InfoCircle)]
        ZobrazPriznanieDetail4Dan,
        [PfeCaption("Posúdenie odvolania")]
        [NodeActionIcon(NodeActionIcons.Retweet)]
        PosudenieOdvolania,
        [PfeCaption("Upravenie splátkového kalendára")]
        UpravenieSplatkovehoKalendara,
        [PfeCaption("Vytvorenie výzvy na zaplatenie nedoplatku")]
        VytvorenieVyzvyNedoplatok,
        [PfeCaption("Vytvoriť úrok z omeškania")]
        [NodeActionIcon(NodeActionIcons.ThumbsDown)]
        VytvorenieRozhodnutiaUroky,
        [PfeCaption("Vytvoriť úrok z odkladu splátok")]
        [NodeActionIcon(NodeActionIcons.ThumbsUp)]
        VytvorenieRozhodnutiaUrokyZOdkladu,
        [PfeCaption("Vytvoriť pokutu")]
        [NodeActionIcon(NodeActionIcons.ThumbsODown)]
        VytvorenieRozhodnutiaPokuta,
        [PfeCaption("Označenie nevymožiteľného rozhodnutia")]
        OznacenieNevymozitelneho,
        [PfeCaption("Detail prílohy")]
        [NodeActionIcon(NodeActionIcons.InfoCircle)]
        ZobrazPrilohaOstatneDane,
        [PfeCaption("Detail prílohy")]
        [NodeActionIcon(NodeActionIcons.InfoCircle)]
        ZobrazPrilohaOsoby,
        [PfeCaption("Detail prílohy")]
        [NodeActionIcon(NodeActionIcons.InfoCircle)]
        ZobrazPrilohaNadoby,
        [PfeCaption("Detail prílohy")]
        [NodeActionIcon(NodeActionIcons.InfoCircle)]
        ZobrazPrilohaZamestnanci,
        [PfeCaption("Nové priznanie")]
        [NodeActionIcon(NodeActionIcons.Plus)]
        NovePriznanie,
        [PfeCaption("Nová príloha")]
        [NodeActionIcon(NodeActionIcons.Plus)]
        NovaPriloha,
        [PfeCaption("Spracovať doručenky")]
        [NodeActionIcon(NodeActionIcons.Calendar)]
        SpracovatDorucenky,
        [PfeCaption("Nová príloha")]
        [NodeActionIcon(NodeActionIcons.Plus)]
        OsobaNovaPriloha,
        [PfeCaption("Rozdeliť prílohu")]
        [NodeActionIcon(NodeActionIcons.Cut)]
        RozdelitPriloha,
        [PfeCaption("Vymazať prílohu")]
        VymazatPrilohu,
        [PfeCaption("Nová príloha")]
        [NodeActionIcon(NodeActionIcons.Plus)]
        ZamNovaPriloha,
        [PfeCaption("Nová príloha")]
        [NodeActionIcon(NodeActionIcons.Plus)]
        NadobaNovaPriloha,
        [PfeCaption("Detail")]
        [NodeActionIcon(NodeActionIcons.InfoCircle)]
        VyzvaDetail,
        [PfeCaption("Odoslať (vyrubiť) <br/> výzvu")]
        [NodeActionIcon(NodeActionIcons.EnvelopeO)]
        OdoslatVyzvu,
        [PfeCaption("Vytvoriť výzvu <br/> k nedoplatku")]
        [NodeActionIcon(NodeActionIcons.Bullhorn)]
        VyzvyNaZaplatenieNedoplatku,
        [PfeCaption("Detail")]
        [NodeActionIcon(NodeActionIcons.InfoCircle)]
        ZobrazitZiadost,
        [PfeCaption("Nová príloha")]
        [NodeActionIcon(NodeActionIcons.Plus)]
        DznNovaPriloha,
        [PfeCaption("Nová príloha")]
        [NodeActionIcon(NodeActionIcons.Plus)]
        OstHisNovaPriloha,
        [PfeCaption("Zmeniť splátky")]
        [NodeActionIcon(NodeActionIcons.PieChart)]
        ZmenaSplatok,
        [PfeCaption("Nová príloha")]
        [NodeActionIcon(NodeActionIcons.Plus)]
        OstUvpNovaPriloha,
        [PfeCaption("Zmena priznania")]
        [NodeActionIcon(NodeActionIcons.PencilSquareO)]
        ZmenaPriznania,
        [PfeCaption("Odpísať pohľadávku")]
        [NodeActionIcon(NodeActionIcons.Ban)]
        OdpisPohladavky,
        [PfeCaption("Aktualizovať prílohy")]
        [NodeActionIcon(NodeActionIcons.Refresh)]
        AktualizovatPrilohy,
        [PfeCaption("Blokovať/odblokovať <br/> priznanie")]
        [NodeActionIcon(NodeActionIcons.UnlockAlt)]
        BlokovatOdblokovatPriznanie,
        [PfeCaption("Zmeniť odsek")]
        [NodeActionIcon(NodeActionIcons.Exchange)]
        DznZmenitOdsek,
        [PfeCaption("Exekučné konanie")]
        [NodeActionIcon(NodeActionIcons.Gavel)]
        ExekucneKonanie,
        [PfeCaption("Nový druh dane")]
        [NodeActionIcon(NodeActionIcons.Plus)]
        NovyDruhDane,
        [PfeCaption("Nová sadzba")]
        [NodeActionIcon(NodeActionIcons.Plus)]
        NovaSadzbaDane,
        [PfeCaption("Nový bankový účet")]
        [NodeActionIcon(NodeActionIcons.Plus)]
        NovyBankovyUcet,
        [PfeCaption("Stornovať rozhodnutie")]
        [NodeActionIcon(NodeActionIcons.ChainBroken)]
        StornovatRozhodnutie,
        [PfeCaption("Sumár")]
        [NodeActionIcon(NodeActionIcons.Euro)]
        Sumar,
        [PfeCaption("Sumár za účtovný rok")]
        [NodeActionIcon(NodeActionIcons.Calculator)]
        SumarZaRok,
        [PfeCaption("Preplatky")]
        [NodeActionIcon(NodeActionIcons.Euro)]
        Preplatky,
        [PfeCaption("Nedoplatky")]
        [NodeActionIcon(NodeActionIcons.Euro)]
        Nedoplatky,
        [PfeCaption("Inkaso")]
        [NodeActionIcon(NodeActionIcons.Euro)]
        Inkaso,
        [PfeCaption("Zoznam nehnuteľností")]
        [NodeActionIcon(NodeActionIcons.Home)]
        ZoznamNehnutelnosti,
        [PfeCaption("Nehnuteľnosti s úľavou")]
        [NodeActionIcon(NodeActionIcons.Home)]
        NehnutelnostisUlavou,
        [PfeCaption("Nehnuteľnosti s oslobodením")]
        [NodeActionIcon(NodeActionIcons.Home)]
        NehnutelnostisOslobodenim,
        [PfeCaption("Výkaz DZN")]
        [NodeActionIcon(NodeActionIcons.Archive)]
        VykazODani,
        [PfeCaption("Celkový prehľad")]
        [NodeActionIcon(NodeActionIcons.PieChart)]
        CelkovyPrehlad,
        [PfeCaption("Integrácia s účtovníctvom")]
        [NodeActionIcon(NodeActionIcons.Handshake)]
        IntegraciaSUctovnictvom,
        [PfeCaption("VZN")]
        [NodeActionIcon(NodeActionIcons.Gavel)]
        VZN,
        [PfeCaption("Nastavenia DaP")]
        [NodeActionIcon(NodeActionIcons.Wrench)]
        NastaveniaDaP,
        [PfeCaption("Vyhľadať <br/> vlastníka")]
        [NodeActionIcon(NodeActionIcons.Wrench)]
        SparovanieOsob,
        [PfeCaption("Preniesť limity do roku")]
        [NodeActionIcon(NodeActionIcons.Clone)]
        PreniestLimityDoRoku,
        [PfeCaption("Nový limit sadzby")]
        [NodeActionIcon(NodeActionIcons.Plus)]
        NovyLimit,
        [PfeCaption("Kopírovať")]
        [NodeActionIcon(NodeActionIcons.FilesO)]
        CopyRowLimit,
        [PfeCaption("Uložiť <br/> zmeny")]
        [NodeActionIcon(NodeActionIcons.FloppyO)]
        UpdateSadzba,
        [PfeCaption("Upraviť")]
        [NodeActionIcon(NodeActionIcons.Edit)]
        ChangeSadzba,
        [PfeCaption("Kopírovať")]
        [NodeActionIcon(NodeActionIcons.FilesO)]
        CopyRowSadzba,
        [PfeCaption("Preniesť priznania <br/> do roku")]
        [NodeActionIcon(NodeActionIcons.Clone)]
        PreniestPriznaniaDoRoku,
        [PfeCaption("Upraviť")]
        [NodeActionIcon(NodeActionIcons.Edit)]
        ChangeLimit,
        [PfeCaption("Preniesť číselníky do roku")]
        [NodeActionIcon(NodeActionIcons.Clone)]
        PreniestCiselnikyDoRoku,
        [PfeCaption("Kopírovať")]
        [NodeActionIcon(NodeActionIcons.FilesO)]
        CopyRowCisKat,
        [PfeCaption("Upraviť")]
        [NodeActionIcon(NodeActionIcons.Edit)]
        UpdateCisKat,
        [PfeCaption("Nová kategória")]
        [NodeActionIcon(NodeActionIcons.Plus)]
        CreateCisKat,
        [PfeCaption("Kopírovať")]
        [NodeActionIcon(NodeActionIcons.FilesO)]
        CopyRowBankovyUcet,
        [PfeCaption("Upraviť")]
        [NodeActionIcon(NodeActionIcons.Edit)]
        ChangeBankovyUcet,
        [PfeCaption("Upraviť")]
        [NodeActionIcon(NodeActionIcons.Edit)]
        ChangeSplatKal,
        [PfeCaption("Nový splátkový kalendár")]
        [NodeActionIcon(NodeActionIcons.Plus)]
        CreateSplatKal,
        [PfeCaption("Kopírovať")]
        [NodeActionIcon(NodeActionIcons.FilesO)]
        CopyRowSplatKal,
        [PfeCaption("Zmazať")]
        [NodeActionIcon(NodeActionIcons.Trash)]
        DeleteCisKat,
        [PfeCaption("Kopírovať")]
        [NodeActionIcon(NodeActionIcons.FilesO)]
        CopyRowTextacie,
        [PfeCaption("Kopírovať")]
        [NodeActionIcon(NodeActionIcons.FilesO)]
        CopyRowPriradenieSadzieb,
        [PfeCaption("Kopírovať")]
        [NodeActionIcon(NodeActionIcons.FilesO)]
        CopyRowTextacieVzn,
        [PfeCaption("Nové priznanie - Riadne")]
        [NodeActionIcon(NodeActionIcons.Plus)]
        RiadnePriznanie,
        [PfeCaption("Nové priznanie - Zmenové")]
        [NodeActionIcon(NodeActionIcons.PencilSquareO)]
        ZmenovePriznanie,
        [PfeCaption("Návrh rozhodnutia")]
        [NodeActionIcon(NodeActionIcons.FilePdfO)]
        MultiPdfDownloadNavrhVyzva,
        [PfeCaption("Nová príloha")]
        [NodeActionIcon(NodeActionIcons.Plus)]
        OstRozvNovaPriloha,
        [PfeCaption("Zmazať")]
        [NodeActionIcon(NodeActionIcons.Trash)]
        DeletePriznPril,
        [PfeCaption("Kopírovať")]
        [NodeActionIcon(NodeActionIcons.FilesO)]
        CopyRowCastObce,
        [PfeCaption("Nové priznanie - Riadne")]
        [NodeActionIcon(NodeActionIcons.Plus)]
        StvorDanRiadnePriznanie,
        [PfeCaption("Nové priznanie - Zmenové")]
        [NodeActionIcon(NodeActionIcons.PencilSquareO)]
        StvorDanZmenovePriznanie,
        [PfeCaption("Presunúť prílohu")]
        [NodeActionIcon(NodeActionIcons.FilesO)]
        PresunutPrilohu,
        [PfeCaption("Osoby podľa pobytu")]
        [NodeActionIcon(NodeActionIcons.Plus)]
        OsobyPodlaPobytu,
        [PfeCaption("Hromadné zmeny príloh")]
        [NodeActionIcon(NodeActionIcons.Bolt)]
        HromadneZmenyPriloh,
        [PfeCaption("Priznanie vzor MFSR")]
        [NodeActionIcon(NodeActionIcons.AddressCard)]
        PriznanieVzorMFSR,
        [PfeCaption("Kopírovať")]
        [NodeActionIcon(NodeActionIcons.FilesO)]
        CopyCisOslobodenie,
        [PfeCaption( "Kontrola duplicít RČ" )]
        [NodeActionIcon( NodeActionIcons.Badge )]
        KontrolaDuplicitRC,
        [PfeCaption("Archivovať výkaz")]
        [NodeActionIcon(NodeActionIcons.History)]
        UlozDoHistorieF112,
        #endregion

        #region Platby

        [PfeCaption("Úhrada <br/> v hotovosti")]
        [NodeActionIcon(NodeActionIcons.Euro)]
        UhradaVHotovosti,

        [PfeCaption("Nový doklad")]
        [NodeActionIcon(NodeActionIcons.Euro)]
        PridatDoklad,

        [PfeCaption("Tlač <br/> potvrdenia")]
        [NodeActionIcon(NodeActionIcons.Print)]
        TlacPotvrdenia,

        [PfeCaption("Import bank. výpisu")]
        [NodeActionIcon(NodeActionIcons.Download)]
        Import,

        [PfeCaption("Automatické <br/> spárovanie")]
        [NodeActionIcon(NodeActionIcons.Link)]
        AutoSparovanie,

        [PfeCaption("Manuálne <br/> spárovanie")]
        [NodeActionIcon(NodeActionIcons.Chain)]
        ManuSparovanie,

        [PfeCaption("Manuálne <br/> rozpárovanie")]
        [NodeActionIcon(NodeActionIcons.ChainBroken)]
        ManuRozparovanie,

        [PfeCaption("Úhrada <br/> prevodom")]
        [NodeActionIcon(NodeActionIcons.Bank)]
        UhradaPrevodom,

        [PfeCaption("Vrátenie <br/> v hotovosti")]
        [NodeActionIcon(NodeActionIcons.Euro)]
        VratenieVHotovosti,

        [PfeCaption("Vrátenie <br/> prevodom")]
        [NodeActionIcon(NodeActionIcons.Bank)]
        VrateniePrikazom,

        [PfeCaption("Detail dokladu")]
        [NodeActionIcon(NodeActionIcons.InfoCircle)]
        DetailPoklDokladu,

        [PfeCaption("Export <br/> do súboru")]
        [NodeActionIcon(NodeActionIcons.Upload)]
        ExportPrikazuNaUhradu,

        [PfeCaption("Odoslať <br/> na schválenie")]
        [NodeActionIcon(NodeActionIcons.Share)]
        SchvalitPrikazNaUhradu,

        [PfeCaption("Detail <br/> príkazu")]
        [NodeActionIcon(NodeActionIcons.InfoCircle)]
        DetailPrikazu,

        [PfeCaption("Saldokonto <br/> osoby")]
        [NodeActionIcon(NodeActionIcons.ListAlt)]
        SaldoKontoOsoby,

        [PfeCaption("Manuálne <br/> spárovanie")]
        [NodeActionIcon(NodeActionIcons.Chain)]
        ManuSparovanieIntDokladu,

        [PfeCaption("Nový príkaz na úhradu")]
        [NodeActionIcon(NodeActionIcons.Plus)]
        NovyPrikazNaUhradu,

        [PfeCaption("Upraviť")]
        [NodeActionIcon(NodeActionIcons.Edit)]
        UpravitPrikazNaUhradu,

        [PfeCaption("Zmazať")]
        [NodeActionIcon(NodeActionIcons.Trash)]
        ZmazatPrikazNaUhradu,

        [PfeCaption("Schváliť príkaz <br/> na úhradu")]
        [NodeActionIcon(NodeActionIcons.Check)]
        SchvalitPrikazDennaAgenda,

        [PfeCaption("Zamietnuť príkaz <br/> na úhradu")]
        [NodeActionIcon(NodeActionIcons.Close)]
        NeschvalitPrikazDennaAgenda,

        [PfeCaption("Preúčtovať")]
        [NodeActionIcon(NodeActionIcons.Random)]
        Preuctovat,

        [PfeCaption("Detail <br/> záväzku")]
        [NodeActionIcon(NodeActionIcons.InfoCircle)]
        DetailZavazku,

        [PfeCaption("Detail <br/> výpisu")]
        [NodeActionIcon(NodeActionIcons.InfoCircle)]
        DetailBankVypisu,

        [PfeCaption("Upraviť")]
        [NodeActionIcon(NodeActionIcons.Edit)]
        UpdateNastavenie,

        [PfeCaption("Rezervovať")]
        [NodeActionIcon(NodeActionIcons.Edit)]
        RezervovatPredpis,

        [PfeCaption("Nový záznam")]
        [NodeActionIcon(NodeActionIcons.Plus)]
        CreateBanka,

        [PfeCaption("Nový záznam")]
        [NodeActionIcon(NodeActionIcons.Plus)]
        CreateBankaPolozka,

        #endregion

        #region Migracia

        [EnumMember(Value = "InsertBulk")]
        InsertBulk,

        [EnumMember(Value = "DeleteBulks")]
        DeleteBulks,

        [EnumMember(Value = "ViewReport")]
        ViewReport,

        [EnumMember(Value = "BusinessValidation")]
        BusinessValidation,

        [PfeCaption("Aktualizovať automatické kontroly")]
        [NodeActionIcon(NodeActionIcons.Refresh)]
        AktualizovatAutKontroly,

        [PfeCaption("Aktualizovať kontrolu")]
        [NodeActionIcon(NodeActionIcons.Refresh)]
        AktualizovatAutKontrolySingle,

        #endregion

        #region Integracia

        [PfeCaption("Zisti údaje <BR/> Dávky zo SP")]
        [EnumMember(Value = "BenefitInfo")]
        BenefitInfo,

        [PfeCaption("Zisti údaje <br/> O zamestnaní")]
        [EnumMember(Value = "EmploymentStatus")]
        EmploymentStatus,

        [PfeCaption("Zisti údaje <BR/> O soc. dávkach")]
        [EnumMember(Value = "RsdBenefits")]
        RsdBenefits,

        [PfeCaption("Zisti údaje <br/> O ŤZP")]
        [EnumMember(Value = "DisabilityStatus")]
        DisabilityStatus,

        [PfeCaption("Zisti údaje <BR/> o nedoplatkoch z FS")]        
        [EnumMember(Value = "DutyAreasInfo")]
        ZistitUdajeONedoplatkochFS,

        #endregion

        #region Reg

        [PfeCaption("Zapnúť služby")]
        [NodeActionIcon(NodeActionIcons.Check)]
        EgovSluzbaKonfiguraciaOn,

        [PfeCaption("Vypnúť služby")]
        [NodeActionIcon(NodeActionIcons.UnlockAlt)]
        EgovSluzbaKonfiguraciaOff,

        [PfeCaption("Zapnúť spracovanie v eGov moduloch")]
        [NodeActionIcon(NodeActionIcons.Check)]
        EgovSluzbaKonfiguraciaSpracovanieEgovOn,

        [PfeCaption("Vypnúť spracovanie v eGov moduloch")]
        [NodeActionIcon(NodeActionIcons.UnlockAlt)]
        EgovSluzbaKonfiguraciaSpracovanieEgovOff,

        [PfeCaption("Protokol z kontrol synchronizácie")]
        [NodeActionIcon(NodeActionIcons.FilePdfO)]
        ZostavaRefreshMigCheck,

        #endregion


        #region BDS

        [PfeCaption("Vybaviť")]
        [NodeActionIcon(NodeActionIcons.CheckSquare)]
        VybavitDoklady = 200,

        [PfeCaption("Odvybaviť")]
        [NodeActionIcon(NodeActionIcons.Square)]
        OdvybavitDoklady = 201,

        #endregion

        #region DMS

        [EnumMember(Value = "CreateFile")]
        [NodeActionIcon(NodeActionIcons.Plus)]
        [PfeCaption("Nový súbor")]
        CreateFile = 901,

        [EnumMember(Value = "CreateFolder")]
        [NodeActionIcon(NodeActionIcons.Plus)]
        [PfeCaption("Nový adresár")]
        CreateFolder = 902,

        [PfeCaption("Premenovať")]
        Premenovať = 903,

        [NodeActionIcon(NodeActionIcons.Link)]
        [EnumMember(Value = "OpenDocument")]
        [PfeCaption("Otvoriť dokument")]
        OpenDocument = 904,

        [NodeActionIcon(NodeActionIcons.NewspaperO)]
        [EnumMember(Value = "ItemHistory")]
        [PfeCaption("Zobraziť históriu")]
        ItemHistory = 905,

        [EnumMember(Value = "ItemPermission")]
        [NodeActionIcon(NodeActionIcons.Eye)]
        [PfeCaption("Prístupové práva")]
        ItemPermission = 906,

        [NodeActionIcon(NodeActionIcons.Folder)]
        [EnumMember(Value = "ItemProperty")]
        [PfeCaption("Vlastnosti")]
        ItemProperty = 907,

        [NodeActionIcon(NodeActionIcons.Users)]
        [EnumMember(Value = "ShowGroupUsers")]
        [PfeCaption("Používatelia v skupine")]
        ShowGroupUsers = 908,

        [NodeActionIcon(NodeActionIcons.Email)]
        [EnumMember(Value = "ItemNotification")]
        [PfeCaption("Notifikácie")]
        ItemNotification = 909,

        [NodeActionIcon(NodeActionIcons.Search)]
        [EnumMember(Value = "FullTextSearch")]
        [PfeCaption("Vyhľadávanie")]
        FullTextSearch = 910,

        [NodeActionIcon(NodeActionIcons.Search)]
        [EnumMember(Value = "VaVFullTextSearch")]
        [PfeCaption("Vyhľadávanie")]
        VaVFullTextSearch = 911,

        [EnumMember(Value = "DownloadFile")]
        [PfeCaption("Stiahnuť <br/> súbor")]
        [NodeActionIcon(NodeActionIcons.Download)]
        DownloadFile = 912,



        #endregion
    }
}