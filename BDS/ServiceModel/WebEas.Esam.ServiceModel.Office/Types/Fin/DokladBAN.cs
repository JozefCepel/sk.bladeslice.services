using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Fin
{
    [Schema("fin")]
    [Alias("D_DokladBAN")]
    [DataContract]
    public class DokladBAN : BaseTenantEntity
    {
        [DataMember]
        [PrimaryKey]
        public long D_DokladBAN_Id { get; set; }

        [DataMember]
        public short Rok { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kredit", Mandatory = true, DefaultValue = 0)]
        public decimal DM_Kredit { get; set; }

        [DataMember]
        [PfeColumn(Text = "Debet", Mandatory = true, DefaultValue = 0)]
        public decimal DM_Debet { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Kredit CM", Mandatory = true, DefaultValue = 0)]
        public decimal CM_Kredit { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Debet CM", Mandatory = true, DefaultValue = 0)]
        public decimal CM_Debet { get; set; }

        [DataMember]
        [PfeColumn(Text = "DCOM", DefaultValue = 0, ReadOnly = true)]
        public bool DCOM { get; set; }

        [DataMember]
        [PfeColumn(Text = "Počet položiek")]
        public short PocetPol { get; set; }
    }
}
