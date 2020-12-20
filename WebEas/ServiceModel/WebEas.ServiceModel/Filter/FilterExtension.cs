using System.Collections.Generic;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// 
    /// </summary>
    public static class FilterExtension
    {
        /// <summary>
        /// Appends the 'Is equal to' condition to this filter with "AND"
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="column">The column.</param>
        /// <param name="value">The value.</param>
        public static Filter AndEq(this Filter filter, string column, object value)
        {
            return filter.And(new FilterElement(column, FilterOperator.Eq, value));
        }

        /// <summary>
        /// Appends the 'Is equal to' condition to this filter with "OR"
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="column">The column.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Filter OrEq(this Filter filter, string column, object value)
        {
            return filter.Or(new FilterElement(column, FilterOperator.Eq, value));
        }

        /// <summary>
        /// Appends the '[NOT] NULL' condition to this filter with "AND"
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="column">The column.</param>
        /// <param name="not">true for NOT NULL comparison</param>
        /// <returns></returns>
        public static Filter AndNull(this Filter filter, string column, bool not)
        {
            return filter.And(not ? FilterElement.NotNull(column) : FilterElement.Null(column));
        }

        /// <summary>
        /// Appends the filter Filter.NotDeleted() to this filter with "AND"
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public static Filter AndNotDeleted(this Filter filter)
        {
            filter.And(Filter.NotDeleted());
            return filter;
        }

        /// <summary>
        /// Get FilterElement by key name
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="key">Name of key</param>
        /// <returns></returns>
        public static FilterElement GetFilterElementByKeyName(this Filter filter, string key)
        {
            foreach (var filterElement in filter.FilterElements)
            {
                if (filterElement is FilterElement el)
                {
                    if (el.Key == key)
                    {
                        return el;
                    }
                }
                else if (filterElement is Filter fel)
                {
                    return GetFilterElementByKeyName(fel, key);
                }
            }

            return null;
        }

        /// <summary>
        /// Get GetFieldValueAndRemove by sFieldName
        /// </summary>
        /// <param name="values">Returned values.</param>
        /// <param name="operators">Returned operators</param>
        /// <param name="sFieldName">Referenced FilterElement field name</param>
        /// <returns></returns>
        public static void GetFieldValueAndRemove(this Filter filter, Dictionary<string, string> values, Dictionary<string, string> operators, string sFieldName)
        {
            Dictionary<string, IFilterElement> forRemove = new Dictionary<string, IFilterElement>();
            for (int i = 0; i < filter.FilterElements.Count; i++)
            {
                if (filter.FilterElements[i] is FilterElement el)
                {
                    if (el.Key.ToString().ToLower().Equals(sFieldName.ToLower()))
                    {
                        values[el.Value.ToString()] = el.Value.ToString();
                        operators[el.Value.ToString()] = el.Operator;
                        forRemove[i.ToString()] = filter.FilterElements[i];
                    }
                }
                else
                {
                    Filter f = (Filter)filter.FilterElements[i];
                    GetFieldValueAndRemove(f, values, operators, sFieldName);
                    if (f.FilterElements.Count == 0)
                        forRemove[i.ToString()] = filter.FilterElements[i];
                }
            }

            foreach (KeyValuePair<string, IFilterElement> d in forRemove)
            {
                filter.FilterElements.Remove(d.Value);
            }
        }

        /// <summary>
        /// Get Remove by sFieldName
        /// </summary>
        /// <param name="sFieldName">Referenced FilterElement field name</param>
        /// <returns></returns>
        public static void Remove(this Filter filter, string sFieldName)
        {
            Dictionary<string, IFilterElement> forRemove = new Dictionary<string, IFilterElement>();
            for (int i = 0; i < filter.FilterElements.Count; i++)
            {
                if (filter.FilterElements[i] is FilterElement el)
                {
                    if (el.Key.ToString().ToLower().Equals(sFieldName.ToLower()))
                    {
                        forRemove[i.ToString()] = filter.FilterElements[i];
                    }
                }
                else
                {
                    Filter f = (Filter)filter.FilterElements[i];
                    Remove(f, sFieldName);
                    if (f.FilterElements.Count == 0)
                        forRemove[i.ToString()] = filter.FilterElements[i];
                }
            }

            foreach (KeyValuePair<string, IFilterElement> d in forRemove)
            {
                filter.FilterElements.Remove(d.Value);
            }
        }


        /// <summary>
        /// Change parameter
        /// </summary>
        /// <param name="findString">String to find</param>
        /// <param name="replaceString">Replace with string</param>
        /// <returns></returns>
        public static void ChangeFilterCondition(this Filter filter, string findString, string replaceString)
        {
            for (int i = 0; i < filter.FilterElements.Count; i++)
            {
                if (filter.FilterElements[i] is FilterElement el)
                {
                    if (el.Value.ToString().Contains(findString))
                    {
                        el.Value = el.Value.ToString().Replace(findString, replaceString);
                    }
                }
                else
                {
                    Filter f = (Filter)filter.FilterElements[i];
                    ChangeFilterCondition(f, findString, replaceString);
                }
            }
        }
    }
}