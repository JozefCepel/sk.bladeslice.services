using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using ServiceStack;
using ServiceStack.Caching;
using ServiceStack.Data;
using ServiceStack.MiniProfiler;
using ServiceStack.MiniProfiler.Data;
using ServiceStack.OrmLite;
using ServiceStack.Testing;
using WebEas;
using WebEas.Core;
using WebEas.Core.Log;
using WebEas.Core.Ninject;
using WebEas.Log;
using WebEas.Ninject;

namespace WebEas.Services.Esb.Test
{
    public abstract class TestBaseProxy
    {
        protected ServiceStackHost appHost;

        /// <summary>
        /// Tests the fixture set up.
        /// </summary>
        [TestInitialize]
        public void TestFixtureSetUp()
        {
            WebEas.Context.Current.RegisterServiceStackLicence();
            WebEasNLogConfig.SetConfig("", true, true, false);
            this.appHost = new BasicAppHost().Init();
            var kernel = new StandardKernel();

            //string connectionString = @"server = esten05; database = Egov; uid = esten05; password = Esten05;";
            //string connectionString = @"server=.;database=egov;Integrated Security=True;Pooling=False";
            //string connectionString = @"server=10.231.79.11\APPDATASB01,60703;database=Egov;uid=Egov_main;password=TempHeslo001;Max Pool Size=1000;Connect Timeout=120";
            string connectionString = @"server=10.231.115.11\APPDATA01,62905;database=Egov;uid=Egov_main;password=utTRe#r132;Max Pool Size=1000;Connect Timeout=120";

            kernel.Bind<ICacheClient>().To<MemoryCacheClient>();
            kernel.Bind<IDbConnectionFactory>().ToMethod<OrmLiteConnectionFactory>(c => new OrmLiteConnectionFactory(connectionString, SqlServerDialect.Provider)
            {
                ConnectionFilter = x => new ProfiledDbConnection(x, Profiler.Current)
            });
            //kernel.Bind<ISecurityRepository>().To<SecurityRepository>();
            kernel.Bind<IWebEasSessionProvider>().To<WebEasSessionProvider>();

           
            NinjectServiceLocator.SetServiceLocator(kernel);

            this.appHost.Container.Adapter = new NinjectContainerAdapter(kernel);
            var container = this.appHost.Container;

            container.Register<IDbConnectionFactory>(new OrmLiteConnectionFactory(connectionString, SqlServerDialect.Provider));          
        }

        /// <summary>
        /// Tests the fixture tear down.
        /// </summary>
        [TestCleanup]
        public void TestFixtureTearDown()
        {
            this.appHost.Dispose();
        }
    }
}
