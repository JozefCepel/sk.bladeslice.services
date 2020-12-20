using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Rzp
{
    [Schema("rzp")]
    [Alias("V_RzpDennik")]
    [DataContract]
    public class RzpDennikViewHelper : BaseTenantEntity
    {
        [DataMember]
        [PrimaryKey]
        [AutoIncrement]
        public long D_RzpDennik_Id { get; set; }

        [DataMember]
        public long D_BiznisEntita_Id { get; set; }

        [DataMember]
        public bool R { get; set; }

        [DataMember]
        public byte? PrijemVydaj { get; set; }

        [DataMember]
        public long? C_RzpPol_Id { get; set; }

        [DataMember]
        public long? D_Program_Id { get; set; }

        [DataMember]
        public int? C_Stredisko_Id { get; set; }

        [DataMember]
        public long? C_Projekt_Id { get; set; }

        [DataMember]
        public string Popis { get; set; }

        [DataMember]
        public decimal Suma { get; set; }
    }
}