using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WebEas
{
    /// <summary>
    /// 
    /// </summary>
    public static class WebEasExceptionExtensions
    {
        private const string BaseIndent = "";

        /// <summary>
        /// Toes the description.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public static string ToDescription(this Exception ex)
        {
            if (ex == null)
            {
                return null;
            }

            var sb = new StringBuilder();
            //  sb.Append(Environment.NewLine);
            sb.Append(BaseIndent);
            sb.Append(ex.Message);
            sb.Append(Environment.NewLine);

            Type type = ex.GetType();
            sb.Append(BaseIndent);
            sb.Append("Type: ");
            sb.Append(ex.GetType());
            sb.Append(Environment.NewLine);

            if (!String.IsNullOrEmpty(ex.Source))
            {
                sb.Append(BaseIndent);
                sb.Append("Source: ");
                sb.Append(ex.Source);
                sb.Append(Environment.NewLine);
            }

            if (ex.GetType() != typeof(Exception))
            {
                foreach (PropertyInfo info in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (info.DeclaringType != typeof(Exception))
                    {
                        if (info.PropertyType.IsClass && info.GetValue(ex, null) != null && info.PropertyType.IsSerializable)
                        {
                            try
                            {
                                sb.Append(BaseIndent);
                                sb.Append(info.Name);
                                sb.Append(": ");
                                sb.Append(WebEas.Core.Log.WebEasNLogExtensions.ToJsonString(info.GetValue(ex, null)));
                                sb.Append(Environment.NewLine);
                            }
                            catch
                            {
                                sb.Append(BaseIndent);
                                sb.Append(info.Name);
                                sb.Append(": ");
                                sb.Append(info.GetValue(ex, null));
                                sb.Append(Environment.NewLine);
                            }
                        }
                        else
                        {
                            sb.Append(BaseIndent);
                            sb.Append(info.Name);
                            sb.Append(": ");
                            sb.Append(info.GetValue(ex, null));
                            sb.Append(Environment.NewLine);
                        }
                    }
                }
            }

            if (!String.IsNullOrEmpty(ex.StackTrace))
            {
                sb.Append(BaseIndent);
                sb.Append("Stack: ");
                String stack = ex.StackTrace.Replace("   at", "        at");
                sb.Append(stack.Remove(0, ("        at").Length));
                sb.Append(Environment.NewLine);
            }

            if (ex.InnerException != null)
            {
                AppendError(ex.InnerException, sb, 1);
            }

            //  sb.Append(Environment.NewLine);
            return sb.ToString();
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public static string GetIdentifier(this Exception ex)
        {
            unchecked
            {
                long result = 17;
                result = result * 23 + ((ex.Message != null) ? ex.Message.GetHashCode() : 0);
                result = result * 23 + ex.GetType().FullName.GetHashCode();
                result = result * 23 + ((ex is WebEasException && ((WebEasException)ex).HasMessageUser) ? ((WebEasException)ex).MessageUser.GetHashCode() : 0);
                result = result * 23 + ((ex.InnerException != null) ? ex.InnerException.GetInnerHashCode() : 0);
                return result > 0 ? string.Format("D{0}", result) : string.Format("DC{0}", result * -1);
            }
        }

        /// <summary>
        /// Gets the name of the exception type.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public static string GetExceptionTypeName(this Exception ex)
        {
            if (ex == null)
            {
                return null;
            }
            if (ex.InnerException != null)
            {
                return ex.InnerException.GetExceptionTypeName();
            }
            return ex.GetType().Name;
        }

        /// <summary>
        /// Gets the inner hash code.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        private static long GetInnerHashCode(this Exception ex)
        {
            unchecked
            {
                long result = 17;
                result = result * 23 + ((ex.Message != null) ? ex.Message.GetHashCode() : 0);
                result = result * 23 + ex.GetType().FullName.GetHashCode();
                result = result * 23 + ((ex is WebEasException && ((WebEasException)ex).HasMessageUser) ? ((WebEasException)ex).MessageUser.GetHashCode() : 0);
                result = result * 23 + ((ex.InnerException != null) ? ex.InnerException.GetInnerHashCode() : 0);
                return result;
            }
        }

        /// <summary>
        /// Append inner error to string
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="sb"></param>
        /// <param name="level"></param>
        private static void AppendError(Exception ex, StringBuilder sb, int level)
        {
            string indent = BaseIndent;
            for (int i = 0; i < level; i++)
            {
                indent += "--";
            }

            indent += " ";

            sb.Append(indent);
            sb.Append(Environment.NewLine);

            sb.Append(indent);
            // sb.Append("!!! ");
            sb.Append(ex.Message);
            sb.Append(Environment.NewLine);

            Type type = ex.GetType();
            sb.Append(indent);
            sb.Append("Type: ");
            sb.Append(ex.GetType());
            sb.Append(Environment.NewLine);

            if (!String.IsNullOrEmpty(ex.Source))
            {
                sb.Append(indent);
                sb.Append("Source: ");
                sb.Append(ex.Source);
                sb.Append(Environment.NewLine);
            }

            if (ex.GetType() != typeof(Exception))
            {
                foreach (PropertyInfo info in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (info.DeclaringType != typeof(Exception))
                    {
                        if (info.PropertyType.IsClass && info.GetValue(ex, null) != null && info.PropertyType.IsSerializable)
                        {
                            try
                            {
                                sb.Append(indent);
                                sb.Append(info.Name);
                                sb.Append(": ");
                                sb.Append(WebEas.Core.Log.WebEasNLogExtensions.ToJsonString(info.GetValue(ex, null)));
                                sb.Append(Environment.NewLine);
                            }
                            catch
                            {
                                sb.Append(indent);
                                sb.Append(info.Name);
                                sb.Append(": ");
                                sb.Append(info.GetValue(ex, null));
                                sb.Append(Environment.NewLine);
                            }
                        }
                        else
                        {
                            sb.Append(indent);
                            sb.Append(info.Name);
                            sb.Append(": ");
                            sb.Append(info.GetValue(ex, null));
                            sb.Append(Environment.NewLine);
                        }
                    }
                }
            }

            if (!String.IsNullOrEmpty(ex.StackTrace))
            {
                sb.Append(indent);
                sb.Append("Stack: ");
                String stack = ex.StackTrace.Replace("   at", string.Format("{0}        at", indent));
                sb.Append(stack.Remove(0, string.Format("{0}        at", indent).Length));
                sb.Append(Environment.NewLine);
            }

            level++;
            if (ex.InnerException != null)
            {
                AppendError(ex.InnerException, sb, level);
            }
        }
    }
}