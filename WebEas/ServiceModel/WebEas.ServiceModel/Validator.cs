using ServiceStack;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace WebEas.ServiceModel
{
    public static class Validator
    {
        private const string RequiredValidationMessage = "Položka '{0}' je povinná!";
        private const string RangeValidationMessage = "Hodnota položky '{0}' musí byť aspoň {1}!";
        private const string MaxLengthValidationMessage = "Hodnota položky '{0}' nesmie byť dlhšia ako {1} znakov!";
        private const string MinLengthValidationMessage = "Hodnota položky '{0}' nesmie byť kratšia ako {1} znakov!";

        /// <summary>
        /// Gets the caption.
        /// </summary>
        /// <param name="dataProp">The data prop.</param>
        /// <param name="descProps">The desc props.</param>
        /// <returns></returns>
        private static string GetCaption(PropertyInfo dataProp, PropertyInfo[] descProps = null)
        {
            if (descProps == null || !descProps.Any(nav => nav.Name == dataProp.Name))
            {
                if (dataProp.HasAttribute<PfeColumnAttribute>())
                {
                    string text = dataProp.FirstAttribute<PfeColumnAttribute>().Text;
                    if (!string.IsNullOrEmpty(text))
                    {
                        return text;
                    }
                }

                return dataProp.Name;
            }

            PropertyInfo p = descProps.First(nav => nav.Name == dataProp.Name);
            if (p.HasAttribute<PfeColumnAttribute>())
            {
                string text = p.FirstAttribute<PfeColumnAttribute>().Text;
                if (!string.IsNullOrEmpty(text))
                {
                    return text;
                }
            }
            return dataProp.Name;
        }

        /// <summary>
        /// Validates the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="descriptionType">Type of the description.</param>
        public static void Validate<T>(T data, Operation operationType, IWebEasRepositoryBase repository, Type descriptionType = null) where T : class
        {
            if (data == null)
            {
                return;
            }

            var errors = new List<string>();
            PropertyInfo[] descProps = descriptionType == null ? null : descriptionType.GetProperties();

            foreach (PropertyInfo prop in data.GetType().GetProperties())
            {
                #region Required

                if (prop.HasAttribute<RequiredAttribute>())
                {
                    object value = prop.GetValue(data);

                    if (value == null || (value is string && string.IsNullOrEmpty(value as string)))
                    {
                        errors.Add(string.Format(RequiredValidationMessage, GetCaption(prop, descProps)));
                        continue;
                    }
                }
                else if (prop.HasAttribute<System.ComponentModel.DataAnnotations.RequiredAttribute>())
                {
                    System.ComponentModel.DataAnnotations.RequiredAttribute requiredAttribute = prop.FirstAttribute<System.ComponentModel.DataAnnotations.RequiredAttribute>();
                    object value = prop.GetValue(data);

                    if (value == null || (value is string && !requiredAttribute.AllowEmptyStrings && string.IsNullOrEmpty(value as string)))
                    {
                        errors.Add(string.IsNullOrEmpty(requiredAttribute.ErrorMessage)
                                   ? string.Format(RequiredValidationMessage, GetCaption(prop, descProps))
                                   : requiredAttribute.ErrorMessage);
                        continue;
                    }
                }
                else if (prop.HasAttribute<NotEmptyOrDefaultAttribute>())
                {
                    NotEmptyOrDefaultAttribute notEmptyAttribute = prop.FirstAttribute<NotEmptyOrDefaultAttribute>();
                    object value = prop.GetValue(data);

                    if (!notEmptyAttribute.IsValid(value))
                    {
                        errors.Add(string.IsNullOrEmpty(notEmptyAttribute.ErrorMessage)
                                   ? string.Format(RequiredValidationMessage, GetCaption(prop, descProps))
                                   : notEmptyAttribute.ErrorMessage);
                        continue;
                    }
                }

                #endregion

                #region Range

                if (prop.HasAttribute<System.ComponentModel.DataAnnotations.RangeAttribute>())
                {
                    System.ComponentModel.DataAnnotations.RangeAttribute rangeAttribute = prop.FirstAttribute<System.ComponentModel.DataAnnotations.RangeAttribute>();

                    object value = prop.GetValue(data);

                    if (value != null)
                    {
                        double min;
                        double max;
                        double val;

                        if (double.TryParse(value.ToString(), out val) &&
                            double.TryParse(rangeAttribute.Minimum.ToString(), out min) &&
                            double.TryParse(rangeAttribute.Maximum.ToString(), out max))
                        {
                            if (val < min || val > max)
                            {
                                errors.Add(string.IsNullOrEmpty(rangeAttribute.ErrorMessage)
                                           ? string.Format(RangeValidationMessage, GetCaption(prop, descProps), rangeAttribute.Minimum)
                                           : rangeAttribute.ErrorMessage);
                                continue;
                            }
                        }
                    }
                }

                if (prop.HasAttribute<RangeAttribute>())
                {
                    RangeAttribute rangeAttribute = prop.FirstAttribute<RangeAttribute>();

                    object value = prop.GetValue(data);

                    if (value != null)
                    {
                        double min;
                        double max;
                        double val;

                        if (double.TryParse(value.ToString(), out val) &&
                            double.TryParse(rangeAttribute.Minimum.ToString(), out min) &&
                            double.TryParse(rangeAttribute.Maximum.ToString(), out max))
                        {
                            if (val < min || val > max)
                            {
                                errors.Add(string.Format(RangeValidationMessage, GetCaption(prop, descProps), rangeAttribute.Minimum));
                                continue;
                            }
                        }
                    }
                }

                #endregion

                #region Length

                if (prop.HasAttribute<System.ComponentModel.DataAnnotations.MaxLengthAttribute>())
                {
                    System.ComponentModel.DataAnnotations.MaxLengthAttribute maxLengthAttribute = prop.FirstAttribute<System.ComponentModel.DataAnnotations.MaxLengthAttribute>();

                    var value = prop.GetValue(data) as string;

                    if (!string.IsNullOrEmpty(value) && value.Length > maxLengthAttribute.Length)
                    {
                        errors.Add(string.IsNullOrEmpty(maxLengthAttribute.ErrorMessage)
                                   ? string.Format(MaxLengthValidationMessage, GetCaption(prop, descProps), maxLengthAttribute.Length)
                                   : maxLengthAttribute.ErrorMessage);
                        continue;
                    }
                }

                if (prop.HasAttribute<StringLengthAttribute>())
                {
                    StringLengthAttribute maxLengthAttribute = prop.FirstAttribute<StringLengthAttribute>();

                    var value = prop.GetValue(data) as string;

                    if (!string.IsNullOrEmpty(value))
                    {
                        if (value.Length > maxLengthAttribute.MaximumLength)
                        {
                            errors.Add(string.Format(MaxLengthValidationMessage, GetCaption(prop, descProps), maxLengthAttribute.MaximumLength));
                            continue;
                        }
                        else if (value.Length < maxLengthAttribute.MinimumLength)
                        {
                            errors.Add(string.Format(MinLengthValidationMessage, GetCaption(prop, descProps), maxLengthAttribute.MinimumLength));
                            continue;
                        }
                    }
                }

                #endregion
            }

            #region SqlValidation
            if (!(data is IDto) && data.GetType().HasAttribute<SqlValidationAttribute>())
            {
                var sqlValidationAttribute = data.GetType().AllAttributes<SqlValidationAttribute>();

                var props = data.GetType().GetProperties();
                if (operationType == Operation.Delete)
                {
                    if (props.Any(nav => nav.HasAttribute<PrimaryKeyAttribute>()))
                    {
                        var pPrimary = props.First(nav => nav.HasAttribute<PrimaryKeyAttribute>());
                        data = repository.Db.SingleById<T>(pPrimary.GetValue(data)); //prebijem "data" z requestu za komplet dáta entity
                    }
                }

                foreach (var attr in sqlValidationAttribute)
                {
                    if (string.IsNullOrEmpty(attr.Sql))
                    {
                        throw new ArgumentNullException(nameof(attr.Sql));
                    }

                    if (attr.OperationType == operationType)
                    {
                        var parameters = new Dictionary<string, object>();

                        if (!string.IsNullOrEmpty(attr.Sql))
                        {
                            if (attr.Sql.Contains("@"))
                            {
                                foreach (var parameterName in Regex.Matches(attr.Sql, @"\@\w+").Cast<Match>().Select(m => m.Value.Remove(0, 1)).Distinct())
                                {
                                    var prop = props.FirstOrDefault(x => x.HasAttribute<AliasAttribute>() && x.GetCustomAttribute<AliasAttribute>().Name == parameterName);

                                    if (prop == null)
                                    {
                                        prop = props.FirstOrDefault(x => x.Name == parameterName);
                                    }

                                    if (prop == null)
                                    {
                                        throw new NotSupportedException($"Nepodarilo sa nájsť hodnotu parametra {parameterName} do where podmienky!");
                                    }

                                    parameters.Add(parameterName, prop.GetValue(data));
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(attr.DatumField))
                        {
                            var (val, minDate, maxDate) = repository.Db.Select<(decimal val, DateTime? minDate, DateTime? maxDate)>(attr.Sql, anonType: parameters).FirstOrDefault();
                            if (val != 0)
                            {
                                string err = attr.ErrorMessage;
                                err = err.Replace($"Min{attr.DatumField}", minDate.Value.ToString("d.M.yyyy"));
                                err = err.Replace($"Max{attr.DatumField}", maxDate.Value.ToString("d.M.yyyy"));
                                errors.Add(err);
                                break; //stačí jedna hláška
                            }
                        }
                        else
                        {
                            decimal val = repository.Db.Scalar<long>(attr.Sql, anonType: parameters);
                            if (val != 0)
                            {
                                errors.Add(attr.ErrorMessage);
                                break; //stačí jedna hláška
                            }
                        }
                    }
                }
            }

            #endregion

            if (errors.Count > 0)
            {
                var ex = new WebEasValidationException(string.Format("Not valid {0}", data.GetType()), errors.Join(", "));
                ex.Errors = errors;
                throw ex;
            }

            if (data is IValidate validate)
            {
                validate.Validate();
            }
        }
    }
}