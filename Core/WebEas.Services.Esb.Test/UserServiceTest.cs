using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebEas.Services.Esb.Test
{
    [TestClass]
    public class UserServiceTest : TestBaseProxy
    {
        [TestMethod]
        public void GetUser()
        {
            using (WebEas.Services.Esb.IamUserProxy proxy = new WebEas.Services.Esb.IamUserProxy(null))
            {
                var user = proxy.GetUser("1015e14d-2e03-486b-9a96-c5bc1e21aefa");
                Assert.IsFalse(user == null, "Nenajdeny user");
            }
        }
    }
}
