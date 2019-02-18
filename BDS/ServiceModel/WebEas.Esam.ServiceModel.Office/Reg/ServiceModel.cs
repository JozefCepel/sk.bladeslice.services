using System.Collections.Generic;
using WebEas.Auth;

namespace WebEas.Esam.ServiceModel.Office.Reg
{
    public class ServiceModel : IRoleList
    {
        /// <summary>
        /// Gets the role list.
        /// </summary>
        /// <value>The role list.</value>
        public List<Role> RoleList => WebEas.ServiceModel.Office.Egov.Reg.Roles.List;
    }
}
