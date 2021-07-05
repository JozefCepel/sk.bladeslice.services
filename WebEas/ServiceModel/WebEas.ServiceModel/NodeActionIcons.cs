using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [DataContract(Name = "Icons")]
    public enum NodeActionIcons
    {
        //[EnumMember(Value = "0xf016")]
        [EnumMember(Value = "")]
        Default,

        [EnumMember(Value = "0xf06e")]
        Eye,

        [EnumMember(Value = "0xf0b0")]
        Filter,

        [EnumMember(Value = "0xf112")]
        Reply,

        [EnumMember(Value = "0xf05a")]
        InfoCircle,

        [EnumMember(Value = "0xf067")]
        Plus,

        [EnumMember(Value = "0xf100")]
        AngleDoubleLeft,

        [EnumMember(Value = "0xf101")]
        AngleDoubleRight,

        [EnumMember(Value = "0xf044")]
        Edit,

        [EnumMember(Value = "0xf1f8")]
        Trash,

        [EnumMember(Value = "0xf234")]
        UserPlus,

        [EnumMember(Value = "0xf0c7")]
        FloppyO,

        [EnumMember(Value = "0xf129")]
        Info,

        [EnumMember(Value = "0xf0f6")]
        FileTextO,

        [EnumMember(Value = "0xf07b")]
        Folder,

        [EnumMember(Value = "0xf153")]
        Euro,

        [EnumMember(Value = "0xf064")]
        Share,

        [EnumMember(Value = "0xf022")]
        ListAlt,

        [EnumMember(Value = "0xf19c")]
        Bank,

        [EnumMember(Value = "0xf02f")]
        Print,

        [EnumMember(Value = "0xf019")]
        Download,

        [EnumMember(Value = "0xf093")]
        Upload,

        [EnumMember(Value = "0xf0c1")]
        Chain,

        [EnumMember(Value = "0xf127")]
        ChainBroken,

        [EnumMember(Value = "0xf0c1")]
        Link,

        [EnumMember(Value = "0xf1c3")]
        FileExcelO,

        [EnumMember(Value = "0xf0ac")]
        Globe,

        [EnumMember(Value = "0xf0ea")]
        Clipboard,

        [EnumMember(Value = "0xf1ea")]
        NewspaperO,

        [EnumMember(Value = "0xf0a1")]
        Bullhorn,

        [EnumMember(Value = "0xf230")]
        FacebookOfficial,

        [EnumMember(Value = "0xf00d")]
        Close,

        [EnumMember(Value = "0xf074")]
        Random,

        [EnumMember(Value = "0xf6dd")]
        FileCsv,

        [EnumMember(Value = "0xf15c")]
        FileText,

        [EnumMember(Value = "0xf15b")]
        File,

        [EnumMember(Value = "0xf1ec")]
        Calculator,

        [EnumMember(Value = "0xf044")]
        PencilSquareO,

        [EnumMember(Value = "0xf0c4")]
        Cut,

        [EnumMember(Value = "0xf014")]
        TrashO,

        [EnumMember(Value = "0xf13e")]
        UnlockAlt,

        [EnumMember(Value = "0xf1c4")]
        FilePowerpointO,

        [EnumMember(Value = "0xf088")]
        ThumbsODown,

        [EnumMember(Value = "0xf021")]
        Refresh,

        [EnumMember(Value = "0xf003")]
        EnvelopeO,

        [EnumMember(Value = "0xf1c1")]
        FilePdfO,

        [EnumMember(Value = "0xf141")]
        EllipsisH,

        [EnumMember(Value = "0xf142")]
        EllipsisV,

        [EnumMember(Value = "0xf165")]
        ThumbsDown,

        [EnumMember(Value = "0xf073")]
        Calendar,

        [EnumMember(Value = "0xf200")]
        PieChart,

        [EnumMember(Value = "0xf05e")]
        Ban,

        [EnumMember(Value = "0xf0e3")]
        Gavel,

        [EnumMember(Value = "0xf0ec")]
        Exchange,

        [EnumMember(Value = "0xf079")]
        Retweet,

        [EnumMember(Value = "0xf1b0")]
        Paw,

        [EnumMember(Value = "0xf015")]
        Home,

        [EnumMember(Value = "0xf1b9")]
        Car,

        [EnumMember(Value = "0xf0ad")]
        Wrench,

        [EnumMember(Value = "0xf24d")]
        Clone,

        [EnumMember(Value = "0xf0c5")]
        FilesO,

        [EnumMember(Value = "0xf1c6")]
        Archive,

        [EnumMember(Value = "0xf2b5")]
        Handshake,

        [EnumMember(Value = "0xf0e7")]
        Bolt,

        [EnumMember(Value = "0xf2bc")]
        AddressCard,

        [EnumMember(Value = "0xf2c1")]
        Badge,

        [EnumMember(Value = "0xf164")]
        ThumbsUp,

        [EnumMember(Value = "0xf1da")]
        History,

        [EnumMember(Value = "0xf017")]
        Clock,

        [EnumMember(Value = "0xf0e2")]
        Undo,

        [EnumMember(Value = "0xf084")]
        Password,

        [EnumMember(Value = "0xf0d0")]
        Magic,

        [EnumMember(Value = "0xf0e7")]
        FlashBolt,

        [EnumMember(Value = "0xf00c")]
        Check,

        [EnumMember(Value = "0xf14a")]
        CheckSquare,

        [EnumMember(Value = "0xf058")] //Plny Circle s checkom
        CheckCircle,

        [EnumMember(Value = "0xf2f1")]
        SyncAlt,

        [EnumMember(Value = "0xf56f")]
        FileImport, //fas fa-file-import

        [EnumMember(Value = "0xf084")]
        Key,

        [EnumMember(Value = "0xf1fa")]
        At,

        [EnumMember(Value = "0xf530")]
        Glasses,

        [EnumMember(Value = "0xf500")]
        Friends,

        [EnumMember(Value = "0xf4fa")]
        UserAltSlash,

        [EnumMember(Value = "0xf688")]
        SearchDollar,

        [EnumMember(Value = "0xf3d1")]
        MoneyBillAlt,

        [EnumMember(Value = "0xf682")]
        Zostavy,

        [EnumMember(Value = "0xf09d")]
        FasFaCreditCard,

        [EnumMember(Value = "0xf068")]
        FasFaMinus,

        [EnumMember(Value = "0xf651")]
        FasFaCommentDollar,

        [EnumMember(Value = "0xf02d")]
        Book,

        #region DMS

        /// <summary>
        /// Ikonka používateľ
        /// </summary>
        [EnumMember(Value = "0xf007")]
        User,

        /// <summary>
        /// Ikonka používateľov
        /// </summary>
        [EnumMember(Value = "0xf0c0")]
        Users,

        [EnumMember(Value = "0xf0e0")]
        Email,

        [EnumMember(Value = "0xf002")]
        Search,

        #endregion
    }
}
