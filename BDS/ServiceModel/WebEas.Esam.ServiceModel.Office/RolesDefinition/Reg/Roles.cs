using System.Collections.Generic;
using WebEas.Auth;

namespace WebEas.Esam.ServiceModel.Office.RolesDefinition.Reg
{
    public static class Roles
    {
        public readonly static List<Role> List;

        public const string RegMember = "REG_MEMBER";
        public const string RegWriter = "REG_WRITER";
        public const string RegCisWriter = "REG_CIS_WRITER";
        public const string RegAdmin = "REG_ADMIN";

        public readonly static Role RoleRegWriter;

        static Roles()
        {
            var member = new Role(RegMember, "ReadOnly");
            RoleRegWriter = new Role(RegWriter, "Write");
            var cisWriter = new Role(RegCisWriter, "Cis Write");
            var admin = new Role(RegAdmin, "Admin");

            // definovanie vazieb
            admin.SubRoles = new List<Role> { member, RoleRegWriter, cisWriter };
            cisWriter.SubRoles = new List<Role> { member, RoleRegWriter };
            RoleRegWriter.SubRoles = new List<Role> { member };

            // vytvorenie zoznamu
            List = new List<Role> { member, RoleRegWriter, cisWriter, admin };
        }
    }
}