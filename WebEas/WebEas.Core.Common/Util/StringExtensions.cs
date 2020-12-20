using System;
using System.Linq;

namespace WebEas
{
    public static class StringExtensions
    {
        /// <summary>
        /// Firsts up.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string FirstUp(this string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return string.Empty;
            }
            return string.Format("{0}{1}", char.ToUpper(data[0]), data.Substring(1));
        }
    }
}