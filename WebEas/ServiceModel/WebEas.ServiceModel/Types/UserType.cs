using System;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel.Types
{
	[Schema("cfe")]
	[Alias("C_UserType")]
    [DataContract]
    public class UserType : BaseEntity
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        public short C_UserType_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kód")]
        public string Kod { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov", Mandatory = true)]
        public string Nazov { get; set; }
    }
}
