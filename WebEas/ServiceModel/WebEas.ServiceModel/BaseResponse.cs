using System;
using System.Linq;
using ServiceStack;
using ServiceStack.DataAnnotations;
using ServiceStack.Model;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// Base Response
    /// </summary>
    public abstract class BaseResponse : IHasId<int>
    {    
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public abstract int Id { get; set; }

        /// <summary>
        /// Gets or sets the response status.
        /// </summary>
        /// <value>The response status.</value>
        //[Ignore]
       // public ResponseStatus ResponseStatus { get; set; }
    }
}