using System;
using System.Linq;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// 
    /// </summary>
    public interface IChangeIdentifier
    {
        /// <summary>
        /// Gets or sets the change identifier dto.
        /// </summary>
        /// <value>The change identifier dto.</value>
        long? ChangeIdentifierDto { get; set; }
    }
}