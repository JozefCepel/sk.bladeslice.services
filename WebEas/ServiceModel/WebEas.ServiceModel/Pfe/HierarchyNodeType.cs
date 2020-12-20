using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [DataContract]
    public static class HierarchyNodeType
    {
        [DataMember]
        public const string ProgramovyRozpocet = "ico-16-pr-rozpocet";

        [DataMember]
        public const string Program = "ico-16-program";

        [DataMember]
        public const string Podprogram = "ico-16-podprogram";

        [DataMember]
        public const string Prvok = "ico-16-prvok";

        [DataMember]
        public const string Doklad = "doklad";

        [DataMember]
        public const string DokladIn = "doklad-in";

        [DataMember]
        public const string DokladOut = "doklad-out";

        [DataMember]
        public const string Priecinok = "-1";

        [DataMember]
        public const string FolderY = "folder-o-yellow";
        
        [DataMember]
        public const string Unknown = "-99";
    }
}