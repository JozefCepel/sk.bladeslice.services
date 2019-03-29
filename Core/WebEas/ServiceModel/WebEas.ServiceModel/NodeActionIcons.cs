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

        [EnumMember(Value = "0xf00c")]
        Check = 30,

        [EnumMember(Value = "0xf074")]
        Random = 31,

        [EnumMember(Value = "0xf14a")]
        CheckSquare = 32,

        [EnumMember(Value = "0xf15c")]
        FileText = 33,

        [EnumMember(Value = "0xf15b")]
        File = 34,

        [EnumMember(Value = "0xf1ec")]
        Calculator = 35,

        [EnumMember(Value = "0xf046")]
        CheckSquareO = 36,

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

        [EnumMember(Value = "0xf0c8")]
        Square = 67,

        [EnumMember(Value = "0xf146")]
        MinusSquare = 68,


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
