using System;
using System.Collections.Generic;
using System.Linq;

namespace WebEas.Auth
{
    /// <summary>
    /// List of roles 
    /// </summary>
    public interface IRoleList
    {
        /// <summary>
        /// Gets the role list.
        /// </summary>
        /// <value>The role list.</value>
        List<Role> RoleList { get; }
    }
}