using System;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [DataContract(Name = "Icons")]
    public enum NodeActionIcons
    {
        //[EnumMember(Value = "0xf016")]
        [EnumMember(Value = "")]
        Default = 0,

        [EnumMember(Value = "0xf06e")]
        Eye = 1,

        [EnumMember(Value = "0xf0b0")]
        Filter = 2,

        [EnumMember(Value = "0xf112")]
        Reply = 3,

        [EnumMember(Value = "0xf05a")]
        InfoCircle = 4,

        [EnumMember(Value = "0xf067")]
        Plus = 5,

        [EnumMember(Value = "0xf044")]
        Edit = 6,

        [EnumMember(Value = "0xf1f8")]
        Trash = 7,

        [EnumMember(Value = "0xf234")]
        UserPlus = 8,

        [EnumMember(Value = "0xf0c7")]
        FloppyO = 9,

        [EnumMember(Value = "0xf129")]
        Info = 10,

        [EnumMember(Value = "0xf0f6")]
        FileTextO = 11,

        [EnumMember(Value = "0xf07b")]
        Folder = 12,

        [EnumMember(Value = "0xf153")]
        Euro = 13,

        [EnumMember(Value = "0xf064")]
        Share = 14,

        [EnumMember(Value = "0xf022")]
        ListAlt = 15,

        [EnumMember(Value = "0xf19c")]
        Bank = 16,

        [EnumMember(Value = "0xf02f")]
        Print = 17,

        [EnumMember(Value = "0xf019")]
        Download = 18,

        [EnumMember(Value = "0xf093")]
        Upload = 19,

        [EnumMember(Value = "0xf0c1")]
        Chain = 20,

        [EnumMember(Value = "0xf127")]
        ChainBroken = 21,

        [EnumMember(Value = "0xf0c1")]
        Link = 22,

        [EnumMember(Value = "0xf1c3")]
        FileExcelO = 23,

        [EnumMember(Value = "0xf0ac")]
        Globe = 24,

        [EnumMember(Value = "0xf0ea")]
        Clipboard = 25,

        [EnumMember(Value = "0xf1ea")]
        NewspaperO = 26,

        [EnumMember(Value = "0xf0a1")]
        Bullhorn = 27,

        [EnumMember(Value = "0xf230")]
        FacebookOfficial = 28,

        [EnumMember(Value = "0xf00d")]
        Close = 29,

        [EnumMember(Value = "0xf074")]
        Random = 31,

        [EnumMember(Value = "0xf6dd")]
        FileCsv = 32,

        [EnumMember(Value = "0xf15c")]
        FileText = 33,

        [EnumMember(Value = "0xf15b")]
        File = 34,

        [EnumMember(Value = "0xf1ec")]
        Calculator = 35,

        [EnumMember(Value = "0xf044")]
        PencilSquareO = 37,

        [EnumMember(Value = "0xf0c4")]
        Cut = 38,

        [EnumMember(Value = "0xf014")]
        TrashO = 39,

        [EnumMember(Value = "0xf13e")]
        UnlockAlt = 40,

        [EnumMember(Value = "0xf1c4")]
        FilePowerpointO = 41,

        [EnumMember(Value = "0xf088")]
        ThumbsODown = 42,

        [EnumMember(Value = "0xf021")]
        Refresh = 43,

        [EnumMember(Value = "0xf003")]
        EnvelopeO = 44,

        [EnumMember(Value = "0xf1c1")]
        FilePdfO = 45,

        [EnumMember(Value = "0xf141")]
        EllipsisH = 46,

        [EnumMember(Value = "0xf165")]
        ThumbsDown = 47,

        [EnumMember(Value = "0xf073")]
        Calendar = 48,

        [EnumMember(Value = "0xf200")]
        PieChart = 49,

        [EnumMember(Value = "0xf05e")]
        Ban = 50,

        [EnumMember(Value = "0xf0e3")]
        Gavel = 51,

        [EnumMember(Value = "0xf0ec")]
        Exchange = 52,

        [EnumMember(Value = "0xf079")]
        Retweet = 53,

        [EnumMember(Value = "0xf1b0")]
        Paw = 54,

        [EnumMember(Value = "0xf015")]
        Home = 55,

        [EnumMember(Value = "0xf1b9")]
        Car = 56,

        [EnumMember(Value = "0xf0ad")]
        Wrench = 57,

        [EnumMember(Value = "0xf24d")]
        Clone = 58,

        [EnumMember(Value = "0xf0c5")]
        FilesO = 59,

        [EnumMember(Value = "0xf1c6")]
        Archive = 60,

        [EnumMember(Value = "0xf2b5")]
        Handshake = 61,

        [EnumMember(Value = "0xf0e7")]
        Bolt = 62,

        [EnumMember(Value = "0xf2bc")]
        AddressCard = 63,

        [EnumMember(Value = "0xf2c1")]
        Badge = 64,

        [EnumMember(Value = "0xf164")]
        ThumbsUp = 65,

        [EnumMember(Value = "0xf1da")]
        History = 66,

        [EnumMember(Value = "0xf017")]
        Clock = 67,

        [EnumMember(Value = "0xf0e2")]
        Undo = 68,

        [EnumMember(Value = "0xf084")]
        Password = 69,

        [EnumMember(Value = "0xf0d0")]
        Magic = 70,

        [EnumMember(Value = "0xf0e7")]
        FlashBolt = 71,

        [EnumMember(Value = "0xf00c")]
        Check = 72,

        [EnumMember(Value = "0xf14a")]
        CheckSquare = 73,

        [EnumMember(Value = "0xf058")] //Plny Circle s checkom
        CheckCircle = 74,

        [EnumMember(Value = "0xf2f1")]
        SyncAlt = 75,

        [EnumMember(Value = "0xf56f")]
        FileImport = 76, //fas fa-file-import

        [EnumMember(Value = "0xf084")]
        Key = 77,

        [EnumMember(Value = "0xf1fa")]
        At = 78,

        [EnumMember(Value = "0xf530")]
        Glasses = 79,

        [EnumMember(Value = "0xf500")]
        Friends = 80,

        [EnumMember(Value = "0xf4fa")]
        UserAltSlash = 81,

        [EnumMember(Value = "0xf688")]
        SearchDollar = 82,

        [EnumMember(Value = "0xf3d1")]
        MoneyBillAlt = 83,

        [EnumMember(Value = "0xf682")]
        Zostavy = 84,

        [EnumMember(Value = "0xf09d")]
        FasFaCreditCard,

        [EnumMember(Value = "0xf068")]
        FasFaMinus,


    #region DMS

    /// <summary>
    /// Ikonka používateľ
    /// </summary>
    [EnumMember(Value = "0xf007")]
        User = 900,

        /// <summary>
        /// Ikonka používateľov
        /// </summary>
        [EnumMember(Value = "0xf0c0")]
        Users = 901,

        [EnumMember(Value = "0xf0e0")]
        Email = 902,

        [EnumMember(Value = "0xf002")]
        Search = 903,

        #endregion
    }
}
