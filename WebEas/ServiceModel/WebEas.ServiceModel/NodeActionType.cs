using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [DataContract(Name = "Type")]
    public enum NodeActionType
    {
        [EnumMember(Value = "ReadList")]
        [PfeCaption("Načítať")]
        [PfeRight(Pravo.Citat)]
        ReadList,

        [EnumMember(Value = "Create")]
        [PfeCaption("Nový záznam")]
        [NodeActionIcon(NodeActionIcons.Plus)]
        [PfeRight(Pravo.Upravovat)]
        Create,

        [EnumMember(Value = "DKL_NastavPS")]
        [PfeCaption("Počiatočný stav")]
        [NodeActionIcon(NodeActionIcons.Plus)]
        [PfeRight(Pravo.Upravovat)]
        DKL_NastavPS,

        [EnumMember(Value = "DKL_NastavCislo")]
        [PfeCaption("Nastaviť číslo dokladu")]
        [NodeActionIcon(NodeActionIcons.Plus)]
        [PfeRight(Pravo.Upravovat)]
        DKL_NastavCislo,

        [EnumMember(Value = "DKL_VyberCislo")]
        [PfeCaption("Chýbajúce číslo dokladu")]
        [NodeActionIcon(NodeActionIcons.Plus)]
        [PfeRight(Pravo.Upravovat)]
        DKL_VyberCislo,

        [EnumMember(Value = "Read")]
        [PfeCaption("Načítať")]
        [PfeRight(Pravo.Citat)]
        Read,

        [EnumMember(Value = "Update")]
        [PfeCaption("Uložiť <br/> zmeny")]
        [NodeActionIcon(NodeActionIcons.FloppyO)]
        [PfeRight(Pravo.Upravovat)]
        Update,

        [EnumMember(Value = "Delete")]
        [PfeCaption("Zmazať")]
        [NodeActionIcon(NodeActionIcons.Trash)]
        [PfeRight(Pravo.Full)]
        Delete,

        [PfeCaption("Upraviť")]
        [NodeActionIcon(NodeActionIcons.Edit)]
        [PfeRight(Pravo.Upravovat)]
        Change,

        [PfeCaption("Vykonať")]
        [NodeActionIcon(NodeActionIcons.Share)]
        [PfeRight(Pravo.Upravovat)]
        ZmenaStavu,

        [PfeCaption("Obnoviť predvolené nastavenie")]
        [NodeActionIcon(NodeActionIcons.Undo)]
        [PfeRight(Pravo.Upravovat)]
        RefreshDefault,

        [PfeCaption("Zobraziť")]
        [PfeRight(Pravo.Citat)]
        MenuButtons,

        [PfeCaption("MenuButtonsAll")]
        [PfeRight(Pravo.Citat)]
        MenuButtonsAll,

        [PfeCaption("Zobraziť <br/> v akciách")]
        [PfeRight(Pravo.Citat)]
        ShowInActions,

        [EnumMember(Value = "MultiPdfDownload")]
        [PfeCaption("Stiahnuť <br/> súbor")]
        [NodeActionIcon(NodeActionIcons.FilePdfO)]
        [PfeRight(Pravo.Citat)]
        MultiPdfDownload,

        [PfeCaption("História zmien")]
        [PfeRight(Pravo.Citat)]
        [NodeActionIcon(NodeActionIcons.History)]
        HistoriaZmien,

        [PfeCaption("Kopírovať")]
        [NodeActionIcon(NodeActionIcons.FilesO)]
        [PfeRight(Pravo.Upravovat)]
        Copy,

        [PfeCaption("Zobraziť v DCOM")]
        [NodeActionIcon(NodeActionIcons.Eye)]
        [PfeRight(Pravo.Citat)]
        ZobrazOsobu,

        [EnumMember(Value = "Custom")]
        [PfeRight(Pravo.Citat)]
        Custom,

        [PfeCaption("Tlač <br/> oznámenia")]
        [NodeActionIcon(NodeActionIcons.Print)]
        [PfeRight(Pravo.Upravovat)]
        TlacOznamenia,

        [PfeCaption("Kópia dokladu")]
        [NodeActionIcon(NodeActionIcons.FilesO)]
        [PfeRight(Pravo.Upravovat)]
        CopyMe,

        [PfeCaption("Cielovy doklad")]
        [NodeActionIcon(NodeActionIcons.FilesO)]
        [PfeRight(Pravo.Upravovat)]
        CopyTo,

        [PfeCaption("Export - dod./odb. faktúra")]
        [NodeActionIcon(NodeActionIcons.FilesO)]
        [PfeRight(Pravo.Upravovat)]
        CopyToFA,

        #region Informovanie a poradenstvo

        [PfeCaption("Manuálne <br/> zverejnenie")]
        [NodeActionIcon(NodeActionIcons.Plus)]
        [PfeRight(Pravo.Upravovat)]
        ManualneZverejnenie,

        [PfeCaption("Na portáli")]
        [NodeActionIcon(NodeActionIcons.Globe)]
        [PfeRight(Pravo.Upravovat)]
        ZverejniNaPortali,
        [PfeCaption("Na úradnej tabuli")]
        [NodeActionIcon(NodeActionIcons.Clipboard)]
        [PfeRight(Pravo.Upravovat)]
        ZverejniNaUradnejTabuli,
        [PfeCaption("V lokálnej tlači")]
        [NodeActionIcon(NodeActionIcons.NewspaperO)]
        [PfeRight(Pravo.Upravovat)]
        ZverejniVLokalnejTlaci,
        [PfeCaption("V obecnom rozhlase")]
        [NodeActionIcon(NodeActionIcons.Bullhorn)]
        [PfeRight(Pravo.Upravovat)]
        ZverejniVObecnomRozhlase,
        [PfeCaption("Na facebooku")]
        [NodeActionIcon(NodeActionIcons.FacebookOfficial)]
        [PfeRight(Pravo.Upravovat)]
        ZverejniNaFacebooku,

        #endregion

        #region Licencovanie a povoľovanie

        [PfeCaption("Oznámenie <br/> pre FS")]
        [NodeActionIcon(NodeActionIcons.FileExcelO)]
        [PfeRight(Pravo.Upravovat)]
        OznameniePreFs,

        #endregion

        [PfeCaption("Vyrubiť <br/> poplatok")]
        [NodeActionIcon(NodeActionIcons.Euro)]
        [PfeRight(Pravo.Upravovat)]
        VyrubitPoplatok,

        //[PfeCaption("Zrušiť <br/> poplatok")]
        [PfeCaption("Stornovať <br/> poplatok")]
        [NodeActionIcon(NodeActionIcons.Close)]
        [PfeRight(Pravo.Full)]
        ZrusitPoplatok,

        [PfeCaption("Zisti údaje <br/> vozidla")]
        [NodeActionIcon(NodeActionIcons.Car)]
        [PfeRight(Pravo.Upravovat)]
        ZistitUdajeVozidla,

        [PfeCaption("List vlastníctva")]
        [NodeActionIcon(NodeActionIcons.ListAlt)]
        [PfeRight(Pravo.Citat)]
        LvPozemku,

        #region Pohľadávky DaP

        [PfeCaption("Vytlačiť")]
        [PfeRight(Pravo.Citat)]
        Vytlacit,

        [PfeCaption("Synchronizovať DaP")]
        [NodeActionIcon(NodeActionIcons.SyncAlt)]
        [PfeRight(Pravo.Upravovat)]
        SynchronizaciaRozhodnuti,

        [PfeCaption("Synchronizovať DaP")]
        [NodeActionIcon(NodeActionIcons.SyncAlt)]
        [PfeRight(Pravo.Upravovat)]
        SynchronizaciaCiselnikov,

        [PfeCaption("Zobraziť rozhodnutia v DCOM")]
        [NodeActionIcon(NodeActionIcons.Eye)]
        [PfeRight(Pravo.Upravovat)]
        ZobrazitRozhodnutiaDcom,

        [PfeCaption("Zobraziť číselníky v DCOM")]
        [NodeActionIcon(NodeActionIcons.Eye)]
        [PfeRight(Pravo.Upravovat)]
        ZobrazitCiselnikyDcom,

        [PfeCaption("Zobraziť v DCOM")]
        [NodeActionIcon(NodeActionIcons.Eye)]
        [PfeRight(Pravo.Upravovat)]
        ZobrazitRozhodnutieDcom,

        #endregion

        #region Platby

        [PfeCaption("Úhrada <br/> v hotovosti")]
        [NodeActionIcon(NodeActionIcons.Euro)]
        [PfeRight(Pravo.Upravovat)]
        UhradaVHotovosti,

        [PfeCaption("Nový doklad")]
        [NodeActionIcon(NodeActionIcons.Euro)]
        [PfeRight(Pravo.Upravovat)]
        PridatDoklad,

        [PfeCaption("Tlač <br/> potvrdenia")]
        [NodeActionIcon(NodeActionIcons.Print)]
        [PfeRight(Pravo.Citat)]
        TlacPotvrdenia,

        [PfeCaption("Import bank. výpisu")]
        [NodeActionIcon(NodeActionIcons.Download)]
        [PfeRight(Pravo.Upravovat)]
        Import,

        [PfeCaption("Automatické <br/> spárovanie")]
        [NodeActionIcon(NodeActionIcons.Link)]
        [PfeRight(Pravo.Upravovat)]
        AutoSparovanie,

        [PfeCaption("Manuálne <br/> spárovanie")]
        [NodeActionIcon(NodeActionIcons.Chain)]
        [PfeRight(Pravo.Upravovat)]
        ManuSparovanie,

        [PfeCaption("Manuálne <br/> rozpárovanie")]
        [NodeActionIcon(NodeActionIcons.ChainBroken)]
        [PfeRight(Pravo.Upravovat)]
        ManuRozparovanie,

        [PfeCaption("Úhrada <br/> prevodom")]
        [NodeActionIcon(NodeActionIcons.Bank)]
        [PfeRight(Pravo.Upravovat)]
        UhradaPrevodom,

        [PfeCaption("Vrátenie <br/> v hotovosti")]
        [NodeActionIcon(NodeActionIcons.Euro)]
        [PfeRight(Pravo.Upravovat)]
        VratenieVHotovosti,

        [PfeCaption("Vrátenie <br/> prevodom")]
        [NodeActionIcon(NodeActionIcons.Bank)]
        [PfeRight(Pravo.Upravovat)]
        VrateniePrikazom,

        [PfeCaption("Detail dokladu")]
        [NodeActionIcon(NodeActionIcons.InfoCircle)]
        [PfeRight(Pravo.Citat)]
        DetailPoklDokladu,

        [PfeCaption("Export <br/> do súboru")]
        [NodeActionIcon(NodeActionIcons.Upload)]
        [PfeRight(Pravo.Citat)]
        ExportPrikazuNaUhradu,

        [PfeCaption("Odoslať <br/> na schválenie")]
        [NodeActionIcon(NodeActionIcons.Share)]
        [PfeRight(Pravo.Upravovat)]
        SchvalitPrikazNaUhradu,

        [PfeCaption("Detail <br/> príkazu")]
        [NodeActionIcon(NodeActionIcons.InfoCircle)]
        [PfeRight(Pravo.Citat)]
        DetailPrikazu,

        [PfeCaption("Saldokonto <br/> osoby")]
        [NodeActionIcon(NodeActionIcons.ListAlt)]
        [PfeRight(Pravo.Citat)]
        SaldoKontoOsoby,

        [PfeCaption("Manuálne <br/> spárovanie")]
        [NodeActionIcon(NodeActionIcons.Chain)]
        [PfeRight(Pravo.Upravovat)]
        ManuSparovanieIntDokladu,

        [PfeCaption("Nový príkaz na úhradu")]
        [NodeActionIcon(NodeActionIcons.Plus)]
        [PfeRight(Pravo.Upravovat)]
        NovyPrikazNaUhradu,

        [PfeCaption("Upraviť")]
        [NodeActionIcon(NodeActionIcons.Edit)]
        [PfeRight(Pravo.Upravovat)]
        UpravitPrikazNaUhradu,

        [PfeCaption("Zmazať")]
        [NodeActionIcon(NodeActionIcons.Trash)]
        [PfeRight(Pravo.Full)]
        ZmazatPrikazNaUhradu,

        [PfeCaption("Schváliť príkaz <br/> na úhradu")]
        [NodeActionIcon(NodeActionIcons.Check)]
        [PfeRight(Pravo.Upravovat)]
        SchvalitPrikazDennaAgenda,

        [PfeCaption("Zamietnuť príkaz <br/> na úhradu")]
        [NodeActionIcon(NodeActionIcons.Close)]
        [PfeRight(Pravo.Upravovat)]
        NeschvalitPrikazDennaAgenda,

        [PfeCaption("Preúčtovať")]
        [NodeActionIcon(NodeActionIcons.Random)]
        [PfeRight(Pravo.Upravovat)]
        Preuctovat,

        [PfeCaption("Detail <br/> záväzku")]
        [NodeActionIcon(NodeActionIcons.InfoCircle)]
        [PfeRight(Pravo.Citat)]
        DetailZavazku,

        [PfeCaption("Detail <br/> výpisu")]
        [NodeActionIcon(NodeActionIcons.InfoCircle)]
        [PfeRight(Pravo.Citat)]
        DetailBankVypisu,

        [PfeCaption("Upraviť")]
        [NodeActionIcon(NodeActionIcons.Edit)]
        [PfeRight(Pravo.Upravovat)]
        UpdateNastavenie,

        [PfeCaption("Rezervovať")]
        [NodeActionIcon(NodeActionIcons.Edit)]
        [PfeRight(Pravo.Upravovat)]
        RezervovatPredpis,

        [PfeCaption("Nový záznam")]
        [NodeActionIcon(NodeActionIcons.Plus)]
        [PfeRight(Pravo.Upravovat)]
        CreateBanka,

        [PfeCaption("Nový záznam")]
        [NodeActionIcon(NodeActionIcons.Plus)]
        [PfeRight(Pravo.Upravovat)]
        CreateBankaPolozka,

        #endregion

        #region Migracia

        [EnumMember(Value = "InsertBulk")]
        [PfeRight(Pravo.Upravovat)]
        InsertBulk,

        [EnumMember(Value = "DeleteBulks")]
        [PfeRight(Pravo.Full)]
        DeleteBulks,

        [EnumMember(Value = "ViewReport")]
        [PfeRight(Pravo.Citat)]
        ViewReport,

        [EnumMember(Value = "BusinessValidation")]
        [PfeRight(Pravo.Upravovat)]
        BusinessValidation,

        [PfeCaption("Aktualizovať automatické kontroly")]
        [NodeActionIcon(NodeActionIcons.Refresh)]
        [PfeRight(Pravo.Upravovat)]
        AktualizovatAutKontroly,

        [PfeCaption("Aktualizovať kontrolu")]
        [NodeActionIcon(NodeActionIcons.Refresh)]
        [PfeRight(Pravo.Upravovat)]
        AktualizovatAutKontrolySingle,

        #endregion

        #region Integracia

        [PfeCaption("Zisti údaje <BR/> Dávky zo SP")]
        [EnumMember(Value = "BenefitInfo")]
        [PfeRight(Pravo.Citat)]
        BenefitInfo,

        [PfeCaption("Zisti údaje <br/> O zamestnaní")]
        [EnumMember(Value = "EmploymentStatus")]
        [PfeRight(Pravo.Citat)]
        EmploymentStatus,

        [PfeCaption("Zisti údaje <BR/> O soc. dávkach")]
        [EnumMember(Value = "RsdBenefits")]
        [PfeRight(Pravo.Citat)]
        RsdBenefits,

        [PfeCaption("Zisti údaje <br/> O ŤZP")]
        [EnumMember(Value = "DisabilityStatus")]
        [PfeRight(Pravo.Citat)]
        DisabilityStatus,

        [PfeCaption("Zisti údaje <BR/> o nedoplatkoch z FS")]
        [EnumMember(Value = "DutyAreasInfo")]
        [PfeRight(Pravo.Citat)]
        ZistitUdajeONedoplatkochFS,

        #endregion

        [PfeCaption("Spracovať")]
        [NodeActionIcon(NodeActionIcons.CheckCircle)]
        [PfeRight(Pravo.Upravovat)]
        SpracovatDoklad,

        [PfeCaption("Predkontovať")]
        [NodeActionIcon(NodeActionIcons.Magic)]
        [PfeRight(Pravo.Upravovat)]
        Predkontovat,

        [PfeCaption("Predkontovať")]
        [NodeActionIcon(NodeActionIcons.Magic)]
        [PfeRight(Pravo.Upravovat)]
        PredkontovatExdDap,

        [PfeCaption("Skontrolovať zaúčtovanie")]
        [NodeActionIcon(NodeActionIcons.ThumbsUp)]
        [PfeRight(Pravo.Upravovat)]
        SkontrolovatZauctovanie,

        [PfeCaption("Zaúčtovať")]
        [NodeActionIcon(NodeActionIcons.FlashBolt)]
        [PfeRight(Pravo.Upravovat)]
        ZauctovatDoklad,

        [PfeCaption("Doposlanie úhrad do DCOM-u")]
        [NodeActionIcon(NodeActionIcons.SyncAlt)]
        [PfeRight(Pravo.Upravovat)]
        DoposlanieUhradDoDcomu,

        #region Reg

        [PfeCaption("Zapnúť služby")]
        [NodeActionIcon(NodeActionIcons.Check)]
        [PfeRight(Pravo.Upravovat)]
        EgovSluzbaKonfiguraciaOn,

        [PfeCaption("Vypnúť služby")]
        [NodeActionIcon(NodeActionIcons.UnlockAlt)]
        [PfeRight(Pravo.Upravovat)]
        EgovSluzbaKonfiguraciaOff,

        [PfeCaption("Zapnúť spracovanie v eGov moduloch")]
        [NodeActionIcon(NodeActionIcons.Check)]
        [PfeRight(Pravo.Upravovat)]
        EgovSluzbaKonfiguraciaSpracovanieEgovOn,

        [PfeCaption("Vypnúť spracovanie v eGov moduloch")]
        [NodeActionIcon(NodeActionIcons.UnlockAlt)]
        [PfeRight(Pravo.Upravovat)]
        EgovSluzbaKonfiguraciaSpracovanieEgovOff,

        [PfeCaption("Synchronizovať prvky ORŠ")]
        [NodeActionIcon(NodeActionIcons.SyncAlt)]
        [PfeRight(Pravo.Upravovat)]
        SynchronizovatPrvkyORS = 400,


        #endregion

        #region FIN

        [PfeCaption("Načítať externé doklady")]
        [NodeActionIcon(NodeActionIcons.SyncAlt)]
        [PfeRight(Pravo.Upravovat)]
        NacitatDokladyExterne,

        [PfeCaption("Migrácia počiatočného stavu")]
        [NodeActionIcon(NodeActionIcons.SyncAlt)]
        [PfeRight(Pravo.Upravovat)]
        MigraciaPociatocnehoStavu,

        [PfeCaption("Import zmien rozpočtu")]
        [NodeActionIcon(NodeActionIcons.SyncAlt)]
        [PfeRight(Pravo.Upravovat)]
        ImportZmienRozpoctu,

        [PfeCaption("Import mesačných pohybov")]
        [NodeActionIcon(NodeActionIcons.SyncAlt)]
        [PfeRight(Pravo.Upravovat)]
        ImportMesacnychPohybov,

        [PfeCaption("Automatické spárovanie úhrad")]
        [NodeActionIcon(NodeActionIcons.Random)]
        [PfeRight(Pravo.Upravovat)]
        AutomatickeSparovanieUhrad,

        [PfeCaption("Výber P/Z")]
        [NodeActionIcon(NodeActionIcons.SearchDollar)]
        [PfeRight(Pravo.Upravovat)]
        VyberPZ,

        [PfeCaption("Nový s výberom P/Z")]
        [NodeActionIcon(NodeActionIcons.SearchDollar)]
        [PfeRight(Pravo.Upravovat)]
        VyberPZNovy,

        [PfeCaption("Nový príjmový")]
        [NodeActionIcon(NodeActionIcons.Plus)]
        [PfeRight(Pravo.Upravovat)]
        NovyPrijmovy,

        [PfeCaption("Nový príjmový POS")]
        [NodeActionIcon(NodeActionIcons.FasFaCreditCard)]
        [PfeRight(Pravo.Upravovat)]
        NovyPrijmovyPos,

        [PfeCaption("Nový výdajový")]
        [NodeActionIcon(NodeActionIcons.FasFaMinus)]
        [PfeRight(Pravo.Upravovat)]
        NovyVydajovy,

        [PfeCaption("Pokladničný doklad - pdf")]
        [NodeActionIcon(NodeActionIcons.FilePdfO)]
        [PfeRight(Pravo.Citat)]
        ReportPoklDoklad,

        [PfeCaption("Pokladničný doklad - náhľad")]
        [NodeActionIcon(NodeActionIcons.Search)]
        [PfeRight(Pravo.Citat)]
        ViewReportPoklDoklad,

        [PfeCaption("Pokladničný doklad - tlač")]
        [NodeActionIcon(NodeActionIcons.Print)]
        [PfeRight(Pravo.Citat)]
        PrintReportPoklDoklad,

        [PfeCaption("Pokladničná kniha")]
        [NodeActionIcon(NodeActionIcons.Book)]
        [PfeRight(Pravo.Citat)]
        ReportPoklKniha,

        [PfeCaption("Stornovať doklad")]
        [NodeActionIcon(NodeActionIcons.Close)]
        [PfeRight(Pravo.Full)]
        StornovatDoklad,
        #endregion

        #region CRM

        [PfeCaption("Synchronizovať doklady")]
        [NodeActionIcon(NodeActionIcons.SyncAlt)]
        [PfeRight(Pravo.Upravovat)]
        SynchronizovatDoklady,

        [PfeCaption("Doklad XXX - pdf")]
        [NodeActionIcon(NodeActionIcons.FilePdfO)]
        [PfeRight(Pravo.Citat)]
        ReportDoklad,

        [PfeCaption("Doklad XXX - náhľad")]
        [NodeActionIcon(NodeActionIcons.Search)]
        [PfeRight(Pravo.Citat)]
        ViewReportDoklad,

        [PfeCaption("Doklad XXX - tlač")]
        [NodeActionIcon(NodeActionIcons.Print)]
        [PfeRight(Pravo.Citat)]
        PrintReportDoklad,

        [PfeCaption("Krycí list - pdf")]
        [NodeActionIcon(NodeActionIcons.FilePdfO)]
        [PfeRight(Pravo.Citat)]
        ReportKryciList,

        [PfeCaption("Krycí list - náhľad")]
        [NodeActionIcon(NodeActionIcons.Search)]
        [PfeRight(Pravo.Citat)]
        ViewReportKryciList,

        [PfeCaption("Krycí list - tlač")]
        [NodeActionIcon(NodeActionIcons.Print)]
        [PfeRight(Pravo.Citat)]
        PrintReportKryciList,

        [PfeCaption("Kniha faktúr - pdf")]
        [NodeActionIcon(NodeActionIcons.FilePdfO)]
        [PfeRight(Pravo.Citat)]
        ReportKnihaFaktur,

        [PfeCaption("Kniha faktúr - náhľad")]
        [NodeActionIcon(NodeActionIcons.Search)]
        [PfeRight(Pravo.Citat)]
        ViewReportKnihaFaktur,

        [PfeCaption("Kniha faktúr - tlač")]
        [NodeActionIcon(NodeActionIcons.Print)]
        [PfeRight(Pravo.Citat)]
        PrintReportKnihaFaktur,

        [PfeCaption("Účtovný doklad - pdf")]
        [NodeActionIcon(NodeActionIcons.FilePdfO)]
        [PfeRight(Pravo.Citat)]
        ReportUctovnyDoklad,

        [PfeCaption("Účtovný doklad - náhľad")]
        [NodeActionIcon(NodeActionIcons.Search)]
        [PfeRight(Pravo.Citat)]
        ViewReportUctovnyDoklad,

        [PfeCaption("Účtovný doklad - tlač")]
        [NodeActionIcon(NodeActionIcons.Print)]
        [PfeRight(Pravo.Citat)]
        PrintReportUctovnyDoklad,

        [PfeCaption("Vytvoriť platobný príkaz")]
        [NodeActionIcon(NodeActionIcons.FasFaCommentDollar)]
        [PfeRight(Pravo.Citat)]
        VytvoritPlatPrikaz,

        [PfeCaption("Prehľad faktúr - pdf")]
        [NodeActionIcon(NodeActionIcons.FilePdfO)]
        [PfeRight(Pravo.Citat)]
        ReportPrehladFa,

        [PfeCaption("Prehľad faktúr - náhľad")]
        [NodeActionIcon(NodeActionIcons.Search)]
        [PfeRight(Pravo.Citat)]
        ViewReportPrehladFa,

        [PfeCaption("Prehľad faktúr - tlač")]
        [NodeActionIcon(NodeActionIcons.Print)]
        [PfeRight(Pravo.Citat)]
        PrintReportPrehladFa,

        #endregion

        #region RZP

        [PfeCaption("Prevziať návrh rozpočtu")]
        [NodeActionIcon(NodeActionIcons.FileImport)]
        [PfeRight(Pravo.Upravovat)]
        PrevziatNavrhRozpoctu = 200,

        [PfeCaption("Uložiť do histórie")]
        [NodeActionIcon(NodeActionIcons.History)]
        [PfeRight(Pravo.Upravovat)]
        SaveToHistory,

        //[PfeCaption("Výkaz FIN 1-12 - pdf")]
        [PfeCaption("Výkaz FIN 1-12")]
        [NodeActionIcon(NodeActionIcons.FilePdfO)]
        [PfeRight(Pravo.Citat)]
        ReportVykazF112,

        [PfeCaption("Výkaz FIN 1-12 - náhľad")]
        [NodeActionIcon(NodeActionIcons.Search)]
        [PfeRight(Pravo.Citat)]
        ViewReportVykazF112,

        [PfeCaption("Výkaz FIN 1-12 - tlač")]
        [NodeActionIcon(NodeActionIcons.Print)]
        [PfeRight(Pravo.Citat)]
        PrintReportVykazF112,

        [PfeCaption("Export FIN výkazov do RISSAM")]
        [NodeActionIcon(NodeActionIcons.FileCsv)]
        [PfeRight(Pravo.Citat)]
        ExportFinRissam,

        [PfeCaption("Export rozpočtu do RISSAM")]
        [NodeActionIcon(NodeActionIcons.FileCsv)]
        [PfeRight(Pravo.Citat)]
        ExportRzpRissam,

        [PfeCaption("Schváliť")]
        [NodeActionIcon(NodeActionIcons.Gavel)]
        [PfeRight(Pravo.Upravovat)]
        Schvalit,

        [PfeCaption("Zrušiť schválenie")]
        [NodeActionIcon(NodeActionIcons.Ban)]
        [PfeRight(Pravo.Upravovat)]
        ZrusitSchvalenie,

        [PfeCaption("Rozpočtový denník - pdf")]
        [NodeActionIcon(NodeActionIcons.FilePdfO)]
        [PfeRight(Pravo.Citat)]
        ReportRzpDennik,

        [PfeCaption("Rozpočtový denník - náhľad")]
        [NodeActionIcon(NodeActionIcons.Search)]
        [PfeRight(Pravo.Citat)]
        ViewReportRzpDennik,

        [PfeCaption("Rozpočtový denník - tlač")]
        [NodeActionIcon(NodeActionIcons.Print)]
        [PfeRight(Pravo.Citat)]
        PrintReportRzpDennik,

        #endregion

        #region UCT

        [PfeCaption("Účtovný denník - pdf")]
        [NodeActionIcon(NodeActionIcons.FilePdfO)]
        [PfeRight(Pravo.Citat)]
        ReportUctDennik,

        [PfeCaption("Účtovný denník - náhľad")]
        [NodeActionIcon(NodeActionIcons.Search)]
        [PfeRight(Pravo.Citat)]
        ViewReportUctDennik,

        [PfeCaption("Účtovný denník - tlač")]
        [NodeActionIcon(NodeActionIcons.Print)]
        [PfeRight(Pravo.Citat)]
        PrintReportUctDennik,

        [PfeCaption("Hlavná kniha - pdf")]
        [NodeActionIcon(NodeActionIcons.FilePdfO)]
        [PfeRight(Pravo.Citat)]
        ReportHlaKniha,

        [PfeCaption("Hlavná kniha - náhľad")]
        [NodeActionIcon(NodeActionIcons.Search)]
        [PfeRight(Pravo.Citat)]
        ViewReportHlaKniha,

        [PfeCaption("Hlavná kniha - tlač")]
        [NodeActionIcon(NodeActionIcons.Print)]
        [PfeRight(Pravo.Citat)]
        PrintReportHlaKniha,

        #endregion

        #region DMS

        [EnumMember(Value = "CreateFile")]
        [NodeActionIcon(NodeActionIcons.Plus)]
        [PfeCaption("Nový súbor")]
        [PfeRight(Pravo.Upravovat)]
        CreateFile = 901,

        [EnumMember(Value = "CreateFolder")]
        [NodeActionIcon(NodeActionIcons.Plus)]
        [PfeCaption("Nový adresár")]
        [PfeRight(Pravo.Upravovat)]
        CreateFolder = 902,

        [PfeCaption("Premenovať")]
        [PfeRight(Pravo.Upravovat)]
        Premenovať = 903,

        [NodeActionIcon(NodeActionIcons.Glasses)]
        [EnumMember(Value = "OpenDocument")]
        [PfeCaption("Otvoriť dokument")]
        [PfeRight(Pravo.Citat)]
        OpenDocument = 904,

        [NodeActionIcon(NodeActionIcons.History)]
        [EnumMember(Value = "ItemHistory")]
        [PfeCaption("Zobraziť históriu")]
        [PfeRight(Pravo.Citat)]
        ItemHistory = 905,

        [EnumMember(Value = "ItemPermission")]
        [NodeActionIcon(NodeActionIcons.Key)]
        [PfeCaption("Prístupové práva - rozpracované")]
        [PfeRight(Pravo.Upravovat)]
        ItemPermission = 906,

        [NodeActionIcon(NodeActionIcons.Folder)]
        [EnumMember(Value = "ItemProperty")]
        [PfeCaption("Vlastnosti")]
        [PfeRight(Pravo.Citat)]
        ItemProperty = 907,

        [NodeActionIcon(NodeActionIcons.Users)]
        [EnumMember(Value = "ShowGroupUsers")]
        [PfeCaption("Používatelia v skupine")]
        [PfeRight(Pravo.Citat)]
        ShowGroupUsers = 908,

        [NodeActionIcon(NodeActionIcons.At)]
        [EnumMember(Value = "ItemNotification")]
        [PfeCaption("Notifikácie - rozpracované")]
        [PfeRight(Pravo.Citat)]
        ItemNotification = 909,

        [NodeActionIcon(NodeActionIcons.Search)]
        [EnumMember(Value = "FullTextSearch")]
        [PfeCaption("Vyhľadávanie")]
        [PfeRight(Pravo.Citat)]
        FullTextSearch = 910,

        [NodeActionIcon(NodeActionIcons.Search)]
        [EnumMember(Value = "VaVFullTextSearch")]
        [PfeCaption("Vyhľadávanie")]
        [PfeRight(Pravo.Citat)]
        VaVFullTextSearch = 911,

        [EnumMember(Value = "DownloadFile")]
        [PfeCaption("Stiahnuť dokument")]
        [NodeActionIcon(NodeActionIcons.Download)]
        [PfeRight(Pravo.Upravovat)]
        DownloadFile = 912,

        [NodeActionIcon(NodeActionIcons.Clipboard)]
        [EnumMember(Value = "CopyUrlToClipboard")]
        [PfeCaption("Skopírovať odkaz k adresáru do schránky")]
        [PfeRight(Pravo.Citat)]
        CopyUrlToClipboard = 913,

        [NodeActionIcon(NodeActionIcons.Clipboard)]
        [EnumMember(Value = "CopyPathToClipboard")]
        [PfeCaption("Skopírovať odkaz")]
        [PfeRight(Pravo.Citat)]
        CopyPathToClipboard = 914,

        #endregion

        #region CFE
        [PfeCaption("Obnoviť zoznam ORŠ")]
        [NodeActionIcon(NodeActionIcons.Refresh)]
        [PfeRight(Pravo.System)]
        ObnovitZoznamORS,

        [PfeCaption("Načítať stromovú štruktúru modulu")]
        [NodeActionIcon(NodeActionIcons.Refresh)]
        [PfeRight(Pravo.System)]
        RefreshModuleTree,

        [PfeCaption("Zablokovať používateľa")]
        [NodeActionIcon(NodeActionIcons.UserAltSlash)]
        [PfeRight(Pravo.Upravovat)]
        BlockUser,

        [PfeCaption("Nastaviť heslo")]
        [NodeActionIcon(NodeActionIcons.Password)]
        [PfeRight(Pravo.Upravovat)]
        ChangePassword,

        [PfeCaption("Skopírovať práva iného používateľa")]
        [NodeActionIcon(NodeActionIcons.Friends)]
        [PfeRight(Pravo.Upravovat)]
        CopyUserPermissions,

        [PfeCaption("Pridať právo")]
        [NodeActionIcon(NodeActionIcons.Handshake)]
        [PfeRight(Pravo.Upravovat)]
        AddRight,

        [PfeCaption("Odobrať právo")]
        [NodeActionIcon(NodeActionIcons.Ban)]
        [PfeRight(Pravo.Upravovat)]
        RemoveRight,

        [PfeCaption("Žiadny")]
        [NodeActionIcon(NodeActionIcons.Ban)]
        [PfeRight(Pravo.Upravovat)]
        SetRightNo,

        [PfeCaption("Čítať")]
        [NodeActionIcon(NodeActionIcons.Eye)]
        [PfeRight(Pravo.Upravovat)]
        SetRightRead,

        [PfeCaption("Upravovať")]
        [NodeActionIcon(NodeActionIcons.Edit)]
        [PfeRight(Pravo.Upravovat)]
        SetRightUpdate,

        [PfeCaption("Plný")]
        [NodeActionIcon(NodeActionIcons.Bullhorn)]
        [PfeRight(Pravo.Upravovat)]
        SetRightFull,

        [PfeCaption("Synchronizovať používateľov z DCOM")]
        [NodeActionIcon(NodeActionIcons.SyncAlt)]
        [PfeRight(Pravo.System)]
        SynchronizeDcomUsers,

        #endregion

        #region OSA

        [PfeCaption("Replikácia osoby")]
        [NodeActionIcon(NodeActionIcons.Refresh)]
        [PfeRight(Pravo.Upravovat)]
        ReplikaciaOsoby,

        [PfeCaption("Hromadná replikácia osôb")]
        [NodeActionIcon(NodeActionIcons.Refresh)]
        [PfeRight(Pravo.Upravovat)]
        HromadnaReplikaciaOsob,

        [PfeCaption("Replikovať osobu podľa GUID")]
        [NodeActionIcon(NodeActionIcons.Refresh)]
        [PfeRight(Pravo.Upravovat)]
        ReplikovatOsobuPodlaGuid,

        [PfeCaption("Zobraziť evidenciu v DCOM")]
        [NodeActionIcon(NodeActionIcons.Eye)]
        [PfeRight(Pravo.Citat)]
        ZobrazitEvidenciuDcom,

        #endregion

        #region VYK

        [PfeCaption("Obnoviť predvolenú definíciu")]
        [NodeActionIcon(NodeActionIcons.Undo)]
        [PfeRight(Pravo.Upravovat)]
        ResetDefaultDefinition,

        #endregion
    }
}