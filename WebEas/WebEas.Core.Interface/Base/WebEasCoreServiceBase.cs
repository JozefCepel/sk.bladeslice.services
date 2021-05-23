using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using ServiceStack;
using ServiceStack.Logging;
using WebEas.AppStatus;
using System.Text;

namespace WebEas.Core.Base
{
    /// <summary>
    /// 
    /// </summary>
    public class WebEasCoreServiceBase : Service, IWebEasCoreServiceBase
    {
        protected static readonly ILog Log = LogManager.GetLogger(typeof(WebEasCoreServiceBase));
        protected IWebEasCoreRepositoryBase repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasCoreServiceBase" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public WebEasCoreServiceBase(IWebEasCoreRepositoryBase repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Gets or sets the repository.
        /// </summary>
        /// <value>The repository.</value>
        public IWebEasCoreRepositoryBase Repository
        {
            get
            {
                return repository;
            }
        }

        /// <summary>
        /// Toes the optimized result using cache.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="dtoObject">The dto object.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public object ToOptimizedResultUsingCache<T>(object dtoObject, Func<T> result)
        {
            return this.ToOptimizedResultUsingCache<T>(dtoObject, new TimeSpan(12, 0, 0), result);
        }

        /// <summary>
        /// Toes the optimized result using cache public.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="dtoObject">The dto object.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public object ToOptimizedResultUsingCachePublic<T>(object dtoObject, Func<object> result)
        {
            return this.ToOptimizedResultUsingCachePublic<T>(dtoObject, new TimeSpan(12, 0, 0), result);
        }

        /// <summary>
        /// Toes the optimized result using cache.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="dtoObject">The dto object.</param>
        /// <param name="expireInTimespan">The expire in timespan.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public object ToOptimizedResultUsingCache<T>(object dtoObject, TimeSpan? expireInTimespan, Func<T> result)
        {
            string cacheKey = this.GetCacheKey(dtoObject);
            return Request.ToOptimizedResultUsingCache(Repository.Cache, cacheKey, expireInTimespan, result);
        }

        /// <summary>
        /// Toes the optimized result using cache public.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="dtoObject">The dto object.</param>
        /// <param name="expireInTimespan">The expire in timespan.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public object ToOptimizedResultUsingCachePublic<T>(object dtoObject, TimeSpan? expireInTimespan, Func<object> result)
        {
            string cacheKey = this.GetCacheKey(dtoObject, typeof(T));
            return Request.ToOptimizedResultUsingCache(Repository.Cache, cacheKey, expireInTimespan, result);
        }

        /// <summary>
        /// Gets the download response.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="mimeType">Type of the MIME.</param>
        /// <returns></returns>
        public object GetDownloadResponse(byte[] data, string fileName, string mimeType = null)
        {
            if (string.IsNullOrEmpty(mimeType))
            {
                mimeType = WebEas.Web.Mime.GetMimeType(fileName);
            }

            base.Response.ContentType = mimeType;
            base.Response.AddHeader("Content-Disposition", string.Concat("attachment; filename=\"", fileName, "\"; filename*=UTF-8''", Uri.EscapeDataString(fileName)));
            return new HttpResult(data, mimeType);
        }

        /// <summary>
        /// Gets response that browser try to open automatically.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="mimeType">Type of the MIME.</param>
        /// <returns></returns>
        public object GetInlineResponse(byte[] data, string fileName, string mimeType = null)
        {
            if (string.IsNullOrEmpty(mimeType))
            {
                mimeType = WebEas.Web.Mime.GetMimeType(fileName);
            }

            base.Response.ContentType = mimeType;
            base.Response.AddHeader("Content-Disposition", string.Format("inline; filename*=UTF-8''{0}", HttpUtility.UrlPathEncode(fileName)));
            return new HttpResult(data, mimeType);
        }

        /// <summary>
        /// Gets the response.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns></returns>
        public object GetResponse(byte[] data, string contentType)
        {
            base.Response.ContentType = contentType;
            return new HttpResult(data, contentType);
        }

        /// <summary>
        /// Gets the HTML response.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public object GetHtmlResponse(byte[] data)
        {
            return this.GetResponse(data, "text/html");
        }

        /// <summary>
        /// Gets the PDF response.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public object GetPdfResponse(byte[] data)
        {
            return this.GetResponse(data, "application/pdf");
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            if (this.repository != null)
            {
                this.repository.Dispose();
            }
        }
                
        private const char FieldPartsSeperator = '/';

        /// <summary>
        /// Gets the cache key.
        /// </summary>
        /// <param name="dtoObject">The dto object.</param>
        /// <returns></returns>
        public string GetCacheKey(object dtoObject, Type cacheType = null)
        {
            if (dtoObject == null)
            {
                throw new ArgumentNullException("Dto object cannot be null");
            }            

            string objectTypeName = cacheType == null || cacheType == typeof(object) ? dtoObject.GetType().FullName : string.Format("{0}:{1}", cacheType.FullName, dtoObject.GetType().FullName);

            var sb = new StringBuilder();            

            foreach (PropertyInfo t in dtoObject.GetType().GetProperties())
            {
                object val = t.GetValue(dtoObject);
                if (val == null)
                {
                    if (sb.Length > 0)
                        sb.Append(FieldPartsSeperator);
                    sb.Append("_");                    
                }
                else
                {
                    if (sb.Length > 0)
                        sb.Append(FieldPartsSeperator);
                    sb.Append(val.ToString());                    
                }
            }            

            return string.Format("urn:{0}:{1}", objectTypeName, sb);            
        }
    }
}