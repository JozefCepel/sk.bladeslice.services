using System.Collections.Generic;
using WebEas.Auth;

namespace WebEas.Esam.ServiceModel.Office.RolesDefinition.Crm
{
    public static class Roles
    {
        public readonly static List<Role> List;

        public const string CrmMember = "CRM_MEMBER";
        public const string CrmWriter = "CRM_WRITER";
        public const string CrmCisWriter = "CRM_CIS_WRITER";
        public const string CrmAdmin = "CRM_ADMIN";

        public readonly static Role RoleCrmWriter;

        static Roles()
        {
            var member = new Role(CrmMember, "ReadOnly");
            RoleCrmWriter = new Role(CrmWriter, "Write");
            var cisWriter = new Role(CrmCisWriter, "Cis Write");
            var admin = new Role(CrmAdmin, "Admin");

            // definovanie vazieb
            admin.SubRoles = new List<Role> { member, RoleCrmWriter, cisWriter };
            cisWriter.SubRoles = new List<Role> { member, RoleCrmWriter };
            RoleCrmWriter.SubRoles = new List<Role> { member };

            // vytvorenie zoznamu
            List = new List<Role> { member, RoleCrmWriter, cisWriter, admin };
        }
    }
}
