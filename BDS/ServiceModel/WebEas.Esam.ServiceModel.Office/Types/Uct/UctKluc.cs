using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Uct
{
    [DataContract]
    [Schema("uct")]
    [Alias("C_UctKluc")]
    public class UctKluc : BaseTenantEntity
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        public long C_UctKluc_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Účtovný kľúč", Mandatory = true)]
        public byte UctovnyKluc { get; set; }

        [DataMember]
        [StringLength(10)]
        [PfeColumn(Text = "Kód", Mandatory = true)]
        public string Kod { get; set; }

        [DataMember]
        [StringLength(50)]
        [PfeColumn(Text = "Názov", Mandatory = true)]
        public string Nazov { get; set; }

    }
}
