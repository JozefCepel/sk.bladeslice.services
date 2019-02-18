using System;
using System.Linq;

namespace WebEas.Client.Encoder
{
    /// <summary>
    /// 
    /// </summary>
    internal class ConfigurationStrings
    {
        internal const string MessageVersion = "messageVersion";
        internal const string MediaType = "mediaType";
        internal const string Encoding = "encoding";
        internal const string AddEncoding = "addEncoding";
        internal const string ReaderQuotas = "readerQuotas";

        internal const string None = "None";
        internal const string Default = "Default";
        internal const string Soap11 = "Soap11";
        internal const string Soap11WSAddressing10 = "Soap11WSAddressing10";
        internal const string Soap11WSAddressingAugust2004 = "Soap11WSAddressingAugust2004";
        internal const string Soap12 = "Soap12";
        internal const string Soap12WSAddressing10 = "Soap12WSAddressing10";
        internal const string Soap12WSAddressingAugust2004 = "Soap12WSAddressingAugust2004";

        internal const string DefaultMessageVersion = Soap12;
        internal const string DefaultMediaType = "application/soap+xml";
        internal const string DefaultEncoding = "UTF-8";
    }
}