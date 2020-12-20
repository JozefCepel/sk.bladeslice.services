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

        [EnumMember(Value = "ZmenaStavu")]
        ZmenaStavu = (long)1 << 9,

        [EnumMember(Value = "Update")]
        Update = (long)1 << 10,

        [EnumMember(Value = "Delete")]
        Delete = (long)1 << 11,

        [EnumMember(Value = "OznameniePreFs")]
        OznameniePreFs = (long)1 << 12,

        [EnumMember(Value = "SpIntegracia")]
        SpIntegracia = (long)1 << 41,

        [EnumMember(Value = "KuIntegracia")]
        KuIntegracia = (long)1 << 42,

        [EnumMember(Value = "FsIntegracia")]
        FsIntegracia = (long)1 << 43,

        [EnumMember(Value = "ZobrazOsobu")]
        ZobrazOsobu = (long)1 << 44,

        #region RZP - zacina od 200

        [EnumMember(Value = "PrevziatNavrhRozpoctu")]
        PrevziatNavrhRozpoctu = (long)1 << 200,

        [EnumMember(Value = "SaveToHistory")]
        SaveToHistory = (long)1 << 202,

        [EnumMember(Value = "Schvalit")]
        Schvalit = (long)1 << 203,

        [EnumMember(Value = "ZrusitSchvalenie")]
        ZrusitSchvalenie = (long)1 << 204,
        #endregion

        #region DMS - zacina od 900

        [EnumMember(Value = "Change")]
        Change = (long)1 << 900,

        [EnumMember(Value = "OpenDocument")]
        OpenDocument = (long)1 << 901,

        [EnumMember(Value = "DownloadFile")]
        DownloadFile = (long)1 << 902,

        [EnumMember(Value = "ItemHistory")]
        ItemHistory = (long)1 << 903,

        [EnumMember(Value = "ItemPermission")]
        ItemPermission = (long)1 << 904,

        [EnumMember(Value = "ItemProperty")]
        ItemProperty = (long)1 << 905,

        [EnumMember(Value = "ItemNotification")]
        ItemNotification = (long)1 << 906,

        #endregion

        #region CFE - zacina od 300
        [EnumMember(Value = "AddRightPermission")]
        AddRight = (long)1 << 302,

        [EnumMember(Value = "RemoveRightPermission")]
        RemoveRight = (long)1 << 303,
        #endregion

        #region REG - zacina od 400

        [EnumMember(Value = "SynchronizovatPrvkyORS")]
        SynchronizovatPrvkyORS = (long)1 << 400,

        #endregion

        #region CRM - zacina od 500

        [EnumMember(Value = "SynchronizovatDoklady")]
        SynchronizovatDoklady = (long)1 << 500,
        #endregion

        [EnumMember(Value = "Predkontovať, skontrolovať")]
        PredkontovatSkontrolovat = (long)1 << 600,

        [EnumMember(Value = "Spracovať")]
        SpracovatDoklad = (long)1 << 601,

        [EnumMember(Value = "Zaúčtovať")]
        ZauctovatDoklad = (long)1 << 602,

        [EnumMember(Value = "Migrácia počiatočného stavu")]
        MigraciaPociatocnehoStavu = (long)1 << 603,

        [EnumMember(Value = "Import zmien rozpočtu")]
        ImportZmienRozpoctu = (long)1 << 604,

        [EnumMember(Value = "Import mesačných pohybov")]
        ImportMesacnychPohybov = (long)1 << 605,

        #region UCT - zacina od 700

        [EnumMember(Value = "Predkontovať")]
        PredkontovatExdDap = (long)1 << 700,

        #endregion

        #region FIN - zacina od 800

        [EnumMember(Value = "Automatické spárovanie úhrad")]
        AutomatickeSparovanieUhrad = (long)1 << 800,

        [EnumMember(Value = "Výber P/Z")]
        VyberPZ = (long)1 << 801,

        #endregion
    }
}
