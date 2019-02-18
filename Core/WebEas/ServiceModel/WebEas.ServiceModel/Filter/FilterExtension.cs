using System;
using System.Collections.Generic;
using System.Linq;

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
        /// Appends the filter 'DatumPlatnosti is null or DatumPlatnosti > getdate()' to this filter with "AND"
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public static Filter AndNotDeleted(this Filter filter)
        {
            filter.And(Filter.OrElements(FilterElement.Null("DatumPlatnosti"), FilterElement.Custom("DatumPlatnosti > GETDATE()")));
            return filter;
        }
    }
}