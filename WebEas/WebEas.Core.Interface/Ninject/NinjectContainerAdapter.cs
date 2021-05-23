using System;
using System.Linq;
using System.Web.Configuration;
using Ninject;
using Ninject.Web.Common;
using ServiceStack;
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
                if (RedisUri.Contains(","))
                {
                    kernel.Bind<IRedisClientsManager>().ToMethod(ctx => new RedisSentinel(RedisUri.Split(','))
                    {
                        RedisManagerFactory = (master, slaves) => new RedisManagerPool(master),
#if PROD
                        HostFilter = host => "Der3dsTR6_56ff@{0}".Fmt(host)
#endif
                    }.Start()).InSingletonScope();
                    //kernel.Bind<ICacheClient>().ToMethod(c => c.Kernel.Get<IRedisClientsManager>().GetCacheClient()).InSingletonScope();
                    //kernel.Bind<IRedisClient>().ToMethod(c => c.Kernel.Get<IRedisClientsManager>().GetClient()).InSingletonScope();
                }
                else
                {
                    //kernel.Bind<IRedisClientsManager>().ToMethod(ctx => new RedisManagerPool(RedisUri, new RedisPoolConfig { MaxPoolSize = 10000}) ).InSingletonScope();
                    //kernel.Bind<RedisManagerPool, IRedisClientsManager>().ToMethod(ctx => new RedisManagerPool(RedisUri, new RedisPoolConfig { MaxPoolSize = 10000 })).InSingletonScope();
                    //kernel.Bind<IRedisClientsManager>().ToMethod(c => new RedisManagerPool(RedisUri, new RedisPoolConfig { MaxPoolSize = 10000 })).InSingletonScope();
                    //kernel.Bind<IRedisClientsManager>().ToConstant(new RedisManagerPool(RedisUri, new RedisPoolConfig { MaxPoolSize = 10000 }));
                    //kernel.Bind<ICacheClient>().ToMethod(c => c.Kernel.Get<IRedisClientsManager>().GetCacheClient()).InSingletonScope();
                    //kernel.Bind<IRedisClient>().ToMethod(c => c.Kernel.Get<IRedisClientsManager>().GetClient()).InSingletonScope();

                    kernel.Bind<IRedisClientsManager>().ToMethod(c => new RedisManagerPool(RedisUri)).InSingletonScope();
                    //kernel.Bind<ICacheClient>().ToMethod(c => c.Kernel.Get<IRedisClientsManager>().GetCacheClient()).InSingletonScope();
                    //kernel.Bind<IRedisClient>().ToMethod(c => c.Kernel.Get<IRedisClientsManager>().GetClient()).InSingletonScope();
                }

                kernel.Bind<IServerEvents>().ToMethod(c => new RedisServerEvents(c.Kernel.Get<IRedisClientsManager>())).InSingletonScope();
                kernel.Get<IServerEvents>().Start();
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