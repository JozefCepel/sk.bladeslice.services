using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel.Reg.Types;

namespace WebEas.ServiceModel
{
    /// <summary>
    ///
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    [DataContract(Name = "Combo")]
    public class PfeComboAttribute : Attribute
    {
        private string idColumnTmp;

        /// <summary>
        /// Initializes a new instance of the <see cref="PfeComboAttribute" /> class.
        /// </summary>
        public PfeComboAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PfeComboAttribute" /> class.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        public PfeComboAttribute(string sql)
        {
            Sql = sql;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PfeComboAttribute" /> class.
        /// </summary>
        /// <param name="tableType">table type</param>
        /// <param name="valueColumn">Stlpec v DB tabulke pre zobrazovanu hodnotu (Value)</param>
        /// <param name="idColumn">Meno ID stlpca v combo entite. (Default Identity field v entite)</param>
        /// <param name="additionalFields"> Doplni do zoznamu stlpcu okrem Id a Value dalsie stlpce</param>
        /// <param name="customSortSqlExp">Vlastne sortovanie pre combo cez SQL. Bez ORDER BY</param>
        /// <param name="filterByOrsPravo">Filtrovanie hodnot na zaklade ORS prava, "OrsPravo > Read"</param>
        /// <param name="singleComboFilter">Single combo filter v ribbone</param>
        /// <param name="allowComboCustomValue">Ak je True, je možné do comba vložiť vlastnú hodnotu</param>
        /// <param name="displayTpl">Formatovany string ako sa budu v combe zobrazovat hodnoty.</param>
        /// <param name="minCharSearch">Formatovany string ako sa budu v combe zobrazovat hodnoty.</param>
        public PfeComboAttribute(Type tableType, string idColumn = null, string comboDisplayColumn = null, string comboIdColumn = null, 
                                 string[] additionalFields = null, bool filterByOrsPravo = false, string customSortSqlExp = null,
                                 bool singleComboFilter = false, bool allowComboCustomValue = false, string displayTpl = null, int minCharSearch = 0)
        {
            TableType = tableType;
            IdColumn = idColumn;
            ComboDisplayColumn = comboDisplayColumn;
            ComboIdColumn = comboIdColumn;
            AdditionalFields = additionalFields;
            CustomSortSqlExp = customSortSqlExp;
            FilterByOrsPravo = filterByOrsPravo;
            SingleComboFilter = singleComboFilter;
            AllowComboCustomValue = allowComboCustomValue;
            DisplayTpl = displayTpl;
            MinCharSearch = minCharSearch;
        }

        /// <summary>
        /// Initializes a new predefined instance of the <see cref="PfeComboAttribute" /> class used in classes based on BaseEvidenciaView.
        /// </summary>
        /// <param name="stavovyPriestor">stavovy priestor</param>
        /// <param name="idColumn">The id column.</param>
        public PfeComboAttribute(StavovyPriestorEnum stavovyPriestor)
        {
            StavovyPriestor = stavovyPriestor;
            ComboDisplayColumn = "Nazov";
            ComboIdColumn = "C_StavEntity_Id";
            TableType = typeof(Office.Egov.Reg.Types.StavEntity);
            AdditionalWhereSql = string.Format("C_StavovyPriestor_Id = {0}", ((int)stavovyPriestor).ToString());
        }

        /// <summary>
        /// Gets or sets the stavovy priestor.
        /// </summary>
        /// <value>The stavovy priestor.</value>
        public StavovyPriestorEnum? StavovyPriestor { get; private set; }

        /// <summary>
        /// Spusti kompletny sql bez vytvarania, Id a Value stlpce musi obsahovat
        /// </summary>
        /// <value>The SQL.</value>
        public string Sql { get; set; }

        /// <summary>
        /// Doplni do sql dany where
        /// </summary>
        public string AdditionalWhereSql { get; set; }

        /// <summary>
        /// Meno display stlpca v combo entite (zobrazovana hodnota)
        /// </summary>
        /// <value>Nazov stlpca</value>
        public string ComboDisplayColumn { get; set; }

        /// <summary>
        /// Meno ID stlpca v combo entite. (Id, Default je IdColumn z gridu)
        /// </summary>
        /// <value>Nazov stlpca</value>
        public string ComboIdColumn { get; set; }

        /// <summary>
        /// Meno ID stlpca v gride na ktory sa binduje combo (Id v DB, default je ComboIdColumn)
        /// </summary>
        /// <value>Nazov property</value>
        public string IdColumn
        {
            get
            {
                return string.IsNullOrEmpty(idColumnTmp) ? ComboIdColumn : idColumnTmp;
            }
            set
            {
                idColumnTmp = value;
            }
        }

        /// <summary>
        /// Formatovany string ako sa budu v combe zobrazovat hodnoty.
        /// Napr: "{value} - {ColName}" zobrazi string "ComboDisplayColumn - XXXX", pricom AdditionalFields musi obsahovat ColName.
        /// Combo hlada text iba v ComboDisplayColumn hodnotach
        /// </summary>
        /// <value>Definícia</value>
        public string DisplayTpl { get; set; }

        /// <summary>
        /// FE bude volať combo službu až po zadaní minimálne tohoto počtu znakov (pre > 0)
        /// </summary>
        /// <value>Počet znakov</value>
        public int MinCharSearch { get; set; }

        /// <summary>
        /// Gets or sets the type of the table.
        /// </summary>
        /// <value>The type of the table.</value>
        public Type TableType { get; set; }

        /// <summary>
        /// Doplni do zoznamu stlpcu okrem Id a Value dalsie stlpce
        /// </summary>
        public string[] AdditionalFields { get; set; }

        /// <summary>
        /// Vlastne sortovanie pre combo cez SQL. Bez ORDER BY
        /// </summary>
        public string CustomSortSqlExp { get; set; }

        /// <summary>
        /// Ak je True, combo bude filtrovane na polozky, ktore maju OrsPravo minimalne "Update"
        /// </summary>
        public bool FilterByOrsPravo { get; set; }

        /// <summary>
        /// Ci ma byt combo pre tento stlpec v ribbon filtri iba so Single-vyberom
        /// </summary>
        public bool SingleComboFilter { get; set; }
        
        /// <summary>
        /// Ak je True, je možné do comba vložiť vlastnú hodnotu
        /// </summary>
        public bool AllowComboCustomValue { get; set; }
    }
}