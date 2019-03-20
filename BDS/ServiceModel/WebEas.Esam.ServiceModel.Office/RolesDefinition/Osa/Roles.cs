using System.Collections.Generic;
using WebEas.Auth;

namespace WebEas.Esam.ServiceModel.Office.RolesDefinition.Osa
{
    public static class Roles
    {
        public readonly static List<Role> List;

        public const string OsaMember = "OSA_MEMBER";
        public const string OsaWriter = "OSA_WRITER";
        public const string OsaCisWriter = "OSA_CIS_WRITER";
        public const string OsaAdmin = "OSA_ADMIN";

        public readonly static Role RoleOsaWriter;

        static Roles()
        {
            var member = new Role(OsaMember, "ReadOnly");
            RoleOsaWriter = new Role(OsaWriter, "Write");
            var cisWriter = new Role(OsaCisWriter, "Cis Write");
            var admin = new Role(OsaAdmin, "Admin");

            // definovanie vazieb
            admin.SubRoles = new List<Role> { member, RoleOsaWriter, cisWriter };
            cisWriter.SubRoles = new List<Role> { member, RoleOsaWriter };
            RoleOsaWriter.SubRoles = new List<Role> { member };

            // vytvorenie zoznamu
            List = new List<Role> { member, RoleOsaWriter, cisWriter, admin };
        }
    }
}
