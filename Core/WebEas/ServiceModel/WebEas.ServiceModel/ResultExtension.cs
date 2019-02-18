using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ServiceStack;
using ServiceStack.DataAnnotations;
using System.Text.RegularExpressions;

namespace WebEas.ServiceModel
{
    public static class ResultExtension
    {
        /// <summary>
        /// Converts to.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static T ConvertTo<T>(this int value) where T : IResult<int>, new()
        {
            var result = new T();
            result.Result = value;
            return result;
        }

        /// <summary>
        /// Converts to.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static T ConvertTo<T>(this bool value) where T : IResult<bool>, new()
        {
            var result = new T();
            result.Result = value;
            return result;
        }

        /// <summary>
        /// Converts to.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <typeparam name="V">The type of the V.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static T ConvertTo<T, V>(this V value)
            where T : IResult<V>, new()
            where V : new()
        {
            var result = new T();
            result.Result = value;
            return result;
        }

        /// <summary>
        /// Toes the identifier.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static long ToIdentifier(this DateTime value)
        {
            return (long)value.TimeOfDay.TotalMilliseconds;
        }

        /// <summary>
        /// Vyhodi vynimku ak nenajde
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static PropertyInfo GetPi(this Type type, string propertyName)
        {
            PropertyInfo pi = type.GetProperty(propertyName);
            if (pi == null)
            {
                throw new WebEasException(string.Format("Zle mapovanie triedy {0} {1}", type, propertyName));
            }
            return pi;
        }

        /// <summary>
        /// Gets the pi.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static PropertyInfo GetPi(this object data, string propertyName)
        {
            if (data == null)
            {
                return null;
            }
            return data.GetType().GetPi(propertyName);
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="data">The data.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="required">The required.</param>
        /// <param name="notFoundMessage">The not found message.</param>
        /// <returns></returns>
        public static object GetValue<T>(this T data, string propertyName, bool required = false, string notFoundMessage = null) where T : class
        {
            object value = data == null 
                ? null
                : data.GetType().GetPi(propertyName).GetValue(data);
            if (value == null && required)
            {
                throw new WebEasValidationException(string.Format("{0} of {1} is null", propertyName, data == null ? typeof(T) : data.GetType()), notFoundMessage);
            }
            return value;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static object GetValue<T>(this T data)
        {
            if (data == null)
            {
                return null;
            }

            IEnumerable<PropertyInfo> properties = data.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance).Where(nav => !nav.HasAttribute<PrimaryKeyAttribute>());
            PropertyInfo valueCol = null;

            if (properties.Any(nav => nav.HasAttribute<PfeValueColumnAttribute>()))
            {
                valueCol = properties.First(nav => nav.HasAttribute<PfeValueColumnAttribute>());
            }
            else if (data.GetType().GetProperties().Any(nav => nav.HasAttribute<PfeValueColumnAttribute>()))
            {
                valueCol = data.GetType().GetProperties().First(nav => nav.HasAttribute<PfeValueColumnAttribute>());
            }
            else if (properties.Any(nav => nav.Name.ToLower().ContainsAny("nazov", "name", "popis")))
            {
                valueCol = properties.First(nav => nav.Name.ToLower().ContainsAny("nazov", "name", "popis"));
            }
            else
            {
                valueCol = properties.Any(nav => nav.PropertyType == typeof(string)) ? properties.First(nav => nav.PropertyType == typeof(string)) : properties.First();
            }

            if (valueCol == null)
            {
                return null;
            }
            else
            {
                return valueCol.GetValue(data);
            }
        }

        /// <summary>
        /// Adds if not exists.
        /// </summary>
        /// <typeparam name="TKey">The type of the T key.</typeparam>
        /// <typeparam name="TValue">The type of the T value.</typeparam>
        /// <param name="data">The data.</param>
        /// <param name="key">The key.</param>
        /// <param name="val">The val.</param>
        public static void AddIfNotExists<TKey, TValue>(this System.Collections.Generic.Dictionary<TKey, TValue> data, TKey key, TValue val)
        {
            if (!data.ContainsKey(key))
            {
                data.Add(key, val);
            }
        }

        /// <summary>
        /// Adds the and change name if exists.
        /// </summary>
        /// <typeparam name="TValue">The type of the T value.</typeparam>
        /// <param name="data">The data.</param>
        /// <param name="key">The key.</param>
        /// <param name="val">The val.</param>
        /// <returns></returns>
        public static string AddAndChangeNameIfExists<TValue>(this System.Collections.Generic.Dictionary<string, TValue> data, string key, TValue val)
        {
            key = key.ToUpper();

            if (data.ContainsKey(key))
            {
                if ((data[key] == null && val == null) || data[key].Equals(val))
                {
                    return key;
                }

                string newName;
                int count = 0;
                do
                {
                    newName = string.Format("{0}__{1}", key.ToUpper(), ++count);
                }
                while (data.ContainsKey(newName));
                key = newName.ToUpper();
                data.Add(key, val);
            }
            else
                data.Add(key, val);
       
            return key;
        }

        /// <summary>
        /// Checks the email address.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        public static string CheckEmailAddress(this string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }

            email = email.Split(';')[0];

            if (!email.StartsWith("mailto:"))
            {
                email = string.Format("mailto:{0}", email);
            }

            try
            {
                var addr = new System.Net.Mail.MailAddress(Regex.Match(email, @"(?<=mailto:).*(?=)").Value);
            }
            catch
            {
                return null;
            }

            return email;
        }

        /// <summary>
        /// Checks the web address.
        /// </summary>
        /// <param name="web">The web.</param>
        /// <returns></returns>
        public static string CheckWebAddress(this string web)
        {
            if (string.IsNullOrEmpty(web))
            {
                return null;
            }

            web = web.Split(';')[0];

            if (!Regex.Match(web, @"http(s)?://(.)").Success)
            {
                web = string.Format("http://{0}", web);
            }

            Uri uriResult;
            bool result = Uri.TryCreate(web, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            return result ? web : null;
        }

        /// <summary>
        /// Checks the phone number.
        /// </summary>
        /// <param name="phone">The phone.</param>
        /// <returns></returns>
        public static string CheckPhoneNumber(this string phone)
        {
            if (string.IsNullOrEmpty(phone))
            {
                return null;
            }

            phone = phone.Replace(" ", "").Replace("/", "");

            if (phone.Contains("-"))
            {
                phone = phone.Split('-')[0];
            }

            if (!phone.StartsWith("+"))
            {
                if (phone.StartsWith("0"))
                {
                    phone = phone.Substring(1);
                }

                phone = string.Format("+421{0}", phone);
            }
            return phone;
        }

        /// <summary>
        /// Checks the psč number.
        /// </summary>
        /// <param name="psc">The psč.</param>
        /// <returns></returns>
        public static string CheckPscNumber(this string psc)
        {
            if (string.IsNullOrEmpty(psc))
            {
                return "";
            }

            psc = psc.Trim().Replace(" ", "");

            if (psc.Length > 3)
            {
                psc = psc.Insert(3, " ");
            }
            
            return psc;
        }

        /// <summary>
        /// Checks the empty.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string CheckEmpty(this string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }
            return data;
        }

        /// <summary>
        /// Toes the byte array.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static byte[] ToByteArray(this Stream stream)
        {
            using (stream)
            {
                using (var memStream = new MemoryStream())
                {
                    stream.CopyTo(memStream);
                    return memStream.ToArray();
                }
            }
        }

        #region int.IN()

        /// <summary>
        /// Check if your value is IN the specified list of possible values
        /// </summary>
        public static bool In(this int value, params int[] list)
        {
            return Array.IndexOf(list, value) >= 0;
        }

        /// <summary>
        /// Check if your value is IN the specified list of possible values
        /// </summary>
        public static bool In(this int value, int val1, int val2)
        {
            return value == val1 || value == val2;
        }

        /// <summary>
        /// Check if your value is IN the specified list of possible values
        /// </summary>
        public static bool In(this int value, int val1, int val2, int val3)
        {
            return value == val1 || value == val2 || value == val3;
        }

        /// <summary>
        /// Check if your value is IN the specified list of possible values
        /// </summary>
        public static bool In(this int value, int val1, int val2, int val3, int val4)
        {
            return value == val1 || value == val2 || value == val3 || value == val4;
        }

        #endregion

        #region long.IN()

        /// <summary>
        /// Check if your value is IN the specified list of possible values
        /// </summary>
        public static bool In(this long value, params long[] list)
        {
            return Array.IndexOf(list, value) >= 0;
        }

        /// <summary>
        /// Check if your value is IN the specified list of possible values
        /// </summary>
        public static bool In(this long value, long val1, long val2)
        {
            return value == val1 || value == val2;
        }

        /// <summary>
        /// Check if your value is IN the specified list of possible values
        /// </summary>
        public static bool In(this long value, long val1, long val2, long val3)
        {
            return value == val1 || value == val2 || value == val3;
        }

        /// <summary>
        /// Check if your value is IN the specified list of possible values
        /// </summary>
        public static bool In(this long value, long val1, long val2, long val3, long val4)
        {
            return value == val1 || value == val2 || value == val3 || value == val4;
        }

        #endregion

        #region string.IN()

        /// <summary>
        /// Check if your value is IN the specified list of possible values
        /// </summary>
        public static bool In(this string value, params string[] list)
        {
            return Array.IndexOf(list, value) >= 0;
        }

        /// <summary>
        /// Check if your value is IN the specified list of possible values
        /// </summary>
        public static bool In(this string value, string val1, string val2)
        {
            return value == val1 || value == val2;
        }

        /// <summary>
        /// Check if your value is IN the specified list of possible values
        /// </summary>
        public static bool In(this string value, string val1, string val2, string val3)
        {
            return value == val1 || value == val2 || value == val3;
        }

        /// <summary>
        /// Check if your value is IN the specified list of possible values
        /// </summary>
        public static bool In(this string value, string val1, string val2, string val3, string val4)
        {
            return value == val1 || value == val2 || value == val3 || value == val4;
        }

        /// <summary>
        /// Check if your value is IN the specified list of possible values
        /// </summary>
        public static bool In(this string value, string val1, string val2, string val3, string val4, string val5)
        {
            return value == val1 || value == val2 || value == val3 || value == val4 || value == val5;
        }

        /// <summary>
        /// Check if your value is IN the specified list of possible values, allow string comparison to be speciefed
        /// </summary>
        public static bool In2(this string value, StringComparer comparer, params string[] list)
        {
            return list.Contains(value, comparer ?? StringComparer.Ordinal);
        }
        
        #endregion
    }
}