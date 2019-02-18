using System;
using System.Collections.Generic;
using System.Linq;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// 
    /// </summary>
    public interface IListDto
    {
        /// <summary>
        /// Gets or sets the kod polozky.
        /// </summary>
        /// <value>The kod polozky.</value>
        string KodPolozky { get; set; }

        /// <summary>
        /// Gets or sets the page.
        /// </summary>
        /// <value>The page.</value>
        int Page { get; set; }

        /// <summary>
        /// Gets or sets the start.
        /// </summary>
        /// <value>The start.</value>
        int Start { get; set; }

        /// <summary>
        /// Gets or sets the limit.
        /// </summary>
        /// <value>The limit.</value>
        int Limit { get; set; }

        /// <summary>
        /// Gets or sets the filter.
        /// </summary>
        /// <value>The filter.</value>
        string Filter { get; set; }

        /// <summary>
        /// Gets or sets the additional attributes.
        /// </summary>
        /// <value>The additional attributes.</value>
        Dictionary<string, string> AdditionalAttributes { get; set; }
    }
}