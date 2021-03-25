using System;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace WebEas.Context
{
    /// <summary>
    /// 
    /// </summary>
    public static class Info
    {
        /// <summary>
        /// Gets the application version.
        /// </summary>
        /// <value>The application version.</value>
        public static string ApplicationVersion
        {
            get
            {
                //return EntryAssembly.GetName().Version.ToString();
                return EntryAssembly.GetName().Version.Revision.ToString();
            }
        }

        /// <summary>
        /// Gets the updated.
        /// </summary>
        /// <value>The updated.</value>
        public static DateTime Updated
        {
            get
            {
                return new System.IO.FileInfo(EntryAssembly.Location).LastWriteTime;                
            }
        }

        /// <summary>
        /// Gets the database environment.
        /// </summary>
        /// <value>The database environment.</value>
        [Obsolete]
        public static string DatabaseEnvironment
        {
            get
            {
                foreach (var connectionstring in ConfigurationManager.ConnectionStrings)
                {
                    if (connectionstring is ConnectionStringSettings)
                    {
                        var conn = (ConnectionStringSettings)connectionstring;
                        if (conn.ConnectionString.Contains("APPDATASB01"))
                        {
                            return "Develop";
                        }

                        if (conn.ConnectionString.Contains("LBDBCL12.INTRA.DCOM.SK"))
                        {
                            if (conn.ConnectionString.Contains("password=65$re"))
                            {
                                return "Integration";
                            }
                            return "Test";
                        }
                    }
                }

                return "Unknown";
            }
        }

        /// <summary>
        /// Gets the entry assembly.
        /// </summary>
        /// <value>The entry assembly.</value>
        private static Assembly EntryAssembly
        {
            get
            {
                var ass = GetWebEntryAssembly();
                if (ass == null)
                {
                    return Assembly.GetExecutingAssembly();
                }
                else
                {
                    return ass;
                }
            }
        }

        /// <summary>
        /// Gets the web entry assembly.
        /// </summary>
        /// <returns></returns>
        private static Assembly GetWebEntryAssembly()
        {
            if (System.Web.HttpContext.Current == null ||
                System.Web.HttpContext.Current.ApplicationInstance == null)
            {
                return null;
            }

            var type = System.Web.HttpContext.Current.ApplicationInstance.GetType();
            while (type != null && type.Namespace == "ASP")
            {
                type = type.BaseType;
            }

            return type == null ? null : type.Assembly;
        }
    }
}