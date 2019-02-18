using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel.Reg.Types
{
    [Schema("reg")]
    [Alias("C_StavEntity_StavEntity")]
    [DataContract]
    [Dial(DialType.Global, DialKindType.BackOffice)]
    public class StavEntityStavEntity : BaseEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public int C_StavEntity_StavEntity_Id { get; set; }

        [DataMember]
        public int C_StavEntity_Id_Parent { get; set; }

        [DataMember]
        public int C_StavEntity_Id_Child { get; set; }

        [DataMember]
        public int C_StavovyPriestor_Id { get; set; }

        [DataMember]
        [StringLength(255)]
        public string NazovUkonu { get; set; }

        [DataMember]
        public bool ManualnyPrechodPovoleny { get; set; }

        [DataMember]
        public byte? ePodatelnaEvent { get; set; }
    }
}
