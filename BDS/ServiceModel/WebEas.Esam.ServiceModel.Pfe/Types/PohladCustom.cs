using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel.Pfe.Types
{
    [Schema("pfe")]
    [Alias("D_Pohlad_Custom")]
    [TenantUpdatable]
    [DataContract]
    public class PohladCustom : BaseTenantEntity
    {
        [AutoIncrement]
        [PrimaryKey]
        [DataMember]
        public int D_Pohlad_Custom_Id { get; set; }

        [DataMember]
        public int D_Pohlad_Id_Master { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        [DataMember]
        public bool? ShowInActions { get; set; }

        [DataMember]
        public int ViewSharing { get; set; }

        [DataMember]
        public bool? DefaultView { get; set; }

        [DataMember]
        public string RibbonFilters { get; set; }

        [DataMember]
        public string FilterText { get; set; }

        [DataMember]
        public string TypAkcie { get; set; }

        [DataMember]
        public int? PageSize { get; set; }

        [DataMember]
        public string Data { get; set; }
    }
}