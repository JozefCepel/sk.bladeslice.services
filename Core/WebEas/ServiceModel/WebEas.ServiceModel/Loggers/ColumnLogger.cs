using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel
{
    [DataContract]
    [Schema("reg")]
    [Alias("D_Logging")]
    public class ColumnLogger : BaseTenantEntityNullable
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        [PfeColumn(Text = "Id")] 
        public long D_Logging_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Id záznamu")]
        public long Row_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Schéma")] 
        public string Schema { get; set; }

        [DataMember]
        [PfeColumn(Text = "Name tabuľky")] 
        public string NazovTabulky { get; set; }

        [DataMember]
        [PfeColumn(Text = "Name stĺpca")] 
        public string NazovStlpca { get; set; }

        [DataMember]
        [PfeColumn(Text = "Typ stĺpca")] 
        public string TypStlpca { get; set; }

        [DataMember]
        [PfeColumn(Text = "Pôvodná hodnota")] 
        public string PovodnaHodnota { get; set; }

        [DataMember]
        [PfeColumn(Text = "Nová hodnota")]
        public string NovaHodnota { get; set; }

        [DataMember]
        [PfeColumn(Text = "Popis zmeny")]
        public string PopisZmeny { get; set; }

        [IgnoreDataMember]
        public bool Hist { get; set; }
    }
}
