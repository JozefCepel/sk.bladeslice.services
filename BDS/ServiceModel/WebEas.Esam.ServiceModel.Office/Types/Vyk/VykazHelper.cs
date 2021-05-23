using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Vyk
{
    [DataContract]
    [Schema("vyk")]
    [Alias("D_Vykaz")]
    public class VykazHelper : BaseTenantEntity
    {
        [DataMember]
        [PrimaryKey]
        [AutoIncrement]
        public long D_Vykaz_Id { get; set; }

        [DataMember]
        public int C_VykazDruh_Id { get; set; }

        [DataMember]
        public short Rok { get; set; }

        [DataMember]
        public short Obdobie { get; set; }

        [DataMember]
        public DateTime Datum { get; set; }

        [DataMember]
        public int? Poradie { get; set; }

        [DataMember]
        public DateTime? ExportDatum { get; set; }

        [DataMember]
        public bool Export { get; set; }

        [DataMember]
        public bool RISSAM { get; set; }

        [DataMember]
        public string Popis { get; set; }

        [DataMember]
        public int? D_Subor_Id { get; set; }

    }
}
