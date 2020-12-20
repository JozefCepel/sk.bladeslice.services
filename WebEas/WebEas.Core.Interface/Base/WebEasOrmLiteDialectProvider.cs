using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.SqlServer;
using WebEas.Core.OrmLiteConverters;
using ServiceStack.OrmLite.SqlServer.Converters;

namespace WebEas.Core.Base
{
    /// <summary>
    /// Customizovany dialect provider - Meni sposob ziskavania ID
    /// </summary>
    public class WebEasOrmLiteDialectProvider : SqlServerOrmLiteDialectProvider
    {
        private static readonly DateTime timeSpanOffset = new DateTime(1900, 01, 01);

        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasOrmLiteDialectProvider" /> class.
        /// </summary>
        public WebEasOrmLiteDialectProvider()
        {
            base.SelectIdentitySql = "SELECT @@IDENTITY AS 'Identity'";
            base.RegisterConverter<DateTime?>(new NullableSqlServerDateTimeConverter());
            base.RegisterConverter<TimeSpan>(new SqlServerTimeConverter());
            base.RegisterConverter<TimeSpan?>(new NullableSqlServerTimeConverter());
            base.RegisterConverter<bool?>(new NullableSqlServerBoolConverter());
            base.RegisterConverter<float?>(new NullableSqlServerFloatConverter());
            base.RegisterConverter<double?>(new NullableSqlServerDoubleConverter());
            base.RegisterConverter<decimal?>(new NullableSqlServerDecimalConverter());
        }
        /*
        /// <summary>
        /// Called when [after init column type map].
        /// </summary>
        public override void OnAfterInitColumnTypeMap()
        {
            base.OnAfterInitColumnTypeMap();

            this.DbTypeMap.Set<TimeSpan>(DbType.Time, this.TimeColumnDefinition);
            this.DbTypeMap.Set<TimeSpan?>(DbType.Time, this.TimeColumnDefinition);

            //throws unknown type exceptions in parameterized queries, e.g: p.DbType = DbType.SByte
            this.DbTypeMap.Set<sbyte>(DbType.Byte, this.IntColumnDefinition);
            this.DbTypeMap.Set<ushort>(DbType.Int16, this.IntColumnDefinition);
            this.DbTypeMap.Set<uint>(DbType.Int32, this.IntColumnDefinition);
            this.DbTypeMap.Set<ulong>(DbType.Int64, this.LongColumnDefinition);
        }
        
        /// <summary>
        /// Sets the db value.
        /// </summary>
        /// <param name="fieldDef">The field def.</param>
        /// <param name="reader">The reader.</param>
        /// <param name="colIndex">Index of the col.</param>
        /// <param name="instance">The instance.</param>
        public override void SetDbValue(FieldDefinition fieldDef, IDataReader reader, int colIndex, object instance)
        {
            try
            {
                if (fieldDef.IsRowVersion)
                {
                    var bytes = reader.GetValue(colIndex) as byte[];
                    if (bytes != null)
                    {
                        ulong ulongValue = OrmLiteUtils.ConvertToULong(bytes);
                        try
                        {
                            fieldDef.SetValueFn(instance, ulongValue);
                        }
                        catch (NullReferenceException)
                        {
                        }
                    }
                }
                else
                {
                    base.SetDbValue(fieldDef, reader, colIndex, instance);
                }
            }
            catch (Exception ex)
            {
                throw new WebEasException(string.Format("Error in setting value to {0} on {1}", fieldDef.Name, instance == null ? null : instance.GetType()), "Nastala chyba pri nastavovanie hodnoty", ex, colIndex, instance);
            }
        }
        
        /// <summary>
        /// Gets the type of the column db.
        /// </summary>
        /// <param name="valueType">Type of the value.</param>
        /// <returns></returns>
        public override DbType GetColumnDbType(Type valueType)
        {
            if (valueType.IsEnum)
            {
                return this.DbTypeMap.ColumnDbTypeMap[typeof(string)];
            }

            return this.DbTypeMap.ColumnDbTypeMap[valueType];
        }
        
        /// <summary>
        /// Converts the db value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public override object ConvertDbValue(object value, Type type)
        {
            try
            {
                if (value == null || value is DBNull)
                {
                    return null;
                }

                if (type == typeof(bool) && !(value is bool))
                {
                    int intVal = Convert.ToInt32(value.ToString());
                    return intVal != 0;
                }

                if (type == typeof(TimeSpan) && value is DateTime)
                {
                    var dateTimeValue = (DateTime)value;
                    return dateTimeValue - timeSpanOffset;
                }

                if (type == typeof(TimeSpan) && value is TimeSpan)
                {
                    return value;
                }

                if (this._ensureUtc && type == typeof(DateTime))
                {
                    object result = base.ConvertDbValue(value, type);
                    if (result is DateTime)
                    {
                        return DateTime.SpecifyKind((DateTime)result, DateTimeKind.Utc);
                    }
                    return result;
                }

                if (type == typeof(byte[]))
                {
                    return value;
                }

                return base.ConvertDbValue(value, type);
            }
            catch (Exception ex)
            {
                throw new WebEasException(string.Format("Error in converting {0} to {1}", value == null ? null : value.ToString(), type), ex);
            }
        }
        */
        /// <summary>
        /// Sets the parameter values.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="dbCmd">The db CMD.</param>
        /// <param name="obj">The obj.</param>
        public override void SetParameterValues<T>(IDbCommand dbCmd, object obj)
        {
            ModelDefinition modelDef = GetModel(typeof(T));
            Dictionary<string, FieldDefinition> fieldMap = base.GetFieldDefinitionMap(modelDef);

            foreach (IDataParameter p in dbCmd.Parameters)
            {
                FieldDefinition fieldDef;
                string fieldName = this.ToFieldName(p.ParameterName);
                fieldMap.TryGetValue(fieldName, out fieldDef);

                if (fieldDef == null)
                {
                    throw new ArgumentException(string.Format("Field Definition '{0}' was not found", fieldName));
                }

                this.SetParameterValue<T>(fieldDef, p, obj);
            }
        }

        /// <summary>
        /// Gets the field definition map.
        /// </summary>
        /// <param name="modelDef">The model def.</param>
        /// <returns></returns>
        public new Dictionary<string, FieldDefinition> GetFieldDefinitionMap(ModelDefinition modelDef)
        {
            return modelDef.GetFieldDefinitionMap(this.SanitizeFieldNameForParamName);
        }

        /// <summary>
        /// Sets the parameter value.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="fieldDef">The field def.</param>
        /// <param name="p">The p.</param>
        /// <param name="obj">The obj.</param>
        public override void SetParameterValue<T>(FieldDefinition fieldDef, IDataParameter p, object obj)
        {
            object value = this.GetValueOrDbNull<T>(fieldDef, obj);
            p.Value = value;
        }

        /// <summary>
        /// Gets the field value.
        /// </summary>
        /// <param name="fieldDef">The field def.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public new object GetFieldValue(FieldDefinition fieldDef, object value)
        {
            if (value != null)
            {
                if (fieldDef.IsRefType)
                {
                    //Let ADO.NET providers handle byte[]
                    if (fieldDef.FieldType == typeof(byte[]))
                    {
                        return value;
                    }
                    return this.StringSerializer.SerializeToString(value);
                }
                if (fieldDef.FieldType.IsEnum)
                {
                    string enumValue = this.StringSerializer.SerializeToString(value);
                    if (enumValue == null)
                    {
                        return null;
                    }

                    enumValue = enumValue.Trim('"');
                    long intEnum;
                    if (Int64.TryParse(enumValue, out intEnum))
                    {
                        return intEnum;
                    }

                    return enumValue;
                }
                //if (fieldDef.FieldType == typeof(TimeSpan))
                //{
                //    var timespan = (TimeSpan)value;
                //    return timespan.Ticks;
                //}
            }

            return value;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="fieldDef">The field def.</param>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        protected override object GetValue<T>(FieldDefinition fieldDef, object obj)
        {
            object value = obj is T
                           ? fieldDef.GetValue(obj)
                           : this.GetAnonValue(fieldDef, obj);

            return this.GetFieldValue(fieldDef, value);
        }
    }
}