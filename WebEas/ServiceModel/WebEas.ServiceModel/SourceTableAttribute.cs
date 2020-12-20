using System;
using System.Linq;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// Definícia zdrojovej tabulky pohladu (db view-u).
    /// Pouzitie pre zistenie ci je nastavene logovanie v danej tabulke na nejaky stlpec
    /// (nastavenie: reg.C_LoggingConfig, data: reg.D_Logging)
    /// Tymto atributom vieme povedat, ze dana trieda ktora je namapovana na db VIEW pouziva ako zdroj data z nasledovnej DB tabulky
    /// V ojedinelych pripadoch je potrebne zadat tento atribut viackrat, popripade prestavit aj PrimaryKey.
    /// priklad: prilohy k priznaniam (dap/osoby) beru data z dvoch tabuliek - D_PriznPril a D_PriznPril_Osoba, 
    /// pricom ako PK je nastaveny stlpec z D_PriznPril_Osoba, ale D_PriznPril ma iny PK...
    /// </summary>
    /// <remarks>
    /// Nenastavujte PrimaryKey pokial to nie je treba.
    /// T.j. iba ak primary key v danej triede (viewu) nezodpoveda prim.klucu v ref.tabulke specifikovanej tu.
    /// Tato trieda sa pouziva v RepositoryBase.GetTableLogging
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class SourceTableAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SourceTableAttribute" /> class.
        /// </summary>
        /// <param name="name">The schema.name of table.</param>
        public SourceTableAttribute(string schemaAndName)
        {
            int idx = schemaAndName.IndexOf('.');
            this.Schema = schemaAndName.Substring(0, idx);
            this.Table = schemaAndName.Substring(idx + 1);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceTableAttribute" /> class.
        /// </summary>
        /// <param name="name">The schema.name of table.</param>
        public SourceTableAttribute(string schema, string table)
        {
            this.Schema = schema;
            this.Table = table;
        }

        /// <summary>
        /// Gets or sets the table name.
        /// </summary>
        /// <value>The name of table.</value>
        public string Table { get; set; }

        /// <summary>
        /// Gets or sets the table schema.
        /// </summary>
        /// <value>The db schema.</value>
        public string Schema { get; set; }

        /// <summary>
        /// Gets or sets the table primary key to be used.
        /// By default is taken from model, but in some situations another column may be needed.
        /// </summary>
        /// <value>The column name.</value>
        public string PrimaryKey { get; set; }
    }
}