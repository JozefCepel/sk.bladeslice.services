using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [DataContract(Name = "Filter")]
    public class PfeFilter : IEquatable<PfeFilter>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PfeFilter" /> class.
        /// </summary>
        public PfeFilter()
        { 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PfeFilter" /> class from PfeFilterUrl instance.
        /// </summary>
        public PfeFilter(PfeFilterUrl urlFilter)
        {
            string[] resString = urlFilter.Config.Split(';');

            ComparisonOperator = resString[0];
            LogicOperator = resString[1];
            Field = resString[2];
            LeftBrace = resString[3] == "1";
            RightBrace = resString[4] == "1";
            OnlyIdentical = resString[5] == "1";
            Value = urlFilter.Value;
            Today = (urlFilter.Value == "TODAY");

            //data type (allow not specified to be backward compatible)
            PfeDataType datatype;
            if (resString.Length > 6)
            {
                if (!Enum.TryParse<PfeDataType>(resString[6], true, out datatype))
                {
                    datatype = PfeDataType.Text;
                }
            }
            else
            {
                datatype = PfeDataType.Unknown;
            }

            LeftBorderBrace = false;
            if (resString.Length > 7)
            {
                LeftBorderBrace = resString[7] == "1";
            }
            RightBorderBrace = false;
            if (resString.Length > 8)
            {
                RightBorderBrace = resString[8] == "1";
            }
            UseCollation = false;
            if (resString.Length > 9)
            {
                UseCollation = resString[9] == "1";
            }

            LeftOuterBrace = false;
            if (resString.Length > 10)
            {
                LeftOuterBrace = resString[10] == "1";
            }
            RightOuterBrace = false;
            if (resString.Length > 11)
            {
                RightOuterBrace = resString[11] == "1";
            }

            //force number pre _ID stlpce
            object val;
            if (datatype != PfeDataType.Number && (this.Field.EndsWith("Id", StringComparison.InvariantCultureIgnoreCase) || this.Field.ToLower().Contains("_id_")) && IsNumber(urlFilter.Value, out val))
            {
                this.Value = val;
                datatype = PfeDataType.Number;
            }

            // Overenie, ci zadana hodnota je datumom s casom a casovou zonou a formatovanie do pozadovaneho tvaru
            if (!string.IsNullOrEmpty(urlFilter.Value) &&
                (datatype == PfeDataType.Date || datatype == PfeDataType.DateTime || datatype == PfeDataType.Time || datatype == PfeDataType.Unknown))
            {
                DateTime dateTime = DateTime.Now;
                if (this.Today)
                {
                    this.Value = dateTime.Date;
                    datatype = PfeDataType.Date;

                }
                else if (DateTime.TryParse(urlFilter.Value, out dateTime))
                {
                    if (datatype == PfeDataType.Unknown)
                    {
                        datatype = PfeDataType.DateTime;
                    }
                    this.Value = datatype == PfeDataType.DateTime
                                 ? dateTime
                                 : (datatype == PfeDataType.Date ? dateTime.Date : (object)dateTime.TimeOfDay);
                }
            }

            this.Type = datatype;
        }

        /// <summary>
        /// Gets or sets the field.
        /// </summary>
        /// <value>The field.</value>
        [DataMember(Name = "field")]
        public string Field { get; set; }

        /// <summary>
        /// Gets or sets the comparison operator.
        /// </summary>
        /// <value>The comparison operator.</value>
        [DataMember(Name = "comparisonOperator")]
        public string ComparisonOperator { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        [DataMember(Name = "value")]
        public object Value { get; set; }

        [IgnoreDataMember]
        public bool Today { get; set; }

        /// <summary>
        /// Gets or sets the only identical. 
        /// </summary>
        /// <value>The only identical.</value>
        [DataMember(Name = "onlyIdentical")]
        public bool OnlyIdentical { get; set; }

        /// <summary>
        /// Gets or sets the logic operator. 
        /// </summary>
        /// <value>The logic operator.</value>
        [DataMember(Name = "logicOperator")]
        public string LogicOperator { get; set; }

        /// <summary>
        /// Gets or sets the left brace. 
        /// </summary>
        /// <value>The left brace.</value>
        [DataMember(Name = "leftBrace")]
        public bool LeftBrace { get; set; }

        /// <summary>
        /// Gets or sets the right brace. 
        /// </summary>
        /// <value>The right brace.</value>
        [DataMember(Name = "rightBrace")]
        public bool RightBrace { get; set; }

        /// <summary>
        /// Gets or sets the left border brace. 
        /// </summary>
        /// <value>The left border brace.</value>
        [DataMember(Name = "leftBorderBrace")]
        public bool LeftBorderBrace { get; set; }

        /// <summary>
        /// Gets or sets the right border brace. 
        /// </summary>
        /// <value>The right border brace.</value>
        [DataMember(Name = "rightBorderBrace")]
        public bool RightBorderBrace { get; set; }

        /// <summary>
        /// Gets or sets whatever to use collation
        /// </summary>
        [DataMember(Name = "useCollation")]
        public bool UseCollation { get; set; }

        /// <summary>
        /// Nejaka dalsia lava zatvorka
        /// </summary>
        /// <value>The left outer brace.</value>
        [DataMember(Name = "leftOuterBrace")]
        public bool LeftOuterBrace { get; set; }

        /// <summary>
        /// Nejaka dalsia prava zatvorka
        /// </summary>
        /// <value>The right outer brace.</value>
        [DataMember(Name = "rightOuterBrace")]
        public bool RightOuterBrace { get; set; }

        /// <summary>
        /// Typ filtra
        /// </summary>
        [DataMember(Name = "type")]
        public PfeDataType Type { get; set; }

        /// <summary>
        /// Returns FilterElement representation of actual instance or null if not valid.
        /// NOTE: The LogicOperator is applied.
        /// </summary>
        public FilterElement ToFilterElement(string dbCollate)
        {
            string sqlOp = "";
            FilterOperator comparison = null;
            string custom = null;
            string value;
            if (Value != null && Value is IEnumerable<object> enumerableValue)
            {
                value = enumerableValue.Select(x => x.ToString()).Join(",");
            }
            else
            {
                value = Value?.ToString();
            }

            switch (this.ComparisonOperator.ToLower())
            {
                case "eq": comparison = FilterOperator.Eq; sqlOp = "="; break;
                case "ne": comparison = FilterOperator.Ne; sqlOp = "<>"; break;
                case "gt":
                case ">": comparison = FilterOperator.Gt; sqlOp = ">"; break;
                case "lt":
                case "<": comparison = FilterOperator.Lt; sqlOp = "<"; break;
                case "ge":
                case ">=": comparison = FilterOperator.Ge; sqlOp = ">="; break;
                case "le":
                case "<=": comparison = FilterOperator.Le; sqlOp = "<="; break;

                case "=":   //Support pre staru verziu
                    comparison = (!this.OnlyIdentical && this.Type == PfeDataType.Text)
                                 ? FilterOperator.Like
                                 : FilterOperator.Eq;
                    if (comparison.Value == FilterOperator.Like.Value)
                        value = string.Format("%{0}%", this.Value);
                    sqlOp = "=";
                    break;

                case "<>": //Support pre staru verziu
                    comparison = (!this.OnlyIdentical && this.Type == PfeDataType.Text)
                                 ? FilterOperator.NotLike
                                 : FilterOperator.Ne;
                    if (comparison.Value == FilterOperator.NotLike.Value)
                        value = string.Format("%{0}%", this.Value);
                    sqlOp = "<>";
                    break;

                case "sw":
                    comparison = FilterOperator.Like;
                    value = string.Format("{0}%", this.Value);
                    break;

                case "ew":
                    comparison = FilterOperator.Like;
                    value = string.Format("%{0}", this.Value);
                    break;
                    
                case "co":
                    comparison = FilterOperator.Like;
                    value = string.Format("%{0}%", this.Value);
                    break;

                case "nc":
                    comparison = FilterOperator.NotLike;
                    value = string.Format("%{0}%", this.Value);
                    break;

                case "em":
                case "empty":
                    if (this.Type == PfeDataType.Text || this.Type == PfeDataType.Unknown)
                    {
                        custom = string.Format("(ISNULL([{0}], '') = '')", this.Field);
                    }
                    else
                    {
                        comparison = FilterOperator.Null;
                    }
                    break;

                case "en":
                case "notempty":
                    if (this.Type == PfeDataType.Text || this.Type == PfeDataType.Unknown)
                    {
                        custom = string.Format("(ISNULL([{0}], '') <> '')", this.Field);
                    }
                    else
                    {
                        comparison = FilterOperator.NotNull;
                    }
                    break;
            }

            //invalid data?
            if (comparison == null && custom == null)
                return null;

            if (Type == PfeDataType.Text && !string.IsNullOrEmpty(dbCollate) && string.IsNullOrEmpty(custom))
                custom = $"{Field} {comparison.Value} '{value}' collate {dbCollate}";

            if (this.Type == PfeDataType.Date && sqlOp != "") 
                custom = string.Format("CAST({0} AS DATE) {1} '{2}'", this.Field, sqlOp, this.Value);
            
            FilterElement condition = (custom == null)
                                      ? new FilterElement(this.Field, comparison, value, this.Type)
                                      : FilterElement.Custom(custom);
            ((IFilterElement)condition).ConnOperator = this.LogicOperator;
            return condition;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"Field: {this.Field}, ComparisonOperator: {this.ComparisonOperator}, Value: {this.Value}, OnlyIdentical: {this.OnlyIdentical}, LogicOperator: {this.LogicOperator}, LeftBrace: {this.LeftBrace}, RightBrace: {this.RightBrace}, Type: {this.Type}, LeftBorderBrace: {this.LeftBorderBrace}, RightBorderBrace: {this.RightBorderBrace}, UseCollation: {this.UseCollation}, LeftOuterBrace: {this.LeftOuterBrace}, RightOuterBrace: {this.RightOuterBrace}";
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
                result = result * 23 + this.Field.GetHashCode();
                result = result * 23 + this.ComparisonOperator.GetHashCode();
                result = result * 23 + this.Value.GetHashCode();
                result = result * 23 + this.OnlyIdentical.GetHashCode();
                result = result * 23 + this.LogicOperator.GetHashCode();
                result = result * 23 + this.LeftBrace.GetHashCode();
                result = result * 23 + this.RightBrace.GetHashCode();
                result = result * 23 + this.Type.GetHashCode();
                result = result * 23 + this.LeftBorderBrace.GetHashCode();
                result = result * 23 + this.RightBorderBrace.GetHashCode();
                result = result * 23 + this.UseCollation.GetHashCode();
                result = result * 23 + this.LeftOuterBrace.GetHashCode();
                result = result * 23 + this.RightOuterBrace.GetHashCode();
                return result;
            }
        }

        /// <summary>
        /// Equalses the specified other to the current instance.
        /// </summary>
        /// <param name="other">The other PfeFilter.</param>
        /// <returns>true if the specified PfeFilter is equal to the current; otherwise false.</returns>
        public bool Equals(PfeFilter other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(Field, other.Field) &&
                   Equals(ComparisonOperator, other.ComparisonOperator) &&
                   Equals(Value, other.Value) &&
                   OnlyIdentical == other.OnlyIdentical &&
                   Equals(LogicOperator, other.LogicOperator) &&
                   LeftBrace == other.LeftBrace &&
                   RightBrace == other.RightBrace &&
                   Type == other.Type &&
                   LeftBorderBrace == other.LeftBorderBrace &&
                   RightBorderBrace == other.RightBorderBrace &&
                   UseCollation == other.UseCollation &&
                   LeftOuterBrace == other.LeftOuterBrace &&
                   RightOuterBrace == other.RightOuterBrace;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise false.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as PfeFilter);
        }

        private static bool IsNumber(string s, out object value)
        {
            value = null;
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }
            //first make it simple
            long l;
            if (long.TryParse(s, out l))
            {
                value = l;
                return true;
            }
            //otherwise try other variations
            decimal d;
            bool ok = decimal.TryParse(s, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out d);
            if (!ok)
            {
                ok = decimal.TryParse(s, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out d);
            }
            if (ok)
            {
                value = d;
            }
            return ok;
        }
    }
}