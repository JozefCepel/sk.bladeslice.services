using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.AppStatus
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class Full : Monitoring
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Full" /> class.
        /// </summary>
        public Full()
        {
            this.Infos = new List<Info>();
        }

        /// <summary>
        /// Gets or sets the infos.
        /// </summary>
        /// <value>The infos.</value>
        [DataMember(Name = "infos", Order = 5)]
        public List<Info> Infos { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class Info
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        [DataMember(Name = "value")]
        public string Value { get; set; }
    }
}