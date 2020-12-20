using System;
using System.Configuration;
using System.Linq;

namespace WebEas.Config
{
    /// <summary>
    /// 
    /// </summary>
    public class WebEasLogElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the log console.
        /// </summary>
        /// <value>The log console.</value>
        [ConfigurationProperty("console", DefaultValue = "true", IsRequired = false)]                
        public bool LogConsole
        {
            get
            {
                return (bool)this["console"];
            }
            set
            {
                this["console"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the log file.
        /// </summary>
        /// <value>The log file.</value>
        [ConfigurationProperty("file", DefaultValue = "true", IsRequired = false)]
        public bool LogFile
        {
            get
            {
                return (bool)this["file"];
            }
            set
            {
                this["file"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the log database.
        /// </summary>
        /// <value>The log database.</value>
        [ConfigurationProperty("database", DefaultValue = "true", IsRequired = false)]
        public bool LogDatabase
        {
            get
            {
                return (bool)this["database"];
            }
            set
            {
                this["database"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the console level.
        /// </summary>
        /// <value>The console level.</value>
        [ConfigurationProperty("consoleLevel", DefaultValue = "Trace", IsRequired = false)]
        public string ConsoleLevel
        {
            get
            {
                return (string)this["consoleLevel"];
            }
            set
            {
                this["consoleLevel"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the file level.
        /// </summary>
        /// <value>The file level.</value>
        [ConfigurationProperty("fileLevel", DefaultValue = "Error", IsRequired = false)]
        public string FileLevel
        {
            get
            {
                return (string)this["fileLevel"];
            }
            set
            {
                this["fileLevel"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the database level.
        /// </summary>
        /// <value>The database level.</value>
        [ConfigurationProperty("databaseLevel", DefaultValue = "Error", IsRequired = false)]
        public string DatabaseLevel
        {
            get
            {
                return (string)this["databaseLevel"];
            }
            set
            {
                this["databaseLevel"] = value;
            }
        }
    }
}