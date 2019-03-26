using System;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [DataContract(Name = "Flag")]
    [Flags]
    public enum NodeActionFlag : long
    {
        [EnumMember(Value = "None")]
        None = 0,

        [EnumMember(Value = "ZobrazSpis")]
        ZobrazSpis = (long)1 << 0,

        [EnumMember(Value = "ZobrazRozhodnutie")]
        ZobrazRozhodnutie = (long)1 << 1,

        [EnumMember(Value = "ZobrazPodanie")]
        ZobrazPodanie = (long)1 << 2,

        [EnumMember(Value = "ZobrazDetailRiadku")]
        ZobrazDetailRiadku = (long)1 << 3,

        [EnumMember(Value = "ZverejniNaPortali")]
        ZverejniNaPortali = (long)1 << 4,

        [EnumMember(Value = "ZverejniNaUradnejTabuli")]
        ZverejniNaUradnejTabuli = (long)1 << 5,

        [EnumMember(Value = "ZverejniVLokalnejTlaci")]
        ZverejniVLokalnejTlaci = (long)1 << 6,

        [EnumMember(Value = "ZverejniVObecnomRozhlase")]
        ZverejniVObecnomRozhlase = (long)1 << 7,

        [EnumMember(Value = "ZverejniNaFacebooku")]
        ZverejniNaFacebooku = (long)1 << 8,

        [EnumMember(Value = "ZmenaStavuPodania")]
        ZmenaStavuPodania = (long)1 << 9,

        [EnumMember(Value = "Update")]
        Update = (long)1 << 10,

        [EnumMember(Value = "Delete")]
        Delete = (long)1 << 11,

        [EnumMember(Value = "OznameniePreFs")]
        OznameniePreFs = (long)1 << 12,

        [EnumMember(Value = "ManualneParovanieIntDokladu")]
        ManualneParovanieIntDokladu = (long)1 << 13,

        [EnumMember(Value = "VratenieZavazku")]
        VratenieZavazku = (long)1 << 14,

        [EnumMember(Value = "NevytvoreneRozhodnutie")]
        NevytvoreneRozhodnutie = (long)1 << 15,

        [EnumMember(Value = "VytvoritRozhodnutie")]
        VytvoritRozhodnutie = (long)1 << 16,

        [EnumMember(Value = "Vystavene")]
        Vystavene = (long)1 << 17,

        [EnumMember(Value = "NeschvalenyPrikaz")]
        NeschvalenyPrikaz = (long)1 << 18,

        [EnumMember(Value = "SpracovatPriznanie")]
        SpracovatPriznanie = (long)1 << 19,

        [EnumMember(Value = "NeriadnePriznNevytRozh")]
        NeriadnePriznNevytRozh = (long)1 << 20,

        [EnumMember(Value = "VyrubitPoplatok")]
        VyrubitPoplatok = (long)1 << 21,

        [EnumMember(Value = "ZrusitPoplatok")]
        ZrusitPoplatok = (long)1 << 22,

        [EnumMember(Value = "ExportPrikazu")]
        ExportPrikazu = (long)1 << 23,

        [EnumMember(Value = "VytvoreneRozhodnutie")]
        VytvoreneRozhodnutie = (long)1 << 24,

        [EnumMember(Value = "Vykonatelne")]
        Vykonatelne = (long)1 << 25,

        [EnumMember(Value = "EditovatPrikazNaUhradu")]
        EditovatPrikazNaUhradu = (long)1 << 26,

        // Pre vykonatelne rozhodnutie typu DAN a vsetky typy priznania
        [EnumMember(Value = "VyrubitUrok")]
        VyrubitUrok = (long)1 << 27,

        [EnumMember(Value = "TypRozhodnutiaNieODP")]
        TypRozhodnutiaNieODP = (long)1 << 28,

        [EnumMember(Value = "SpracovanieDoruceniek")]
        SpracovanieDoruceniek = (long)1 << 29,

        [EnumMember(Value = "ZmenaSplatok")]
        ZmenaSplatok = (long)1 << 30,

        [EnumMember(Value = "rezervaciaDo")]
        RezervaciaDo = (long)1 << 31,

        [EnumMember(Value = "AktualizovatPrilohy")]
        AktualizovatPrilohy = (long)1 << 32,

        [EnumMember(Value = "VystaveneDan")]
        VystaveneDan = (long)1 << 33,

        [EnumMember(Value = "DoruceneAleboVOdvolani")]
        DoruceneAleboVOdvolani = (long)1 << 34,

        [EnumMember(Value = "BlokovatOdblokovatPriznanie")]
        BlokovatOdblokovatPriznanie = (long)1 << 35,

        [EnumMember(Value = "ExekucneKonanie")]
        ExekucneKonanie = (long)1 << 36,

        [EnumMember(Value = "VyzvaNedoplatok")]
        VyzvaNedoplatok = (long)1 << 37,

        [EnumMember(Value = "VystavitPokutu")]
        VystavitPokutu = (long)1 << 38,

        [EnumMember(Value = "VygenerovatPredpis")]
        VygenerovatPredpis = (long)1 << 39,

        [EnumMember(Value = "StornovatRozhodnutie")]
        StornovatRozhodnutie = (long)1 << 40,

        [EnumMember(Value = "SpIntegracia")]
        SpIntegracia = (long)1 << 41,

        [EnumMember(Value = "KuIntegracia")]
        KuIntegracia = (long)1 << 42,

        [EnumMember(Value = "FsIntegracia")]
        FsIntegracia = (long)1 << 43,

        [EnumMember(Value = "ZobrazOsobu")]
        ZobrazOsobu = (long)1 << 44,

        [EnumMember(Value = "Import")]
        Import = (long)1 << 45,

        [EnumMember(Value = "Create")]
        Create = (long)1 << 46,

        #region BDS - zacina od 50

        [EnumMember(Value = "VybavitDoklady")]
        VybavitDoklady = (long)1 << 50,

        [EnumMember(Value = "OdvybavitDoklady")]
        OdvybavitDoklady = (long)1 << 51,

        #endregion


        #region DMS - zacina od 900

        [EnumMember(Value = "Change")]
        Change = 1 << 3,

        [EnumMember(Value = "OpenDocument")]
        OpenDocument = 1 << 4,

        [EnumMember(Value = "DownloadFile")]
        DownloadFile = 1 << 5,

        [EnumMember(Value = "ItemHistory")]
        ItemHistory = 1 << 8,

        [EnumMember(Value = "ItemPermission")]
        ItemPermission = 1 << 9,

        [EnumMember(Value = "ItemProperty")]
        ItemProperty = 1 << 10,

        [EnumMember(Value = "ItemNotification")]
        ItemNotification = 1 << 11,

        #endregion

    }
}
