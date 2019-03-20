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
                    list.AddRange(Reg.Roles.List);
                    list.AddRange(Crm.Roles.List);
                    list.AddRange(Dap.Roles.List);
                    list.AddRange(Fin.Roles.List);
                    list.AddRange(Osa.Roles.List);
                    list.AddRange(Uct.Roles.List);

                    roleList = list;
                }
                return roleList;
            }
        }
    }
}