using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// Represents simple filter condition (something like 'Column = Value')
    /// </summary>
    public class FilterElement : IFilterElement
    {
        /// <summary>
        /// Gets or sets the key (column).
        /// If the key="CUSTOM" means a special condition where Value is used only.
        /// </summary>
        public string Key { get; set; }

        private string parameterName;

        /// <summary>
        /// Gets or sets the name of the parameter.
        /// </summary>
        /// <value>The name of the parameter.</value>
        public string ParameterName
        {
            get
            {
                if (string.IsNullOrEmpty(this.parameterName))
                {
                    return this.Key.ToUpper();
                }
                return this.parameterName.ToUpper();
            }
            set
            {
                this.parameterName = value;
            }
        }

        /// <summary>
        /// Gets or sets the operator between Key and Value.
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets the data type of the value.
        /// Default is 'Unknown' which is treat as 'Text'
        /// </summary>
        public PfeDataType DataType { get; set; }

        public FilterElement()
        {
        }

        internal FilterElement(string key, FilterOperator op, object value)
        {
            this.Key = key;

            //support null value
            if (value == null)
            {
                if (op.Value == FilterOperator.Eq.Value || op.Value == FilterOperator.Null.Value)
                {
                    this.Operator = FilterOperator.Null.Value;
                }
                else
                {
                    this.Operator = FilterOperator.NotNull.Value;
                }
            }
            else
            {
                this.Operator = op.Value;
                this.DataType = GetPfeDataType(value.GetType());
                this.Value = value;
                // this.Parameters.Add(string.Format("@{0}", key), value);
                //if (this.DataType == PfeDataType.Boolean)
                //    this.Value = (bool)value ? "1" : "0";
                //else if (this.DataType == PfeDataType.DateTime)
                //    this.Value = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
                //else
                //    this.Value = value.ToString();
            }
        }

        internal FilterElement(string key, FilterOperator op, object value, PfeDataType type)
        {
            this.Key = key;

            //support null value
            if (value == null)
            {
                if (op.Value == FilterOperator.Eq.Value || op.Value == FilterOperator.Null.Value)
                {
                    this.Operator = FilterOperator.Null.Value;
                }
                else
                {
                    this.Operator = FilterOperator.NotNull.Value;
                }
            }
            else
            {
                this.Operator = op.Value;
                this.Value = value;
                this.DataType = type;
                // this.Parameters.Add(string.Format("@{0}", key), value);
            }
        }

        private FilterElement(string key, IEnumerable<string> values)
        {
            this.Key = key;
            this.Operator = FilterOperator.In.Value;
            this.Value = values.ToList();
            this.DataType = PfeDataType.Text;
        }

        private FilterElement(string key, IEnumerable<long> values)
        {
            this.Key = key;
            this.Operator = FilterOperator.In.Value;
            this.Value = values.ToList();
            this.DataType = PfeDataType.Number;
        }

        private FilterElement(string key, IEnumerable<int> values)
        {
            this.Key = key;
            this.Operator = FilterOperator.In.Value;
            this.Value = values.ToList();
            this.DataType = PfeDataType.Number;
        }

        private FilterElement(string key, IEnumerable<short> values)
        {
            this.Key = key;
            this.Operator = FilterOperator.In.Value;
            this.Value = values.ToList();
            this.DataType = PfeDataType.Number;
        }

        private FilterElement(string customQuery)
        {
            this.Key = "CUSTOM";
            this.Value = customQuery;
            this.DataType = PfeDataType.Unknown;
        }

        #region Static initializers

        /// <summary>
        /// Creates 'Equal to' FilterElement.
        /// </summary>
        /// <param name="key">The key (column)</param>
        /// <param name="value">The value.</param>
        public static FilterElement Eq(string key, object value, PfeDataType type = PfeDataType.Unknown)
        {
            return new FilterElement(key, FilterOperator.Eq, value, type);
        }

        /// <summary>
        /// Creates 'Not equal to' FilterElement.
        /// </summary>
        /// <param name="key">The key (column)</param>
        /// <param name="value">The value.</param>
        public static FilterElement NotEq(string key, object value, PfeDataType type = PfeDataType.Unknown)
        {
            return new FilterElement(key, FilterOperator.Ne, value, type);
        }

        /// <summary>
        /// Creates 'Greater then' FilterElement.
        /// </summary>
        /// <param name="key">The key (column)</param>
        /// <param name="value">The value.</param>
        public static FilterElement GreaterThan(string key, object value, PfeDataType type = PfeDataType.Unknown)
        {
            return new FilterElement(key, FilterOperator.Gt, value, type);
        }

        /// <summary>
        /// Creates 'Greater or equal to' FilterElement.
        /// </summary>
        /// <param name="key">The key (column)</param>
        /// <param name="value">The value.</param>
        public static FilterElement GreaterThanOrEq(string key, object value, PfeDataType type = PfeDataType.Unknown)
        {
            return new FilterElement(key, FilterOperator.Ge, value, type);
        }

        /// <summary>
        /// Creates 'Not greater then' FilterElement.
        /// </summary>
        /// <param name="key">The key (column)</param>
        /// <param name="value">The value.</param>
        public static FilterElement NotGreaterThan(string key, object value, PfeDataType type = PfeDataType.Unknown)
        {
            return new FilterElement(key, FilterOperator.Ng, value, type);
        }

        /// <summary>
        /// Creates 'Less then' FilterElement.
        /// </summary>
        /// <param name="key">The key (column)</param>
        /// <param name="value">The value.</param>
        public static FilterElement LessThan(string key, object value, PfeDataType type = PfeDataType.Unknown)
        {
            return new FilterElement(key, FilterOperator.Lt, value, type);
        }

        /// <summary>
        /// Creates 'Less or equal to' FilterElement.
        /// </summary>
        /// <param name="key">The key (column)</param>
        /// <param name="value">The value.</param>
        public static FilterElement LessThanOrEq(string key, object value, PfeDataType type = PfeDataType.Unknown)
        {
            return new FilterElement(key, FilterOperator.Le, value, type);
        }

        /// <summary>
        /// Creates 'Not less then' FilterElement.
        /// </summary>
        /// <param name="key">The key (column)</param>
        /// <param name="value">The value.</param>
        public static FilterElement NotLessThan(string key, object value, PfeDataType type = PfeDataType.Unknown)
        {
            return new FilterElement(key, FilterOperator.Nl, value, type);
        }

        /// <summary>
        /// Creates 'LIKE %value%' FilterElement.
        /// </summary>
        /// <param name="key">The key (column)</param>
        /// <param name="value">The value.</param>
        public static FilterElement Like(string key, object value)
        {
            return new FilterElement(key, FilterOperator.Like, value, PfeDataType.Text);
        }

        /// <summary>
        /// Creates 'NOT LIKE %value%' FilterElement.
        /// </summary>
        /// <param name="key">The key (column)</param>
        /// <param name="value">The value.</param>
        public static FilterElement NotLike(string key, object value)
        {
            return new FilterElement(key, FilterOperator.NotLike, value, PfeDataType.Text);
        }

        /// <summary>
        /// Creates 'IN (,,)' FilterElement for strings.
        /// </summary>
        /// <param name="key">The key (column)</param>
        /// <param name="value">list of string values</param>
        public static FilterElement In(string key, IEnumerable<string> values)
        {
            return new FilterElement(key, values);
        }

        /// <summary>
        /// Creates 'IN (,,)' FilterElement for numbers.
        /// </summary>
        /// <param name="key">The key (column)</param>
        /// <param name="valuee">list of number values</param>
        public static FilterElement In(string key, IEnumerable<long> values)
        {
            return new FilterElement(key, values);
        }

        /// <summary>
        /// Creates 'IN (,,)' FilterElement for numbers.
        /// </summary>
        /// <param name="key">The key (column)</param>
        /// <param name="valuee">list of number values</param>
        public static FilterElement In(string key, IEnumerable<int> values)
        {
            return new FilterElement(key, values);
        }

        /// <summary>
        /// Creates 'IN (,,)' FilterElement for numbers.
        /// </summary>
        /// <param name="key">The key (column)</param>
        /// <param name="valuee">list of number values</param>
        public static FilterElement In(string key, IEnumerable<short> values)
        {
            return new FilterElement(key, values);
        }

        /// <summary>
        /// Creates 'IS NULL' FilterElement.
        /// </summary>
        /// <param name="key">The key (column)</param>
        public static FilterElement Null(string key)
        {
            return new FilterElement(key, FilterOperator.Null, null, PfeDataType.Unknown);
        }

        /// <summary>
        /// Creates 'IS NOT NULL' FilterElement.
        /// </summary>
        /// <param name="key">The key (column)</param>
        public static FilterElement NotNull(string key)
        {
            return new FilterElement(key, FilterOperator.NotNull, null, PfeDataType.Unknown);
        }

        /// <summary>
        /// Create custom query string
        /// </summary>
        /// <param name="customQuery">The query string.</param>
        public static FilterElement Custom(string customQuery)
        {
            return new FilterElement(customQuery);
        }

        /// <summary>
        /// Create custom query string
        /// </summary>
        public static FilterElement Custom(string format, params object[] args)
        {
            return new FilterElement(string.Format(format, args));
        }

        #endregion

        #region IFilterElement Members

        private string connOperator;

        /// <summary>
        /// Gets or sets the SQL operator (AND/OR) used to concatenate this statement. Default is AND.
        /// </summary>
        string IFilterElement.ConnOperator
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

        public Filter ToFilter()
        {
            return new Filter(this);
        }

        /// <summary>
        /// Returns a new object with same values
        /// </summary>
        /// <returns></returns>
        IFilterElement IFilterElement.Clone()
        {
            var obj = new FilterElement()
            {
                Key = this.Key,
                Value = this.Value,
                connOperator = this.connOperator,
                Operator = this.Operator,
                DataType = this.DataType,
            };
            return obj;
        }

        void IFilterElement.ToSqlString(StringBuilder where)
        {
            if (this.Key == "CUSTOM")
            {
                where.Append(this.Value);
                return;
            }

            if (this.Key == "CUSTOM_PAROVANIE")
            {
                where.AppendFormat("D_BiznisEntita_Id_1 = @{0} OR D_BiznisEntita_Id_2 = @{0}", this.ParameterName);
                return;
            }
            if (this.Key == "CUSTOM_VZTAH")
            {
                where.AppendFormat("D_Osoba_Id_1 = @{0} OR D_Osoba_Id_2 = @{0}", this.ParameterName);
                return;
            }

            //column (quote if not yet)
            if (this.Key[0] == '[' || this.Key[0] == '"')
            {
                where.Append(this.Key).Append(" ");
            }
            else
            {
                where.Append("[").Append(this.Key).Append("] ");
            }

            //operator
            where.Append(this.Operator);

            //value
            if (this.Operator == FilterOperator.In.Value || this.Operator == FilterOperator.NotIn.Value)
            {
                //numbers?
                //if (this.DataType == PfeDataType.Number)
                //{
                //    where.Append(" (").Append(string.Join(",", this.InValues)).Append(")");
                //}
                //else
                //{
                //    where.Append(" ('").Append(string.Join("','", this.InValues)).Append("')");
                //}
                where.AppendFormat(" @{0}", this.ParameterName);
            }
            else if (this.Operator == FilterOperator.Null.Value || this.Operator == FilterOperator.NotNull.Value)
            {
                //just skip value..
            }
            else
            {
                //switch (this.DataType)
                //{
                //    case PfeDataType.Boolean:
                //    case PfeDataType.Number:
                //        where.Append(" ").Append(this.Value);
                //        break;
                //    default:
                //        string value = this.Value == null ? "" : this.Value.Replace("'", "''");
                //        where.Append(" '").Append(value).Append("'");
                //        break;
                //}
                where.AppendFormat(" @{0}", this.ParameterName);
            }
        }

        #endregion

        #region Static helper methods

        public static bool IsNumberType(Type type)
        {
            return "Byte,Int16,Int32,Int64,SByte,UInt16,UInt32,UInt64,".Contains(string.Format("{0},", type.Name));
        }

        public static PfeDataType GetPfeDataType(Type type)
        {
            if (type.Name == "String")
            {
                return PfeDataType.Text;
            }
            if (type.Name == "Boolean")
            {
                return PfeDataType.Boolean;
            }
            if (type.Name == "DateTime")
            {
                return PfeDataType.DateTime;
            }
            if ("Byte,Int16,Int32,Int64,SByte,UInt16,UInt32,UInt64,".Contains(string.Format("{0},", type.Name)))
            {
                return PfeDataType.Number;
            }
            //otherwise
            return PfeDataType.Unknown;
        }

        #endregion

        /// <summary>
        /// Returns a SQL string representing this filter condition.
        /// </summary>
        public override string ToString()
        {
            var where = new StringBuilder(50);

            //appends conn operator (if any)
            if (!string.IsNullOrEmpty(this.connOperator))
            {
                where.Append(this.connOperator).Append(' ');
            }

            //writes the SQL string
            ((IFilterElement)this).ToSqlString(where);

            return where.ToString();
        }

        /// <summary>
        /// Adds the parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public void AddParameters(Dictionary<string, object> parameters)
        {
            if (this.Operator == FilterOperator.Null.Value || this.Operator == FilterOperator.NotNull.Value || this.Key == "CUSTOM")
            {
                return;
            }

            if (parameters.ContainsKey(this.Key.ToUpper()))
            {
                if ((parameters[this.Key.ToUpper()] == null && this.Value == null) || parameters[this.Key.ToUpper()].Equals(this.Value))
                {
                    return;
                }

                string newName;
                int count = 0;
                do
                {
                    newName = string.Format("{0}__{1}", this.Key.ToUpper(), ++count);
                }
                while (parameters.ContainsKey(newName));
                this.ParameterName = newName.ToUpper();
            }
            parameters.Add(this.ParameterName, this.Value);
        }
    }
}