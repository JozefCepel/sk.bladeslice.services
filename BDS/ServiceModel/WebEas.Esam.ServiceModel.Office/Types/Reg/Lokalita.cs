using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Reg
{
    [DataContract]
    [Schema("reg")]
    [Alias("C_Lokalita")]
    public class Lokalita : BaseEntity
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        public byte C_Lokalita_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Lokalita", Mandatory = true)]
        [StringLength(100)]
        public string Nazov { get; set; }
    }

}
