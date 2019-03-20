using System.Collections.Generic;
using WebEas.Auth;

namespace WebEas.Esam.ServiceModel.Office.RolesDefinition.Fin
{
    public static class Roles
    {
        public readonly static List<Role> List;

        public const string FinMember = "FIN_MEMBER";
        public const string FinWriter = "FIN_WRITER";
        public const string FinCisWriter = "FIN_CIS_WRITER";
        public const string FinAdmin = "FIN_ADMIN";

        public readonly static Role RoleFinWriter;

        static Roles()
        {
            var member = new Role(FinMember, "ReadOnly");
            RoleFinWriter = new Role(FinWriter, "Write");
            var cisWriter = new Role(FinCisWriter, "Cis Write");
            var admin = new Role(FinAdmin, "Admin");

            // definovanie vazieb
            admin.SubRoles = new List<Role> { member, RoleFinWriter, cisWriter };
            cisWriter.SubRoles = new List<Role> { member, RoleFinWriter };
            RoleFinWriter.SubRoles = new List<Role> { member };

            // vytvorenie zoznamu
            List = new List<Role> { member, RoleFinWriter, cisWriter, admin };
        }
    }
}
