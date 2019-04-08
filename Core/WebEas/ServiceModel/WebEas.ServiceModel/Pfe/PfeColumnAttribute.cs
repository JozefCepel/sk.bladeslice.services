using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    [DataContract(Name = "Column")]
    public class PfeColumnAttribute : Attribute, IEquatable<PfeColumnAttribute>
    {
        private bool? hidden;
        private bool? hideable;
        private bool? editable;
        private bool? mandatory;
        private bool? readOnly;

        /// <summary>
        /// Konštruktor pre vytvorenie stĺpca
        /// </summary>
        /// <param name="name">Name stĺpca</param>
        /// <param name="text">Popis stĺpca</param>
        /// <param name="rank">Poradie stĺpca</param>
        /// <param name="hidden">Skrytie stĺpca (bez dát)</param>
        /// <param name="alwaysData">Skrytý stĺpec s dátami</param>
        /// <param name="type">Dátový typ stĺpca</param>
        /// <param name="xtype">Typ stĺpca</param>
        /// <param name="editable"></param>
        /// <param name="mandatory">Je stĺpece povinný</param>
        /// <param name="width">Šírka stĺpca</param>
        /// <param name="align">Zarovnanie obsahu stĺpca</param>
        /// <param name="tooltip">Tooltip pre stĺpec</param>
        /// <param name="sortable">Povolanie zoradenia</param>
        /// <param name="sortDirection">Určenie povoloného zoradenia</param>
        /// <param name="filterable">Povolenie možnosti textového filtra</param>
        /// <param name="format">Formátovanie obsahu stĺpca</param>
        /// <param name="decimalPalces">Počet miest na zaokrúhľovanie</param>
        /// <param name="dataUrl"><Odkaz na číselník/param>
        /// <param name="total">Spocitavanie??</param>
        /// <param name="defaultValue">Prednastavená hodnota stĺpca</param>
        /// <param name="maxlength">Maximálna dĺžka text. reťazca</param>
        /// <param name="description">Popis stĺpca</param>
        /// <param name="validator">PFE validator</param>
        public PfeColumnAttribute(string name, string text, int rank, bool hidden = false, bool? hideable = null, PfeDataType type = PfeDataType.Unknown, PfeXType xtype = PfeXType.Unknown,
            bool? editable = null, bool? mandatory = null, int width = 0, PfeAligment align = PfeAligment.Unknown, string tooltip = "", bool sortable = true, PfeOrder sortDirection = PfeOrder.Asc,
            bool filterable = true, string format = "0", int decimalPlaces = 2, string dataUrl = "", object defaultValue = null, bool? readOnly = null, int? maxlength = null, string description = "", PfeValidator validator = null)
        {
            this.Name = name;
            this.Text = text;
            this.Rank = rank;
            this.Type = type;
            this.Xtype = xtype;
            this.Width = width;
            this.editable = editable;
            this.mandatory = mandatory;
            this.Hidden = hidden;
            this.hideable = hideable;
            this.Align = align;
            this.Tooltip = tooltip;
            this.Sortable = sortable;
            this.SortDirection = sortDirection;
            this.Filterable = filterable;
            this.Format = format;
            this.DecimalPlaces = decimalPlaces;
            this.DataUrl = dataUrl;            
            this.DefaultValue = defaultValue;
            this.readOnly = readOnly;
            this.MaxLength = maxlength;
            this.Description = description;
            this.Validator = validator;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PfeColumnAttribute" /> class.
        /// </summary>
        public PfeColumnAttribute()
        {
            this.Type = PfeDataType.Unknown;
            this.Xtype = PfeXType.Unknown;
            this.Width = 0;
            this.Align = PfeAligment.Unknown;
            this.Tooltip = String.Empty;
            this.Sortable = true;
            this.SortDirection = PfeOrder.Asc;
            this.Filterable = true;
            // this.Format = "0";
            this.DecimalPlaces = 2;
            this.DataUrl = String.Empty;            
            this.DefaultValue = null;
            this.Description = String.Empty;
        }

        /// <summary>
        /// Gets or sets the is decimal.
        /// </summary>
        /// <value>The is decimal.</value>
        public bool IsDecimal { get; set; }

        /// <summary>
        /// Name stĺpca
        /// </summary>
        /// <value>The name.</value>
        [DataMember(Name = "nam")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the is primary.
        /// </summary>
        /// <value>The is primary.</value>
        [IgnoreDataMember]
        public bool IsPrimary { get; set; }

        /// <summary>
        /// Name stĺpca, ktorý sa vygeneruje, default je pomenovanie property v class-e
        /// </summary>
        /// <value>The type.</value>
        [DataMember(Name = "typ")]
        public PfeDataType Type { get; set; }

        /// <summary>
        /// Gets the has editable.
        /// </summary>
        /// <value>The has editable.</value>
        public bool HasEditable
        {
            get
            {
                return this.editable.HasValue;
            }
        }

        /// <summary>
        /// true/false pre možnosť editovania daného stĺpca
        /// </summary>
        /// <value>The editable.</value>

        //[DataMember(Name = "editable")]
        [IgnoreDataMember]
        public bool Editable
        {
            get
            {
                return this.editable ?? true;
            }
            set
            {
                this.editable = value;
            }
        }

        /// <summary>
        /// Gets the has read only.
        /// </summary>
        /// <value>The has read only.</value>
        public bool HasReadOnly
        {
            get
            {
                return this.readOnly.HasValue;
            }
        }

        /// <summary>
        /// true/false označuje stĺpec, ktorý sa pri ukladaní nemá odoslať na backend
        /// </summary>
        /// <value>The readonly value.</value>
        //[DataMember(Name = "readonly")]
        [IgnoreDataMember]
        public bool ReadOnly
        {
            get
            {
                return this.readOnly ?? false;
            }
            set
            {
                this.readOnly = value;
            }
        }

        /// <summary>
        /// Gets the has mandatory.
        /// </summary>
        /// <value>The has mandatory.</value>
        public bool HasMandatory
        {
            get
            {
                return this.mandatory.HasValue;
            }
        }

        /// <summary>
        /// true/false pre určenie, či je stĺpce povinný/nepovinný
        /// </summary>
        /// <value>The mandatory.</value>
        //[DataMember(Name = "mandatory")]
        [IgnoreDataMember]
        public bool Mandatory
        {
            get
            {
                return this.mandatory ?? false;
            }
            set
            {
                this.mandatory = value;
            }
        }

        /// <summary>
        /// Gets the has hidden.
        /// </summary>
        /// <value>The has hidden.</value>
        [IgnoreDataMember]
        public bool HasHidden
        {
            get
            {
                return this.hidden.HasValue;
            }
        }

        /// <summary>
        /// true/false pre skrytie/zobrazenie stĺpca
        /// </summary>
        /// <value>The hidden.</value>
        //[DataMember(Name = "hidden")]
        [IgnoreDataMember]
        public bool Hidden
        {
            get
            {
                return this.hidden ?? false;
            }
            set
            {
                this.hidden = value;
            }
        }

        /// <summary>
        /// Gets the has hideable.
        /// </summary>
        /// <value>The has hideable.</value>
        [IgnoreDataMember]
        public bool HasHideable
        {
            get
            {
                return this.hideable.HasValue;
            }
        }

        /// <summary>
        /// true/false pre povolenie možnosti zobrazenia stĺpca/zakázanie zobrazenia
        /// </summary>
        /// <value>The hideable.</value>
        //[DataMember(Name = "hideable")]
        [IgnoreDataMember]
        public bool Hideable
        {
            get
            {
                return this.hideable ?? true;
            }
            set
            {
                this.hideable = value;
            }
        }

        /// <summary>
        /// šírka stĺpca (Width = -1 – FE automaticky určí šírku podľa dĺžky caption, ak sa atribút nepošle, FE dodá konštantnú šírku)
        /// </summary>
        /// <value>The width.</value>
        [DataMember(Name = "wid")]
        public int Width { get; set; }

        /// <summary>
        /// Popis stĺpca (caption)
        /// </summary>
        /// <value>The text.</value>
        [DataMember(Name = "txt")]
        public string Text { get; set; }

        /// <summary>
        /// Typ stĺpca
        /// </summary>
        /// <value>The xtype.</value>
        [DataMember(Name = "xtp")]
        public PfeXType Xtype { get; set; }

        /// <summary>
        /// Zarovnanie obsahu stĺpca
        /// </summary>
        /// <value>The align.</value>
        [DataMember(Name = "aln")]
        public PfeAligment Align { get; set; }

        /// <summary>
        /// Tooltip pre stĺpec
        /// </summary>
        /// <value>The tooltip.</value>
        [DataMember(Name = "tip")]
        public string Tooltip { get; set; }

        /// <summary>
        /// true/false pre povolenie zoradenia/zakázanie zoradenia
        /// </summary>
        /// <value>The sortable.</value>
        //[DataMember(Name = "sortable")]
        [IgnoreDataMember]
        public bool Sortable { get; set; }

        /// <summary>
        /// Určenie povoloného zoradenia
        /// </summary>
        /// <value>The sort direction.</value>
        [DataMember(Name = "sdr")]
        public PfeOrder SortDirection { get; set; }

        /// <summary>
        /// Povolenie možnosti textového filtra
        /// </summary>
        /// <value>The filterable.</value>
        //[DataMember(Name = "filterable")]
        [IgnoreDataMember]
        public bool Filterable { get; set; }

        /// <summary>
        /// Formátovanie obsahu stĺpca
        /// </summary>
        /// <value>The format.</value>
        [DataMember(Name = "frm")]
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets the decimal places.
        /// </summary>
        /// <value>The decimal places.</value>
        [DataMember(Name = "plc")]
        public int DecimalPlaces { get; set; }

        /// <summary>
        /// Odkaz na číselník
        /// </summary>
        /// <value>The data URL.</value>
        [DataMember(Name = "url")]
        public string DataUrl { get; set; }

        /// <summary>
        /// Nazov fieldu na ktory sa ma referencovat
        /// </summary>
        /// <value>The name field.</value>
        [DataMember(Name = "nfd")]
        public string NameField { get; set; }

        /// <summary>
        /// Id field v combo
        /// </summary>
        /// <value>The id field.</value>
        [DataMember(Name = "ifd")]
        public string IdField { get; set; }

        /// <summary>
        /// Value field v combo
        /// </summary>
        /// <value>The value field.</value>
        [DataMember(Name = "vfd")]
        public string ValueField { get; set; }

        /// <summary>
        /// Zoznam pozadovanych stlpcov pre combo
        /// </summary>
        /// <value>The required fields.</value>
        [DataMember(Name = "rfds")]
        public string[] RequiredFields { get; set; }

        /// <summary>
        /// prednastavená hodnota stĺpca (typ object)
        /// </summary>
        /// <value>The default value.</value>
        [DataMember(Name = "dvl")]
        public object DefaultValue { get; set; }

        /// <summary>
        /// Popis stĺpca
        /// </summary>
        /// <value>The description.</value>
        [DataMember(Name = "dsc")]
        public string Description { get; set; }

        /// <summary>
        /// Poradie stĺpca
        /// </summary>
        /// <value>The rank.</value>
        [DataMember(Name = "rnk")]
        public int Rank { get; set; }

        /// <summary>
        /// Maximálna dĺžka text. reťazca
        /// </summary>
        /// <value>The max. dĺžka reťazca.</value>
        [DataMember(Name = "max")]
        public int? MaxLength { get; set; }

        /// <summary>
        /// Ci dany stlpec obsahuje data z DB, lebo iba default data (kvoli optimalizacii na DB https://jira.posam.sk/browse/DCOMPREV-5380 ).
        /// </summary>
        [DataMember(Name = "wod")]
        public bool WithoutData { get; set; }

        /// <summary>
        /// Custom validacie na FE.
        /// </summary>
        [DataMember(Name = "vld")]
        public PfeValidator Validator { get; set; }
        
        /// <summary>
        /// Gets or sets the flag.
        /// </summary>
        /// <value>The flag.</value>
        [DataMember(Name = "flg")]
        public int Flag
        {
            get
            {
                return (int)this.GetPfeColumnAttributeFlag();
            }
        }

        /// <summary>
        /// Typ property
        /// </summary>
        /// <value>The type of the property.</value>
        [IgnoreDataMember]
        public PropertyInfo PropertyTypeInfo { get; set; }

        public bool ShouldSerializeText()
        {
            return !String.IsNullOrEmpty(this.Text);
        }

        public bool ShouldSerializeTooltip()
        {
            return !String.IsNullOrEmpty(this.Tooltip);
        }

        public bool ShouldSerializeFormat()
        {
            return !String.IsNullOrEmpty(this.Format);
        }

        public bool ShouldSerializeDataUrl()
        {
            return !String.IsNullOrEmpty(this.DataUrl);
        }

        public bool ShouldSerializeNameField()
        {
            return !String.IsNullOrEmpty(this.NameField);
        }

        public bool ShouldSerializeIdField()
        {
            return !String.IsNullOrEmpty(this.IdField);
        }

        public bool ShouldSerializeValueField()
        {
            return !String.IsNullOrEmpty(this.ValueField);
        }

        public bool ShouldSerializeRequiredFields()
        {
            return this.RequiredFields != null;
        }

        public bool ShouldSerializeDefaultValue()
        {
            return this.DefaultValue != null;
        }

        public bool ShouldSerializeDescription()
        {
            return !string.IsNullOrEmpty(this.Description);
        }

        public bool ShouldSerializeMaxLength()
        {
            return this.MaxLength.HasValue;
        }

        /// <summary>
        /// Shoulds the serialize actions.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeDecimalPlaces()
        {
            return this.Xtype == PfeXType.Numberfield && this.IsDecimal && this.DecimalPlaces >= 0;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"Name: {Name}, Type: {this.Type}, Editable: {this.Editable}, Mandatory: {this.Mandatory}, Hidden: {this.Hidden}, Hideable: {this.Hideable}, Width: {this.Width}, Text: {this.Text}, Xtype: {this.Xtype}, Align: {this.Align}, Tooltip: {this.Tooltip}, Sortable: {this.Sortable}, SortDirection: {this.SortDirection}, Filterable: {this.Filterable}, Format: {this.Format}, DecimalPlaces: {this.DecimalPlaces}, DataUrl: {this.DataUrl}, DefaultValue: {this.DefaultValue}, Description: {this.Description}, Rank: {this.Rank}, ReadOnly: {this.ReadOnly}, MaxLength: {this.MaxLength}, Validator: {this.Validator}";
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = 17;
                result = result * 23 + base.GetHashCode();
                result = result * 23 + ((this.Name != null) ? this.Name.GetHashCode() : 0);
                result = result * 23 + this.Type.GetHashCode();
                result = result * 23 + this.Editable.GetHashCode();
                result = result * 23 + this.Mandatory.GetHashCode();
                result = result * 23 + this.Hidden.GetHashCode();
                result = result * 23 + this.Hideable.GetHashCode();
                result = result * 23 + this.Width.GetHashCode();
                result = result * 23 + ((this.Text != null) ? this.Text.GetHashCode() : 0);
                result = result * 23 + this.Xtype.GetHashCode();
                result = result * 23 + this.Align.GetHashCode();
                result = result * 23 + ((this.Tooltip != null) ? this.Tooltip.GetHashCode() : 0);
                result = result * 23 + this.Sortable.GetHashCode();
                result = result * 23 + this.SortDirection.GetHashCode();
                result = result * 23 + this.Filterable.GetHashCode();
                result = result * 23 + ((this.Format != null) ? this.Format.GetHashCode() : 0);
                result = result * 23 + this.DecimalPlaces.GetHashCode();
                result = result * 23 + ((this.DataUrl != null) ? this.DataUrl.GetHashCode() : 0);                
                result = result * 23 + ((this.DefaultValue != null) ? this.DefaultValue.GetHashCode() : 0);
                result = result * 23 + ((this.Description != null) ? this.Description.GetHashCode() : 0);
                result = result * 23 + this.Rank.GetHashCode();
                result = result * 23 + this.ReadOnly.GetHashCode();
                result = result * 23 + this.MaxLength.GetHashCode();
                result = result * 23 + Validator.GetHashCode();
                return result;
            }
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(PfeColumnAttribute other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return base.Equals(other) &&
                   Equals(this.Name, other.Name) &&
                   this.Type.Equals(other.Type) &&
                   this.Editable == other.Editable &&
                   this.Mandatory == other.Mandatory &&
                   this.Hidden == other.Hidden &&
                   this.Hideable == other.Hideable &&
                   this.Width == other.Width &&
                   Equals(this.Text, other.Text) &&
                   this.Xtype.Equals(other.Xtype) &&
                   this.Align.Equals(other.Align) &&
                   Equals(this.Tooltip, other.Tooltip) &&
                   this.Sortable == other.Sortable &&
                   this.SortDirection.Equals(other.SortDirection) &&
                   this.Filterable == other.Filterable &&
                   Equals(this.Format, other.Format) &&
                   this.DecimalPlaces == other.DecimalPlaces &&
                   Equals(this.DataUrl, other.DataUrl) &&
                   Equals(this.DefaultValue, other.DefaultValue) &&
                   Equals(this.Description, other.Description) &&
                   this.Rank == other.Rank &&
                   this.ReadOnly == other.ReadOnly &&
                   this.MaxLength == other.MaxLength &&
                   this.Validator == other.Validator;
        }

        /// <summary>
        /// Returns a value that indicates whether this instance is equal to a specified
        /// object.
        /// </summary>
        /// <param name="obj">An <see cref="T:System.Object" /> to compare with this instance
        /// or null.</param>
        /// <returns>
        /// true if <paramref name="obj" /> equals the type and value of this instance;
        /// otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            var temp = obj as PfeColumnAttribute;
            if (temp == null)
            {
                return false;
            }
            return this.Equals(temp);
        }

        /// <summary>
        /// Gets the node action flag.
        /// </summary>
        /// <returns></returns>
        private PfeColumnAttributeFlag GetPfeColumnAttributeFlag()
        {
            PfeColumnAttributeFlag flag = PfeColumnAttributeFlag.None;

            if (this.Editable)
            {
                flag |= PfeColumnAttributeFlag.Editable;
            }
            if (this.ReadOnly)
            {
                flag |= PfeColumnAttributeFlag.ReadOnly;
            }
            if (this.Mandatory)
            {
                flag |= PfeColumnAttributeFlag.Mandatory;
            }
            if (this.Hidden)
            {
                flag |= PfeColumnAttributeFlag.Hidden;
            }
            if (this.Hideable)
            {
                flag |= PfeColumnAttributeFlag.Hiddeable;
            }
            if (this.Sortable)
            {
                flag |= PfeColumnAttributeFlag.Sortable;
            }
            if (this.Filterable)
            {
                flag |= PfeColumnAttributeFlag.Filterable;
            }
            return flag;
        }
    }
}