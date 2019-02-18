using System;
using ServiceStack.OrmLite.SqlServer.Converters;

namespace WebEas.Core.OrmLiteConverters
{
    public class NullableSqlServerDateTimeConverter : SqlServerDateTimeConverter
    {
        public override object FromDbValue(object value)
        {
            if (value != null)
            {
                return base.FromDbValue(value);
            }

            return null;
        }

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
