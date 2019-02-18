using ServiceStack.OrmLite.SqlServer.Converters;
using System;

namespace WebEas.Core.OrmLiteConverters
{

    public class NullableSqlServerTimeConverter : SqlServerTimeConverter
    {
        public override object ToDbValue(Type fieldType, object value)
        {
            if (value != null)
            {
                return base.ToDbValue(fieldType, value);
            }

            return null;
        }
    }
}