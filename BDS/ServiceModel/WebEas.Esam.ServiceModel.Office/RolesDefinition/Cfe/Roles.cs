using System.Collections.Generic;
using WebEas.Auth;

namespace WebEas.Esam.ServiceModel.Office.RolesDefinition.Cfe
{
    public static class Roles
    {
        public readonly static List<Role> List;

        public const string CfeMember = "CFE_MEMBER";
        public const string CfeWriter = "CFE_WRITER";
        public const string CfeCisWriter = "CFE_CIS_WRITER";
        public const string CfeAdmin = "CFE_ADMIN";

        static Roles()
        {
            var member = new Role(CfeMember, "ReadOnly");
            var writer = new Role(CfeWriter, "Write");
            var cisWriter = new Role(CfeCisWriter, "Cis Write");
            var admin = new Role(CfeAdmin, "Admin");

            // definovanie vazieb
            admin.SubRoles = new List<Role> { member, writer, cisWriter };
            cisWriter.SubRoles = new List<Role> { member };
            writer.SubRoles = new List<Role> { member, Reg.Roles.RoleRegWriter };

            // vytvorenie zoznamu
            List = new List<Role> { member, writer, cisWriter, admin };
        }
    }
}