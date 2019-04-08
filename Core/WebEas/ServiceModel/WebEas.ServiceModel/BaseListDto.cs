using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class BaseListDto : IListDto
    {
        /// <summary>
        /// Gets or sets the kod polozky.
        /// </summary>
        /// <value>The kod polozky.</value>
        [ApiMember(Name = "KodPolozky", Description = "Kód položky", DataType = "string", IsRequired = true)]
        [DataMember]
        public string KodPolozky { get; set; }

        [ApiMember(Name = "Code", Description = "Kód položky", DataType = "string", IsRequired = true)]
        [DataMember]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the page.
        /// </summary>
        /// <value>The page.</value>
        [DataMember(Name = "page")]
        public int Page { get; set; }

        /// <summary>
        /// Gets or sets the start.
        /// </summary>
        /// <value>The start.</value>
        [DataMember(Name = "start")]
        public int Start { get; set; }

        /// <summary>
        /// Gets or sets the limit.
        /// </summary>
        /// <value>The limit.</value>
        [DataMember(Name = "limit")]
        public int Limit { get; set; }

        /// <summary>
        /// Gets or sets the filters.
        /// </summary>
        /// <value>The filters.</value>
        [DataMember(Name = "filters")]
        public string Filter { get; set; }

        /// <summary>
        /// Gets or sets the sort.
        /// </summary>
        /// <value>The sort.</value>
        [DataMember(Name = "sort")]
        public string Sort { get; set; }

        /// <summary>
        /// Gets or sets the group.
        /// </summary>
        /// <value>The group.</value>
        [DataMember(Name = "Mat. group")]
        public string Group { get; set; }

        /// <summary>
        /// Gets or sets which columns to fetch with data.
        /// </summary>
        /// <value>The sort.</value>
        [DataMember(Name = "colwdat")]
        public string ColumnsWithData { get; set; }

        /// <summary>
        /// Gets or sets the additional attributes.
        /// </summary>
        /// <value>The additional attributes.</value>
        public Dictionary<string, string> AdditionalAttributes { get; set; }
    }
}