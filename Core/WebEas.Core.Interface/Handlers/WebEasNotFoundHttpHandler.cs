using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ServiceStack;
using ServiceStack.Host.AspNet;
using ServiceStack.Host.Handlers;
using ServiceStack.Logging;
using ServiceStack.Text;
using ServiceStack.Web;

namespace WebEas.Core.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    public class WebEasNotFoundHttpHandler : HttpAsyncTaskHandler
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(WebEasNotFoundHttpHandler));

        /// <summary>
        /// Gets or sets the is integrated pipeline.
        /// </summary>
        /// <value>The is integrated pipeline.</value>
        public bool? IsIntegratedPipeline { get; set; }

        /// <summary>
        /// Gets or sets the web host physical path.
        /// </summary>
        /// <value>The web host physical path.</value>
        public string WebHostPhysicalPath { get; set; }

        /// <summary>
        /// Gets or sets the web host root file names.
        /// </summary>
        /// <value>The web host root file names.</value>
        public List<string> WebHostRootFileNames { get; set; }

        /// <summary>
        /// Gets or sets the web host URL.
        /// </summary>
        /// <value>The web host URL.</value>
        public string WebHostUrl { get; set; }

        /// <summary>
        /// Gets or sets the default name of the root file.
        /// </summary>
        /// <value>The default name of the root file.</value>
        public string DefaultRootFileName { get; set; }

        /// <summary>
        /// Gets or sets the default handler.
        /// </summary>
        /// <value>The default handler.</value>
        public string DefaultHandler { get; set; }

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler" />
        /// instance.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler" /> instance is reusable;
        /// otherwise, false.</returns>
        /// <value></value>
        public override bool IsReusable
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        /// <param name="operationName">Name of the operation.</param>
        public override void ProcessRequest(IRequest request, IResponse response, string operationName)
        {
            log.DebugFormat("{0} Request not found: {1}", request.UserHostAddress, request.RawUrl);

            var text = new StringBuilder();

            string contentType = request.ContentType;

            var req = request as AspNetRequest;
            if (req != null)
            {
                if (req.AcceptTypes != null && req.AcceptTypes.Length > 0)
                {
                    log.Info(string.Format("Accept {0}", req.AcceptTypes.Join(",")));
                    contentType = req.AcceptTypes[0];
                }
            }
            log.Debug(string.Format("Process request {0}", contentType));

            if (String.IsNullOrEmpty(contentType) || contentType == "text/plain" || contentType == "text/html")
            {
                contentType = "text/plain";
                if (HostContext.DebugMode)
                {
                    text.AppendLine("Handler for Request not found: \n\n")
                        .AppendLine(string.Format("Request.HttpMethod: {0}", request.Verb))
                        .AppendLine(string.Format("Request.PathInfo: {0}", request.PathInfo))
                        .AppendLine(string.Format("Request.QueryString: {0}", request.QueryString))
                        .AppendLine(string.Format("Request.RawUrl: {0}", request.RawUrl));
                }
                else
                {
                    text.Append("404");
                }
            }

            response.ContentType = contentType;
            response.StatusCode = 404;
            response.EndHttpHandlerRequest(skipClose: true, afterHeaders: r => r.Write(text.ToString()));
        }

        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="context">The context.</param>
        public override void ProcessRequest(HttpContextBase context)
        {
            HttpRequestBase request = context.Request;
            HttpResponseBase response = context.Response;

            IHttpRequest httpReq = context.ToRequest(this.GetType().GetOperationName());
            if (!request.IsLocal)
            {
                this.ProcessRequestAsync(httpReq, httpReq.Response, null);
                return;
            }

            log.DebugFormat("{0} Request not found: {1}", request.UserHostAddress, request.RawUrl);

            var sb = new StringBuilder();
            string contentType = request.ContentType;

            if (request.AcceptTypes != null && request.AcceptTypes.Length > 0)
            {
                log.Debug(string.Format("Accept {0}", request.AcceptTypes.Join(",")));
                contentType = request.AcceptTypes[0];
            }

            log.Debug(string.Format("Process request context{0}", contentType));

            if (String.IsNullOrEmpty(contentType) || contentType == "text/plain" || contentType == "text/html")
            {
                contentType = "text/plain";

                sb.AppendLine("Handler for Request not found: \n\n");

                sb.AppendLine(string.Format("Request.ApplicationPath: {0}", request.ApplicationPath));
                sb.AppendLine(string.Format("Request.CurrentExecutionFilePath: {0}", request.CurrentExecutionFilePath));
                sb.AppendLine(string.Format("Request.FilePath: {0}", request.FilePath));
                sb.AppendLine(string.Format("Request.HttpMethod: {0}", request.HttpMethod));
                sb.AppendLine(string.Format("Request.MapPath('~'): {0}", request.MapPath("~")));
                sb.AppendLine(string.Format("Request.Path: {0}", request.Path));
                sb.AppendLine(string.Format("Request.PathInfo: {0}", request.PathInfo));
                sb.AppendLine(string.Format("Request.ResolvedPathInfo: {0}", httpReq.PathInfo));
                sb.AppendLine(string.Format("Request.PhysicalPath: {0}", request.PhysicalPath));
                sb.AppendLine(string.Format("Request.PhysicalApplicationPath: {0}", request.PhysicalApplicationPath));
                sb.AppendLine(string.Format("Request.QueryString: {0}", request.QueryString));
                sb.AppendLine(string.Format("Request.RawUrl: {0}", request.RawUrl));
                try
                {
                    sb.AppendLine(string.Format("Request.Url.AbsoluteUri: {0}", request.Url.AbsoluteUri));
                    sb.AppendLine(string.Format("Request.Url.AbsolutePath: {0}", request.Url.AbsolutePath));
                    sb.AppendLine(string.Format("Request.Url.Fragment: {0}", request.Url.Fragment));
                    sb.AppendLine(string.Format("Request.Url.Host: {0}", request.Url.Host));
                    sb.AppendLine(string.Format("Request.Url.LocalPath: {0}", request.Url.LocalPath));
                    sb.AppendLine(string.Format("Request.Url.Port: {0}", request.Url.Port));
                    sb.AppendLine(string.Format("Request.Url.Query: {0}", request.Url.Query));
                    sb.AppendLine(string.Format("Request.Url.Scheme: {0}", request.Url.Scheme));
                    sb.AppendLine(string.Format("Request.Url.Segments: {0}", request.Url.Segments));
                }
                catch (Exception ex)
                {
                    sb.AppendLine(string.Format("Request.Url ERROR: {0}", ex.Message));
                }
                if (this.IsIntegratedPipeline.HasValue)
                {
                    sb.AppendLine(string.Format("App.IsIntegratedPipeline: {0}", this.IsIntegratedPipeline));
                }
                if (!this.WebHostPhysicalPath.IsNullOrEmpty())
                {
                    sb.AppendLine(string.Format("App.WebHostPhysicalPath: {0}", this.WebHostPhysicalPath));
                }
                if (!this.WebHostRootFileNames.IsEmpty())
                {
                    sb.AppendLine(string.Format("App.WebHostRootFileNames: {0}", TypeSerializer.SerializeToString(this.WebHostRootFileNames)));
                }
                if (!this.WebHostUrl.IsNullOrEmpty())
                {
                    sb.AppendLine(string.Format("App.ApplicationBaseUrl: {0}", this.WebHostUrl));
                }
                if (!this.DefaultRootFileName.IsNullOrEmpty())
                {
                    sb.AppendLine(string.Format("App.DefaultRootFileName: {0}", this.DefaultRootFileName));
                }
                if (!this.DefaultHandler.IsNullOrEmpty())
                {
                    sb.AppendLine(string.Format("App.DefaultHandler: {0}", this.DefaultHandler));
                }
                if (!HttpHandlerFactory.DebugLastHandlerArgs.IsNullOrEmpty())
                {
                    sb.AppendLine(string.Format("App.DebugLastHandlerArgs: {0}", HttpHandlerFactory.DebugLastHandlerArgs));
                }
            }
            response.ContentType = contentType;
            response.StatusCode = 404;
            response.Write(sb.ToString());
            response.EndRequest();
        }
    }
}