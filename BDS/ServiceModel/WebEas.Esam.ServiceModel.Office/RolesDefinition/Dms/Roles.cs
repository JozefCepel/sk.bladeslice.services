using System.Collections.Generic;
using WebEas.Auth;

namespace WebEas.Esam.ServiceModel.Office.RolesDefinition.Dms
{
    public static class Roles
    {
        public readonly static List<Role> List;

        public const string DmsMember = "DMS_MEMBER";
        public const string DmsWriter = "DMS_WRITER";
        public const string DmsCisWriter = "DMS_CIS_WRITER";
        public const string DmsAdmin = "DMS_ADMIN";

        static Roles()
        {
            var member = new Role(DmsMember, "ReadOnly");
            var writer = new Role(DmsWriter, "Write");
            var cisWriter = new Role(DmsCisWriter, "Cis Write");
            var admin = new Role(DmsAdmin, "Admin");

            // definovanie vazieb
            admin.SubRoles = new List<Role> { member, writer, cisWriter };
            cisWriter.SubRoles = new List<Role> { member };
            writer.SubRoles = new List<Role> { member, Reg.Roles.RoleRegWriter };

            // vytvorenie zoznamu
            List = new List<Role> { member, writer, cisWriter, admin };
        }
    }
}