﻿using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [DataContract(Name = "Type")]
    public enum NodeActionType
    {
        [EnumMember(Value = "ReadList")]
        [PfeCaption("Read list")]
        [PfeRight(Pravo.Citat)]
        ReadList,

        [EnumMember(Value = "Create")]
        [PfeCaption("New item")]
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
        [PfeCaption("Read")]
        [PfeRight(Pravo.Citat)]
        Read,

        [EnumMember(Value = "Update")]
        [PfeCaption("Save changes")]
        [NodeActionIcon(NodeActionIcons.FloppyO)]
        [PfeRight(Pravo.Upravovat)]
        Update,

        [EnumMember(Value = "Delete")]
        [PfeCaption("Delete")]
        [NodeActionIcon(NodeActionIcons.Trash)]
        [PfeRight(Pravo.Full)]
        Delete,

        [PfeCaption("Edit")]
        [NodeActionIcon(NodeActionIcons.Edit)]
        [PfeRight(Pravo.Upravovat)]
        Change,

        [PfeCaption("Vykonať")]
        [NodeActionIcon(NodeActionIcons.Share)]
        [PfeRight(Pravo.Upravovat)]
        ZmenaStavu,

        [PfeCaption("Restore default setting")]
        [NodeActionIcon(NodeActionIcons.Undo)]
        [PfeRight(Pravo.Upravovat)]
        RefreshDefault,

        [PfeCaption("Show")]
        [PfeRight(Pravo.Citat)]
        MenuButtons,

        [PfeCaption("MenuButtonsAll")]
        [PfeRight(Pravo.Citat)]
        MenuButtonsAll,

        [PfeCaption("Show in actions")]
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

        [PfeCaption("Copy")]
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

        [PfeCaption("Edit")]
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

        #endregion

        #region CRM

        [PfeCaption("Synchronizovať doklady")]
        [NodeActionIcon(NodeActionIcons.SyncAlt)]
        [PfeRight(Pravo.Upravovat)]
        SynchronizovatDoklady,
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

        [PfeCaption("Výkaz pdf")]
        [NodeActionIcon(NodeActionIcons.PieChart)]
        [PfeRight(Pravo.Citat)]
        Vykazf112Pdf,

        [PfeCaption("Export do RISSAM")]
        [NodeActionIcon(NodeActionIcons.FileCsv)]
        [PfeRight(Pravo.Citat)]
        VykazRissam,

        [PfeCaption("Schváliť")]
        [NodeActionIcon(NodeActionIcons.Gavel)]
        [PfeRight(Pravo.Upravovat)]
        Schvalit,

        [PfeCaption("Zrušiť schválenie")]
        [NodeActionIcon(NodeActionIcons.Ban)]
        [PfeRight(Pravo.Upravovat)]
        ZrusitSchvalenie,

        [PfeCaption("Rozpočtový denník")]
        [NodeActionIcon(NodeActionIcons.FilePdfO)]
        [PfeRight(Pravo.Citat)]
        ReportRzpDennik,

        #endregion

        #region BDS

        [PfeCaption("Approve")]
        [NodeActionIcon(NodeActionIcons.CheckSquare)]
        [PfeRight(Pravo.Upravovat)]
        VybavitDoklady = 200,

        [PfeCaption("Cancel approve")]
        [NodeActionIcon(NodeActionIcons.MinusSquare)]
        [PfeRight(Pravo.Upravovat)]
        OdvybavitDoklady = 201,

        #endregion
		
        #region UCT

        [PfeCaption("Účtovný denník")]
        [NodeActionIcon(NodeActionIcons.FilePdfO)]
        [PfeRight(Pravo.Citat)]
        ReportUctDennik,

        [PfeCaption("Náhľad účtovného denníka")]
        [NodeActionIcon(NodeActionIcons.Search)]
        [PfeRight(Pravo.Citat)]
        ViewReportUctDennik,

        [PfeCaption("Hlavná kniha")]
        [NodeActionIcon(NodeActionIcons.FilePdfO)]
        [PfeRight(Pravo.Citat)]
        ReportHlaKniha,

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
        [PfeCaption("Prístupové práva")]
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
        [PfeCaption("Notifikácie")]
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
        [PfeCaption("Stiahnuť <br/> súbor")]
        [NodeActionIcon(NodeActionIcons.Download)]
        [PfeRight(Pravo.Upravovat)]
        DownloadFile = 912,

        [NodeActionIcon(NodeActionIcons.Folder)]
        [EnumMember(Value = "OpenFolder")]
        [PfeCaption("Otvoriť v programe Prieskumník")]
        [PfeRight(Pravo.Citat)]
        OpenFolder = 913,

        #endregion

        #region CFE
        [PfeCaption("Refresh the list of elements")]
        [NodeActionIcon(NodeActionIcons.Refresh)]
        [PfeRight(Pravo.System)]
        ObnovitZoznamORS,

        [PfeCaption("Read tree structure")]
        [NodeActionIcon(NodeActionIcons.Refresh)]
        [PfeRight(Pravo.System)]
        RefreshModuleTree,

        [PfeCaption("Blok user")]
        [NodeActionIcon(NodeActionIcons.UserAltSlash)]
        [PfeRight(Pravo.Upravovat)]
        BlockUser,

        [PfeCaption("Change password")]
        [NodeActionIcon(NodeActionIcons.Password)]
        [PfeRight(Pravo.Upravovat)]
        ChangePassword,

        [PfeCaption("Copy rights from another user")]
        [NodeActionIcon(NodeActionIcons.Friends)]
        [PfeRight(Pravo.Upravovat)]
        CopyUserPermissions,

        [PfeCaption("Add right")]
        [NodeActionIcon(NodeActionIcons.Handshake)]
        [PfeRight(Pravo.Upravovat)]
        AddRight,

        [PfeCaption("Remove right")]
        [NodeActionIcon(NodeActionIcons.Ban)]
        [PfeRight(Pravo.Upravovat)]
        RemoveRight,

        [PfeCaption("No access")]
        [NodeActionIcon(NodeActionIcons.Ban)]
        [PfeRight(Pravo.Upravovat)]
        SetRightNo,

        [PfeCaption("Read")]
        [NodeActionIcon(NodeActionIcons.Eye)]
        [PfeRight(Pravo.Upravovat)]
        SetRightRead,

        [PfeCaption("Edit")]
        [NodeActionIcon(NodeActionIcons.Edit)]
        [PfeRight(Pravo.Upravovat)]
        SetRightUpdate,

        [PfeCaption("Full")]
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
    }
}