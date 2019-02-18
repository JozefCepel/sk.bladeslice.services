using System;
using System.Linq;
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
        private string nameColumn;

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
            this.Sql = sql;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PfeComboAttribute" /> class.
        /// </summary>
        /// <param name="valueColumn">The value column.</param>
        /// <param name="idColumn">The id column.</param>
        public PfeComboAttribute(Type tableType, string nameColumn = null, string valueColumn = null, string idColumn = null)
        {
            this.DisplayColumn = valueColumn;
            this.IdColumnCombo = idColumn;
            this.TableType = tableType;
            this.NameColumn = nameColumn;
        }

        /// <summary>
        /// Initializes a new predefined instance of the <see cref="PfeComboAttribute" /> class used in classes based on BaseEvidenciaView.
        /// </summary>
        /// <param name="stavovyPriestor">stavovy priestor</param>
        /// <param name="idColumn">The id column.</param>
        public PfeComboAttribute(StavovyPriestorEnum stavovyPriestor)
        {
            this.StavovyPriestor = stavovyPriestor;
            this.DisplayColumn = "Nazov";
            this.IdColumnCombo = "C_StavEntity_Id";
            this.TableType = typeof(WebEas.ServiceModel.Office.Egov.Reg.Types.StavEntity);
            this.AdditionalWhereSql = string.Format("C_StavovyPriestor_Id = {0}", ((int)stavovyPriestor).ToString());
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
        /// Doplni do where dany sql
        /// </summary>
        /// <value>The additional where SQL.</value>
        public string AdditionalWhereSql { get; set; }

        /// <summary>
        /// Stlpec v DB tabulke pre zobrazovanu hodnotu (Value)
        /// </summary>
        /// <value>Nazov stlpca</value>
        public string DisplayColumn { get; set; }

        /// <summary>
        /// Meno ID stlpca v combo entite. (Default Identity field v entite)
        /// </summary>
        /// <value>Nazov stlpca</value>
        public string IdColumnCombo { get; set; }

        /// <summary>
        /// Nazov property na ktoru sa binduje (default IdColumn)
        /// </summary>
        /// <value>Nazov property</value>
        public string NameColumn
        {
            get
            {
                return String.IsNullOrEmpty(this.nameColumn) ? this.IdColumnCombo : this.nameColumn;
            }
            set
            {
                this.nameColumn = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of the table.
        /// </summary>
        /// <value>The type of the table.</value>
        public Type TableType { get; set; }
    }
}