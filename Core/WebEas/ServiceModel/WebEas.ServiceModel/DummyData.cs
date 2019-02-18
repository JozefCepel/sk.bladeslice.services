using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// Dummy data class
    /// </summary>
    [DataContract]
    [PfeDataModel(MultiSortEnabled = true, RowFilterEnabled = true, Type = PfeModelType.Grid, Name = "Dummy model")]
    public class DummyData : BaseTenantEntity
    {
        /// <summary>
        /// Gets or sets the dummy data_ id.
        /// </summary>
        /// <value>The dummy data_ id.</value>
        [PrimaryKey]
        [AutoIncrement]
        public long DummyData_Id { get; set; }

        /// <summary>
        /// Gets or sets the nazov.
        /// </summary>
        /// <value>The nazov.</value>
        [PfeColumn(Text = "Názov")]
        [DataMember]
        public string Nazov { get; set; }

        /// <summary>
        /// Gets or sets the popis.
        /// </summary>
        /// <value>The popis.</value>
        [DataMember]
        public string Popis { get; set; }

        /// <summary>
        /// Gets or sets the hodnota.
        /// </summary>
        /// <value>The hodnota.</value>
        [DataMember]
        public int Hodnota { get; set; }

        /// <summary>
        /// Gets or sets the velkost.
        /// </summary>
        /// <value>The velkost.</value>
        [DataMember]
        [PfeColumn(Text = "Veľkosť")]
        public Decimal Velkost { get; set; }

        /// <summary>
        /// Gets or sets the aktualny datum.
        /// </summary>
        /// <value>The aktualny datum.</value>
        [DataMember]
        [PfeColumn(Text = "Aktuálny dátum")]
        public DateTime AktualnyDatum { get; set; }

        /// <summary>
        /// Gets or sets the zverejnene.
        /// </summary>
        /// <value>The zverejnene.</value>
        [DataMember]
        [PfeColumn(Text = "Zverejnené")]
        public bool Zverejnene { get; set; }

        /// <summary>
        /// Gets or sets the odoslane.
        /// </summary>
        /// <value>The odoslane.</value>
        [DataMember]
        [PfeColumn(Text = "Odoslané")]
        public bool Odoslane { get; set; }

        /// <summary>
        /// Gets or sets the stav.
        /// </summary>
        /// <value>The stav.</value>
        [DataMember]
        [PfeCombo(typeof(DummyComboStav))]
        public string Stav { get; set; }

        /// <summary>
        /// Gets or sets the typ.
        /// </summary>
        /// <value>The typ.</value>
        [DataMember]
        [PfeCombo(typeof(DummyCombo))]
        public string Typ { get; set; }
    }
}