using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.AppStatus
{
    /// <summary>
    /// 
    /// </summary>
    public enum IssueType
    {
        warn,
        error
    }

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class Monitoring : HealthCheck
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Monitoring" /> class.
        /// </summary>
        public Monitoring()
        {
            this.Issues = new List<Issue>();
        }

        /// <summary>
        /// Gets or sets the issues.
        /// </summary>
        /// <value>The issues.</value>
        [DataMember(Name = "issues", Order = 4)]
        public List<Issue> Issues { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class Issue
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [DataMember(Name = "type")]
        public IssueType Type { get; set; }

        /// <summary>
        /// Gets or sets the desc.
        /// </summary>
        /// <value>The desc.</value>
        [DataMember(Name = "desc")]
        public string Desc { get; set; }
    }
}