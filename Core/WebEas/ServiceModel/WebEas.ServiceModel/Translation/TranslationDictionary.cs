using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel
{
    [DataContract]
    public class TranslationDictionary : BaseEntity
    {
        public readonly static List<string> AcceptableLanguages = new List<string> { "cs", "hu", "pl", "de", "en", "uk", "rom", "rue" };

        [DataMember]
        [PfeColumn(Text = "Id", Hidden = true, Hideable = false)]
        public long? D_PrekladovySlovnik_Id { get; set; }

        /// <summary>
        /// Gets the global record.
        /// </summary>
        /// <value>The global record.</value>
        [Ignore]
        [DataMember]
        [PfeColumn(Text = "Globálny záznam", Editable = false, Rank = 1, ReadOnly = true)]
        public bool GlobalRecord
        {
            get
            {
                return this.D_Tenant_Id == null;
            }
        }

        [DataMember]
        [PfeColumn(Text = "Identifikátor")]
        public string Identifier { get; set; }

        [DataMember]
        [PfeColumn(Text = "Modul", Hidden = true, Hideable = false)]
        public string ModulName { get; set; }

        [DataMember]
        [PfeColumn(Text = "Entita", Hidden = true, Hideable = false)]
        public string TableName { get; set; }

        [DataMember]
        [PfeColumn(Text = "Stĺpec", Hidden = true, Hideable = false)]
        public string ColumnName { get; set; }
                
        [PrimaryKey]
        [DataMember]
        [PfeColumn(Text = "Kľúč", Hidden = true)]
        public string Kluc { get; set; }

        [DataMember]
        [PfeColumn(Text = "Slovenčina", Editable = false)]
        public string PovodnaHodnota { get; set; }

        [DataMember]
        [PfeColumn(Text = "Angličtina")]
        public string En { get; set; }

        [DataMember]
        [PfeColumn(Text = "Čeština")]
        public string Cs { get; set; }

        [DataMember]
        [PfeColumn(Text = "Maďarčina")]
        public string Hu { get; set; }

        [DataMember]
        [PfeColumn(Text = "Polština")]
        public string Pl { get; set; }

        [DataMember]
        [PfeColumn(Text = "Nemčina")]
        public string De { get; set; }

        [DataMember]
        [PfeColumn(Text = "Ukrajinčina")]
        public string Uk { get; set; }

        [DataMember]
        [PfeColumn(Text = "Rómština")]
        public string Rom { get; set; }

        [DataMember]
        [PfeColumn(Text = "Rusínčina")]
        public string Rue { get; set; }

        [DataMember]
        [PfeColumn(Hidden = true, Hideable = false)]
        public Guid? D_Tenant_Id { get; set; }

        [DataMember]
        [Ignore]
        [PfeColumn(Hidden = true, Hideable = false)]
        public string UniqueIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the poznamka.
        /// </summary>
        /// <value>The poznamka.</value>
        [DataMember]
        [PfeColumn(Text = "Poznámka", Hidden = true, Editable = false, Xtype = PfeXType.Textarea, Rank = 104)]
        [StringLength(255)]
        public new string Poznamka { get; set; }
    }
}