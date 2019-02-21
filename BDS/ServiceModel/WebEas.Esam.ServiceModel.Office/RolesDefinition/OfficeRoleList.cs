using System.Collections.Generic;
using WebEas.Auth;

namespace WebEas.Esam.ServiceModel.Office.RolesDefinition
{
    public class OfficeRoleList : IRoleList
    {
        private static List<Role> roleList = null;

        public List<Role> RoleList
        {
            get
            { 
                if (roleList == null)
                {
                    var list = new List<Role>();
                    list.AddRange(Bds.Roles.List);
                    list.AddRange(Cfe.Roles.List);
                    list.AddRange(WebEas.ServiceModel.Office.Egov.Reg.Roles.List);

                    roleList = list;
                }
                return roleList;
            }
        }
    }
}