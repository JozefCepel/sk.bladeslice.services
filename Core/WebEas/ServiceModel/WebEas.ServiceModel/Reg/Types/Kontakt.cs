using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel.Office.Egov.Reg.Types
{
    [Schema("reg")]
    [Alias("D_Kontakt")]
    [DataContract]
    public class Kontakt : BaseEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public long D_Kontakt_Id { get; set; }

        [StringLength(500)]
        [DataMember]
        public string Email { get; set; }

        [StringLength(50)]
        [DataMember]
        public string TelefonneCislo { get; set; }

        [StringLength(10)]
        [DataMember]
        public string TypTelefonu { get; set; }

        public bool IsDefined()
        {
            return !string.IsNullOrEmpty(this.Email) || !string.IsNullOrEmpty(this.TelefonneCislo);
        }
    }
}