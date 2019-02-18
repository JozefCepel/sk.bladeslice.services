using System;
using System.Linq;
using ServiceStack;
using ServiceStack.Host;

namespace WebEas.Core.Base
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">The type of the T.</typeparam>
    public class WebEasServiceRunner<T> : ServiceRunner<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasServiceRunner" /> class.
        /// </summary>
        /// <param name="appHost">The app host.</param>
        /// <param name="actionContext">The action context.</param>
        public WebEasServiceRunner(IAppHost appHost, ActionContext actionContext)
            : base(appHost, actionContext)
        {
        }

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="requestDto">The request dto.</param>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public override object HandleException(global::ServiceStack.Web.IRequest request, T requestDto, Exception ex)
        {
            var response = WebEasErrorHandling.CreateErrorResponse(request, requestDto, ex);
            this.AfterEachRequest(request, requestDto, response == null ? ex : response.Response ?? ex);
            return response;
        }

        /// <summary>
        /// Befores the each request.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        /// <param name="request">The request.</param>
        public override void BeforeEachRequest(ServiceStack.Web.IRequest requestContext, T request)
        {
            if (WebEas.Context.Current.HasHttpContext)
            {
                WebEas.Context.Current.HttpContext.Items["currentRequest"] = request;
                string corrId = requestContext.GetHeader("x-corr-id");
                if (!string.IsNullOrEmpty(corrId))
                {
                    Guid val;
                    if (Guid.TryParse(corrId, out val))
                    {
                        WebEas.Context.Current.CurrentCorrelationID = val;
                    }
                }
            }
            base.BeforeEachRequest(requestContext, request);
        }

        /// <summary>
        /// Afters the each request.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        /// <returns></returns>
        public override object AfterEachRequest(ServiceStack.Web.IRequest requestContext, T request, object response)
        {
            if (WebEas.Context.Current.HasHttpContext)
            {
                requestContext.Response.AddHeader("x-corr-id", WebEas.Context.Current.CurrentCorrelationID.ToString().ToLower());
            }
            return base.AfterEachRequest(requestContext, request, response);
        }
    }
}