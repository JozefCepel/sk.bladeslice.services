using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebEas.Services.Esb.Test
{
    [TestClass]
    public class RoleServiceTest : TestBaseProxy
    {
        [TestMethod]
        public void GetCertificate()
        {
            var cert = WebEas.Core.Sts.SecurityTokenServiceHelper.GetCertificate("28DBC3F303908DA7D6055CE6201B17223327E146");

            string val = cert.ToString();
        }

        [TestMethod]
        public void GetUserTest()
        {
            using (WebEas.Services.Esb.IamUserProxy proxy = new IamUserProxy(null))
            {
                var response = proxy.GetUser("1015E14D-2E03-486B-9A96-C5BC1E21AEFA");
                Trace.WriteLine(response.name);
            }
        }


        [TestMethod]
        public void GetUserRolesTest()
        {
            using (WebEas.Services.Esb.IamRoleProxy proxy = new WebEas.Services.Esb.IamRoleProxy(null))
            {
                var roles = proxy.GetUserRoles("1015E14D-2E03-486B-9A96-C5BC1E21AEFA".ToUpper(), "9FF9C399-6FF3-4EFD-891A-7FFAFC5C02B4".ToUpper());
                Assert.IsFalse(roles == null || roles.Length == 0, "Nenajdena rola");
            }
        }

        [TestMethod]
        public void GetRoleUsersTest()
        {
            using (WebEas.Services.Esb.IamRoleProxy proxy = new WebEas.Services.Esb.IamRoleProxy(null))
            {
                var users = proxy.GetRoleUsers("EDM_WRITER", "a0f99009-241d-4e9a-85b1-3bac94f9b4d4");
                Assert.IsFalse(users == null || users.Length == 0, "Nenajdena rola");
            }
        }

        [TestMethod]
        public void IsInRoleTest()
        {
            using (WebEas.Services.Esb.IamRoleProxy proxy = new WebEas.Services.Esb.IamRoleProxy(null))
            {
                proxy.IsUserInRole("87358b5c-bfcb-42b0-8b86-c0d9aba9dc69", "EDM_WRITER", "a0f99009-241d-4e9a-85b1-3bac94f9b4d4");
            }
        }

        [TestMethod]
        public void GetUnionRolesForMunicipalityTest()
        {
            using (WebEas.Services.Esb.IamRoleProxy proxy = new WebEas.Services.Esb.IamRoleProxy(null))
            {
                proxy.GetUnionRolesForMunicipality("87358b5c-bfcb-42b0-8b86-c0d9aba9dc69");
            }
        }

        [TestMethod]
        public void GetUserTenantsTest()
        {
            using (WebEas.Services.Esb.IamRoleProxy proxy = new WebEas.Services.Esb.IamRoleProxy(null))
            {
                var tenants = proxy.GetUserTenants("7bdc9985-a81b-4c4d-8402-00b3d5551cd9");
                Trace.WriteLine(tenants.Length);
            }
        }
    }
}