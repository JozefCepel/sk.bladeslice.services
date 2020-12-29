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
        /// <param name="name">Názov stĺpca</param>
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
        /// <param name="loadWhenVisible">Ak je dany stlpec visible tak sa bude selectovat z DB</param>
        public PfeColumnAttribute(string name, string text, int rank, bool hidden = false, bool? hideable = null, PfeDataType type = PfeDataType.Unknown, PfeXType xtype = PfeXType.Unknown,
            bool? editable = null, bool? mandatory = null, int width = 0, PfeAligment align = PfeAligment.Unknown, string tooltip = "", bool sortable = true, PfeOrder sortDirection = PfeOrder.Asc,
            bool filterable = true, string format = "0", int decimalPlaces = 2, string dataUrl = "", object defaultValue = null, bool? readOnly = null, int? maxlength = null, string description = "",
            bool loadWhenVisible = false)
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
            this.LoadWhenVisible = loadWhenVisible;
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
        /// Názov stĺpca
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
        /// Názov stĺpca, ktorý sa vygeneruje, default je pomenovanie property v class-e
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
                return editable.HasValue;
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
                return editable ?? true;
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
                return readOnly.HasValue;
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
                return readOnly ?? false;
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
                return mandatory.HasValue;
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
                return mandatory ?? false;
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
                return hidden.HasValue;
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
                return hidden ?? false;
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
                return hideable.HasValue;
            }
        }

        /// <summary>
        /// Ci je property nullable
        /// </summary>
        [IgnoreDataMember]
        public byte Nullable { get; set; }

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
                return hideable ?? true;
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
        /// Formatovany string ako sa budu v combe zobrazovat hodnoty.
        /// Napr: "{value};{ColName}" zobrazi data v dvoch columnoch, pricom AdditionalFields musi obsahovat ColName.
        /// Combo hlada text iba v ComboDisplayColumn hodnotach
        /// </summary>
        /// <value>Definícia</value>
        [DataMember(Name = "tpl")]
        public string Tpl { get; set; }

        /// <summary>
        /// FE bude volať combo službu až po zadaní minimálne tohoto počtu znakov (pre > 0)
        /// </summary>
        /// <value>Počet znakov</value>
        [DataMember(Name = "mcs")]
        public int MinCharSearch { get; set; }

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
        /// Ak je dany stlpec visible tak sa bude selectovat z DB, inac dotiahne default data (kvoli optimalizacii na DB https://jira.datalan.sk/browse/DTLNESAMINT-390)
        /// </summary>
        //[DataMember(Name = "lwv")]
        [IgnoreDataMember]
        public bool LoadWhenVisible { get; set; }

        /// <summary>
        /// Custom validacie na FE.
        /// </summary>
        [DataMember(Name = "vld")]
        public PfeValidator Validator { get; set; }

        /// <summary>
        /// Konfiguracia pre zobrazenie xtype SearchFieldSS a SearchFieldMS
        /// </summary>
        [DataMember(Name = "sfd")]
        public List<PfeSearchFieldDefinition> SearchFieldDefinition { get; set; }

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
        /// Ci ma byt combo pre tento stlpec v ribbon filtri iba so Single-vyberom
        /// </summary>
        //[DataMember(Name = "scf")] //Presunuté do FLAG-u
        [IgnoreDataMember]
        public bool SingleComboFilter { get; set; }

        /// <summary>
        /// Ak je True, je možné do comba vložiť vlastnú hodnotu
        /// </summary>
        //[DataMember(Name = "acv")] //Presunuté do FLAG-u
        [IgnoreDataMember]
        public bool AllowComboCustomValue { get; set; }

        /// <summary>
        /// Typ property
        /// </summary>
        /// <value>The type of the property.</value>
        [IgnoreDataMember]
        public PropertyInfo PropertyTypeInfo { get; set; }

        /// <summary>
        /// Do pfe.D_Pohlad.Data ukladame iba fieldy: Name, Rank, Width
        /// </summary>
        public bool DoNotSerializeProperty { get; set; }

        public bool ShouldSerializeText()
        {
            if (DoNotSerializeProperty)
            {
                return false;
            }

            return !string.IsNullOrEmpty(this.Text);
        }

        public bool ShouldSerializeTooltip()
        {
            if (DoNotSerializeProperty)
            {
                return false;
            }

            return !string.IsNullOrEmpty(Tooltip);
        }

        public bool ShouldSerializeFormat()
        {
            if (DoNotSerializeProperty)
            {
                return false;
            }

            return !string.IsNullOrEmpty(Format);
        }

        public bool ShouldSerializeDataUrl()
        {
            if (DoNotSerializeProperty)
            {
                return false;
            }

            return !string.IsNullOrEmpty(DataUrl);
        }

        public bool ShouldSerializeNameField()
        {
            if (DoNotSerializeProperty)
            {
                return false;
            }

            return !string.IsNullOrEmpty(NameField);
        }

        public bool ShouldSerializeIdField()
        {
            if (DoNotSerializeProperty)
            {
                return false;
            }

            return !string.IsNullOrEmpty(IdField);
        }

        public bool ShouldSerializeValueField()
        {
            if (DoNotSerializeProperty)
            {
                return false;
            }

            return !string.IsNullOrEmpty(ValueField);
        }

        public bool ShouldSerializeDisplayTpl()
        {
            if (DoNotSerializeProperty)
            {
                return false;
            }

            return !string.IsNullOrEmpty(Tpl);
        }

        public bool ShouldSerializeMinCharSearch()
        {
            if (DoNotSerializeProperty)
            {
                return false;
            }

            return MinCharSearch > 0;
        }

        public bool ShouldSerializeRequiredFields()
        {
            if (DoNotSerializeProperty)
            {
                return false;
            }

            return RequiredFields != null;
        }

        public bool ShouldSerializeDefaultValue()
        {
            if (DoNotSerializeProperty)
            {
                return false;
            }

            return DefaultValue != null;
        }

        public bool ShouldSerializeDescription()
        {
            if (DoNotSerializeProperty)
            {
                return false;
            }

            return !string.IsNullOrEmpty(Description);
        }

        public bool ShouldSerializeMaxLength()
        {
            if (DoNotSerializeProperty)
            {
                return false;
            }

            return MaxLength.HasValue;
        }

        public bool ShouldSerializeDecimalPlaces()
        {
            if (DoNotSerializeProperty)
            {
                return false;
            }

            return Xtype == PfeXType.Numberfield && this.IsDecimal && this.DecimalPlaces >= 0;
        }

        public bool ShouldSerializeType()
        {
            if (DoNotSerializeProperty)
            {
                return false;
            }

            return true;
        }

        public bool ShouldSerializeXtype()
        {
            if (DoNotSerializeProperty)
            {
                return false;
            }

            return true;
        }

        public bool ShouldSerializeAlign()
        {
            if (DoNotSerializeProperty)
            {
                return false;
            }

            return true;
        }

        public bool ShouldSerializeSortDirection()
        {
            if (DoNotSerializeProperty)
            {
                return false;
            }

            return true;
        }

        public bool ShouldSerializeValidator()
        {
            if (DoNotSerializeProperty)
            {
                return false;
            }

            return Validator != null;
        }

        public bool ShouldSerializeSearchFieldDefinition()
        {
            if (DoNotSerializeProperty)
            {
                return false;
            }

            return SearchFieldDefinition != null && SearchFieldDefinition.Any();
        }

        public bool ShouldSerializeFlag()
        {
            if (DoNotSerializeProperty)
            {
                return false;
            }

            return true;
        }

        public bool ShouldSerializeWidth()
        {
            //if (DoNotSerializeProperty)
            //{
            //    return false;
            //}

            return Width != default;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"Name: {Name}, Type: {Type}, Editable: {Editable}, Mandatory: {Mandatory}, Hidden: {Hidden}, Hideable: {Hideable}, Width: {Width}, Text: {Text}, Xtype: {Xtype}, Align: {Align}, Tooltip: {Tooltip}, Sortable: {Sortable}, SortDirection: {SortDirection}, Filterable: {Filterable}, Format: {Format}, DecimalPlaces: {DecimalPlaces}, DataUrl: {DataUrl}, DefaultValue: {DefaultValue}, Description: {Description}, Rank: {Rank}, ReadOnly: {ReadOnly}, MaxLength: {MaxLength}, Validator: {Validator}, SearchFieldDefinition: {SearchFieldDefinition}, SingleComboFilter: {SingleComboFilter}, AllowComboCustomValue: {AllowComboCustomValue}, Nullabe: {Nullable}, LoadWhenVisible: {LoadWhenVisible}";
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
                result = result * 23 + ((Name != null) ? Name.GetHashCode() : 0);
                result = result * 23 + Type.GetHashCode();
                result = result * 23 + Editable.GetHashCode();
                result = result * 23 + Mandatory.GetHashCode();
                result = result * 23 + Hidden.GetHashCode();
                result = result * 23 + Hideable.GetHashCode();
                result = result * 23 + Width.GetHashCode();
                result = result * 23 + ((Text != null) ? Text.GetHashCode() : 0);
                result = result * 23 + Xtype.GetHashCode();
                result = result * 23 + Align.GetHashCode();
                result = result * 23 + ((Tooltip != null) ? Tooltip.GetHashCode() : 0);
                result = result * 23 + Sortable.GetHashCode();
                result = result * 23 + SortDirection.GetHashCode();
                result = result * 23 + Filterable.GetHashCode();
                result = result * 23 + ((Format != null) ? Format.GetHashCode() : 0);
                result = result * 23 + DecimalPlaces.GetHashCode();
                result = result * 23 + ((DataUrl != null) ? DataUrl.GetHashCode() : 0);
                result = result * 23 + ((DefaultValue != null) ? DefaultValue.GetHashCode() : 0);
                result = result * 23 + ((Description != null) ? Description.GetHashCode() : 0);
                result = result * 23 + Rank.GetHashCode();
                result = result * 23 + ReadOnly.GetHashCode();
                result = result * 23 + MaxLength.GetHashCode();
                result = result * 23 + ((Validator != null) ? Validator.GetHashCode() : 0);
                result = result * 23 + ((SearchFieldDefinition != null) ? SearchFieldDefinition.GetHashCode() : 0);
                result = result * 23 + SingleComboFilter.GetHashCode();
                result = result * 23 + AllowComboCustomValue.GetHashCode();
                result = result * 23 + LoadWhenVisible.GetHashCode();
                result = result * 23 + Nullable.GetHashCode();
                result = result * 23 + ((Tpl != null) ? Tpl.GetHashCode() : 0);
                result = result * 23 + MinCharSearch.GetHashCode();
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
                   Equals(Name, other.Name) &&
                   Type.Equals(other.Type) &&
                   Editable == other.Editable &&
                   Mandatory == other.Mandatory &&
                   Hidden == other.Hidden &&
                   Hideable == other.Hideable &&
                   Width == other.Width &&
                   Equals(Text, other.Text) &&
                   Xtype.Equals(other.Xtype) &&
                   Align.Equals(other.Align) &&
                   Equals(Tooltip, other.Tooltip) &&
                   Sortable == other.Sortable &&
                   SortDirection.Equals(other.SortDirection) &&
                   Filterable == other.Filterable &&
                   Equals(Format, other.Format) &&
                   DecimalPlaces == other.DecimalPlaces &&
                   Equals(DataUrl, other.DataUrl) &&
                   Equals(DefaultValue, other.DefaultValue) &&
                   Equals(Description, other.Description) &&
                   Rank == other.Rank &&
                   ReadOnly == other.ReadOnly &&
                   MaxLength == other.MaxLength &&
                   Validator == other.Validator &&
                   SearchFieldDefinition == other.SearchFieldDefinition &&
                   Nullable == other.Nullable &&
                   SingleComboFilter == other.SingleComboFilter &&
                   AllowComboCustomValue == other.AllowComboCustomValue &&
                   LoadWhenVisible == other.LoadWhenVisible &&
                   Tpl == other.Tpl &&
                   MinCharSearch == other.MinCharSearch;
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
            return Equals(temp);
        }

        /// <summary>
        /// Gets the node action flag.
        /// </summary>
        /// <returns></returns>
        private PfeColumnAttributeFlag GetPfeColumnAttributeFlag()
        {
            PfeColumnAttributeFlag flag = PfeColumnAttributeFlag.None;

            if (Editable)
            {
                flag |= PfeColumnAttributeFlag.Editable;
            }
            if (ReadOnly)
            {
                flag |= PfeColumnAttributeFlag.ReadOnly;
            }
            if (Mandatory)
            {
                flag |= PfeColumnAttributeFlag.Mandatory;
            }
            if (Hidden)
            {
                flag |= PfeColumnAttributeFlag.Hidden;
            }
            if (Hideable)
            {
                flag |= PfeColumnAttributeFlag.Hiddeable;
            }
            if (Sortable)
            {
                flag |= PfeColumnAttributeFlag.Sortable;
            }
            if (Filterable)
            {
                flag |= PfeColumnAttributeFlag.Filterable;
            }
            if (SingleComboFilter)
            {
                flag |= PfeColumnAttributeFlag.SingleComboFilter;
            }
            if (AllowComboCustomValue)
            {
                flag |= PfeColumnAttributeFlag.AllowComboCustomValue;
            }
            if (Nullable == 1)
            {
                flag |= PfeColumnAttributeFlag.Nullable;
            }
            if (LoadWhenVisible)
            {
                flag |= PfeColumnAttributeFlag.LoadWhenVisible;
            }
            return flag;
        }
    }
}