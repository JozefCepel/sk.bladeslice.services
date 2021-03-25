using CloneExtensions;
using Diacritics.Extensions;
using ServiceStack;
using ServiceStack.DataAnnotations;
using ServiceStack.Logging;
using ServiceStack.OrmLite;
using ServiceStack.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Office.Egov.Reg.Types;

namespace WebEas.ServiceInterface
{
    public static class RepositoryExtension
    {
        private static Dictionary<Type, ModelDefinitionEgov> typeModelDefinitionMap;

        private static readonly ILog log = LogManager.GetLogger(typeof(RepositoryExtension));

        static RepositoryExtension()
        {
            RepositoryExtension.typeModelDefinitionMap = new Dictionary<Type, ModelDefinitionEgov>();
        }

        /// <summary>
        /// Gets the name of the primary key.
        /// </summary>
        /// <param name="sourceType">Type of the source.</param>
        /// <returns></returns>
        public static string GetPrimaryKeyName(this Type sourceType)
        {
            foreach (PropertyInfo property in sourceType.GetProperties())
            {
                object[] attrs = property.GetCustomAttributes(typeof(PrimaryKeyAttribute), true);
                if (attrs != null && attrs.Length != 0)
                {
                    return GetPropertyAlias(property);
                }
            }
            return null;
        }

        /// <summary>
        /// Lasts the message.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public static string LastMessage(this Exception ex)
        {
            if (ex.InnerException != null)
            {
                return LastMessage(ex.InnerException);
            }
            return ex.Message;
        }

        /// <summary>
        /// Gets the model definition.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <returns></returns>
        public static ModelDefinitionEgov GetModelDefinition(this Type modelType)
        {

            if (typeModelDefinitionMap.TryGetValue(modelType, out ModelDefinitionEgov modelDef))
            {
                return modelDef;
            }
            if (modelType.IsValueType || modelType == typeof(string))
            {
                return null;
            }

            AliasAttribute modelAliasAttr = modelType.FirstAttribute<AliasAttribute>();
            SchemaAttribute schemaAttr = modelType.FirstAttribute<SchemaAttribute>();

            PreCreateTableAttribute preCreate = modelType.FirstAttribute<PreCreateTableAttribute>();
            PostCreateTableAttribute postCreate = modelType.FirstAttribute<PostCreateTableAttribute>();
            PreDropTableAttribute preDrop = modelType.FirstAttribute<PreDropTableAttribute>();
            PostDropTableAttribute postDrop = modelType.FirstAttribute<PostDropTableAttribute>();

            modelDef = new ModelDefinitionEgov
            {
                ModelType = modelType,
                Name = modelType.Name,
                Alias = modelAliasAttr?.Name,
                Schema = schemaAttr?.Name,
                PreCreateTableSql = preCreate?.Sql,
                PostCreateTableSql = postCreate?.Sql,
                PreDropTableSql = preDrop?.Sql,
                PostDropTableSql = postDrop?.Sql,
            };

            modelDef.CompositeIndexes.AddRange(
                modelType.GetCustomAttributes(typeof(CompositeIndexAttribute), true)
                         .ToList()
                         .ConvertAll(x => (CompositeIndexAttribute)x));

            List<PropertyInfo> objProperties = modelType.GetProperties(
                                                             BindingFlags.Public | BindingFlags.Instance).ToList();

            bool hasPkAttr = objProperties.Any(p => p.HasAttribute<PrimaryKeyAttribute>());

            bool hasIdField = CheckForIdField(objProperties);

            int i = 0;
            foreach (PropertyInfo propertyInfo in objProperties)
            {
                if (propertyInfo.GetIndexParameters().Length > 0)
                {
                    continue; //Is Indexer
                }

                SequenceAttribute sequenceAttr = propertyInfo.FirstAttribute<SequenceAttribute>();
                ComputeAttribute computeAttr = propertyInfo.FirstAttribute<ComputeAttribute>();
                DecimalLengthAttribute decimalAttribute = propertyInfo.FirstAttribute<DecimalLengthAttribute>();
                BelongToAttribute belongToAttribute = propertyInfo.FirstAttribute<BelongToAttribute>();
                bool isFirst = i++ == 0;

                bool isPrimaryKey = (!hasPkAttr && (propertyInfo.Name == OrmLiteConfig.IdField || (!hasIdField && isFirst))) ||
                                    propertyInfo.HasAttributeNamed(typeof(PrimaryKeyAttribute).Name);

                bool isRowVersion = propertyInfo.Name == ModelDefinition.RowVersionName &&
                                    propertyInfo.PropertyType == typeof(ulong);

                bool isNullableType = IsNullableType(propertyInfo.PropertyType);

                bool isNullable = (!propertyInfo.PropertyType.IsValueType &&
                                   !propertyInfo.HasAttributeNamed(typeof(RequiredAttribute).Name)) ||
                                  isNullableType;

                Type propertyType = isNullableType
                                    ? Nullable.GetUnderlyingType(propertyInfo.PropertyType)
                                    : propertyInfo.PropertyType;

                Type treatAsType = null;
                if (propertyType.IsEnumFlags())
                {
                    treatAsType = Enum.GetUnderlyingType(propertyType);
                }

                //if (propertyType == typeof(TimeSpan))
                //{
                //    treatAsType = typeof(long);
                //}

                AliasAttribute aliasAttr = propertyInfo.FirstAttribute<AliasAttribute>();

                IndexAttribute indexAttr = propertyInfo.FirstAttribute<IndexAttribute>();
                bool isIndex = indexAttr != null;
                bool isUnique = isIndex && indexAttr.Unique;

                UniqueConstraintAttribute constraintAttr = propertyInfo.FirstAttribute<UniqueConstraintAttribute>();
                bool isUniqueConstraint = constraintAttr != null;

                StringLengthAttribute stringLengthAttr = propertyInfo.CalculateStringLength(decimalAttribute);

                DefaultAttribute defaultValueAttr = propertyInfo.FirstAttribute<DefaultAttribute>();

                ReferencesAttribute referencesAttr = propertyInfo.FirstAttribute<ReferencesAttribute>();
                ReferenceAttribute referenceAttr = propertyInfo.FirstAttribute<ReferenceAttribute>();
                ForeignKeyAttribute foreignKeyAttr = propertyInfo.FirstAttribute<ForeignKeyAttribute>();
                CustomFieldAttribute customFieldAttr = propertyInfo.FirstAttribute<CustomFieldAttribute>();

                var fieldDefinition = new FieldDefinition
                {
                    Name = propertyInfo.Name,
                    Alias = aliasAttr?.Name,
                    FieldType = propertyType,
                    TreatAsType = treatAsType,
                    PropertyInfo = propertyInfo,
                    IsNullable = isNullable,
                    IsPrimaryKey = isPrimaryKey,
                    AutoIncrement =
                                   isPrimaryKey &&
                                   propertyInfo.HasAttributeNamed(typeof(AutoIncrementAttribute).Name),
                    IsIndexed = isIndex,
                    IsUniqueIndex = isUnique,
                    IsUniqueConstraint = isUniqueConstraint,
                    IsClustered = indexAttr != null && indexAttr.Clustered,
                    IsNonClustered = indexAttr != null && indexAttr.NonClustered,
                    IsRowVersion = isRowVersion,
                    FieldLength = stringLengthAttr != null
                                  ? stringLengthAttr.MaximumLength
                                  : (int?)null,
                    DefaultValue = defaultValueAttr?.DefaultValue,
                    ForeignKey = foreignKeyAttr == null
                                 ? referencesAttr != null ? new ServiceStack.OrmLite.ForeignKeyConstraint(referencesAttr.Type) : null
                                 : new ServiceStack.OrmLite.ForeignKeyConstraint(foreignKeyAttr.Type,
                                     foreignKeyAttr.OnDelete,
                                     foreignKeyAttr.OnUpdate,
                                     foreignKeyAttr.ForeignKeyName),
                    IsReference = referenceAttr != null && propertyType.IsClass,
                    GetValueFn = propertyInfo.CreateGetter(),
                    SetValueFn = propertyInfo.CreateSetter(),
                    Sequence = sequenceAttr != null ? sequenceAttr.Name : string.Empty,
                    IsComputed = computeAttr != null,
                    ComputeExpression = computeAttr != null ? computeAttr.Expression : string.Empty,
                    Scale = decimalAttribute != null ? decimalAttribute.Scale : (int?)null,
                    BelongToModelName = belongToAttribute?.BelongToTableType.GetModelDefinition().ModelName,
                    CustomFieldDefinition = customFieldAttr?.Sql,
                    IsRefType = propertyType.IsRefType(),
                };

                bool isIgnored = propertyInfo.HasAttributeNamed(typeof(IgnoreAttribute).Name) ||
                                 fieldDefinition.IsReference;
                if (isIgnored)
                {
                    modelDef.IgnoredFieldDefinitions.Add(fieldDefinition);
                }
                else
                {
                    modelDef.FieldDefinitions.Add(fieldDefinition);
                }

                if (isRowVersion)
                {
                    modelDef.RowVersion = fieldDefinition;
                }
            }

            modelDef.AfterInit();

            Dictionary<Type, ModelDefinitionEgov> snapshot, newCache;
            do
            {
                snapshot = typeModelDefinitionMap;
                newCache = new Dictionary<Type, ModelDefinitionEgov>(typeModelDefinitionMap)
                {
                    [modelType] = modelDef
                };
            }
            while (!ReferenceEquals(
                Interlocked.CompareExchange(ref typeModelDefinitionMap, newCache, snapshot), snapshot));

            LicenseUtils.AssertValidUsage(LicenseFeature.OrmLite, QuotaType.Tables, typeModelDefinitionMap.Count);

            return modelDef;
        }

        /// <summary>
        /// Checks for id field.
        /// </summary>
        /// <param name="objProperties">The obj properties.</param>
        /// <returns></returns>
        internal static bool CheckForIdField(IEnumerable<PropertyInfo> objProperties)
        {
            // Not using Linq.Where() and manually iterating through objProperties just to avoid dependencies on System.Xml??
            foreach (PropertyInfo objProperty in objProperties)
            {
                if (objProperty.Name != OrmLiteConfig.IdField)
                {
                    continue;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Calculates the length of the string.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="decimalAttribute">The decimal attribute.</param>
        /// <returns></returns>
        public static StringLengthAttribute CalculateStringLength(this PropertyInfo propertyInfo, DecimalLengthAttribute decimalAttribute)
        {
            StringLengthAttribute attr = propertyInfo.FirstAttribute<StringLengthAttribute>();
            if (attr != null)
            {
                return attr;
            }

            System.ComponentModel.DataAnnotations.StringLengthAttribute componentAttr = propertyInfo.FirstAttribute<System.ComponentModel.DataAnnotations.StringLengthAttribute>();
            if (componentAttr != null)
            {
                return new StringLengthAttribute(componentAttr.MaximumLength);
            }

            return decimalAttribute != null ? new StringLengthAttribute(decimalAttribute.Precision) : null;
        }

        /// <summary>
        /// Removes invalid chars. from filename
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string CleanFileName(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return filename;
            }

            string regexSearch = string.Format("{0}{1}", new string(Path.GetInvalidFileNameChars()), new string(Path.GetInvalidPathChars()));
            var r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(filename, "_");
        }

        internal static void SetParameters(this IDbCommand dbCmd, object anonType, bool excludeDefaults)
        {
            dbCmd.Parameters.Clear();
            // lastQueryType = null;
            if (anonType == null)
            {
                return;
            }

            PropertyInfo[] pis = anonType.GetType().GetSerializableProperties();

            foreach (PropertyInfo pi in pis)
            {
                MethodInfo mi = pi.GetGetMethod();
                if (mi == null)
                {
                    continue;
                }

                object value = mi.Invoke(anonType, new object[0]);
                if (excludeDefaults && value == null)
                {
                    continue;
                }

                IDbDataParameter p = dbCmd.CreateParameter();

                p.ParameterName = pi.Name;
                dbCmd.GetDialectProvider().InitDbParam(p, pi.PropertyType);
                p.Direction = ParameterDirection.Input;
                p.Value = value ?? DBNull.Value;
                dbCmd.Parameters.Add(p);
            }
        }

        /// <summary>
        /// Runs the procedure.
        /// </summary>
        /// <param name="dbCmd">The db CMD.</param>
        /// <param name="name">The name.</param>
        /// <param name="inParams">The in params.</param>
        /// <param name="excludeDefaults">The exclude defaults.</param>
        public static void RunProcedure(this IDbCommand dbCmd, string name, object inParams = null, bool excludeDefaults = false)
        {
            dbCmd.CommandType = CommandType.StoredProcedure;
            dbCmd.CommandText = name;
            dbCmd.CommandTimeout = OrmLiteConfig.CommandTimeout;

            SetParameters(dbCmd, inParams, excludeDefaults);

            dbCmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Sets the parameter values.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="dbCmd">The db CMD.</param>
        /// <param name="obj">The obj.</param>
        public static void SetParameterValues<T>(IDbCommand dbCmd, object obj)
        {
            ModelDefinitionEgov modelDef = obj.GetType().GetModelDefinition();
            Dictionary<string, FieldDefinition> fieldMap = OrmLiteConfig.DialectProvider.GetFieldDefinitionMap(modelDef);

            foreach (IDataParameter p in dbCmd.Parameters)
            {
                string fieldName = OrmLiteConfig.DialectProvider.ToFieldName(p.ParameterName);
                fieldMap.TryGetValue(fieldName, out FieldDefinition fieldDef);

                if (fieldDef == null)
                {
                    throw new ArgumentException("Field Definition '{0}' was not found".Fmt(fieldName));
                }
                else if (OrmLiteConfig.DialectProvider is ServiceStack.OrmLite.SqlServer.SqlServerOrmLiteDialectProvider)
                {
                    ((ServiceStack.OrmLite.SqlServer.SqlServerOrmLiteDialectProvider)OrmLiteConfig.DialectProvider).SetParameterValue<T>(fieldDef, p, obj);
                }
                else
                {
                    throw new NotSupportedException("Dialect provider is not supported");
                }
                //  ((OrmLiteDialectProviderBase<IOrmLiteDialectProvider>)OrmLiteConfig.DialectProvider).SetParameterValue<T>(fieldDef, p, obj);
            }
        }

        /// <summary>
        /// Inserts the data.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="dbCmd">The db CMD.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="obj">The obj.</param>
        public static long InsertData<T>(this IDbCommand dbCmd, WebEasRepositoryBase repository, T obj) where T : class, IBaseEntity
        {
            if (obj == null)
            {
                throw new ArgumentNullException(string.Format("Object of type {0} cannot be null", typeof(T)));
            }

            try
            {
                Validator.Validate(obj, Operation.Insert, repository);

                ModelDefinitionEgov modelDefinition = obj.GetType().GetModelDefinition();
                obj.PrepareInsertParameters(repository);
                OrmLiteConfig.InsertFilter?.Invoke(dbCmd, obj);

                PrepareParameterizedInsertStatement<T>(OrmLiteConfig.DialectProvider, obj, dbCmd);
                SetParameterValues<T>(dbCmd, obj);

                long retVal = 0;
                if (!modelDefinition.HasAutoIncrementId)
                {
                    if (log.IsDebugEnabled)
                    {
                        log.Debug(dbCmd.CommandText);
                    }
                    retVal = dbCmd.ExecuteNonQuery();
                }
                else
                {
                    if (log.IsDebugEnabled)
                    {
                        log.Debug(dbCmd.CommandText);
                    }

                    long num = dbCmd.ExecLongScalar(dbCmd.CommandText + dbCmd.GetDialectProvider().GetLastInsertIdSqlSuffix<T>());
                    object obj1 = OrmLiteConfig.DialectProvider.ConvertNumber(modelDefinition.PrimaryKey.FieldType, num);
                    modelDefinition.PrimaryKey.SetValueFn(obj, obj1);
                    retVal = num;
                }

                //LogChangesToIap(repository, obj, OperacieEnum.Insert, retVal);
                repository.EgovLogChangesToIap(dbCmd, obj, Operation.Insert, retVal);
                repository.RemoveFromCache(obj.GetType());

                return retVal;
            }
            catch (SqlException ex)
            {
                throw new WebEasSqlException(string.Format("Cannot insert data to {0}", obj.GetType()), ex, obj);
            }
            catch (Exception ex)
            {
                throw new WebEasException(string.Format("Cannot insert data to {0}", obj == null ? typeof(T) : obj.GetType()), ex);
            }
        }



        /// <summary>
        /// Inserts the data.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="dbCmd">The db CMD.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="objs">The objs.</param>
        public static void InsertData<T>(this IDbCommand dbCmd, WebEasRepositoryBase repository, params T[] objs) where T : class, IBaseEntity
        {
            dbCmd.InsertAllData<T>(repository, objs);
        }

        /// <summary>
        /// Inserts all data.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="dbCmd">The db CMD.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="objs">The objs.</param>
        public static void InsertAllData<T>(this IDbCommand dbCmd, WebEasRepositoryBase repository, IEnumerable<T> objs) where T : class, IBaseEntity
        {
            if (objs.Count() == 0)
            {
                return;
            }
            using IDbTransaction dbTransaction = dbCmd.Transaction == null ? dbCmd.Connection.BeginTransaction() : null;
            IOrmLiteDialectProvider dialectProvider = OrmLiteConfig.DialectProvider;
            PrepareParameterizedInsertStatement<T>(dialectProvider, objs.First(), dbCmd);
            foreach (T obj in objs)
            {
                try
                {
                    if (obj == null)
                    {
                        throw new ArgumentNullException(string.Format("Object of type {0} cannot be null", typeof(T)));
                    }
                    Validator.Validate(obj, Operation.Insert, repository);
                    obj.PrepareInsertParameters(repository);
                    OrmLiteConfig.InsertFilter?.Invoke(dbCmd, obj);
                    SetParameterValues<T>(dbCmd, obj);
                    if (log.IsDebugEnabled)
                    {
                        log.Debug(dbCmd.CommandText);
                    }
                    int id = dbCmd.ExecuteNonQuery();

                    //LogChangesToIap(repository, obj, OperacieEnum.Insert, id);


                    repository.EgovLogChangesToIap(dbCmd, obj, Operation.Insert, id);
                    repository.RemoveFromCache(obj.GetType());
                }
                catch (SqlException ex)
                {
                    throw new WebEasSqlException(string.Format("Cannot insert data to {0}", obj.GetType()), ex, obj);
                }
                catch (Exception ex)
                {
                    throw new WebEasException(string.Format("Cannot insert data to {0}", obj == null ? typeof(T) : obj.GetType()), ex);
                }
            }
            if (dbTransaction != null)
            {
                dbTransaction.Commit();
            }
        }

        /// <summary>
        /// Updates the specified db CMD.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="dbCmd">The db CMD.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static int UpdateData<T>(this IDbCommand dbCmd, WebEasRepositoryBase repository, T obj) where T : class, IBaseEntity
        {
            if (obj == null)
            {
                throw new ArgumentNullException(string.Format("Object of type {0} cannot be null", typeof(T)));
            }

            try
            {
                Validator.Validate(obj, Operation.Update, repository);
                if (obj is ITenantEntityNullable && ((ITenantEntityNullable)obj).D_Tenant_Id == null)
                {
                    if (repository.Session.AdminLevel != AdminLevel.SysAdmin)
                    {
                        throw new WebEasUnauthorizedAccessException();
                    }
                }
                // Kontrola konkurencie
                obj.CheckChangeDate(repository);

                OrmLiteConfig.UpdateFilter?.Invoke(dbCmd, obj);

                IOrmLiteDialectProvider dialectProvider = OrmLiteConfig.DialectProvider;

                var excludeFields = new List<string>
                {
                    "Vytvoril",
                    "DatumVytvorenia"
                };

                if (!obj.GetType().HasAttribute<TenantUpdatableAttribute>())
                {
                    excludeFields.Add("D_Tenant_Id");
                }

                //Fix: DatumZmeny a Zmenil musi byt nastaveny pred generovanim statementu.
                obj.PrepareUpdateParameters(repository);

                PrepareParameterizedUpdateStatement(dialectProvider, obj, dbCmd, excludeFields);

                if (string.IsNullOrEmpty(dbCmd.CommandText))
                {
                    return 0;
                }


                SetParameterValues<T>(dbCmd, obj);

                if (log.IsDebugEnabled)
                {
                    log.Debug(dbCmd.CommandText);
                }

                // log column value changes, create column value history (must be before the update command is executed..)
                LogColumnsChangesByConfig(repository, obj);
                // UPDATE data in table
                int result = dbCmd.ExecuteNonQuery();

                repository.EgovLogChangesToIap(dbCmd, obj, Operation.Update);
                repository.RemoveFromCache(obj.GetType());
                return result;
            }
            catch (SqlException ex)
            {
                throw new WebEasSqlException(string.Format("Cannot update data to {0}", obj.GetType()), ex, obj);
            }
            catch (Exception ex)
            {
                throw new WebEasException(string.Format("Cannot update data to {0}", obj == null ? typeof(T) : obj.GetType()), ex);
            }
        }

        /// <summary>
        /// Checks the change date.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="obj">The obj.</param>
        /// <param name="repository">The repository.</param>
        private static void CheckChangeDate<T>(this T obj, WebEasRepositoryBase repository) where T : class, IBaseEntity
        {
            if (obj.DatumZmeny == DateTime.MinValue)
            {
                ModelDefinitionEgov model = obj.GetType().GetModelDefinition();
                string sql = string.Format("SELECT DatumZmeny FROM {0}.{1} WHERE {2}={3}", model.Schema, model.Alias ?? model.Name, model.PrimaryKey.Alias ?? model.PrimaryKey.Name, obj.GetId());
                obj.DatumZmeny = repository.Db.SqlList<DateTime>(sql).FirstOrDefault();
            }

            if (obj.ChangeIdentifierDto.HasValue && obj.ChangeIdentifierDto.Value != obj.ChangeIdentifier)
            {
                throw new WebEasUnprocessableException(string.Format("Záznam typu {0} nie je aktuálny {1}/{2}", obj.GetType().Name, TimeSpan.FromMilliseconds(obj.ChangeIdentifierDto.Value), TimeSpan.FromMilliseconds(obj.ChangeIdentifier)), "Upravovaný záznam bol zmenený! Ak aj napriek tomu chcete vykonať zmeny je potrebné úpravu zopakovať.");
            }
        }

        /// <summary>
        /// Updates the specified db CMD.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="dbCmd">The db CMD.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="objs">The objs.</param>
        /// <returns></returns>
        public static int UpdateData<T>(this IDbCommand dbCmd, WebEasRepositoryBase repository, params T[] objs) where T : class, IBaseEntity
        {
            return dbCmd.UpdateAllData<T>(repository, objs);
        }

        /// <summary>
        /// Updates all.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="dbCmd">The db CMD.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="objs">The objs.</param>
        /// <returns></returns>
        public static int UpdateAllData<T>(this IDbCommand dbCmd, WebEasRepositoryBase repository, IEnumerable<T> objs) where T : class, IBaseEntity
        {
            int num = 0;

            if (objs.Count() == 0)
            {
                return 0;
            }

            var dialectProvider = OrmLiteConfig.DialectProvider;

            var excludeFields = new List<string>
            {
                "Vytvoril",
                "DatumVytvorenia"
            };

            if (!typeof(T).HasAttribute<TenantUpdatableAttribute>())
            {
                excludeFields.Add("D_Tenant_Id");
            }

            foreach (T obj in objs)
            {
                if (obj == null)
                {
                    throw new ArgumentNullException(string.Format("Object of type {0} cannot be null", typeof(T)));
                }
                try
                {
                    Validator.Validate(obj, Operation.Update, repository);
                    if (obj is ITenantEntityNullable && ((ITenantEntityNullable)obj).D_Tenant_Id == null)
                    {
                        if (repository.Session.AdminLevel != AdminLevel.SysAdmin)
                        {
                            throw new WebEasUnauthorizedAccessException();
                        }
                    }

                    obj.PrepareUpdateParameters(repository);
                    PrepareParameterizedUpdateStatement(dialectProvider, obj, dbCmd, excludeFields);

                    if (!string.IsNullOrEmpty(dbCmd.CommandText))
                    {
                        OrmLiteConfig.UpdateFilter?.Invoke(dbCmd, obj);
                        SetParameterValues<T>(dbCmd, obj);

                        // log column value changes, create column value history
                        LogColumnsChangesByConfig(repository, obj);
                        //LogChangesToIap(repository, obj, OperacieEnum.Update);

                        repository.EgovLogChangesToIap(dbCmd, obj, Operation.Update);

                        if (log.IsDebugEnabled)
                        {
                            log.Debug(dbCmd.CommandText);
                        }
                        num += dbCmd.ExecuteNonQuery();
                    }
                }
                catch (SqlException ex)
                {
                    throw new WebEasSqlException(string.Format("Cannot update data to {0}", obj.GetType()), ex, obj);
                }
                catch (Exception ex)
                {
                    throw new WebEasException(string.Format("Cannot update data to {0}", obj == null ? typeof(T) : obj.GetType()), ex);
                }
            }

            repository.RemoveFromCache(typeof(T));

            return num;
        }

        /// <summary>
        /// Deletes the specified db CMD.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="dbCmd">The db CMD.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static int DeleteData<T>(this IDbCommand dbCmd, WebEasRepositoryBase repository, bool safeDelete, params object[] id) where T : class, IBaseEntity
        {
            int deletedRows = 0;
            var dialectProvider = OrmLiteConfig.DialectProvider;
            string primaryKeyName = typeof(T).GetModelDefinition().FieldDefinitions.Find(p => p.IsPrimaryKey).FieldName;
            IEnumerable ids = id.First() is IEnumerable ? (IEnumerable)id.First() : id;

            foreach (var idecko in ids)
            {
                try
                {
                    long.TryParse(idecko.ToString(), out long longId);
                    var obj = (T)Activator.CreateInstance(typeof(T));
                    obj.PrepareDeleteParameters(repository);

                    if (safeDelete)
                    {
                        PrepareParameterizedSafeDeleteStatement<T>(dialectProvider, dbCmd, repository);
                    }
                    else
                    {
                        PrepareParameterizedDeleteStatement<T>(dialectProvider, dbCmd, repository);
                    }

                    if (string.IsNullOrEmpty(dbCmd.CommandText))
                    {
                        continue;
                    }

                    // set PK value
                    typeof(T).GetProperty(primaryKeyName).SetValue(obj, idecko);
                    OrmLiteConfig.DialectProvider.SetParameterValues<T>(dbCmd, obj);
                    Validator.Validate(obj, Operation.Delete, repository);

                    if (log.IsDebugEnabled)
                    {
                        log.Debug(dbCmd.CommandText);
                    }

                    deletedRows += dbCmd.ExecuteNonQuery();
                    repository.EgovLogChangesToIap<T>(dbCmd, null, Operation.Delete, longId);
                }
                catch (SqlException ex)
                {
                    throw new WebEasSqlException(string.Format("Cannot delete data to {0}", typeof(T)), ex, idecko);
                }
                catch (WebEasValidationException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new WebEasException(string.Format("Cannot delete data to {0}", typeof(T)), ex);
                }
            }

            repository.RemoveFromCache(typeof(T));
            //LogChangesToIap<T>(repository, null, OperacieEnum.Delete, longId);

            return deletedRows;
        }

        /// <summary>
        /// Clears the cache.
        /// </summary>
        public static void ClearCache()
        {
            RepositoryExtension.typeModelDefinitionMap = new Dictionary<Type, ModelDefinitionEgov>();
        }

        /// <summary>
        /// Determines whether [is ref type] [the specified field type].
        /// </summary>
        /// <param name="fieldType">Type of the field.</param>
        /// <returns></returns>
        public static bool IsRefType(this Type fieldType)
        {
            if (fieldType.UnderlyingSystemType.IsValueType)
            {
                if (!JsConfig.TreatValueAsRefTypes.Contains((fieldType.IsGenericType ? fieldType.GetGenericTypeDefinition() : fieldType)))
                {
                    return false;
                }
            }
            return fieldType != typeof(string);
        }

        public static IEnumerable<T> DatatableWhere<T>(this IEnumerable<T> collection, string expression)
        {
            Type type = typeof(T);
            var valuesPair = new Dictionary<object[], T>();
            var properties = type.GetProperties();

            var table = new DataTable();
            foreach (PropertyInfo info in properties)
            {
                table.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (T entity in collection)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    if (properties[i].PropertyType == typeof(String))
                    {
                        values[i] = properties[i].GetValue(entity);
                        if (values[i] != null && values[i].ToString().HasDiacritics())
                        {
                            values[i] = values[i].ToString().RemoveDiacritics();
                        }
                    }
                    else
                        values[i] = properties[i].GetValue(entity);
                }

                valuesPair.AddIfNotExists(table.Rows.Add(values).ItemArray, entity);
            }
            var rows = table.Select(expression.RemoveDiacritics());

            var matches = rows.Select(
                row => valuesPair.First(
                    kvp => kvp.Key.SequenceEqual(row.ItemArray)).Value);

            return matches;
        }

        /// <summary>
        /// Return list of T (Ciselnik)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbCmd"></param>
        /// <param name="repository"></param>
        /// <returns></returns>
        public static List<T> GetList<T>(this IDbCommand dbCmd,
                                         WebEasRepositoryBase repository,
                                         Filter filter = null,
                                         PaggingParameters pagging = null,
                                         List<PfeSortAttribute> userSort = null,
                                         List<string> hiddenFields = null,
                                         List<string> selectedFields = null,
                                         Filter ribbonFilter = null,
                                         HierarchyNode node = null) where T : class
        {
            string sql = string.Empty;
            try
            {
                Type dataType = typeof(T);
                ModelDefinition modelDefinition = dataType.GetModelDefinition();
                List<FieldDefinition> fieldDefinition = modelDefinition.FieldDefinitions;
                IOrmLiteDialectProvider dialectProvider = OrmLiteConfig.DialectProvider;

                //bool isTenantEntity = dataType.IsAssignableFromType(typeof(ITenantEntity));
                bool isTenantEntity = dataType.HasInterface(typeof(ITenantEntity));
                bool isTenantEntityNullable = dataType.HasInterface(typeof(ITenantEntityNullable));

                //string language = WebEas.Context.Current.Session.Language;
                //bool canTranslate = language != "Sk" && typeof(ColumnTranslation).GetProperties().Any(nav => nav.Name.ToLower() == language.ToLower());

                //  string primaryKeyName = null;
                // find primary key column name in T
                //if (fieldDefinition.Any(p => p.IsPrimaryKey))
                //{
                //    primaryKeyName = fieldDefinition.First(p => p.IsPrimaryKey).FieldName;
                //}

                //                canTranslate = canTranslate && !String.IsNullOrEmpty(primaryKeyName);

                // list of columns names for translation
                //List<FieldDefinition> columnsForTranslation = fieldDefinition.FindAll(p => Attribute.IsDefined(p.PropertyInfo, typeof(TranslationAttribute)));

                var sqlSelectPart = new StringBuilder();
                var sqlFromPart = new StringBuilder();
                var sqlWherePart = new StringBuilder();
                var sqlOrderPart = new StringBuilder();
                var sorts = new List<PfeSortAttribute>();
                string optionRecompile = "";
                string tenantFld = "D_Tenant_Id";

                #region Select part

                foreach (FieldDefinition def in fieldDefinition)
                {
                    if (def.Name == tenantFld && def.FieldName != tenantFld)
                    {
                        tenantFld = def.FieldName; //.PropertyInfo.FirstAttribute<AliasAttribute>().Name;
                    }

                    if (hiddenFields != null && hiddenFields.Contains(def.FieldName))
                    {
                        if (def.PropertyInfo.HasAttribute<PfeColumnAttribute>())
                        {
                            //Vypnute 9.2.2021
                            continue;
                            /*if (def.PropertyInfo.FirstAttribute<PfeColumnAttribute>().LoadWhenVisible)
                            {
                                continue;
                            }*/
                        }
                    }

                    if (selectedFields != null && !selectedFields.Contains(def.FieldName))
                    {
                        continue;
                    }

                    if (sqlSelectPart.Length > 0)
                    {
                        sqlSelectPart.Append(",");
                    }

                    // default
                    //string columnPart = dialectProvider.GetQuotedColumnName(def.FieldName);

                    //if (canTranslate && columnsForTranslation.Exists(p => p.FieldName == def.FieldName))
                    //{
                    //    // get column name for translation
                    //    //var columnTranslationName = typeof(ColumnTranslation).GetProperty(language).GetCustomAttributes<AliasAttribute>().FirstOrDefault().Name;
                    //    string columnTranslationName = dialectProvider.GetQuotedColumnName(language);
                    //    // buid key for finding translation in dictionary
                    //    string key = string.Concat(modelDefinition.Schema, ".", modelDefinition.Alias, ".", def.FieldName, ".");

                    //    columnPart = string.Format("ISNULL((SELECT TOP 1 {0} FROM [reg].[D_PrekladovySlovnik] WHERE [Kluc] = N'{1}' + CAST({2} AS NVARCHAR(10))), {3}) {4}", columnTranslationName, key, dialectProvider.GetQuotedColumnName(primaryKeyName), dialectProvider.GetQuotedColumnName(def.FieldName), dialectProvider.GetQuotedColumnName(def.FieldName));
                    //}

                    sqlSelectPart.AppendFormat("[{0}]", def.FieldName);

                    if (def.PropertyInfo.HasAttribute<PfeSortAttribute>())
                    {
                        PfeSortAttribute srt = def.PropertyInfo.FirstAttribute<PfeSortAttribute>();
                        if (string.IsNullOrEmpty(srt.Field))
                        {
                            srt.Field = def.FieldName;
                        }

                        sorts.Add(srt);
                    }
                }

                #endregion

                #region FROM part

                string sqlFromAlias = modelDefinition.Alias;

                #endregion

                #region OrderBy part

                if (sorts.Count > 1)
                {
                    sorts = sorts.OrderBy(nav => nav.Rank).ToList();
                }

                if (userSort != null)
                {
                    for (int i = userSort.Count - 1; i > -1; i--)
                    {
                        //check for valid name (especially alias..)
                        FieldDefinition col = fieldDefinition.FirstOrDefault(a => string.Compare(a.Name, userSort[i].Field, true) == 0);
                        if (col == null)
                        {
                            continue;
                        }

                        userSort[i].Field = col.FieldName;
                        if (sorts.Any(nav => nav.Field == userSort[i].Field))
                        {
                            sorts.Remove(sorts.First(nav => string.Compare(nav.Field, userSort[i].Field, true) == 0));
                        }
                        sorts.Insert(0, userSort[i]);
                    }
                }

                if (sorts.Count > 0)
                {
                    sqlOrderPart.Append(" ORDER BY");
                    for (int i = 0; i < sorts.Count; i++)
                    {
                        if (i != 0)
                        {
                            sqlOrderPart.Append(",");
                        }
                        sqlOrderPart.AppendFormat(" [{0}]", sorts[i].Field);
                        if (sorts[i].Sort == PfeOrder.Desc)
                        {
                            sqlOrderPart.Append(" DESC");
                        }
                    }
                }

                #endregion

                #region Pagging

                // TU je FALSE preto lebo robime strankovanie na BE
                if (pagging != null && !pagging.NotDefined && ribbonFilter == null)
                {
                    int pageStartRow = (pagging.PageNumber - 1) * pagging.PageSize;

                    if (sqlOrderPart.Length > 0)
                    {
                        sqlOrderPart.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY ", pageStartRow.ToString(), pagging.PageSize.ToString());
                    }
                    else
                    {
                        string primaryKeyName = null;
                        // find primary key column name in T
                        if (fieldDefinition.Any(p => p.IsPrimaryKey))
                        {
                            primaryKeyName = fieldDefinition.First(p => p.IsPrimaryKey).FieldName;
                        }
                        string orderField = primaryKeyName ?? dialectProvider.GetQuotedColumnName(fieldDefinition.FirstOrDefault().FieldName); // musi byt nejake usporiadanie
                        sqlOrderPart.AppendFormat(" ORDER BY {0} ASC OFFSET {1} ROWS FETCH NEXT {2} ROWS ONLY", orderField, pageStartRow.ToString(), pagging.PageSize.ToString());
                    }
                    // get records count
                    //  sql = String.Format("SELECT COUNT(*) FROM {0}{1}", sqlFromPart.ToString(), sqlWherePart.ToString());
                    // pagging.RecordsCount = repository.Db.Single<int>(sql);
                }

                #endregion

                #region Where part

                var whereFilter = new Filter();
                Dictionary<string, object> parameters = null;

                if (filter != null && !filter.IsEmpty())
                {
                    whereFilter.And(filter);
                }

                if ((isTenantEntity || isTenantEntityNullable) && repository.Session.TenantId != null)
                {
                    if (isTenantEntity)
                    {
                        whereFilter.AndEq(tenantFld, repository.Session.TenantIdGuid.Value);
                    }
                    else
                    {
                        whereFilter.And(Filter.OrElements(
                            FilterElement.Eq(tenantFld, repository.Session.TenantIdGuid.Value),
                            FilterElement.Null(tenantFld)));
                    }
                }
                else if ((isTenantEntity || isTenantEntityNullable) && repository.Session.TenantId == null)
                {
                    whereFilter.AndNull(tenantFld, false);
                }

                #region IBeforeGetList

                if (dataType.HasInterface(typeof(IBeforeGetList)))
                {
                    ((IBeforeGetList)Activator.CreateInstance(typeof(T))).BeforeGetList(repository, node, ref sql, ref whereFilter, ref sqlFromAlias, sqlOrderPart.ToString());
                    if (whereFilter == null)
                    {
                        whereFilter = new Filter();
                    }
                }

                #endregion

                if (!whereFilter.IsEmpty())
                {
                    sqlWherePart.Append(" WHERE ");
                    parameters = whereFilter.ToSqlString(sqlWherePart, false);
                    //Kedze SELECT bude obsahovat dynamicke WHERE podmienky je nutne pridat tuto clausulu - staci aj filter na tenanta a uz to obcas blbne
                    //optionRecompile = " OPTION (RECOMPILE, USE HINT (N'FORCE_LEGACY_CARDINALITY_ESTIMATION'))";

                    var optimizedViews = new List<string>();
                    if (repository.Cache is ServiceStack.Redis.RedisClientManagerCacheClient)
                    {
                        optimizedViews = repository.GetCacheOptimized("ViewsSqlOption", () =>
                        {
                            return new List<string>();
                        }, TimeSpan.MaxValue);
                    }

                    if (optimizedViews.Any(x => x.ToLower().StartsWith($"{modelDefinition.Schema}.{modelDefinition.Alias}".ToLower())))
                    {
                        var data = optimizedViews.First(x => x.ToLower().StartsWith($"{modelDefinition.Schema}.{modelDefinition.Alias}".ToLower())).Split(':');
                        if (data.Length == 2)
                        {
                            optionRecompile = data[1];
                        }
                    }

                    if (string.IsNullOrEmpty(optionRecompile))
                    {
                        optionRecompile = " OPTION (RECOMPILE)";
                    }
                }

                #endregion

                if (sqlSelectPart.Length == 0)
                {
                    //Pivot neobsahuje pri prvom načítaní žiadne fieldy, tak v tomto prípade dám hviezdičku
                    //FE posiela v takomto prípade ID ale to v F_RzpKomplet - je to Ignore field
                    //sqlSelectPart.Append(dataType.GetProperties().First(nav => nav.HasAttribute<PrimaryKeyAttribute>()).Name);
                    sqlSelectPart.Append("*");
                }

                if (!string.IsNullOrEmpty(sqlFromAlias))
                {
                    sqlFromPart.AppendFormat("{0}.{1}", modelDefinition.Schema, sqlFromAlias);
                }

                // get data with pagging
                if (string.IsNullOrEmpty(sql))
                {
                    sql = $"SELECT {sqlSelectPart} FROM {sqlFromPart}{sqlWherePart}{sqlOrderPart}{optionRecompile}";
                }

                var records = new List<T>();
                if (sqlFromPart.Length > 0)
                {
                    records = GetData<T>(dbCmd, ref sql, parameters);
                }

                if (ribbonFilter != null)
                {
                    var ribbonFilterSql = ribbonFilter.ToString();
                    if (ribbonFilterSql.Contains("@"))
                    {
                        foreach (var par in ribbonFilter.Parameters)
                        {
                            ribbonFilterSql = Regex.Replace(ribbonFilterSql, $"@{par.Key}", $"'{par.Value}'", RegexOptions.IgnoreCase);
                        }
                    }
                    records = records.DatatableWhere(ribbonFilterSql).ToList();
                }

                if (pagging != null)
                {
                    //sqlSelectPart.Append("COUNT(*) OVER() [RecordsCount]");
                    //sRecordCount = String.Format("(SELECT COUNT(*) FROM {0}{1}) AS RecordsCount, ", sqlFromPart.ToString(), sqlWherePart.ToString());

                    if (ribbonFilter != null)
                    {
                        //Strankovanie na BE
                        pagging.RecordsCount = records.Count;
                        return records.Skip((pagging.PageNumber - 1) * pagging.PageSize).Take(pagging.PageSize).ToList();
                    }
                    else
                    {
                        //Stránkovanie na DTB
                        if (pagging.NotDefined)
                        {
                            pagging.RecordsCount = records.Count;
                        }
                        else
                        {
                            pagging.RecordsCount = pagging.PageSize == records.Count || pagging.PageNumber > 1 ?
                                                        (int)dbCmd.Scalar($"SELECT COUNT(*) FROM {sqlFromPart}{sqlWherePart} ") :
                                                        records.Count;
                        }
                    }
                }

                if (dataType.HasInterface(typeof(IAfterGetList)))
                {
                    ((IAfterGetList)Activator.CreateInstance(typeof(T))).AfterGetList(repository, ref records, filter);
                }

                repository.SetAccessFlag(records);
                return records;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Cannot get data from {0} - {1}", typeof(T), sql), ex);
            }
        }

        public static void ChangeFilterCondition(Filter filter, string findString, string replaceString)
        {

        }

        private static readonly ILog logDb = LogManager.GetLogger(typeof(OrmLiteReadCommandExtensions));

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="dbCmd">The db CMD.</param>
        /// <param name="sql">The SQL.</param>
        /// <param name="paging">The paging.</param>
        /// <returns></returns>
        private static List<T> GetData<T>(IDbCommand dbCmd, ref string sql, Dictionary<string, object> parameters = null) where T : class
        {
            IOrmLiteDialectProvider dialectProvider = dbCmd.GetDialectProvider();

            if (parameters != null)
            {
                dbCmd.Parameters.Clear();

                foreach (KeyValuePair<string, object> kvp in parameters)
                {
                    try
                    {
                        if (parameters[kvp.Key] is ICollection)
                        {
                            var name = new StringBuilder();
                            int count = 0;
                            foreach (object val in (ICollection)parameters[kvp.Key])
                            {
                                string keyName = string.Format("{0}_{1}", kvp.Key, ++count);
                                if (name.Length > 0)
                                {
                                    name.Append(",");
                                }
                                name.AppendFormat("@{0}", keyName);
                                dbCmd.AddParam(keyName, val);
                            }
                            sql = sql.Replace(string.Format("@{0}", kvp.Key), string.Format("({0})", name));
                        }
                        else
                        {
                            dbCmd.AddParam(kvp.Key, parameters[kvp.Key]);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new WebEasException(string.Format("Cannot add parameter {0} with value {1}", kvp.Key, parameters[kvp.Key]), ex);
                    }
                }
            }

            System.Collections.Generic.List<T> ts;
            dbCmd.CommandText = sql;
            dbCmd.CommandTimeout = 60;

            if (logDb.IsDebugEnabled)
            {
                var sb = new StringBuilder();

                sb.Append("SQL: ").Append(dbCmd.CommandText);

                if (dbCmd.Parameters.Count > 0)
                {
                    sb.AppendLine()
                      .Append("PARAMS: ");

                    for (int i = 0; i < dbCmd.Parameters.Count; i++)
                    {
                        var p = (IDataParameter)dbCmd.Parameters[i];
                        if (i > 0)
                        {
                            sb.Append(", ");
                        }
                        sb.AppendFormat("{0}={1}", p.ParameterName, p.Value);
                    }
                }

                logDb.Debug(sb.ToString());
            }

            using (System.Data.IDataReader dataReader = dbCmd.ExecuteReader())
            {
                FieldDefinition[] allFieldDefinitionsArray = ModelDefinition<T>.Definition.AllFieldDefinitionsArray;
                ts = new List<T>();
                var indexFieldsCache = dataReader.GetIndexFieldsCache(ModelDefinition<T>.Definition, dialectProvider);
                var values = new object[dataReader.FieldCount];
                while (dataReader.Read())
                {
                    T t = OrmLiteUtils.CreateInstance<T>();
                    t.PopulateWithSqlReader(dialectProvider, dataReader, indexFieldsCache, values);
                    if (t is IBaseEntity)
                    {
                        (t as IBaseEntity).DefaultRecord = t.GetClone();
                    }
                    ts.Add(t);
                }
            }
            return ts;
        }

        /// <summary>
        /// Returns count of data in database
        /// </summary>
        public static List<TreeNodeCount> GetCount<T>(this IDbCommand dbCmd, WebEasRepositoryBase repository, Filter filter = null, string[] codes = null) where T : class
        {
            string sql = string.Empty;
            try
            {
                var result = new List<TreeNodeCount>();
                ModelDefinition modelDefinition = typeof(T).GetModelDefinition();

                bool isTenantEntity = typeof(T).HasInterface(typeof(ITenantEntity));
                bool isTenantEntityNullable = typeof(T).HasInterface(typeof(ITenantEntityNullable));

                var whereFilter = new Filter();
                Dictionary<string, object> parameters = null;

                if ((isTenantEntity || isTenantEntityNullable) && repository.Session.TenantId != null)
                {
                    if (isTenantEntity)
                    {
                        whereFilter.AndEq("D_Tenant_Id", repository.Session.TenantIdGuid.Value);
                    }
                    else
                    {
                        whereFilter.And(Filter.OrElements(
                            FilterElement.Eq("D_Tenant_Id", repository.Session.TenantIdGuid.Value),
                            FilterElement.Null("D_Tenant_Id")));
                    }
                }
                else if ((isTenantEntity || isTenantEntityNullable) && repository.Session.TenantId == null)
                {
                    whereFilter.AndNull("D_Tenant_Id", false);
                }

                if (filter != null && !filter.IsEmpty())
                {
                    whereFilter.And(filter);
                }

                if (codes != null && codes.Any(x => x.Contains('!')) && codes.Length > 1)
                {
                    string parameterName = null;
                    var f = new Filter();
                    var node = repository.RenderModuleRootNode(codes.First());
                    foreach (var code in codes)
                    {
                        // ------ HirarchyNode.ParametrizedDbFilter
                        var nodeDynamicFilter = node.TryFindNode(code).ParametrizedDbFilter();

                        if (nodeDynamicFilter != null)
                        {
                            if (parameterName == null)
                            {
                                PropertyInfo prop = null;
                                if (modelDefinition.ModelType.GetProperties().Any(nav => nav.HasAttribute<HierarchyNodeParameterAttribute>()))
                                {

                                    prop = modelDefinition.ModelType.GetProperties().First(nav => nav.HasAttribute<HierarchyNodeParameterAttribute>());

                                }
                                else
                                {
                                    prop = modelDefinition.ModelType.GetProperties().First(nav => nav.HasAttribute<PrimaryKeyAttribute>());
                                }

                                parameterName = prop.HasAttribute<AliasAttribute>() ? prop.FirstAttribute<AliasAttribute>().Name : prop.Name;
                            }


                            foreach (KeyValuePair<string, object> val in nodeDynamicFilter)
                            {
                                f.Or(FilterElement.Eq(val.Key, val.Value.ToString()));
                            }
                        }
                    }

                    if (!f.IsEmpty())
                    {
                        if (whereFilter.IsEmpty())
                        {
                            whereFilter = f;
                        }
                        else
                        {
                            whereFilter.And(f);
                        }
                    }

                    var sqlWherePart = new StringBuilder();
                    if (!whereFilter.IsEmpty())
                    {
                        sqlWherePart.Append(" WHERE ");
                        parameters = whereFilter.ToSqlString(sqlWherePart, false);
                    }

                    sql = $"SELECT {parameterName} AS Id, COUNT(*) as Count FROM {modelDefinition.Schema}.{modelDefinition.Alias} WITH(NOLOCK) {sqlWherePart} GROUP BY {parameterName}";
                    var data = repository.Db.Select<(long Id, int Count)>(sql, parameters);
                    foreach (var code in codes)
                    {
                        result.Add(new TreeNodeCount
                        {
                            Code = code,
                            Count = data.Any(x => x.Id == long.Parse(code.LastRightPart('!'))) ?  data.First(x => x.Id == long.Parse(code.LastRightPart('!'))).Count : 0
                        });
                    }
                }
                else
                {
                    var sqlWherePart = new StringBuilder();
                    if (!whereFilter.IsEmpty())
                    {
                        sqlWherePart.Append(" WHERE ");
                        parameters = whereFilter.ToSqlString(sqlWherePart, false);
                    }

                    sql = $"SELECT COUNT(*) FROM {modelDefinition.Schema}.{modelDefinition.Alias} WITH(NOLOCK){sqlWherePart.ToString()}";
                    result.Add(new TreeNodeCount
                    {
                        Code = codes.First(),
                        Count = repository.Db.SqlScalar<int>(sql, parameters)
                    });
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Cannot get data from {0} - {1}", typeof(T), sql), ex);
            }
        }

        /// <summary>
        /// Gets the property alias.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        private static string GetPropertyAlias(PropertyInfo property)
        {
            string name = property.Name;

            object[] attrs = property.GetCustomAttributes(typeof(AliasAttribute), true);
            foreach (object attr in attrs)
            {
                var aliasAttr = attr as AliasAttribute;
                if (aliasAttr != null)
                {
                    name = aliasAttr.Name;
                }
            }

            return name;
        }

        /// <summary>
        /// Prepares the parameterized insert statement.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="dialectProvider">The dialect provider.</param>
        /// <param name="cmd">The CMD.</param>
        /// <param name="excludeFields">The exclude fields.</param>
        private static void PrepareParameterizedInsertStatement<T>(this IOrmLiteDialectProvider dialectProvider, T obj, IDbCommand cmd, ICollection<string> excludeFields = null)
        {
            var stringBuilder = new StringBuilder();
            var stringBuilder1 = new StringBuilder();
            ModelDefinition modelDefinition = obj.GetType().GetModelDefinition();
            cmd.Parameters.Clear();
            cmd.CommandTimeout = OrmLiteConfig.CommandTimeout;
            //FieldDefinition[] fieldDefinitionsArray = modelDefinition.FieldDefinitions.ToArray();
            //for (int i = 0; i < fieldDefinitionsArray.Length; i++)
            foreach (FieldDefinition fieldDefinition in modelDefinition.FieldDefinitions)
            {
                if (fieldDefinition.PropertyInfo.HasAttribute<IgnoreInsertOrUpdateAttribute>())
                {
                    continue;
                }

                //FieldDefinition fieldDefinition = fieldDefinitionsArray[i];
                if (!fieldDefinition.AutoIncrement && !fieldDefinition.IsComputed && (excludeFields == null || !excludeFields.Contains(fieldDefinition.Name)))
                {
                    if (stringBuilder.Length > 0)
                    {
                        stringBuilder.Append(",");
                    }
                    if (stringBuilder1.Length > 0)
                    {
                        stringBuilder1.Append(",");
                    }
                    try
                    {
                        stringBuilder.Append(dialectProvider.GetQuotedColumnName(fieldDefinition.FieldName));
                        stringBuilder1.Append(dialectProvider.GetParam(dialectProvider.SanitizeFieldNameForParamName(fieldDefinition.FieldName)));
                        dialectProvider.AddParameter(cmd, fieldDefinition);
                    }
                    catch (Exception)
                    {
                        // Exception exception = exception1;
                        //  OrmLiteDialectProviderBase<TDialect>.Log.Error(string.Concat("ERROR in PrepareParameterizedInsertStatement(): ", exception.Message), exception);
                        throw;
                    }
                }
            }
            cmd.CommandText = string.Format("INSERT INTO {0} ({1}) VALUES ({2})", dialectProvider.GetQuotedTableName(modelDefinition), stringBuilder, stringBuilder1);
        }

        /// <summary>
        /// Prepares the parameterized update statement.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="dialectProvider">The dialect provider.</param>
        /// <param name="cmd">The CMD.</param>
        /// <param name="excludeFields">The exclude fields.</param>
        private static void PrepareParameterizedUpdateStatement<T>(this IOrmLiteDialectProvider dialectProvider, T obj, IDbCommand cmd, ICollection<string> excludeFields = null)
        {
            var sbWhere = new StringBuilder();
            var sbSet = new StringBuilder();
            ModelDefinition modelDefinition = obj.GetType().GetModelDefinition();
            cmd.Parameters.Clear();
            cmd.CommandText = string.Empty;
            cmd.CommandTimeout = OrmLiteConfig.CommandTimeout;
            bool isTenantEntity = modelDefinition.ModelType.HasInterface(typeof(ITenantEntity));
            var changedFields = new List<string>();

            foreach (FieldDefinition fieldDefinition in modelDefinition.FieldDefinitions)
            {
                if (fieldDefinition.IsComputed)
                {
                    continue;
                }
                try
                {
                    if (fieldDefinition.IsPrimaryKey)
                    {
                        if (sbWhere.Length > 0)
                        {
                            sbWhere.Append(" AND ");
                        }
                        sbWhere.Append(dialectProvider.GetQuotedColumnName(fieldDefinition.FieldName)).Append("=").Append(dialectProvider.GetParam(dialectProvider.SanitizeFieldNameForParamName(fieldDefinition.FieldName)));
                        dialectProvider.AddParameter(cmd, fieldDefinition);
                    }
                    else if (excludeFields == null || excludeFields.Count <= 0 || !excludeFields.Contains(fieldDefinition.Name))
                    {
                        if (fieldDefinition.PropertyInfo.HasAttribute<IgnoreInsertOrUpdateAttribute>())
                        {
                            continue;
                        }

                        if (obj is BaseEntity && (obj as BaseEntity).DefaultRecord != null)
                        {
                            var curValue = fieldDefinition.PropertyInfo.GetValue(obj);
                            var defValue = (obj as BaseEntity).DefaultRecord.GetPi(fieldDefinition.PropertyInfo.Name).GetValue((obj as BaseEntity).DefaultRecord);
                            if (curValue?.GetHashCode() == defValue?.GetHashCode())
                            {
                                continue;
                            }
                        }

                        if (sbSet.Length > 0)
                        {
                            sbSet.Append(", ");
                        }
                        sbSet.Append(dialectProvider.GetQuotedColumnName(fieldDefinition.FieldName)).Append("=").Append(dialectProvider.GetParam(dialectProvider.SanitizeFieldNameForParamName(fieldDefinition.FieldName)));
                        dialectProvider.AddParameter(cmd, fieldDefinition);
                        changedFields.AddIfNotExists(fieldDefinition.FieldName);
                    }

                    if (fieldDefinition.Name == "D_Tenant_Id" && isTenantEntity && !sbWhere.ToString().Contains("D_Tenant_Id"))
                    {
                        if (sbWhere.Length > 0)
                        {
                            sbWhere.Append(" AND ");
                        }

                        sbWhere.Append(dialectProvider.GetQuotedColumnName(fieldDefinition.FieldName));
                        sbWhere.Append("=");
                        sbWhere.Append(dialectProvider.GetParam(dialectProvider.SanitizeFieldNameForParamName(fieldDefinition.FieldName)));

                        dialectProvider.AddParameter(cmd, fieldDefinition);
                    }
                }
                catch (Exception ex)
                {
                    //   Exception exception = exception1;
                    throw ex;
                    //  OrmLiteDialectProviderBase<TDialect>.Log.Error(string.Concat("ERROR in PrepareParameterizedUpdateStatement(): ", exception.Message), exception);
                }
            }

            if (sbSet.Length < 1)
            {
                return;
            }
            string tbl = dialectProvider.GetQuotedTableName(modelDefinition);

            // ak je zmeneny iba datum zmeny a ziadny iny stlpec, neupdatujeme
            if (changedFields.Count == 1 && changedFields.Contains(nameof(IBaseEntity.DatumZmeny)) ||
                changedFields.Count == 2 && changedFields.Contains(nameof(IBaseEntity.DatumZmeny)) && changedFields.Contains(nameof(IBaseEntity.Zmenil)))
            {
                if (!tbl.Contains("D_Doklad") && !tbl.Contains("D_FO_Osoba") && !tbl.Contains("D_PO_Osoba"))
                {
                    //WORKAROUND: prípady rozširujúcej tabuľky. SAVE musí zapísať DatumZmeny do rozširujúcej tabuľky aj v prípade, že zmena bola iba v hlavnej (tč.D_BiznisEntita, D_Osoba)
                    return;
                }
            }

            cmd.CommandText = string.Format("UPDATE {0} SET {1} {2}", tbl, sbSet, (sbWhere.Length > 0 ? string.Concat("WHERE ", sbWhere) : ""));
        }

        /// <summary>
        /// Prepares the parameterized delete statement.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="dialectProvider">The dialect provider.</param>
        /// <param name="cmd">The CMD.</param>
        private static void PrepareParameterizedDeleteStatement<T>(this IOrmLiteDialectProvider dialectProvider, IDbCommand cmd, WebEasRepositoryBase repository)
        {
            var stringBuilder = new StringBuilder();
            ModelDefinition modelDefinition = typeof(T).GetModelDefinition();
            cmd.Parameters.Clear();
            cmd.CommandTimeout = OrmLiteConfig.CommandTimeout;

            bool isTenantEntity = modelDefinition.ModelType.HasInterface(typeof(ITenantEntity));
            bool isTenantEntityNullable = modelDefinition.ModelType.HasInterface(typeof(ITenantEntityNullable));

            foreach (FieldDefinition fieldDefinition in modelDefinition.FieldDefinitions)
            {
                if (fieldDefinition.IsComputed)
                {
                    continue;
                }
                try
                {
                    if (fieldDefinition.IsPrimaryKey)
                    {
                        if (stringBuilder.Length > 0)
                        {
                            stringBuilder.Append(" AND ");
                        }
                        stringBuilder.Append(dialectProvider.GetQuotedColumnName(fieldDefinition.FieldName)).Append("=").Append(dialectProvider.GetParam(dialectProvider.SanitizeFieldNameForParamName(fieldDefinition.FieldName)));
                        dialectProvider.AddParameter(cmd, fieldDefinition);
                    }

                    if (fieldDefinition.Name == "D_Tenant_Id" && (isTenantEntity || isTenantEntityNullable))
                    {
                        if (stringBuilder.Length > 0)
                        {
                            stringBuilder.Append(" AND ");
                        }

                        stringBuilder.Append($"({(repository.Session.AdminLevel == AdminLevel.SysAdmin ? "D_Tenant_Id IS NULL OR " : string.Empty) }D_Tenant_Id = '{repository.Session.TenantId}')");
                    }
                }
                catch (Exception)
                {
                    //  Exception exception = exception1;
                    throw;
                    //  OrmLiteDialectProviderBase<TDialect>.Log.Error(string.Concat("ERROR in PrepareParameterizedUpdateStatement(): ", exception.Message), exception);
                }
            }
            cmd.CommandText = string.Format("DELETE FROM {0} WHERE {1}", dialectProvider.GetQuotedTableName(modelDefinition), stringBuilder);
        }

        /// <summary>
        /// Prepares the parameterized 'safe' delete statement (which does update)
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="dialectProvider">The dialect provider.</param>
        /// <param name="cmd">The CMD.</param>
        private static void PrepareParameterizedSafeDeleteStatement<T>(this IOrmLiteDialectProvider dialectProvider, IDbCommand cmd, WebEasRepositoryBase repository)
        {
            var stringBuilder = new StringBuilder();
            var sbSet = new StringBuilder("DatumPlatnosti = (getdate())");
            ModelDefinition modelDefinition = typeof(T).GetModelDefinition();
            cmd.Parameters.Clear();
            cmd.CommandTimeout = OrmLiteConfig.CommandTimeout;

            bool isTenantEntity = modelDefinition.ModelType.HasInterface(typeof(ITenantEntity));
            bool isTenantEntityNullable = modelDefinition.ModelType.HasInterface(typeof(ITenantEntityNullable));

            foreach (FieldDefinition fieldDefinition in modelDefinition.FieldDefinitions)
            {
                if (fieldDefinition.IsComputed)
                {
                    continue;
                }
                try
                {
                    if (fieldDefinition.IsPrimaryKey)
                    {
                        stringBuilder.Append(" AND ");
                        stringBuilder.Append(dialectProvider.GetQuotedColumnName(fieldDefinition.FieldName)).Append("=").Append(dialectProvider.GetParam(dialectProvider.SanitizeFieldNameForParamName(fieldDefinition.FieldName)));
                        dialectProvider.AddParameter(cmd, fieldDefinition);
                    }

                    if (fieldDefinition.Name == "D_Tenant_Id" && (isTenantEntity || isTenantEntityNullable))
                    {
                        if (stringBuilder.Length > 0)
                        {
                            stringBuilder.Append(" AND ");
                        }

                        stringBuilder.Append($"({(repository.Session.AdminLevel == AdminLevel.SysAdmin ? "D_Tenant_Id IS NULL OR " : string.Empty) }D_Tenant_Id = '{repository.Session.TenantId}')");
                    }

                    if (fieldDefinition.Name == "Zmenil" || fieldDefinition.Name == "DatumZmeny")
                    {
                        sbSet.Append(" ,");
                        sbSet.Append(dialectProvider.GetQuotedColumnName(fieldDefinition.FieldName));
                        sbSet.Append("=");
                        sbSet.Append(dialectProvider.GetParam(dialectProvider.SanitizeFieldNameForParamName(fieldDefinition.FieldName)));

                        dialectProvider.AddParameter(cmd, fieldDefinition);
                    }
                }
                catch (Exception)
                {
                    //  Exception exception = exception1;
                    throw;
                    //  OrmLiteDialectProviderBase<TDialect>.Log.Error(string.Concat("ERROR in PrepareParameterizedUpdateStatement(): ", exception.Message), exception);
                }
            }
            cmd.CommandText = string.Format("UPDATE {0} SET {1} WHERE DatumPlatnosti IS NULL {2}", dialectProvider.GetQuotedTableName(modelDefinition), sbSet, stringBuilder);
        }

        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="dialectProvider">The dialect provider.</param>
        /// <param name="cmd">The CMD.</param>
        /// <param name="fieldDef">The field def.</param>
        private static void AddParameter(this IOrmLiteDialectProvider dialectProvider, IDbCommand cmd, FieldDefinition fieldDef)
        {
            IDbDataParameter dbDataParameter = cmd.CreateParameter();
            SetParameter(dialectProvider, fieldDef, dbDataParameter);
            cmd.Parameters.Add(dbDataParameter);
        }

        /// <summary>
        /// Sets the parameter.
        /// </summary>
        /// <param name="dialectProvider">The dialect provider.</param>
        /// <param name="fieldDef">The field def.</param>
        /// <param name="p">The p.</param>
        private static void SetParameter(this IOrmLiteDialectProvider dialectProvider, FieldDefinition fieldDef, IDbDataParameter p)
        {
            p.ParameterName = dialectProvider.GetParam(dialectProvider.SanitizeFieldNameForParamName(fieldDef.FieldName));
            dialectProvider.InitDbParam(p, fieldDef.ColumnType);

            // pri timespane sa dbtype nenastavi, zabere to az pri sqldbtype
            if ((fieldDef.ColumnType == typeof(System.TimeSpan) || fieldDef.ColumnType == typeof(System.TimeSpan?)) && p is System.Data.SqlClient.SqlParameter)
            {
                ((SqlParameter)p).SqlDbType = SqlDbType.Time;
            }

            if (fieldDef.ColumnType == typeof(string) && p is SqlParameter)
            {
                ((SqlParameter)p).SqlDbType = SqlDbType.NVarChar;
            }
        }

        /// <summary>
        /// Adds the insert parameters.
        /// </summary>
        /// <param name="dbCmd">The db CMD.</param>
        /// <param name="repository">The repository.</param>
        private static void PrepareInsertParameters<T>(this T obj, WebEasRepositoryBase repository) where T : IBaseEntity
        {
            var ses = repository.Session;
            if (obj is IBaseEntity)
            {
                obj.Vytvoril = ses.UserIdGuid;
                obj.DatumVytvorenia = DateTime.Now;
                obj.Zmenil = ses.UserIdGuid;
                obj.DatumZmeny = DateTime.Now;
            }
            if (obj is ITenantEntity)
            {
                if (!ses.TenantIdGuid.HasValue)
                {
                    throw new Exception(string.Format("TenantId is not defined {0}", repository.Session.TenantId));
                }

                ((ITenantEntity)obj).D_Tenant_Id = ses.TenantIdGuid.Value;
            }
            else if (obj is ITenantEntityNullable)
            {
                //7.5.2016 - Aj ked je RZP_ADMIN globalne zaznamy nemoze pridavat. Tie sa pridavaju iba cez skripty
                //((ITenantEntityNullable)obj).D_Tenant_Id = ses.HasRole(Roles.Admin) ? null : ses.TenantIdGuid;
                ((ITenantEntityNullable)obj).D_Tenant_Id = ses.TenantIdGuid;
            }
        }

        /// <summary>
        /// Adds the insert parameters.
        /// </summary>
        /// <param name="dbCmd">The db CMD.</param>
        /// <param name="repository">The repository.</param>
        private static void PrepareUpdateParameters<T>(this T obj, WebEasRepositoryBase repository) where T : IBaseEntity
        {
            var ses = repository.Session;
            if (obj is IBaseEntity)
            {
                obj.Zmenil = ses.UserIdGuid;
                obj.DatumZmeny = DateTime.Now;
            }
            if (obj is ITenantEntity)
            {
                if (!ses.TenantIdGuid.HasValue)
                {
                    throw new Exception(string.Format("TenantId is not defined {0}", ses.TenantId));
                }

                ((ITenantEntity)obj).D_Tenant_Id = ses.TenantIdGuid.Value;
            }
            else if (obj is ITenantEntityNullable)
            {
                //9.5.2016 D_Tenant_Id - nemenit
                //((ITenantEntityNullable)obj).D_Tenant_Id = ses.HasRole(Roles.Admin) ? null : ses.TenantIdGuid;
                ((ITenantEntityNullable)obj).D_Tenant_Id = ses.TenantIdGuid;
            }
        }

        /// <summary>
        /// Prepares the delete parameters.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="obj">The obj.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="id">The id.</param>
        private static void PrepareDeleteParameters<T>(this T obj, WebEasRepositoryBase repository) where T : IBaseEntity
        {
            var ses = repository.Session;

            if (obj is IBaseEntity)
            {
                obj.Zmenil = ses.UserIdGuid;
                obj.DatumZmeny = DateTime.Now;
            }

            if (obj is ITenantEntity)
            {
                if (!ses.TenantIdGuid.HasValue)
                {
                    throw new Exception(string.Format("TenantId is not defined {0}", ses.TenantId));
                }

                ((ITenantEntity)obj).D_Tenant_Id = ses.TenantIdGuid.Value;
            }
        }

        /// <summary>
        /// Determines whether [is nullable type] [the specified the type].
        /// </summary>
        /// <param name="theType">The type.</param>
        /// <returns></returns>
        private static bool IsNullableType(Type theType)
        {
            if (!theType.IsGenericType)
            {
                return false;
            }
            return theType.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// Logging column defined by configuration of by attribute
        /// </summary>
        private static void LogColumnsChangesByConfig<T>(WebEasRepositoryBase repository, T obj) where T : class, IBaseEntity
        {
            if (obj == null)
            {
                throw new ArgumentNullException(string.Format("Object {0} cannot be null", typeof(T)));
            }

            Type objType = typeof(T);

            ModelDefinition modelDefinition = objType.GetModelDefinition();

            ReplacementsDictionary replacements = null;
            List<LoggingConfig> loggingConfig = null;

            //get list of columns to log (in [reg].[C_LoggingConfig] table)
            //but check against cache first to avoid db call...
            if (repository.IsTableLogged(modelDefinition.Schema, modelDefinition.Alias))
            {
                Filter filter = Filter.AndElements(
                    FilterElement.Eq("Schema", modelDefinition.Schema),
                    FilterElement.Eq("NazovTabulky", modelDefinition.Alias));
                loggingConfig = repository.GetList<LoggingConfig>(filter);
            }

            if (loggingConfig != null && loggingConfig.Count > 0)
            {
                // find primary key column name and value
                string primaryKeyName = modelDefinition.FieldDefinitions.Find(p => p.IsPrimaryKey).FieldName;
                object primaryKeyValue = objType.GetProperty(primaryKeyName).GetValue(obj);
                long primaryKeyId = 0;
                if (!long.TryParse(primaryKeyValue.ToString(), out primaryKeyId))
                {
                    return; //unsupported primary key...?
                }

                bool isTenantEntity = objType.HasInterface(typeof(ITenantEntity));
                bool isTenantEntityNullable = !isTenantEntity && objType.HasInterface(typeof(ITenantEntityNullable));

                // get entity actual - old values
                T entityItem = repository.GetById<T>(primaryKeyValue);

                foreach (FieldDefinition def in modelDefinition.FieldDefinitions)
                {
                    LoggingConfig logColumn = loggingConfig.FirstOrDefault(a => string.Compare(a.NazovStlpca, def.FieldName, true) == 0);
                    if (logColumn != null)
                    {
                        object entityItemPropertyValueOld = objType.GetProperty(def.FieldName).GetValue(entityItem);
                        object entityItemPropertyValueNew = objType.GetProperty(def.FieldName).GetValue(obj);

                        // compare logged property old vs new value
                        if (!Nullable.Equals(entityItemPropertyValueOld, entityItemPropertyValueNew))
                        {
                            if (replacements == null)
                            {
                                replacements = new ReplacementsDictionary();
                                TextationProcessor.FillDictionaryFromObjectByProperties(replacements, obj);
                            }

                            string popisZmeny = TextationProcessor.Process(log, logColumn.PopisZmeny, replacements);
                            //if (string.IsNullOrEmpty(popisZmeny))
                            //    popisZmeny = string.Format("Zmena hodnoty stĺpca '{0}' z \"{1}\" na \"{2}\"", def.FieldName, entityItemPropertyValueOld, entityItemPropertyValueNew);

                            var columnLogger = new ColumnLogger()
                            {
                                Row_Id = primaryKeyId,
                                NazovStlpca = def.FieldName,
                                NazovTabulky = modelDefinition.Alias,
                                TypStlpca = def.FieldType.Name,
                                Schema = modelDefinition.Schema,
                                PovodnaHodnota = string.Format("{0}", entityItemPropertyValueOld),
                                NovaHodnota = string.Format("{0}", entityItemPropertyValueNew),
                                PopisZmeny = popisZmeny
                            };

                            if (isTenantEntity)
                            {
                                columnLogger.D_Tenant_Id = ((ITenantEntity)obj).D_Tenant_Id;
                            }
                            else if (isTenantEntityNullable)
                            {
                                columnLogger.D_Tenant_Id = ((ITenantEntityNullable)obj).D_Tenant_Id;
                            }

                            repository.Db.Exec((IDbCommand dbCmd) => dbCmd.InsertData<ColumnLogger>(repository, columnLogger));
                        }
                    }
                }
            }
        }

        public static string GetMd5fromObj<T>(this T obj)
        {
            string str = obj.ToJson();
            // First we need to convert the string into bytes, which
            // means using a text encoder.
            System.Text.Encoder enc = System.Text.Encoding.Unicode.GetEncoder();

            // Create a buffer large enough to hold the string
            byte[] unicodeText = new byte[str.Length * 2];
            enc.GetBytes(str.ToCharArray(), 0, str.Length, unicodeText, 0, true);

            // Now that we have a byte array we can ask the CSP to hash it
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(unicodeText);

            // Build the final string by converting each byte
            // into hex and appending it to a StringBuilder
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("X2"));
            }

            // And return it
            return sb.ToString();
        }

        public static bool ValidateBankAccount(this string iban)
        {
            if (iban.Length < 4 || iban[0] == ' ' || iban[1] == ' ' || iban[2] == ' ' || iban[3] == ' ') return false;

            var checksum = 0;
            var ibanLength = iban.Length;
            for (int charIndex = 0; charIndex < ibanLength; charIndex++)
            {
                if (iban[charIndex] == ' ') continue;

                int value;
                var c = iban[(charIndex + 4) % ibanLength];
                if ((c >= '0') && (c <= '9'))
                {
                    value = c - '0';
                }
                else if ((c >= 'A') && (c <= 'Z'))
                {
                    value = c - 'A';
                    checksum = (checksum * 10 + (value / 10 + 1)) % 97;
                    value %= 10;
                }
                else if ((c >= 'a') && (c <= 'z'))
                {
                    value = c - 'a';
                    checksum = (checksum * 10 + (value / 10 + 1)) % 97;
                    value %= 10;
                }
                else return false;

                checksum = (checksum * 10 + value) % 97;
            }
            return checksum == 1;
        }

        public static DateTime GeDateFromRodneCislo(this string rc)
        {
            rc = rc.Replace(@"/", string.Empty);

            if (rc.Length < 9 || rc.Length > 10)
            {
                throw new WebEasValidationException(null, "Nesprávny formát rodného čísla!");
            }

            var validCharsLenTest = new Regex(@"\d{9}[0-9aA]?");

            if (validCharsLenTest.IsMatch(rc) == false)
            {
                throw new WebEasValidationException(null, "Nesprávny formát rodného čísla!");
            }

            #region Kontrola Roku

            int year = Convert.ToInt32(rc.Substring(0, 2));

            if (rc.Length == 9 || (rc.Length == 10 && year >= 54))
            {
                year += 1900;
            }
            else
            {
                year += 2000;
            }

            #endregion

            #region Kontrola Mesiaca

            int month = Convert.ToInt32(rc.Substring(2, 2));
            if (month > 70 && year > 2003)
            {
                month -= 70;   // 53/2004 Sb.
            }
            if (month > 50)
            {
                month -= 50;
            }
            if (month > 20 && year > 2003)
            {
                month -= 20;   // 53/2004 Sb.
            }

            #endregion

            #region Kontrola dni a dni vo february prestupneho roka

            int day = Convert.ToInt32(rc.Substring(4, 2));
            //den + 50 = cudzinec zijuci v CSSR
            if (day > 50)
            {
                day -= 50;
            }

            #endregion

            return new DateTime(year, month, day);
        }

        //#endregion Logovanie do IaP
        //        if (dbTransaction != null)
        //        {
        //            dbTransaction.Commit();
        //        }
        //    }
        //}
        //        dbCmd.InsertData<PotvrdeniePublikovania>(repository, pp);
        //        var pp = new PotvrdeniePublikovania
        //        {
        //            C_InformacnyKanal_Id = (int)InformacnyKanalEnum.DcomPortalObce,
        //            D_DataBuffer_Id = (int)zasobnikId,
        //            DatumPublikovania = DateTime.Now,
        //            PredmetOznamu = ai.Popis,
        //            TextOznamu = ai.Popis
        //        };
        //        long zasobnikId = dbCmd.InsertData<Zasobnik>(repository, zasobnik);
        //            C_StavZverejnenia_Id_Portal = 1,
        //            C_StavZverejnenia_Id_Tlac = 0,
        //            C_StavZverejnenia_Id_UrTabula = 0,
        //            C_StavZverejnenia_Id_Facebook = 0,
        //            C_StavZverejnenia_Id_Rozhlas = 0
        //        };
        //    using (IDbTransaction dbTransaction = dbCmd.Transaction == null ? dbCmd.Connection.BeginTransaction() : null)
        //    {
        //        var zasobnik = new Zasobnik
        //        {
        //            C_Operacie_Id = (int)operacia,
        //            C_Sluzba_Id = ai.C_Sluzba_Id,
        //            C_StavovyPriestor_Id = (int)StavovyPriestorEnum.ZverejnovanieZmien,
        //            C_StavEntity_Id = (int)StavEntityEnum.IAP_Automat,
        //            C_SledovanaPolozka_Id = ai.C_SledovanaPolozka_Id,
        //            D_Pohlad_Id = ai.D_Pohlad_Id,
        //            Popis = ai.Popis,
        //            Zaznam_Id = id,
        //private static void VytvorAutomatickeZverejnenie(WebEasRepositoryBase repository, AutomatickeInformovanie ai, OperacieEnum operacia, long id)
        //{
        //    IDbCommand dbCmd = repository.Db.CreateCommand();
        //            if (results != null && results.Count != 0)
        //            {
        //                kodPolozky = node.KodPolozky;
        //                break;
        //            }
        //        }
        //    }
        //    return kodPolozky;
        //}
        //            var results = (IList)repository.GetType().GetMethod("GetList", new Type[] { typeof(Filter) }).MakeGenericMethod(node.ModelType).Invoke(repository, new object[] { f });
        //        foreach (var node in nodes)
        //        {
        //            Filter f = null;
        //            if (node.AdditionalFilter == null)
        //            {
        //                f = new Filter(primaryKeyName, id);
        //            }
        //            else
        //            {
        //                f = node.AdditionalFilter.Clone();
        //                f.FilterElementGroups.Add(FilterElementGroup.And(FilterElement.Eq(primaryKeyName, id.ToString())));
        //            }
        //    if (nodes.Count == 1)
        //    {
        //        kodPolozky = nodes[0].KodPolozky;
        //    }
        //    else
        //    {
        //        var modelDefinition = typeof(T).GetModelDefinition();
        //        var primaryKeyName = modelDefinition.FieldDefinitions.Find(p => p.IsPrimaryKey).FieldName;
        //    string kodPolozky = null;
        //    if (nodes.IsNullOrEmpty())
        //        return null;
        //private static string NajdiKodPolozky<T>(WebEasRepositoryBase repository, long id)
        //{
        //    var nodes = RenderModuleRootNode().FindNode(typeof(T));
        //private static short VyhodnotPouzitieKanala(InformacnyKanalEnum infKanal, List<NastavenieView> nastavenia, Logovanie ai)
        //{
        //    switch (infKanal)
        //    {
        //        case InformacnyKanalEnum.DcomPortalObce:
        //            if (nastavenia != null && nastavenia.Any(x => x.PolozkaNastaveniaKod == PolozkaNastaveniaKodEnum.InfKanalPortal.ToString()
        //                                                        && x.AkciaNastaveniaKod == AkciaNastavenieKodEnum.VYP.ToString()))
        //                return 0;
        //            return (ai.InfKanalPortal.HasValue ? ai.InfKanalPortal.Value : ai.ExplInfKanalPortal) ? (short)1 : (short)0;
        //        case InformacnyKanalEnum.LokalnaTlac:
        //            if (nastavenia != null && nastavenia.Any(x => x.PolozkaNastaveniaKod == PolozkaNastaveniaKodEnum.InfKanalTlac.ToString()
        //                                                        && x.AkciaNastaveniaKod == AkciaNastavenieKodEnum.VYP.ToString()))
        //                return 0;
        //            return (ai.InfKanalTlac.HasValue ? ai.InfKanalTlac.Value : ai.ExplInfKanalTlac) ? (short)1 : (short)0;
        //        case InformacnyKanalEnum.UradnaTabulaObce:
        //            if (nastavenia != null && nastavenia.Any(x => x.PolozkaNastaveniaKod == PolozkaNastaveniaKodEnum.InfKanalUrTabula.ToString()
        //                                                        && x.AkciaNastaveniaKod == AkciaNastavenieKodEnum.VYP.ToString()))
        //                return 0;
        //            return (ai.InfKanalUrTabula.HasValue ? ai.InfKanalUrTabula.Value : ai.ExplInfKanalUrTabula) ? (short)1 : (short)0;
        //        case InformacnyKanalEnum.Facebook:
        //            if (nastavenia != null && nastavenia.Any(x => x.PolozkaNastaveniaKod == PolozkaNastaveniaKodEnum.InfKanalFacebook.ToString()
        //                                                        && x.AkciaNastaveniaKod == AkciaNastavenieKodEnum.VYP.ToString()))
        //                return 0;
        //            return (ai.InfKanalFacebook.HasValue ? ai.InfKanalFacebook.Value : ai.ExplInfKanalFacebook) ? (short)1 : (short)0;
        //        case InformacnyKanalEnum.ObecnyRozhlas:
        //            if (nastavenia != null && nastavenia.Any(x => x.PolozkaNastaveniaKod == PolozkaNastaveniaKodEnum.InfKanalRozhlas.ToString()
        //                                                        && x.AkciaNastaveniaKod == AkciaNastavenieKodEnum.VYP.ToString()))
        //                return 0;
        //            return (ai.InfKanalRozhlas.HasValue ? ai.InfKanalRozhlas.Value : ai.ExplInfKanalRozhlas) ? (short)1 : (short)0;
        //    }
        //    return 0;
        //}
        //        IDbCommand dbCmd = repository.Db.CreateCommand();
        //        dbCmd.InsertData<Zasobnik>(repository, zasobnik);
        //    }
        //}
        //            C_StavZverejnenia_Id_Portal = VyhodnotPouzitieKanala(InformacnyKanalEnum.DcomPortalObce, nastavenia, ai),
        //            C_StavZverejnenia_Id_Tlac = VyhodnotPouzitieKanala(InformacnyKanalEnum.LokalnaTlac, nastavenia, ai),
        //            C_StavZverejnenia_Id_UrTabula = VyhodnotPouzitieKanala(InformacnyKanalEnum.UradnaTabulaObce, nastavenia, ai),
        //            C_StavZverejnenia_Id_Facebook = VyhodnotPouzitieKanala(InformacnyKanalEnum.Facebook, nastavenia, ai),
        //            C_StavZverejnenia_Id_Rozhlas = VyhodnotPouzitieKanala(InformacnyKanalEnum.ObecnyRozhlas, nastavenia, ai),
        //        };
        //        var zasobnik = new Zasobnik
        //        {
        //            C_Operacie_Id = (int)operacia,
        //            C_Sluzba_Id = ai.C_Sluzba_Id,
        //            C_StavovyPriestor_Id = (int)StavovyPriestorEnum.ZverejnovanieZmien,
        //            C_StavEntity_Id = (int)StavEntityEnum.IAP_New,
        //            C_SledovanaPolozka_Id = ai.C_SledovanaPolozka_Id,
        //            D_Pohlad_Id = ai.D_Pohlad_Id,
        //            Popis = ai.Popis,
        //            Zaznam_Id = id,
        //        if (ai.Zlucovat)
        //        {
        //            var data = repository.Db.Single<Zasobnik>(x => x.C_SledovanaPolozka_Id == ai.C_SledovanaPolozka_Id
        //                                                            && x.C_StavEntity_Id == (int)StavEntityEnum.IAP_New);
        //            if (data != null)
        //            {
        //                if (data.Zaznam_Id.HasValue)
        //                {
        //                    data.Zaznam_Id = null;
        //                    IDbCommand dbCmdUpd = repository.Db.CreateCommand();
        //                    dbCmdUpd.UpdateData<Zasobnik>(repository, data);
        //                }
        //                continue;
        //            }
        //        }
        //        else
        //        {
        //            var data = repository.Db.Single<Zasobnik>(x => x.C_SledovanaPolozka_Id == ai.C_SledovanaPolozka_Id
        //                                                            && x.C_StavEntity_Id == (int)StavEntityEnum.IAP_New
        //                                                            && x.Zaznam_Id == id);
        //            if (data != null)
        //                continue;
        //        }
        //    foreach (var ai in logs)
        //    {
        //        if (ai.C_SposobVykonaniaUlohy_Id == (int)SposobVykonaniaUlohyEnum.Automaticky)
        //        {
        //            VytvorAutomatickeZverejnenie(repository, ai, operacia, id);
        //            return;
        //        }
        //    var nastavenia = repository.GetList<NastavenieView>();
        //    if (logs.IsNullOrEmpty())
        //        return;
        //    var logs = repository.Db.Select<Logovanie>(x => x.SledovanaPolozkaKod == kodPolozky);
        //    if (kodPolozky == null)
        //        return;
        //    var kodPolozky = NajdiKodPolozky<T>(repository, id);
        //    if (id == 0 && obj != null)
        //    {
        //        var modelDefinition = typeof(T).GetModelDefinition();
        //        var primaryKeyName = modelDefinition.FieldDefinitions.Find(p => p.IsPrimaryKey).Name;
        //        var primaryKeyValue = typeof(T).GetProperty(primaryKeyName).GetValue(obj);
        //        long.TryParse(primaryKeyValue.ToString(), out id);
        //    }
        //private static void LogChangesToIap<T>(WebEasRepositoryBase repository, T obj, OperacieEnum operacia, long id = 0) where T : class, IBaseEntity
        //{
        //    if (typeof(T).Equals(typeof(Zasobnik)) || typeof(T).Equals(typeof(PotvrdeniePublikovania)))
        //        return;
        ///// <summary>
        ///// Logs the changes to iap.
        ///// </summary>
        ///// <typeparam name="T">The type of the T.</typeparam>
        ///// <param name="repository">The repository.</param>
        ///// <param name="obj">The obj.</param>
        ///// <param name="operacia">The operacia.</param>
        ///// <param name="id">The id.</param>
        //#region Logovanie do IaP
    }
}
