using System.Collections.Generic;
using WebEas.Auth;

namespace WebEas.Esam.ServiceModel.Office.RolesDefinition.Bds
{
    public static class Roles
    {
        public readonly static List<Role> List;

        public const string BdsMember = "BDS_MEMBER";
        public const string BdsWriter = "BDS_WRITER";
        public const string BdsCisWriter = "BDS_CIS_WRITER";
        public const string BdsAdmin = "BDS_ADMIN";

        static Roles()
        {
            var member = new Role(BdsMember, "ReadOnly");
            var writer = new Role(BdsWriter, "Write");
            var cisWriter = new Role(BdsCisWriter, "Cis Write");
            var admin = new Role(BdsAdmin, "Admin");

            // definovanie vazieb
            admin.SubRoles = new List<Role> { member, writer, cisWriter };
            cisWriter.SubRoles = new List<Role> { member };
            writer.SubRoles = new List<Role> { member };

            // vytvorenie zoznamu
            List = new List<Role> { member, writer, cisWriter, admin };
        }
    }
}