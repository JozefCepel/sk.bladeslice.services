using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.OrmLite;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// 
    /// </summary>
    public class Filter : IFilterElement
    {
        private readonly List<IFilterElement> filterElements = new List<IFilterElement>();

        /// <summary>
        /// Gets the filter elements list.
        /// </summary>
        public List<IFilterElement> FilterElements
        {
            get
            {
                return this.filterElements;
            }
        }

        /// <summary>
        /// Determines whether this instance is empty.
        /// </summary>
        public bool IsEmpty()
        {
            return this.filterElements.Count == 0;
        }

        /// <summary>
        /// Adds the parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public void AddParameters(Dictionary<string, object> parameters)
        {
            foreach (IFilterElement el in FilterElements)
            {
                el.AddParameters(parameters);
            }
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        public Dictionary<string, Object> Parameters
        {
            get
            {
                var parameters = new Dictionary<string, object>();
                this.AddParameters(parameters);
                return parameters;
            }
        }

        #region Constructors

        /// <summary>
        /// Initializes a new empty instance of the <see cref="Filter" /> class.
        /// </summary>
        public Filter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Filter" /> class with specified condition.
        /// </summary>
        public Filter(IFilterElement condition)
        {
            //check null??
            this.filterElements.Add(condition);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Filter" /> class with one filter element 
        /// of 'Equal to' condition or 'IS NULL' condition (if value is null)
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="value">The value.</param>
        public Filter(string column, object value)
        {
            if (value == null)
            {
                this.filterElements.Add(FilterElement.Null(column));
            }
            else
            {
                this.filterElements.Add(new FilterElement(column, FilterOperator.Eq, value));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Filter" /> class with specified conditions.
        /// </summary>
        public Filter(IEnumerable<IFilterElement> conditions)
        {
            this.filterElements.AddRange(conditions);
        }

        #endregion

        #region Public methods (Append, Join)

        /// <summary>
        /// Appends specific condition to current instance
        /// </summary>
        /// <returns>Actual filter instance (this)</returns>
        public Filter Append(IFilterElement condition)
        {
            this.filterElements.Add(condition);
            return this;
        }

        /// <summary>
        /// Appends specific condition to current instance with AND
        /// </summary>
        /// <returns>Actual filter instance (this)</returns>
        public Filter And(IFilterElement condition)
        {
            condition.ConnOperator = FilterOperator.And.Value;
            this.filterElements.Add(condition);
            return this;
        }

        /// <summary>
        /// Appends specific condition to current instance with OR
        /// </summary>
        /// <returns>Actual filter instance (this)</returns>
        public Filter Or(IFilterElement condition)
        {
            condition.ConnOperator = FilterOperator.Or.Value;
            this.filterElements.Add(condition);
            return this;
        }

        /// <summary>
        /// Creates new compound filter joined this and other filter with specified operator
        /// </summary>
        /// <returns>New filter instance</returns>
        public Filter Join(FilterOperator and_or, Filter other)
        {
            other.ConnOperator = and_or.Value;
            return new Filter(new IFilterElement[] { this, other });
        }

        /// <summary>
        /// Creates new compound filter joined this and other filter.
        /// </summary>
        /// <returns>New filter instance</returns>
        public Filter Join(Filter other)
        {
            return new Filter(new IFilterElement[] { this, other });
        }

        #endregion

        #region Static initializers

        /// <summary>
        /// Creates a new instance of the <see cref="Filter" /> class with FilterElements connected with AND.
        /// </summary>
        public static Filter AndElements(params IFilterElement[] conditions)
        {
            return new Filter(conditions.Select(a => { a.ConnOperator = FilterOperator.And.Value; return a; }));
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Filter" /> class with FilterElements connected with OR.
        /// </summary>
        public static Filter OrElements(params IFilterElement[] conditions)
        {
            return new Filter(conditions.Select(a => { a.ConnOperator = FilterOperator.Or.Value; return a; }));
        }

        /// <summary>
        /// Builds the filter from PfeFilterUrl collection (obtained from FE)
        /// </summary>
        public static Filter BuildFilter(List<PfeFilterUrl> decodeFilters, ModelDefinitionEgov allFields, string dbCollate)
        {
            var filterStack = new Stack<Filter>();
            bool first = true;

            foreach (PfeFilter el in decodeFilters.Select(a => new PfeFilter(a)))
            {
                //Konverzia na SQL podporovany format datumu
                if ((el.Type == PfeDataType.Date)
                    && el.ComparisonOperator.ToLower() != "empty" && el.ComparisonOperator.ToLower() != "notempty"
                    && el.ComparisonOperator.ToLower() != "em" && el.ComparisonOperator.ToLower() != "en")
                {
                    //Pokial ide o Typ Date, tak odstranim akekolvek casove udaje, neskor sa bude convertovat na DATE aj DataField aby fungovali operatory
                    DateTime d = DateTime.Parse(el.Value.ToString());
                    el.Value = d.ToString("yyyyMMdd");
                }
                if ((el.Type == PfeDataType.DateTime)
                    && el.ComparisonOperator.ToLower() != "empty" && el.ComparisonOperator.ToLower() != "notempty"
                    && el.ComparisonOperator.ToLower() != "em" && el.ComparisonOperator.ToLower() != "en")
                {
                    DateTime d = DateTime.Parse(el.Value.ToString());
                    el.Value = d.ToString("yyyyMMdd HH:mm:00.000");   //"yyyyMMdd HH:mm:ss.mmm" - vznikaju problemy ak su tam aj sekundy. Formatovanim sa nikde v APP sekundy nezobrazuju 
                }

                if (el.Type == PfeDataType.Boolean)
                {
                    if (el.Field.ToLower() == "globalrecord")
                    {
                        // DCOMDEUSUAT-2085, DCOMDEUSUAT-2086: Fix filtrovania globalnych zaznamov
                        // vsetky triedy BaseTenantEntityNullable maju D_Tenant_Id a na FE pouzivaju [PfeColumn(Text = "Globálny záznam", Editable = false, Rank = 99, ReadOnly = true)]
                        el.Field = "D_Tenant_Id";

                        if (el.ComparisonOperator.ToLower() == "eq" || el.ComparisonOperator == "=")
                        {
                            el.ComparisonOperator = el.Value.Equals("1") ? "em" : "en";
                        }
                        else if (el.ComparisonOperator.ToLower() == "ne" || el.ComparisonOperator == "<>")
                        {
                            el.ComparisonOperator = el.Value.Equals("0") ? "em" : "en";
                        }

                    }
                    else if (el.Value.Equals("true")) //Osetrenie problemu vo FE, kedy posle pre boolean text "true"
                    {
                        el.Value = 1;
                    }
                    else if (el.Value.Equals("false")) //Osetrenie problemu vo FE, kedy posle pre boolean text "false"
                    {
                        el.Value = 0;
                    }
                }

                //Dohľadanie pripadného aliasu
                if (allFields != null)
                {
                    el.Field = allFields.FieldName(el.Field);

                    if (!el.Field.ToLower().Equals("inputsearchfield") && //Špeciálne kvôli performance je field "InputSearchField" otagovaný tiež ako [Ignored] ale filtrovať ho chcem
                        !el.Field.ToLower().Equals("globalrecord") &&     //Globálne záznamy sa filtrujú špeciálne
                        allFields.IgnoredFieldDefinitions.Any(item => item.FieldName.ToLower() == el.Field.ToLower()))
                    {
                        throw new WebEasValidationException(null, $"Filter obsahuje nedatabázové pole '{el.Field}', ktoré nie je možné filtrovať!");
                    }
                }

                if (first)
                {
                    var filter = new Filter();
                    filterStack.Push(filter);
                    first = false;

                    if (decodeFilters.Count > 1) //Pre istotu pridam este zatvorku - ak by boli nejake OR podmienky neozatvorkovane
                    {
                        var filter2 = new Filter();
                        filterStack.Peek().Append(filter2);
                        filterStack.Push(filter2);
                    }
                }
                else if (el.LeftBrace || el.LeftBorderBrace || el.LeftOuterBrace)
                {
                    var filter = new Filter
                    {
                        ConnOperator = el.LogicOperator
                    };
                    filterStack.Peek().Append(filter);
                    filterStack.Push(filter);
                }

                filterStack.Peek().Append(el.ToFilterElement(CanUseCollationOnField(dbCollate, el)));

                if (el.RightBrace || el.RightBorderBrace || el.RightOuterBrace)
                {
                    if (filterStack.Count > 1)
                    {
                        filterStack.Pop();
                    }
                    else
                    {
                        Filter prevroot = filterStack.Pop();
                        var filter = new Filter();
                        filter.Append(prevroot);
                        filterStack.Push(filter);
                    }
                }
            }

            //close unclosed braces
            while (filterStack.Count > 1)
            {
                filterStack.Pop();
            }

            Filter main = filterStack.Pop();

            //4.4.2016 - optimalizacia zrusena. Kedze sa nasledne este pridavaju rozne podmienky, tak ak su v tomto filtri OR tak to blbne
            
            //optimize filter if just nested without reason...
            //return main.FilterElements.Count == 1 && main.FilterElements[0] is Filter ? (Filter)main.FilterElements[0] : main;
            return main;
        }

        private static string CanUseCollationOnField(string dbCollate, PfeFilter el)
        {
            if (el.UseCollation && !string.IsNullOrEmpty(dbCollate))
            {
                switch (el.Field)
                {
                    case "PrijateOd": return dbCollate;
                    case "MenoNazov": return dbCollate;
                    case "PriezviskoMeno": return dbCollate;
                    case "Priezvisko": return dbCollate;
                    case "Meno": return dbCollate;
                    case "OsobaNazov": return dbCollate;
                    case "ZiadatelNazov": return dbCollate;
                    case "SparovanaOsoba": return dbCollate;
                    case "FormatovaneMeno": return dbCollate;
                    case "DanovnikFormatovaneMenoSort": return dbCollate;
                    case "UradnyDokumentNazov": return dbCollate;
                    case "UradnyDokumentRegistraturneCislo": return dbCollate;
                    case "UradnyDokumentVec": return dbCollate;
                    case "Adresa": return dbCollate;
                    case "Kategoria": return dbCollate;
                    case "LandUseName": return dbCollate;
                    case "Description": return dbCollate;
                    case "KN_Ownership": return dbCollate;
                    case "Nazov": return dbCollate;
                    case "DruhNazov": return dbCollate;
                    case "Popis": return dbCollate;
                    case "NazovBanky": return dbCollate;
                    case "PoleSablony": return dbCollate;
                    case "Predmet": return dbCollate;
                    default: return null;
                }
            }

            return null;
        }

        /// <summary>
        /// Builds the filter from 'WHERE ()' statement (generated by ServiceStack.OrmLite.SqlServer.SqlServerExpression)
        /// ..but allows to be used as initializer with custom sql in form like 'col1 = 1 or (col2 = 3 and col1 is null)'
        /// </summary>
        public static Filter FromSql(string wheresql)
        {
            if (string.IsNullOrEmpty(wheresql))
            {
                return null;
            }

            if (wheresql.StartsWith("WHERE ", StringComparison.InvariantCultureIgnoreCase))
            {
                if (wheresql[6] == '(')
                {
                    wheresql = wheresql.Substring(7, wheresql.Length - 8);
                }
                else
                {
                    wheresql = wheresql.Substring(6, wheresql.Length - 6);
                }
            }
            return new Filter(FilterElement.Custom(wheresql));
        }

        #endregion

        #region IFilterElement Members

        private string connOperator;

        /// <summary>
        /// Gets or sets the SQL operator (AND/OR) used to concatenate this statement. Default is AND.
        /// </summary>
        public string ConnOperator
        {
            get
            {
                return this.connOperator ?? "AND";
            }
            set
            {
                this.connOperator = string.IsNullOrEmpty(value) ? null : value;
            }
        }

        public IFilterElement Clone()
        {
            var filter = new Filter();
            foreach (IFilterElement el in this.filterElements)
            {
                filter.filterElements.Add(el.Clone());
            }
            return filter;
        }

        public Filter CloneAsFilter()
        {
            var filter = new Filter();
            foreach (IFilterElement el in this.filterElements)
            {
                filter.filterElements.Add(el.Clone());
            }
            return filter;
        }

        public Filter ToFilter()
        {
            return this;
        }

        public void ToSqlString(StringBuilder where)
        {
            this.ToSqlString(where, true);
        }

        public Dictionary<string,object> ToSqlString(StringBuilder where, bool with = true)
        {            
            if (this.filterElements.Count == 0)
            {
                return new Dictionary<string,object>();
            }
            var parameters = Parameters;
            if (with)
            {
                where.Append('(');
            }
            for (int i = 0; i < this.filterElements.Count; i++)
            {
                IFilterElement el = filterElements[i];
                if (i > 0)
                {
                    where.Append(' ').Append(el.ConnOperator).Append(' ');
                }
                //Dokonalejsie riesenie by bolo overit ci nema field Alias a do WHERE podmienky poslat Aliasovany stlpec
                //Aktualne sa to robi iba pre PFE filtre. Mozno to bude padat akekolvek ine filtre.
                //Dokonalejsie riesenie je pre SORT, tak sa to robi tak, ze sa prebehnu fieldy na zoradenie a ak maju Alias tak sa pouzije
                el.ToSqlString(where);
            }
            if (with)
            {
                where.Append(')');
            }
            return parameters;
        }

        #endregion

        /// <summary>
        /// Gets the filter 'DatumPlatnosti is null or DatumPlatnosti > getdate()' to this filter with "AND"
        /// </summary>
        /// <returns></returns>
        public static Filter NotDeleted()
        {
            return OrElements(FilterElement.Null("DatumPlatnosti"), FilterElement.Custom("DatumPlatnosti > GETDATE()"));
        }

        /// <summary>
        /// Returns a SQL string representing this filter condition.
        /// </summary>
        public override string ToString()
        {
            var where = new StringBuilder(50 * this.filterElements.Count);
            if (!string.IsNullOrEmpty(this.connOperator))
            {
                where.Append(this.connOperator).Append(' ');
            }
            this.ToSqlString(where);

            return where.ToString();
        }
    }
}