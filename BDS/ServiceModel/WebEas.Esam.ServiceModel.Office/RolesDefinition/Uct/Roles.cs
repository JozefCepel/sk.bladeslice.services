using System.Collections.Generic;
using WebEas.Auth;

namespace WebEas.Esam.ServiceModel.Office.RolesDefinition.Uct
{
    public static class Roles
    {
        public readonly static List<Role> List;

        public const string UctMember = "UCT_MEMBER";
        public const string UctWriter = "UCT_WRITER";
        public const string UctCisWriter = "UCT_CIS_WRITER";
        public const string UctAdmin = "UCT_ADMIN";

        public readonly static Role RoleUctWriter;

        static Roles()
        {
            var member = new Role(UctMember, "ReadOnly");
            RoleUctWriter = new Role(UctWriter, "Write");
            var cisWriter = new Role(UctCisWriter, "Cis Write");
            var admin = new Role(UctAdmin, "Admin");

            // definovanie vazieb
            admin.SubRoles = new List<Role> { member, RoleUctWriter, cisWriter };
            cisWriter.SubRoles = new List<Role> { member, RoleUctWriter };
            RoleUctWriter.SubRoles = new List<Role> { member };

            // vytvorenie zoznamu
            List = new List<Role> { member, RoleUctWriter, cisWriter, admin };
        }
    }
}
