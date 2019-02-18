using WebEas.ServiceModel.Office.Egov.Types;
using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel.Office.Egov.Types
{
    [Schema("dap")]
    [Alias("C_KodRok")]
    [DataContract]
    public class KodRokOstatneDane : BaseTenantEntity
    {
        [PrimaryKey]
        [DataMember]
        [AutoIncrement]
        public long C_KodRok_Id { get; set; }

        [DataMember]
        public string Druh { get; set; }

        [DataMember]
        public short Rok { get; set; }

        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        public string PopisText1 { get; set; }

        [DataMember]
        public string PopisText2 { get; set; }

        [DataMember]
        public string PopisText3 { get; set; }

        [DataMember]
        public string PopisText4 { get; set; }

        [DataMember]
        public string PopisHodn { get; set; }

        [DataMember]
        public string PopisKoef { get; set; }

        [DataMember]
        public string PopisPozn { get; set; }
    }
}
