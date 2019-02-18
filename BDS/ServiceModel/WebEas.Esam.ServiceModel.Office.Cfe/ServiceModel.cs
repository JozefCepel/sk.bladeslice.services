using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebEas.Auth;

namespace WebEas.Esam.ServiceModel.Office.Cfe
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
                return RolesDefinition.Cfe.Roles.List;
            }
        }
    }
}
