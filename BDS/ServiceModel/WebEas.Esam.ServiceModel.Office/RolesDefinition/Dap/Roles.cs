using System.Collections.Generic;
using WebEas.Auth;

namespace WebEas.Esam.ServiceModel.Office.RolesDefinition.Dap
{
    public static class Roles
    {
        public readonly static List<Role> List;

        public const string DapMember = "DAP_MEMBER";
        public const string DapWriter = "DAP_WRITER";
        public const string DapCisWriter = "DAP_CIS_WRITER";
        public const string DapAdmin = "DAP_ADMIN";

        public readonly static Role RoleDapWriter;

        static Roles()
        {
            var member = new Role(DapMember, "ReadOnly");
            RoleDapWriter = new Role(DapWriter, "Write");
            var cisWriter = new Role(DapCisWriter, "Cis Write");
            var admin = new Role(DapAdmin, "Admin");

            // definovanie vazieb
            admin.SubRoles = new List<Role> { member, RoleDapWriter, cisWriter };
            cisWriter.SubRoles = new List<Role> { member, RoleDapWriter };
            RoleDapWriter.SubRoles = new List<Role> { member };

            // vytvorenie zoznamu
            List = new List<Role> { member, RoleDapWriter, cisWriter, admin };
        }
    }
}
