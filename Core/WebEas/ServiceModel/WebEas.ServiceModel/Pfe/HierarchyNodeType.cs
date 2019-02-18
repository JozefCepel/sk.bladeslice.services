using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [DataContract]
    public static class HierarchyNodeType
    {
        [DataMember]
        public const string Ciselnik = "file-o";

        [DataMember]
        public const string DatovaPolozka = "file-text-o";

        [DataMember]
        public const string NovePodanie = "exclamation-circle";

        [DataMember]
        public const string StatickyCiselnik = "file-code-o";

        [DataMember]
        public const string History = "fa-history";

        [DataMember]
        public const string Money = "fa-money";

        [DataMember]
        public const string University = "fa-university";

        [DataMember]
        public const string Settings = "fa-wrench";

        #region eSam

        /// <summary>
        /// Programový rozpočet
        /// </summary>
        [DataMember]
        public const string ProgramovyRozpocet = "ico-16-pr-rozpocet";

        /// <summary>
        /// Program
        /// </summary>
        [DataMember]
        public const string Program = "ico-16-program";

        /// <summary>
        /// Podprogram
        /// </summary>
        [DataMember]
        public const string Podprogram = "ico-16-podprogram";

        /// <summary>
        /// Prvok
        /// </summary>
        [DataMember]
        public const string Prvok = "ico-16-prvok";

        #endregion

        #region INE

        [DataMember]
        //public const string Priecinok = "nechame_sa_prekvapit";
        public const string Priecinok = "-1";

        [DataMember]
        //public const string Unknown = "nechame_sa_prekvapit";
        public const string Unknown = "-99";

        #endregion
    }
}