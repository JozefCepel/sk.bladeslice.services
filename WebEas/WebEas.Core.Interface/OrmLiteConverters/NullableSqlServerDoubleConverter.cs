using ServiceStack.OrmLite.SqlServer.Converters;
using System;

namespace WebEas.Core.OrmLiteConverters
{
    class NullableSqlServerDoubleConverter : SqlServerDoubleConverter
    {
        public override object FromDbValue(Type fieldType, object value)
        {
            if (value != null)
            {
                return base.FromDbValue(fieldType, value);
            }

            return null;
        }

        public override object ToDbValue(Type fieldType, object value)
        {
            if (value != null)
            {
                return base.ToDbValue(fieldType, value);
            }

            return null;
        }

        public override string ToQuotedString(Type fieldType, object value)
        {
            if (value != null)
            {
                return base.ToQuotedString(fieldType, value);
            }

            return null;
        }
    }
}
