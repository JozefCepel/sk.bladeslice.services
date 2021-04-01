using System;
using System.Linq;
using System.Text;
using WebEas.ServiceModel;

namespace WebEas.ServiceInterface
{
    public class TextationProcessor
    {
        public static string Process(ServiceStack.Logging.ILog Log, string textation, ReplacementsDictionary replacements)
        {
            //kontrola parameterov a ci je co prepisovat
            if (replacements == null || string.IsNullOrEmpty(textation) || textation.IndexOf('{') < 0) return textation;
            
            //mozem pokracovat
            StringBuilder sb = new StringBuilder(textation.Length);

            int sidx = -1; //zaciatok tagu
            int sidx1 = -1; //zaciatok tagu
            int eidx = -1; //index konca tagu
            try
            {
                do
                {
                    //advance to next tag
                    eidx++;
                    sidx = textation.IndexOf('{', eidx);
                    sidx1 = textation.IndexOf('{', sidx+1);
                    if (sidx < 0) break;

                    if (sidx - eidx > 0)
                        sb.Append(textation.Substring(eidx, sidx - eidx));
                    eidx = textation.IndexOf('}', sidx);
                   
                        if (eidx < 0) //no tag end?
                        {
                            eidx = textation.Length;
                            break;
                        }

                        //get key and replace with value
                        string key = textation.Substring(sidx + 1, eidx - sidx - 1);
                        string value;

                        int fidx = key.IndexOf(';');
                        if (fidx > 0)
                        {
                            if (sidx1 < eidx && sidx1 > 0)
                            { value = "{" + key.Substring(0, fidx) + ";"; }
                            else
                            { value = replacements.Get(key.Substring(0, fidx), key.Substring(fidx + 1)); if (value == "{Podpis}") { value = "{Podpis;;;}"; } }
                        }
                        else
                        {
                             value = replacements.Get(key);
                          }

                        if (!string.IsNullOrEmpty(value))
                            sb.Append(value);
                        if (sidx1 < eidx && sidx1 > 0)
                        { eidx = sidx1 - 1; }
                    
                }
                while (true);

                //append rest of content
                if (eidx < textation.Length)
                    sb.Append(textation.Substring(eidx));
            }
            catch (Exception ex)
            {
                if (Log != null) Log.Error(ex);
                return textation;
            }


            return sb.ToString();
        }

        public static string ProcessExtract(ServiceStack.Logging.ILog Log, string textation)
        {

            //mozem pokracovat
            StringBuilder sb = new StringBuilder(textation.Length);

            int sidx = -1; //zaciatok tagu
            int sidx1 = -1; //zaciatok tagu
            int eidx = -1; //index konca tagu
            try
            {
                do
                {
                    //advance to next tag
                    eidx++;
                    sidx = textation.IndexOf('{', eidx);
                    sidx1 = textation.IndexOf('{', sidx + 1);
                    if (sidx < 0) break;

                    if (sidx - eidx > 0)
                        sb.Append(textation.Substring(eidx, sidx - eidx));
                    eidx = textation.IndexOf('}', sidx);

                    if (eidx < 0) //no tag end?
                    {
                        eidx = textation.Length;
                        break;
                    }

                    //get key and replace with value
                    string key = textation.Substring(sidx + 1, eidx - sidx - 1);
                    //string value;

                    //int fidx = key.IndexOf(';');
                    //if (fidx > 0)
                    //{
                    //    if (sidx1 < eidx && sidx1 > 0)
                    //    { value = "{" + key.Substring(0, fidx) + ";"; }
                    //    else
                    //    { value = replacements.Get(key.Substring(0, fidx), key.Substring(fidx + 1)); }
                    //}
                    //else
                    //{
                    //    value = replacements.Get(key);
                    //}

                    //if (!string.IsNullOrEmpty(value))
                    //    sb.Append(value);
                    if (sidx1 < eidx && sidx1 > 0)
                    { eidx = sidx1 - 1; }

                }
                while (true);

                //append rest of content
                if (eidx < textation.Length)
                    sb.Append(textation.Substring(eidx));
            }
            catch (Exception ex)
            {
                if (Log != null) Log.Error(ex);
                return textation;
            }


            return sb.ToString();
        }

        /// <summary>
        /// Adds to replacements dictionary name-value pairs from all public properties from given data which has PfeColumnAttribute.
        /// The path should be specified in case of nested data (by dependency relation) - and defines the 'prefix' (which is then appended by '/' char inside)
        /// </summary>
        public static void FillDictionaryFromObject(ReplacementsDictionary replacements, object data, string path = null)
        {
            //standardize path
            path = path == null ? "" : path + ".";

            var props = (from p in data.GetType().GetProperties()
                         let attr = p.GetCustomAttributes(typeof(PfeColumnAttribute), true)
                         where attr.Length == 1
                         select new { Property = p, Attribute = attr.First() as PfeColumnAttribute }).ToList();

            foreach (var item in props)
            {
                object ovalue = item.Property.GetValue(data);

                string format = null;
                if (ovalue != null)
                {
                    if (item.Attribute.Type == PfeDataType.Date)
                    {
                        format = "D"; //date
                    }
                    else if (item.Attribute.Type == PfeDataType.DateTime)
                    {
                        format = "DT"; //date and time
                    }
                    else if (item.Attribute.Type == PfeDataType.Time)
                    {
                        format = "T"; //time
                    }
                    else if (ovalue is decimal)
                    {
                        switch (item.Attribute.DecimalPlaces)
                        {
                            case 0:
                                format = "N0";
                                break;
                            case 1:
                                format = "N1";
                                break;
                            case 2:
                                format = "N2";
                                break;
                            case 3:
                                format = "N3";
                                break;
                            case 4:
                                format = "N4";
                                break;
                            case 5:
                                format = "N5";
                                break;
                        }
                    }
                }

                string name = path + (item.Attribute.Text ?? item.Property.Name);
                replacements.Add(name, ovalue, format);
            }
        }

        /// <summary>
        /// Adds to replacements dictionary name-value pairs from all public properties from given data which has PfeColumnAttribute.
        /// The path should be specified in case of nested data (by dependency relation) - and defines the 'prefix' (which is then appended by '/' char inside)
        /// </summary>
        public static void FillDictionaryFromObjectByProperties(ReplacementsDictionary replacements, object data, string path = null)
        {
            //standardize path
            path = path == null ? "" : path + ".";

            foreach (var propertyInfo in data.GetType().GetProperties())
            {
                object ovalue = propertyInfo.GetValue(data);
                string format = null;
                string name = path + propertyInfo.Name;

                replacements.Add(name, ovalue, format);
            }
        }


    }
}
