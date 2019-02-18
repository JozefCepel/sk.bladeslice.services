using System.Collections.Generic;
using WebEas.Auth;

namespace WebEas.Esam.ServiceModel.Office.Rzp
{
    public class ServiceModel :  IRoleList
    {
        /// <summary>
        /// Gets the role list.
        /// </summary>
        /// <value>The role list.</value>
        public List<Role> RoleList
        {
            get
            {
                return RolesDefinition.Rzp.Roles.List;
            }
        }
    }
}
