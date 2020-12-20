using System;
using System.Collections.Generic;
using System.Linq;
using WebEas.Auth;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// 
    /// </summary>
    public interface IServiceModel: IRoleList
    {
        /// <summary>
        /// Gets the XSD types.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, Type> GetXsdTypes();

        /// <summary>
        /// Adds the XSD types.
        /// </summary>
        /// <param name="collection">The collection.</param>
        Dictionary<string, Type> AddXsdTypes(Dictionary<string, Type> collection);
    }
}