using System;
using System.Linq;
using System.Web.Configuration;
using Ninject;
using Ninject.Web.Common;
using ServiceStack.Caching;
using ServiceStack.Configuration;
using ServiceStack.Redis;
using WebEas.Ninject;

namespace WebEas.Core.Ninject
{
    public class NinjectContainerAdapter : IContainerAdapter
    {
        private readonly IKernel kernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectContainerAdapter" /> class.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        public NinjectContainerAdapter(IKernel kernel)
        {
            this.kernel = kernel;
        }

        /// <summary>
        /// Gets the redis URI.
        /// </summary>
        /// <value>The redis URI.</value>
        private static string RedisUri
        {
            get
            {
                if (String.IsNullOrEmpty(WebConfigurationManager.AppSettings["RedisUri"]))
                {
                    return null;
                }
                return WebConfigurationManager.AppSettings["RedisUri"];
            }
        }

        /// <summary>
        /// Creates the kernel.
        /// </summary>
        /// <returns></returns>
        public static IKernel CreateKernel()
        {
            var kernel = new StandardKernel(
                new NinjectSettings
                {
                    AllowNullInjection = true
                });

            //System.Web.HttpContext.Current
            //kernel.Bind<IWebEasSessionProvider>().To<WebEasSessionProvider>().When(x => System.Web.HttpContext.Current != null).InSingletonScope();
            //kernel.Bind<IWebEasSessionProvider>().To<WebEasSessionProvider>().When(x => System.Web.HttpContext.Current == null).InThreadScope();

            if (!string.IsNullOrEmpty(RedisUri))
            {
                kernel.Bind<IRedisClientsManager>().ToMethod(ctx => new PooledRedisClientManager(RedisUri)).InSingletonScope();
                //TODO: rework Redis Clients
                /*var pooledRedisClientManager = new PooledRedisClientManager(RedisUri);
                kernel.Bind<ICacheClient>().ToMethod(ctx => pooledRedisClientManager.GetCacheClient()).InSingletonScope();
                kernel.Bind<IRedisClient>().ToMethod(ctx => pooledRedisClientManager.GetClient()).InSingletonScope();*/
            }
            else
            {
                kernel.Bind<IRedisClientsManager>().ToMethod(ctx => null).InSingletonScope();
                //kernel.Bind<ICacheClient>().To<MemoryCacheClient>();
            }

            NinjectServiceLocator.SetServiceLocator(kernel);

            return kernel;
        }

        /// <summary>
        /// Tries the resolve.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <returns></returns>
        public T TryResolve<T>()
        {
            return this.kernel.TryGet<T>();
        }

        /// <summary>
        /// Resolves this instance.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <returns></returns>
        public T Resolve<T>()
        {
            return this.kernel.Get<T>();
        }
    }
}