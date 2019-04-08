using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel.Office.Egov.Reg.Types
{
    [Schema("reg")]
    [Alias("C_PoleFormulara")]
    [DataContract]
    [Dial(DialType.Global, DialKindType.BackOffice)]
    public class PoleFormulara : BaseEntity
    {
        [PrimaryKey]
        [DataMember]        
        public int C_PoleFormulara_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Name")]
        [Required]
        [StringLength(500)]
        public string Nazov { get; set; }
        
        [DataMember]        
        public int D_Formular_Id { get; set; }
    }
}