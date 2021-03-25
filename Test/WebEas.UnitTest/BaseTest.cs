using Funq;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.SqlServer;
using ServiceStack.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebEas.Core.Ninject;
using WebEas.Esam.ServiceInterface.Office.Dap;
using WebEas.Esam.ServiceInterface.Office.Osa;
using WebEas.Esam.ServiceInterface.Office.Rzp;
using WebEas.Esam.ServiceModel.Office;

namespace WebEas.UnitTest
{
    public class BaseTest
    {
        public BaseTest()
        {
            Licensing.RegisterLicense(@"17741-e1JlZjoxNzc0MSxOYW1lOiJEQVRBTEFOLCBhLnMuIixUeXBlOkJ1c2luZXNzLE1ldGE6MCxIYXNoOlhBeTIxZi9GZUtGR1ZGRk1pTmlabXZVazhuc1lDLzA5ZGdlbGVJS0VjQUw1OVdEWnY0MnN2OFJVSGtsdXZ2ODJ6aUdDdFBKVE1DTUZObHBEY2huZzV2RmNLejFMcnJvbzVBQkJXZGwyWVdoeDZvVTRyZm9Jc2pKRnp1dlF1UCtuaCtZeGNmUkE5LzZsZFVrUWQvckR2dkhIQkpuelIxUjFPV1krM2YwUVc1bz0sRXhwaXJ5OjIwMjItMDMtMjR9");
        }

        public EsamSession CreateLitavaUserSession()
        {

            // TESTOVACI TENANT URCENY IBA NA TESTOVANIE SYNCHRONIZACII
            return new EsamSession
            {
                TenantId = "5cfb1741-315a-40b2-b675-082e2ae003e4",
                AdminLevel = AdminLevel.SysAdmin,
                D_Tenant_Id_Externe = Guid.Parse("4099558d-3d12-4d50-a101-489c7e7b1317"),
                OrsPermissions = "FFFFF",
                UserAuthName = "litava",
                FirstName = "Tester",
                LastName = "Litava",
                DisplayName = "Litava Tester",
                Email = "litava@litava.sk",
                FullName = "Litava Tester",
                Roles = new List<string>
                {
                    "CFE_ADMIN",
                    "CFE_MEMBER",
                    "CFE_SYS_ADMIN",
                    "OSA_ADMIN",
                    "REG_ADMIN",
                    "DAP_ADMIN",
                    "RZP_ADMIN",
                },
                IsAuthenticated = true,
                AuthProvider = "credentials"
            };
        }

        protected BasicAppHost GetBasicAppHost(string connectionString, string modul, EsamSession session = null, List<Assembly> assemblies = null)
        {
#if ITP
            connectionString = connectionString.Replace("sd1esamdb31.datalan.sk\\SQL2017", "st1esamdb31.datalan.sk\\SQL2019");
#endif
            var appHost = new BasicAppHost
            {
                TestMode = true,
                ServiceName = "EsamUnitTest",
                ConfigureContainer = container =>
                {
                    container.Adapter = new NinjectContainerAdapter(NinjectContainerAdapter.CreateKernel());
                    container.Register<IAuthSession>(c => session ?? CreateLitavaUserSession());
                },
                ConfigureAppHost = host => {
                    //host.Plugins.Add(new CancellableRequestsFeature());
                },
            };
            appHost.Init();

            if (assemblies != null)
            {
                foreach (var assembly in assemblies)
                {
                    foreach (var service in assembly.GetTypes().Where(x => x.HasInterface(typeof(Core.IWebEasCoreServiceBase)) && x?.BaseType?.Name == "ServiceBase"))
                    {
                        appHost.Container.AddTransient(service);
                    }

                    foreach (var repository in assembly.GetTypes().Where(x => x.HasInterface(typeof(IRepositoryBase)) && x?.BaseType?.Name == "RepositoryBase"))
                    {
                        var intrface = repository.GetInterfaces().Where(x => x.HasInterface(typeof(IRepositoryBase))).First();
                        appHost.Container.RegisterAutoWiredType(repository, intrface, ReuseScope.Request);
                    }
                }
            }
            else
            {
                if (modul == "osa")
                {
                    appHost.Container.AddTransient<OsaService>();
                    appHost.Container.RegisterAutoWiredType(typeof(OsaRepository), typeof(IOsaRepository), ReuseScope.Request);
                }

                if (modul == "dap")
                {
                    appHost.Container.AddTransient<DapService>();
                    appHost.Container.RegisterAutoWiredType(typeof(DapRepository), typeof(IDapRepository), ReuseScope.Request);
                }

                if (modul == "rzp")
                {
                    appHost.Container.AddTransient<RzpService>();
                    appHost.Container.RegisterAutoWiredType(typeof(RzpRepository), typeof(IRzpRepository), ReuseScope.Request);
                }
            }

            var provider = new SqlServerOrmLiteDialectProvider();

            var olcf = new OrmLiteConnectionFactory(connectionString, provider);
            appHost.Container.Register<IDbConnectionFactory>(c => olcf);

            ServiceStack.Logging.LogManager.LogFactory = new WebEas.Log.WebEasNLogFactory();
            WebEas.Log.WebEasNLogConfig.SetConfig(null, console: true, database: false);
            return appHost;
        }
    }
}
