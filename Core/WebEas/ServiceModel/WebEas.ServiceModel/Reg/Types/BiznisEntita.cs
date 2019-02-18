using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel.Office.Egov.Reg.Types
{
    [DataContract]
    [Schema("reg")]
    [Alias("D_BiznisEntita")]
    public class BiznisEntita : BaseTenantEntity
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        public long D_BiznisEntita_Id { get; set; }

        [DataMember]
        public int C_TypBiznisEntity_Id { get; set; }

        [DataMember]
        public int C_StavEntity_Id { get; set; }

        [DataMember]
        public int C_StavovyPriestor_Id { get; set; }

        [DataMember]
        public string PolozkaStromu { get; set; }

        [DataMember]
        public long? C_Stredisko_Id { get; set; }

        [DataMember]
        public string CisloDokladu { get; set; }

        [DataMember]
        public DateTime DatumDokladu { get; set; }

        [DataMember]
        public string VS { get; set; }

        [DataMember]
        public byte? UO { get; set; }

        [DataMember]
        public int Rok { get; set; }
    }
}