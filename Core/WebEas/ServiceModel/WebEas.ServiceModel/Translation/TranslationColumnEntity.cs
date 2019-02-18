using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel
{
    [DataContract]
    public class TranslationColumnEntity
    {
        [DataMember]
        [PfeColumn(Text = "Kód modulu", Hidden = true)]
        public string Module { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov modulu")]
        public string ModuleName { get; set; }

        [DataMember]
        [PfeColumn(Text = "Entita", Hidden = true)]
        public string Entity { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov tabuľky")]
        public string EntityName { get; set; }

        [DataMember]
        [PfeColumn(Text = "Stĺpec", Hidden = true)]
        public string Column { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov stĺpca")]
        public string ColumnName { get; set; }

        [DataMember]
        [PfeColumn(Text = "Dátový typ", Hidden = true)]
        public Type Type { get; set; }

        [DataMember]
        [PrimaryKey]
        public string UniqueIdentifier
        {
            get
            {
                return string.Format("{0}, {1}!{2}", this.Type, this.Type.Assembly.GetName().Name, this.Column);
            }
        }
    }
}