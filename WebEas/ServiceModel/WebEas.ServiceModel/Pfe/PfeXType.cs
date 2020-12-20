using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// Typ control-u na FE
    /// </summary>
    [DataContract(Name = "XType")]
    public enum PfeXType
    {
        [EnumMember(Value = "0")]
        Default = 0,
        [EnumMember(Value = "1")]
        Textfield = 1,
        [EnumMember(Value = "2")]
        Textarea = 2,
        [EnumMember(Value = "3")]
        Numberfield = 3,
        [EnumMember(Value = "4")]
        Datefield = 4,
        [EnumMember(Value = "5")]
        Checkbox = 5,
        [EnumMember(Value = "6")]
        Combobox = 6,
        [EnumMember(Value = "7")]
        Link = 7,
        [EnumMember(Value = "8")]
        FileUploader = 8,
        [EnumMember(Value = "9")]
        BtnLink = 9,
        [EnumMember(Value = "10")]
        SearchPerson = 10,
        [EnumMember(Value = "11")]
        SearchCompany = 11,
        [EnumMember(Value = "12")]
        SearchPersonCompany = 12,
        [EnumMember(Value = "13")]
        SearchAddress = 13,
        [EnumMember(Value = "14")]
        PersonLink = 14,
        [EnumMember(Value = "15")]
        FolderLink = 15,
        //[EnumMember(Value = "16")]
        //RichText = 16,
        [EnumMember(Value = "17")]
        TextareaWW = 17,
        //[EnumMember(Value = "18")]
        //ChangeBankAccount = 18,
        [EnumMember(Value = "19")]
        RichTextRpt = 19,
        [EnumMember(Value = "20")]
        FileSize = 20,
        //[EnumMember(Value = "21")]
        //AddressLink = 21,
        //[EnumMember(Value = "22")]
        //MultiselectComboBox = 22,
        [EnumMember(Value = "23")]
        DateTimeField = 23,
        //[EnumMember(Value = "24")]
        //EPodDokumentLink = 24,
        [EnumMember(Value = "25")]
        SearchFieldSS = 25,
        [EnumMember(Value = "26")]
        SearchFieldMS = 26,
        [EnumMember(Value = "-1")]
        Unknown = -1
    }
}