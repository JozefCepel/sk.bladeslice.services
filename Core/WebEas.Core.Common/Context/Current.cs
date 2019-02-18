using System;
using System.Linq;
using System.ServiceModel;
using Ninject;
using ServiceStack;
using ServiceStack.Caching;
using WebEas.Core;

namespace WebEas.Context
{
    /// <summary>
    /// Current Context
    /// </summary>
    public static class Current
    {
        public static MemoryCacheClient LocalMachineCache = new MemoryCacheClient();
        private static EndpointType currentEndpoint = EndpointType.Unknown;
        private static bool isBackgroundProcess = false;

        //   private static RemoveFromCacheDelegate delRemoveFromCache;
        private delegate bool RemoveFromCacheDelegate(string key);

        private delegate bool SetToCacheDelegate<T>(string key, T data, TimeSpan expireCacheIn);

        /// <summary>
        /// Gets or sets the current endpoint.
        /// </summary>
        /// <value>The current endpoint.</value>
        public static EndpointType CurrentEndpoint
        {
            get
            {
                return currentEndpoint;
            }
            set
            {
                currentEndpoint = value;
            }
        }

        /// <summary>
        /// Gets or sets the is background process. (Zatial len zvysenie timeout pre dlhe operacie)
        /// </summary>
        /// <value>The is background process.</value>
        public static bool IsBackgroundProcess
        {
            get
            {
                return isBackgroundProcess || !HasHttpContext;
            }
            set
            {
                isBackgroundProcess = value;
            }
        }

        /// <summary>
        /// Gets the session.
        /// </summary>
        /// <value>The session.</value>
        [Obsolete("Použiť namiesto toho Repository.Session - performance issue")]
        public static IWebEasSession Session
        {
            get
            {
                try
                {
                    IWebEasSessionProvider sessionProvider = Ninject.NinjectServiceLocator.Kernel.Get<IWebEasSessionProvider>();
                    return sessionProvider.GetSession();
                }
                catch (Exception ex)
                {
                    if (Ninject.NinjectServiceLocator.Kernel == null)
                    {
                        throw new WebEasException("Ninject kernel in Ninject.NinjectServiceLocator is not initialized", ex);
                    }
                    throw new WebEasException("Maybe IWebEasSessionProvider is not registered in ninject", ex);
                }
            }
        }

        /// <summary>
        /// Gets the HTTP context.
        /// </summary>
        /// <value>The HTTP context.</value>
        public static System.Web.HttpContext HttpContext
        {
            get
            {
                return System.Web.HttpContext.Current;
            }
        }

        /// <summary>
        /// Gets the has HTTP context.
        /// </summary>
        /// <value>The has HTTP context.</value>
        public static bool HasHttpContext
        {
            get
            {
                return System.Web.HttpContext.Current != null;
            }
        }

        /// <summary>
        /// Gets the is service stack.
        /// </summary>
        /// <value>The is service stack.</value>
        public static bool IsServiceStack
        {
            get
            {
                return HostContext.AppHost != null;
            }
        }

        /// <summary>
        /// Gets or sets the last SOAP request message.
        /// </summary>
        /// <value>The last SOAP request message.</value>
        public static string LastSoapRequestMessage
        {
            get
            {
                return GetFromCurrentRequest<string>("LastSoapRequestMessage");
            }
            internal set
            {
                SetToCurrentRequest("LastSoapRequestMessage", value);
            }
        }

        /// <summary>
        /// Gets or sets the last SOAP response message.
        /// </summary>
        /// <value>The last SOAP response message.</value>
        public static string LastSoapResponseMessage
        {
            get
            {
                return GetFromCurrentRequest<string>("LastSoapResponseMessage");
            }
            internal set
            {
                SetToCurrentRequest("LastSoapResponseMessage", value);
            }
        }

        /// <summary>
        /// Gets the current dto.
        /// </summary>
        /// <value>The current dto.</value>
        public static object CurrentDto
        {
            get
            {
                if (WebEas.Context.Current.HttpContext != null)
                {
                    return WebEas.Context.Current.HttpContext.Items["currentRequest"];
                }
                return null;
            }
        }

        /// <summary>
        /// Gets or sets the current correlation ID.
        /// </summary>
        /// <value>The current correlation ID.</value>
        public static Guid CurrentCorrelationID
        {
            get
            {
                Guid? id = GetFromCurrentRequest<Guid?>("CorrId");

                if (!id.HasValue)
                {
                    id = Guid.NewGuid();
                    CurrentCorrelationID = id.Value;
                }
                return id.Value;
            }
            set
            {
                SetToCurrentRequest("CorrId", value);
            }
        }

        /// <summary>
        /// Sets the background process session unique key.
        /// </summary>
        /// <param name="uniqueKey">The unique key.</param>
        public static void SetBackgroundProcessSessionUniqueKey(string uniqueKey)
        {
            try
            {
                IWebEasSessionProvider sessionProvider = Ninject.NinjectServiceLocator.Kernel.Get<IWebEasSessionProvider>();
                sessionProvider.SetBackgroundSession(uniqueKey);
            }
            catch (Exception ex)
            {
                if (Ninject.NinjectServiceLocator.Kernel == null)
                {
                    throw new WebEasException("Ninject kernel in Ninject.NinjectServiceLocator is not initialized", ex);
                }
                throw new WebEasException("Maybe IWebEasSessionProvider is not registered in ninject", ex);
            }
        }

        /// <summary>
        /// Registers the service stack licence.
        /// </summary>
        public static void RegisterServiceStackLicence()
        {
            ServiceStack.Licensing.RegisterLicense(@"6061-e1JlZjo2MDYxLE5hbWU6IkRBVEFMQU4sIGEucy4iLFR5cGU6QnVzaW5lc3MsSGFzaDpISmRCVC9RK1RXTkVEbWQvVUVuNC9FYlNZekNnTkpBTU5DWnhlRzBpeXVMTi9DeHNXczZzMkxqMmpuQTlybmtobEMzSzF4NGpNTzRxbGFlS1VSQlZ5K1VaelhEYUlwL2pDSzA3bUlaQ2VaTHZHemJnUUZPVUMveFlURFlHNzRaVzdQZW5sZlJPU2o2NjZMZzAzL01tbEMxV0JSS3ZEd3ZDekIvWmxSWVBCS0E9LEV4cGlyeToyMDE5LTAzLTIyfQ==");
        }

        /// <summary>
        /// Gets from current request.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private static T GetFromCurrentRequest<T>(string key)
        {
            if (HasHttpContext)
            {
                return (T)System.Web.HttpContext.Current.Items[key];
            }
            else if (OperationContext.Current != null)
            {
                return (T)LocalMachineCache.Get(string.Format("{0}{1}", key, OperationContext.Current.SessionId));
            }
            return (T)LocalMachineCache.Get(string.Format("{0}{1}", key, System.Threading.Thread.CurrentThread.ManagedThreadId.ToString()));
        }

        /// <summary>
        /// Sets to current request.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        private static void SetToCurrentRequest<T>(string key, T data)
        {
            if (HasHttpContext)
            {
                System.Web.HttpContext.Current.Items[key] = data;
            }
            else if (OperationContext.Current != null)
            {
                LocalMachineCache.Set(string.Format("{0}{1}", key, OperationContext.Current.SessionId), data);
            }
            else
            {
                LocalMachineCache.Set(string.Format("{0}{1}", key, System.Threading.Thread.CurrentThread.ManagedThreadId.ToString()), data);
            }
        }
    }
}