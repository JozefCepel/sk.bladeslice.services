using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using ServiceStack.Logging;

namespace WebEas.ServiceModel
{
    public class EgovLogger : ILog
    {
        private const string DEBUG = "DEBUG: ";
        private const string ERROR = "ERROR: ";
        private const string FATAL = "FATAL: ";
        private const string INFO = "INFO: ";
        private const string WARN = "WARN: ";

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugLogger"/> class.
        /// </summary>
        public EgovLogger(string type)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugLogger"/> class.
        /// </summary>
        public EgovLogger(Type type)
        {
        }

        public bool IsDebugEnabled { get; set; }

        public void Debug(object message, Exception exception)
        {
            Log(string.Format("{0}{1}", DEBUG, message), exception);
        }

        public void Debug(object message)
        {
            Log(string.Format("{0}{1}", DEBUG, message));
        }

        public void DebugFormat(string format, params object[] args)
        {
            LogFormat(string.Format("{0}{1}", DEBUG, format), args);
        }

        public void Error(object message, Exception exception)
        {
            Log(string.Format("{0}{1}", ERROR, message), exception);
        }

        public void Error(object message)
        {
            Log(string.Format("{0}{1}", ERROR, message));
        }

        public void ErrorFormat(string format, params object[] args)
        {
            LogFormat(string.Format("{0}{1}", ERROR, format), args);
        }

        public void Fatal(object message, Exception exception)
        {
            Log(string.Format("{0}{1}", FATAL, message), exception);
        }

        public void Fatal(object message)
        {
            Log(string.Format("{0}{1}", FATAL, message));
        }

        public void FatalFormat(string format, params object[] args)
        {
            LogFormat(string.Format("{0}{1}", FATAL, format), args);
        }

        public void Info(object message, Exception exception)
        {
            Log(string.Format("{0}{1}", INFO, message), exception);
        }

        public void Info(object message)
        {
            Log(string.Format("{0}{1}", INFO, message));
        }

        public void InfoFormat(string format, params object[] args)
        {
            LogFormat(string.Format("{0}{1}", INFO, format), args);
        }

        public void Warn(object message, Exception exception)
        {
            Log(string.Format("{0}{1}", WARN, message), exception);
        }

        public void Warn(object message)
        {
            Log(string.Format("{0}{1}", WARN, message));
        }

        public void WarnFormat(string format, params object[] args)
        {
            LogFormat(string.Format("{0}{1}", WARN, format), args);
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        private static void Log(object message, Exception exception)
        {
            string msg = message == null ? string.Empty : message.ToString();
            if (exception != null)
            {
                msg = String.Format("{0} \n {1}", msg, GetErrorDescription(exception));                
            }
            Trace.WriteLine(msg);
            //Console.WriteLine(msg);
        }

        /// <summary>
        /// Logs the format.
        /// </summary>
        private static void LogFormat(object message, params object[] args)
        {
            string msg = message == null ? string.Empty : message.ToString();
            Trace.WriteLine(String.Format(msg, args));            
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        private static void Log(object message)
        {
            string msg = message == null ? string.Empty : message.ToString();
            Trace.WriteLine(msg);
            //Console.WriteLine(msg);
        }

        /// <summary>
        /// Get parsed error description
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private static string GetErrorDescription(Exception ex)
        {
            if (ex == null)
            {
                return null;
            }

            StringBuilder sb = new StringBuilder();
            //  sb.Append(Environment.NewLine);
            //sb.Append("!!! ");
            sb.Append(ex.Message);
            sb.Append(Environment.NewLine);

            Type type = ex.GetType();
            sb.Append("Type: ");
            sb.Append(ex.GetType());
            sb.Append(Environment.NewLine);

            if (!String.IsNullOrEmpty(ex.Source))
            {
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
                        sb.Append(info.Name);
                        sb.Append(": ");
                        sb.Append(info.GetValue(ex, null));
                        sb.Append(Environment.NewLine);
                    }
                }
            }

            if (!String.IsNullOrEmpty(ex.StackTrace))
            {
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
        /// Append inner error to string
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="sb"></param>
        /// <param name="level"></param>
        private static void AppendError(Exception ex, StringBuilder sb, int level)
        {
            string indent = "";
            for (int i = 0; i < level; i++)
            {
                indent += "--";
            }

            indent += " ";

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
                        sb.Append(indent);
                        sb.Append(info.Name);
                        sb.Append(": ");
                        sb.Append(info.GetValue(ex, null));
                        sb.Append(Environment.NewLine);
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