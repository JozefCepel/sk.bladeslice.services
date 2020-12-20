using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel.Office.Egov.Cis.Types
{
    [Flags]
    public enum ExistencnyStavEnum
    {
        Zivy = 1,
        Mrtvy = 2,
        Nezvestny = 3
    }

    [Schema("cfe")]
    [Alias("C_ExistencnyStav")]
    [DataContract]
    [Dial(DialType.Static, DialKindType.BackOffice)]
    public class ExistencnyStav
    {
        [PrimaryKey]
        [DataMember]
        [Alias("ID")]
        public int Id { get; set; }

        [DataMember]
        public string Nazov { get; set; }
    }
}
