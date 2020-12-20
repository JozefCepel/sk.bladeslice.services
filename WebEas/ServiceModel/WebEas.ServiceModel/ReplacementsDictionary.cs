using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebEas.ServiceModel
{
    public class ReplacementsDictionary
    {
        Dictionary<string, DicItem> dictionary;
        CultureInfo culture;

        public ReplacementsDictionary()
        {
            dictionary = new Dictionary<string, DicItem>(100);
            culture = System.Globalization.CultureInfo.CreateSpecificCulture("sk-sk");
        }

        /// <summary>
        /// Add new simple string replacement
        /// </summary>
        public void Add(string key, string value)
        {
            key = key.ToLower();
            if (!dictionary.ContainsKey(key))
                dictionary.Add(key, new DicItem(value));
        }

        /// <summary>
        /// Add new replacement with format specified.
        /// The format must be one of predefined types: N0, N1, N2, N3, N4, D, T, DT
        /// </summary>
        public void Add(string key, object value, string format)
        {
            key = key.ToLower();
            if (!dictionary.ContainsKey(key))
                dictionary.Add(key, new DicItem(value, format));
        }

        /// <summary>
        /// Gets the string which should be replaced instead of given key.
        /// If the key is not valid, returns the key surronded with brackets - {key}
        /// If format is specified will force this one instead of predefined (based on data type and PfeColumn attribute..)
        /// </summary>
        /// <param name="key">Key without the brackets</param>
        /// <param name="format">One of the predefined formats (N0, N1, N2, N3, N4, D, T, DT) or null</param>
        public string Get(string key, string format = null)
        {
            DicItem item;
            string oldkey = key;
            key = key.ToLower();
            if (!dictionary.TryGetValue(key, out item))
                return "{" + oldkey + "}";

            if (item.value == null)
            {
                return "";
            }

            if (format == null) format = item.format ?? "";

            switch (format)
            {
                case "D": return string.Format(culture, "{0:d.M.yyyy}", item.value); //dd.MM.yyyy ?
                case "DT": return string.Format(culture, "{0:d.M.yyyy H:mm}", item.value); //dd.MM.yyyy HH:mm ?
                case "T": return string.Format(culture, "{0:H:mm}", item.value); //HH:mm ?
                case "N0": return string.Format(culture, "{0:0}", item.value);
                case "N1": return string.Format(culture, "{0:0.0}", item.value);
                case "N2": return string.Format(culture, "{0:0.00}", item.value);
                case "N3": return string.Format(culture, "{0:0.000}", item.value);
                case "N4": return string.Format(culture, "{0:0.0000}", item.value);
                default: return item.value.ToString();
            }
        }

        class DicItem
        {
            public object value;
            public string format;

            public DicItem(string text)
            {
                this.value = text;
                this.format = null;
            }

            public DicItem(object value, string format)
            {
                this.value = value;
                this.format = format;
            }
        }
    }
}
