using System.Collections.Generic;
using WebEas.Auth;

namespace WebEas.Esam.ServiceModel.Office.RolesDefinition.Rzp
{
    public static class Roles
    {
        public readonly static List<Role> List;

        public const string RzpMember = "RZP_MEMBER";
        public const string RzpWriter = "RZP_WRITER";
        public const string RzpCisWriter = "RZP_CIS_WRITER";
        public const string RzpAdmin = "RZP_ADMIN";

        static Roles()
        {
            var member = new Role(RzpMember, "ReadOnly");
            var writer = new Role(RzpWriter, "Write");
            var cisWriter = new Role(RzpCisWriter, "Cis Write");
            var admin = new Role(RzpAdmin, "Admin");

            // definovanie vazieb
            admin.SubRoles = new List<Role> { member, writer, cisWriter };
            cisWriter.SubRoles = new List<Role> { member };
            writer.SubRoles = new List<Role> { member, Reg.Roles.RoleRegWriter};

            // vytvorenie zoznamu
            List = new List<Role> { member, writer, cisWriter, admin };
        }
    }
}