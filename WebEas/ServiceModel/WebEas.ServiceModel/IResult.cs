using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// 
    /// </summary>
    public interface IResult<T>
    {
        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        [DataMember]
        T Result { get; set; }
    }
}