using System;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel
{
    [Schema("reg")]
    [Alias("D_PrekladovySlovnik")]
    [DataContract]
    public class ColumnTranslation : BaseEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public long D_PrekladovySlovnik_Id { get; set; }

        [DataMember]
        public Guid? D_Tenant_Id { get; set; }

        [DataMember]
        public string ModulName { get; set; }

        [DataMember]
        public string TableName { get; set; }

        [DataMember]
        public string ColumnName { get; set; }

        [DataMember]
        public string Identifier { get; set; }

        [DataMember]
        public string Kluc { get; set; }

        //[Alias("cs-CZ")]
        [DataMember]
        public string Cs { get; set; }

        //[Alias("hu-HU")]
        [DataMember]
        public string Hu { get; set; }

        //[Alias("pl-PL")]
        [DataMember]
        public string Pl { get; set; }

        //[Alias("de-AT")]
        [DataMember]
        public string De { get; set; }

        //[Alias("en-US")]
        [DataMember]
        public string En { get; set; }

        [DataMember]
        public string Uk { get; set; }

        [DataMember]
        public string Rom { get; set; }

        [DataMember]
        public string Rue { get; set; }
    }
}