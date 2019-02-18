using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using ServiceStack;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;
using WebEas.Core;
using WebEas.ServiceModel;

namespace WebEas.ServiceInterface
{
    public static class RepositoryBaseExtension
    {
        public static List<T> NacitatZoznam<T>(this IWebEasCoreRepositoryBase sourceType, object dto, string filter = null, bool searchColumnsCheck = true) where T : class
        {
            var parameters = new Dictionary<string, object>();
            IOrmLiteDialectProvider dialectProvider = OrmLiteConfig.DialectProvider;
            Type type = typeof(T);
            var sql = new StringBuilder("SELECT DISTINCT ");

            var selectExpressions = new Dictionary<string, string>(); // pre vlozenie select vyrazov do where podmienok
            PropertyInfo[] props = type.GetProperties();
            bool commaAdded = false;
            foreach (PropertyInfo prop in props)
            {
                if (prop.HasAttribute<IgnoreAttribute>())
                {
                    continue;
                }

                string colName = prop.HasAttribute<PfeOriginalAliasAttribute>() ? prop.FirstAttribute<PfeOriginalAliasAttribute>().Name : prop.HasAttribute<AliasAttribute>() ? prop.FirstAttribute<AliasAttribute>().Name : prop.Name;
                string alias = prop.Name;

                sql.Append(commaAdded ? ", " : "");
                commaAdded = true;
                if (dto is ILocale && prop.HasAttribute<TranslateByAttribute>())
                {
                    string locale = ((ILocale)dto).Locale ?? "sk";
                    locale = locale.ToLower();
                    if (TranslationDictionary.AcceptableLanguages.Any(nav => nav == locale))
                    {
                        string schema = prop.FirstAttribute<TranslateByAttribute>().Schema;
                        string table = prop.FirstAttribute<TranslateByAttribute>().Table;
                        string column = prop.FirstAttribute<TranslateByAttribute>().Column;
                        string primKey = string.Format("{0}_Id", table);

                        string tenantFilter = "";
                        if (dto is ITenantFilter)
                        {
                            if (!string.IsNullOrEmpty(((ITenantFilter)dto).TenantId))
                            {
                                tenantFilter = " AND (D_Tenant_Id IS NULL OR D_Tenant_Id = @tenant ) ";
                                parameters.AddIfNotExists("@tenant", ((ITenantFilter)dto).TenantId);
                            }
                            else
                            {
                                tenantFilter = " AND D_Tenant_Id IS NULL ";
                            }
                        }
                        string aux = string.Format("ISNULL((SELECT TOP 1 {0} FROM [egov_pub].[V_REG_D_PrekladovySlovnik] WHERE [Kluc] = CONCAT('{1}.{2}.{3}.',{4}){5}), {6})", locale, schema, table, column, dialectProvider.GetQuotedColumnName(primKey), tenantFilter, dialectProvider.GetQuotedColumnName(colName));
                        sql.AppendFormat("{0} AS {1}", aux, alias);

                        selectExpressions.Add(alias, aux);
                    }
                    else
                    {
                        sql.AppendFormat(colName == alias ? "{0}" : "{0} AS {1}", colName, alias);
                        selectExpressions.Add(alias, colName);
                    }
                }
                else
                {
                    sql.AppendFormat(colName == alias ? "{0}" : "{0} AS {1}", colName, alias);
                    selectExpressions.Add(alias, colName);
                }
            }

            // FROM
            sql.Append(" FROM ");
            sql.Append(type.FirstAttribute<SchemaAttribute>().Name);
            sql.Append(".");
            sql.Append(type.HasAttribute<AliasAttribute>() ? type.FirstAttribute<AliasAttribute>().Name : type.Name);

            // WHERE            
            bool whereAdded = false;
            if (typeof(ITenantEntity).IsAssignableFrom(type) && dto is ITenantFilter)
            {
                if (string.IsNullOrEmpty(((ITenantFilter)dto).TenantId))
                {
                    return new List<T>();
                }
                sql.Append(" WHERE (D_Tenant_Id = @tenant)");
                parameters.AddIfNotExists("@tenant", ((ITenantFilter)dto).TenantId);
                whereAdded = true;
            }
            else if (typeof(ITenantEntityNullable).IsAssignableFrom(type) && dto is ITenantFilter)
            {
                if (string.IsNullOrEmpty(((ITenantFilter)dto).TenantId))
                {
                    sql.Append(" WHERE D_Tenant_Id is null");
                    whereAdded = true;
                }
                else
                {
                    sql.Append(" WHERE (D_Tenant_Id = @tenant OR D_Tenant_Id is null)");
                    parameters.AddIfNotExists("@tenant", ((ITenantFilter)dto).TenantId);
                    whereAdded = true;
                }
            }

            if (typeof(ICasovaPlatnost).IsAssignableFrom(type))
            {
                PropertyInfo piPlatnostOd = type.GetProperty("PlatnostOd");
                string platnostOdName = piPlatnostOd.HasAttribute<AliasAttribute>() ? piPlatnostOd.FirstAttribute<AliasAttribute>().Name : piPlatnostOd.Name;
                PropertyInfo piPlatnostDo = type.GetProperty("PlatnostDo");
                string platnostDoName = piPlatnostDo.HasAttribute<AliasAttribute>() ? piPlatnostDo.FirstAttribute<AliasAttribute>().Name : piPlatnostOd.Name;

                sql.Append(whereAdded ? " AND " : " WHERE ");
                whereAdded = true;
                sql.AppendFormat("(({0} IS NULL OR CONVERT(date, {0}) <= CONVERT(date, getdate())) AND ({1} IS NULL OR CONVERT(date, {1}) >= CONVERT(date, getdate())))", platnostOdName, platnostDoName);
            }

            Type dtoType = dto.GetType();
            List<PropertyInfo> criteriaProps = dtoType.GetProperties().Where(x => x.HasAttribute<SearchColumnsAttribute>()).Where(x => x.GetValue(dto) != null).ToList();
            if (criteriaProps.Count > 0)
            {
                var whereClause = new StringBuilder();
                bool andAdded = false;
                foreach (PropertyInfo pCrit in criteriaProps)
                {
                    var conjunct = new StringBuilder();
                    bool orAdded = false;
                    foreach (string s in pCrit.FirstAttribute<SearchColumnsAttribute>().Columns)
                    {
                        string condition = null;
                        PropertyInfo pFromView = type.GetProperties().Where(x => x.Name.EqualsIgnoreCase(s)).FirstOrDefault();
                        if (pFromView == null)
                        {
                            if (searchColumnsCheck)
                            {
                                throw new WebEasException(string.Format("Search column {0} not found in view {1}", s, type.Name));
                            }
                            else
                            {
                                continue;
                            }
                        }

                        Type typeFromView = pFromView.PropertyType;
                        if (typeFromView == typeof(string))
                        {
                            condition = string.Format("({0} LIKE CONCAT('%',@{1},'%'))", selectExpressions.ContainsKey(s) ? selectExpressions[s] : s, s);
                        }
                        else
                        {
                            condition = string.Format("({0} = @{1})", selectExpressions.ContainsKey(s) ? selectExpressions[s] : s, s);
                        }

                        if (condition != null)
                        {
                            conjunct.Append(orAdded ? " OR " : "");
                            orAdded = true;
                            conjunct.Append(condition);
                            parameters.AddIfNotExists(string.Format("@{0}", s), pCrit.GetValue(dto));
                        }
                    }

                    if (conjunct.Length != 0)
                    {
                        whereClause.Append(andAdded ? ") AND (" : "(");
                        andAdded = true;
                        whereClause.Append(conjunct);
                    }
                }

                if (whereClause.Length != 0)
                {
                    sql.Append(whereAdded ? " AND " : " WHERE ");
                    whereAdded = true;
                    sql.Append(whereClause);
                    sql.Append(")");
                }
            }

            if (!string.IsNullOrEmpty(filter))
            {
                sql.Append(whereAdded ? " AND (" : " WHERE (");
                whereAdded = true;

                sql.Append(filter);
                sql.Append(")");
            }

            // PRIDANE fixny filter ak je zadefinovany
            if (type.HasAttribute<FixedFilterAttribute>())
            {
                string val = type.FirstAttribute<FixedFilterAttribute>().Value;
                if (!string.IsNullOrEmpty(val))
                {
                    sql.Append(whereAdded ? " AND " : " WHERE ");
                    whereAdded = true;

                    sql.AppendFormat("({0})", val);
                }
            }

            List<T> result = sourceType.Db.SqlList<T>(sql.ToString(), parameters);
            return result;
        }

        public static object NacitatFiltrovanyZoznam<T>(this IWebEasCoreRepositoryBase sourceType, object dto) where T : class
        {
            return new { Response = sourceType.NacitatZoznam<T>(dto) };
        }

        public static object NacitatFiltrovanyZoznamSPrilohami<T, TPriloha>(this IWebEasCoreRepositoryBase sourceType, object dto, string idColumn)
            where T : class
            where TPriloha : class
        {
            List<T> zoznam = sourceType.NacitatZoznam<T>(dto);

            // ziska sa primary key hodnota a ta sa vlozi do filtra
            Type type = typeof(T);
            PropertyInfo pPrimary = null;
            if (type.GetProperties().Any(nav => nav.HasAttribute<PrimaryKeyAttribute>()))
            {
                pPrimary = type.GetProperties().First(nav => nav.HasAttribute<PrimaryKeyAttribute>());
            }
            else
            {
                throw new WebEasException(string.Format("Primary key not defined for {0}", type));
            }

            foreach (T z in zoznam)
            {
                ((WebEas.ServiceModel.IPrilohy<TPriloha>)z).Prilohy = sourceType.NacitatZoznam<TPriloha>(dto, string.Format("{0} = {1}", idColumn, pPrimary.GetValue(z)), false);
            }

            return new { Response = zoznam };
        }

        public static object NacitatJedinecneNazvy<T>(this IWebEasCoreRepositoryBase sourceType, object dto, string filter = null) where T : class
        {
            var parameters = new Dictionary<string, object>();
            IOrmLiteDialectProvider dialectProvider = OrmLiteConfig.DialectProvider;
            Type type = typeof(T);
            var sql = new StringBuilder("SELECT DISTINCT");

            PropertyInfo pColName = type.GetProperty("Nazov");
            string colName = pColName.HasAttribute<PfeOriginalAliasAttribute>() ? pColName.FirstAttribute<PfeOriginalAliasAttribute>().Name : pColName.HasAttribute<AliasAttribute>() ? pColName.FirstAttribute<AliasAttribute>().Name : pColName.Name;
            string primaryKeyName = null;
            if (type.GetProperties().Any(nav => nav.HasAttribute<PrimaryKeyAttribute>()))
            {
                PropertyInfo pPrimary = type.GetProperties().First(nav => nav.HasAttribute<PrimaryKeyAttribute>());
                primaryKeyName = pPrimary.HasAttribute<AliasAttribute>() ? pPrimary.FirstAttribute<AliasAttribute>().Name : pPrimary.Name;
            }
            else
            {
                PropertyInfo pPrimary = type.GetProperties().FirstOrDefault(nav => nav.Name != "Locale" && nav.Name != colName && nav.Name != "D_Tenant_Id");
                if (pPrimary != null)
                {
                    primaryKeyName = pPrimary.HasAttribute<AliasAttribute>() ? pPrimary.FirstAttribute<AliasAttribute>().Name : pPrimary.Name;
                }
                else
                {
                    throw new WebEasException(string.Format("Primary key not defined for {0}", type));
                }
            }

            bool hasCodeListCode = type.HasAttribute<CodeListCodeAttribute>();
            if (hasCodeListCode)
            {
                sql.AppendFormat(" {0} AS ItemCode,", primaryKeyName);
            }

            if (dto is ILocale)
            {
                string locale = ((ILocale)dto).Locale ?? "sk";
                locale = locale.ToLower();
                if (TranslationDictionary.AcceptableLanguages.Any(nav => nav == locale))
                {
                    string aliasName = type.FirstAttribute<AliasAttribute>().Name;
                    string[] alias = aliasName.Split('_'); // V_EDM_C_SposobDoruceniaOdpovede
                    string schema = type.HasAttribute<PfeOriginalSchemaAttribute>() ? type.FirstAttribute<PfeOriginalSchemaAttribute>().Name : alias[1].ToLower();
                    string table = type.HasAttribute<PfeOriginalAliasAttribute>() ? type.FirstAttribute<PfeOriginalAliasAttribute>().Name : aliasName.Substring(aliasName.IndexOf("_", 2) + 1);

                    string tenantFilter = "";
                    if (dto is ITenantFilter)
                    {
                        if (!string.IsNullOrEmpty(((ITenantFilter)dto).TenantId))
                        {
                            tenantFilter = " AND (D_Tenant_Id IS NULL OR D_Tenant_Id = @tenant ) ";
                            parameters.AddIfNotExists("@tenant", ((ITenantFilter)dto).TenantId);
                        }
                        else
                        {
                            tenantFilter = " AND D_Tenant_Id IS NULL ";
                        }
                    }

                    sql.AppendFormat(" ISNULL((SELECT TOP 1 {0} FROM [egov_pub].[V_REG_D_PrekladovySlovnik] WHERE [Kluc] = CONCAT('{1}.{2}.{3}.',{4}){5}), {6}) AS ItemName", locale, schema, table, colName, dialectProvider.GetQuotedColumnName(primaryKeyName), tenantFilter, dialectProvider.GetQuotedColumnName(colName));
                }
                else
                {
                    sql.AppendFormat(" {0} AS ItemName", colName);
                }
            }
            else
            {
                sql.AppendFormat(" {0} AS ItemName", colName);
            }

            //FROM ");
            sql.Append(" FROM ");
            sql.Append(type.FirstAttribute<SchemaAttribute>().Name);
            sql.Append(".");
            sql.Append(type.HasAttribute<AliasAttribute>() ? type.FirstAttribute<AliasAttribute>().Name : type.Name);
            //ak sa pouziva IZdvLocale, jedna sa o tabulkovu funkciu co ocakava parameter @locale
            if (dto is IZdvLocale)
            {
                sql.Append("(@locale)");
                parameters.AddIfNotExists("@locale", ((IZdvLocale)dto).Locale ?? "sk");
            }

            bool whereAdded = false;
            if (typeof(ITenantEntity).IsAssignableFrom(type) && dto is ITenantFilter)
            {
                if (string.IsNullOrEmpty(((ITenantFilter)dto).TenantId))
                {
                    return new List<object>();
                }
                sql.Append(" WHERE (D_Tenant_Id = @tenant)");
                parameters.AddIfNotExists("@tenant", ((ITenantFilter)dto).TenantId);
                whereAdded = true;
            }
            else if (typeof(ITenantEntityNullable).IsAssignableFrom(type) && dto is ITenantFilter)
            {
                if (string.IsNullOrEmpty(((ITenantFilter)dto).TenantId))
                {
                    sql.Append(" WHERE D_Tenant_Id is null");
                    whereAdded = true;
                }
                else
                {
                    sql.Append(" WHERE (D_Tenant_Id = @tenant OR D_Tenant_Id is null)");
                    parameters.AddIfNotExists("@tenant", ((ITenantFilter)dto).TenantId);
                    whereAdded = true;
                }
            }

            if (typeof(ICasovaPlatnost).IsAssignableFrom(type))
            {
                sql.Append(whereAdded ? " AND " : " WHERE ");
                whereAdded = true; // PRIDANE

                PropertyInfo piPlatnostOd = type.GetProperty("PlatnostOd");
                string platnostOdName = piPlatnostOd.HasAttribute<AliasAttribute>() ? piPlatnostOd.FirstAttribute<AliasAttribute>().Name : piPlatnostOd.Name;
                PropertyInfo piPlatnostDo = type.GetProperty("PlatnostDo");
                string platnostDoName = piPlatnostDo.HasAttribute<AliasAttribute>() ? piPlatnostDo.FirstAttribute<AliasAttribute>().Name : piPlatnostDo.Name;

                sql.AppendFormat("(({0} IS NULL OR {0} <= getdate()) AND ({1} IS NULL OR {1} >= getdate()))", platnostOdName, platnostDoName);
            }

            // PRIDANE dodatocny filter ak je zadefinovany
            Type dtoType = dto.GetType();
            List<PropertyInfo> criteriaProps = dtoType.GetProperties().Where(x => x.HasAttribute<SearchColumnsAttribute>()).Where(x => x.GetValue(dto) != null).ToList();
            if (criteriaProps.Count > 0)
            {
                sql.Append(whereAdded ? " AND " : " WHERE ");
                whereAdded = true;

                bool andAdded = false;
                foreach (PropertyInfo pCrit in criteriaProps)
                {
                    sql.Append(andAdded ? ") AND (" : "(");
                    andAdded = true;

                    bool orAdded = false;
                    foreach (string s in pCrit.FirstAttribute<SearchColumnsAttribute>().Columns)
                    {
                        sql.Append(orAdded ? " OR " : "");
                        orAdded = true;

                        PropertyInfo pFromView = type.GetProperties().Where(x => x.Name.EqualsIgnoreCase(s)).FirstOrDefault();
                        if (pFromView == null)
                        {
                            throw new WebEasException(string.Format("Search column {0} not found in view {1}", s, type.Name));
                        }

                        string alias = s;
                        if (s == "Nazov")
                        {
                            alias = colName;
                        }
                        else if (pFromView.HasAttribute<AliasAttribute>())
                        {
                            alias = pFromView.FirstAttribute<AliasAttribute>().Name;
                        }

                        Type typeFromView = pFromView.PropertyType;
                        if (typeFromView == typeof(string))
                        {
                            sql.AppendFormat("({0} LIKE CONCAT('%',@{1},'%'))", alias, s);
                        }
                        else
                        {
                            sql.AppendFormat("({0} LIKE @{1})", alias, s);
                        }
                        parameters.AddIfNotExists(string.Format("@{0}", s), pCrit.GetValue(dto));
                    }
                }
                sql.Append(")");
            }

            // PRIDANE fixny filter ak je zadefinovany
            if (type.HasAttribute<FixedFilterAttribute>())
            {
                string val = type.FirstAttribute<FixedFilterAttribute>().Value;
                if (!string.IsNullOrEmpty(val))
                {
                    sql.Append(whereAdded ? " AND " : " WHERE ");
                    whereAdded = true;

                    sql.AppendFormat("({0})", val);
                }
            }

            // PRIDANE dodatocny filter ak je zadefinovany
            if (!string.IsNullOrEmpty(filter))
            {
                sql.Append(whereAdded ? " AND " : " WHERE ");
                whereAdded = true;

                sql.AppendFormat("({0})", filter);
            }

            sql.Append(" ORDER BY ItemName");

            if (hasCodeListCode)
            {
                List<DialEntity<long>> result = sourceType.Db.SqlList<DialEntity<long>>(sql.ToString(), parameters);
                return new { Response = result };
            }
            else
            {
                List<string> result = sourceType.Db.SqlList<string>(sql.ToString(), parameters);
                return new { Response = result };
            }
        }

        public static List<T> NacitatZoznam<T>(this IWebEasCoreRepositoryBase sourceType, Expression<Func<T, bool>> filter, string tenantId) where T : class
        {
            SqlExpression<T> ev = sourceType.Db.GetDialectProvider().SqlExpression<T>();
            ev.Where(filter);

            return sourceType.NacitatZoznam<T>(ev, tenantId);
        }

        private static List<T> NacitatZoznam<T>(this IWebEasCoreRepositoryBase sourceType, SqlExpression<T> filter, string tenantId) where T : class 
        {
            Guid filterTenantGuid;

            Type typEntity = typeof(T);

            if (typeof(ITenantEntity).IsAssignableFrom(typEntity))
            {
                if (string.IsNullOrWhiteSpace(tenantId) || !Guid.TryParse(tenantId, out filterTenantGuid))
                {
                    return new List<T>();
                }
                
                filter.And<ITenantEntity>(f => f.D_Tenant_Id == filterTenantGuid);
            }
            else if (typeof(ITenantEntityNullable).IsAssignableFrom(typEntity))
            {
                if (string.IsNullOrWhiteSpace(tenantId) || !Guid.TryParse(tenantId, out filterTenantGuid))
                {
                    return new List<T>();
                }

                filter.And<ITenantEntityNullable>(f => f.D_Tenant_Id == filterTenantGuid || f.D_Tenant_Id == null);
            }

            return sourceType.Db.Select<T>(filter).ToList();
        }
    }
}